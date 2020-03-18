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

            var modelObjectCreated = new ModelObjectCreated(classType);
            if (!modelObjectCreated.IsExport)
                throw new ArgumentException($"The parameter should be " +
                                            $"have the {nameof(ExportAttribute)} attribute", nameof(classType));

            var exportAttribute = modelObjectCreated.Type.GetCustomAttribute<ExportAttribute>();
            if (!exportAttribute.IsInterfaceInitializer)
                throw new ArgumentException($"The parameter should be " +
                                            $"have {nameof(ExportAttribute)} attribute " +
                                            $"with the {nameof(interfaceType)} parameter.", nameof(classType));

            if (!modelObjectCreated.Type.GetInterfaces().Any(classInterface => classInterface == interfaceType))
                throw new ArgumentException($"The parameter should implement " +
                                            $"the {nameof(interfaceType)} interface.", nameof(classType));

            if (_exportInterfaces.ContainsKey(interfaceType))
                throw new ArgumentException("This interface type is already exist.", nameof(interfaceType));

            _exportInterfaces.Add(interfaceType, classType);
            AddType(classType);
        }

        public void AddType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException($"The parameter can not be null", nameof(type));

            var modelObjectCreated = new ModelObjectCreated(type);

            var hasAnyAttr = false;
            if (modelObjectCreated.IsExport)
            {
                if (_exportClasses.ContainsKey(type))
                    throw new ArgumentException("This type is already exist.", nameof(type));

                _exportClasses.Add(type, null);
                hasAnyAttr = true;
            }

            if (modelObjectCreated.IsConstructorImport ^
                modelObjectCreated.IsPropertyImport)
            {
                if (_importClasses.ContainsKey(type))
                    throw new ArgumentException("This type is already exist.", nameof(type));

                _importClasses.Add(type, null);
                hasAnyAttr = true;
            }

            if (modelObjectCreated.IsPropertyImport)
            {
                if (_importPropClasses.ContainsKey(type))
                    throw new ArgumentException("This type is already exist.", nameof(type));

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
                throw new ArgumentException("This type does not exist in container.", nameof(type));

            var modelObjectCreated = new ModelObjectCreated(type);

            modelObjectCreated.Instance = CreateInstanceForImportClass(modelObjectCreated);

            if (modelObjectCreated.IsConstructorImport)
                return modelObjectCreated.Instance;

            if (modelObjectCreated.IsPropertyImport)
                SetProrertyForInstance(modelObjectCreated);

            return modelObjectCreated.Instance;
        }

        public TInstanceType CreateInstance<TInstanceType>()
        {
            return (TInstanceType)CreateInstance(typeof(TInstanceType));
        }

        private void SetProrertyForInstance(ModelObjectCreated modelObjectCreated)
        {
            var propValues = new List<object>();

            foreach (var importedProperty in modelObjectCreated.ImportedProperties)
            {
                ModelObjectCreated modelObjectCreatedProperty = new ModelObjectCreated(importedProperty.PropertyType);

                if (modelObjectCreatedProperty.Type.IsInterface)
                {
                    if (!_exportInterfaces.ContainsKey(modelObjectCreatedProperty.Type))
                        throw new ArgumentException("The interface does not found.", modelObjectCreatedProperty.Type.FullName);

                    modelObjectCreatedProperty = new ModelObjectCreated(_exportInterfaces[modelObjectCreatedProperty.Type]);
                }
                
                modelObjectCreatedProperty.Instance = CreateInstanceForExportClass(modelObjectCreatedProperty);

                propValues.Add(modelObjectCreatedProperty.Instance);
            }

            if (!_importPropClasses.ContainsKey(modelObjectCreated.Type))
                throw new ArgumentException("The type does not found", modelObjectCreated.Type.FullName);

            if (_importPropClasses[modelObjectCreated.Type] == null)
                _importPropClasses[modelObjectCreated.Type] = CreateMethodForSetImportProperty(modelObjectCreated);

            _importPropClasses[modelObjectCreated.Type].Invoke(modelObjectCreated.Instance, propValues.ToArray());
        }

        private object CreateInstanceForImportClass(ModelObjectCreated modelObjectCreated)
        {
            object[] objParameters = new object[modelObjectCreated.ConstructorParameters.Length];

            int i = 0;
            foreach (ParameterInfo parameter in modelObjectCreated.ConstructorParameters)
            {
                if (parameter.ParameterType.IsInterface)
                {
                    if (!_exportInterfaces.ContainsKey(parameter.ParameterType))
                        throw new ArgumentException($"The {modelObjectCreated.Type.FullName} class should be have implementation " +
                            $"for {parameter.ParameterType.FullName} interface", nameof(modelObjectCreated.Type));

                    var modelObjectCreatedPrameter = new ModelObjectCreated(_exportInterfaces[parameter.ParameterType]);
                    objParameters[i] = CreateInstanceForExportClass(modelObjectCreatedPrameter);
                }
                else
                {
                    objParameters[i] = CreateInstanceForExportClass(new ModelObjectCreated(parameter.ParameterType));
                }
                i++;
            }

            if (_importClasses[modelObjectCreated.Type] == null)
                _importClasses[modelObjectCreated.Type] = CreateMethodForConstructImportClass(modelObjectCreated);

            return _importClasses[modelObjectCreated.Type].Invoke(objParameters);
        }

        private object CreateInstanceForExportClass(ModelObjectCreated modelObjectCreated)
        {
            if (!_exportClasses.ContainsKey(modelObjectCreated.Type))
                throw new ArgumentException($"The {modelObjectCreated.Type.FullName} type does not found", nameof(modelObjectCreated.Type));

            if (_exportClasses[modelObjectCreated.Type] == null)
                _exportClasses[modelObjectCreated.Type] = CreateMethodForConstructExportClass(modelObjectCreated);

            return _exportClasses[modelObjectCreated.Type].Invoke();
        }
    }
}
