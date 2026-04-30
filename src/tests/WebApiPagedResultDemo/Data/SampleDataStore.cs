using RzR.ResultMessage.Pagination.Abstractions.Models.Result;

namespace WebApiPagedResultDemo.Data
{

    public sealed class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }
    }

    public sealed class SampleDataStore
    {
        private static readonly string[] Statuses = { "active", "draft", "archived" };

        private readonly IEnumerable<Product> _items =
            Enumerable.Range(1, 137)
                .Select(i => new Product { Id = i, Name = $"Product #{i}", Price = 10m + i, Status = Statuses[i % Statuses.Length] })
                .ToList();

        public IEnumerable<Product> All() => _items;


        public PagedResult<Product> GetPaged(int page, int pageSize)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 10;

            var start = System.Diagnostics.Stopwatch.GetTimestamp();
            var slice = _items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalCount = _items.Count();
            var pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paged = new PagedResult<Product>
            {
                IsSuccess = true,
                Response = slice,
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = pageCount,
                RowCount = totalCount
            };

            var elapsedMs = (long)((System.Diagnostics.Stopwatch.GetTimestamp() - start)
                * 1000.0 / System.Diagnostics.Stopwatch.Frequency);
            paged.ExecutionDetails.SetExecutionTimeMs(elapsedMs, DateTime.UtcNow);

            return paged;
        }
    }
}