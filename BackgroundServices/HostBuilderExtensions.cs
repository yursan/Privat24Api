using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace BackgroundServices
{
    public static class HostBuilderExtensions
    {
        private const string EnvironmentKey = "CurrentEnvironment";

        public static IHostBuilder ConfigureConfiguration(this IHostBuilder build, string[] args)
        {
            build.ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile(@"./config/settings.json", true, true);
                builder.AddEnvironmentVariables();

                var config = builder.Build();
                var configEnvironment = config[EnvironmentKey];
                
                builder.AddJsonFile(
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? @"c:\privat24\settings.json" //global settings file
                        : @"/etc/privat24/settings.json"
                    , true, true);

                // Override environment gained from environment variables if possible.
                if (!string.IsNullOrWhiteSpace(configEnvironment))
                    builder.AddInMemoryCollection(new[]
                        {
                            new KeyValuePair<string, string>(EnvironmentKey, configEnvironment)
                        }
                    );

                builder.AddCommandLine(args);
            });

            return build;
        }
     }
}