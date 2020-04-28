using System;

namespace FlightPrices.Skyscanner.WebAPI.Services
{
    /// <summary>
    /// The <c>TripAdvisorUrlBuilder</c> class builds Url strings for making HTTP requests to the TripAdvisor API
    /// </summary>
    public class TripAdvisorUrlBuilder
    {
        /// <summary>
        /// Builds a URL for making an HTTP request to the Create Session endpoint for the TripAdvisor API
        /// for round-trip flights.
        /// </summary>
        /// <param name="originIataCode">The IATA code for the origin airport.</param>
        /// <param name="destinationIataCode">The IATA code for the destination airport.</param>
        /// <param name="DepartureDate">The desired departure date.</param>
        /// <param name="ReturnDate">The desired return date.</param>
        /// <returns>
        /// The Url as a string.
        /// </returns>
        public static string BuildSessionUrl(
            string originIataCode, 
            string destinationIataCode, 
            DateTime DepartureDate, 
            DateTime ReturnDate)
        {
            return $"flights/create-session?" +
                   $"dd2={ReturnDate.ToString("yyyy-MM-dd")}&" +
                   $"currency=USD&" +
                   $"o2={destinationIataCode}&" +
                   $"d2={originIataCode}&" +
                   $"ta=1&" +
                   $"c=0&" +
                   $"d1={destinationIataCode}&" +
                   $"o1={originIataCode}&" +
                   $"dd1={DepartureDate.ToString("yyyy-MM-dd")}";
        }

        /// <summary>
        /// Builds a URL for making an HTTP request to the Create Session endpoint for the TripAdvisor API
        /// for one-way flights.
        /// </summary>
        /// <param name="originIataCode">The IATA code for the origin airport.</param>
        /// <param name="destinationIataCode">The IATA code for the destination airport.</param>
        /// <param name="DepartureDate">The desired departure date.</param>
        /// <returns>
        /// The Url as a string.
        /// </returns>
        public static string BuildSessionUrl(
            string originIataCode,
            string destinationIataCode,
            DateTime DepartureDate)
        {
            return $"flights/create-session?" +
                $"currency=USD&" +
                $"ta=1&" +
                $"c=0&" +
                $"d1={destinationIataCode}&" +
                $"o1={originIataCode}&" +
                $"dd1={DepartureDate.ToString("yyyy-MM-dd")}";
        }

        /// <summary>
        /// Builds a URL for making an HTTP request to the Poll Session endpoint for the TripAdvisor API.
        /// </summary>
        /// <param name="sessionId">The session Id received from the creation session response.</param>
        /// <returns>
        ///     The Url as a string.
        /// </returns>
        public static string BuildPollUrl(string sessionId)
        {
            return $"flights/poll?sid={sessionId}&currency=USD&n=15&ns=NON_STOP%252CONE_STOP&so=PRICE&o=0";
        }

    }
}
