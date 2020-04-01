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
        private readonly FlightPricesContext _context;

        public MapController(ILogger<HomeController> logger, FlightPricesContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = new MapIndexPageViewModel()
            {
                Airports = _context.Airports.ToList()
            };

            return View(viewModel);
        }
    }
}