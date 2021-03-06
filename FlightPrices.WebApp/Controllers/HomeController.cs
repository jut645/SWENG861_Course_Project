﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FlightPrices.WebApp.ViewModels.Home;
using System.Net.Http;
using FlightPrices.Skyscanner.WebAPI.Models;
using Newtonsoft.Json;
using FlightPrices.WebApp.Payloads;
using FlightPrices.WebApp.Exceptions;
using FlightPrices.WebApp.ViewModels.ErrorPage;

namespace FlightPrices.WebApp.Controllers
{
    /// <summary>
    ///     Class <c>HomeController</c> handles events that come from the Home Page UI.
    ///     <remarks>
    ///         Extends the Controller base class.
    ///     </remarks> 
    /// </summary>
    public class HomeController : Controller
    {
        // Private fields
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        ///     The <c>HomeController</c> class constructor.
        ///     <param name="logger">An ILogger implementation instance.</param>
        ///     <param name="factory">An IHttpClientFactory implementation instance.</param>
        ///     <remarks>
        ///         The ILogger and IHttpClientFactory implementations are injected via the .NET Dependency Injection
        ///         framework on a typical program execution.
        ///     </remarks>
        /// </summary>
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _httpClientFactory = factory;
        }

        /// <summary>
        ///     The entry point for the Home page UI.
        ///     <return>
        ///         The Task corresponding to the ViewResult for the Home page UI.
        ///     </return>
        ///     <remarks>
        ///         Asynchronous because it makes an HTTP Get request via an HttpClient instance to get the list of
        ///         valid airport names.
        ///     </remarks>
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("WebApp requesting Airport data from WebAPI");

                // Get valid airport names
                var airports = await GetAirports();

                _logger.LogInformation($"WebAPI responded with data for {airports.Count} airports.");

                // Initialize the Home page view model with the valid airports
                var viewModel = new HomeSearchViewModel
                {
                    Airports = airports
                };

                return View(viewModel);
            }
            catch (HttpRequestException)    // This would indicate a failure such as internet loss
            {
                _logger.LogError("WebApp was unable to get airport data from WebAPI");

                // Indicate that the service is unavailable
                var viewModel = new HttpErrorPageViewModel(System.Net.HttpStatusCode.ServiceUnavailable);
                return View("HttpErrorPage", viewModel);
            }
        }

        /// <summary>
        ///     Performs a query for flight prices based on the form model submitted.
        ///     <param name="searchForm">The view model containing the expected input into the system.</param>
        ///     <returns>
        ///         The task instance corresponding to the ViewResult for the Flights view if the form is valid. 
        ///         If there is a validation error, the ViewResult will be for the Home page with the validation
        ///         error data.
        ///     </returns>
        ///     <remarks>
        ///         Asynchronous because it makes an HTTP Get request via an HttpClient instance to get the list of
        ///         matching flight prices.
        ///     </remarks>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Search(HomeSearchViewModel searchForm)
        {
            _logger.LogInformation($"Received form from the user:" + Environment.NewLine + 
                $"Destination Airport: {searchForm.DestinationAirport}" + Environment.NewLine +
                $"Origin Airport: {searchForm.OriginAirport}" + Environment.NewLine +
                $"Return Date: {searchForm.ReturnDate}" + Environment.NewLine +
                $"Takeoff Date: {searchForm.TakeOffDate}" + Environment.NewLine +
                $"Round Trip: {searchForm.IsRoundTrip}");

            _logger.LogInformation("WebApp requesting Airport data from WebAPI");

            // Get the list of airports for validation
            var airports = await GetAirports();

            _logger.LogInformation($"WebAPI responded with data for {airports.Count} airports.");

            // Make sure the Origin Airport name is valid
            if (!airports.Select(a => a.AirportName).Contains(searchForm.OriginAirport))
            {
                ModelState.AddModelError("OriginAirport", "Invalid airport name");
            }

            // Make sure the Destination Airport name is valid
            if (!airports.Select(a => a.AirportName).Contains(searchForm.DestinationAirport))
            {
                ModelState.AddModelError("DestinationAirport", "Invalid airport name");
            }

            // Perform other validation. See the data annotations on the HomeSearchViewModel class for more.
            // If the validation fails, return ViewResult for the Home page with error information
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Form validation failed.");

                searchForm.Airports = airports;
                return View("Index", searchForm);
            }

            // Send the appropriate HTTP Get request or return to the home page if the form is in an invalid state.
            if (!searchForm.IsRoundTrip)
            {
                _logger.LogInformation("WebApp querying WebAPI for one-way flight prices.");

                try
                {
                    return await GetOneWayFlights(searchForm);
                }
                catch (FlightQuoteApiException ex)  // This indicates a failure on the WebAPI side
                {
                    var viewModel = new HttpErrorPageViewModel(ex.StatusCode);
                    return View("HttpErrorPage", viewModel);
                }
                catch (HttpRequestException)       // This indicates a failure such as internet loss
                {
                    var viewModel = new HttpErrorPageViewModel(System.Net.HttpStatusCode.ServiceUnavailable);
                    return View("HttpErrorPage", viewModel);
                }
            }

            _logger.LogInformation("WebApp querying WebAPI for round-trip flight prices.");

            try
            {
                // We've passed all validation and the only option remaining is roundtrip flights
                return await GetRoundTripFlights(searchForm);
            }
            catch (FlightQuoteApiException ex)  // This indicates a failure on the WebAPI side
            {
                var viewModel = new HttpErrorPageViewModel(ex.StatusCode);
                return View("HttpErrorPage", viewModel);
            }
            catch (HttpRequestException)       // This indicates a failure such as internet loss
            {
                var viewModel = new HttpErrorPageViewModel(System.Net.HttpStatusCode.ServiceUnavailable);
                return View("HttpErrorPage", viewModel);
            }

        }

        /// <summary>
        ///     Helper methods to get the airport objects via the HttpClient.
        ///     <returns>
        ///         The task instance corresponding to the collection of airports.
        ///     </returns>
        ///     <remarks>
        ///         Asynchronous because it makes an HTTP Get request via an HttpClient instance to get the list of
        ///         valid airports.
        ///     </remarks>
        /// </summary>
        public async Task<IList<Airports>> GetAirports()
        {
            // Get the airports data from the WebAPI project
            var airportsPayload = await MakeHTTPGetRequest<AirportsPayload>("airports");

            return airportsPayload.Airports;
        }

        /// <summary>
        ///     Make an Http GET request for one way flights corresponding to a search form.
        /// <param name="searchForm">The view model containing the expected input into the system.</param>
        ///     <returns>
        ///         The task corresponding to the ViewResult for the one-way flight prices.
        ///     </returns>
        ///     <remarks>
        ///         Asynchronous because it makes an HTTP Get request via an HttpClient instance to get the list of
        ///         one-way flight prices.
        ///     </remarks>
        /// </summary>
        private async Task<IActionResult> GetOneWayFlights(HomeSearchViewModel searchForm)
        {
            // Construct the url based on the form
            string url = $"https://localhost:44320/browsequotes/oneWay?" +
                $"origin={searchForm.OriginAirport}" +
                $"&destination={searchForm.DestinationAirport}" +
                $"&departureDate={searchForm.TakeOffDate.Value.ToString("yyyy-MM-dd")}";

            // Make the HTTP Get request
            var quotesResponse = await MakeHTTPGetRequest<BrowseQuotesPayload>(url);

            // Build the view model for the Flights view
            var viewModel = new HomePageQuotesViewModel
            {
                Quotes = quotesResponse.Quotes,
                IsRoundTrip = false,
                DepartureDate = searchForm.TakeOffDate.Value,
                DestinationAirport = searchForm.DestinationAirport,
                OriginAirport = searchForm.OriginAirport
            };

            return View("Flights", viewModel);
        }

        /// <summary>
        ///     Make an Http GET request for round trip flights corresponding to a search form.
        /// <param name="searchForm">The view model containing the expected input into the system.</param>
        ///     <returns>
        ///         The task corresponding to the ViewResult for the round-trip flight prices.
        ///     </returns>
        ///     <remarks>
        ///         Asynchronous because it makes an HTTP Get request via an HttpClient instance to get the list of
        ///         round-trip flight prices.
        ///     </remarks>
        /// </summary>
        private async Task<IActionResult> GetRoundTripFlights(HomeSearchViewModel searchForm)
        {
            // Build the url based on the search form
            string url = $"https://localhost:44320/browsequotes/roundTrip?" +
                $"origin={searchForm.OriginAirport}" +
                $"&destination={searchForm.DestinationAirport}" +
                $"&departureDate={searchForm.TakeOffDate.Value.ToString("yyyy-MM-dd")}" +
                $"&returnDate={searchForm.ReturnDate.Value.ToString("yyyy-MM-dd")}";

            // Make the HTTP Get request for round-trip prices
            var quotesResponse = await MakeHTTPGetRequest<BrowseQuotesPayload>(url);

            // Build the view model for the Flights view
            var viewModel = new HomePageQuotesViewModel
            {
                Quotes = quotesResponse.Quotes,
                IsRoundTrip = true,
                DepartureDate = searchForm.TakeOffDate.Value,
                ReturnDate = searchForm.ReturnDate.Value,
                DestinationAirport = searchForm.DestinationAirport,
                OriginAirport = searchForm.OriginAirport
            };

            return View("Flights", viewModel);
        }

        /// <summary>
        ///     Helper method to get an HttpClient from the IHttpClientFactory instance with the base url configs.
        ///     <returns>
        ///         The HttpClient with the base URL configured.
        ///     </returns>
        /// </summary>
        private HttpClient GetHttpClient()
        {
            // Create the HttpClient from the factory
            var httpClient = _httpClientFactory.CreateClient();

            // Set the base Url 
            httpClient.BaseAddress = new Uri("https://localhost:44320/");

            return httpClient;
        }

        /// <summary>
        ///     Helper method to make an HTTP get request based on a generic payload structure.
        ///     <param name="url">The url to append to the base url for the HTTP request.</param>
        ///     <returns>
        ///         The task corresponding to the HTTP Get request payload.
        ///     </returns>
        ///     <remarks>
        ///     An example url is 'airports'. This would be appends to the httpClient base url.
        ///     </remarks>
        /// </summary>
        private async Task<T> MakeHTTPGetRequest<T>(string url)
        {
            // Get an HttpClient instance
            var httpClient = GetHttpClient();
            
            // Make the HTTP GET request
            var response = await httpClient.GetAsync(url);

            // Test if the response was successful
            // If not successful, throw an exception with the status code of the response
            if (!response.IsSuccessStatusCode)
            {
                throw new FlightQuoteApiException(response.StatusCode);
            }

            // Get the JSON content
            var content = response.Content;
            var json = content.ReadAsStringAsync().Result;

            // Deserialize the JSON into the generic type
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
