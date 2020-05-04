using BackgroundServices.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundServices
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<Program>>())
                .AddSingleton(_configuration)
                .AddHangfireServerWithCustomConfig(_configuration)
                .AddSingleton(typeof(IJobClient), typeof(JobClient))
                .AddHostedService<CurrencyRatesScheduler>()
                .RegisterRepositories()
                .RegisterJobs();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
        }
    }
}