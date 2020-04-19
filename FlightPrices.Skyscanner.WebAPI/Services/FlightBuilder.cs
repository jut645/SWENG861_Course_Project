using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Services
{
    public class FlightBuilder
    {
        private BrowseQuotesResponse _response;
        private IList<Flight> _flights = new List<Flight>();
        private Dictionary<int, string> _carriers;

        public FlightBuilder(BrowseQuotesResponse response)
        {
            _response = response;
            _carriers = response.Carriers.ToDictionary(c => c.CarrierId, c => c.Name);
        }

        public IList<Flight> Build()
        {
            foreach (var quote in _response.Quotes)
            {
                var flight = new Flight();

                flight.Cost = new Money(Convert.ToDecimal(quote.MinPrice), CurrencyType.UnitedStatesOfAmericaDollar);
                flight.IsDirect = quote.Direct;

                HydrateFlightForOutboundFromQuote(quote, flight);

                if (_response.IsRoundTrip)
                {
                    HydrateFlightForInboundFromQuote(quote, flight);
                }

                // Add flight to the list
                _flights.Add(flight);
            }

            return _flights;
        }

        private void HydrateFlightForOutboundFromQuote(Quote quote, Flight flight)
        {
            // Get the quote data from the response payload
            var carrierId = quote.OutboundLeg.CarrierIds.Single();

            flight.DepartureAirline = _carriers[carrierId];
            flight.DepartureTakeoffTime = quote.OutboundLeg.DepartureDate;
        }

        private void HydrateFlightForInboundFromQuote(Quote quote, Flight flight)
        {
            // Get the quote data from the response payload
            var carrierId = quote.InboundLeg.CarrierIds.Single();

            flight.ReturnAirline = _carriers[carrierId];
            flight.ReturnTakeoffTime = quote.InboundLeg.DepartureDate;
        }

    }
}
