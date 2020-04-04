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
            MockDbConnector.Setup(connector => connector.ProviderName).Returns("System.Data.SqlClient");
            Northwind = new Northwind(MockDbConnector.Object);
        }

        [Test]
        public void CustOrderHis_CallTheCallStoredProcedureMethod_ShouldCallCallStoredProcedureWithCorrectParemeter()
        {
            //Arrange
            var expected = "CustOrderHis";

            //Act
            Northwind.CustOrderHis("");

            //Assert
            MockDbConnector.Verify(dbConnector => 
                dbConnector.CallStoredProcedure(expected, new System.Data.IDataParameter[]{ null }));
        }

        [Test] 
        public void CustOrdersDetail_CallTheCallStoredProcedureMethod_ShouldCallCallStoredProcedureWithCorrectParemeter()
        {
            //Arrange
            var expected = "CustOrdersDetail";

            //Act
            Northwind.CustOrdersDetail(0);

            //Assert
            MockDbConnector.Verify(dbConnector =>
                dbConnector.CallStoredProcedure(expected, new System.Data.IDataParameter[] { null }));
        }
    }
}
