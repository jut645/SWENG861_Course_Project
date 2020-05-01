using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FlightPrices.Skyscanner.WebAPI.Controllers
{
    /// <summary>
    ///     Class <c>AirportsController</c> serves HTTP GET requests for the Airports instances.
    ///     <remarks>
    ///         This controller services requests for the route "/airports/".
    ///     </remarks>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AirportsController
    {
        private readonly IClient _client;
        private readonly ILogger<AirportsController> _logger;

        /// <summary>
        ///     The <c>AirportsController</c> class constructor.
        ///     <param name="client">An IClient interface implementation.</param>
        ///     <param name="logger">An ILogger implementation.</param>
        /// </summary>
        public AirportsController(IClient client, ILogger<AirportsController> logger)
        {
            _client = client;
            _logger = logger;
        }

        /// <summary>
        ///     Serves HTTP GET Requests for the list of Airports.
        ///     <returns>
        ///         The JSON payload corresponding to the list of airports.
        ///     </returns>
        ///     <remarks>
        ///         Services HTTP GET requests to the Url "/airports/"
        ///     </remarks>
        /// </summary>
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("Request received for Aiports data.");

            // Package payload for JSON serialization
            var payload = new { airports = _client.GetAirports() };

            _logger.LogInformation($"Returning data for {payload.airports.Count} airports.");

            return JsonConvert.SerializeObject(payload);    // Serialize the payload to JSON
        }
    }
}
