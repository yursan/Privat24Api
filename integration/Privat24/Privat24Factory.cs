using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Privat24
{
    public class Privat24Factory : IPrivat24Factory
    {
        private readonly ILogger<Privat24ApiClient> _log;
        private const string _pb24BaseUrl = "https://api.privatbank.ua/p24api";

        public Privat24Factory(ILogger<Privat24ApiClient> log)
        {
            _log = log;
        }

        public IPrivat24ApiClient CreatePublicClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_pb24BaseUrl)
            };
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", oauthSignature);

            return new Privat24ApiClient(httpClient, _log);
        }
    }
}