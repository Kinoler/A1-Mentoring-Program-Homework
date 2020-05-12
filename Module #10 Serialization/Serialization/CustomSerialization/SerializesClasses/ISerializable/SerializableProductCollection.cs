using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.SerializesClasses
{
    [Serializable]
    public class SerializableProductCollection : ISerializable, IEnumerable<Product>
    {
        private readonly IEnumerable<Product> _products;

        public SerializableProductCollection()
        {
        }

        public SerializableProductCollection(SerializationInfo info, StreamingContext context)
        {
            _products = ((Northwind)context.Context).Products;

            foreach (var product in _products)
            {
                var objectContext = ((IObjectContextAdapter) ((Northwind)context.Context)).ObjectContext;
                objectContext.LoadProperty(product, prod => prod.Order_Details);
                objectContext.LoadProperty(product, prod => prod.Category);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Products", _products);
        }

        public IEnumerator<Product> GetEnumerator()
        {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
