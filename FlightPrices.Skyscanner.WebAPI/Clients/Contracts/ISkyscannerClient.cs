using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Clients.Contracts
{
    public interface ISkyscannerClient
    {
        Task<IList<Flight>> GetRoundTripFlights(string OriginAirportName,
            string DestintationAiportName,
            DateTime DepartureDate,
            DateTime ReturnDate);

        Task<IList<Flight>> GetOneWayFlights(string OriginAirportName,
            string DestintationAiportName,
            DateTime DepartureDate);

        IList<Airports> GetAirports();
    }
}
