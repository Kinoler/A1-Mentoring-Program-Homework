using System;
using System.Linq;
using System.Reflection;
using Reflection.Attributes;

namespace Reflection.Models
{
    internal class ModelObjectCreated
    {
        public bool IsExport => 
            Type.GetCustomAttributes().Any(at => at.GetType() == typeof(ExportAttribute));
        public bool IsConstructorImport => 
            Type.GetCustomAttributes().Any(at => at.GetType() == typeof(ImportConstructorAttribute));
        public bool IsPropertyImport => 
            Properties.Any(prop => prop.GetCustomAttributes().Any(at => at.GetType() == typeof(ImportAttribute)));

        public object Instance { get; set; }

        public Type Type { get; }

        public ConstructorInfo Constructor => Type.GetConstructors().ToList().FirstOrDefault();

        public ParameterInfo[] ConstructorParameters => Constructor?.GetParameters() ?? new ParameterInfo[0];

        public PropertyInfo[] Properties => Type?.GetProperties() ?? new PropertyInfo[0];

        public PropertyInfo[] ImportedProperties => Type.GetProperties()
            .Where(prop => prop.GetCustomAttributes().Any(attr => attr.GetType() == typeof(ImportAttribute))).ToArray();

        public ModelObjectCreated(Type type)
        {
            Type = type;
        }
    }
}
