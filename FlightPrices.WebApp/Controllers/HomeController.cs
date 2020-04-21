﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FlightPrices.WebApp.ViewModels.Home;
using System.Net.Http;
using FlightPrices.Skyscanner.WebAPI.Models;
using Newtonsoft.Json;
using FlightPrices.WebApp.Payloads;

namespace FlightPrices.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _httpClientFactory = factory;
        }

        public async Task<IActionResult> Index()
        {
            var airports = await GetAirports();

            var viewModel = new HomeSearchViewModel
            {
                Airports = airports
            };

            return View(viewModel);
        }

        public async Task<IList<string>> GetAirportNames()
        {
            var airports = await MakeHTTPGetRequest<AirportsPayload>("airports");

            return airports.Airports.Select(a => a.AirportName).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Search(HomeSearchViewModel searchForm)
        {
            var airports = await GetAirports();

            if (!airports.Select(a => a.AirportName).Contains(searchForm.OriginAirport))
            {
                ModelState.AddModelError("OriginAirport", "Invalid airport name");
            }

            if (!airports.Select(a => a.AirportName).Contains(searchForm.DestinationAirport))
            {
                ModelState.AddModelError("DestinationAirport", "Invalid airport name");
            }

            if (!ModelState.IsValid)
            {
                searchForm.Airports = airports;
                return View("Index", searchForm);
            }

            if (!searchForm.ReturnDate.HasValue && !searchForm.IsRoundTrip)
            {
                return await GetOneWayFlights(searchForm);
            }
            else if (searchForm.ReturnDate.HasValue && searchForm.IsRoundTrip)
            {
                return await GetRoundTripFlights(searchForm);
            }
            else
            {
                throw new Exception("Invalid form.");
            }

        }

        public async Task<IList<Airports>> GetAirports()
        {
            var airportsPayload = await MakeHTTPGetRequest<AirportsPayload>("airports");

            return airportsPayload.Airports;
        }

        private async Task<IActionResult> GetOneWayFlights(HomeSearchViewModel searchForm)
        {
            string url = $"https://localhost:44320/browsequotes/oneWay?" +
                $"originAirportName={searchForm.OriginAirport}" +
                $"&destinationAirportName={searchForm.DestinationAirport}" +
                $"&departureDate={searchForm.TakeOffDate.Value.ToString("yyyy-MM-dd")}";

            var quotesResponse = await MakeHTTPGetRequest<BrowseQuotesPayload>(url);

            var viewModel = new HomePageQuotesViewModel
            {
                Quotes = quotesResponse.Quotes,
                IsRoundTrip = false
            };

            return View("Flights", viewModel);
        }

        private async Task<IActionResult> GetRoundTripFlights(HomeSearchViewModel searchForm)
        {
            string url = $"https://localhost:44320/browsequotes/roundTrip?" +
                $"originAirportName={searchForm.OriginAirport}" +
                $"&destinationAirportName={searchForm.DestinationAirport}" +
                $"&departureDate={searchForm.TakeOffDate.Value.ToString("yyyy-MM-dd")}" +
                $"&returnDate={searchForm.ReturnDate.Value.ToString("yyyy-MM-dd")}";

            var quotesResponse = await MakeHTTPGetRequest<BrowseQuotesPayload>(url);

            var viewModel = new HomePageQuotesViewModel
            {
                Quotes = quotesResponse.Quotes,
                IsRoundTrip = true
            };

            return View("Flights", viewModel);
        }


        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:44320/");

            return httpClient;
        }

        private async Task<T> MakeHTTPGetRequest<T>(string url)
        {
            var httpClient = GetHttpClient();
            var response = await httpClient.GetAsync(url);
            var content = response.Content;
            var json = content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
