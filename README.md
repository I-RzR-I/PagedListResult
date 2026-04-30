> **Note:** This repository targets `netstandard2.0` (and `netstandard2.1` for the ASP.NET Core integration) and runs on .NET 5, 6, 7, 8 and 9.

| Name | NuGet |
|------|-------|
| RzR.ResultMessage.Pagination.Abstractions | [![NuGet Version](https://img.shields.io/nuget/v/RzR.ResultMessage.Pagination.Abstractions?style=flat&logo=nuget)](https://www.nuget.org/packages/PRzR.ResultMessage.Pagination.Abstractions/) [![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.ResultMessage.Pagination.Abstractions.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.Abstractions) |
| RzR.ResultMessage.Pagination.Core | [![NuGet Version](https://img.shields.io/nuget/v/RzR.ResultMessage.Pagination.Core.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.Core/) [![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.ResultMessage.Pagination.Core.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.Core) |
| RzR.ResultMessage.Pagination.EntityFrameworkCore | [![NuGet Version](https://img.shields.io/nuget/v/RzR.ResultMessage.Pagination.EntityFrameworkCore.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.EntityFrameworkCore/) [![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.ResultMessage.Pagination.EntityFrameworkCore.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.EntityFrameworkCore) |
| RzR.ResultMessage.Pagination.AspNetCore | [![NuGet Version](https://img.shields.io/nuget/v/RzR.ResultMessage.Pagination.AspNetCore.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.AspNetCore/) [![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.ResultMessage.Pagination.AspNetCore.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.ResultMessage.Pagination.AspNetCore) |


<details>

  <summary>Old version</summary>
  
  
| Name | NuGet |
|------|-------|
| PagedListResult.DataModels | [![NuGet Version](https://img.shields.io/nuget/v/PagedListResult.DataModels.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.DataModels/) [![Nuget Downloads](https://img.shields.io/nuget/dt/PagedListResult.DataModels.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.DataModels) |
| PagedListResult.Common | [![NuGet Version](https://img.shields.io/nuget/v/PagedListResult.Common.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.Common/) [![Nuget Downloads](https://img.shields.io/nuget/dt/PagedListResult.Common.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.Common) |
| PagedListResult | [![NuGet Version](https://img.shields.io/nuget/v/PagedListResult.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult/) [![Nuget Downloads](https://img.shields.io/nuget/dt/PagedListResult.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult) |

</details>

## What this is

The repository started from a very common need: building server-side pagination for grids and tables (page, page size, search, ordering, filters, predefined "pinned" rows) without re-inventing the request and response shapes in every project. It has grown into a small set of layered libraries you can pick and choose from.

| Project | What it gives you |
|---------|-------------------|
| `PagedListResult.DataModels` -> (`RzR.ResultMessage.Pagination.Abstractions`) | Plain `netstandard2.0` request/response models (`PagedRequest`, `PageRequestWithFilters`, `PagedResult<T>`, `DataFilter`, `DataSearchDefinition`, `DataOrderDefinition`, `DataPredefinedFilterDefinition`, ...). Use this when you only need to share the contract between client and server. |
| `PagedListResult.Common` -> (`RzR.ResultMessage.Pagination.Core`) | The expression-builder layer on top of `System.Linq.Expressions`: dynamic search across text fields, ordering by property name, conditional filters, predefined records, validation. No EF Core dependency. |
| `PagedListResult` -> (`RzR.ResultMessage.Pagination.EntityFrameworkCore`) | The `IQueryable<T>` extensions that wire everything together for `Microsoft.EntityFrameworkCore` consumers: `GetPaged`, `GetPagedAsync`, `GetPagedWithMainFiltersAsync`, `GetPagedWithFiltersAsync`. |
| `PagedListResult.Web` -> (`RzR.ResultMessage.Pagination.AspNetCore`) | The ASP.NET Core integration: a base controller, a model binder (`[FromPagedQuery]`), minimal-API binders (`PagedQuery<T>`, `PagedQueryWithFilters<T>`), an action filter and an endpoint filter that emit the standard pagination headers, an allow-list registry, and an API Explorer convention. Targets `netstandard2.1` and .NET 5/6/7/8/9 (minimal-API helpers are gated to .NET 7+). |

## Request and response shapes

### Request

```jsonc
{
  "page": 1,
  "pageSize": 20,
  "search": {
    "search": "usb",
    "searchInAllTextFields": true,
    "searchInAllFields": false,
    "customSearchTextProperties": [ "Name", "Description" ]
  },
  "order": {
    "orderByProperty": "Price",
    "orderDirection": 1, // 0 = Asc, 1 = Desc
    "orderByDefaultProperty": false
  },
  "fields": [ "Id", "Name", "Price" ],
  "predefinedRecord": {
    "predefinedFieldName": "Id",
    "predefinedRecords": [ "1", "2", "3" ]
  },
  "filters": [
    {
      "filterValue": {
        "condition": 0, // Equals
        "propertyName": "Status",
        "values": [ "active" ],
        "compareValue": null
      },
      "filterApplyOrder": 0,
      "dependencies": []
    },
    {
      "filterValue": {
        "condition": 14, // Between (uses values[0] as lower bound and compareValue as upper bound)
        "propertyName": "Price",
        "values": [ "10" ],
        "compareValue": "100"
      },
      "filterApplyOrder": 1,
      "dependencies": [
        {
          "parentFilterLinkType": 0, // 0 = And, 1 = Or
          "filterValue": {
            "condition": 18, // IsNotNull
            "propertyName": "PublishedOn",
            "values": [],
            "compareValue": null
          }
        }
      ]
    }
  ]
}
```

The numeric values for `condition`, `orderDirection` and `parentFilterLinkType` map to the `FilterType`, `OrderDirection` and `FilterConditionType` enums; the full mapping table is in the [usage guide](docs/usage.md) and the [Data Models Reference](https://github.com/I-RzR-I/PagedListResult/wiki/Data-Models-Reference) wiki page.

### Response

```jsonc
{
  "currentPage": 1,
  "pageCount": 0,
  "pageSize": 10,
  "rowCount": 0,
  "executionDetails": {
    "executionTimeMs": 0,
    "executionDate": "yyyy-MM-ddTHH:mm:ss.fff"
  },
  "response": [],
  "isSuccess": true,
  "messages": []
}
```
## What is new in the Web integration

The Web layer (`RzR.ResultMessage.Pagination.AspNetCore`) adds a number of conveniences for ASP.NET Core projects:

- A registration entry point `services.AddPagedListResultWeb(...)` that exposes options such as `MaxPageSize`, `DefaultPageSize`, header toggles and a status-code mapper.
- A per-entity allow-list registered via `ConfigurePageable<TEntity>(b => b.AllowSort(...).AllowFilter(...).AllowSearch(...))`. Property names not in the allow-list are rejected with a `400 ValidationProblem` before any reflection runs.
- An MVC model binder activated by `[FromPagedQuery]` (and `[FromPagedQuery(typeof(TEntity))]`), so you can accept `PagedRequest` / `PageRequestWithFilters` straight from the query string in addition to the body-bound usage you already had.
- Minimal-API binders `PagedQuery<TEntity>` and `PagedQueryWithFilters<TEntity>` (.NET 7+) that follow the `BindAsync` pattern, expose `IsValid` / `Errors`, and share the exact same parsing semantics as the MVC pipeline.
- Standard pagination headers emitted by the action filter (MVC) and by the endpoint filter `WithPagedResult()` (minimal API): `X-Total-Count`, `X-Page-Count`, `X-Page-Size`, `X-Current-Page`, the RFC 5988 `Link` header (`first`, `prev`, `next`, `last`), and an opt-in `Server-Timing: paged;dur=...`.
- An API Explorer convention (`AddPagedListResultApiExplorer`) that auto-attaches `[Produces("application/json")]`, `[ProducesResponseType(typeof(PagedResult<T>), 200)]` and `[ProducesResponseType(typeof(ProblemDetails), 400)]` to any action whose return type resolves to `IPagedResult<T>` (including `Task<>`, `ValueTask<>` and `ActionResult<>` wrappers). Existing user-declared response types are preserved.
- RFC 9457 `application/problem+json` on the failure path through `AggregatedGenericResultMessage.Web`. The base controller helpers were renamed to `PagedOkResult<T>` and `PagedXmlResult<T>`; the previous `JsonResult<T>` and `XmlResult<T>` are kept as `[Obsolete]` for one release to avoid a hard break.

The full walk-through with code samples lives in the [usage guide](docs/usage.md).

## Content

1. [Usage guide](docs/usage.md)
2. [Changelog](docs/CHANGELOG.md)
3. [Branch guide](docs/branch-guide.md)
