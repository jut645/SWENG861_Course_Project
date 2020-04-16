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
        [Route("[controller]/direct")]
        public string Get(string OriginAirportName, string DestinationAirportName, DateTime DepartureDate)
        {
            _client.GetOneWayFlights(OriginAirportName, DestinationAirportName, DepartureDate);

            return "";
        }
    }
}
