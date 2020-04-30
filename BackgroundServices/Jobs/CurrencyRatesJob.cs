using Data.Repositories.Privat24;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundServices.Jobs
{
    public class CurrencyRatesJob : IJob
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ILogger<CurrencyRatesJob> _logger;
        /*
        public CurrencyRatesJob(ICurrencyRateRepository currencyRateRepository, ILogger<CurrencyRatesJob> log)
        {
            _logger = log;
            _currencyRateRepository = currencyRateRepository;
        }
        */
        public async Task Execute(CancellationToken cancellationToken)
        {
            //var elderRates = await _currencyRateRepository.GetCurrencyRates(DateTime.Today);
            _logger.LogDebug($"Got {0} currencyRates");
            //todo
        }
    }
}