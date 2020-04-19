using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrowseQuotesController
    {
        private readonly ILogger<BrowseQuotesController> _logger;
        private readonly ISkyscannerClient _client;

        public BrowseQuotesController(ILogger<BrowseQuotesController> logger,
            ISkyscannerClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        [Route("oneWay")]
        public async Task<string> PostOneWay(string OriginAirportName, string DestinationAirportName, DateTime DepartureDate)
        {
            var quotes = await _client.GetOneWayFlights(OriginAirportName, DestinationAirportName, DepartureDate);
            var payload = new { quotes = quotes };

            return JsonConvert.SerializeObject(payload);
        }

        [HttpGet]
        [Route("roundTrip")]
        public async Task<string> PostRoundTrip(string OriginAirportName, string DestinationAirportName, DateTime DepartureDate, DateTime ReturnDate)
        {
            var quotes = await _client.GetRoundTripFlights(OriginAirportName, DestinationAirportName, DepartureDate, ReturnDate);
            var payload = new { quotes = quotes };

            return JsonConvert.SerializeObject(payload);
        }
    }
}
