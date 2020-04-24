using FlightPrices.Skyscanner.WebAPI.Models;
using System.Collections.Generic;

namespace FlightPrices.WebApp.Payloads
{
    /// <summary>
    /// The <c>BrowseQuotesPayload</c> class is a container for the payload coming from the 
    /// BrowseQuotesController in the WebAPI project.
    /// </summary>
    public class BrowseQuotesPayload
    {
        public IList<Quote> Quotes { get; set; }    // List of quotes matching a search
    }
}
