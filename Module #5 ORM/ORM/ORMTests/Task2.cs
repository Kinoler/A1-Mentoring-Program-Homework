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

        [TestCase("Chai", "Charlotte Cooper", "Beverages")]
        public void TheListOfProductsWithCategoryAndSupplier(
            string productName,
            string supplierName,
            string categoryName)
        {
            // Arrange
            var expectedProductName = productName;
            var expectedSupplierName = supplierName;
            var expectedCategoryName = categoryName;

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

        [TestCase("Nancy", 1)]
        public void TheListOfEmployeesWithRegion(
            string firstName,
            int regionId)
        {
            // Arrange
            var expectedFirstName = firstName;
            var expectedRegionId = regionId;

            var employees = NorthwindDb.EmployeeTerritories
                .Join(
                    NorthwindDb.Employees,
                    et => et.EmployeeID,
                    e => e.EmployeeID,
                    (et, e) => new {e.FirstName, et.TerritoryID })
                .Where(joined => joined.FirstName == firstName)
                .Join(
                    NorthwindDb.Territories,
                    et => et.TerritoryID,
                    t => t.TerritoryID,
                    (joined, t) => new { joined.FirstName, t.RegionID});

            // Act
            var firstEmployee = employees.FirstOrDefault();
            
            var actualFirstName = firstEmployee?.FirstName;
            var actualRegion = firstEmployee?.RegionID;

            // Assert
            Assert.AreEqual(expectedFirstName, actualFirstName);
            Assert.AreEqual(expectedRegionId, actualRegion);
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
                        (et, t) => new {et.EmployeeID, t.RegionID})
                    .Distinct())
                .GroupBy(grouped => grouped.RegionID)
                .Where(grouped => grouped.Key == regionId)
                .Select(grouped => new { grouped.Key, Count = grouped.Count() });

            // Act
            var regionById = regionGrouped.FirstOrDefault(grouped => grouped.Key == regionId);

            var actualRegion = regionById?.Key;
            var actualCount = regionById?.Count;

            // Assert
            Assert.AreEqual(expectedRegion, actualRegion);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestCase(1, new int[] {1, 2, 3})]
        [TestCase(2, new int[] {1, 2, 3})]
        [TestCase(3, new int[] {1, 2, 3})]
        [TestCase(4, new int[] {1, 2, 3})]
        public void TheListOfEmployeeWithShipVia(int regionId, int[] count)
        {
            // Arrange
            var expectedRegion = regionId;
            var expectedCount = count;

            var regionGrouped =
                NorthwindDb.Orders
                    .Select(order => new { order.EmployeeID, order.ShipVia })
                    .Distinct()
                    .GroupBy(order => order.EmployeeID);


            // Act
            var regionById = regionGrouped.FirstOrDefault(grouped => grouped.Key == regionId);

            var actualRegion = regionById?.Key;
            var actualCount = regionById?.Select(r => r.ShipVia).ToList();

            // Assert
            Assert.AreEqual(expectedRegion, actualRegion);
            CollectionAssert.AreEquivalent(expectedCount, actualCount);
        }
    }
}
