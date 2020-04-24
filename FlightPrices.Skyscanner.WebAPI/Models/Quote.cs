using System;

namespace FlightPrices.Skyscanner.WebAPI.Models
{
    /// <summary>
    /// The <c>Quote</c> class is a Data Transfer Object for communicating Quote information.
    /// </summary>
    public class Quote
    {
        public Money Cost { get; set; }                        // Cheapest cost of flight

        public string DepartureAirline { get; set; }           // Airline of first departure

        public DateTime DepartureTakeoffTime { get; set; }     // Time of first departure

        public string ReturnAirline { get; set; }              // Airline of return trip (optional)

        public DateTime? ReturnTakeoffTime { get; set; }       // Time of return trip (optional)

        public bool IsDirect { get; set; }                     // True if there are no stops during the flight.
    }
}
