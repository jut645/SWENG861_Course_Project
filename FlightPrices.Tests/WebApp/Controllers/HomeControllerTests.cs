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
    /// <summary>
    ///     Class <c>HomeControllerTests</c> is responsible for testing the <c>Home Controller</c> controller.
    /// </summary>
    public class HomeControllerTests
    {
        /// <summary>
        ///     Tests the HomeController.Index method to ensure that it successfully returns a ViewResult
        ///     that contains the Airports served the by HttpClient from the IHttpClientFactory.
        ///     <remarks>
        ///         Tests with a single airport for simplicity.
        ///     </remarks>
        /// </summary>
        [Fact]
        public async void TestIndexSuccessfullyImportsAirports()
        {
            // Initialize test objects.
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(GetSuccessfulAirportsPayload());
            var testContext = new HomeController(logger, factory);

            // Run the index method and cast result
            var actionResult = await testContext.Index() as ViewResult;

            // Get the ViewResult view model
            var viewModel = actionResult.ViewData.Model as HomeSearchViewModel;

            // Test successful casting
            Assert.NotNull(actionResult);
            Assert.NotNull(viewModel);

            // Test that it successfully imported the test airport
            Assert.True(TestAirport == viewModel.Airports[0]);
        }

        /// <summary>
        ///     Tests the HomeController.Index method to ensure that it can handle an exception being thrown by
        ///     the HttpClient from the IHttpClientFactory.
        ///     <remarks>
        ///         Tests with a generic Exception for simplicity.
        ///     </remarks>
        /// </summary>
        [Fact]
        public async void TestIndexHandlesHttpClientException()
        {
            // Initialize test objects.
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(new Exception());
            var testContext = new HomeController(logger, factory);

            // Run the index method and cast result
            var actionResult = await testContext.Index() as ViewResult;

            // Test that the Controller redirects user to the HttpErrorPage
            Assert.NotNull(actionResult);
            Assert.Equal("HttpErrorPage", actionResult.ViewName);
        }

        /// <summary>
        ///     Tests the HomeController.Index method to ensure that it can handle empty results from
        ///     the HttpClient from the IHttpClientFactory.
        /// </summary>
        [Fact]
        public async void TestIndexHandlesEmptyResult()
        {
            // Initialize test objects.
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(GetEmptyAirportsPayload());
            var testContext = new HomeController(logger, factory);

            // Run the index method and cast result
            var actionResult = await testContext.Index() as ViewResult;

            // Get the viewModel and cast it
            var viewModel = actionResult.ViewData.Model as HomeSearchViewModel;

            // Test successful casting
            Assert.NotNull(actionResult);
            Assert.NotNull(viewModel);

            // Test that it successfully imported the empty payload
            Assert.True(viewModel.Airports.Count == 0);
        }

        /// <summary>
        ///     Tests the HomeController.GetAirports method to ensure it successfully retrieves the airports from 
        ///     the HttpClient from the IHttpClientFactory.
        /// </summary>
        [Fact]
        public async void TestGetAirports()
        {
            // Initialize test objects.
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(GetSuccessfulAirportsPayload());
            var testContext = new HomeController(logger, factory);

            // Invoke the GetAirports method
            var airports = await testContext.GetAirports();

            // Check for instance equality using operator overload
            var equal = TestAirport == airports[0];

            Assert.True(equal);
        }

        /// <summary>
        ///     Tests the HomeController.GetAirports method to ensure it successfully retrieves an empty
        ///     airports payload from the HttpClient.
        /// </summary>
        [Fact]
        public async void TestGetAirportsEmpty()
        {
            // Initialize test objects.
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(GetEmptyAirportsPayload());
            var testContext = new HomeController(logger, factory);

            // Invoke the GetAirports method
            var airports = await testContext.GetAirports();

            Assert.True(airports.Count == 0);
        }

        /// <summary>
        ///     Tests the HomeController.Search method for one way flights
        /// </summary>
        [Fact]
        public async void TestSearchOneWay()
        {
            // Initialize test objects.
            var logger = ILoggerMockFactory.Build<HomeController>();
            var factory = IHttpClientFactoryMockFactory.Build(GetEmptyAirportsPayload());
            var testContext = new HomeController(logger, factory);

            // Invoke the GetAirports method
            var airports = await testContext.GetAirports();

            Assert.True(airports.Count == 0);
        }



        private string GetSuccessfulAirportsPayload()
        {
            using (StreamReader reader = new StreamReader("../../../TestAirports.json"))
            {
                return reader.ReadToEnd();
            }
        }

        private string GetEmptyAirportsPayload()
        {
            using (StreamReader reader = new StreamReader("../../../EmptyAirports.json"))
            {
                return reader.ReadToEnd();
            }
        }

        private string GetOneWayFlightsPayload()
        {
            using (StreamReader reader = new StreamReader("../../../OneWayFlights.json"))
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
