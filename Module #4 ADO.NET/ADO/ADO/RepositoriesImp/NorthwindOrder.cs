using ADO.DbConnectors;
using ADO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.Interfaces;

namespace ADO.RepositoriesImp
{
    public class NorthwindOrder : INorthwindTable<Order>
    {
        private readonly IDbConnector _dbHelper;
        private readonly INorthwindTable<OrderDetail> _northwindOrderDetail;

        public const string AddQuery =
            @"INSERT INTO Orders 
            VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12});";

        public const string SelectAllQuery =
            @"SELECT * FROM Orders;";

        public const string SelectOneQuery =
            @"SELECT * FROM Orders WHERE OrderID = {0};";

        public const string DeleteOneQuery =
            @"DELETE FROM Orders WHERE OrderID = {0};";

        public const string UpdateQuery =
            @"UPDATE * FROM Orders
            SET 
            CustomerID = {1},
            EmployeeID = {2},
            OrderDate = {3},
            RequiredDate = {4},
            ShippedDate = {5},
            ShipVia = {6},
            Freight = {7},
            ShipName = {8},
            ShipAddress = {9},
            ShipCity = {10},
            ShipRegion = {11},
            ShipPostalCode = {12},
            ShipCountry = {13}
            WHERE OrderID = {0};";

        public NorthwindOrder(IDbConnector dbHelper, INorthwindTable<OrderDetail> northwindOrderDetail = null)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _northwindOrderDetail = northwindOrderDetail ?? new NorthwindOrderDetail(dbHelper);
        }

        public void Add(Order order)
        {
            var query = string.Format(
                AddQuery,
                order.CustomerID.ConvertToDbParameter(),
                order.EmployeeID.ConvertToDbParameter(),
                order.OrderDate.ConvertToDbParameter(),
                order.RequiredDate.ConvertToDbParameter(),
                order.ShippedDate.ConvertToDbParameter(),
                order.ShipVia.ConvertToDbParameter(),
                order.Freight.ConvertToDbParameter(),
                order.ShipName.ConvertToDbParameter(),
                order.ShipAddress.ConvertToDbParameter(),
                order.ShipCity.ConvertToDbParameter(),
                order.ShipRegion.ConvertToDbParameter(),
                order.ShipPostalCode.ConvertToDbParameter(),
                order.ShipCountry.ConvertToDbParameter());

            _dbHelper.ExecuteNonQuery(query);
        }

        public void Delete(Order order)
        {
            if (order.Status == OrderState.Complete)
                throw new ArgumentException($"The order with status {OrderState.Complete}, can not be deleted", nameof(order));

            var query = string.Format(DeleteOneQuery, order.OrderID);
            _dbHelper.ExecuteNonQuery(query);
        }

        public Order GetElement(int id)
        {
            var query = string.Format(SelectOneQuery, id);
            return FillDependences(
                _dbHelper
                    .GetDataTable(query).Select()
                    .FirstOrDefault().ToObject<Order>());
        }

        public IEnumerable<Order> GetElements()
        {
            return _dbHelper
                .GetDataTable(SelectAllQuery)?.Select()?
                .Select(dataRow => FillDependences(dataRow.ToObject<Order>()));
        }

        public void Update(Order order)
        {
            if (order.Status != OrderState.New)
                throw new ArgumentException($"The state of order should be {OrderState.New} to update", nameof(order));

            InternalUpdate(order);
        }

        internal void InternalUpdate(Order order)
        {
            var query = string.Format(
                UpdateQuery,
                order.OrderID.ConvertToDbParameter(),
                order.CustomerID.ConvertToDbParameter(),
                order.EmployeeID.ConvertToDbParameter(),
                order.OrderDate.ConvertToDbParameter(),
                order.RequiredDate.ConvertToDbParameter(),
                order.ShippedDate.ConvertToDbParameter(),
                order.ShipVia.ConvertToDbParameter(),
                order.Freight.ConvertToDbParameter(),
                order.ShipName.ConvertToDbParameter(),
                order.ShipAddress.ConvertToDbParameter(),
                order.ShipCity.ConvertToDbParameter(),
                order.ShipRegion.ConvertToDbParameter(),
                order.ShipPostalCode.ConvertToDbParameter(),
                order.ShipCountry.ConvertToDbParameter());

            _dbHelper.ExecuteNonQuery(query);
        }

        public Order FillDependences(Order order)
        {
            order.OrderDetails = _northwindOrderDetail
                .GetElements()?
                .Where(orderDetail => orderDetail.OrderID == order.OrderID)
                .ToList();

            return order;
        }
    }
}
