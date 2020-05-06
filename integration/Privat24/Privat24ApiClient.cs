using Microsoft.Extensions.Logging;
using Privat24.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Privat24
{
    public class Privat24ApiClient : IPrivat24ApiClient
    {
        private readonly ILogger<Privat24ApiClient> _log;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public Privat24ApiClient(HttpClient httpClient, ILogger<Privat24ApiClient> log)
        {
            _httpClient = httpClient;
            _log = log;
            _jsonOptions.Converters.Add(new DateTimeConverterForUkrainianFormat());
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyRates(DateTime? date)
        {
            var dateTime = date.HasValue ? date.Value.Date : DateTime.Now.Date;
            var fmt = new CultureInfo("uk-UA").DateTimeFormat;
            
            var queryString = $"p24api/exchange_rates?json&date={dateTime.ToString(fmt.ShortDatePattern)}";
            _log.LogDebug($"API query string: {queryString}");

            using (var response = /*new HttpResponseMessage(HttpStatusCode.OK)*/await _httpClient.GetAsync(queryString))
            {
                if (response.Content != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (!response.Content.Headers.ContentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase)
                        || string.IsNullOrEmpty(responseString))
                    {
                        _log.LogInformation($"Wrong response. {responseString}{Environment.NewLine}");
                        return Enumerable.Empty<ExchangeRate>();
                    }
                    try
                    {
                        _log.LogDebug($"Got response: {responseString} {Environment.NewLine}");

                        var rates = JsonSerializer.Deserialize<CurrencyRatesResponse>(responseString, _jsonOptions);
                        return rates.ExchangeRates;
                    }
                    catch (JsonException ex)
                    {
                        _log.LogError(ex.Message);
                    }
                    catch (Exception e)
                    {
                        _log.LogError(e.Message);
                    }
                }
                _log.LogInformation($"Can't get response. Status: {response.StatusCode}");
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}