using System.Collections.Generic;
using System.Data;

namespace DAL
{
    internal static class ExecuteHelper
    {
		internal static void ExecuteCommand(
			string sqlCommand,
			CommandType commandType,
			IEnumerable<IDataParameter> parameters,
			DbSetting databaseSetting)
		{
			using (var connection = ConnectionHelper.GetConnection(databaseSetting))
			{
				var command = CommandHelper.CreateCommand(connection, sqlCommand, commandType, parameters, databaseSetting);
				command.ExecuteNonQuery();
			}
		}
		internal static int ExecuteCommandWithReturnValue(
			string sqlCommand,
			CommandType commandType,
			IEnumerable<IDataParameter> parameters,
			DbSetting databaseSetting)
		{
			using (var connection = ConnectionHelper.GetConnection(databaseSetting))
			{
				var command = CommandHelper.CreateCommandWithReturnValue(connection, sqlCommand, commandType, parameters, databaseSetting);

				command.ExecuteNonQuery();
				return (int)command.Parameters["ReturnValue"].Value;
			}
		}

		internal static IDataReader ExecuteSelect(
			string sqlCommand,
			CommandType commandType,
			IEnumerable<IDataParameter> parameters,
			DbSetting databaseSetting)
		{
			var connection = ConnectionHelper.GetConnection(databaseSetting);
			var command = CommandHelper.CreateCommand(connection, sqlCommand, commandType, parameters, databaseSetting);

			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}
	}
}