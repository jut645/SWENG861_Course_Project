using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Responses
{
    public class ListMarketResponse
    {
        public IList<ListMarket> Countries { get; set; }
    }
}
