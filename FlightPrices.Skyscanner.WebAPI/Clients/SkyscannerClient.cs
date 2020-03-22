using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    public class SkyscannerClient<T>
    {
        private readonly string _apiKey;
        private readonly SkyscannerUrls _skyscannerUrls;

        public SkyscannerClient(ApiKey key, SkyscannerUrls urls)
        {
            _apiKey = key.Value;
            _skyscannerUrls = urls;
        }

        private Type ClientType => typeof(T);

        public string Url => _skyscannerUrls.SkyscannerBaseUrl + _skyscannerUrls.Urls[ClientType];

        public async Task<T> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-RapidAPI-Host", "skyscanner-skyscanner-flight-search-v1.p.rapidapi.com");
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-RapidAPI-Key", _apiKey);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                var json = await client.GetStringAsync(Url);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
