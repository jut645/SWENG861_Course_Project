using System;
using System.Collections.Generic;

namespace FlightPrices.Skyscanner.WebAPI.Models
{
    public partial class Airports
    {
        public int AirportId { get; set; }
        public string AirportName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IataCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string SkyscannerPlaceId { get; set; }
    }
}
