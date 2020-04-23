using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Responses.TravelAdvisor;
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

            return await PollCurrentSession(isRoundTrip: false);
        }

        public async Task<IList<Flight>> GetRoundTripFlights(string OriginAirportName, string DestintationAiportName, DateTime DepartureDate, DateTime ReturnDate)
        {
            await CreateSession(OriginAirportName, DestintationAiportName, DepartureDate, ReturnDate);

            Thread.Sleep(3000);    // Wait 3 seconds for poll to accumulate results on API side

            return await PollCurrentSession(isRoundTrip: true);
        }

        private async Task<IList<Flight>> PollCurrentSession(bool isRoundTrip)
        {
            string url = $"flights/poll?sid={_sessionId}&currency=USD&n=15&ns=NON_STOP%252CONE_STOP&so=PRICE&o=0";

            var jsonResponse = await MakeHTTPRequestRaw(url);

            var flights = !(isRoundTrip) ? GetQuoteDataFromJsonOneWay(jsonResponse) : 
                GetQuoteDataFromJsonRoundTrip(jsonResponse);

            return flights;
        }

        private IList<Flight> GetQuoteDataFromJsonOneWay(string json)
        {
            var flights = new List<Flight>();

            JToken token = JToken.Parse(json);
            JArray itineraries = (JArray)token.SelectToken("itineraries");
            JArray carriers = (JArray)token.SelectToken("carriers");

            foreach (var itinerary in itineraries)
            {
                var flight = new Flight();

                // Get best price
                int cheapestPrice = GetCheapestPriceFromItinerary(itinerary);
                flight.Cost = new Money(cheapestPrice, CurrencyType.UnitedStatesOfAmericaDollar);

                // Determine if the flight is direct
                bool isDirect = GetIsDirectFromItinerary(itinerary);
                flight.IsDirect = isDirect;

                // Get departure airline
                string departureAirline = GetDepartureAirlineFromItinerary(itinerary, carriers);
                flight.DepartureAirline = departureAirline;

                // Get departure airline 
                DateTime departureTakeoffTime = GetDepartureTimeFromItineary(itinerary);
                flight.DepartureTakeoffTime = departureTakeoffTime;

                flight.ReturnAirline = null;
                flight.ReturnTakeoffTime = null;

                flights.Add(flight);
            }

            return flights;
        }

        private IList<Flight> GetQuoteDataFromJsonRoundTrip(string json)
        {
            var flights = new List<Flight>();

            JToken token = JToken.Parse(json);
            JArray itineraries = (JArray)token.SelectToken("itineraries");
            JArray carriers = (JArray)token.SelectToken("carriers");

            foreach (var itinerary in itineraries)
            {
                var flight = new Flight();

                // Get best price
                int cheapestPrice = GetCheapestPriceFromItinerary(itinerary);
                flight.Cost = new Money(cheapestPrice, CurrencyType.UnitedStatesOfAmericaDollar);

                // Determine if the flight is direct
                bool isDirect = GetIsDirectFromItinerary(itinerary);
                flight.IsDirect = isDirect;

                // Get departure airline
                string departureAirline = GetDepartureAirlineFromItinerary(itinerary, carriers);
                flight.DepartureAirline = departureAirline;

                // Get departure airline 
                DateTime departureTakeoffTime = GetDepartureTimeFromItineary(itinerary);
                flight.DepartureTakeoffTime = departureTakeoffTime;

                // Get return airline
                string returnAirline = GetReturnAirlineFromItinerary(itinerary, carriers);
                flight.ReturnAirline = returnAirline;

                // Get return takeoff time
                DateTime returnTime = GetReturnTimeFromItineary(itinerary);
                flight.ReturnTakeoffTime = returnTime;

                flights.Add(flight);
            }

            return flights;
        }

        private DateTime GetDepartureTimeFromItineary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var dateTime = firstLeg.SelectToken("dd").ToString();

            return DateTime.Parse(dateTime);
        }

        private DateTime GetReturnTimeFromItineary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.Last();
            var dateTime = firstLeg.SelectToken("ad").ToString();

            return DateTime.Parse(dateTime);
        }

        private bool GetIsDirectFromItinerary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");

            return legs.Count == 1;
        }

        private string GetReturnAirlineFromItinerary(JToken itinerary, JArray carriers)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var lastLeg = legs.Last();
            var carrierId = lastLeg.SelectToken("o").ToString();

            foreach (var carrier in carriers)
            {
                var id = carrier.SelectToken("c").ToString();
                if (id == carrierId)
                {
                    return carrier.SelectToken("n").ToString();
                }
            }

            throw new ArgumentException($"Carrier {carrierId} not found.");
        }

        private string GetDepartureAirlineFromItinerary(JToken itinerary, JArray carriers)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var carrierId = firstLeg.SelectToken("o").ToString();

            foreach (var carrier in carriers)
            {
                var id = carrier.SelectToken("c").ToString();
                if (id == carrierId)
                {
                    return carrier.SelectToken("n").ToString();
                }
            }

            throw new ArgumentException($"Carrier {carrierId} not found.");
        }

        private int GetCheapestPriceFromItinerary(JToken itinerary)
        {
            var agents = itinerary.SelectToken("l");

            // Get cheapest price
            int cheapestPriceInDollars = int.MaxValue;
            foreach (var agent in agents)
            {
                var priceInDollars = GetIntegerPriceFromAgentToken(agent);

                if (priceInDollars < cheapestPriceInDollars)
                {
                    cheapestPriceInDollars = priceInDollars;
                }
            }

            return cheapestPriceInDollars;
        }

        private int GetIntegerPriceFromAgentToken(JToken agent)
        {
            var priceJson = agent.SelectToken("pr.dp").ToString();
            priceJson = priceJson.Replace("$", string.Empty);

            return Convert.ToInt32(priceJson);
        }

        private async Task CreateSession(string origin, string destination, DateTime DepartureDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            string url = $"flights/create-session?" +
                $"currency=USD&ta=1&c=0&d1={destinationAirport.IataCode}&o1={originAirport.IataCode}" +
                $"&dd1={DepartureDate.ToString("yyyy-MM-dd")}";

            var response = await MakeHTTPRequest<CreateSessionResponse>(url);

            _sessionId = response.search_params.sid;
        }

        private async Task CreateSession(string origin, string destination, DateTime DepartureDate, DateTime ReturnDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            /*
             * "https://tripadvisor1.p.rapidapi.com/flights/create-session?
             * dd2=2020-05-03&
             * currency=USD&
             * o2=ORD&
             * d2=SFO&
             * ta=1&
             * tc=11%252C5&
             * c=0&
             * d1=ORD&
             * o1=SFO&
             * dd1=2020-05-01"
             * 
             */

            string url = $"flights/create-session?" +
                $"dd2={ReturnDate.ToString("yyyy-MM-dd")}&" +
                $"currency=USD&" +
                $"o2={destinationAirport.IataCode}&" +
                $"d2={originAirport.IataCode}&" + 
                $"ta=1&" +
                $"c=0&" +
                $"d1={destinationAirport.IataCode}&" +
                $"o1={originAirport.IataCode}&" +
                $"dd1={DepartureDate.ToString("yyyy-MM-dd")}";

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
