using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BackgroundServices
{
    class Program
    {
        public static Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
			var config = LogManager.Configuration;
			if (config != null)
			{
				config.Variables["logDirectory"] = Path.Combine(AppContext.BaseDirectory, "logs");
			}

			logger.Debug("Starting main program");

			return ServiceHostFactory.Create(args)
				 .ConfigureWebHostDefaults(webBuilder =>
				 {
					 webBuilder
						 .UseStartup<Startup>()
						 .UseUrls("http://+:58101");
				 })
				 .RunAsServiceAsync();
		}
	}
}