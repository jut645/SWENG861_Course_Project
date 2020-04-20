using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.ViewModels.Home
{
    public class HomePageQuotesViewModel
    {
        public IList<Flight> Quotes { get; set; }

        public bool IsRoundTrip { get; set; }
    }
}
