#pragma checksum "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3f246686cfafc79421f51921110ccd4936b318a9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Phases_alttxt), @"mvc.1.0.view", @"/Views/Phases/alttxt.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f246686cfafc79421f51921110ccd4936b318a9", @"/Views/Phases/alttxt.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Phases_alttxt : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
  
    ViewData["Title"] = Localizer["ALT Text"];
    ViewData["PageTitle"] = Localizer["ALT Text"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<input type=\"hidden\" name=\"myConfirm\" id=\"myConfirm\"");
            BeginWriteAttribute("value", " value=\"", 245, "\"", 274, 1);
#nullable restore
#line 10 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
WriteAttributeValue("", 253, Localizer["Confirm"], 253, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n<div class=\"content\">\r\n    <div class=\"container-fluid\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-12\">\r\n                <div class=\"card\">\r\n                    <div class=\"card-header card-header-primary card-header-icon\">\r\n");
            WriteLiteral("                        <div class=\"row\">\r\n                            <div class=\"form-group col-sm-10\">\r\n                                <input type=\"text\" id=\"txtSearch\" class=\"form-control col-sm-12\"");
            BeginWriteAttribute("value", " value=\"", 960, "\"", 983, 1);
#nullable restore
#line 23 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
WriteAttributeValue("", 968, ViewBag.Search, 968, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                                <br />\r\n");
            WriteLiteral(@"                            </div>
                            <div class=""form-group col-sm-2"">
                                <button type=""button"" class=""btn btn-primary btn-sm float-sm-right btnSearch btn-primary-1"">
                                    <i class=""fa fa-search""></i> ");
#nullable restore
#line 29 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                                            Write(Localizer["Search"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class=""card-body"">
                        <div class=""toolbar"">
                            <!--        Here you can write extra buttons/actions for the toolbar              -->
                        </div>
                        <input type=""hidden"" id=""currentDate""");
            BeginWriteAttribute("value", " value=\"", 2026, "\"", 2070, 1);
#nullable restore
#line 38 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
WriteAttributeValue("", 2034, DateTime.Now.ToString("yyyy-MM-dd"), 2034, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                        <input type=\"hidden\" id=\"forDue2Days\"");
            BeginWriteAttribute("value", " value=\"", 2137, "\"", 2194, 1);
#nullable restore
#line 39 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
WriteAttributeValue("", 2145, DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"), 2145, 49, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                        <div class=""material-datatables"">
                            <table id=""AltTextTable"" class=""table table-striped table-no-bordered table-hover"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                <thead>
                                    <tr>
                                        <th>");
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                       Write(Localizer["Engagement Num"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("#</th>\r\n                                        <th>");
#nullable restore
#line 45 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                       Write(Localizer["File ID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th>");
#nullable restore
#line 46 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                       Write(Localizer["File Name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th>");
#nullable restore
#line 47 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                       Write(Localizer["DeadLine"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th>");
#nullable restore
#line 48 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                       Write(Localizer["Assigned To"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th class=\"disabled-sorting\">");
#nullable restore
#line 49 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
                                                                Write(Localizer["Actions"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>

                            </table>
                        </div>
                    </div>
                    <!-- end content-->
                </div>
                <!--  end card  -->
            </div>
            <!-- end col-md-12 -->
        </div>
        <!-- end row -->
    </div>
</div>
<div id=""popupSendFile"">
</div>
<div id=""popupQuoteDetail"">
</div>
");
            DefineSection("scripts", async() => {
                WriteLiteral("\r\n\r\n\r\n    <!-- BEGIN PAGE LEVEL PLUGINS -->\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3f246686cfafc79421f51921110ccd4936b318a99469", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 3613, "~/assets/js/pages/AltTextTable.js?v=", 3613, 36, true);
#nullable restore
#line 75 "D:\Project\abledocs\abledoc\Views\Phases\alttxt.cshtml"
AddHtmlAttributeValue("", 3649, abledoc.Utility.CommonHelper.Version, 3649, 37, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    <!-- END PAGE LEVEL PLUGINS -->\r\n");
            }
            );
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