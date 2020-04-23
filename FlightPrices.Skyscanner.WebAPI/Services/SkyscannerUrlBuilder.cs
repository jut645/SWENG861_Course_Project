using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Services
{
    public static class SkyscannerUrlBuilder
    {
        public static string BuildOneWayFlightUrl(Airports origin, Airports destination, DateTime departureDate)
        {
            var originId = origin.SkyscannerPlaceId;
            var destinationId = destination.SkyscannerPlaceId;
            var outboundDate = departureDate.ToString("yyyy-MM-dd");

            return $"apiservices/browseroutes/v1.0/US/USD/en-US/{originId}/{destinationId}/{outboundDate}";
        }

        public static string BuildRoundTripFlightUrl(
            Airports origin, 
            Airports destination, 
            DateTime departureDate, 
            DateTime returnDate)
        {
            var originId = origin.SkyscannerPlaceId;
            var destinationId = destination.SkyscannerPlaceId;
            var outboundDate = departureDate.ToString("yyyy-MM-dd");
            var inboundDate = returnDate.ToString("yyyy-MM-dd");

            return $"apiservices/browseroutes/v1.0/US/USD/en-US/" +
                $"{originId}/{destinationId}/{outboundDate}/{inboundDate}";
        }

    }
}
