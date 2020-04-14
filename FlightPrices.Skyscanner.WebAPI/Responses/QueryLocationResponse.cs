using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Responses
{
    public class QueryLocationResponse
    {
        public IList<Place> Places { get; set; }
    }
}
