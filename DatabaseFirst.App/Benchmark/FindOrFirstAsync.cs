using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class FindOrFirstAsyncBenchmark()
{
    [Benchmark]
    public async Task FindAsyncQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = await context.Products.FirstAsync(x=>x.ProductId == 123);
        }
    }

    [Benchmark]
    public async Task FirstOrDefaultAsyncQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = await context.Products.FirstOrDefaultAsync(x => x.ProductId == 123);
        }
    }
}