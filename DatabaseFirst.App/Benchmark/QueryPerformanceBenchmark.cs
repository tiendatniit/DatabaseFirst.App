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

    //[Benchmark]
    //public async Task<List<Product>> QueryNotOptimize()
    //{
    //    var utcNow = DateTime.UtcNow;
    //    var tenMonthsAgo = utcNow.AddMonths(-10);
    //    var orders = await dbContext.Products.ToListAsync();

    //    var myOrder = orders.Where(o => o.ModifiedDate > tenMonthsAgo && o.ModifiedDate < utcNow)
    //        .OrderBy(o => o.ModifiedDate).ToList();

    //    return orders;
    //}

    [Benchmark]
    public async Task<List<Product>> QueryAlreadyOptimize()
    {
        var utcNow = DateTime.UtcNow;
        var tenMonthsAgo = utcNow.AddMonths(-10);
        var pageNumber = 2;
        var pageSize = 10;
        var result = await dbContext.Products.AsNoTracking()
            .Where(o => o.ModifiedDate > tenMonthsAgo && o.ModifiedDate < utcNow)
            .OrderBy(o => o.ModifiedDate).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new Product
            {
                ModifiedDate = utcNow,
                Color = p.Color,
                Name = p.Name,
            })
            .ToListAsync();

        return result;
    }
}