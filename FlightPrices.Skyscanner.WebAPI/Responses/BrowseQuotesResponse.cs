using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Responses
{
    public class BrowseQuotesResponse
    {
        public IList<Currency> Currencies { get; set; }

        public IList<Carrier> Carriers { get; set; }

        public IList<Place> Places { get; set; }

        public IList<Quote> Quotes { get; set; }

        public bool IsRoundTrip { get; set; }
    }
}
