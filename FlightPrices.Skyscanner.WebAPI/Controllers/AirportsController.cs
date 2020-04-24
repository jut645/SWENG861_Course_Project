using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        ///     The <c>AirportsController</c> class constructor.
        ///     <param name="client">An IClient interface implementation.</param>
        /// </summary>
        public AirportsController(IClient client)
        {
            _client = client;
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
            var payload = new { airports = _client.GetAirports() };

            return JsonConvert.SerializeObject(payload);    // Serialize the payload to JSON
        }
    }
}
