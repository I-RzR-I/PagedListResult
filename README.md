> **Note** This repository is developed for .netstandard2.0 with support .net5, net6, .net7, .net8 and .net9.

| Name     | Details |
|----------|----------|
| PagedListResult.DataModels | [![NuGet Version](https://img.shields.io/nuget/v/PagedListResult.DataModels.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.DataModels/) [![Nuget Downloads](https://img.shields.io/nuget/dt/PagedListResult.DataModels.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.DataModels)|
| PagedListResult.Common | [![NuGet Version](https://img.shields.io/nuget/v/PagedListResult.Common.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.Common/) [![Nuget Downloads](https://img.shields.io/nuget/dt/PagedListResult.Common.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult.Common)|
| PagedListResult | [![NuGet Version](https://img.shields.io/nuget/v/PagedListResult.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult/) [![Nuget Downloads](https://img.shields.io/nuget/dt/PagedListResult.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/PagedListResult) |


This repository results from the necessity to implement pagination for grids/tables, a server-side pagination. The current solution is based on 3 projects, the first(`PagedListResult.DataModels`) represent the base data models, the second(`PagedListResult.Common`) represents the expressions builder for filters, search, and validation; the third(`PagedListResult`) uses extension methods to create the pagination requests and pagination results.

The `PagedListResult.DataModels` is written using the framework `.netstandard2.0` and contains the base required models, with the possibility of creating a new implementation based on them.

The `PagedListResult.Common` is written using framework `.netstandard2.0`, and based on `System.Linq.Expressions`. And the base available functionalities are:
search in fields, order records, filter be specific conditions, set on top some records, etc.

The `PagedListResult` have the extension methods to make pagination request and result more easily to integrate. It depends by `PagedListResult.Common` and the almost implementation are based on `Microsoft.EntityFrameworkCore`.

The `request` contains:
```json
{
  "page": 1,
  "pageSize": 10,
  "search": {
    "search": "string",
    "searchInAllTextFields": true,
    "searchInAllFields": false,
    "customSearchTextProperties": [
      "string"
    ]
  },
  "order": {
    "orderByProperty": "string",
    "orderDirection": 0,
    "orderByDefaultProperty": false
  },
  "fields": [
    "string"
  ],
  "predefinedRecord": {
    "predefinedFieldName": "string",
    "predefinedRecords": [
        "string"
    ]
  },
  "filters": [
    {
      "filterValue": {
        "condition": 0,
        "propertyName": "string",
        "values": [
          "string"
        ],
        "compareValue": "string"
      },
      "filterApplyOrder": 0,
      "dependencies": [
        {
          "parentFilterLinkType": 0,
          "filterValue": {
            "condition": 0,
            "propertyName": "string",
            "values": [
              "string"
            ],
            "compareValue": "string"
          }
        }
      ]
    }
  ]
}
```


The `result` contains:
```json
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



**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/PagedListResult" target="_blank">nuget.org</a>** or specify what version you want:


> `Install-Package PagedListResult -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)