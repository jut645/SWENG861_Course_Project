using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.Payloads
{
    public class AirportsPayload
    {
        public IList<Airports> Airports { get; set; }
    }
}
