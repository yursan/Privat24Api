using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationServices.Privat24;
using ApplicationServices.Privat24.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IEnumerable<CurrencyRateModel>> Get()
        {
            _logger.LogInformation("CurrencyRates - Getting rates for date range");

            return await _ratesService.GetCurrencyRates(DateTime.Now.AddYears(-1), DateTime.Now);
        }
    }
}