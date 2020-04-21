using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.Tests.Mocks;
using FlightPrices.WebApp.Controllers;
using FlightPrices.WebApp.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace FlightPrices.Tests.WebApp.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _testContext;

        public HomeControllerTests()
        {
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(GetSuccessfulAirportsPayload());

            _testContext = new HomeController(logger: logger, factory: factory);
        }

        [Fact]
        public async void TestIndex()
        {
            var actionResult = await _testContext.Index() as ViewResult;
            var viewModel = actionResult.ViewData.Model as HomeSearchViewModel;
    
            Assert.True(TestAirport == viewModel.Airports[0]);
        }

        [Fact]
        public async void TestGetAirports()
        {
            var airports = await _testContext.GetAirports();
            var equal = TestAirport == airports[0];

            Assert.True(equal);

        }

        [Fact]
        public async void TestGetAirportNames()
        {
            var airportNames = await _testContext.GetAirportNames();
            var equal = TestAirport.AirportName == airportNames[0];

            Assert.True(equal);
        }

        private string GetSuccessfulAirportsPayload()
        {
            using (StreamReader reader = new StreamReader("../../../TestAirports.json"))
            {
                return reader.ReadToEnd();
            }
        }

        private Airports TestAirport => JsonConvert.DeserializeObject<Airports>(@"{
                ""AirportId"": 1,
                ""AirportName"": ""Goroka Airport"",
                ""City"": ""Goroka"",
                ""Country"": ""Papua New Guinea"",
                ""IataCode"": ""GKA"",
                ""Latitude"": -6.0816898345900015,
                ""Longitude"": 145.391998291,
                ""SkyscannerPlaceId"": ""GKA-sky""}");
    }
}
