using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.Privat24
{
    public interface ICurrencyRateRepository
    {
        Task<DateTime> GetLatestCurrencyRateDate();
        Task<IReadOnlyList<CurrencyRateEntity>> GetCurrencyRates(DateTime date);
        Task AddCurrencyRates(IReadOnlyList<CurrencyRateInsertEntity> rates);
    }
}