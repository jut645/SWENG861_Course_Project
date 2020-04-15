using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.ViewModels.Home
{
    public class HomeSearchViewModel
    {
        public Airports OriginAiport { get; set; }

        public Airports DestinationAirport { get; set; }

        public DateTime TakeOffDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsRoundTrip { get; set; }

        public IList<Airports> Airports { get; set; }
    }
}
