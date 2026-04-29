using RzR.Extensions.EntityMock.Extensions;
using RzR.ResultMessage.Pagination;
using RzR.ResultMessage.Pagination.Web.Configuration;
using RzR.ResultMessage.Pagination.Web.Extensions;
using RzR.ResultMessage.Pagination.Web.MinimalApi;
using WebApiPagedResultDemo.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PagedListResult.Web Demo",
        Version = "v1",
        Description = "Demo API showing pagination headers + query-string binding"
    });
});

// Register PagedListResult.Web:
//   * adds the MVC convention + global action filter
//   * registers the singleton metadata registry (allow-lists)
//   * registers the model-binder provider for [FromPagedQuery]
//   * (net7+) adds the endpoint filter as a singleton
builder.Services
    .AddPagedListResultWeb(opts =>
    {
        opts.DefaultPageSize       = 10;
        opts.MaxPageSize           = 100;
        opts.EmitTotalCountHeader  = true;   // X-Total-Count / X-Page-* headers
        opts.EmitLinkHeader        = true;   // RFC 5988 Link header
        opts.EmitServerTimingHeader = true;  // Server-Timing: paged;dur=<ms>
    })

    // per-entity allow-list. Properties NOT listed here are rejected
    // by the parser (with a 400 ValidationProblem) before any reflection runs.
    .ConfigurePageable<Product>(b => b
        .AllowSort("name", "price", "id")
        .AllowFilter("status", "price", "name")
        .AllowSearch("name"));

builder.Services.AddSingleton<SampleDataStore>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "PagedListResult.Web Demo v1");
    o.RoutePrefix = "swagger";
});

// MVC controllers — headers emitted automatically by the action filter.
// ProductsController also showcases [FromPagedQuery] on a query-bound action.
app.MapControllers();

var minimal = app.MapGroup("/minimal/products").WithPagedResult();

minimal.MapGet("/", (int? page, int? pageSize, SampleDataStore store) =>
    Results.Ok(store.GetPaged(page ?? 1, pageSize ?? 10)))
    .WithName("MinimalProducts_Ok");

minimal.MapGet("/raw", (int? page, int? pageSize, SampleDataStore store) =>
    store.GetPaged(page ?? 1, pageSize ?? 10))
    .WithName("MinimalProducts_Raw");

minimal.MapGet("/http", (int? page, int? pageSize, SampleDataStore store, HttpContext ctx) =>
    store.GetPaged(page ?? 1, pageSize ?? 10).ToPagedHttpResult(ctx))
    .WithName("MinimalProducts_ToPagedHttpResult");

// GET /minimal/products/typed?page=2&pageSize=5&order=price:desc&search=Product
// GET /minimal/products/typed?order=secret:asc
// GET /minimal/products/typed?pageSize=99999 
minimal.MapGet("/typed", async (PagedQuery<Product> query, SampleDataStore store, HttpContext ctx) =>
{
    if (!query.IsValid)
        return Results.ValidationProblem(
            query.Errors.ToDictionary(e => e.Key, e => new[] { e.Value }));

    var result = await store.All().ToMockAsyncEnumerable().GetPagedAsync(query);

    return result.ToPagedHttpResult(ctx);
})
.WithName("MinimalProducts_PagedQuery");


// GET /minimal/products/filtered?filter=price:GreaterThan:50&order=price:desc
// GET /minimal/products/filtered?filter=status:Equals:active&filter=price:LessThan:100
// GET /minimal/products/filtered?filter=secret:Equals:x 
// GET /minimal/products/filtered?filter=price:Bogus:1 
minimal.MapGet("/filtered", async (PagedQueryWithFilters<Product> query, SampleDataStore store, HttpContext ctx) =>
{
    if (!query.IsValid)
        return Results.ValidationProblem(
            query.Errors.ToDictionary(e => e.Key, e => new[] { e.Value }));


    var result = await store.All().ToMockAsyncEnumerable().GetPagedWithFiltersAsync(query);

    return result.ToPagedHttpResult(ctx);
})
.WithName("MinimalProducts_PagedQueryWithFilters");

app.Run();
