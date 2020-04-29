using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.Exceptions
{
    public class FlightQuoteApiException : Exception
    {
        public FlightQuoteApiException(HttpStatusCode code) : 
            base($"The Flight Quotes API responded with status code {code}")
        {
            StatusCode = code;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}
