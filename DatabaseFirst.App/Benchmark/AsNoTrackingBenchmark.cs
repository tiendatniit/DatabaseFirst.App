using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class AsNoTrackingBenchmark : Config
{
    private readonly AdventureWorks2019Context dbContext;

    public AsNoTrackingBenchmark()
    {
        dbContext = new AdventureWorks2019Context();
    }

    [Benchmark]
    public async Task<List<WorkOrder>> QueryWithTracking()
    {
        var orders = await dbContext.WorkOrders.Include(x => x.ScrapReason)
            .Where(o => o.ScrapReason.Name.StartsWith("ABC")).ToListAsync();

        return orders;
    }

    [Benchmark]
    public async Task<List<WorkOrder>> QueryWithoutTracking()
    {
        var orders = await dbContext.WorkOrders.AsNoTracking().Include(x=>x.ScrapReason)
            .Where(o => o.ScrapReason.Name.StartsWith("ABC")).ToListAsync();

        return orders;
    }
}
