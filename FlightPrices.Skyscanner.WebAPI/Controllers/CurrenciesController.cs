using FlightPrices.Skyscanner.WebAPI.Clients;
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
        private readonly SkyscannerClient<CurrenciesResponse> _client;

        public CurrenciesController(ILogger<CurrenciesController> logger,
            SkyscannerClient<CurrenciesResponse> client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task<CurrenciesResponse> Get()
        {
            return await _client.GetAll();
        }
    }
}
