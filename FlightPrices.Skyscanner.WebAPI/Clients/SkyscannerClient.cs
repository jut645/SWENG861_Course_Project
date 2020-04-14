using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    public class SkyscannerClient : ISkyscannerClient
    {
        private readonly string _apiKey;
        private readonly string _skyscannerBaseUrl;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SkyscannerClient> _logger;
        private readonly FlightPricesContext _context;

        public SkyscannerClient(ApiKey key, 
            SkyscannerBaseUrl url, 
            ILogger<SkyscannerClient> logger, 
            IHttpClientFactory httpClientFactory,
            FlightPricesContext context) 
        {
            _apiKey = key.Value;
            _skyscannerBaseUrl = url.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _context = context;

            _logger.LogInformation("Skyscanner API Key = {ApiKey}", _apiKey);
            _logger.LogInformation("Skyscanner Base Url = {BaseUrl}", 
                _skyscannerBaseUrl);
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_skyscannerBaseUrl);
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);

            return httpClient;
        }

        public async void QueryLocation(string AirportName)
        {
            var httpClient = GetHttpClient();

            var result = await httpClient.GetAsync($"apiservices/autosuggest/v1.0/USA/USD/en-US/?query={AirportName}");
            var content = result.Content;

            string jsonContent = content.ReadAsStringAsync().Result;
            QueryLocationResponse locationResponse = JsonConvert.DeserializeObject<QueryLocationResponse>(jsonContent);

        }

        public async Task<IList<Currency>> GetCurrencies()
        {
            var httpClient = GetHttpClient();

            var result = await httpClient.GetAsync("apiservices/reference/v1.0/currencies");
            var content = result.Content;

            string jsonContent = content.ReadAsStringAsync().Result;
            CurrenciesResponse currenciesResponse = JsonConvert.DeserializeObject<CurrenciesResponse>(jsonContent);

            return currenciesResponse.Currencies;
        }

        public IList<Airports> GetAirports()
        {
            return _context.Airports.ToList();
        }

        public Task<IList<Flight>> GetRoundTripFlights(string OriginAirportName, string DestintationAiportName, DateTime DepartureDate, DateTime ReturnDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Flight>> GetOneWayFlights(string OriginAirportName, string DestintationAiportName, DateTime DepartureDate)
        {
            var client = GetHttpClient();

            var url = "apiservices/browseroutes/v1.0/US/USD/en-US/SFO-sky/ORD-sky/2020-04-15?inboundpartialdate=2019-12-01";

            var result = await client.GetAsync(url);
            var content = result.Content;

            string jsonContent = content.ReadAsStringAsync().Result;
            BrowseQuotesResponse quoteResponse = JsonConvert.DeserializeObject<BrowseQuotesResponse>(jsonContent);

            return new List<Flight>();
        }

        private IList<Flight> BuildFlightsFromBrowseQuotesResponse(BrowseQuotesResponse response)
        {
            return new List<Flight>();

        }
    }
}
