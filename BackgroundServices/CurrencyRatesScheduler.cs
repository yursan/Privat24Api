using BackgroundServices.Jobs;
using Hangfire;
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

        public CurrencyRatesScheduler(ILogger<CurrencyRatesScheduler> logger, IJobClient jobClient)
        {
            _logger = logger;
            _jobClient = jobClient;
        }

        public override void Dispose()
        {
            _logger.LogDebug($"{GetType().Name} is disposing.");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{GetType().Name} is starting.");

            var result = BackgroundJob.Enqueue(() => Console.WriteLine("ENQUEUE JOB!"));
            _logger.LogDebug($"!!!Result from job Enqueue: {result}");

            _logger.LogInformation("Schedule job to run every 2 mins - '*/2 * * * *'");
            RecurringJob.AddOrUpdate("Privat24_LoadCurrencyRates", () => Console.WriteLine("--> Recurring job!"), "*/2 * * * *", TimeZoneInfo.Utc);

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