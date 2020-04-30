using BackgroundServices.Jobs;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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


            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{GetType().Name} is stopping.");

            return Task.CompletedTask;
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = BackgroundJob.Enqueue(() => Execute(stoppingToken));

            return Task.CompletedTask;
        }

        public Task Execute(CancellationToken cancellationToken)
        {
            //var elderRates = await _currencyRateRepository.GetCurrencyRates(DateTime.Today);
            _logger.LogDebug($"Got {0} currencyRates");
            //todo
            return Task.CompletedTask;
        }
    }
}