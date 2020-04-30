using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading.Tasks;

namespace BackgroundServices
{
    class Program
    {
        public static Task Main(string[] args)
        {
            //LogManager.Configuration.Variables["logDirectory"] = Path.Combine(AppContext.BaseDirectory, "logs");
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Starting main program");

			return ServiceHostFactory.Create(args)
				 .ConfigureWebHostDefaults(webBuilder =>
				 {
					 webBuilder
						 .UseStartup<Startup>()
						 .UseUrls("http://+:58101");
				 })
				.Build()
				.Initialize()
				.RunAsync();
			/*
#if DEBUG
				await builder.RunConsoleAsync();
#else
				await builder.RunAsServiceAsync();
#endif
*/
				//LogManager.Shutdown();
		}
	}
}