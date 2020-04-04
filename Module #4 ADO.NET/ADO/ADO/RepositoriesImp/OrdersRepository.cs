using ADO.DbConnectors;
using ADO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Extensions;
using ADO.Interfaces;
using ADO.Quaries;

namespace ADO.RepositoriesImp
{
    internal class OrdersRepository : IOrdersRepository
    {
        private readonly IDbConnector _dbHelper;
        private readonly OrderQuary _orderQuary;

        public OrdersRepository(IDbConnector dbConnetor) : 
            this(dbConnetor, OrderQuary.OrderQuaryFactory(dbConnetor.ProviderName))
        {
        }

        public OrdersRepository(IDbConnector dbHelper, OrderQuary orderQuary)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _orderQuary = orderQuary ?? throw new ArgumentNullException(nameof(orderQuary));
        }

        public IEnumerable<Order> GetOrders(int windowCapacity, int windowNumber)
        {
            var offset = _dbHelper.CreateParameter("@Offset", windowCapacity * windowNumber);
            var count = _dbHelper.CreateParameter("@Count", windowNumber);
            var dataSet = _dbHelper.GetDataSet(_orderQuary.SelectAllQuery, offset, count);

            var dataRows = dataSet?.Tables[0].Select();
            return dataRows?.Select(dataRow => dataRow.ToObject<Order>());
        }

        public Order GetOrder(int id)
        {
            var orderId = _dbHelper.CreateParameter("@OrderId", id);
            var dataSet = _dbHelper.GetDataSet(_orderQuary.SelectOneQuery, orderId);

            var dataRow = dataSet.Tables[0].Select().FirstOrDefault();
            return dataRow?.ToObject<Order>();
        }

        public DetailOfOrder[] GetDetailOfOrder(int id)
        {
            var orderId = _dbHelper.CreateParameter("@OrderId", id);
            var dataSet = _dbHelper.GetDataSet(_orderQuary.SelectOneDetailQuery, orderId);

            var dataRows = dataSet.Tables[0].Select();

            var details = dataRows.Select(dataRow => dataRow.ToObject<DetailOfOrder>()).ToList();
            details.ForEach(detail => detail.Picture = detail.Picture.Skip(78).ToArray());

            return details.ToArray();
        }

        public void Add(Order order)
        {
            var parameters = new IDataParameter[13];
            parameters[0] = _dbHelper.CreateParameter("@CustomerID", order.CustomerID);
            parameters[1] = _dbHelper.CreateParameter("@EmployeeID", order.EmployeeID);
            parameters[2] = _dbHelper.CreateParameter("@OrderDate", order.OrderDate);
            parameters[3] = _dbHelper.CreateParameter("@RequiredDate", order.RequiredDate);
            parameters[4] = _dbHelper.CreateParameter("@ShippedDate", order.ShippedDate);
            parameters[5] = _dbHelper.CreateParameter("@ShipVia", order.ShipVia);
            parameters[6] = _dbHelper.CreateParameter("@Freight", order.Freight);
            parameters[7] = _dbHelper.CreateParameter("@ShipName", order.ShipName);
            parameters[8] = _dbHelper.CreateParameter("@ShipAddress", order.ShipAddress);
            parameters[9] = _dbHelper.CreateParameter("@ShipCity", order.ShipCity);
            parameters[10] = _dbHelper.CreateParameter("@ShipRegion", order.ShipRegion);
            parameters[11] = _dbHelper.CreateParameter("@ShipPostalCode", order.ShipPostalCode);
            parameters[12] = _dbHelper.CreateParameter("@ShipCountry", order.ShipCountry);

            _dbHelper.ExecuteNonQuery(_orderQuary.AddQuery, parameters);
        }

        public void Delete(Order order)
        {
            if (order.Status == OrderState.Complete)
                throw new InvalidOperationException(
                    $"The {nameof(order)} with status {OrderState.Complete}, can not be deleted");

            var orderId = _dbHelper.CreateParameter("@OrderId", order.OrderID);
            _dbHelper.ExecuteNonQuery(_orderQuary.DeleteOneQuery, orderId);
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.New)
                throw new ArgumentException($"The order should be {OrderState.New}", nameof(order));

            order.OrderDate = orderDate;
            InternalUpdate(order);
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.InProgress)
                throw new ArgumentException($"The order should be {OrderState.InProgress}", nameof(order));

            order.ShippedDate = shippedDate;
            InternalUpdate(order);
        }

        public void Update(Order order)
        {
            if (order.Status != OrderState.New)
                throw new InvalidOperationException(
                    $"The state of {nameof(order)} should be {OrderState.New} to update");

            InternalUpdate(order);
        }

        internal void InternalUpdate(Order order)
        {
            var parameters = new IDataParameter[14];
            parameters[0] = _dbHelper.CreateParameter("@CustomerID", order.CustomerID);
            parameters[1] = _dbHelper.CreateParameter("@EmployeeID", order.EmployeeID);
            parameters[2] = _dbHelper.CreateParameter("@OrderDate", order.OrderDate);
            parameters[3] = _dbHelper.CreateParameter("@RequiredDate", order.RequiredDate);
            parameters[4] = _dbHelper.CreateParameter("@ShippedDate", order.ShippedDate);
            parameters[5] = _dbHelper.CreateParameter("@ShipVia", order.ShipVia);
            parameters[6] = _dbHelper.CreateParameter("@Freight", order.Freight);
            parameters[7] = _dbHelper.CreateParameter("@ShipName", order.ShipName);
            parameters[8] = _dbHelper.CreateParameter("@ShipAddress", order.ShipAddress);
            parameters[9] = _dbHelper.CreateParameter("@ShipCity", order.ShipCity);
            parameters[10] = _dbHelper.CreateParameter("@ShipRegion", order.ShipRegion);
            parameters[11] = _dbHelper.CreateParameter("@ShipPostalCode", order.ShipPostalCode);
            parameters[12] = _dbHelper.CreateParameter("@ShipCountry", order.ShipCountry);
            parameters[13] = _dbHelper.CreateParameter("@OrderID", order.OrderID);

            _dbHelper.ExecuteNonQuery(_orderQuary.UpdateQuery, parameters);
        }
    }
}
