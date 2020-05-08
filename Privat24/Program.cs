using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Privat24
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging((ctx, logBuilder) =>
                {
                    var connStr = ctx.Configuration.GetSection("ConnectionStrings");
                    var logSection = ctx.Configuration.GetSection("Logging");
                    logBuilder.SetMinimumLevel(LogLevel.Trace)
                        .AddConfiguration(logSection)
                        .ClearProviders();
#if DEBUG
                    logBuilder.AddConsole();
#endif
                    logBuilder.AddNLog();
                });
    }
}
