# Usage

This guide walks through the four packages of the repository and shows how to combine them. The order is intentional: start with the data models, then the EF Core extensions, then the ASP.NET Core integration. Pick whatever subset matches your project.

- [1. Packages and namespaces](#1-packages-and-namespaces)
- [2. The core: `IQueryable<T>` extensions](#2-the-core-iqueryablet-extensions)
  - [2.1 `GetPaged` / `GetPagedAsync`](#21-getpaged--getpagedasync)
  - [2.2 `GetPagedWithMainFiltersAsync`](#22-getpagedwithmainfiltersasync)
  - [2.3 `GetPagedWithFiltersAsync`](#23-getpagedwithfiltersasync)
  - [2.4 The `PredefinedRecord` definition](#24-the-predefinedrecord-definition)
- [3. ASP.NET Core MVC integration](#3-aspnet-core-mvc-integration)
  - [3.1 `BaseApiPagedResultController`](#31-baseapipagedresultcontroller)
  - [3.2 Body-bound usage with MediatR](#32-body-bound-usage-with-mediatr)
  - [3.3 Query-string binding with `[FromPagedQuery]`](#33-query-string-binding-with-frompagedquery)
- [4. Minimal-API integration (.NET 7+)](#4-minimal-api-integration-net-7)
  - [4.1 `PagedQuery<TEntity>` and `PagedQueryWithFilters<TEntity>`](#41-pagedquerytentity-and-pagedquerywithfilterstentity)
  - [4.2 Returning a paged result with `ToPagedHttpResult`](#42-returning-a-paged-result-with-topagedhttpresult)
- [5. Configuration: `AddPagedListResultWeb`](#5-configuration-addpagedlistresultweb)
  - [5.1 Allow-list registration with `ConfigurePageable<T>`](#51-allow-list-registration-with-configurepageablet)
  - [5.2 Options reference](#52-options-reference)
- [6. The query-string syntax](#6-the-query-string-syntax)
- [7. Response headers and ProblemDetails](#7-response-headers-and-problemdetails)
- [8. OpenAPI: `AddPagedListResultApiExplorer`](#8-openapi-addpagedlistresultapiexplorer)

---

## 1. Packages and namespaces

| Package | Assembly | Root namespace |
|---------|----------|----------------|
| `PagedListResult.DataModels` | `RzR.ResultMessage.Pagination.Abstractions` | `RzR.ResultMessage.Pagination.Abstractions.*` |
| `PagedListResult.Common` | `RzR.ResultMessage.Pagination.Core` | `RzR.ResultMessage.Pagination.Core.*` |
| `PagedListResult` | `RzR.ResultMessage.Pagination.EntityFrameworkCore` | `RzR.ResultMessage.Pagination.EntityFrameworkCore.*` |
| `PagedListResult.Web` | `RzR.ResultMessage.Pagination.AspNetCore` | `RzR.ResultMessage.Pagination.AspNetCore.*` |

---

## 2. The core: `IQueryable<T>` extensions

All paginated queries are built on top of four extension methods exposed from `RzR.ResultMessage.Pagination.EntityFrameworkCore`. Both the synchronous and asynchronous flavors share the same shape.

### 2.1 `GetPaged` / `GetPagedAsync`

Two overloads:

| Signature | When to use |
|-----------|-------------|
| `PagedResult<TSource> GetPaged<TSource>(this IQueryable<TSource> query, int page, int pageSize)` | Plain pagination, no search, no order, no filters. You only need to slice an `IQueryable<T>`. |
| `PagedResult<TSource> GetPaged<TSource, TPageRequest>(this IQueryable<TSource> query, TPageRequest request, DefaultPrimaryKeyDefinition defaultPrimaryKey = null) where TPageRequest : PagedRequest` | Pagination + ordering + free-text search + projection (`fields`) + predefined records. The `defaultPrimaryKey` is needed only when `PredefinedRecord` is used (it tells the builder which property holds the entity key). |

`GetPagedAsync` mirrors both overloads and accepts a `CancellationToken`.

> Both `GetPaged` and `GetPagedAsync` use `TPageRequest` only for pagination, ordering, search and predefined records. Filters from `PageRequestWithFilters.Filters` are ignored here; that is what the next two methods are for.

### 2.2 `GetPagedWithMainFiltersAsync`

Adds the simplest possible filter handling on top of the previous overloads. All entries from `PageRequestWithFilters.Filters` are AND-combined at the top level. Their `Dependencies` collection is ignored.

```text
Code = 'Test001' AND IsActive = true AND CreatedOn >= @from
```

### 2.3 `GetPagedWithFiltersAsync`

The full filter pipeline. On top of everything `GetPagedWithMainFiltersAsync` does, it honors:

- `filterLink` (`FilterConditionType.And` or `FilterConditionType.Or`) between top-level filters.
- The `DataFilterDependence.ParentFilterLinkType` between a top-level filter and each of its dependencies, so you can express compound expressions like:

  ```text
  (authorId IN (2) OR authorId = 3)
    AND createdOn BETWEEN @start AND @end
  ```

A complete sample (controller + handler + request) wired through MediatR:

```csharp
//------------------------------------------------
// Controller
//------------------------------------------------
[Produces("application/json")]
[Route("api/[controller]/[action]")]
public class GetDataController : BaseApiPagedResultController
{
    private readonly IMediator _mediator;

    public GetDataController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(PagedResult<PostDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRecords(
        [FromBody] GetRecordsQuery query,
        CancellationToken cancellationToken)
    {
        var queryResponse = await _mediator.Send(query, cancellationToken);

        return PagedOkResult(queryResponse);
    }
}

//------------------------------------------------
// Request and handler
//------------------------------------------------
public class GetRecordsQuery : PageRequestWithFilters,
    IRequest<IPagedResult<PostDetail>> { }

public class GetRecordsHandler
    : IRequestHandler<GetRecordsQuery, IPagedResult<PostDetail>>
{
    private readonly AppDbContext _db;

    public GetRecordsHandler(AppDbContext db) => _db = db;

    public async Task<IPagedResult<PostDetail>> Handle(
        GetRecordsQuery request, CancellationToken cancellationToken)
    {
        var data = _db.Posts
            .Include(x => x.Author)
            .Select(x => new PostDetail
            {
                AuthorId = x.AuthorId,
                AuthorName = x.Author.Name,
                Contents = x.Contents,
                CreatedOn = x.CreatedOn,
                Id = x.Id,
                Title = x.Title,
                ModifiedOn = x.ModifiedOn
            });

        return await data.GetPagedWithFiltersAsync(
            request,
            new DefaultPrimaryKeyDefinition("Id"),
            cancellationToken: cancellationToken);
    }
}
```

A request body that exercises both filter dependencies and the OR top-level link:

```csharp
var pageRequest = new PageRequestWithFilters
{
    Page = 1,
    PageSize = 5,
    Filters = new List<DataFilter>
    {
        new DataFilter
        {
            FilterValue = new DataFilterValue
            {
                Values = new List<string> { "2" },
                PropertyName = "authorId",
                Condition = FilterType.IsIn
            },
            FilterApplyOrder = 0,
            Dependencies = new List<DataFilterDependence>
            {
                new DataFilterDependence
                {
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "authorId",
                        Condition = FilterType.Equals,
                        Values = new List<string> { "3" }
                    },
                    ParentFilterLinkType = FilterConditionType.Or
                }
            }
        },
        new DataFilter
        {
            FilterValue = new DataFilterValue
            {
                Values = new List<string> { DateTime.Now.StartOfDay().ToString() },
                CompareValue = DateTime.Now.EndOfDay().ToString(),
                PropertyName = "createdOn",
                Condition = FilterType.Between
            },
            FilterApplyOrder = 1
        }
    }
};

var query = _dbContext.Posts
    .Include(x => x.Author)
    .Select(x => new PostDetail
    {
        AuthorId = x.AuthorId,
        AuthorName = x.Author.Name,
        Contents = x.Contents,
        CreatedOn = x.CreatedOn,
        Id = x.Id,
        Title = x.Title,
        ModifiedOn = x.ModifiedOn
    });

var records = await query.GetPagedWithFiltersAsync(
    pageRequest,
    filterLink: FilterConditionType.Or);
```

### 2.4 The `PredefinedRecord` definition

The `PredefinedRecord` block lets you pin specific rows on top of the result regardless of paging or filters.

| Property | Description |
|----------|-------------|
| `PredefinedFieldName` | Name of the property to match against. |
| `PredefinedRecords` | Identifiers (or any string-encoded values) of the rows to pin. |

When you use `PredefinedRecord` in `GetPaged*` methods, also pass a `DefaultPrimaryKeyDefinition` so the builder knows which property to project on if `PredefinedFieldName` is empty.

---

## 3. ASP.NET Core MVC integration

### 3.1 `BaseApiPagedResultController`

`BaseApiPagedResultController` derives from `ResultBaseApiController` (from `AggregatedGenericResultMessage.Web`) and adds two protected helpers tailored to paged responses:

| Helper | Behavior |
|--------|----------|
| `PagedOkResult<T>(IPagedResult<T> response)` | On success: `200 OK` with the full paged envelope (paging metadata + `Response`). On failure: an RFC 9457 `application/problem+json` payload built by the ambient `IProblemDetailsResultFactory` (defaults to `400`; override with `services.AddProblemDetailsResultFactory<TFactory>()`). |
| `PagedXmlResult<T>(IPagedResult<T> response)` | Same flow, but successful responses are serialized as SOAP-friendly XML (`text/xml`). Failure uses ProblemDetails as well. |

The previous `JsonResult<T>` and `XmlResult<T>` helpers are kept as `[Obsolete]` for one more release. They will be removed in the next major version because `JsonResult` collides with `Microsoft.AspNetCore.Mvc.JsonResult`.

### 3.2 Body-bound usage with MediatR

This is the most common pattern for a back-office API. The body holds the full `PageRequestWithFilters` and the controller delegates to a handler:

```csharp
[HttpPost]
public async Task<IActionResult> GetRecords(
    [FromBody] GetRecordsQuery query, CancellationToken ct)
    => PagedOkResult(await _mediator.Send(query, ct));
```

See the full sample under section 2.3.

### 3.3 Query-string binding with `[FromPagedQuery]`

When you would rather expose a `GET` endpoint that accepts `?page=...&pageSize=...&order=...`, decorate the parameter with `[FromPagedQuery]`. Without the attribute, `PagedRequest` falls through to the default complex-object binder, so existing body-bound endpoints stay backwards compatible.

```csharp
// Plain query binding (no allow-list):
[HttpGet]
public IActionResult List([FromPagedQuery] PagedRequest request)
    => PagedOkResult(_service.GetPaged(request));
// GET /products?page=2&pageSize=20&order=name:asc&search=usb
```

Add an entity type to opt into per-entity allow-list validation (see section 5.1):

```csharp
// Allow-listed binding with filters:
[HttpGet]
public IActionResult List(
    [FromPagedQuery(typeof(Product))] PageRequestWithFilters request)
    => PagedOkResult(_service.GetPaged(request));

// GET /products?filter=status:Equals:active&filter=price:GreaterThan:100&order=price:desc
// A property that is not in the allow-list returns 400 ValidationProblem.
```

The binder is registered automatically by `AddPagedListResultWeb()` (see section 5).

---

## 4. Minimal-API integration (.NET 7+)

### 4.1 `PagedQuery<TEntity>` and `PagedQueryWithFilters<TEntity>`

Both wrappers implement the `BindAsync(HttpContext, ParameterInfo)` pattern recognized by ASP.NET Core, so endpoint handlers can accept them directly as parameters. They use the very same parser that the MVC binder uses, including allow-list validation.

```csharp
app.MapGet("/products",
    (PagedQuery<Product> q, IProductService svc, HttpContext ctx) =>
    {
        if (!q.IsValid)
            return Results.ValidationProblem(
                q.Errors.ToDictionary(e => e.Key, e => new[] { e.Value }));

        return svc.GetPaged(q.Page, q.PageSize).ToPagedHttpResult(ctx);
    })
    .WithPagedResult(); // emits X-Total-Count, Link, etc.

// GET /products?page=2&pageSize=20&order=price:desc&search=usb
```

`PagedQueryWithFilters<TEntity>` extends `PageRequestWithFilters` and additionally supports the repeatable `?filter=` parameter:

```csharp
app.MapGet("/products",
    (PagedQueryWithFilters<Product> q, IProductService svc) =>
        q.IsValid
            ? Results.Ok(svc.GetPaged(q))
            : Results.ValidationProblem(
                q.Errors.ToDictionary(e => e.Key, e => new[] { e.Value })))
    .WithPagedResult();

// GET /products?filter=status:Equals:active&filter=price:GreaterThan:50&order=price:desc
```

### 4.2 Returning a paged result with `ToPagedHttpResult`

The upstream `ResultToHttpResult.ToHttpResult<T>(IResult<T>)` only serializes `result.Response` on success, which would silently drop the paging metadata of `IPagedResult<T>` (it is `IResult<IList<T>>`). `ToPagedHttpResult` solves this:

- On a `null` paged result it returns `204 No Content`.
- On success it returns `Results.Ok(paged)` with the full envelope (so `WithPagedResult()` can detect it and emit pagination headers).
- On failure it delegates to the upstream ProblemDetails machinery, producing an `application/problem+json` payload that mirrors the MVC pipeline.

```csharp
return paged.ToPagedHttpResult(ctx);
```

`WithPagedResult()` is just a friendlier alias for `AddPagedResultEndpointFilter()`. Both attach `PagedResultEndpointFilter` to the route or route group.

---

## 5. Configuration: `AddPagedListResultWeb`

A single registration call wires up everything: options, the singleton `IPageableMetadataRegistry`, the MVC convention, the action filter, the model binder provider, and (on .NET 7+) the endpoint filter.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddPagedListResultApiExplorer(); // optional, see section 8

builder.Services
    .AddPagedListResultWeb(o =>
    {
        o.MaxPageSize = 100; // default 200
        o.DefaultPageSize = 25; // default 10
        o.EmitLinkHeader = true; // RFC 5988 Link
        o.EmitTotalCountHeader = true; // X-Total-Count, X-Page-Count, X-Page-Size, X-Current-Page
        o.EmitServerTimingHeader = false; // Server-Timing: paged;dur=<ms>
    })
    .ConfigurePageable<Product>(b => b
        .AllowSort("name", "price")
        .AllowFilter("status", "price")
        .AllowSearch("name", "description"));

var app = builder.Build();
app.MapControllers();
app.Run();
```

### 5.1 Allow-list registration with `ConfigurePageable<T>`

`ConfigurePageable<T>` registers a per-type allow-list. Property names submitted via `order=`, `filter=`, `search=` or `searchFields=` are validated against this list before any reflection or expression-builder work occurs. A disallowed property results in a `400 ValidationProblem` with a clear error key (`filter`, `order`, ...).

Method | What it allows
-------|---------------
`AllowSort(params string[])` | Property names allowed in `?order=`.
`AllowFilter(params string[])` | Property names allowed in `?filter=`.
`AllowSearch(params string[])` | Property names allowed in `?searchFields=`.

If you do not register an allow-list for a type, no allow-list validation is applied (existing payloads remain backwards compatible).

### 5.2 Options reference

| Option | Default | Description |
|--------|---------|-------------|
| `MaxPageSize` | `200` | Hard upper bound enforced by the binder and the action filter. Values above this cap produce a `400` error. |
| `DefaultPageSize` | `10` | Page size used when the request does not provide one. |
| `EmitLinkHeader` | `true` | Adds the RFC 5988 `Link` header (`first`, `prev`, `next`, `last`) on `GET` responses. |
| `EmitTotalCountHeader` | `true` | Adds `X-Total-Count`, `X-Page-Count`, `X-Page-Size`, `X-Current-Page`. |
| `EmitServerTimingHeader` | `false` | Adds `Server-Timing: paged;dur={ExecutionTimeMs}` from `PagedResult<T>.ExecutionDetails`. |
| `StatusCodeMapper` | `null` | Optional `Func<IResult, int, int>` that maps a failed result to a status code. The fallback (typically `400`) is always provided as the second argument. |
| `ProblemTypeBaseUri` | `null` | Base URI used to build RFC 9457 `type` values. Falls back to the factory default. |
| `JsonSerializerOptions` | `null` | Optional `JsonSerializerOptions` override for paged payloads. |

---

## 6. The query-string syntax

Both the MVC binder (`[FromPagedQuery]`) and the minimal-API wrappers share a single parser. The supported keys are exposed as constants on `PagedQueryKeys`:

| Key | Example | Notes |
|-----|---------|-------|
| `page` | `?page=2` | 1-based page index. Must be `>= 1`. |
| `pageSize` | `?pageSize=20` | Capped by `MaxPageSize`. Falls back to `DefaultPageSize`. |
| `search` | `?search=usb` | Free-text search term written into `DataSearchDefinition.Search`. |
| `searchAll` | `?searchAll=true` | Search-mode flag. Accepted values: `true`/`text`, `fields`/`all`, `false`/`none`. |
| `searchFields` | `?searchFields=name,description` | Restricts free-text search to listed properties. Validated against `AllowSearch`. |
| `order` | `?order=name:desc` | Single-key sort. Direction is case-insensitive (`asc`/`desc`). Validated against `AllowSort`. |
| `filter` | `?filter=status:Equals:active` | Repeatable. Format: `propertyName:condition[:value[,value2]]`. See below. |
| `fields` | `?fields=id,name,price` | Projection / partial response. |
| `predefinedField` | `?predefinedField=Id` | Predefined record key (maps to `PredefinedRecord.PredefinedFieldName`). |
| `predefinedRecords` | `?predefinedRecords=1,2,3` | Identifiers for the predefined records. |

The `filter` key is the most expressive one:

| Form | Maps to |
|------|---------|
| `filter=status:Equals:active` | `PropertyName=status`, `Condition=Equals`, `Values=["active"]` |
| `filter=price:Between:10,20` | Left/right pair: `Values[0]="10"`, `CompareValue="20"` |
| `filter=status:IsIn:active,draft` | Multi-value: every token lands in `Values`. |
| `filter=name:IsNull` and `filter=name:IsNotNull` | Value-less. |

Multiple `filter=` occurrences are kept in submission order and AND-combined by default (use the body shape if you need OR / dependencies).

Allow-list violations (sort, filter, search) are reported with the matching key in the `Errors` dictionary, then surfaced as `400 ValidationProblem` (MVC) or as `q.Errors` / `Results.ValidationProblem(...)` (minimal API).

You can also drive the parser yourself if you need to integrate with a custom binding pipeline:

```csharp
var req = new PagedRequest();
var parse = PagedRequestQueryParser.Populate(req, httpContext.Request.Query, options);
if (!parse.IsValid)
    return Results.ValidationProblem(
        parse.Errors.ToDictionary(e => e.Key, e => new[] { e.Value }));
```

---

## 7. Response headers and ProblemDetails

When the action result body (or the awaited minimal-API result) implements `IPagedResult<T>`, the corresponding filter writes the headers configured on `PagedListResultWebOptions`:

| Header | When | Source |
|--------|------|--------|
| `X-Total-Count` | `EmitTotalCountHeader=true` | `PagedResult<T>.RowCount` |
| `X-Page-Count` | `EmitTotalCountHeader=true` | `PagedResult<T>.PageCount` |
| `X-Page-Size` | `EmitTotalCountHeader=true` | `PagedResult<T>.PageSize` |
| `X-Current-Page` | `EmitTotalCountHeader=true` | `PagedResult<T>.CurrentPage` |
| `Link` | `EmitLinkHeader=true` and `pageCount > 0` and HTTP `GET` | `first`, `prev`, `next`, `last` URIs preserving every other query-string entry. |
| `Server-Timing: paged;dur=<ms>` | `EmitServerTimingHeader=true` | `PagedResult<T>.ExecutionDetails.ExecutionTimeMs` |

The minimal-API endpoint filter unwraps common typed results (`Ok<T>`, `JsonHttpResult<T>`, ...) by reading their `Value` property, so you can return whichever shape feels natural at the call site.

On the failure side, both `BaseApiPagedResultController.PagedOkResult` and the minimal-API `ToPagedHttpResult` produce an RFC 9457 `application/problem+json` body via the configured `IProblemDetailsResultFactory` from `AggregatedGenericResultMessage.Web`. The default status code is `400`, customizable through `PagedListResultWebOptions.StatusCodeMapper`.

---

## 8. OpenAPI: `AddPagedListResultApiExplorer`

`AddPagedListResultApiExplorer` is an `IMvcBuilder` extension that registers the `PagedResultApplicationModelConvention`. The convention scans every controller action and, for actions whose return type resolves to `IPagedResult<T>` or `PagedResult<T>` (recursively unwrapping `Task<>`, `ValueTask<>` and `ActionResult<>`), it auto-attaches:

- `[Produces("application/json")]`
- `[ProducesResponseType(typeof(PagedResult<T>), 200)]`
- `[ProducesResponseType(typeof(ProblemDetails), 400)]`

User-declared `[ProducesResponseType]` with the same status code are preserved, so you can still override the success / failure shape per action.

```csharp
builder.Services
    .AddControllers()
    .AddPagedListResultApiExplorer();
```

Combined with `AddPagedListResultWeb()`, your Swagger / OpenAPI document picks up the correct paged envelope without any manual annotation work.
