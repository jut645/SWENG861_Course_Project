using System;
using System.Net;

namespace FlightPrices.WebAPI.Exceptions
{
    /// <summary>
    /// The <c>IClientApiException</c> class is thrown whenever something goes wrong for an IClient implementation.
    /// </summary>
    public class IClientApiException : Exception
    {
        /// <summary>
        /// The <c>IClientApiException</c> class constructor.
        /// </summary>
        /// <param name="code"> The HttpStatusCode of the failed Http Response.</param>
        public IClientApiException(HttpStatusCode code) : base($"Trip Advisor API responded with Status Code {code}")
        {
            StatusCode = code;
        }

        // The HTtpStatusCode of the failed response for later reference
        public HttpStatusCode StatusCode { get; private set; }
    }
}
