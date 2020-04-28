using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using StackExchange.Redis;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Order = NorthwindLibrary.Order;

namespace CachingSolutionsSamples
{
	class OrdersRedisCache : IOrdersCache
	{
		private ConnectionMultiplexer redisConnection;
		string prefix = "Cache_Orders";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Order>));

		public OrdersRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public IEnumerable<Order> Get(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<Order>)serializer
				.ReadObject(new MemoryStream(s));
        }

		public void Set(string forUser, IEnumerable<Order> orders)
		{
			var db = redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (orders == null)
			{
				db.StringSet(key, RedisValue.Null, TimeSpan.FromMinutes(10));
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, orders);
				db.StringSet(key, stream.ToArray(), TimeSpan.FromMinutes(10));
			}
		}
	}
}
