using ADO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ADO
{
	public class Northwind
	{
		private DbHelper dbHelper;

		public Northwind(string connectionStringName)
		{
			dbHelper = new DbHelper(connectionStringName);
		}

		public List<Order> Orders => dbHelper.SelectData<Order>(nameof(Orders));

		public List<Product> Products => dbHelper.SelectData<Product>(nameof(Products));

		public List<Region> Region => dbHelper.SelectData<Region>(nameof(Region));

		public List<OrderDetail> OrderDetails => dbHelper.SelectData<OrderDetail>("[Northwind].[dbo].[Order Details]"); 

		public List<Category> Categories => dbHelper.SelectData<Category>(nameof(Categories));

		public Order CreateOrder()
		{
			var order = new Order();
			dbHelper.Insert(order, nameof(Orders));
			return order;
		}

		public void UpdateOrder(Order order)
		{
			if (order == null)
				throw new ArgumentNullException(nameof(order));

			if (order.State() != OrderState.New)
				throw new ArgumentException($"The state of order should be {OrderState.New} to update", nameof(order));

			dbHelper.Update(order, nameof(Orders));
		}

		public void DeleteOrder(Order order)
		{
			if (order == null)
				throw new ArgumentNullException(nameof(order));

			if (order.State() == OrderState.Complete)
			{
				throw new ArgumentException($"The order with status {OrderState.Complete}, can not be deleted", nameof(order));
			}

			dbHelper.Delete(order, nameof(Orders));
		}

		public void SetOrderDate(Order order, DateTime orderDate)
		{
			if (order == null)
				throw new ArgumentNullException(nameof(order));
			if (orderDate == null)
				throw new ArgumentNullException(nameof(orderDate));

			order.OrderDate = orderDate;
			UpdateOrder(order);
		}

		public void SetShippedDate(Order order, DateTime shippedDate)
		{
			if (order == null)
				throw new ArgumentNullException(nameof(order));
			if (shippedDate == null)
				throw new ArgumentNullException(nameof(shippedDate));

			order.ShippedDate = shippedDate;
			UpdateOrder(order);
		}

		public List<CustOrderHist_Result> CustOrderHis(string CustomerId)
		{
			var customerIdParam = dbHelper.CreateParameter($"@{nameof(CustomerId)}", CustomerId, System.Data.DbType.String);
			return dbHelper.CallStoredProcedure<CustOrderHist_Result>("CustOrderHis", customerIdParam);
		}

		public List<CustOrdersDetail_Result> CustOrderHis(int OrderID)
		{
			var orderIdParam = dbHelper.CreateParameter($"@{nameof(OrderID)}", OrderID, System.Data.DbType.Int32);
			return dbHelper.CallStoredProcedure<CustOrdersDetail_Result>("CustOrdersDetail", orderIdParam);
		}
	}
}
