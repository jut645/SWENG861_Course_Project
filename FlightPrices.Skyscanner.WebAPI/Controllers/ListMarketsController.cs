using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlightPrices.Skyscanner.WebAPI.Clients;
using FlightPrices.Skyscanner.WebAPI.Responses;

namespace FlightPrices.Skyscanner.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListMarketsController : ControllerBase
    {
        private readonly ILogger<ListMarketsController> _logger;
        private readonly SkyscannerClient<ListMarketResponse> _client;

        public ListMarketsController(ILogger<ListMarketsController> logger,
            SkyscannerClient<ListMarketResponse> listMarketClient)
        {
            _logger = logger;
            _client= listMarketClient;
        }

        [HttpGet]
        public async Task<ListMarketResponse> Get()
        {
            return await _client.GetAll();
        }
    }
}
