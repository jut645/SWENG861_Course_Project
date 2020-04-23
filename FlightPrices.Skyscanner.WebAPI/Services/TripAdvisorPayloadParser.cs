using FlightPrices.Skyscanner.WebAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Services
{
    public class TripAdvisorPayloadParser
    {
        private readonly JToken _payload;

        public TripAdvisorPayloadParser(string tripAdvisorJsonPayload)
        {
            _payload = JToken.Parse(tripAdvisorJsonPayload);
        }

        public JArray Itineraries => (JArray)_payload.SelectToken("itineraries");

        public JArray Carriers => (JArray)_payload.SelectToken("carriers");

        public bool IsRoundTrip()
        {
            foreach (var itinerary in Itineraries)
            {
                var returnFlight = itinerary.SelectToken("f[1]");
                
                if (returnFlight == null)
                {
                    return false;
                }
            }

            return true;
        }

        public IList<Flight> GetQuoteData()
        {
            var flights = new List<Flight>();

            foreach (var itinerary in Itineraries)
            {
                var flight = new Flight();

                // Get best price
                int cheapestPrice = GetCheapestPriceFromItinerary(itinerary);
                flight.Cost = new Money(cheapestPrice, CurrencyType.UnitedStatesOfAmericaDollar);

                // Determine if the flight is direct
                bool isDirect = GetIsDirectFromItinerary(itinerary);
                flight.IsDirect = isDirect;

                // Get departure airline
                string departureAirline = GetDepartureAirlineFromItinerary(itinerary, Carriers);
                flight.DepartureAirline = departureAirline;

                // Get departure airline 
                DateTime departureTakeoffTime = GetDepartureTimeFromItineary(itinerary);
                flight.DepartureTakeoffTime = departureTakeoffTime;

                if (IsRoundTrip())
                {
                    // Get return airline
                    string returnAirline = GetReturnAirlineFromItinerary(itinerary, Carriers);
                    flight.ReturnAirline = returnAirline;

                    // Get return takeoff time
                    DateTime returnTime = GetReturnTimeFromItineary(itinerary);
                    flight.ReturnTakeoffTime = returnTime;

                }

                flights.Add(flight);
            }

            return flights;
        }

        private DateTime GetDepartureTimeFromItineary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var dateTime = firstLeg.SelectToken("dd").ToString();

            return DateTime.Parse(dateTime);
        }

        private DateTime GetReturnTimeFromItineary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.Last();
            var dateTime = firstLeg.SelectToken("ad").ToString();

            return DateTime.Parse(dateTime);
        }

        private bool GetIsDirectFromItinerary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");

            return legs.Count == 1;
        }

        private string GetReturnAirlineFromItinerary(JToken itinerary, JArray carriers)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var lastLeg = legs.Last();
            var carrierId = lastLeg.SelectToken("o").ToString();

            foreach (var carrier in carriers)
            {
                var id = carrier.SelectToken("c").ToString();
                if (id == carrierId)
                {
                    return carrier.SelectToken("n").ToString();
                }
            }

            throw new ArgumentException($"Carrier {carrierId} not found.");
        }

        private string GetDepartureAirlineFromItinerary(JToken itinerary, JArray carriers)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var carrierId = firstLeg.SelectToken("o").ToString();

            foreach (var carrier in carriers)
            {
                var id = carrier.SelectToken("c").ToString();
                if (id == carrierId)
                {
                    return carrier.SelectToken("n").ToString();
                }
            }

            throw new ArgumentException($"Carrier {carrierId} not found.");
        }

        private int GetCheapestPriceFromItinerary(JToken itinerary)
        {
            var agents = itinerary.SelectToken("l");

            // Get cheapest price
            int cheapestPriceInDollars = int.MaxValue;
            foreach (var agent in agents)
            {
                var priceInDollars = GetIntegerPriceFromAgentToken(agent);

                if (priceInDollars < cheapestPriceInDollars)
                {
                    cheapestPriceInDollars = priceInDollars;
                }
            }

            return cheapestPriceInDollars;
        }

        private int GetIntegerPriceFromAgentToken(JToken agent)
        {
            var priceJson = agent.SelectToken("pr.dp").ToString();
            priceJson = priceJson.Replace("$", string.Empty);

            return Convert.ToInt32(priceJson);
        }


    }
}
