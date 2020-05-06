using Microsoft.Extensions.Logging;
using Moq;
using Privat24;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Privat24.Tests
{
    public class Privat24ApiClientTests
    {
        [Fact]
        public async Task GetCurrencyRatesForToday()
        {
            // Arrange
            var apiClient = CreatePrivat24ApiClient();
            var date = DateTime.Now;

            //Act
            var rates = await apiClient.GetCurrencyRates(date);

            // Assert
            Assert.NotNull(rates);
            var uahToEur = rates.FirstOrDefault(r => !string.IsNullOrEmpty(r.Currency) && r.Currency.Equals("EUR") && r.BaseCurrency.Equals("UAH"));
            Assert.NotNull(uahToEur);
            Assert.True(uahToEur.SaleRate > 29);
        }

        private IPrivat24ApiClient CreatePrivat24ApiClient()
        {
            var logger = Mock.Of<ILogger<Privat24ApiClient>>();
            var factory = new Privat24Factory(logger);
            return factory.CreatePublicClient();
        }
    }
}