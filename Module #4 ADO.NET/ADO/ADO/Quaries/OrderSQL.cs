namespace ADO.Quaries
{
    internal class OrderSQL : OrderQuary
    {
        public override string AddQuery =>
            @"INSERT INTO Orders 
            VALUES (
            CustomerID,
            EmployeeID,
            OrderDate,
            RequiredDate,
            ShippedDate,
            ShipVia,
            Freight,
            ShipName,
            ShipAddress,
            ShipCity,
            ShipRegion,
            ShipPostalCode,
            ShipCountry);";

        public override string SelectAllQuery =>
            @"SELECT * FROM Orders 
            ORDER BY Orders.OrderID 
            OFFSET @Offset ROWS
            FETCH NEXT @Count ROWS ONLY;";

        public override string SelectOneQuery =>
            @"SELECT * FROM Orders WHERE OrderID = @OrderID;";

        public override string SelectOneDetailQuery =>
            @"SELECT *
            FROM [Order Details] ordarDetails
            LEFT JOIN Products product ON product.ProductID = ordarDetails.ProductID
            LEFT JOIN Categories categories ON categories.CategoryID = product.CategoryID
            WHERE ordarDetails.OrderID = @OrderID;";

        public override string DeleteOneQuery =>
            @"DELETE FROM Orders WHERE OrderID = @OrderID;";

        public override string UpdateQuery =>
            @"UPDATE * FROM Orders
            SET 
            CustomerID = @CustomerID,
            EmployeeID = @EmployeeID,
            OrderDate = @OrderDate,
            RequiredDate = @RequiredDate,
            ShippedDate = @ShippedDate,
            ShipVia = @ShipVia,
            Freight = @Freight,
            ShipName = @ShipName,
            ShipAddress = @ShipAddress,
            ShipCity = @ShipCity,
            ShipRegion = @ShipRegion,
            ShipPostalCode = @ShipPostalCode,
            ShipCountry = @ShipCountry
            WHERE OrderID = @OrderID;";
    }
}
