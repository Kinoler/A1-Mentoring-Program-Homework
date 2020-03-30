using System;
using System.Collections.Generic;
using System.Linq;
using ADO.DbConnectors;
using ADO.Models;

namespace ADO.RepositoriesImp
{
    public class NorthwindCategory : INorthwindTable<Category>
    {
        private readonly IDbConnector _dbHelper;
        private readonly INorthwindTable<Product> _northwindProduct;

        internal const string SelectAllQuery =
            @"SELECT * FROM Categories;";

        internal const string SelectOneQuery =
            @"SELECT * FROM Categories WHERE CategoryID = {0};";

        public NorthwindCategory(IDbConnector dbHelper, INorthwindTable<Product> northwindProduct = null)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _northwindProduct = northwindProduct ?? new NorthwindProduct(dbHelper);
        }

        public void Add(Category item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Category GetElement(int id)
        {
            var query = string.Format(SelectOneQuery, id);
            return FillDependences(
                _dbHelper
                    .GetDataSet(query).Tables[0].Select()
                    .First().ToObject<Category>());
        }

        public IEnumerable<Category> GetElements()
        {
            return _dbHelper
                .GetDataSet(SelectAllQuery).Tables[0].Select()
                .Select(dataRow => FillDependences(dataRow.ToObject<Category>()));
        }

        public void Update(Category item)
        {
            throw new NotImplementedException();
        }

        public Category FillDependences(Category category)
        {
            category.Products = _northwindProduct
                .GetElements()
                .Where(product => product.CategoryID == category.CategoryID)
                .ToList();

            return category;
        }
    }
}
