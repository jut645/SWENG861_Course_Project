using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Models
{
    public class Flight
    {
        public Money Cost { get; set; }

        public string DepartureAirline { get; set; }

        public DateTime DepartureTakeoffTime { get; set; }

        public string ReturnAirline { get; set; }

        public DateTime? ReturnTakeoffTime { get; set; }

        public bool IsDirect { get; set; }
    }
}
