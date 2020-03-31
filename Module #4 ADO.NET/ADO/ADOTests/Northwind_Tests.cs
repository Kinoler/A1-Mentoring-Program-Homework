using System;
using ADO;
using ADO.Interfaces;
using ADO.Models;
using Moq;
using NUnit;
using NUnit.Framework;

namespace ADOTests
{
    [TestFixture]
    public class Northwind_Tests
    {
        private Mock<IDbConnector> MockDbConnector { get; set; }
        private Northwind Northwind { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockDbConnector = new Mock<IDbConnector>();
            Northwind = new Northwind(MockDbConnector.Object);
        }

        [Test]
        public void Northwind_SetOrderDate_ShouldSet_OrderDate()
        {
            var order = new Order();
            var expected = DateTime.MinValue;

            Northwind.SetOrderDate(order, expected);

            var actual = order.OrderDate;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Northwind_SetOrderDate_ShouldThrow_WhenTryToSet_OrdetWithStatusInProgress()
        {
            var order = new Order();
            var expected = DateTime.MinValue;

            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MaxValue);

            Assert.Throws<ArgumentException>(() => Northwind.SetOrderDate(order, expected));
        }

        [Test]
        public void Northwind_SetOrderDate_ShouldThrow_WhenTryToSet_OrdetWithStatusComplete()
        {
            var order = new Order();
            var expected = DateTime.MinValue;

            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MaxValue);
            order.GetType().GetProperty("ShippedDate")?.SetValue(order, DateTime.MaxValue);

            Assert.Throws<ArgumentException>(() => Northwind.SetOrderDate(order, expected));
        }

        [Test]
        public void Northwind_SetShippedDate_ShouldSet_ShippedDate()
        {
            var order = new Order();
            var expected = DateTime.MinValue;
            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MaxValue);

            Northwind.SetShippedDate(order, expected);

            var actual = order.ShippedDate;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Northwind_SetShippedDate_ShouldThrow_WhenTryToSet_OrdetWithStatusInProgress()
        {
            var order = new Order();
            var expected = DateTime.MinValue;

            Assert.Throws<ArgumentException>(() => Northwind.SetShippedDate(order, expected));
        }

        [Test]
        public void Northwind_SetShippedDate_ShouldThrow_WhenTryToSet_OrdetWithStatusComplete()
        {
            var order = new Order();
            var expected = DateTime.MinValue;

            order.GetType().GetProperty("OrderDate")?.SetValue(order, DateTime.MaxValue);
            order.GetType().GetProperty("ShippedDate")?.SetValue(order, DateTime.MaxValue);

            Assert.Throws<ArgumentException>(() => Northwind.SetShippedDate(order, expected));
        }

        [Test]
        public void Northwind_CustOrderHis_ShouldCall_CallStoredProcedure_WithCorrectParemeter()
        {
            var expected = "CustOrderHis";
            Northwind.CustOrderHis("");

            MockDbConnector.Verify(dbConnector => 
                dbConnector.CallStoredProcedure(expected, new System.Data.IDataParameter[]{ null }));
        }

        [Test]
        public void Northwind_CustOrdersDetail_ShouldCall_CallStoredProcedure_WithCorrectParemeter()
        {
            var expected = "CustOrdersDetail";
            Northwind.CustOrdersDetail(0);

            MockDbConnector.Verify(dbConnector =>
                dbConnector.CallStoredProcedure(expected, new System.Data.IDataParameter[] { null }));
        }
    }
}
