using FlightPrices.Skyscanner.WebAPI.Models;
using FlightPrices.WebApp.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.ViewModels.Home
{
    public class HomeSearchViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "The Origin Airport is required")]
        public string OriginAirport { get; set; }

        [Required(ErrorMessage = "The Destination Airport is required")]
        public string DestinationAirport { get; set; }

        [Required(ErrorMessage = "The Take Off Date is required")]
        [CurrentDate(ErrorMessage = "The take off date cannot be in the past.")]
        public DateTime? TakeOffDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Please indicate whether the flight is a round trip or not.")]
        public bool IsRoundTrip { get; set; } = false;

        public IList<Airports> Airports { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (OriginAirport == DestinationAirport)
            {
                results.Add(new ValidationResult("Origin and Destination airports cannot be the same.", 
                    new[] { "DestinationAirport", "OriginAirport" }));
            }

            if (IsRoundTrip && !ReturnDate.HasValue)
            {
                results.Add(new ValidationResult("If the flight is round trip, then you must enter a return date.",
                    new[] { "IsRoundTrip", "ReturnDate" }));
            }

            if (!IsRoundTrip && ReturnDate.HasValue)
            {
                results.Add(new ValidationResult("If the return date is specified, the flight must be round trip.",
                    new[] { "IsRoundTrip", "ReturnDate" }));
            }

            return results;
        }
    }
}
