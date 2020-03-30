using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Web.Configuration;
using System.Text;

namespace ADO.DbConnectors
{
    internal class DbConnetor : IDbConnector
    {
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public DbConnetor(string connectionStringName)
        {
            var connectionStringSettings = WebConfigurationManager.ConnectionStrings[connectionStringName];
            ConnectionString = connectionStringSettings.ConnectionString;
            ProviderName = connectionStringSettings.ProviderName;
        }

        public List<T> CallStoredProcedure<T>(string procedureName, params DbParameter[] parameters) where T : new()
        {
            return CallStoredProcedure(procedureName, parameters).ToObjects<T>();
        }


        public void ExecuteNonQuery(string commandText)
        {
            var connection = DbProviderFactories.GetFactory(ProviderName).CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            var command = DbProviderFactories.GetFactory(ProviderName).CreateCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }

        public DataSet GetDataSet(string commandText)
        {
            var connection = DbProviderFactories.GetFactory(ProviderName).CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            var command = DbProviderFactories.GetFactory(ProviderName).CreateCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;

            var dataset = new DataSet();
            var dataAdaper = DbProviderFactories.GetFactory(ProviderName).CreateDataAdapter();
            dataAdaper.SelectCommand = command;
            dataAdaper.Fill(dataset);

            return dataset;
        }

        public IDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            var parameter = DbProviderFactories.GetFactory(ProviderName).CreateParameter();
            parameter.DbType = dbType;
            parameter.ParameterName = name;
            parameter.Size = size;
            parameter.Direction = direction;
            parameter.Value = value;

            return parameter;
        }

        public IDataReader CallStoredProcedure(string procedureName, params IDataParameter[] parameters)
        {
            using var connection = DbProviderFactories.GetFactory(ProviderName).CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            using var command = DbProviderFactories.GetFactory(ProviderName).CreateCommand();
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
