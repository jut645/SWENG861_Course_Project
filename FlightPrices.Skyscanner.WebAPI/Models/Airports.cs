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

        public static bool operator==(Airports airportLeft, Airports airportRight)
        {
            return airportLeft.AirportId == airportRight.AirportId &&
                   airportLeft.AirportName == airportRight.AirportName &&
                   airportLeft.City == airportRight.City &&
                   airportLeft.Country == airportRight.Country &&
                   airportLeft.IataCode == airportRight.IataCode &&
                   airportLeft.Latitude == airportRight.Latitude &&
                   airportLeft.Longitude == airportRight.Longitude &&
                   airportLeft.SkyscannerPlaceId == airportRight.SkyscannerPlaceId;
        }

        public static bool operator !=(Airports airportLeft, Airports airportRight)
        {
            return !(airportLeft == airportRight);
        }
    }
}
