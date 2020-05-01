using System;
using System.Net;

namespace FlightPrices.WebApp.Exceptions
{
    /// <summary>
    /// The <c>FlightQuoteApiException</c> class objects are thrown when an error occurs in the WebAPI project.
    /// </summary>
    public class FlightQuoteApiException : Exception
    {
        /// <summary>
        /// The <c>FlightQuoteApiException</c> class constructor.
        /// </summary>
        /// <param name="code">The HttpStatusCode of the failing HTTP response.</param>
        public FlightQuoteApiException(HttpStatusCode code) : 
            base($"The Flight Quotes API responded with status code {code}")
        {
            StatusCode = code;
        }
        
        // The Http Status Code for later reference.
        public HttpStatusCode StatusCode { get; private set; }
    }
}
