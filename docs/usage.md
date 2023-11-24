# USING

For current use this repository and available functionalities, you must install `PagedListResult`, which will be install all necessary dependencies.

In the `PagedListResult` are a extension methods that allow to build and generate pageed grid result:
- `GetPaged`;
- `GetPagedAsync`;
- `GetPagedWithMainFiltersAsync`;
- `GetPagedWithFiltersAsync`.

Also this repository have reference to `AggregatedGenericResultMessage.Web`, which will do more easily return structured JSON result. For confortable work with paged result in repository is added a derived class for `BaseApiPagedResultController` from `ResultBaseApiController`.
This class have method `JsonResult`, that accept result type of `IPagedResult` or `PagedResult`, which is part of `PagedListResult` based for paged result contructor.

| Param       | Description                              |
|-------------|------------------------------------------|
| PagedResult | Implementation of how data will be returned to the client, fields like `CurrentPage`, `PageCount`, `PageSize`, `RowCount`, `Response`, and execution time details `ExecutionDetails`. |
|DefaultPrimaryKeyDefinition|This object represent information about the Model/DTO primary key field which will be used in on search for in top grid records (`PredefinedRecords`).|
|TPageRequest -> `PagedRequest` | Represent information which is required to cread paged result (`Page`, `PageSize`, `Search`, `Order`, `Fields`, `PredefinedRecords`). |

The `GetPaged` have two different implementation:
- the first is for simple paged result, with no filters: `PagedResult<TSource> GetPaged<TSource>(this IQueryable<TSource> query, int page, int pageSize)`. You must provide current query, current user page, and page size (how many records in grid page);
- the second implementation, accept query filters: `PagedResult<TSource> GetPaged<TSource, TPageRequest>(this IQueryable<TSource> query, TPageRequest request, DefaultPrimaryKeyDefinition defaultPrimaryKey = null)`. In this case you must set your query, current page request filters and default primary key for DTO (used in case when you user filters to set some records on the top of result `PredefinedRecords`).

The `GetPagedAsync` have two different implementation as in `GetPaged`.
Importat to specify that in both methods `GetPaged` and `GetPagedAsync`, the `TPageRequest` information will be used only for pagination, order and general seach data.

For next two methods(`GetPagedWithMainFiltersAsync`, `GetPagedWithFiltersAsync`) the implementation is more specific:

| Param       | Description                              |
|-------------|------------------------------------------|
|TPageRequest -> `PageRequestWithFilters` | Represents an extensions for `PagedRequest` and have additional information, `Filters`. |

The `GetPagedWithMainFiltersAsync` is implementation of pagination, search, order, set records in top of result, and simple implementation of filter.
The simple implementation means that provided filters will be applied at the top level and condition between filters will be 'AND' (Ex. Code = 'Test001' AND IsActive = true).

The `GetPagedWithFiltersAsync` include all previous specified options, and in addition with more possibilities, to specify main filter link `AND` or `OR`, and possibility to specify dependencies filter link type, and also all specified dependecies filter by general filter will be applied.

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
    [ProducesResponseType(typeof(IEnumerable<MessageModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRecords(
        [FromBody] GetRecordsQuery query,
        CancellationToken cancellationToken)
    {
        var queryResponse = await _mediator.Send(query, cancellationToken);

        return JsonResult(queryResponse);
    }
}

//------------------------------------------------
// Requst query
//------------------------------------------------
 public class GetRecordsQuery : PageRequestWithFilters,
     IRequest<IPagedResult<PostDetail>>
 {

 }
 
//------------------------------------------------ 
// Request handler
//------------------------------------------------
public class GetRecordsHandler : IRequestHandler<GetRecordsQuery, IPagedResult<PostDetail>>
{
    private readonly AppDbContext _db;

    public GetRecordsHandler(AppDbContext db) => _db = db;

    /// <inheritdoc />
    public async Task<IPagedResult<PostDetail>> Handle(GetRecordsQuery request, CancellationToken cancellationToken)
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

        return await data
            .GetPagedWithFiltersAsync(request, 
                new DefaultPrimaryKeyDefinition("Id"), 
                cancellationToken: cancellationToken);
    }
}
```
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

var records = await query
.GetPagedWithFiltersAsync(pageRequest, filterLink: FilterConditionType.Or);
```



