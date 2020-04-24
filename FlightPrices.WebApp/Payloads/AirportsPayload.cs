using FlightPrices.Skyscanner.WebAPI.Models;
using System.Collections.Generic;

namespace FlightPrices.WebApp.Payloads
{
    /// <summary>
    /// The <c>AirportsPayload</c> class is a container for the payload coming from the 
    /// AirportsController in the WebAPI project.
    /// </summary>
    public class AirportsPayload
    {
        public IList<Airports> Airports { get; set; }    // List of airports
    }
}
