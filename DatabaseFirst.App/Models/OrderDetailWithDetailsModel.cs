namespace DatabaseFirst.App.Models;

public class OrderDetailWithDetailsModel
{
    public int SalesOrderID { get; set; }
    public int ProductID { get; set; }
    public decimal UnitPrice { get; set; }
    public byte Status { get; set; } // Assuming Status comes from SalesOrderHeader
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string Class { get; set; }
    public string Color { get; set; }
    public decimal Weight { get; set; }
    public int CustomerID { get; set; }
    public string CustomerName { get; set; }
}