using Microsoft.AspNetCore.Mvc;
using RzR.ResultMessage.Pagination.DataModels.Models.Request.Page;
using RzR.ResultMessage.Pagination.Web;
using RzR.ResultMessage.Pagination.Web.Attributes;
using WebApiPagedResultDemo.Data;

namespace WebApiPagedResultDemo.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductsController : BaseApiPagedResultController
{
    private readonly SampleDataStore _store;

    public ProductsController(SampleDataStore store) => _store = store;

    // /products/GetV1?page=2&amp;pageSize=20
    [HttpGet("GetV1")]
    public IActionResult GetV1([FromQuery] int? page, [FromQuery] int? pageSize)
        => PagedOkResult(_store.GetPaged(page ?? 1, pageSize ?? 10));

    [HttpPost("GetByPostV1")]
    public IActionResult GetByPostV1()
        => PagedOkResult(_store.GetPaged(1, 10));

    // /products/query?page=2&amp;pageSize=20&amp;order=name:desc&amp;search=Product
    [HttpGet("query")]
    public IActionResult Query([FromPagedQuery] PagedRequest request)
        => PagedOkResult(_store.GetPaged(request.Page, request.PageSize));

    ///   GET /products/secured?order=price:desc&amp;filter=price:GreaterThan:50
    ///   GET /products/secured?order=secret:asc
    ///   GET /products/secured?filter=secret:Equals:x
    [HttpGet("secured")]
    public IActionResult Secured([FromPagedQuery(typeof(Product))] PageRequestWithFilters request)
        => PagedOkResult(_store.GetPaged(request.Page, request.PageSize));
}
