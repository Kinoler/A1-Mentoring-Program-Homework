using System;
using System.Collections.Generic;
using System.Linq;
using ADO.DbConnectors;
using ADO.Interfaces;
using ADO.Models;

namespace ADO.RepositoriesImp
{
    public class NorthwindCategory : INorthwindTable<Category>
    {
        private readonly IDbConnector _dbHelper;

        internal const string SelectAllQuery =
            @"SELECT * FROM Categories;";

        internal const string SelectOneQuery =
            @"SELECT * FROM Categories WHERE CategoryID = {0};";

        public NorthwindCategory(IDbConnector dbHelper)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
        }

        public void Add(Category item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category item)
        {
            throw new NotImplementedException();
        }

        public Category GetElement(int id)
        {
            var query = string.Format(SelectOneQuery, id);
            return FillDependences(
                _dbHelper
                    .GetDataTable(query).Select()
                    .First().ToObject<Category>());
        }

        public IEnumerable<Category> GetElements()
        {
            return _dbHelper
                .GetDataTable(SelectAllQuery).Select()
                .Select(dataRow => FillDependences(dataRow.ToObject<Category>()));
        }

        public void Update(Category item)
        {
            throw new NotImplementedException();
        }

        public Category FillDependences(Category category)
        {

            return category;
        }
    }
}
