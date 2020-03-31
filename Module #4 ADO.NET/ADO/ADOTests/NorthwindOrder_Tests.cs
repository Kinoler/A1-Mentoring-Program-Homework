using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ADO;
using ADO.DbConnectors;
using ADO.Interfaces;
using ADO.Models;
using ADO.RepositoriesImp;
using NUnit.Framework;
using Moq;

namespace ADOTests
{
    [TestFixture]
    public class NorthwindOrder_Tests
    {
        private NorthwindOrder NorthwindOrder { get; set; }
        private Mock<IDbConnector> MockDbConnector { get; set; }
        private Mock<INorthwindTable<OrderDetail>> MockNorthwindOrderDetail { get; set; }
        private DataTable DataTableOrder { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockDbConnector = new Mock<IDbConnector>();
            MockNorthwindOrderDetail = new Mock<INorthwindTable<OrderDetail>>();

            NorthwindOrder = new NorthwindOrder(MockDbConnector.Object, MockNorthwindOrderDetail.Object);

            DataTableOrder = new DataTable();
            var dataColumn = DataTableOrder.Columns.Add("OrderID", typeof(int));
            dataColumn = DataTableOrder.Columns.Add("CustomerID", typeof(string)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("EmployeeID", typeof(int)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("OrderDate", typeof(DateTime)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("RequiredDate", typeof(DateTime)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShippedDate", typeof(DateTime)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipVia", typeof(int)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("Freight", typeof(decimal)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipName", typeof(string)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipAddress", typeof(string)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipCity", typeof(string)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipRegion", typeof(string)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipPostalCode", typeof(string)); dataColumn.AllowDBNull = true;
            dataColumn = DataTableOrder.Columns.Add("ShipCountry", typeof(string)); dataColumn.AllowDBNull = true;

        }
        public DataRow NewRow(
            int orderId, 
            string customerId = null, 
            int? employeeId = null,
            DateTime? orderDate = null, 
            DateTime? requiredDate = null, 
            DateTime? shippedDate = null,
            int? shipVia = null,
            decimal? freight = null,
            string shipName = null,
            string shipAddress = null,
            string shipCity = null,
            string shipRegion = null,
            string shipPostalCode = null,
            string shipCountry = null)
        {
            var newRow = DataTableOrder.NewRow();
            newRow[0] = orderId;
            newRow[1] = customerId ?? (object)DBNull.Value;
            newRow[2] = employeeId ?? (object)DBNull.Value;
            newRow[3] = orderDate ?? (object)DBNull.Value;
            newRow[4] = requiredDate ?? (object)DBNull.Value;
            newRow[5] = shippedDate ?? (object)DBNull.Value;
            newRow[6] = shipVia ?? (object)DBNull.Value;
            newRow[7] = freight ?? (object)DBNull.Value;
            newRow[8] = shipName ?? (object)DBNull.Value;
            newRow[9] = shipAddress ?? (object)DBNull.Value;
            newRow[10] = shipCity ?? (object)DBNull.Value;
            newRow[11] = shipRegion ?? (object)DBNull.Value;
            newRow[12] = shipPostalCode ?? (object)DBNull.Value;
            newRow[13] = shipCountry ?? (object)DBNull.Value;

            DataTableOrder.Rows.Add(newRow);
            return newRow;
        }

        [Test]
        public void NorthwindOrder_GetElements_ShouldCall_GetDataTable_WithCorrectQuery()
        {
            var expected = NorthwindOrder.SelectAllQuery;

            NorthwindOrder.GetElements();

            MockDbConnector.Verify(dbConnector => dbConnector.GetDataTable(expected));
        }

        [Test]
        public void NorthwindOrder_GetElements_ShouldReturn_AllOrders_WithCorrectFillValue()
        {
            var order = NewRow(0).ToObject<Order>();
            var expected = new List<Order>() { order };

            MockDbConnector.Setup(dbConnector => 
                    dbConnector
                    .GetDataTable(It.IsAny<string>()))
                .Returns(DataTableOrder);

            MockNorthwindOrderDetail.Setup(northwindOrderDetail => 
                    northwindOrderDetail
                    .GetElements())
                .Returns<ICollection<OrderDetail>>(null);

            var actual = NorthwindOrder.GetElements().ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void NorthwindOrder_Add_ShouldCall_ExecuteNonQuery_WithCorrectQuery()
        {
            var order = new Order();
            var expected = string.Format(NorthwindOrder.AddQuery, "NULL", 
                "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", 
                "NULL", "NULL", "NULL", "NULL", "NULL", "NULL" );

            NorthwindOrder.Add(order);

            MockDbConnector.Verify(dbConnector => dbConnector.ExecuteNonQuery(expected));
        }

        [Test]
        public void NorthwindOrder_Update_ShouldThrow_WhenTryToUpdete_OrderWithStatusInProgres()
        {
            var order = new Order();
            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MinValue);

            Assert.Throws<ArgumentException>(() => NorthwindOrder.Update(order));
        }

        [Test]
        public void NorthwindOrder_Update_ShouldThrow_WhenTryToUpdete_OrderWithStatusComplete()
        {
            var order = new Order();
            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MinValue);
            order.GetType().GetProperty("ShippedDate")?.SetValue(order, DateTime.MinValue);

            Assert.Throws<ArgumentException>(() => NorthwindOrder.Update(order));
        }

        [Test]
        public void NorthwindOrder_Delete_ShouldThrow_WhenTryToDelete_OrderWithStatusComplete()
        {
            var order = new Order();
            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MinValue);
            order.GetType().GetProperty("ShippedDate")?.SetValue(order, DateTime.MinValue);

            Assert.Throws<ArgumentException>(() => NorthwindOrder.Delete(order));
        }

        [Test]
        public void NorthwindOrder_Delete_ShouldCall_ExecuteNonQuery_WithCorrectQuery()
        {
            const int orderId = 1;
            var order = new Order();
            order.GetType().GetProperty("OrderID")?.SetValue(order, orderId);

            var expected = string.Format(NorthwindOrder.DeleteOneQuery, orderId);

            NorthwindOrder.Delete(order);

            MockDbConnector.Verify(dbConnector => dbConnector.ExecuteNonQuery(expected));
        }
    }
}
