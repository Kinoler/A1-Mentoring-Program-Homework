using System;
using System.Runtime.Serialization;

namespace Task.SerializesClasses
{
    public class BinderProductCollection : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return typeof(SerializableProductCollection);
        }
    }
}
