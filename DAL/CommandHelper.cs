using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
	internal static class CommandHelper
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		internal static SqlCommand CreateCommand(
			SqlConnection connection,
			string sqlCommand,
			CommandType commandType,
			IEnumerable<IDataParameter> parameters,
			DbSetting databaseSetting)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand));
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));
			if (databaseSetting == null) throw new ArgumentNullException(nameof(databaseSetting));

			var command = new SqlCommand(sqlCommand, connection)
			{
				CommandType = commandType,
				CommandTimeout = databaseSetting.Timeout
			};

			foreach (var parameter in parameters)
			{
				command.Parameters.Add(parameter);
			}

			return command;
		}

		internal static SqlCommand CreateCommandWithReturnValue(
			SqlConnection connection,
			string sqlCommand,
			CommandType commandType,
			IEnumerable<IDataParameter> parameters,
			DbSetting databaseSetting)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (sqlCommand == null) throw new ArgumentNullException(nameof(sqlCommand));
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));
			if (databaseSetting == null) throw new ArgumentNullException(nameof(databaseSetting));

			var command = CreateCommand(connection, sqlCommand, commandType, parameters, databaseSetting);

			var outParameter = new SqlParameter("ReturnValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
			command.Parameters.Add(outParameter);

			return command;
		}
	}
}