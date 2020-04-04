using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Web.Configuration;
using System.Text;
using ADO.Interfaces;

namespace ADO.DbConnectors
{
    internal class DbConnetor : IDbConnector
    {
        private string ConnectionString { get; set; }
        private DbProviderFactory DbProviderFactory { get; set; }
        public string ProviderName { get; }

        public DbConnetor(string connectionStringName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            ConnectionString = connectionStringSettings.ConnectionString;
            ProviderName = connectionStringSettings.ProviderName;

            DbProviderFactory = DbProviderFactories.GetFactory(ProviderName);
        }

        public void ExecuteNonQuery(string commandText, params IDataParameter[] parameters)
        {
            var command = OpenConnectionAndCreateCommand(commandText, CommandType.Text, parameters);
            command.ExecuteNonQuery();
        }

        public DataSet GetDataSet(string commandText, params IDataParameter[] parameters)
        {
            var command = OpenConnectionAndCreateCommand(commandText, CommandType.Text, parameters);

            var dataset = new DataSet();
            var dataAdaper = DbProviderFactory.CreateDataAdapter();
            dataAdaper.SelectCommand = command;
            dataAdaper.Fill(dataset);

            return dataset;
        }

        public DataTable CallStoredProcedure(string procedureName, params IDataParameter[] parameters)
        {
            var command = OpenConnectionAndCreateCommand(procedureName, CommandType.StoredProcedure, parameters);

            var table = new DataTable();
            table.Load(command.ExecuteReader());
            return table;
        }

        private DbConnection CreateConnection(string connectionString)
        {
            var connection = DbProviderFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            return connection;
        }

        private DbCommand OpenConnectionAndCreateCommand(string commandText, CommandType type = CommandType.Text, params IDataParameter[] parameters)
        {
            var connection = CreateConnection(ConnectionString);
            connection.Open();

            var command = DbProviderFactory.CreateCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            return command;
        }

        public IDataParameter CreateParameter(
            string name,
            object value,
            DbType dbType,
            bool isNullable,
            ParameterDirection direction)
        {
            var parameter = DbProviderFactory.CreateParameter();
            parameter.DbType = dbType;
            parameter.ParameterName = name;
            parameter.Direction = direction;
            parameter.Value = value;
            parameter.IsNullable = isNullable;

            return parameter;
        }

        public IDataParameter CreateParameter(
            string name,
            object value,
            bool isNullable = false,
            ParameterDirection direction = ParameterDirection.Input)
        {
            return CreateParameter(name, value, GetDbType(value), isNullable, direction);
        }

        private DbType GetDbType(object value)
        {
            switch (value)
            {
                case string _:
                    return DbType.String;
                case int _:
                    return DbType.Int32;
                case DateTime _:
                    return DbType.DateTime;
                case bool _:
                    return DbType.Boolean;
            }

            return DbType.Object;
        }
    }
}
