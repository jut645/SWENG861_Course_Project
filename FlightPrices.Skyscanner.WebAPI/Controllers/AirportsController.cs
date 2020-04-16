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
    public class AirportsController
    {
        private readonly ILogger<AirportsController> _logger;
        private readonly ISkyscannerClient _client;

        public AirportsController(ILogger<AirportsController> logger,
            ISkyscannerClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public string Get()
        {
            var payload = new { airports = _client.GetAirports() };

            return JsonConvert.SerializeObject(payload);
        }
    }
}
