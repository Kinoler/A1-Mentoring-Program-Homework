using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ADO;
using ADO.DbConnectors;
using ADO.Interfaces;
using ADO.Models;
using ADO.Quaries;
using ADO.RepositoriesImp;
using NUnit.Framework;
using Moq;
using ADO.Extensions;

namespace ADOTests
{
    [TestFixture]
    public class NorthwindOrder_Tests
    {
        private OrdersRepository NorthwindOrder { get; set; }
        private Mock<IDbConnector> MockDbConnector { get; set; }
        private DataSet DataSetOrder { get; set; }
        private DataTable DataTableOrder { get; set; }
        private OrderSQL OrderSql { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockDbConnector = new Mock<IDbConnector>();
            OrderSql = new OrderSQL();

            NorthwindOrder = new OrdersRepository(MockDbConnector.Object, OrderSql);
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

            DataSetOrder = new DataSet();
            DataSetOrder.Tables.Add(DataTableOrder);
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
        public void GetElements_CallMethodWithValidValue_ShouldCallGetDataTableWithCorrectQuery()
        {
            //Arrange
            var expected = OrderSql.SelectAllQuery;
            var windowCapacity = 0;
            var windowNumber = 0;

            //Act
            NorthwindOrder.GetOrders(windowCapacity, windowNumber);

            //Assert
            MockDbConnector.Verify(dbConnector => dbConnector.GetDataSet(expected, null, null));
        }

        [Test]
        public void GetElements_CallMethodWithValidValue_ShouldReturnAllOrdersWithCorrectFillValue()
        {
            //Arrange
            var order = NewRow(0).ToObject<Order>();
            var expected = order ;

            //Act
            MockDbConnector.Setup(dbConnector =>
                    dbConnector.GetDataSet(It.IsAny<string>(), null, null))
                .Returns(DataSetOrder);

            var actual = NorthwindOrder.GetOrders(0,0).ToList().FirstOrDefault();

            //Assert
            //That is the place where was needed the implementation of equals
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.CustomerID, actual.CustomerID);
            Assert.AreEqual(expected.EmployeeID, actual.EmployeeID);
            Assert.AreEqual(expected.Freight, actual.Freight);
            Assert.AreEqual(expected.OrderDate, actual.OrderDate);
            Assert.AreEqual(expected.OrderID, actual.OrderID);
            Assert.AreEqual(expected.RequiredDate, actual.RequiredDate);
            Assert.AreEqual(expected.ShipAddress, actual.ShipAddress);
            Assert.AreEqual(expected.ShipCity, actual.ShipCity);
            Assert.AreEqual(expected.ShipCountry, actual.ShipCountry);
            Assert.AreEqual(expected.ShipName, actual.ShipName);
            Assert.AreEqual(expected.ShippedDate, actual.ShippedDate);
            Assert.AreEqual(expected.ShipPostalCode, actual.ShipPostalCode);
            Assert.AreEqual(expected.ShipRegion, actual.ShipRegion);
            Assert.AreEqual(expected.ShipVia, actual.ShipVia);
        }

        [Test]
        public void Add_CallMethodWithValidValue_ShouldCallExecuteNonQueryWithCorrectQuery()
        {
            //Arrange
            var order = new Order();
            var expected = OrderSql.AddQuery;

            //Act
            NorthwindOrder.Add(order);

            //Assert
            MockDbConnector.Verify(dbConnector => dbConnector.ExecuteNonQuery(expected, 
                null, null, null, null, null, null,
                null, null, null, null, null, null, null));
        }

    [Test]
        public void Update_CallMethodWithOrderStateInProgress_ThrowInvalidOperationException()
        {
            //Arrange
            var order = new Order {OrderDate = DateTime.MinValue};

            //Assert
            Assert.Throws<InvalidOperationException>(() => NorthwindOrder.Update(order));
        }

        [Test]
        public void Update_CallMethodWithOrderStateComplete_ThrowInvalidOperationException()
        {
            //Arrange
            var order = new Order {OrderDate = DateTime.MinValue, ShippedDate = DateTime.MinValue};

            //Assert
            Assert.Throws<InvalidOperationException>(() => NorthwindOrder.Update(order));
        }

        [Test]
        public void Delete_CallMethodWithOrderStateComplete_ThrowInvalidOperationException()
        {
            //Arrange
            var order = new Order { OrderDate = DateTime.MinValue, ShippedDate = DateTime.MinValue };

            //Assert
            Assert.Throws<InvalidOperationException>(() => NorthwindOrder.Delete(order));
        }

        [Test]
        public void SetOrderDate_TryToSetSomeValueToOrderDateProperty_ShouldSetOrderDate()
        {
            //Arrange
            var order = new Order();
            var expected = DateTime.MinValue;

            //Act
            NorthwindOrder.SetOrderDate(order, expected);
            var actual = order.OrderDate;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test] 
        public void SetOrderDate_CallMethodWithOrderStateInProgress_ThrowInvalidOperationException()
        {
            //Arrange
            var order = new Order {OrderDate = DateTime.MaxValue};
            var expected = DateTime.MinValue;

            //Assert
            Assert.Throws<ArgumentException>(() => NorthwindOrder.SetOrderDate(order, expected));
        }

        [Test]
        public void SetOrderDate_CallMethodWithOrderStateComplete_ThrowInvalidOperationException()
        {
            //Arrange
            var order = new Order { OrderDate = DateTime.MaxValue, ShippedDate = DateTime.MaxValue };
            var expected = DateTime.MinValue;

            //Assert
            Assert.Throws<ArgumentException>(() => NorthwindOrder.SetOrderDate(order, expected));
        }

        [Test]
        public void SetShippedDate_TryToSetSomeValueToShippedDateProperty_ShouldSetShippedDate()
        {
            //Arrange
            var order = new Order { OrderDate = DateTime.MaxValue };
            var expected = DateTime.MinValue;

            //Act
            NorthwindOrder.SetShippedDate(order, expected);
            var actual = order.ShippedDate;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetShippedDate_CallMethodWithOrderStateInProgress_ThrowInvalidOperationException()
        {
            //Arrange
            var order = new Order();
            var expected = DateTime.MinValue;

            //Assert
            Assert.Throws<ArgumentException>(() => NorthwindOrder.SetShippedDate(order, expected));
        }

        [Test]
        public void SetShippedDate_CallMethodWithOrderStateComplete_ThrowInvalidOperationExceptionComplete()
        {
            //Arrange
            var order = new Order { OrderDate = DateTime.MaxValue, ShippedDate = DateTime.MaxValue };
            var expected = DateTime.MinValue;

            //Assert
            Assert.Throws<ArgumentException>(() => NorthwindOrder.SetShippedDate(order, expected));
        }
    }
}
