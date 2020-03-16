using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Reflection.Attributes;
using Reflection.Models;
using static Reflection.EmitHelper;

namespace Reflection
{
    public class Container
    {
        private readonly Dictionary<Type, CreateImportObjectDelegate> _importClasses;
        private readonly Dictionary<Type, SetImportPropertyDelegate> _importPropClasses;
        private readonly Dictionary<Type, CreateExportObjectDelegate> _exportClasses;
        private readonly Dictionary<Type, Type> _exportInterfaces;

        public Container()
        {
            _importClasses = new Dictionary<Type, CreateImportObjectDelegate>();
            _importPropClasses = new Dictionary<Type, SetImportPropertyDelegate>();
            _exportClasses = new Dictionary<Type, CreateExportObjectDelegate>();
            _exportInterfaces = new Dictionary<Type, Type>();
        }

        public void AddType(Type classType, Type interfaceType)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));
            if (!classType.IsClass)
                throw new ArgumentException($"The parameter should be class", nameof(classType));
            if (!interfaceType.IsInterface)
                throw new ArgumentException($"The parameter should be interface", nameof(interfaceType));

            var createdObject = new CreatedObjectModel(classType);
            if (!createdObject.IsExport)
                throw new ArgumentException($"The parameter should be " +
                                            $"have the {nameof(ExportAttribute)} attribute", nameof(classType));

            var exportAttribute = createdObject.Type.GetCustomAttribute<ExportAttribute>();
            if (!exportAttribute.IsInterfaceInitializer)
                throw new ArgumentException($"The parameter should be " +
                                            $"have {nameof(ExportAttribute)} attribute " +
                                            $"with the {nameof(interfaceType)} parameter.", nameof(classType));

            if (!createdObject.Type.GetInterfaces().Any(classInterface => classInterface == interfaceType))
                throw new ArgumentException($"The parameter should implement " +
                                            $"the {nameof(interfaceType)} interface.", nameof(classType));

            if (_exportInterfaces.ContainsKey(interfaceType))
                throw new ArgumentException("This interface type is already exist.");

            _exportInterfaces.Add(interfaceType, classType);
            AddType(classType);
        }

        public void AddType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException($"The parameter can not be null", nameof(type));

            var createdObject = new CreatedObjectModel(type);

            var hasAnyAttr = false;
            if (createdObject.IsExport)
            {
                if (_exportClasses.ContainsKey(type))
                    throw new ArgumentException("This type is already exist.");

                _exportClasses.Add(type, null);
                hasAnyAttr = true;
            }

            if (createdObject.IsConstructorImport ^
                createdObject.IsPropertyImport)
            {
                if (_importClasses.ContainsKey(type))
                    throw new ArgumentException("This type is already exist.");

                _importClasses.Add(type, null);
                hasAnyAttr = true;
            }

            if (createdObject.IsPropertyImport)
            {
                if (_importPropClasses.ContainsKey(type))
                    throw new ArgumentException("This type is already exist.");

                _importPropClasses.Add(type, null);
                hasAnyAttr = true;
            }

            if (!hasAnyAttr)
            {
                throw new ArgumentException($"The should be " +
                                            $"have only one of {nameof(ExportAttribute)}, " +
                                            $"{nameof(ImportAttribute)} or " +
                                            $"{nameof(ImportConstructorAttribute)} attributes.", nameof(type));
            }
        }

        public object CreateInstance(Type type)
        {
            if (!_importClasses.ContainsKey(type))
                throw new ArgumentException("This type does not exist in container.");

            var createdObject = new CreatedObjectModel(type);

            createdObject.Instance = CreateInstanceFromImportClass(createdObject);

            if (createdObject.IsConstructorImport)
                return createdObject.Instance;

            if (createdObject.IsPropertyImport)
                SetInstanceForProrerty(createdObject);

            return createdObject.Instance;
        }

        public void SetInstanceForProrerty(CreatedObjectModel createdObject)
        {
            var propValues = new List<object>();

            foreach (var importedProperty in createdObject.ImportedProperties)
            {
                CreatedObjectModel createdObjectProperty = new CreatedObjectModel(importedProperty.PropertyType);

                if (createdObjectProperty.Type.IsInterface)
                {
                    if (!_exportInterfaces.ContainsKey(createdObjectProperty.Type))
                        throw new ArgumentException("The interface does not found.", createdObjectProperty.Type.FullName);

                    createdObjectProperty = new CreatedObjectModel(_exportInterfaces[createdObjectProperty.Type]);
                }
                
                createdObjectProperty.Instance = CreateInstanceFromExportClass(createdObjectProperty);

                propValues.Add(createdObjectProperty.Instance);
            }

            if (!_importPropClasses.ContainsKey(createdObject.Type))
                throw new ArgumentException("The type does not found", createdObject.Type.FullName);

            if (_importPropClasses[createdObject.Type] == null)
                _importPropClasses[createdObject.Type] = CreateMethodForSetImportProperty(createdObject);

            _importPropClasses[createdObject.Type].Invoke(createdObject.Instance, propValues.ToArray());
        }

        public object CreateInstanceFromImportClass(CreatedObjectModel createdObject)
        {
            object[] objParameters = new object[createdObject.ConstructorParameters.Length];

            int i = 0;
            foreach (ParameterInfo parameter in createdObject.ConstructorParameters)
            {
                if (parameter.ParameterType.IsInterface)
                {
                    if (!_exportInterfaces.ContainsKey(parameter.ParameterType))
                        throw new ArgumentException($"The class should be have implementation " +
                            $"for {parameter.ParameterType.FullName} interface", createdObject.Type.FullName);

                    var createdObjectPrameter = new CreatedObjectModel(_exportInterfaces[parameter.ParameterType]);
                    objParameters[i] = CreateInstanceFromExportClass(createdObjectPrameter);
                }
                else
                {
                    objParameters[i] = CreateInstanceFromExportClass(new CreatedObjectModel(parameter.ParameterType));
                }
                i++;
            }

            if (_importClasses[createdObject.Type] == null)
                _importClasses[createdObject.Type] = CreateMethodForConstructImportClass(createdObject);

            return _importClasses[createdObject.Type].Invoke(objParameters);
        }

        public object CreateInstanceFromExportClass(CreatedObjectModel createdObject)
        {
            if (!_exportClasses.ContainsKey(createdObject.Type))
                throw new ArgumentException($"The type does not found", createdObject.Type.FullName);

            if (_exportClasses[createdObject.Type] == null)
                _exportClasses[createdObject.Type] = CreateMethodForConstructExportClass(createdObject);

            return _exportClasses[createdObject.Type].Invoke();
        }

        public TInstanceType CreateInstance<TInstanceType>()
        {
            return (TInstanceType)CreateInstance(typeof(TInstanceType));
        }
    }
}
