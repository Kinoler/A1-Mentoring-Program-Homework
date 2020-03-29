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

		public List<Order> Orders => dbHelper.SelectData<Order>();

		public List<Product> Products => dbHelper.SelectData<Product>();

		public List<Region> Regions => dbHelper.SelectData<Region>();

		public List<OrderDetail> OrderDetails => dbHelper.SelectData<OrderDetail>(); 

		public List<Category> Category => dbHelper.SelectData<Category>();

		public void CreateOrder(Order order)
		{
			dbHelper.Insert(order);
		}

		public void UpdateOrder(Order order)
		{
			dbHelper.Update(order);
		}

		public void DeleteOrder(Order order)
		{
			if (order.State() == OrderState.Complete)
			{
				throw new ArgumentException($"The order with status {OrderState.Complete}, can not be deleted", nameof(order));
			}

			dbHelper.Delete(order);
		}

		public void SetOrderDate(Order order, DateTime orderDate)
		{
			order.OrderDate = orderDate;
			UpdateOrder(order);
		}

		public void SetShippedDate(Order order, DateTime shippedDate)
		{
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
