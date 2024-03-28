DROP PROCEDURE IF EXISTS [dbo].[GetOrderDetailsWithDetails]
go
CREATE PROCEDURE GetOrderDetailsWithDetails (@orderId int)
AS
BEGIN
  SELECT 
    od.SalesOrderID,
    od.ProductID,
    od.UnitPrice,
    ohd.Status,
    p.Name AS ProductName,
    pc.Name AS CategoryName,
    p.Class,
    p.Color,
    p.Weight,
    c.CustomerID,
    ps.FirstName + ' ' + ps.LastName AS CustomerName
  FROM [Sales].[SalesOrderDetail]  od
  INNER JOIN [Sales].[SalesOrderHeader] ohd on ohd.SalesOrderID = od.SalesOrderID
  INNER JOIN Production.Product p ON od.ProductID = p.ProductID
  INNER JOIN Production.ProductCategory pc ON p.ProductSubCategoryID = pc.ProductCategoryID
  INNER JOIN [Production].[WorkOrder]  o ON od.SalesOrderID = o.WorkOrderID
 INNER JOIN  [Sales].[Customer] c on c.CustomerID = ohd.CustomerID
 INNER JOIN  [Person].[Person]  ps on ps.BusinessEntityID = c.PersonID

  WHERE od.SalesOrderID = @orderId;
END

-- EXEC GetOrderDetailsWithDetails  @orderId = 46666 


--SELECT 
--    count(od.SalesOrderID) as [sort],
--    od.SalesOrderID
--  FROM [Sales].[SalesOrderDetail]  od
--  INNER JOIN [Sales].[SalesOrderHeader] ohd on ohd.SalesOrderID = od.SalesOrderID
--  INNER JOIN Production.Product p ON od.ProductID = p.ProductID
--  INNER JOIN Production.ProductCategory pc ON p.ProductSubCategoryID = pc.ProductCategoryID
--  INNER JOIN [Production].[WorkOrder]  o ON od.SalesOrderID = o.WorkOrderID
-- INNER JOIN  [Sales].[Customer] c on c.CustomerID = ohd.CustomerID
-- INNER JOIN  [Person].[Person]  ps on ps.BusinessEntityID = c.PersonID
-- group by od.SalesOrderID
-- order by  [sort] desc