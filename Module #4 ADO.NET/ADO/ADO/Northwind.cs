using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO.DbConnectors;
using ADO.Extensions;
using ADO.Interfaces;
using ADO.Models;
using ADO.Quaries;
using ADO.RepositoriesImp;

namespace ADO
{
    public class Northwind
    {
        private readonly IDbConnector _dbConnetor;

        public Northwind(string connectionStringName) : this(new DbConnetor(connectionStringName))
        {
        }

        internal Northwind(IDbConnector dbConnetor)
        {
            _dbConnetor = dbConnetor ?? throw new ArgumentNullException(nameof(dbConnetor));
            Orders = new OrdersRepository(_dbConnetor);
        }

        public IOrdersRepository Orders { get; private set; }

        public IEnumerable<CustOrderHist_Result> CustOrderHis(string CustomerId)
        {
            var customerIdParam = _dbConnetor.CreateParameter($"@CustomerId", CustomerId);

            return _dbConnetor
                .CallStoredProcedure("CustOrderHis", customerIdParam)?.Select()
                .Select(dataRow => dataRow.ToObject<CustOrderHist_Result>());
        }

        public IEnumerable<CustOrdersDetail_Result> CustOrdersDetail(int orderID)
        {
            var orderIdParam = _dbConnetor.CreateParameter($"@OrderID", orderID);

            return _dbConnetor
                .CallStoredProcedure("CustOrdersDetail", orderIdParam)?.Select()
                .Select(dataRow => dataRow.ToObject<CustOrdersDetail_Result>());
        }
    }
}
