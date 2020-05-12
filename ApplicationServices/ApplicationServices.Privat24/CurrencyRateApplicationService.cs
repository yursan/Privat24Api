using ApplicationServices.Privat24.Models;
using Data.Repositories.Privat24;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationServices.Privat24
{
    public class CurrencyRateApplicationService : ICurrencyRateApplicationService
    {
        private readonly ILogger<CurrencyRateApplicationService> _logger;
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CurrencyRateApplicationService(ILogger<CurrencyRateApplicationService> logger, ICurrencyRateRepository currencyRateRepository)
        {
            _logger = logger;
            _currencyRateRepository = currencyRateRepository;
        }

        public async Task<IReadOnlyList<CurrencyRateModel>> GetCurrencyRates(DateTime? dateStart, DateTime? dateEnd)
        {
            var rateEntities = await _currencyRateRepository.GetCurrencyRates(dateStart, dateEnd);
            var rateModels = rateEntities.Select(x => new CurrencyRateModel
            {
                BaseCurrency = x.BaseCurrency,
                Currency = x.ToCurrency,
                Date = x.Date,
                SaleRateNBU = x.SaleRateNBU,
                PurchaseRateNBU = x.PurchaseRateNBU,
                SaleRatePB = x.SaleRatePB,
                PurchaseRatePB = x.PurchaseRatePB
            }).ToArray();
            return rateModels;
        }
    }
}