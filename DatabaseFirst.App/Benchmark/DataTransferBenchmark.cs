// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class DataTransferBenchmark : Config
{
    private readonly int _orderId = 43659; // Replace with your desired work order ID

    [Benchmark]
    public async Task<List<SalesOrderDetail>> GetOrderNotFilter_Fields_Query()
    {
        using (var context = new AdventureWorks2019Context())
        {
            return await context.SalesOrderDetails.AsNoTracking()
            .Include(wo => wo.SalesOrder)
            .ToListAsync();
        }
    }

    [Benchmark]
    public async Task<List<OrderDetails>> GetOrder_Filter_Fields_Query()
    {

        using (var context = new AdventureWorks2019Context())
        {
            return await context.SalesOrderDetails.AsNoTracking()
            .Include(wo => wo.SalesOrder).ThenInclude(xx => xx.CreditCard)
            .Include(wo => wo.SalesOrder).ThenInclude(woo => woo.ShipToAddress)
            .Include(wo => wo.SalesOrder).ThenInclude(xx => xx.BillToAddress)
            .Include(wo => wo.SalesOrder).ThenInclude(xx => xx.SalesPerson)
            .Include(wo => wo.SalesOrder).ThenInclude(xx => xx.ShipMethod)
            .Include(wo => wo.SalesOrder).ThenInclude(xx => xx.Customer)
            .Include(wo => wo.SalesOrder).ThenInclude(xx => xx.SalesOrderHeaderSalesReasons)
            .Select(x => new OrderDetails
            {
                SaleOrderId = x.SalesOrderId,
                CarrierTrackingNumber = x.CarrierTrackingNumber!,
                OrderQuantity = x.OrderQty,
                UnitPrice = x.UnitPrice,
                AccountNumber = x.SalesOrder.AccountNumber!,
                BIllingAddress = x.SalesOrder.BillToAddress,
                ShippingAddress = x.SalesOrder.ShipToAddress,
                Status = x.SalesOrder.Status,
                Tax = x.SalesOrder.TaxAmt,
                CardNumber = x.SalesOrder.CreditCard!.CardNumber!,
            })
            .ToListAsync();
        }

    }

    [Benchmark]
    public async Task<List<OrderDetails>> GetOrder_Filtered_Fields_Query()
    {

        using (var context = new AdventureWorks2019Context())
        {
            return await context.SalesOrderDetails.AsNoTracking()
            .Include(wo => wo.SalesOrder)
            .Select(x => new OrderDetails
            {
                SaleOrderId = x.SalesOrderId,
                CarrierTrackingNumber = x.CarrierTrackingNumber!,
                OrderQuantity = x.OrderQty,
                UnitPrice = x.UnitPrice,
                AccountNumber = x.SalesOrder.AccountNumber!,
                BIllingAddress = x.SalesOrder.BillToAddress,
                ShippingAddress = x.SalesOrder.ShipToAddress,
                Status = x.SalesOrder.Status,
                Tax = x.SalesOrder.TaxAmt,
                CardNumber = x.SalesOrder.CreditCard!.CardNumber!,
            })
            .ToListAsync();
        }
    }
}

public class OrderDetails
{
    public int SaleOrderId { get; set; }
    public string? CarrierTrackingNumber { get; set; }
    public short OrderQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? AccountNumber { get; set; }
    public Address? BIllingAddress { get; set; }
    public Address? ShippingAddress { get; set; }
    public decimal? Bonus { get; set; }

    public byte Status { get; set; }
    public decimal Tax { get; set; }
    public string? CardNumber { get; set; }
}