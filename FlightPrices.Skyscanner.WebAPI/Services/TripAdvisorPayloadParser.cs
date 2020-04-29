using FlightPrices.Skyscanner.WebAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Services
{
    /// <summary>
    /// The <c>TripAdvisorPayloadParser</c> parses information from the JSON content 
    /// sent in response to a Poll Session HTTP request to the TripAdvisor API.
    /// </summary>
    public class TripAdvisorPayloadParser
    {
        // The JSON payload from the Http response
        private readonly JToken _payload;

        /// <summary>
        /// The <c>TripAdvisorPayloadParser</c> class constructor.
        /// </summary>
        /// <param name="tripAdvisorJsonPayload">The JSON content from the TripAdvisor Http response.</param>
        public TripAdvisorPayloadParser(string tripAdvisorJsonPayload)
        {
            _payload = JToken.Parse(tripAdvisorJsonPayload);
        }

        /// <summary>
        /// Itinerary JSON objects.
        /// </summary>
        /// <returns>
        /// The Itineraries as a JArray for further parsing.
        /// </returns>
        public JArray Itineraries => (JArray)_payload.SelectToken("itineraries");

        /// <summary>
        /// Carrier JSON objects.
        /// </summary>
        /// <returns>
        /// The Carriers as a JArray for further parsing.
        /// </returns>
        public JArray Carriers => (JArray)_payload.SelectToken("carriers");

        public JToken Summary => _payload.SelectToken("summary");

        /// <summary>
        /// Determines if the TripAdvisor JSON payload is empty or not.
        /// </summary>
        /// <returns>
        /// True if the payload is null, empty, or purely whitespace.
        /// </returns>
        public bool IsEmpty => string.IsNullOrEmpty(_payload.ToString()) || 
                               string.IsNullOrWhiteSpace(_payload.ToString());

        /// <summary>
        /// Determines if there is a return-trip flight in the JSON payload.
        /// </summary>
        /// <returns>
        /// True if round-trip; false otherwise.
        /// </returns>
        public bool IsRoundTrip()
        {
            // Confirm there is a second flight in each itinerary.
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

        public bool IsComplete()
        {
            var summaryCompleteFlag = Summary.SelectToken("c");
            var complete = Convert.ToBoolean(summaryCompleteFlag);

            return complete;
        }

        public string GetKeyFromItinerary(JToken itinerary)
        {
            var key = itinerary.SelectToken("key").ToString();
            return key;
        }

        /// <summary>
        /// Gets the list of quotes from the TripAdvisor Poll Sessio response JSON payload.
        /// </summary>
        /// <returns>
        /// The list of quotes corresponding to the itineraries.
        /// </returns>
        public IList<Quote> GetQuoteData()
        {
            var flights = new List<Quote>();

            // If no itineraries, return empty list
            if (Itineraries == null || IsEmpty)
            {
                return flights;
            }

            foreach (var itinerary in Itineraries)
            {
                var flight = new Quote();

                // Get best price
                int cheapestPrice = GetCheapestPriceFromItinerary(itinerary);
                flight.Cost = new Money(cheapestPrice, CurrencyType.UnitedStatesOfAmericaDollar);

                // Get departure airline
                flight.DepartureAirline = GetDepartureAirlineFromItinerary(itinerary, Carriers); ;

                // Get departure airline 
                flight.DepartureTakeoffTime = GetDepartureTimeFromItineary(itinerary);

                // Get departure arrival time
                flight.DepartureArrivalTime = GetDepartureArrivalTime(itinerary);

                // Get number of stops on the departure trip
                flight.DepartureStopCount = GetDepartureStopsCount(itinerary);

                if (IsRoundTrip())
                {
                    // Get return airline
                    flight.ReturnAirline = GetReturnAirlineFromItinerary(itinerary, Carriers);

                    // Get return takeoff time
                    flight.ReturnTakeoffTime = GetReturnTimeFromItineary(itinerary);

                    // Get time of return trip arrival
                    flight.ReturnArrivalTime = GetReturnArrivalTime(itinerary);

                    // Get number of stops on the return trip
                    flight.ReturnStopCount = GetReturnStopsCount(itinerary);
                }

                flight.Key = GetKeyFromItinerary(itinerary);

                flights.Add(flight);
            }

            return flights;
        }

        /// <summary>
        /// Parse the return arrival time from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The datetime corresponding to the return arrival time.
        /// </returns>
        private DateTime GetReturnArrivalTime(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var lastLeg = legs.Last();
            var arrivalTime = lastLeg.SelectToken("ad").ToString();

            return DateTime.Parse(arrivalTime);
        }

        /// <summary>
        /// Parse the departure arrival time from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The datetime corresponding to the departure arrival time.
        /// </returns>
        private DateTime GetDepartureArrivalTime(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var lastLeg = legs.Last();
            var arrivalTime = lastLeg.SelectToken("ad").ToString();

            return DateTime.Parse(arrivalTime);

        }

        /// <summary>
        /// Parse the number of intermediate stops on the departure trip.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The number of intermediate stops on the departure trip.
        /// </returns>
        private int GetDepartureStopsCount(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");

            return legs.Count - 1;
        }

        /// <summary>
        /// Parse the number of intermediate stops on the return trip.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The number of intermediate stops on the return trip.
        /// </returns>
        private int GetReturnStopsCount(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");

            return legs.Count - 1;
        }

        /// <summary>
        /// Parse the departure takeoff time from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The datetime corresponding to the departure takeoff time.
        /// </returns>
        private DateTime GetDepartureTimeFromItineary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var dateTime = firstLeg.SelectToken("dd").ToString();

            return DateTime.Parse(dateTime);
        }

        /// <summary>
        /// Parse the return takeoff time from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The datetime corresponding to the return takeoff time.
        /// </returns>
        private DateTime GetReturnTimeFromItineary(JToken itinerary)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var dateTime = firstLeg.SelectToken("dd").ToString();

            return DateTime.Parse(dateTime);
        }

        /// <summary>
        /// Parse the return airline from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The name of the return airline.
        /// </returns>
        private string GetReturnAirlineFromItinerary(JToken itinerary, JArray carriers)
        {
            var flight = itinerary.SelectToken("f[1]");
            var legs = (JArray)flight.SelectToken("l");
            var lastLeg = legs.Last();
            var carrierId = lastLeg.SelectToken("o").ToString();

            // Find the carrier from the carrier JSON
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

        /// <summary>
        /// Parse the departure airline from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The name of the departure airline.
        /// </returns>
        private string GetDepartureAirlineFromItinerary(JToken itinerary, JArray carriers)
        {
            var flight = itinerary.SelectToken("f[0]");
            var legs = (JArray)flight.SelectToken("l");
            var firstLeg = legs.First();
            var carrierId = firstLeg.SelectToken("o").ToString();

            // Find the carrier in the Carriers json
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

        /// <summary>
        /// Parse the cheapest price from an JSON itinarary object.
        /// </summary>
        /// <param name="itinerary">The itineray JSON.</param>
        /// <returns>
        /// The integer value for the cheapest price.
        /// </returns>
        private int GetCheapestPriceFromItinerary(JToken itinerary)
        {
            var agents = itinerary.SelectToken("l");

            // Find cheapest price
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

        /// <summary>
        /// Parse the price from an agent JSON token.
        /// </summary>
        /// <param name="agent">The agent (e.g. KIWI.com) JSON token.</param>
        /// <returns>
        /// The integer value of the cheapest price found.
        /// </returns>
        private int GetIntegerPriceFromAgentToken(JToken agent)
        {
            var priceJson = agent.SelectToken("pr.p").ToString();
            var price = Convert.ToDouble(priceJson);

            return (int)Math.Round(price, MidpointRounding.AwayFromZero);
        }
    }
}
