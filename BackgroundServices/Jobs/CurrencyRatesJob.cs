using Data.Repositories.Privat24;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundServices.Jobs
{
    public class CurrencyRatesJob : IJob
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ILogger<CurrencyRatesJob> _logger;
        
        public CurrencyRatesJob(ICurrencyRateRepository currencyRateRepository, ILogger<CurrencyRatesJob> log)
        {
            _logger = log;
            _currencyRateRepository = currencyRateRepository;
        }
        
        public async Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"CurrencyRatesJob is starting.");

            var latestDate = await _currencyRateRepository.GetLatestCurrencyRateDate();
            _logger.LogDebug($"Last query time: {latestDate}");
            //todo
        }
    }
}