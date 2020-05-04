using System;
using System.Runtime.Serialization;

namespace Task.SerializesClasses
{
    public class NetDataContractWithSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return typeof(NetDataContractWithISerializable);
        }
    }
}
