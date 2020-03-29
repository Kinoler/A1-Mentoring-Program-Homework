using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Web.Configuration;
using System.Text;

namespace ADO
{
    internal class DbHelper
    {
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public DbHelper(string connectionStringName)
        {
            var connectionStringSettings = WebConfigurationManager.ConnectionStrings[connectionStringName];
            ConnectionString = connectionStringSettings.ConnectionString;
            ProviderName = connectionStringSettings.ProviderName;
        }

        public List<T> SelectData<T>(string tableName) where T : new()
        {
            string commandText = $"SELECT * FROM {tableName}";
            var result = new List<T>();
            var dataTeble = GetDataTable(commandText);

            for (int i = 0; i < dataTeble.Rows.Count; i++)
            {
                result.Add(dataTeble.Rows[i].ToObject<T>());

            }
            return result;
        }

        public void Update<T>(T data, string tableName)
        {
            var valueProperties = data.ToValueProperties().ToArray();
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"UPDATE {tableName}");
            stringBuilder.Append($" SET ");
            for (int i = 1; i < valueProperties.Length - 1; i++)
                stringBuilder.Append($" {valueProperties[i].Key} = {valueProperties[i].Value},");
            stringBuilder.Append($" {valueProperties[valueProperties.Length - 1].Key} = {valueProperties[valueProperties.Length - 1].Value}");
            stringBuilder.Append($" WHERE {valueProperties[0].Key} = {valueProperties[0].Value}");

            ExecuteNonQuery(stringBuilder.ToString());
        }

        public void Delete<T>(T data, string tableName)
        {
            var valueProperties = data.ToValueProperties().ToArray();
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($" DELETE FROM {tableName}");
            stringBuilder.Append($" WHERE {valueProperties[0].Key} = {valueProperties[0].Value}");

            ExecuteNonQuery(stringBuilder.ToString());
        }

        public void Insert<T>(T data, string tableName)
        {
            var valueProperties = data.ToValueProperties().ToArray();
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($" INSERT INTO {tableName}");
            stringBuilder.Append($" VALUES (");
            for (int i = 1; i < valueProperties.Length - 1; i++)
                stringBuilder.Append($"{valueProperties[i].Value},");
            stringBuilder.Append($"{valueProperties[valueProperties.Length - 1].Value});");

            ExecuteNonQuery(stringBuilder.ToString());
        }

        public List<T> CallStoredProcedure<T>(string procedureName, params DbParameter[] parameters) where T : new()
        {
            return CallStoredProcedure(procedureName, parameters).ToObjects<T>();
        }


        private void ExecuteNonQuery(string commandText)
        {
            using (var connection = DbProviderFactories.GetFactory(ProviderName).CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = DbProviderFactories.GetFactory(ProviderName).CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                }
            }
        }

        public DataSet GetDataSet(string commandText)
        {
            using (var connection = DbProviderFactories.GetFactory(ProviderName).CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = DbProviderFactories.GetFactory(ProviderName).CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    var dataset = new DataSet();
                    var dataAdaper = DbProviderFactories.GetFactory(ProviderName).CreateDataAdapter();
                    dataAdaper.SelectCommand = command;
                    dataAdaper.Fill(dataset);
                    return dataset;
                }
            }
        }

        public DataTable GetDataTable(string commandText)
        {
            return GetDataSet(commandText).Tables[0];
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType)
        {
            return CreateParameter(name, 0, value, dbType, ParameterDirection.Input);
        }

        public DbParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            var parameter = DbProviderFactories.GetFactory(ProviderName).CreateParameter();
            parameter.DbType = dbType;
            parameter.ParameterName = name;
            parameter.Size = size;
            parameter.Direction = direction;
            parameter.Value = value;

            return parameter;
        }

        public DbDataReader CallStoredProcedure(string procedureName, params DbParameter[] parameters)
        {
            using (var connection = DbProviderFactories.GetFactory(ProviderName).CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = DbProviderFactories.GetFactory(ProviderName).CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    return command.ExecuteReader();
                }
            }
        }
    }
}
