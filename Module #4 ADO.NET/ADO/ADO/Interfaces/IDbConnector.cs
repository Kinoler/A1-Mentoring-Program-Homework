using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Interfaces
{
    internal interface IDbConnector
    {
        string ProviderName { get; }

        DataTable CallStoredProcedure(string procedureName, params IDataParameter[] parameters);

        IDataParameter CreateParameter(
            string name, 
            object value,
            DbType dbType,
            bool isNullable,
            ParameterDirection direction);

        IDataParameter CreateParameter(
            string name,
            object value,
            bool isNullable = false,
            ParameterDirection direction = ParameterDirection.Input);

        DataSet GetDataSet(string commandText, params IDataParameter[] parameters);

        void ExecuteNonQuery(string commandText, params IDataParameter[] parameters);
    }
}