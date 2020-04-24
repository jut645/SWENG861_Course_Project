namespace FlightPrices.Skyscanner.WebAPI.Models
{
    /// <summary>
    ///     Class <c>Airports</c> represents the Airports table in the FlightPrices database.
    ///     <remarks>
    ///         Nuilt automatically by the Scaffold-DbContext dotnet command with EntityFramework.
    ///     </remarks> 
    /// </summary>
    public partial class Airports
    {
        // These properties correspond to columns in the Airports table schema
        public int AirportId { get; set; }

        public string AirportName { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string IataCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string SkyscannerPlaceId { get; set; }

        /// <summary>
        ///     Overloads the == operator to compare two Airports instances for property-wise equality.
        ///     <param name="airportLeft">The airport instance on the left of the == operator.</param>
        ///     <param name="airportRight">The airport instance on the right of the == operator.</param>
        ///      <returns>
        ///         True if the flights are property-wise equal; false otherwise.
        ///     </returns>
        /// </summary>
        public static bool operator==(Airports airportLeft, Airports airportRight)
        {
            return airportLeft.AirportId == airportRight.AirportId &&
                   airportLeft.AirportName == airportRight.AirportName &&
                   airportLeft.City == airportRight.City &&
                   airportLeft.Country == airportRight.Country &&
                   airportLeft.IataCode == airportRight.IataCode &&
                   airportLeft.Latitude == airportRight.Latitude &&
                   airportLeft.Longitude == airportRight.Longitude &&
                   airportLeft.SkyscannerPlaceId == airportRight.SkyscannerPlaceId;
        }

        /// <summary>
        ///     Overloads the != operator to compare two Airports instances for property-wise equality.
        ///     <param name="airportLeft">The airport instance on the left of the != operator.</param>
        ///     <param name="airportRight">The airport instance on the right of the != operator.</param>
        ///      <returns>
        ///         True if the flights are not property-wise equal; false otherwise.
        ///     </returns>
        /// </summary>
        public static bool operator !=(Airports airportLeft, Airports airportRight)
        {
            return !(airportLeft == airportRight);
        }
    }
}
