using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.ViewModels.ErrorPage
{
    public class HttpErrorPageViewModel
    {
        public HttpErrorPageViewModel(HttpStatusCode code)
        {
            StatusCode = code;
        }


        public HttpStatusCode StatusCode { get; private set; }
    }
}
