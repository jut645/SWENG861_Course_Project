using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.ViewModels.Home
{
    public class HomeSearchViewModel
    {
        public string OriginAiport { get; set; }

        public string DestinationAirport { get; set; }

        public DateTime TakeOffDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsRoundTrip { get; set; }

    }
}
