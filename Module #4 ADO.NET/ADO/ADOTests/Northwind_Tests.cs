using System;
using ADO;
using ADO.Models;
using NUnit;
using NUnit.Framework;

namespace ADOTests
{
    [TestFixture]
    public class Northwind_Tests
    {
        Northwind Northwind;
        [SetUp]
        public void SetUp()
        {
            Northwind = new Northwind("Northwind");
        }

        [Test]
        public void TestMethod1()
        {
            var Orders = Northwind.Orders;

            var order = new Order();

            Orders.Add(order);
        }
    }
}
