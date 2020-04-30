using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        public static void Log(System.IServiceProvider resolver)
        {
            var isDebugging = resolver.GetService<IConfiguration>().GetValue<bool?>("debug") == true || Debugger.IsAttached;

            var title = $"Background Service is starting";
            title = $"{title} on PID {Process.GetCurrentProcess().Id}";
            Console.WriteLine(title);
            Console.Title = title;

            var connectionStrings = resolver.GetService<IOptions<ConnectionStrings>>().Value;

            var logger = resolver.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogDebug("Initializing Host environment");
                logger.LogInformation(title);
                logger.LogInformation($"Using GC-Mode: {(GCSettings.IsServerGC ? "Server" : "Client")}");

                if (string.IsNullOrWhiteSpace(connectionStrings.Privat24Db))
                    logger.LogWarning($"No connection string provided for '{nameof(connectionStrings.Privat24Db)}'.");
                else
                    logger.LogInformation(
                        $"Using connection string '{AnonymizeSqlConnectionString(connectionStrings.Privat24Db)}' for '{nameof(connectionStrings.Privat24Db)}'."
                    );
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