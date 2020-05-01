using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Responses.TravelAdvisor;
using FlightPrices.Skyscanner.WebAPI.Services;
using FlightPrices.WebAPI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    /// <summary>
    ///     Class <c>TripAdvisorClient</c> faciliates communication with the TripAdvisor API.
    ///     <remarks>
    ///         Implements the <c>IClient</c> interface. 
    ///     </remarks> 
    /// </summary>
    public class TripAdvisorClient : IClient
    {
        // Declare private fields
        private readonly FlightPricesContext _context;
        private readonly IHttpClientFactory _factory;
        private string _sessionId = string.Empty;

        /// <summary>
        ///     The <c>TripAdvisorClient</c> class constructor.
        ///     <param name="factory">An IHttpClientFactory implementation instance.</param>
        ///     <param name="context">The DbContext for the FlightPrices database.</param>
        /// </summary>
        public TripAdvisorClient(FlightPricesContext context, IHttpClientFactory factory)
        {
            _context = context;
            _factory = factory;
        }

        /// <summary>
        ///     Gets the collection of airports that can be used in a query.
        ///     <returns>
        ///         The<c>Airports</c> instances from the database.
        ///     </returns>
        ///     <remarks>
        ///         This method eagerly loads the airports from the database.
        ///     </remarks>
        /// </summary>
        public IList<Airports> GetAirports()
        {
            return _context.Airports.ToList();
        }

        /// <summary>
        ///     Gets the real-time one-way flight prices based on search criteria.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///      <returns>
        ///         The <c>Quote</c> instances for this search.
        ///     </returns>
        /// </summary>
        public async Task<IList<Quote>> GetOneWayFlights(string origin, string destination, DateTime departureDate)
        {
            await CreateSession(origin, destination, departureDate);    // Create session for this search

            return await PollCurrentSession();  // Poll the results
        }

        /// <summary>
        ///     Gets the real-time one-way flight prices based on search criteria.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///     <param name="returnDate">The date that the flight will depart to return to the origin.</param>
        ///      <returns>
        ///         The <c>Quote</c> instances for this search.
        ///     </returns>
        /// </summary>
        public async Task<IList<Quote>> GetRoundTripFlights(string origin, 
            string destination, 
            DateTime departureDate, 
            DateTime returnDate)
        {
            // Create session for this search
            await CreateSession(origin, destination, departureDate, returnDate);

            return await PollCurrentSession();  // Poll the results

        }

        /// <summary>
        ///     Polls the current session for quotes.
        ///      <returns>
        ///         The <c>Quote</c> instances for this search.
        ///     </returns>
        /// </summary>
        private async Task<IList<Quote>> PollCurrentSession()
        {
            // Flag to track if TripAdvisor API has completed compiling results
            bool complete = false;    

            // Container for the flight quotes
            var flightCollection = new List<Quote>();

            // Build request URL for current session
            string url = TripAdvisorUrlBuilder.BuildPollUrl(_sessionId);

            do
            {
                // Wait 1 second to avoid excessive API calls.
                // The free TripAdvisor API has a limit, so we need to avoid 
                // wasting calls.
                Thread.Sleep(1000);    

                // Query TripAdvisor API
                var jsonResponse = await MakeHTTPRequestRaw(url);

                // Build quote parser for json payload
                var flightParser = new TripAdvisorPayloadParser(jsonResponse);

                // Parse the payload into quote instances
                var flights = flightParser.GetQuoteData();

                // Add the batch of flight quotes
                flightCollection.AddRange(flights);

                // Check if the TripAdvisor API has signaled completion
                if (flightParser.IsComplete())
                {
                    complete = true;
                }

            } while (!complete);  // Iterate until TripAdvisor is done

            // Return the flight quotes, dropping duplicates
            return flightCollection
                .GroupBy(x => new 
                { 
                    x.Key, 
                    x.Cost.CurrencyType,
                    x.Cost.Value,
                    x.DepartureAirline,
                    x.DepartureArrivalTime,
                    x.DepartureStopCount,
                    x.DepartureTakeoffTime,
                    x.ReturnAirline,
                    x.ReturnArrivalTime,
                    x.ReturnStopCount,
                    x.ReturnTakeoffTime,
                    x.ReturnFlightNumber,
                    x.DepartureFlightNumber
                })
                .Select(x => x.First())
                .ToList();
        }

        /// <summary>
        ///     Creates a session for the search parameters.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///      <returns>
        ///         An awaitable Task instance.
        ///     </returns>
        /// </summary>
        private async Task CreateSession(string origin, string destination, DateTime departureDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            // Build Url to request the creation of the session
            string url = TripAdvisorUrlBuilder.BuildSessionUrl(
                originAirport.IataCode,
                destinationAirport.IataCode, 
                departureDate);

            // Send request to create session
            var response = await MakeHTTPRequest<CreateSessionResponse>(url);

            // Extract session Id for later polling
            _sessionId = response.search_params.sid;
        }

        /// <summary>
        ///     Creates a session for the search parameters.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///     <param name="returnDate">The date that the flight will depart to return to the origin.</param>
        ///      <returns>
        ///         An awaitable Task instance.
        ///     </returns>
        /// </summary>
        private async Task CreateSession(string origin, 
            string destination, 
            DateTime departureDate, 
            DateTime returnDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            // Build Url to request the creation of the session
            string url = TripAdvisorUrlBuilder.BuildSessionUrl(originAirport.IataCode,
                destinationAirport.IataCode,
                departureDate,
                returnDate);

            // Send request to create session
            var response = await MakeHTTPRequest<CreateSessionResponse>(url);

            // Extract session Id for later polling
            _sessionId = response.search_params.sid;
        }

        /// <summary>
        ///     Makes an Http Request and returns the response content as raw json.
        ///     <param name="url">The url for the Http Request.</param>
        ///      <returns>
        ///         The string corresponding the JSON response content.
        ///     </returns>
        /// </summary>
        private async Task<string> MakeHTTPRequestRaw(string url)
        {
            // Get the HttpClient for the Skyscanner API
            var client = GetHttpClient();
            HttpResponseMessage result = null;

            try 
            { 
                // Make the HTTP Get Request
                result = await client.GetAsync(url);
            }
            catch (HttpRequestException)    // Errors such as internet loss caught here
            {
                throw new IClientApiException(System.Net.HttpStatusCode.ServiceUnavailable);
            }

            // If the request was unsuccessful, throw exception indicating the failure
            if (!result.IsSuccessStatusCode)
            {
                throw new IClientApiException(result.StatusCode);
            }

            // Get content of response as JSON
            var content = result.Content;
            return content.ReadAsStringAsync().Result;
        }

        /// <summary>
        ///     Makes an Http Request and returns the response content as an instance of an object.
        ///     <param name="url">The url for the Http Request.</param>
        ///      <returns>
        ///         The object corresponding to the response content payload.
        ///     </returns>
        /// </summary>
        private async Task<T> MakeHTTPRequest<T>(string url)
        {
            // Get the HttpClient for the Skyscanner API
            var client = GetHttpClient();
            HttpResponseMessage result = null;

            try
            {
                // Make the HTTP Get Request
                result = await client.GetAsync(url);
            }
            catch (HttpRequestException)    // Errors such as internet loss caught here 
            {
                throw new IClientApiException(System.Net.HttpStatusCode.ServiceUnavailable);
            }

            // If the request is unsuccessful, throw exception indicating failure
            if (!result.IsSuccessStatusCode)
            {
                throw new IClientApiException(result.StatusCode);
            }

            // Get content of response as JSON
            var content = result.Content;
            string jsonContent = content.ReadAsStringAsync().Result;

            // Deserialize the JSON into its corresponding response object
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        /// <summary>
        ///     Gets the <c>Airports</c> instance corresponding to the airport name.
        ///     <param name="airportName">the name of the airport.</param>
        ///      <returns>
        ///         The <c>Airports</c> instance corresponding to the airport name.
        ///     </returns>
        /// </summary>
        private Airports GetAirportFromAirportName(string airportName)
        {
            var airport = _context.Airports
                .Where(a => a.AirportName == airportName)
                .Single();

            return airport;
        }

        /// <summary>
        ///     Helper method to get a configured HttpClient from the IHttpFactory instance.
        ///      <returns>
        ///         The configured HttpClient.
        ///     </returns>
        /// </summary>
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
