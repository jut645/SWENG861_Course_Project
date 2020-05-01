using System.Net;

namespace FlightPrices.WebApp.ViewModels.ErrorPage
{
    /// <summary>
    /// The <c>HttpErrorPageViewModel</c> is the View Model for the /Views/Home/HttpErrorPage.cshtml view
    /// </summary>
    public class HttpErrorPageViewModel
    {
        /// <summary>
        /// The <c>HttpErrorPageViewModel</c> class constructor.
        /// </summary>
        /// <param name="code">The HttpStatusCode of the failing reponse.</param>
        public HttpErrorPageViewModel(HttpStatusCode code)
        {
            StatusCode = code;
        }

        // The Http Code from the response for display.
        public HttpStatusCode StatusCode { get; private set; }
    }
}
