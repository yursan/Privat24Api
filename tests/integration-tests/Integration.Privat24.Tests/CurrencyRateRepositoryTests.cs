using Data.Repositories.Privat24;
using Microsoft.Extensions.Logging;
using Moq;
using Privat24;
using Privat24.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Privat24.Tests
{
    public class CurrencyRateRepositoryTests
    {
        [Fact]
        public async Task AddCurrencyRatesForToday()
        {
            // Arrange
            var apiClient = CreatePrivat24ApiClient();
            if (!DateTime.TryParseExact("04.11.2014", @"dd.MM.yyyy", new CultureInfo("uk-UA"), DateTimeStyles.None, out DateTime date))
                date = DateTime.Now;

            var repository = new CurrencyRateRepository();

            //Act
            var rates = await apiClient.GetCurrencyRates(date);

            // Assert
            Assert.NotNull(rates);
            var uahToEur = rates.FirstOrDefault(r => !string.IsNullOrEmpty(r.Currency) && r.Currency.Equals("EUR") && r.BaseCurrency.Equals("UAH"));
            Assert.NotNull(uahToEur);

            var uahToUsd = rates.FirstOrDefault(r => !string.IsNullOrEmpty(r.Currency) && r.Currency.Equals("USD") && r.BaseCurrency.Equals("UAH"));
            Assert.NotNull(uahToUsd);

            var list = MapRatesToDbEntity(uahToEur, uahToUsd, date);

            await repository.AddCurrencyRates(list);
        }

        [Fact]
        public async Task GetCurrencyRatesForToday()
        {
            // Arrange
            var date = DateTime.Now;
            var repository = new CurrencyRateRepository();

            //Act
            var rates = await repository.GetCurrencyRates(date);

            // Assert
            Assert.NotNull(rates);
            Assert.True(rates.Count() > 0);
        }
        
        private IReadOnlyList<CurrencyRateInsertEntity> MapRatesToDbEntity(ExchangeRate uahToEur, ExchangeRate uahToUsd, DateTime date)
        {
            return new List<CurrencyRateInsertEntity>
            {
                new CurrencyRateInsertEntity
                {
                    BaseCurrency = uahToEur.BaseCurrency,
                    ToCurrency = uahToEur.Currency,
                    SaleRateNBU = uahToEur.SaleRateNB,
                    PurchaseRateNBU = uahToEur.PurchaseRateNB,
                    SaleRatePB = (decimal)uahToEur.SaleRate,
                    PurchaseRatePB = (decimal)uahToEur.PurchaseRate,
                    Date = date
                },
                new CurrencyRateInsertEntity
                {
                    BaseCurrency = uahToUsd.BaseCurrency,
                    ToCurrency = uahToUsd.Currency,
                    SaleRateNBU = uahToUsd.SaleRateNB,
                    PurchaseRateNBU = uahToUsd.PurchaseRateNB,
                    SaleRatePB = (decimal)uahToUsd.SaleRate,
                    PurchaseRatePB = (decimal)uahToUsd.PurchaseRate,
                    Date = date
                }
            };
        }

        private IPrivat24ApiClient CreatePrivat24ApiClient()
        {
            var logger = Mock.Of<ILogger<Privat24ApiClient>>();
            return new Privat24Factory(logger).CreatePublicClient();
        }
    }
}