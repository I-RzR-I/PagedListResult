### **v6.1.0.4896** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 25-09-2026
* [5e5bfdd] (RzR) -> Upgrade `RzR.ResultMessage.Web` to `6.0.0.4426` and pin the `netstandard2.1` paged envelope overload explicitly.
* [5e5bfdd] (RzR) -> Accept the value-less filter syntax `?filter=name:IsNull`, which was rejected with `400` despite being documented.
* [5e5bfdd] (RzR) -> Guard `PagedRequest.Fields` against null before adding parsed `?fields=` values.
* [5e5bfdd] (RzR) -> Return `204 No Content` instead of throwing when a null paged result reaches `PagedOkResult` or `PagedXmlResult`.
* [5e5bfdd] (RzR) -> Point the sample controllers at `PagedOkResult` and declare `ResultMessageProblemDetails` as the `400` body.
* [5e5bfdd] (RzR) -> Document the `JsonResult<T>` removal, the `pageSize` rejection, and the options that are not honoured.
* [5e5bfdd] (RzR) -> Add regression tests for the paged failure path, the filter arity and the null guards.

### **v6.0.0.7820** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 24-09-2026
* [803bf32] (RzR) -> Auto commit uncommited files
* [c3913cb] (RzR) -> Remove obsolete `JsonResult<T>`/`XmlResult<T>` helpers.
* [2c21b72] (RzR) -> Upgrade `RzR.ResultMessage.Web` version and adapt the execution code.

### **v5.1.1.7490** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 06-08-2026
* [7931f66] (RzR) -> Auto commit uncommited files
* [b9624c6] (RzR) -> FIx wrap string properties in ToString() when building predicate.

### **v5.1.0.5630** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 31-07-2026
* [6772287] (RzR) -> Fix filter on Guid/non-IConvertible types; return failed result (not 500) on invalid filter/sort

### **v5.0.0.8548** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 02-07-2026
* [637655b] (RzR) -> Auto commit uncommited files
* [0591e26] (RzR) -> Fix ExecutionTimeMs assertion in Net7 paged test
* [79da268] (RzR) -> Adapt the using and readme documentation.
* [3613315] (RzR) -> Rename MinimalApi folder and namespace to Query
* [b765741] (RzR) -> Add query-string paged binding working on both MVC and Minimal API
* [a7c1ced] (RzR) -> Update package references to packages

### **v4.0.0.7837** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 30-04-2026
| Old (3.x)                      | New (4.x)         	                              |
|----------------------------------|------------------------------------------------------|
| `PagedListResult.DataModels`     | `RzR.ResultMessage.Pagination.Abstractions`          |
| `PagedListResult.Common`         | `RzR.ResultMessage.Pagination.Core`                  |
| `PagedListResult`                | `RzR.ResultMessage.Pagination.EntityFrameworkCore`   |
|  						           | `RzR.ResultMessage.Pagination.AspNetCore`            |



* [DEV] - (RzR) -> ASP.NET Core: `BaseApiPagedResultController.PagedOkResult` / `PagedXmlResult` with RFC 9457 `ProblemDetails` on failure.
* [DEV] - (RzR) -> ASP.NET Core: `[FromPagedQuery]` MVC binder + `PagedQuery<T>` / `PagedQueryWithFilters<T>` minimal-API wrappers (shared parser, allow-list validation).
* [DEV] - (RzR) -> ASP.NET Core: `AddPagedListResultWeb(...)` registers options, model binder, action filter, and (NET 7+) `WithPagedResult()` endpoint filter.
* [DEV] - (RzR) -> ASP.NET Core: `ToPagedHttpResult(...)` preserves the full paged envelope (fixes upstream `ToHttpResult` dropping paging metadata).
* [DEV] - (RzR) -> ASP.NET Core: response headers `X-Total-Count`, `X-Page-Count`, `X-Page-Size`, `X-Current-Page`, RFC 5988 `Link`, optional `Server-Timing`.
* [DEV] - (RzR) -> OpenAPI: `AddPagedListResultApiExplorer()` auto-attaches `[ProducesResponseType]` for `IPagedResult<T>` actions.
* [DEV] - (RzR) -> Core: `SearchInAllFields` mode and per-entity allow-list registration via `ConfigurePageable<T>(...)`.
* [DEV] - (RzR) -> Core: extra validation when `PredefinedRecord.PredefinedFieldName` is not provided; safer property/value conversion in filters.
* [DEV] - (RzR) -> Change root namespace from `PagedListResult` to `RzR.ResultMessage.Pagination`. Decouple web component `PagedListResult`.

* [FIX] - (RzR) -> `BaseApiPagedResultController.JsonResult<T>` / `XmlResult<T>` are `[Obsolete]` (collision with `Microsoft.AspNetCore.Mvc.JsonResult`); use `PagedOkResult` / `PagedXmlResult`. Will be removed in `5.0.0`.
* [FIX] - (RzR) -> `PagedResult<T>` overloads on `BaseApiPagedResultController` removed in favor of the `IPagedResult<T>` overloads.

### **v3.1.3.4848** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 30-03-2026
* [5ca0ecd] (RzR) -> Auto commit uncommited files
* [97d65c9] (RzR) -> Add new .net8 api for test
* [7a8c4c2] (RzR) -> Exclude the redundant `.ToString()` call on `Contains` and `DoesNotContains` filter.

### **v3.1.2.7586** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 19-03-2026
* [646d29d] (RzR) -> Fix possible error on predefined record filter.

### **v3.1.1.481** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 25-02-2026
* [897c27d] (RzR) -> Auto commit uncommited files
* [3f44f36] (RzR) -> Upgrade reference packages version

### **v3.1.0.6767** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 16-02-2026
* [a1d829c] (RzR) -> Add general search in all fields `SearchInAllFields`.
* [ed3b656] (RzR) -> Add new script for version gen.
 
 
### **v3.0.0.0**
#### Breaking changes
-> Changed definition for `PredefinedRecords`!<br />
-> Rename from `PredefinedRecords` -> `PredefinedRecord`;<br />
-> Set `PredefinedRecord` as object with 2 properties: <br />
    - `PredefinedFieldName` - name of the field/column to identify records <br />
    - `PredefinedRecords` - identifiers of the predefined records<br />

FROM
```csharp
public class PagedRequest
{
    // ...
    public ICollection<string> PredefinedRecords { get; set; } = new     HashSet<string>();
    // ...
}
```

TO
```csharp
public class PagedRequest
{
    // ...
    public DataPredefinedFilterDefinition PredefinedRecord { get; set; } = new DataPredefinedFilterDefinition();
    // ...
}
```

```csharp
public class DataPredefinedFilterDefinition
{
    public string PredefinedFieldName { get; set; }
    
    public ICollection<string> PredefinedRecords { get; set; } = new     HashSet<string>();
}
```

### **v2.0.0.0**
-> Add new project with required data models; <br />
-> Update reference packages version; <br />
-> Fix the references and update related projects; <br />
-> Adjust XML/SOAP in paged result; <br />
-> Generate new version and update changelog file;<br />
-> Update readme file;<br />

### **v1.0.3.6458**
-> Update reference package version, fixing CVE (`CVE-2024-43485`);<br />

### **v1.0.1.1104**
-> Update libs; <br />
-> Add cast result to Xml/Soap result; <br />

### **v1.0.2.7174**
-> Update libs version; <br />
-> Fix using reference; <br />



