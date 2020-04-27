using Privat24.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Privat24
{
    public class Privat24ApiClient : IPrivat24ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public Privat24ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyRates(DateTime? date)
        {
            var dateTime = date.HasValue ? date.Value.Date : DateTime.Now.Date;
            var fmt = new CultureInfo("uk-UA").DateTimeFormat;
            
            var queryString = $"p24api/exchange_rates?json&date={dateTime.ToString(fmt.ShortDatePattern)}";
            using (var response = await _httpClient.GetAsync(queryString))
            {
                if (response.Content != null)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (!response.Content.Headers.ContentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        //log error
                        return Enumerable.Empty<ExchangeRate>();
                    }
                    try
                    {
                        _jsonOptions.Converters.Add(new DateTimeConverterForUkrainianFormat());

                        var rates = JsonSerializer.Deserialize<CurrencyRatesResponse>(responseString, _jsonOptions);
                        return rates.ExchangeRates;
                    }
                    catch (System.Text.Json.JsonException ex)
                    {
                        //to do...
                        //log error
                    }
                }
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}