using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Responses.TravelAdvisor
{

    public class SearchParameters
    {
        public string sid { get; set; }
    }

    public class CreateSessionResponse
    {
        public SearchParameters search_params { get; set; }
    }
}
