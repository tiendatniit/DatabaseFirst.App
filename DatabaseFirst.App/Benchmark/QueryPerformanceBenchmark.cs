using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class QueryPerformanceBenchmark : Config
{
    private readonly AdventureWorks2019Context dbContext;

    public QueryPerformanceBenchmark()
    {
        dbContext = new AdventureWorks2019Context();
    }

    [Benchmark]
    public async Task<List<Product>> Query_NotOptimize()
    {
        var utcNow = DateTime.UtcNow;
        var tenMonthsAgo = utcNow.AddMonths(-10);
        var orders = await dbContext.Products.ToListAsync();

        var myOrder = orders.Where(o => o.ModifiedDate > tenMonthsAgo && o.ModifiedDate < utcNow)
            .OrderBy(o => o.ModifiedDate).ToList();

        return orders;
    }

    [Benchmark]
    public async Task<List<ProductDataModel>> Query_Fully_Optimize()
    {
        var utcNow = DateTime.UtcNow;
        var tenMonthsAgo = utcNow.AddMonths(-10);
        var pageNumber = 1;
        var pageSize = 10;

        var result = await dbContext.Products.AsNoTracking()
            .Where(o => o.ModifiedDate > tenMonthsAgo && o.ModifiedDate < utcNow)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductDataModel
            {
                ModifiedDate = x.ModifiedDate,
                Class = x.Class,
                Color = x.Color,
            })
            .ToListAsync();

        return result;
    }
}

public class ProductDataModel
{
    public DateTime ModifiedDate { get; set; }
    public string? Class { get; set; }
    public string? Color { get; set; }
}