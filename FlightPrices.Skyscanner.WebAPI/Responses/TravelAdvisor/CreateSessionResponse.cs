namespace FlightPrices.Skyscanner.WebAPI.Responses.TravelAdvisor
{
    /// <summary>
    /// The <c>SearchParameters</c> class is modeled after the JSON response from the 
    /// TripAdvisor API - this class models the SearchParameters JSON object.
    /// </summary>
    public class SearchParameters
    {
        public string sid { get; set; }    // This Session Id assigned by TripAdvisor API
    }

    /// <summary>
    /// The <c>CreateSessionResponse</c> class is modeled after the JSON response from the 
    /// TripAdvisor API - this lass models the response from a Create Session request.
    /// </summary>
    public class CreateSessionResponse
    {
        public SearchParameters search_params { get; set; }
    }
}
