using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
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
    public class BrowseQuotesController
    {
        private readonly ILogger<CurrenciesController> _logger;
        private readonly ISkyscannerClient _client;

        public BrowseQuotesController(ILogger<CurrenciesController> logger,
            ISkyscannerClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public IList<Airports> Get()
        {
            return _client.GetAirports();
        }
    }
}
