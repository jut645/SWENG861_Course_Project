using FlightPrices.Skyscanner.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightPrices.WebApp.ViewModels.Home
{
    /// <summary>
    /// The <c>HomeSearchViewModel</c> class is the ViewModel for the Views/Home/Index.cshtml View.
    /// <remarks>
    ///     This View Model uses Data Annotations to define the validation rules for the form.
    /// </remarks>
    /// </summary>
    public class HomeSearchViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "The Origin Airport is required")]    // origin is required
        public string OriginAirport { get; set; }

        [Required(ErrorMessage = "The Destination Airport is required")]  // destination is required
        public string DestinationAirport { get; set; }

        [Required(ErrorMessage = "The Take Off Date is required")]   // Takeoff date is required
        public DateTime? TakeOffDate { get; set; }

        public DateTime? ReturnDate { get; set; }     // optional; required if IsRoundTrip == true

        // IsRoundTrip is required; must be true or false
        [Required(ErrorMessage = "Please indicate whether the flight is a round trip or not.")]
        public bool IsRoundTrip { get; set; } = false;

        public IList<Airports> Airports { get; set; }    // This list is used to provide airport suggestions in the UI

        /// <summary>
        /// Used to validate the ViewModel for non-data annotation validation rules.
        /// </summary>
        /// <param name="validationContext">The context used for validation data.</param>
        /// <returns>
        ///     The list of validation statements for each validation failure.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // The origin and destination airport names cannot be the same
            if (OriginAirport == DestinationAirport)
            {
                results.Add(new ValidationResult("Origin and Destination airports cannot be the same.", 
                    new[] { "DestinationAirport", "OriginAirport" }));
            }

            // The return date must have a value of the IsRoundTrip value is true
            if (IsRoundTrip && !ReturnDate.HasValue)
            {
                results.Add(new ValidationResult("If the flight is round trip, then you must enter a return date.",
                    new[] { "IsRoundTrip", "ReturnDate" }));
            }

            // The IsRoundTrip must be true if the return date has a value
            if (!IsRoundTrip && ReturnDate.HasValue)
            {
                results.Add(new ValidationResult("If the return date is specified, the flight must be round trip.",
                    new[] { "IsRoundTrip", "ReturnDate" }));
            }

            return results;
        }
    }
}
