using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;

namespace FlightPrices.WebApp.ViewModels.Home
{
    /// <summary>
    /// The <c>HomePageQuotesViewModel</c> is the view model for the /Views/Home/Index.cshtml view
    /// </summary>
    public class HomePageQuotesViewModel
    {
        public IList<Quote> Quotes { get; set; }     // The list of quotes corresponding to the current search

        public bool IsRoundTrip { get; set; }        // True if round trip search; false otherwise

        public DateTime DepartureDate { get; set; }  // The date of departure from the origin aiport.

        public DateTime ReturnDate { get; set; }     // The date of departure from the destination airport.

        public string OriginAirport { get; set; }    // The origin airport name

        public string DestinationAirport { get; set; }    // The destination airport name
    }
}
