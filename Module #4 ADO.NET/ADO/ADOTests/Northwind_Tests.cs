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
            var Categories = Northwind.Categories;
            var OrderDetails = Northwind.OrderDetails;
            var Products = Northwind.Products; 
            var Orders = Northwind.Orders;
            var Regions = Northwind.Region;

            var order = new Order();

            Northwind.AddOrder(order);
        }
    }
}
