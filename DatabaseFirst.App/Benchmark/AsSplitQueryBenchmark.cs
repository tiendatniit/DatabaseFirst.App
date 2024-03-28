using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class NSelectQueryBenchmark
{
    private readonly int orderId = 46666;

    [Benchmark]
    public async Task GetWorkOrdersWithNSelect()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var orders = await context.WorkOrders.Where(o => o.WorkOrderId == orderId).ToListAsync();
            foreach (var order in orders)
            {
                order.WorkOrderRoutings = await context.WorkOrderRoutings.Where(od => od.WorkOrderId == orderId).ToListAsync();
            }
        }
    }

    [Benchmark]
    public async Task GetWorkOrdersWithInClude()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var orders = await context.WorkOrders
                .AsNoTracking()
                .Where(o => o.WorkOrderId == orderId)
                .Include(o => o.WorkOrderRoutings)
                .Select(s => new WorkOrderRoutingDetail
                {
                    WorkOrderId = s.WorkOrderId,
                    ProductId = s.ProductId,
                    Quantity = s.OrderQty,
                    Location = s.WorkOrderRoutings
                }).ToListAsync();
        }
    }

}

internal class WorkOrderRoutingDetail
{
    public int WorkOrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public ICollection<WorkOrderRouting> Location { get; set; }
}