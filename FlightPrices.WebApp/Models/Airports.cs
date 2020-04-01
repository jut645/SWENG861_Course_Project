using System;
using System.Collections.Generic;

namespace FlightPrices.WebApp.Models
{
    public partial class Airports
    {
        public int Id { get; set; }
        public string AirportName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
