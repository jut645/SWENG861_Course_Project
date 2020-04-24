using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients.Contracts
{    
    /// <summary>
     ///     Interface <c>IClient</c> defines the interface that the Controllers will depend upon.
     /// </summary>
    public interface IClient
    {
        /// <summary>
        ///     The method signature to implement for getting a list of quotes for a round trip.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///     <param name="returnDate">The date that the flight will depart to return to the origin.</param>
        ///      <returns>
        ///         The <c>Flight</c> instances for this search.
        ///     </returns>
        /// </summary>
        Task<IList<Quote>> GetRoundTripFlights(string origin,
            string destination,
            DateTime departureDate,
            DateTime returnDate);

        /// <summary>
        ///     The method signature to implement for getting a list of quotes for a one way trip.
        ///     <param name="origin">The name of the origin </param>
        ///     <param name="destination">The name of the destination airport.</param>
        ///     <param name="departureDate">The date that the flight will depart from the origin airport.</param>
        ///      <returns>
        ///         The <c>Flight</c> instances for this search.
        ///     </returns>
        /// </summary>
        Task<IList<Quote>> GetOneWayFlights(string origin, string destination, DateTime departureDate);

        /// <summary>
        ///     The method signature to implement for getting the list of Airports instances from the database.
        ///      <returns>
        ///         The <c>Airports</c> instances.
        ///     </returns>
        /// </summary>
        IList<Airports> GetAirports();
    }
}
