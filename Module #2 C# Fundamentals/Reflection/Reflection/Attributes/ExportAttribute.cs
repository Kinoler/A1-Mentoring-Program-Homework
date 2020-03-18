using System;

namespace Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        internal Type InterfaceType { get; }

        internal bool IsInterfaceInitializer => InterfaceType != null;

        public ExportAttribute()
        {
        }

        public ExportAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}
