using ApplicationServices.Privat24.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.Privat24
{
    public interface ICurrencyRateApplicationService
    {
        Task<IReadOnlyList<CurrencyRateModel>> GetCurrencyRates(DateTime? dateStart, DateTime? dateEnd);
    }
}