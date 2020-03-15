using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Reflection.Attributes;
using Reflection.Models;

namespace Reflection
{
    public class Container
    {
        private readonly List<Type> _importClasses;
        private readonly List<Type> _exportClasses;
        private readonly Dictionary<Type, Type> _exportInterfaces;

        public Container()
        {
            _importClasses = new List<Type>();
            _exportClasses = new List<Type>();
            _exportInterfaces = new Dictionary<Type, Type>();
        }

        public void AddType(Type classType, Type interfaceType)
        {
            if (classType == null)
                throw new ArgumentNullException($"The {nameof(classType)} parameter can not be null");
            if (interfaceType == null)
                throw new ArgumentNullException($"The {nameof(interfaceType)} parameter can not be null");
            if (!classType.IsClass)
                throw new ArgumentException($"The {nameof(classType)} parameter should be class");
            if (!interfaceType.IsInterface)
                throw new ArgumentException($"The {nameof(interfaceType)} parameter should be interface");

            CreatedObjectModel createdObject = new CreatedObjectModel(classType);
            if (!createdObject.IsExport)
                throw new ArgumentException($"The {nameof(classType)} parameter should be " +
                                            $"have the {nameof(ExportAttribute)} attribute");

            ExportAttribute exportAttribute = createdObject.Type.GetCustomAttribute<ExportAttribute>();
            if (!exportAttribute.IsInterfaceInitializer)
                throw new ArgumentException($"The {nameof(classType)} parameter should be " +
                                            $"have {nameof(ExportAttribute)} attribute " +
                                            $"with the {nameof(interfaceType)} parameter.");

            if (!createdObject.Type.GetInterfaces().Any(classInterface => classInterface == interfaceType))
                throw new ArgumentException($"The {nameof(classType)} parameter should implement " +
                                            $"the {nameof(interfaceType)} interface.");

            if (_exportInterfaces.ContainsKey(interfaceType))
                throw new ArgumentException("This interface type is already exist.");

            _exportInterfaces.Add(interfaceType, classType);
        }

        public void AddType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException($"The {nameof(type)} parameter can not be null");

            CreatedObjectModel createdObject = new CreatedObjectModel(type);

            bool hasAnyAttr = false;
            if (createdObject.IsExport)
            {
                if (_exportClasses.Contains(type))
                    throw new ArgumentException("This type is already exist.");

                _exportClasses.Add(type);
                hasAnyAttr = true;
            }

            if (createdObject.IsConstructorImport ^
                createdObject.IsPropertyImport)
            {
                if (_importClasses.Contains(type))
                    throw new ArgumentException("This type is already exist.");

                _importClasses.Add(type);
                hasAnyAttr = true;
            }

            if (!hasAnyAttr)
            {
                throw new ArgumentException($"The {nameof(type)} should be " +
                                            $"have only one of {nameof(ExportAttribute)}, " +
                                            $"{nameof(ImportAttribute)} or " +
                                            $"{nameof(ImportConstructorAttribute)} attributes.");
            }
        }

        public object CreateInstance(Type type)
        {
            if (!_importClasses.Contains(type))
            {
                throw new ArgumentException("This type does not exist in container.");
            }

            var createdObject = new CreatedObjectModel(type);

            createdObject.Instance = CreateInstanceFromConstructor(createdObject);

            if (createdObject.IsConstructorImport)
            {
                return createdObject.Instance;
            }

            if (createdObject.IsPropertyImport)
            {
                SetInstanceForProrerty(createdObject);
            }

            return createdObject.Instance;
        }

        public void SetInstanceForProrerty(CreatedObjectModel createdObject)
        {
            var importedProperties = createdObject.Type
                .GetProperties()
                .Where(prop => prop.GetCustomAttributes()
                    .Any(attr => attr.GetType() == typeof(ImportAttribute)));

            foreach (var importedProperty in importedProperties)
            {
                CreatedObjectModel createdObjectProperty = new CreatedObjectModel(importedProperty.PropertyType);

                if (createdObjectProperty.Type.IsInterface)
                {
                    if (!_exportInterfaces.ContainsKey(createdObjectProperty.Type))
                        throw new ArgumentException($"The {createdObjectProperty.Type.FullName} interface does not found.");

                    createdObjectProperty = new CreatedObjectModel(_exportInterfaces[createdObjectProperty.Type]);
                }
                
                createdObjectProperty.Instance = CreateInstanceFromConstructor(createdObjectProperty);

                importedProperty.SetValue(createdObject.Instance, createdObjectProperty.Instance);
            }
        }

        public object CreateInstanceFromConstructor(CreatedObjectModel createdObject)
        {
            object[] objParameters = new object[createdObject.ConstructorParameters.Length];

            int i = 0;
            foreach (ParameterInfo parameter in createdObject.ConstructorParameters)
            {
                if (parameter.ParameterType.IsInterface)
                {
                    if (!_exportInterfaces.ContainsKey(parameter.ParameterType))
                    {
                        throw new ArgumentException($"The {createdObject.Type.FullName} class should be have" +
                                                    $"implementation for {parameter.ParameterType.FullName} interface");
                    }

                    var createdObjectPrameter = new CreatedObjectModel(_exportInterfaces[parameter.ParameterType]);
                    objParameters[i] = CreateInstanceFromConstructor(createdObjectPrameter);
                }
                else
                {
                    if (!_exportClasses.Contains(parameter.ParameterType))
                    {
                        throw new ArgumentException($"The {createdObject.Type.FullName} type does not found");
                    }

                    objParameters[i] = CreateInstanceFromConstructor(new CreatedObjectModel(parameter.ParameterType));
                }
                i++;
            }

            return createdObject.Constructor.Invoke(objParameters);
        }

        public TInstanceType CreateInstance<TInstanceType>()
        {
            return (TInstanceType)CreateInstance(typeof(TInstanceType));
        }
    }
}
