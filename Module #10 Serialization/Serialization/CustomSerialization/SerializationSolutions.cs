using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Task.SerializesClasses;

namespace Task
{
	[TestClass]
	public class SerializationSolutions
	{
		Northwind dbContext;

		[TestInitialize]
		public void Initialize()
		{
			dbContext = new Northwind();
		}

		[TestMethod]
		public void SerializationCallbacks()
		{
            // Arrange
            dbContext.Configuration.ProxyCreationEnabled = false;

            var streamingContext = new StreamingContext(StreamingContextStates.All, dbContext);
            var tester =
                new XmlDataContractSerializerTester<NetDataContractWithCallback>(
                    new NetDataContractSerializer(streamingContext),
                    true);

			var categories = dbContext.Categories.ToList();
            var netDataContractWithCallback = new NetDataContractWithCallback(categories);

			// Act
			var deserializedCategories = tester.SerializeAndDeserialize(netDataContractWithCallback);

            // Assert
            var actual = deserializedCategories.All(categ => categ.Products.Any());
            Assert.IsTrue(actual);
        }

		[TestMethod]
		public void ISerializable()
		{
            // Arrange
			dbContext.Configuration.ProxyCreationEnabled = false;

            var serializationContext =
                new SerializationContext(((IObjectContextAdapter)dbContext).ObjectContext);

            var netDataContractSerializer = new NetDataContractSerializer
            {
                Binder = new NetDataContractWithSerializationBinder(),
				Context = new StreamingContext(StreamingContextStates.All, serializationContext)
            };

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(netDataContractSerializer, true);
			var products = dbContext.Products.ToList();

            // Act
            var deserializedCategories = tester.SerializeAndDeserialize(products);

            // Assert
            var actual = deserializedCategories.All(prod => prod.Order_Details.Any());
            Assert.IsTrue(actual);

            actual = deserializedCategories.All(prod => prod.Category != null);
            Assert.IsTrue(actual);
		}


		[TestMethod]
		public void ISerializationSurrogate()
		{
            // Arrange
            dbContext.Configuration.ProxyCreationEnabled = false;

            var serializationContext = 
                    new SerializationContext(((IObjectContextAdapter)dbContext).ObjectContext);

            var serializer = new NetDataContractSerializer
            {
                SurrogateSelector = new OrderDetailSurrogateSelector(),
                Context = new StreamingContext(StreamingContextStates.All, serializationContext)
            };

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(serializer, true);
			var orderDetails = dbContext.Order_Details.ToList();

            // Act
            var deserializedOrderDetails = tester.SerializeAndDeserialize(orderDetails);

            // Assert
            var actual = deserializedOrderDetails.All(prod => prod.Order != null);
            Assert.IsTrue(actual);

            actual = deserializedOrderDetails.All(prod => prod.Product != null);
            Assert.IsTrue(actual);
        }

		[TestMethod]
		public void IDataContractSurrogate()
		{
            // Arrange
            dbContext.Configuration.ProxyCreationEnabled = true;
			dbContext.Configuration.LazyLoadingEnabled = true;

            var serializer = new DataContractSerializer(typeof(IEnumerable<Order>), new DataContractSerializerSettings()
            {
                DataContractSurrogate = new OrderDataContractSurrogate()
            });

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(serializer, true);
			var orders = dbContext.Orders.ToList();

			tester.SerializeAndDeserialize(orders);

            // Act
            var deserializedOrders = tester.SerializeAndDeserialize(orders);

            // Assert
            var actual = deserializedOrders.All(prod => prod.Customer != null);
            Assert.IsTrue(actual);

            actual = deserializedOrders.All(prod => prod.Employee != null);
            Assert.IsTrue(actual);

            actual = deserializedOrders.All(prod => prod.Shipper != null);
            Assert.IsTrue(actual);

            actual = deserializedOrders.All(prod => prod.Order_Details.Any());
            Assert.IsTrue(actual);
        }
	}
}
