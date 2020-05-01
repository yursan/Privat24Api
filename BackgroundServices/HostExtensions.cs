using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime;

namespace BackgroundServices
{
    public static class HostExtensions
    {
        public static IHost Initialize(this IHost host)
        {
            Log(host.Services);

            return host;
        }
        public static void Log(IServiceProvider resolver)
        {
            var isDebugging = resolver.GetService<IConfiguration>().GetValue<bool?>("debug") == true || Debugger.IsAttached;

            var title = $"Background Service is starting on PID {Process.GetCurrentProcess().Id}";
            Console.Title = title;

            var connectionString = resolver.GetService<IConfiguration>().GetConnectionString("Privat24Db");

            var logger = resolver.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogInformation(title);
                logger.LogInformation($"Using GC-Mode: {(GCSettings.IsServerGC ? "Server" : "Client")}");

                if (string.IsNullOrWhiteSpace(connectionString))
                    logger.LogWarning($"No connection string provided for 'Privat24Db'.");
                else
                    logger.LogInformation( $"Using connection string '{AnonymizeSqlConnectionString(connectionString)}' for 'Privat24Db'.");
            }
        }

        private static string AnonymizeSqlConnectionString(string connectionString)
        {
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            if (builder.IntegratedSecurity)
            {
                return builder.ConnectionString;
            }

            builder.UserID = new string('*', 10);
            builder.Password = new string('*', 10);

            return builder.ConnectionString;
        }
    }
}