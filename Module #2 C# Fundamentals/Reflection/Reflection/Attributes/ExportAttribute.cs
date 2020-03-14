using System;

namespace Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        internal Type _interfaceType;

        public bool IsInterfaceInitializer => _interfaceType != null;

        public ExportAttribute()
        {
        }

        public ExportAttribute(Type interfaceType)
        {
            _interfaceType = interfaceType;
        }
    }
}
