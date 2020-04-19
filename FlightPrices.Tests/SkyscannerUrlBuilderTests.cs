using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Skyscanner.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlightPrices.Tests
{
    public class SkyscannerUrlBuilderTests
    {
        [Fact]
        public void TestOneWayUrlBuild()
        {
            var correctUrl = $"apiservices/browsequotes/v1.0/US/USD/en-US/JAX-sky/AUS-sky/2020-04-20";
            var testedUrl = SkyscannerUrlBuilder.BuildOneWayFlightUrl(OriginTest, DestinationTest, DepartureDateTest);

            Assert.Equal(correctUrl, testedUrl);
        }

        [Fact]
        public void TestRoundTripUrlBuild()
        {
            var correctUrl = $"apiservices/browsequotes/v1.0/US/USD/en-US/JAX-sky/AUS-sky/" +
                $"2020-04-20/2020-04-22";
            var testedUrl = SkyscannerUrlBuilder.BuildRoundTripFlightUrl(
                OriginTest, 
                DestinationTest, 
                DepartureDateTest,
                ReturnDateTest);

            Assert.Equal(correctUrl, testedUrl);
        }

        private Airports OriginTest => new Airports
        {
            AirportId = 2901,
            AirportName = "Jacksonville International Airport",
            City = "Jacksonville",
            Country = "United States",
            IataCode = "JAX",
            Latitude = 30.4941005706787,
            Longitude = -81.6878967285156,
            SkyscannerPlaceId = "JAX-sky"
        };

        private Airports DestinationTest => new Airports
        {
            AirportId = 2864,
            AirportName = "Austin Bergstrom International Airport",
            City = "Austin",
            Country = "United States",
            IataCode = "AUS",
            Latitude = 30.1944999694824,
            Longitude = -97.6698989868164,
            SkyscannerPlaceId = "AUS-sky"
        };

        private DateTime DepartureDateTest => new DateTime(2020, 4, 20);

        private DateTime ReturnDateTest => new DateTime(2020, 4, 22);
    }
}
