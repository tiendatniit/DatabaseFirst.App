using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class SingleOrDefaultAsyncBenchmark : Config
{
    [Benchmark]
    public async Task<Product> GetSingleOrDefaultAsyncQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            return await context.Products.SingleOrDefaultAsync(p => p.ProductId == 123);
        }
    }

    [Benchmark]
    public async Task<Product> GetDefaultOrDefaultAsyncQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            return await context.Products.FirstOrDefaultAsync(p => p.ProductId == 123);
        }
    }
}
