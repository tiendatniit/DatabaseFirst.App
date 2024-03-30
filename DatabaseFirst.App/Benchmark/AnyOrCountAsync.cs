using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class AnyOrCountAsyncBenchmark()
{
    [Benchmark]
    public async Task AnyAsync_With_ConditionQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = await context.Products.AnyAsync(x => x.ProductId == 123);
        }
    }

    [Benchmark]
    public async Task CountAsync_With_ConditionQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = await context.Products.CountAsync(x => x.ProductId == 123);
        }
    }

    [Benchmark]
    public async Task AnyAsync_No_Condition_Query()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = await context.Products.AnyAsync();
        }
    }

    [Benchmark]
    public async Task CountAsync_No_Condition_Query()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = await context.Products.CountAsync();
        }
    }
}
