#pragma checksum "D:\Project\abledocs\abledoc\Views\Jobs\_FileTreeToBeDeliverd.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c12a10d91e07e77c5a45981166e8ef4acbaa9bc3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Jobs__FileTreeToBeDeliverd), @"mvc.1.0.view", @"/Views/Jobs/_FileTreeToBeDeliverd.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\Jobs\_FileTreeToBeDeliverd.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c12a10d91e07e77c5a45981166e8ef4acbaa9bc3", @"/Views/Jobs/_FileTreeToBeDeliverd.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Jobs__FileTreeToBeDeliverd : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<input type=\"hidden\" id=\"JobID\"");
            BeginWriteAttribute("value", " value=\"", 111, "\"", 133, 1);
#nullable restore
#line 3 "D:\Project\abledocs\abledoc\Views\Jobs\_FileTreeToBeDeliverd.cshtml"
WriteAttributeValue("", 119, ViewBag.JobID, 119, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />

<div class=""material-datatables"">
    <table id=""FirstLevelDeliverd"" class=""table table-striped table-no-bordered table-hover"" cellspacing=""0"" width=""100%"" style=""width:100%"">
        <thead>
            <tr>
                <th></th>
                <th>");
#nullable restore
#line 10 "D:\Project\abledocs\abledoc\Views\Jobs\_FileTreeToBeDeliverd.cshtml"
               Write(Localizer["File Name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th> ");
#nullable restore
#line 11 "D:\Project\abledocs\abledoc\Views\Jobs\_FileTreeToBeDeliverd.cshtml"
                Write(Localizer["DeadLine"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n            </tr>\r\n        </thead>\r\n    </table>\r\n</div>\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
