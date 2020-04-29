using System;
using System.Data.SqlClient;
using System.Globalization;

namespace DAL
{
	internal static class ConnectionHelper
	{
		internal static SqlConnection GetConnection(DbSetting databaseSetting)
		{
			if (databaseSetting == null) throw new ArgumentNullException(nameof(databaseSetting));

			var connection = new SqlConnection(databaseSetting.ConnectionString);
			try
			{
				connection.Open();
			}
			catch (InvalidOperationException e)
			{
				string message = string.Format(
					CultureInfo.InvariantCulture,
					"Connection could not be opened. ConnectionString: \"{0}\"", connection.ConnectionString);
				//ToDO...
				//Log Error(message, exception);
				throw new Exception(message, e);
			}

			return connection;
		}
	}
}