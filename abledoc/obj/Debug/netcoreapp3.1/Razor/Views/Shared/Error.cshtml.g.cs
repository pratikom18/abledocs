#pragma checksum "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a8778797a9388365c9444f2a624e894a5e81f826"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Error), @"mvc.1.0.view", @"/Views/Shared/Error.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\_ViewImports.cshtml"
using abledoc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Project\abledocs\abledoc\Views\_ViewImports.cshtml"
using abledoc.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a8778797a9388365c9444f2a624e894a5e81f826", @"/Views/Shared/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ErrorViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
  
    ViewData["Title"] = Localizer["Error"];
    ViewData["PageTitle"] = Localizer["Error"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1 class=\"text-danger\">");
#nullable restore
#line 9 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                   Write(Localizer["Error."]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n<h2 class=\"text-danger\">");
#nullable restore
#line 10 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                   Write(Localizer["An error occurred while processing your request."]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n\r\n");
#nullable restore
#line 12 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
 if (Model.ShowRequestId)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>\r\n        <strong>");
#nullable restore
#line 15 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
           Write(Localizer["Request ID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</strong> <code>");
#nullable restore
#line 15 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                    Write(Model.RequestId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</code>\r\n    </p>\r\n");
#nullable restore
#line 17 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h3>");
#nullable restore
#line 19 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
Write(Localizer["Development Mode"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n<p>\r\n    ");
#nullable restore
#line 21 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
Write(Localizer["Swapping to"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <strong>");
#nullable restore
#line 21 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                 Write(Localizer["Development"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong> ");
#nullable restore
#line 21 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                                    Write(Localizer["environment will display more detailed information about the error that occurred."]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</p>\r\n<p>\r\n    <strong>");
#nullable restore
#line 24 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
       Write(Localizer["The Development environment shouldn't be enabled for deployed applications."]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n    ");
#nullable restore
#line 25 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
Write(Localizer["It can result in displaying sensitive information from exceptions to end users."]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    ");
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
Write(Localizer["For local debugging, enable the"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <strong>");
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                     Write(Localizer["Development"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong> ");
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                                                        Write(Localizer["environment by setting the"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <strong>");
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                                                                                                         Write(Localizer["ASPNETCORE_ENVIRONMENT"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong> ");
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                                                                                                                                                       Write(Localizer["environment variable to"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <strong>");
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
                                                                                                                                                                                                                                     Write(Localizer["Development"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n    ");
#nullable restore
#line 27 "D:\Project\abledocs\abledoc\Views\Shared\Error.cshtml"
Write(Localizer["and restarting the app."]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</p>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IViewLocalizer Localizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ErrorViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
