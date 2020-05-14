using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.SerializesClasses
{
    public class SurrogateSelectorOrderDetailsCollection : ISerializationSurrogate, ISurrogateSelector
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var orderDetails = (IEnumerable<Order_Detail>)obj;
            foreach (var orderDetail in orderDetails)
            {
                var objectContext = (ObjectContext)context.Context;
                objectContext.LoadProperty(orderDetail, prod => prod.Order);
                objectContext.LoadProperty(orderDetail, prod => prod.Product);
            }

            return orderDetails;
        }

        public void ChainSelector(ISurrogateSelector selector)
        {
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            selector = this;
            return this;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return this;
        }
    }
}
