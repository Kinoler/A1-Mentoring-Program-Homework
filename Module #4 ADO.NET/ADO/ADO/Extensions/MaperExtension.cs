using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Extensions
{
    internal static class MaperExtension
    {
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            var item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                SetProperty(item, column.ColumnName, dataRow[column]);
            }

            return item;
        }

        private static void SetProperty(object obj, string propertyName, object value)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            if (property != null && value != DBNull.Value)
            {
                property.SetValue(obj, ChangeType(value, property.PropertyType), null);
            }
        }

        private static object ChangeType(object value, Type type)
        {
            if (value == null)
                return null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));

            return Convert.ChangeType(value, type);
        }
    }
}
