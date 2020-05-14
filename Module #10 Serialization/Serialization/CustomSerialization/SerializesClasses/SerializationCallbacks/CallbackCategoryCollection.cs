using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.SerializesClasses
{
    [Serializable]
    public class CallbackCategoryCollection : IEnumerable<Category>
    {
        private readonly IEnumerable<Category> _categories;

        public CallbackCategoryCollection(IEnumerable<Category> categories)
        {
            _categories = categories;
        }

        [OnSerializing()]
        internal void OnSerializing(StreamingContext context)
        {
            foreach (var category in _categories)
            {
                var objectContext = (ObjectContext)context.Context;
                objectContext.LoadProperty(category, prod => prod.Products);
            }
        }

        public IEnumerator<Category> GetEnumerator()
        {
            return _categories.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
