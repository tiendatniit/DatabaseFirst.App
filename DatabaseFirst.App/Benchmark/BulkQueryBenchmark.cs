using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class BulkQueryBenchmark : Config
{
    [Benchmark]
    public async Task Update_By_Loop_Query()
    {
        using (var dbContext = new AdventureWorks2019Context())
        {
            var products = await dbContext.Products.Where(x => x.ProductId > 1).ToListAsync();

            foreach (Product product in products)
            {
                product.ModifiedDate = DateTime.UtcNow;
                dbContext.Products.Update(product);
                dbContext.SaveChanges();
            }
        }

    }

    [Benchmark]
    public async Task Update_By_Entry_Query()
    {
        using (var dbContext = new AdventureWorks2019Context())
        {
            var products = await dbContext.Products.Where(x => x.ProductId > 1).ToListAsync();

            foreach (Product product in products)
            {
                product.ModifiedDate = DateTime.UtcNow;
                // Update the UnitPrice property using EntityEntry
                dbContext.Entry(product).Property(p => p.ModifiedDate).CurrentValue = DateTime.UtcNow;

                // Mark the entity as modified (optional, but recommended for clarity)
                dbContext.Entry(product).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();
        }


    }

    [Benchmark]
    public async Task Bulk_Update_By_Single_Query()
    {
        using (var dbContext = new AdventureWorks2019Context())
        {
            var products = await dbContext.Products.Where(x => x.ProductId > 1).ToListAsync();
            products.ForEach(x => x.ModifiedDate = DateTime.UtcNow);

            dbContext.Products.UpdateRange(products);

            await dbContext.SaveChangesAsync();
        }
    }
}

