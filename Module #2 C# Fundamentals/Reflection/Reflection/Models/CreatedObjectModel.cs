using System;
using System.Linq;
using System.Reflection;
using Reflection.Attributes;

namespace Reflection.Models
{
    public class CreatedObjectModel
    {
        public bool IsExport => Type.GetCustomAttributes().Any(at => at.GetType() == typeof(ExportAttribute));
        public bool IsConstructorImport => Type.GetCustomAttributes().Any(at => at.GetType() == typeof(ImportConstructorAttribute));
        public bool IsPropertyImport => Properties.Any(prop => prop.GetCustomAttributes().Any(at => at.GetType() == typeof(ImportAttribute)));

        public object Instance { get; set; }

        public Type Type { get; set; }

        public ConstructorInfo Constructor => Type.GetConstructors().ToList().FirstOrDefault();

        public ParameterInfo[] ConstructorParameters => Constructor?.GetParameters() ?? new ParameterInfo[0];

        public PropertyInfo[] Properties => Type?.GetProperties() ?? new PropertyInfo[0];

        public CreatedObjectModel(Type type)
        {
            Type = type;
        }
    }
}
