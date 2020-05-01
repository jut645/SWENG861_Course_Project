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

        public DateTime DepartureArrivalTime { get; set; }     // Time of arrival for first trip

        public int DepartureStopCount { get; set; }            // Number of stops on the way to the destination

        public string ReturnAirline { get; set; }              // Airline of return trip (optional)

        public DateTime? ReturnTakeoffTime { get; set; }       // Time of return trip (optional)

        public DateTime? ReturnArrivalTime { get; set; }      // Time of arrival for the return trip

        public int ReturnStopCount { get; set; }               // Number of stops on the return trip

        public string Key { get; set; }                       // The key for the itinerary

        public int DepartureFlightNumber { get; set; }        // The departure flight number
        
        public int ReturnFlightNumber { get; set; }           // The return flight number
    }
}
