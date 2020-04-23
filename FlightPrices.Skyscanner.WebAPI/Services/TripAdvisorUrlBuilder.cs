using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Services
{
    public class TripAdvisorUrlBuilder
    {
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

    }
}
