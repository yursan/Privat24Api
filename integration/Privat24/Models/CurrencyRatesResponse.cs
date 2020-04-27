using System;
using System.Text.Json.Serialization;

namespace Privat24.Models
{
    public class CurrencyRatesResponse
    {
        public DateTime Date { get; set; }
        public string Bank { get; set; }
        public int BaseCurrency { get; set; }
        public string BaseCurrencyLit { get; set; }

        [JsonPropertyName("exchangeRate")]
        public ExchangeRate[] ExchangeRates { get; set; }
    }
}