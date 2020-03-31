using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Interfaces
{
    public interface IDbConnector
    {
        DataTable CallStoredProcedure(string procedureName, params IDataParameter[] parameters);
        IDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction);
        DataTable GetDataTable(string commandText);
        void ExecuteNonQuery(string commandText);
    }
}