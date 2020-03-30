using System;
using System.Collections.Generic;
using System.Linq;
using ADO.DbConnectors;
using ADO.Models;

namespace ADO.RepositoriesImp
{
    public class NorthwindProduct : INorthwindTable<Product>
    {
        private readonly IDbConnector _dbHelper;
        private readonly INorthwindTable<OrderDetail> _northwindOrderDetail;
        private readonly INorthwindTable<Category> _northwindCategory;

        internal const string SelectAllQuery =
            @"SELECT * FROM Products;";

        internal const string SelectOneQuery =
            @"SELECT * FROM Products WHERE ProductID = {0};";


        public NorthwindProduct(IDbConnector dbHelper, INorthwindTable<OrderDetail> northwindOrderDetail = null, INorthwindTable<Category> northwindCategory = null)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _northwindOrderDetail = northwindOrderDetail ?? new NorthwindOrderDetail(dbHelper);
            _northwindCategory = northwindCategory ?? new NorthwindCategory(dbHelper);
        }

        public void Add(Product item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetElement(int id)
        {
            var query = string.Format(SelectOneQuery, id);
            return FillDependences(
                _dbHelper
                    .GetDataSet(query).Tables[0].Select()
                    .First().ToObject<Product>());
        }

        public IEnumerable<Product> GetElements()
        {
            return _dbHelper
                .GetDataSet(SelectAllQuery).Tables[0].Select()
                .Select(dataRow => FillDependences(dataRow.ToObject<Product>()));
        }

        public void Update(Product item)
        {
            throw new NotImplementedException();
        }

        public Product FillDependences(Product product)
        {
            product.OrderDetails = _northwindOrderDetail
                .GetElements()
                .Where(orderDetail => orderDetail.ProductID == product.ProductID)
                .ToList();

            if (product.CategoryID.HasValue)
            {
                product.Category = _northwindCategory
                    .GetElement(product.CategoryID.Value);
            }

            return product;
        }
    }
}
