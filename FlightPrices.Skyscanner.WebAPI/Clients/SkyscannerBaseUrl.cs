using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    /// <summary>
    /// This class acts as a container for the Skyscanner API URL to be used 
    /// by .NET core's dependency injection framework.
    /// </summary>
    public class SkyscannerBaseUrl
    {
        /// <summary>
        /// Property for retrieving the actual value of the Skyscanner API URL
        /// from the wrapper object.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Constructor for the SkyscannerBaseUrl class.
        /// </summary>
        /// <param name="value">The value of the Skyscanner API URL.</param>
        public SkyscannerBaseUrl(string value)
        {
            Value = value;
        }
    }
}
