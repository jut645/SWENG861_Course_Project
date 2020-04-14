using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPrices.WebApp.Models;
using FlightPrices.WebApp.ViewModels.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightPrices.WebApp.Controllers
{
    public class MapController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public MapController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = new MapIndexPageViewModel()
            {
            };

            return View(viewModel);
        }
    }
}