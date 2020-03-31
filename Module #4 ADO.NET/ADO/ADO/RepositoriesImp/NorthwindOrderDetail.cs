﻿using System;
using System.Collections.Generic;
using System.Linq;
using ADO.DbConnectors;
using ADO.Interfaces;
using ADO.Models;

namespace ADO.RepositoriesImp
{
    public class NorthwindOrderDetail : INorthwindTable<OrderDetail>
    {
        private readonly IDbConnector _dbHelper;
        private readonly INorthwindTable<Product> _northwindProduct;

        internal const string SelectAllQuery =
            @"SELECT * FROM [Northwind].[dbo].[Order Details];";

        public NorthwindOrderDetail(IDbConnector dbHelper, INorthwindTable<Product> northwindProduct = null)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _northwindProduct = northwindProduct ?? new NorthwindProduct(dbHelper);
        }

        public void Add(OrderDetail item)
        {
            throw new NotImplementedException();
        }

        public void Delete(OrderDetail item)
        {
            throw new NotImplementedException();
        }

        public OrderDetail GetElement(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderDetail> GetElements()
        {
            return _dbHelper
                .GetDataTable(SelectAllQuery).Select()
                .Select(dataRow => FillDependences(dataRow.ToObject<OrderDetail>()));
        }

        public void Update(OrderDetail item)
        {
            throw new NotImplementedException();
        }

        public OrderDetail FillDependences(OrderDetail order)
        {
            order.Product = _northwindProduct
                .GetElement(order.ProductID);

            return order;
        }
    }
}
