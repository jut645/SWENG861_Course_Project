#pragma checksum "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6d36521ea6b1e36572a00d934ed4c15adf6b760a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Flights), @"mvc.1.0.view", @"/Views/Home/Flights.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\_ViewImports.cshtml"
using FlightPrices.WebApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\_ViewImports.cshtml"
using FlightPrices.WebApp.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6d36521ea6b1e36572a00d934ed4c15adf6b760a", @"/Views/Home/Flights.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af6af76d3f9fdd9b466306eded34bf5778e526e4", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Flights : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<FlightPrices.WebApp.ViewModels.Home.HomePageQuotesViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml"
  
    ViewData["Title"] = "Flights";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<table id=""flights""
        class=""table table-striped table-bordered""
        style=""width:100%"">
    <thead>
        <tr>
            <th>Airline</th>
            <th>Price</th>
            <th>Departure Takeoff Time</th>
        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 19 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml"
         foreach (var quote in Model.Quotes)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>");
#nullable restore
#line 22 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml"
               Write(quote.Airline);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 23 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml"
               Write(quote.Cost.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 24 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml"
               Write(quote.DepartureTakeoffTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 26 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Flights.cshtml"

        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n\r\n</table>\r\n\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <script>
        $(document).ready(function () {
            $('#flights').DataTable(
                {
                    ""bInfo"": false,
                    ""pageLength"": 5,
                    ""bLengthChange"": false,
                });
        });
    </script>
");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<FlightPrices.WebApp.ViewModels.Home.HomePageQuotesViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
