using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class CompiledQueryBenchmark : Config
{
    [Benchmark]
    public async Task GetProductsWithUnCompiledQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var products = context.Products
           .Where(p => p.DaysToManufacture > 10 && p.ListPrice > 1).ToListAsync();
        }
    }

    [Benchmark]
    public async Task GetProductsWithCompiledQuery_1()
    {
        Func<AdventureWorks2019Context, string, decimal, Task<List<Product>>> getProductsAsync = async (AdventureWorks2019Context context, string category, decimal minPrice)
            =>
        {
            var parameterCategory = new SqlParameter("@category", category);
            var parameterMinPrice = new SqlParameter("@minPrice", minPrice);

            return await context.Products
              .Where(p => p.DaysToManufacture > 10 && p.ListPrice > 1).ToListAsync();
        };

        using (var context = new AdventureWorks2019Context())
        {
            var products = await getProductsAsync(context, "ABC", 1);
        }
    }
}
