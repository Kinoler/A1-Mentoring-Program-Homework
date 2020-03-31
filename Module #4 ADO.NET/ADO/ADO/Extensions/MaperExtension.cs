﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public static class MaperExtension
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
        /*
        public static List<T> ToObjects<T>(this IDataReader reader) where T : new()
        {
            var result = new List<T>();

            while (reader.Read())
            {
                var item = new T();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    SetProperty(item, reader.GetName(i), reader.GetValue(i));
                }

                result.Add(item);
            }

            return result;
        }
        
        public static Dictionary<string, string> ToValueProperties(this object obj)
        {
            var result = new Dictionary<string, string>();
            foreach (var property in obj.GetType().GetProperties())
            {
                result.Add(property.Name, property.GetValue(obj).ConvertToDb());
            }

            return result;
        }
        */
        internal static string ConvertToDbParameter(this object obj)
        {
            switch (obj)
            {
                case null:
                    return "NULL";
                case DateTime _:
                case string _:
                    return $"'{obj}'";
                default:
                    return obj.ToString();
            }
        }
        
        private static void SetProperty(object obj, string propertyName, object value)
        {
            PropertyInfo property = GetProperty(obj.GetType(), propertyName);

            if (property != null && value != DBNull.Value && value.ToString() != "NULL")
            {
                property.SetValue(obj, ChangeType(value, property.PropertyType), null);
            }
        }

        private static PropertyInfo GetProperty(Type type, string propertyName)
        {
            PropertyInfo property = type.GetProperty(propertyName);

            if (property == null)
                throw new InvalidOperationException($"The type {type} have not a property with name {propertyName}");

            return property;
        }

        private static object ChangeType(object value, Type type)
        {
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, type);

            return 
                value == null ? 
                null : 
                Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? throw new ArgumentNullException(nameof(type)));
        }
    }
}
