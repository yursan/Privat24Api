using Privat24.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Privat24
{
    public interface IPrivat24ApiClient
    {
        Task<IEnumerable<ExchangeRate>> GetCurrencyRates(DateTime? date);
    }
}