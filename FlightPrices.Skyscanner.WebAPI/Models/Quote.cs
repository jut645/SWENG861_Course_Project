using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }

        public double MinPrice { get; set; }

        public bool Direct { get; set; }

        public DateTime QuoteDateTime { get; set; }

        public Leg OutboundLeg { get; set; }
    }
}
