using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using NUnit.Framework;

namespace ORMTests
{
    [TestFixture]
    public class Task2
    {
        public NorthwindDB NorthwindDb { get; set; }

        [SetUp]
        public void SetUp()
        {
            NorthwindDb = new NorthwindDB("Northwind");
        }

        [TearDown]
        public void TearDown()
        {
            NorthwindDb.Dispose();
        }

        [Test]
        public void TheListOfProductsWithCategoryAndSupplier()
        {
            // Arrange
            const string expectedProductName = "Chai";
            const string expectedSupplierName = "Charlotte Cooper";
            const string expectedCategoryName = "Beverages";

            var products = NorthwindDb.Products
               .SelectMany(
                   product => NorthwindDb.Categories
                       .Where(category => category.CategoryID == product.CategoryID)
                       .DefaultIfEmpty(),
                   (product, category) => new {product, category})
               .SelectMany(
                   productWithCategory => NorthwindDb.Suppliers
                       .Where(supplier => supplier.SupplierID == productWithCategory.product.SupplierID)
                       .DefaultIfEmpty(),
                   (productWithCategory, supplier) => 
                       Product.Build(productWithCategory.product, productWithCategory.category, supplier));

            // Act
            var firstProduct = products.FirstOrDefault();

            var actualProductName = firstProduct?.ProductName;
            var actualSupplierName = firstProduct?.Supplier.ContactName;
            var actualCategoryName = firstProduct?.Category.CategoryName;

            // Assert
            Assert.AreEqual(expectedProductName, actualProductName);
            Assert.AreEqual(expectedSupplierName, actualSupplierName);
            Assert.AreEqual(expectedCategoryName, actualCategoryName);
        }


        [Test]
        public void TheListOfEmployeesWithRegion()
        {
            // Arrange
            const string expectedFirstName = "Nancy";
            const string expectedRegion = "WA";

            var employees = NorthwindDb.Employees.Select(s => s);

            // Act
            var firstEmployee = employees.FirstOrDefault();
            
            var actualFirstName = firstEmployee?.FirstName;
            var actualRegion = firstEmployee?.Region;

            // Assert
            Assert.AreEqual(expectedFirstName, actualFirstName);
            Assert.AreEqual(expectedRegion, actualRegion);
        }

        [TestCase(1, 4)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 1)]
        public void TheStatisticByTheRegion(int regionId, int count)
        {
            // Arrange
            var expectedRegion = regionId;
            var expectedCount = count;

            var regionGrouped =
                (NorthwindDb.EmployeeTerritories
                    .Join(
                        NorthwindDb.Territories,
                        et => et.TerritoryID,
                        t => t.TerritoryID,
                        (et, t) => new {et, t})
                    .GroupBy(
                        grouped => new {grouped.t.RegionID, grouped.et.EmployeeID},
                        grouped => grouped.t))
                .GroupBy(grouped => grouped.Key.RegionID);
                

            // Act
            var regionById = regionGrouped.FirstOrDefault(grouped => grouped.Key == regionId);

            var actualRegion = regionById?.Key;
            var actualCount = regionById?.Count();

            // Assert
            Assert.AreEqual(expectedRegion, actualRegion);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestCase(1, new int[] {1, 2, 3})]
        [TestCase(2, new int[] {1, 2, 3})]
        [TestCase(3, new int[] {1, 2, 3})]
        [TestCase(4, new int[] {1, 2, 3})]
        public void TheStatisticByTheRegikkon(int regionId, int[] count)
        {
            // Arrange
            var expectedRegion = regionId;
            var expectedCount = count;

            var regionGrouped =
                (NorthwindDb.Orders
                    .GroupBy(
                        order => new { order.EmployeeID, order.ShipVia },
                        order => order))
                .GroupBy(order => order.Key.EmployeeID);


            // Act
            var regionById = regionGrouped.FirstOrDefault(grouped => grouped.Key == regionId);

            var actualRegion = regionById?.Key;
            var actualCount = regionById?.Select(r => r.Key.ShipVia).ToList();

            // Assert
            Assert.AreEqual(expectedRegion, actualRegion);
            CollectionAssert.AreEquivalent(expectedCount, actualCount);
        }
    }
}
