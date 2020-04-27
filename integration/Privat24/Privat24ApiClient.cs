using Privat24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Privat24
{
    public class Privat24ApiClient : IPrivat24ApiClient
    {
        private readonly HttpClient _httpClient;

        public Privat24ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyRates(DateTime? date)
        {
            var dateTime = date.HasValue ? date.Value.ToLocalTime() : DateTime.Now.ToLocalTime();
            using (var response = await _httpClient.GetAsync($"exchange_rates?json&date={dateTime}"))
            {
                if (response.Content != null)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var rates = JsonSerializer.Deserialize<CurrencyRatesResponse>(responseString);
                    return rates.ExchangeRates;
                }
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}