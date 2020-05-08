using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;

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
                    var logSection = ctx.Configuration.GetSection("Logging");
                    var config = LogManager.Configuration;
                    if (config != null)
                    {
                        config.Variables["baseDirectory"] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    }
                    logBuilder
                        .AddConfiguration(logSection)
                        .ClearProviders();
#if DEBUG
                    logBuilder.AddConsole();
#endif
                    logBuilder.AddNLog();
                });
    }
}
