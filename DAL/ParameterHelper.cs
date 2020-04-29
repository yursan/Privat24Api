using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public static class ParameterHelper
    {
		public static IDataParameter CreateParameter(string name, object value, SqlDbType dbType)
		{
			return new SqlParameter(name, dbType)
			{
				Value = value ?? DBNull.Value
			};
		}
	}
}