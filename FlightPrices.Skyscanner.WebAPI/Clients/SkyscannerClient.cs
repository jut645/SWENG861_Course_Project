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
            var response = await MakeHTTPRequest<BrowseQuotesResponse>(url);

            return BuildFlightsFromBrowseQuotesResponse(response);
        }

        private string BuildUrl(string OriginAirportName, string DestinationAiportName, DateTime DepartureDate)
        {
            var originAirport = GetAirportFromAirportName(OriginAirportName);
            var destinationAirport = GetAirportFromAirportName(DestinationAiportName);
            
            var originId = originAirport.SkyscannerPlaceId;
            var destinationId = destinationAirport.SkyscannerPlaceId;
            var outboundDate = DepartureDate.ToString("yyyy-MM-dd");

            return $"apiservices/browsequotes/v1.0/US/USD/en-US/{originId}/{destinationId}/{outboundDate}";


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
            var carriers = response.Carriers.ToDictionary(c => c.CarrierId, c => c.Name);
            var flights = new List<Flight>();
            
            foreach (var quote in response.Quotes)
            {
                var carrierId = quote.OutboundLeg.CarrierIds.Single();
                var airline = carriers[carrierId];
                var cost = new Money(Convert.ToDecimal(quote.MinPrice), 
                    CurrencyType.UnitedStatesOfAmericaDollar);
                var departureTakeoffTime = quote.OutboundLeg.DepartureDate;

                var flight = new Flight
                {
                    Airline = airline,
                    Cost = cost,
                    NumberOfStops = 0,
                    ReturnTakeoffTime = null,
                    ReturnArrivalTime = null,
                    DepartureTakeoffTime = departureTakeoffTime
                };

                flights.Add(flight);
            }

            return flights;

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
