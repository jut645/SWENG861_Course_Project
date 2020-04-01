#pragma checksum "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "813266d624658c5a4e49ed6f7317d0947c3b2cca"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"813266d624658c5a4e49ed6f7317d0947c3b2cca", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af6af76d3f9fdd9b466306eded34bf5778e526e4", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Index.cshtml"
 using (Html.BeginForm(actionName: "DisplayCustomer", controllerName: "DisplayResult", FormMethod.Post))
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <div class=""form-group"">
        <label for=""origin"">Where From?</label>
        <input type=""text"" class=""form-control"" id=""origin"" aria-describedby=""origin-help"" placeholder=""Select Origin"">
        <small id=""origin-help"" class=""form-text text-muted"">This is the aiport you'll depart from.</small>
    </div>
    <div class=""form-group"">
        <label for=""destination"">Where To?</label>
        <input type=""text"" class=""form-control"" id=""destination"" aria-describedby=""destination-help"" placeholder=""Select Destination"">
        <small id=""destination-help"" class=""form-text text-muted"">This is the aiport you'll arrive at.</small>
    </div>
    <div class=""form-group"">
        <div>
            <label for=""departure-date"">When Do You Want To Leave?</label>
        </div>
        <input id=""departure-date"" type=""datetime-local"">
    </div>
    <div class=""form-group"">
        <div>
            <label for=""return-date"">When Do You Want To Return?</label>
        </div>
        <input id=");
            WriteLiteral(@"""return-date"" type=""datetime-local"" disabled>
    </div>
    <div class=""form-check"">
        <input type=""checkbox"" class=""form-check-input"" id=""round-trip"">
        <label class=""form-check-label"" for=""round-trip"">Round Trip</label>
    </div>
    <button type=""submit"" class=""btn btn-primary"">Submit</button>
");
#nullable restore
#line 34 "C:\Users\jaket\Documents\SWENG861\CourseProject\FlightPrices\FlightPrices.WebApp\Views\Home\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>
        $('#round-trip').click(function () {

            if ($('#round-trip').prop(""checked""))
            {
                $(""#return-date"").prop('disabled', false);
            }
            else
            {
                $(""#return-date"").prop('disabled', true);
            }
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
