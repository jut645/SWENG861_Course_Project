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
            var url = BuildUrl(OriginAirportName, DestintationAiportName, DepartureDate);
            var response = MakeHTTPRequest<BrowseQuotesResponse>(url);

            return new List<Flight>();
        }

        private string BuildUrl(string OriginAirportName, string DestinationAiportName, DateTime DepartureDate)
        {
            var originAirport = GetAirportFromAirportName(OriginAirportName);
            var destinationAirport = GetAirportFromAirportName(DestinationAiportName);
            
            var originId = originAirport.SkyscannerPlaceId;
            var destinationId = destinationAirport.SkyscannerPlaceId;
            var outboundDate = DepartureDate.ToString("yyyy-MM-dd");

            return $"apiservices/browseroutes/v1.0/US/USD/en-US/{originId}/{destinationId}/{outboundDate}";


        }

        private async Task<T> MakeHTTPRequest<T>(string url)
        {
            var client = GetHttpClient();
            var result = await client.GetAsync(url);
            var content = result.Content;

            string jsonContent = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        private Airports GetAirportFromAirportName(string AirportName)
        {
            var airport = _context.Airports
                .Where(a => a.AirportName == AirportName)
                .Single();

            return airport;
        }

        private IList<Flight> BuildFlightsFromBrowseQuotesResponse(BrowseQuotesResponse response)
        {
            return new List<Flight>();

        }

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_skyscannerBaseUrl);
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);

            return httpClient;
        }
    }
}
