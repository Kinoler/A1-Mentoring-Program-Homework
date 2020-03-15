using Reflection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Reflection.Container;

namespace Reflection
{
    static class EmitHelper
    {
        public delegate object CreateImportObjectDelegate(object[] constructorArgs);
        public delegate object CreateExportObjectDelegate();
        public delegate void SetImportPropertyDelegate(object instance, object[] propValues);

        public static CreateImportObjectDelegate CreateMethodForConstructImportClass(CreatedObjectModel createdObject)
        {
            DynamicMethod construct = new DynamicMethod(
                ".ctor",
                createdObject.Type,
                new[] { typeof(object[]) });

            var il = construct.GetILGenerator();
            var ctorParams = createdObject.ConstructorParameters;
            for (int i = 0; i < ctorParams.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                Type paramType = ctorParams[i].ParameterType;
                il.Emit(paramType.IsValueType ? OpCodes.Unbox_Any
                    : OpCodes.Castclass, paramType);
            }

            il.Emit(OpCodes.Newobj, createdObject.Constructor);
            il.Emit(OpCodes.Ret);

            var createDelegate =
                (CreateImportObjectDelegate)construct.CreateDelegate(typeof(CreateImportObjectDelegate));

            return createDelegate;
        }

        public static CreateExportObjectDelegate CreateMethodForConstructExportClass(CreatedObjectModel createdObject)
        {
            DynamicMethod construct = new DynamicMethod(
                ".ctor",
                typeof(object),
                new Type[0],
                typeof(Container).Module);

            ILGenerator il = construct.GetILGenerator();
            il.Emit(OpCodes.Newobj, createdObject.Constructor);
            il.Emit(OpCodes.Ret);

            var createDelegate =
                (CreateExportObjectDelegate)construct.CreateDelegate(typeof(CreateExportObjectDelegate));

            return createDelegate;
        }

        public static SetImportPropertyDelegate CreateMethodForSetImportProperty(CreatedObjectModel createdObject)
        {
            DynamicMethod construct = new DynamicMethod(
                "SetImportProperty",
                null,
                new[] { typeof(object), typeof(object[]) });

            ILGenerator il = construct.GetILGenerator();

            var props = createdObject.ImportedProperties;
            for (int i = 0; i < props.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, createdObject.Type);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                Type paramType = props[i].PropertyType;
                il.Emit(paramType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, paramType);

                il.Emit(OpCodes.Call, props[i].SetMethod);
            }

            il.Emit(OpCodes.Ret);

            var createDelegate =
                (SetImportPropertyDelegate)construct.CreateDelegate(typeof(SetImportPropertyDelegate));

            return createDelegate;
        }
    }
}
