using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Responses;
using FlightPrices.Skyscanner.WebAPI.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    /// <summary>
    ///     Class <c>SkyscannerClient</c> faciliates communication with the Skyscanner API.
    ///     <remarks>
    ///         Implements the <c>ISkyscanner</c> interface. 
    ///     </remarks> 
    /// </summary>
    public class SkyscannerClient : ISkyscannerClient
    {
        // Private fields
        private readonly string _apiKey;
        private readonly string _skyscannerBaseUrl;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SkyscannerClient> _logger;
        private readonly FlightPricesContext _context;

        /// <summary>
        ///     The <c>SkyscannerClient</c> class constructor.
        ///     <param name="key">The container for the Skyscanner API authentication key.</param>
        ///     <param name="url">The container for the Skyscanner Base URL, common to all requests.</param>
        ///     <param name="logger">An ILogger implementation instance.</param>
        ///     <param name="httpClientFactory">An IHttpClientFactory implementation instance.</param>
        ///     <param name="context">The DbContext for the FlightPrices database.</param>
        ///     <remarks>
        ///         Logs the values for the Skyscanner API Authentication Key and the Skyscanner Base URL.
        ///         The parameters of this method are injected via the .NET Dependency Injection mechanism.
        ///     </remarks>
        /// </summary>
        public SkyscannerClient(
            ApiKey key, 
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

            // Log config information
            _logger.LogInformation("Skyscanner API Key = {ApiKey}", _apiKey);
            _logger.LogInformation("Skyscanner Base Url = {BaseUrl}", 
                _skyscannerBaseUrl);
        }

        /// <summary>
        ///     Gets the collection of airports that can be used in a query.
        ///     <remarks>
        ///         This method eagerly loads the airports from the database.
        ///     </remarks>
        /// </summary>
        public IList<Airports> GetAirports()
        {
            return _context.Airports.ToList();
        }

        public async Task<IList<Flight>> GetRoundTripFlights(
            string origin, 
            string destination, 
            DateTime departureDate, 
            DateTime returnDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destination);

            // Build Url for request
            var url = SkyscannerUrlBuilder.BuildRoundTripFlightUrl(
                originAirport, 
                destinationAirport, 
                departureDate,
                returnDate);

            // Make HTTP Request to the Skyscanner API
            var response = await MakeHTTPRequest<BrowseQuotesResponse>(url);

            response.IsRoundTrip = true;

            // Build and return the Flight objects
            return new FlightBuilder(response).Build();
        }

        /// <summary>
        ///     Gets the real-time one-way flight prices based on search criteria.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destintation">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///     <see cref="GetOneWayFlights(string, string, DateTime)"/>
        ///     <see cref="SkyscannerUrlBuilder.BuildOneWayFlightUrl(Airports, Airports, DateTime)"/>>
        ///     <see cref="MakeHTTPRequest{T}(string)"/>
        ///     <remarks>This method makes an HTTP Request and is thus asynchronous.</remarks>
        /// </summary>
        public async Task<IList<Flight>> GetOneWayFlights(string origin, string destintation, DateTime departureDate)
        {
            // Get airport objects from names
            var originAirport = GetAirportFromAirportName(origin);
            var destinationAirport = GetAirportFromAirportName(destintation);

            // Build Url for request
            var url = SkyscannerUrlBuilder.BuildOneWayFlightUrl(originAirport, destinationAirport, departureDate);
            
            // Make HTTP Request to the Skyscanner API
            var response = await MakeHTTPRequest<BrowseQuotesResponse>(url);

            response.IsRoundTrip = false;

            // Build and return the Flight objects
            return new FlightBuilder(response).Build();
        }

        /// <summary>
        ///     Make an HTTP GET request to the Skyscanner API.
        ///     <param name="url">The url to append the Skyscanner base url.</param>
        ///     <see cref="GetHttpClient"></see>
        ///     <remarks>
        ///         This method makes an HTTP Request and is thus asynchronous. The HTTP client is served by the 
        ///         IHttpClientFactory instance. The Skyscanner responses are mapped to corresponding objects; 
        ///         these objects make it possible to make this algorithm generic.
        ///     </remarks>
        /// </summary>
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

        /// <summary>
        ///     Get the Airports objects from the database corresponding to the airportName
        ///     <param name="airportName">The name of the airport to retrieve.</param>
        ///     <exception cref="InvalidOperationException">
        ///         Thrown if the airport name does not match any Airports entry or if matches more than one.
        ///     </exception>
        ///     <remarks>
        ///         There should be exactly one airports entry for each name. 
        ///     </remarks>
        /// </summary>
        private Airports GetAirportFromAirportName(string airportName)
        {
            var airport = _context.Airports
                .Where(a => a.AirportName == airportName)
                .Single();

            return airport;
        }

        /// <summary>
        ///     Gets an HTTP Client from the IHttpClientFactory instance and configures it.
        /// </summary>
        private HttpClient GetHttpClient()
        {
            // Get the HttpClient instance
            var httpClient = _httpClientFactory.CreateClient();

            // Set configuration
            httpClient.BaseAddress = new Uri(_skyscannerBaseUrl);
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);

            return httpClient;
        }
    }
}
