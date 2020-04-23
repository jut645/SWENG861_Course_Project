using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Responses.TravelAdvisor;
using FlightPrices.Skyscanner.WebAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    public class TravelAdvisorClient : ISkyscannerClient
    {
        private readonly FlightPricesContext _context;
        private readonly IHttpClientFactory _factory;
        private string _sessionId = string.Empty;

        public TravelAdvisorClient(FlightPricesContext context, IHttpClientFactory factory)
        {
            _context = context;
            _factory = factory;
        }

        public IList<Airports> GetAirports()
        {
            return _context.Airports.ToList();
        }

        public async Task<IList<Flight>> GetOneWayFlights(string OriginAirportName, string DestintationAiportName, DateTime DepartureDate)
        {
            await CreateSession(OriginAirportName, DestintationAiportName, DepartureDate);

            Thread.Sleep(3000);    // Wait 3 seconds for poll to accumulate results on API side

            return await PollCurrentSession();
        }

        public async Task<IList<Flight>> GetRoundTripFlights(string OriginAirportName, string DestintationAiportName, DateTime DepartureDate, DateTime ReturnDate)
        {
            await CreateSession(OriginAirportName, DestintationAiportName, DepartureDate, ReturnDate);

            Thread.Sleep(3000);    // Wait 3 seconds for poll to accumulate results on API side

            return await PollCurrentSession();
        }

        private async Task<IList<Flight>> PollCurrentSession()
        {
            string url = $"flights/poll?sid={_sessionId}&currency=USD&n=15&ns=NON_STOP%252CONE_STOP&so=PRICE&o=0";

            var jsonResponse = await MakeHTTPRequestRaw(url);

            var flightParser = new TripAdvisorPayloadParser(jsonResponse);

            return flightParser.GetQuoteData();
        }

        private async Task CreateSession(string origin, string destination, DateTime DepartureDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            string url = TripAdvisorUrlBuilder.BuildSessionUrl(originAirport.IataCode,
                destinationAirport.IataCode, DepartureDate);

            var response = await MakeHTTPRequest<CreateSessionResponse>(url);

            _sessionId = response.search_params.sid;
        }

        private async Task CreateSession(string origin, string destination, DateTime DepartureDate, DateTime ReturnDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            string url = TripAdvisorUrlBuilder.BuildSessionUrl(originAirport.IataCode,
                destinationAirport.IataCode,
                DepartureDate,
                ReturnDate);

            var response = await MakeHTTPRequest<CreateSessionResponse>(url);

            _sessionId = response.search_params.sid;
        }

        private async Task<string> MakeHTTPRequestRaw(string url)
        {
            // Get the HttpClient for the Skyscanner API
            var client = GetHttpClient();

            // Make the HTTP Get Request
            var result = await client.GetAsync(url);

            // Get content of response as JSON
            var content = result.Content;
            return content.ReadAsStringAsync().Result;
        }

        private async Task<T> MakeHTTPRequest<T>(string url)
        {
            // Get the HttpClient for the Skyscanner API
            var client = GetHttpClient();

            // Make the HTTP Get Request
            var result = await client.GetAsync(url);

            // Get content of response as JSON
            var content = result.Content;
            string jsonContent = content.ReadAsStringAsync().Result;

            // Deserialize the JSON into its corresponding response object
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        private Airports GetAirportFromAirportName(string airportName)
        {
            var airport = _context.Airports
                .Where(a => a.AirportName == airportName)
                .Single();

            return airport;
        }

        private HttpClient GetHttpClient()
        {
            // Get the HttpClient instance
            var httpClient = _factory.CreateClient();

            // Set configuration
            httpClient.BaseAddress = new Uri("https://tripadvisor1.p.rapidapi.com/");
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "1fc24be137msh3ce8b51b9bc9c79p12692fjsn9d45c95b5cea");
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "tripadvisor1.p.rapidapi.com");

            return httpClient;
        }
    }
}
