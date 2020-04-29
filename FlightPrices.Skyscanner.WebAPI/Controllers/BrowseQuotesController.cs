using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Controllers
{
    /// <summary>
    ///     Class <c>BrowseQuotesController</c> serves HTTP GET requests for flight quotes.
    ///     <remarks>
    ///         This controller services requests for the route "/browsequotes/".
    ///     </remarks>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BrowseQuotesController : Controller
    {
        private readonly IClient _client;
        private readonly ILogger<BrowseQuotesController> _logger;

        /// <summary>
        ///     The <c>BrowseQuotesController</c> class constructor.
        ///     <param name="client">An IClient interface implementation.</param>
        /// </summary>
        public BrowseQuotesController(IClient client, ILogger<BrowseQuotesController> logger)
        {
            _client = client;
            _logger = logger;
        }

        /// <summary>
        ///     Services HTTP GET Requests for OneWay flight quotes.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///      <returns>
        ///         The JSON payload corresponding to the one way flight quotes.
        ///     </returns>
        ///      <remarks>
        ///         Example Url:
        ///         /browsequotes/oneWay?origin=Jacksonville+International+Airport&destination=San+Francisco+
        ///         International+Airport&departureDate=2020-05-1
        ///     </remarks>
        /// </summary>
        [HttpGet]
        [Route("oneWay")]
        public async Task<IActionResult> GetOneWay(string origin, string destination, DateTime departureDate)
        {
            _logger.LogInformation("Request received for one way flight quotes.");
            _logger.LogInformation($"Origin Airport: {origin}");
            _logger.LogInformation($"Destination Airport: {destination}");
            _logger.LogInformation($"Departure Date: {departureDate}");

            IList<Quote> quotes = null;
            try
            {
                // Get quotes from client
                quotes = await _client.GetOneWayFlights(origin, destination, departureDate);
            }
            catch (IClientApiException ex)
            {
                return StatusCode((int)ex.StatusCode);
            }

            var payload = new { quotes = quotes };

            return Ok(JsonConvert.SerializeObject(payload));   // Serialize payload to JSON
        }

        /// <summary>
        ///     Services HTTP GET Requests for OneWay flight quotes.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///     <param name="returnDate">The date that the flight will depart to return to the origin.</param>
        ///      <returns>
        ///         The JSON payload corresponding to the one way flight quotes.
        ///     </returns>
        ///     <remarks>
        ///         Example Url:
        ///         /browsequotes/roundTrip?origin=Jacksonville+International+Airport&destination=San+Francisco+
        ///         International+Airport&departureDate=2020-05-1&returnDate=2020-05-04
        ///     </remarks>
        /// </summary>
        [HttpGet]
        [Route("roundTrip")]
        public async Task<IActionResult> GetRoundTrip(string origin, 
            string destination, 
            DateTime departureDate, 
            DateTime returnDate)
        {
            IList<Quote> quotes = null;

            try
            {   // Get quotes from client
                quotes = await _client.GetRoundTripFlights(origin, destination, departureDate, returnDate);
            }
            catch(IClientApiException ex)
            {
                return StatusCode((int)ex.StatusCode);
            }

            var payload = new { quotes = quotes };

            return Ok(JsonConvert.SerializeObject(payload));    // Serialize payload to JSON
        }
    }
}
