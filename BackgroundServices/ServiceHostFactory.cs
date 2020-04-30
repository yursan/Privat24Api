using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace BackgroundServices
{
    public static class ServiceHostFactory
    {
        public static IHostBuilder Create(string[] args)
        {
            //DapperMapper.AddTypeHandler<Date>(new DateDapperTypeHandler());

            return Host.CreateDefaultBuilder(args)
                .ConfigureConfiguration(args)
                .ConfigureLogging((ctx, logBuilder) =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Trace)
                        .AddConfiguration(ctx.Configuration.GetSection("Logging"))
                        .ClearProviders();
#if DEBUG
                    logBuilder.AddConsole();
#endif
                    logBuilder.AddNLog();
                });
        }
    }
}
