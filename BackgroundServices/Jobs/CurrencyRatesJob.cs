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
    public class CurrencyRatesJob : IJob
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IPrivat24ApiClient _privat24Api;
        private readonly ILogger<CurrencyRatesJob> _logger;
        
        public CurrencyRatesJob(ICurrencyRateRepository currencyRateRepository, IPrivat24Factory privat24Factory, ILogger<CurrencyRatesJob> log)
        {
            _logger = log;
            _currencyRateRepository = currencyRateRepository;
            _privat24Api = privat24Factory.CreatePublicClient();
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"CurrencyRatesJob is starting.");

            var latestDate = await _currencyRateRepository.GetLatestCurrencyRateDate();
            _logger.LogDebug($"Last query time: {latestDate}");

            var date = latestDate.HasValue ? GetPrevDate(latestDate.Value) : DateTime.Now;

            var dt1 = GetFirstDayOfMonth(date);
            var dt2 = GetMiddleOfMonth(date);
            var dt3 = GetLastDayOfMonth(date);
            _logger.LogDebug($"Dates for next query: {Environment.NewLine}");
            _logger.LogDebug($"\tFirst day: {dt1}{Environment.NewLine}");
            _logger.LogDebug($"\tMiddle of the month: {dt2}{Environment.NewLine}");
            _logger.LogDebug($"\tLast day: {dt3}{Environment.NewLine}");

            var currencyRates1 = await _privat24Api.GetCurrencyRates(dt1);
            await StoreCurencyRates(currencyRates1, dt1);

            if (dt2 < DateTime.Now)
            {
                var currencyRates2 = await _privat24Api.GetCurrencyRates(dt2);
                await StoreCurencyRates(currencyRates2, dt2);
            }
            if (dt3 < DateTime.Now)
            {
                var currencyRates3 = await _privat24Api.GetCurrencyRates(dt3);
                await StoreCurencyRates(currencyRates3, dt3);
            }
        }

        private async Task StoreCurencyRates(IEnumerable<ExchangeRate> currencyRates, DateTime date)
        {
            if (currencyRates.Any())
            {
                var entityToInsert = currencyRates
                    .Where(x => !string.IsNullOrEmpty(x.Currency)
                        && (x.Currency.Equals(CurrencyRateConstants.CurrencyEUR, StringComparison.OrdinalIgnoreCase)
                        || x.Currency.Equals(CurrencyRateConstants.CurrencyUSD, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => new CurrencyRateInsertEntity {
                        Date = date,
                        BaseCurrency = x.BaseCurrency,
                        ToCurrency = x.Currency,
                        SaleRateNBU = x.SaleRateNB,
                        PurchaseRateNBU = x.PurchaseRateNB,
                        SaleRatePB = (decimal)x.SaleRate,
                        PurchaseRatePB = (decimal)x.PurchaseRate
                    })
                    .ToList();
                _logger.LogDebug($"Trying to insert {entityToInsert.Count} entities");
                await _currencyRateRepository.AddCurrencyRates(entityToInsert);
            }
        }

        private DateTime GetPrevDate(DateTime currentDate)
        {
            return currentDate.AddMonths(-1);
        }

        private DateTime GetFirstDayOfMonth(DateTime dtDate)
        {
            var dtFrom = new DateTime(dtDate.Year, dtDate.Month, 1);
            return dtFrom.AddDays(-(dtFrom.Day - 1));
        }

        private DateTime GetLastDayOfMonth(DateTime dtDate)
        {
            var dtTo = dtDate;
            dtTo = dtTo.AddMonths(1);
            return dtTo.AddDays(-(dtTo.Day));
        }

        private DateTime GetMiddleOfMonth(DateTime dtDate)
        {
            var days = DateTime.DaysInMonth(dtDate.Year, dtDate.Month);
            var middle = (int)days / 2;
            return new DateTime(dtDate.Year, dtDate.Month, middle);
        }

    }
}