using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightPrices.WebAPI.Exceptions
{
    public class IClientApiException : Exception
    {
        public IClientApiException(HttpStatusCode code) : base($"Trip Advisor API responded with Status Code {code}")
        {
            StatusCode = code;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}
