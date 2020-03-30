using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.DbConnectors;
using ADO.Models;
using ADO.RepositoriesImp;

namespace ADO
{
    public class Northwind
    {
        private readonly IDbConnector _dbHelper;

        public Northwind(string connectionStringName)
        {
            _dbHelper = new DbConnetor(connectionStringName);
            Orders = new NorthwindOrder(_dbHelper);
        }

        public INorthwindTable<Order> Orders { get; private set; }

        public List<CustOrderHist_Result> CustOrderHis(string CustomerId)
        {
            var customerIdParam = _dbHelper.CreateParameter(
                $"@{nameof(CustomerId)}", 
                0, 
                CustomerId, 
                System.Data.DbType.String, 
                System.Data.ParameterDirection.Input);

            return _dbHelper
                .CallStoredProcedure("CustOrderHis", customerIdParam)
                .ToObjects<CustOrderHist_Result>();
        }

        public List<CustOrdersDetail_Result> CustOrderHis(int OrderID)
        {
            var orderIdParam = _dbHelper.CreateParameter(
                $"@{nameof(OrderID)}", 
                0, 
                OrderID, 
                System.Data.DbType.Int32,
                System.Data.ParameterDirection.Input);

            return _dbHelper
                .CallStoredProcedure("CustOrdersDetail", orderIdParam)
                .ToObjects<CustOrdersDetail_Result>();
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.New)
                throw new ArgumentException($"The order should be {OrderState.New}", nameof(order));

            order.OrderDate = orderDate;
            Orders.Update(order);
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.InProgress)
                throw new ArgumentException($"The order should be {OrderState.InProgress}", nameof(order));

            order.ShippedDate = shippedDate;
            Orders.Update(order);
        }
    }
}
