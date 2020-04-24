using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using LinqToDB;
using NUnit.Framework;

namespace ORMTests
{
    [TestFixture]
    public class Task3
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

        [TestCase("FirstName", "LastName", new string[] { "06897", "14450", "98104" })]
        public void AddNewEmployeeWithTerritories(
            string firstName,
            string lastName,
            string[] territories)
        {
            // Arrange
            var expectedFirstName = firstName;
            var expectedLastName = lastName;
            var expectedTerritories = territories;

            // Act
            var employee = new Employee {FirstName = firstName, LastName = lastName };

            employee.EmployeeID = NorthwindDb.InsertWithInt32Identity(employee);
            foreach (var territoryID in territories)
                NorthwindDb.Insert(new EmployeeTerritory(employee.EmployeeID, territoryID));

            var employeeTerritories = NorthwindDb.EmployeeTerritories
                .Where(employeeTerritory => employeeTerritory.EmployeeID == employee.EmployeeID)
                .Select(employeeTerritory => employeeTerritory.TerritoryID);

            var employeeName = NorthwindDb.Employees
                .FirstOrDefault(employees => employees.EmployeeID == employee.EmployeeID);

            var actualFirstName = employeeName?.FirstName;
            var actualLastName = employeeName?.LastName;
            var actualTerritories = employeeTerritories.ToArray();

            // Assert
            Assert.AreEqual(expectedFirstName, actualFirstName);
            Assert.AreEqual(expectedLastName, actualLastName);
            CollectionAssert.AreEqual(expectedTerritories, actualTerritories);

            // Delete inserted
            foreach (var territoryID in territories)
                NorthwindDb.EmployeeTerritories.Delete(empTerritory => 
                    empTerritory.EmployeeID == employee.EmployeeID);

            NorthwindDb.Employees.Delete(emp => emp.EmployeeID == employee.EmployeeID);
        }

        [TestCase(1, 2)]
        public void MoveProductToAnotherCatigory(
            int productId,
            int toCatigoryId)
        {
            // Arrange
            var expectedProductId = productId;
            var expectedToCatigoryId = toCatigoryId;

            var oldCatigory = NorthwindDb.Products
                .Where(product => product.ProductID == productId)
                .Select(product => product.CategoryID)
                .FirstOrDefault();

            // Act
            NorthwindDb.Products
                .Where(p => p.ProductID == productId)
                .Set(p => p.CategoryID, toCatigoryId)
                .Update();

            var newCatigory = NorthwindDb.Products
                .Where(product => product.ProductID == productId)
                .Select(product => product.CategoryID);

            var actualToCatigoryId = newCatigory.FirstOrDefault();

            // Assert
            Assert.AreEqual(expectedToCatigoryId, actualToCatigoryId);

            // Remove
            NorthwindDb.Products
                .Where(p => p.ProductID == productId)
                .Set(p => p.CategoryID, oldCatigory)
                .Update();
        }

        [TestCase("productName", "companyName", "categoryName")]
        public void AddTheListOfProduct(
            string productName,
            string companyName,
            string categoryName)
        {
            // Arrange
            var expectedProductName = productName;
            var expectedCompanyName = companyName;
            var expectedCategoryName = categoryName;

            // Act
            var supplierId = NorthwindDb.Suppliers.FirstOrDefault(sup => sup.CompanyName == companyName)?.SupplierID ?? 
                             NorthwindDb.InsertWithInt32Identity(new Supplier { CompanyName = companyName });

            var categoryId = NorthwindDb.Categories.FirstOrDefault(cat => cat.CategoryName == categoryName)?.CategoryID ?? 
                             NorthwindDb.InsertWithInt32Identity(new Category { CategoryName = categoryName });

            var productId = NorthwindDb.InsertWithInt32Identity(new Product() { ProductName = productName, SupplierID = supplierId, CategoryID = categoryId });

            var actualProductName = NorthwindDb.Products.Find(productId).ProductName;
            var actualCompanyName = NorthwindDb.Suppliers.Find(supplierId).CompanyName;
            var actualCategoryName = NorthwindDb.Categories.Find(categoryId).CategoryName;

            // Assert

            Assert.AreEqual(expectedProductName, actualProductName);
            Assert.AreEqual(expectedCompanyName, actualCompanyName);
            Assert.AreEqual(expectedCategoryName, actualCategoryName);

            // Remove
            NorthwindDb.Products.Delete(prod => prod.ProductID == productId);
            NorthwindDb.Suppliers.Delete(sup => sup.SupplierID == supplierId);
            NorthwindDb.Categories.Delete(cat => cat.CategoryID == categoryId);
        }

        [TestCase(1, 2)]
        public void ReplaceTheProductToItsCopy(int oldProductId, int newProductId)
        {
            // Act
            NorthwindDb.OrderDetails
                .Where(orderDetail => orderDetail.ProductID == oldProductId)
                .Set(
                    orderDetail => orderDetail.ProductID,
                    newProductId)
                .Update();

            // Assert
            var actualOldProducts = NorthwindDb.OrderDetails.Where(orderDetail => orderDetail.ProductID == oldProductId);

            CollectionAssert.IsEmpty(actualOldProducts);

            // Remove
            NorthwindDb.OrderDetails
                .Where(orderDetail => orderDetail.ProductID == newProductId)
                .Set(
                    orderDetail => orderDetail.ProductID,
                    oldProductId)
                .Update();
        }
    }
}
