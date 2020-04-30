using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackgroundServices
{
    internal static class ServiceCollectionExtensions
    {
		public static IServiceCollection AddHangfireServerWithCustomConfig(this IServiceCollection services, IConfiguration configuration)
		{
			var connStr = configuration.GetConnectionString("Privat24Db");

			return services
				.AddHangfire(config => config
					.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
					.UseColouredConsoleLogProvider()
					.UseSimpleAssemblyNameTypeSerializer()
					.UseRecommendedSerializerSettings()
					.UseSqlServerStorage(connStr, 
					new SqlServerStorageOptions
					{
						CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
						SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
						QueuePollInterval = TimeSpan.Zero,
						UseRecommendedIsolationLevel = true,
						UsePageLocksOnDequeue = true,
						DisableGlobalLocks = true
					}))
				// Add the processing server as IHostedService
				.AddHangfireServer();
		}
	}
}