using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class DataHandler
    {
        public virtual IDataReader ExecuteSelect(string storedProcedureName, IEnumerable<IDataParameter> parameters, DbSetting databaseSetting)
        {
            return ExecuteHelper.ExecuteSelect(storedProcedureName, CommandType.StoredProcedure, parameters, databaseSetting);
        }
    }
}