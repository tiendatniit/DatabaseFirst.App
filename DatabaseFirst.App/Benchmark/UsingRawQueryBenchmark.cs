// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using DatabaseFirst.App.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MemoryDiagnoser]
[Config(typeof(Config))]
public class UsingRawQueryBenchmark
{
    private readonly int orderId = 46666;

    [Benchmark]
    public async Task<List<OrderDetailWithDetailsModel>> GetOrderDetailsWithLinqQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            var results = await context.SalesOrderDetails
              .Where(od => od.SalesOrderId == orderId)
              .Join(context.SalesOrderHeaders,
                  od => od.SalesOrderId,
                  ohd => ohd.SalesOrderId,
                  (od, ohd) => new { OrderDetail = od, SalesOrderHeader = ohd }
              )
              .Join(context.Products,
                  odAndOhd => odAndOhd.OrderDetail.ProductId,
                  p => p.ProductId,
                  (odAndOhd, p) => new { Details = odAndOhd, Product = p }
              )
              .Join(context.ProductCategories,
                  detailsAndProduct => detailsAndProduct.Product.ProductSubcategoryId,
                  pc => pc.ProductCategoryId,
                  (detailsAndProduct, pc) => 
                  new 
                  { 
                      OrderDetails = detailsAndProduct.Details, 
                      Product = detailsAndProduct.Product, Category = pc 
                  }
              )
              .Join(context.Customers,
                  orderDetailsAndProductAndCategory => orderDetailsAndProductAndCategory.OrderDetails.SalesOrderHeader.CustomerId,
                  c => c.CustomerId,
                  (orderDetailsAndProductAndCategory, c) => new
                  {
                      Details = orderDetailsAndProductAndCategory.OrderDetails,
                      Product = orderDetailsAndProductAndCategory.Product,
                      Category = orderDetailsAndProductAndCategory.Category,
                      Customer = c
                  }
              )
              .Join(context.People,
                  detailsProductAndCategoryAndCustomer => detailsProductAndCategoryAndCustomer.Customer.PersonId,
                  ps => ps.BusinessEntityId,
                  (detailsProductAndCategoryAndCustomer, ps) => new OrderDetailWithDetailsModel
                  {
                      SalesOrderID = detailsProductAndCategoryAndCustomer.Details.OrderDetail.SalesOrderId,
                      ProductID = detailsProductAndCategoryAndCustomer.Details.OrderDetail.ProductId,
                      UnitPrice = detailsProductAndCategoryAndCustomer.Details.OrderDetail.UnitPrice,
                      Status = detailsProductAndCategoryAndCustomer.Details.SalesOrderHeader.Status, // Assuming Status comes from SalesOrderHeader
                      ProductName = detailsProductAndCategoryAndCustomer.Product.Name,
                      CategoryName = detailsProductAndCategoryAndCustomer.Category.Name,
                      Class = detailsProductAndCategoryAndCustomer.Product.Class!,
                      Color = detailsProductAndCategoryAndCustomer.Product.Color ?? string.Empty,
                      Weight = detailsProductAndCategoryAndCustomer.Product.Weight ?? 0M,
                      CustomerID = detailsProductAndCategoryAndCustomer.Customer.CustomerId,
                      CustomerName = ps.FirstName + " " + ps.LastName
                  }
              )
              .ToListAsync();

            return results;
        }
    }

    [Benchmark]
    public async Task<List<OrderDetailWithDetailsModel>> GetProductsByCategoryByRawQuery()
    {
        using (var context = new AdventureWorks2019Context())
        {
            // Define parameters for the stored procedure
            var parameter = new SqlParameter("@orderId", orderId);

            // Execute the stored procedure using FromSqlRaw with parameters
            var results = await context.Set<OrderDetailWithDetailsModel>()
              .FromSqlRaw("EXEC GetOrderDetailsWithDetails @orderId", parameter)
              .ToListAsync();

            return results;
        }
    }
}