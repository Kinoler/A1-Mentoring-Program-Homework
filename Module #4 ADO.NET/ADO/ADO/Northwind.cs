using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.DbConnectors;
using ADO.Interfaces;
using ADO.Models;
using ADO.RepositoriesImp;

namespace ADO
{
    public class Northwind
    {
        private readonly IDbConnector _dbConnetor;

        public Northwind(string connectionStringName) : this(new DbConnetor(connectionStringName))
        {
        }

        public Northwind(IDbConnector dbConnetor)
        {
            _dbConnetor = dbConnetor ?? throw new ArgumentNullException(nameof(dbConnetor));
            Orders = new NorthwindOrder(_dbConnetor);
        }

        public INorthwindTable<Order> Orders { get; private set; }

        public List<CustOrderHist_Result> CustOrderHis(string CustomerId)
        {
            var customerIdParam = _dbConnetor.CreateParameter(
                $"@{nameof(CustomerId)}", 
                0, 
                CustomerId, 
                System.Data.DbType.String, 
                System.Data.ParameterDirection.Input);

            return _dbConnetor
                .CallStoredProcedure("CustOrderHis", customerIdParam)?.Select()
                .Select(dataRow => dataRow.ToObject<CustOrderHist_Result>())
                .ToList();
        }

        public List<CustOrdersDetail_Result> CustOrdersDetail(int OrderID)
        {
            var orderIdParam = _dbConnetor.CreateParameter(
                $"@{nameof(OrderID)}", 
                0, 
                OrderID, 
                System.Data.DbType.Int32,
                System.Data.ParameterDirection.Input);

            return _dbConnetor
                .CallStoredProcedure("CustOrdersDetail", orderIdParam)?.Select()
                .Select(dataRow => dataRow.ToObject<CustOrdersDetail_Result>())
                .ToList();
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.New)
                throw new ArgumentException($"The order should be {OrderState.New}", nameof(order));

            order.OrderDate = orderDate;
            (Orders as NorthwindOrder)?.InternalUpdate(order);
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.Status != OrderState.InProgress)
                throw new ArgumentException($"The order should be {OrderState.InProgress}", nameof(order));

            order.ShippedDate = shippedDate;
            (Orders as NorthwindOrder)?.InternalUpdate(order);
        }
    }
}
