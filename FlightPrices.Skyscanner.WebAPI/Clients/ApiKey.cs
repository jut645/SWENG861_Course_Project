using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients
{
    /// <summary>
    /// This class acts as a container for the Skyscanner API key to be used by
    /// .NET core's dependency injection framework.
    /// </summary>
    public class ApiKey
    {
        /// <summary>
        /// Property for retrieving the actual value of the Skyscanner API key
        /// from the wrapper object.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Constructor for the ApiKey class.
        /// </summary>
        /// <param name="value">The value of the Skyscanner API key.</param>
        public ApiKey(string value)
        {
            Value = value;
        }
    }
}
