using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Models
{
    public class Flight
    {
        public Money Cost { get; set; }

        public Airports Origin { get; set; }

        public Airports Destination { get; set; }

        public DateTime DepartureTakeoffTime { get; set; }

        public DateTime DepartureArrivalTime { get; set; }

        public DateTime? ReturnTakeoffTime { get; set; }

        public DateTime? ReturnArrivalTime { get; set; }

        public string Airline { get; set; }

        public int NumberOfStops { get; set; }
    }
}
