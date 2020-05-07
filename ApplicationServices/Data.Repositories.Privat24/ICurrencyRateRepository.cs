using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.Privat24
{
    public interface ICurrencyRateRepository
    {
        Task<DateTime?> GetLatestCurrencyRateDate();
        Task<IReadOnlyList<CurrencyRateEntity>> GetCurrencyRates(DateTime? dateStart, DateTime? dateEnd);
        Task AddCurrencyRates(IReadOnlyList<CurrencyRateInsertEntity> rates);
    }
}