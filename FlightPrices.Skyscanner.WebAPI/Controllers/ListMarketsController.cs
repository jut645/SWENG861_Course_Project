using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlightPrices.Skyscanner.WebAPI.Clients;
using FlightPrices.Skyscanner.WebAPI.Responses;
using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;

namespace FlightPrices.Skyscanner.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListMarketsController : ControllerBase
    {
        private readonly ILogger<ListMarketsController> _logger;
        private readonly ISkyscannerClient _client;
        //private readonly FlightPricesContext _context;

        public ListMarketsController(ILogger<ListMarketsController> logger,
            ISkyscannerClient listMarketClient)
            //FlightPricesContext context)
        {
            _logger = logger;
            _client= listMarketClient;
            //_context = context;
        }

        [HttpGet]
        public async Task<IList<Currency>> Get()
        {
            return await _client.GetCurrencies();
        }
    }
}
