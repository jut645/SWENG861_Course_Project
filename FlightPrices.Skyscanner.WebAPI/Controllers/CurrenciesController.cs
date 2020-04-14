using FlightPrices.Skyscanner.WebAPI.Clients;
using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController
    {
        private readonly ILogger<CurrenciesController> _logger;
        private readonly ISkyscannerClient _client;

        public CurrenciesController(ILogger<CurrenciesController> logger,
            ISkyscannerClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return "Hello Currencies";
        }
    }
}
