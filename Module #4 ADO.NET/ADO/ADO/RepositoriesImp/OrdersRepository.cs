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
        private readonly IDbConnector _dbConnector;
        private readonly OrderQuary _orderQuary;

        public OrdersRepository(IDbConnector dbConnetor) : 
            this(dbConnetor, OrderQuary.OrderQuaryFactory(dbConnetor.ProviderName) ?? throw new ArgumentNullException(""))
        {
        }

        public OrdersRepository(IDbConnector dbConnector, OrderQuary orderQuary)
        {
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
            _orderQuary = orderQuary ?? throw new ArgumentNullException(nameof(orderQuary));
        }

        public IEnumerable<Order> GetOrders(int windowCapacity, int windowNumber)
        {
            var offset = _dbConnector.CreateParameter("@Offset", windowCapacity * windowNumber);
            var count = _dbConnector.CreateParameter("@Count", windowNumber);
            var dataSet = _dbConnector.GetDataSet(_orderQuary.SelectAllQuery, offset, count);

            var dataRows = dataSet?.Tables[0].Select();
            return dataRows?.Select(dataRow => dataRow.ToObject<Order>());
        }

        public Order GetOrder(int id)
        {
            var orderId = _dbConnector.CreateParameter("@OrderId", id);
            var dataSet = _dbConnector.GetDataSet(_orderQuary.SelectOneQuery, orderId);

            var dataRow = dataSet.Tables[0].Select().FirstOrDefault();
            return dataRow?.ToObject<Order>();
        }

        public IEnumerable<DetailOfOrder> GetDetailOfOrder(int id)
        {
            var orderId = _dbConnector.CreateParameter("@OrderId", id);
            var dataSet = _dbConnector.GetDataSet(_orderQuary.SelectOneDetailQuery, orderId);

            var dataRows = dataSet.Tables[0].Select();

            var details = dataRows.Select(dataRow => dataRow.ToObject<DetailOfOrder>()).ToList();
            details.ForEach(detail => detail.Picture = detail.Picture.Skip(78).ToArray());

            return details.ToArray();
        }

        public void Add(Order order)
        {
            var parameters = new IDataParameter[]
            {
                _dbConnector.CreateParameter("@CustomerID", order.CustomerID),
                _dbConnector.CreateParameter("@EmployeeID", order.EmployeeID),
                _dbConnector.CreateParameter("@OrderDate", order.OrderDate),
                _dbConnector.CreateParameter("@RequiredDate", order.RequiredDate),
                _dbConnector.CreateParameter("@ShippedDate", order.ShippedDate),
                _dbConnector.CreateParameter("@ShipVia", order.ShipVia),
                _dbConnector.CreateParameter("@Freight", order.Freight),
                _dbConnector.CreateParameter("@ShipName", order.ShipName),
                _dbConnector.CreateParameter("@ShipAddress", order.ShipAddress),
                _dbConnector.CreateParameter("@ShipCity", order.ShipCity),
                _dbConnector.CreateParameter("@ShipRegion", order.ShipRegion),
                _dbConnector.CreateParameter("@ShipPostalCode", order.ShipPostalCode),
                _dbConnector.CreateParameter("@ShipCountry", order.ShipCountry)
            };

        _dbConnector.ExecuteNonQuery(_orderQuary.AddQuery, parameters);
        }

        public void Delete(Order order)
        {
            if (order.Status == OrderState.Complete)
                throw new InvalidOperationException(
                    $"The {nameof(order)} with status {OrderState.Complete}, can not be deleted");

            var orderId = _dbConnector.CreateParameter("@OrderId", order.OrderID);
            _dbConnector.ExecuteNonQuery(_orderQuary.DeleteOneQuery, orderId);
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.New)
                throw new InvalidOperationException(
                    $"The {nameof(order)} should be {OrderState.New}");

            order.OrderDate = orderDate;
            InternalUpdate(order);
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.InProgress)
                throw new InvalidOperationException(
                    $"The {nameof(order)} should be {OrderState.InProgress}");

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
            var parameters = new IDataParameter[]
            {
                _dbConnector.CreateParameter("@CustomerID", order.CustomerID),
                _dbConnector.CreateParameter("@EmployeeID", order.EmployeeID),
                _dbConnector.CreateParameter("@OrderDate", order.OrderDate),
                _dbConnector.CreateParameter("@RequiredDate", order.RequiredDate),
                _dbConnector.CreateParameter("@ShippedDate", order.ShippedDate),
                _dbConnector.CreateParameter("@ShipVia", order.ShipVia),
                _dbConnector.CreateParameter("@Freight", order.Freight),
                _dbConnector.CreateParameter("@ShipName", order.ShipName),
                _dbConnector.CreateParameter("@ShipAddress", order.ShipAddress),
                _dbConnector.CreateParameter("@ShipCity", order.ShipCity),
                _dbConnector.CreateParameter("@ShipRegion", order.ShipRegion),
                _dbConnector.CreateParameter("@ShipPostalCode", order.ShipPostalCode),
                _dbConnector.CreateParameter("@ShipCountry", order.ShipCountry),
                _dbConnector.CreateParameter("@OrderID", order.OrderID)
            };

        _dbConnector.ExecuteNonQuery(_orderQuary.UpdateQuery, parameters);
        }
    }
}
