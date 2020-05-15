using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.Privat24;
using ApplicationServices.Privat24.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Privat24WebApp.Models;

namespace Privat24WebApp.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/v1/currencyRates")]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly ILogger<CurrencyRatesController> _logger;
        private readonly ICurrencyRateApplicationService _ratesService;

        public CurrencyRatesController(ILogger<CurrencyRatesController> logger, ICurrencyRateApplicationService ratesService)
        {
            _logger = logger;
            _ratesService = ratesService;
        }

        [HttpGet]
        public async Task<IEnumerable<CurrencyRateApiModel>> Get()
        {
            _logger.LogInformation("CurrencyRates - Getting rates for date range");
            var currencies = await _ratesService.GetCurrencyRates(DateTime.Now.AddMonths(-3), DateTime.Now);
            return currencies.GroupBy(x => x.Date).Select(x =>
            new CurrencyRateApiModel
            {
                Date = x.Key,
                USDSaleRateNBU = x.FirstOrDefault(v => v.Currency == "USD").SaleRateNBU,
                USDSaleRatePB = x.FirstOrDefault(v => v.Currency == "USD").SaleRatePB,
                EURSaleRateNBU = x.FirstOrDefault(v => v.Currency == "EUR").SaleRateNBU,
                EURSaleRatePB = x.FirstOrDefault(v => v.Currency == "EUR").SaleRatePB,
                USDPurchaseRateNBU = x.FirstOrDefault(v => v.Currency == "USD").PurchaseRateNBU,
                USDPurchaseRatePB = x.FirstOrDefault(v => v.Currency == "USD").PurchaseRatePB,
                EURPurchaseRateNBU = x.FirstOrDefault(v => v.Currency == "EUR").PurchaseRateNBU,
                EURPurchaseRatePB = x.FirstOrDefault(v => v.Currency == "EUR").PurchaseRatePB,
            });
        }
    }
}