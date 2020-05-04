using BackgroundServices.Jobs;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundServices
{
    public class CurrencyRatesScheduler : BackgroundService
    {
        private readonly IJobClient _jobClient;
        private readonly ILogger<CurrencyRatesScheduler> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CurrencyRatesScheduler(ILogger<CurrencyRatesScheduler> logger, IJobClient jobClient, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _jobClient = jobClient;
            _serviceProvider = serviceProvider;
        }

        public override void Dispose()
        {
            _logger.LogDebug($"{GetType().Name} is disposing.");
        }

        public override Task StartAsync(CancellationToken token)
        {
            _logger.LogDebug($"{GetType().Name} is starting.");
            _logger.LogInformation("Try to schedule job that run every 2 mins - '*/2 * * * *'");

            var job = _serviceProvider.GetService<IJob>();

            if(job != null) RecurringJob.AddOrUpdate("Privat24_LoadCurrencyRates", () => job.Execute(token), "*/2 * * * *", TimeZoneInfo.Utc);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{GetType().Name} is stopping.");
            return Task.CompletedTask;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"{GetType().Name} ExecuteAsync is called.");

            var result = BackgroundJob.Enqueue(() => Console.WriteLine("Execute Async ENQUEUED JOB!"));
            _logger.LogDebug($"!!!Result from ExecuteAsync job Enqueue: {result}");
            await Task.Delay(1000);
        }
    }
}