using Data.Repositories.Privat24;
using Microsoft.Extensions.Logging;
using Privat24;
using Privat24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundServices.Jobs
{
    public class EveryDayCurrencyRatesJob : IJob
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IPrivat24ApiClient _privat24Api;
        private readonly ILogger<EveryDayCurrencyRatesJob> _logger;

        public EveryDayCurrencyRatesJob(ICurrencyRateRepository currencyRateRepository, IPrivat24Factory privat24Factory, ILogger<EveryDayCurrencyRatesJob> log)
        {
            _logger = log;
            _currencyRateRepository = currencyRateRepository;
            _privat24Api = privat24Factory.CreatePublicClient();
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"EveryDayCurrencyRatesJob is starting.");

            var latestDate = DateTime.Parse("2017-08-22");
            _logger.LogDebug($"Last query time: {latestDate}");

            while (latestDate != DateTime.MinValue)
            {
                var currencyRates = await _privat24Api.GetCurrencyRates(latestDate);
                await StoreCurencyRates(currencyRates, latestDate);

                latestDate = latestDate.AddDays(-1);
                _logger.LogDebug($"Last query time: {latestDate}");
                await Task.Delay(2000);
            }
        }

        private async Task StoreCurencyRates(IEnumerable<ExchangeRate> currencyRates, DateTime date)
        {
            _logger.LogDebug($"StoreCurencyRates: {currencyRates.Count()}");
            if (currencyRates.Any())
            {
                try
                {
                    var filteredEntity = currencyRates
                        .Where(x => !string.IsNullOrEmpty(x.Currency)
                            && (x.Currency.Equals(CurrencyRateConstants.CurrencyEUR, StringComparison.OrdinalIgnoreCase)
                            || x.Currency.Equals(CurrencyRateConstants.CurrencyUSD, StringComparison.OrdinalIgnoreCase))).ToArray();

                    var entityToInsert = filteredEntity
                        .Select(x => new CurrencyRateInsertEntity
                        {
                            Date = date,
                            BaseCurrency = x.BaseCurrency,
                            ToCurrency = x.Currency,
                            SaleRateNBU = x.SaleRateNB,
                            PurchaseRateNBU = x.PurchaseRateNB,
                            SaleRatePB = (x.SaleRate?? x.SaleRateNB),
                            PurchaseRatePB = (x.PurchaseRate?? x.PurchaseRateNB)
                        })
                        .ToList();

                    _logger.LogDebug($"Trying to insert {entityToInsert.Count} entities");
                    await _currencyRateRepository.AddCurrencyRates(entityToInsert);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.StackTrace);
                }
            }
        }
     }
}
