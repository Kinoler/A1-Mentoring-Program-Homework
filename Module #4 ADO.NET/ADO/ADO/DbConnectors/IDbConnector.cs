using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.DbConnectors
{
    public interface IDbConnector
    {
        IDataReader CallStoredProcedure(string procedureName, params IDataParameter[] parameters);
        IDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction);
        DataSet GetDataSet(string commandText);
        void ExecuteNonQuery(string commandText);
    }
}