#pragma checksum "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1a3658f19ac7d4f3860b24254ad3d6bb8591695d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Phases_tagging), @"mvc.1.0.view", @"/Views/Phases/tagging.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1a3658f19ac7d4f3860b24254ad3d6bb8591695d", @"/Views/Phases/tagging.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Phases_tagging : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
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
#nullable restore
#line 3 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
  
    ViewData["Title"] = Localizer["Phase1"];
    ViewData["PageTitle"] = Localizer["Phase1"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<input type=\"hidden\" name=\"myedit\" id=\"myedit\"");
            BeginWriteAttribute("value", " value=\"", 231, "\"", 257, 1);
#nullable restore
#line 8 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
WriteAttributeValue("", 239, Localizer["Edit"], 239, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n<div class=\"content\">\r\n    <div class=\"container-fluid\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-12\">\r\n                <div class=\"card\">\r\n                    <div class=\"card-header card-header-primary card-header-icon\">\r\n");
            WriteLiteral("                        <div class=\"row\">\r\n                            <div class=\"form-group col-sm-10\">\r\n                                <input type=\"text\" id=\"txtSearch\" class=\"form-control col-sm-12\"");
            BeginWriteAttribute("value", " value=\"", 943, "\"", 966, 1);
#nullable restore
#line 21 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
WriteAttributeValue("", 951, ViewBag.Search, 951, 15, false);

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
#line 27 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
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
            BeginWriteAttribute("value", " value=\"", 2009, "\"", 2053, 1);
#nullable restore
#line 36 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
WriteAttributeValue("", 2017, DateTime.Now.ToString("yyyy-MM-dd"), 2017, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                        <input type=\"hidden\" id=\"forDue2Days\"");
            BeginWriteAttribute("value", " value=\"", 2120, "\"", 2177, 1);
#nullable restore
#line 37 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
WriteAttributeValue("", 2128, DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"), 2128, 49, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                        <div class=""material-datatables"">
                            <table id=""TaggingTable"" class=""table table-striped table-no-bordered table-hover"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                <thead>
                                    <tr>
                                        <th>");
#nullable restore
#line 42 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
                                       Write(Localizer["Engagement Num"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("#</th>\r\n                                        <th>");
#nullable restore
#line 43 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
                                       Write(Localizer["File ID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th>");
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
                                       Write(Localizer["File Name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th>");
#nullable restore
#line 45 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
                                       Write(Localizer["DeadLine"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th>");
#nullable restore
#line 46 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
                                       Write(Localizer["Assigned To"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th class=\"disabled-sorting\">");
#nullable restore
#line 47 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
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
<div id=""popupQuoteDetail"">
</div>
");
            DefineSection("scripts", async() => {
                WriteLiteral("\r\n\r\n\r\n    <!-- BEGIN PAGE LEVEL PLUGINS -->\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1a3658f19ac7d4f3860b24254ad3d6bb8591695d9438", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 3602, "~/assets/js/pages/Tagging.js?v=", 3602, 31, true);
#nullable restore
#line 71 "D:\Project\abledocs\abledoc\Views\Phases\tagging.cshtml"
AddHtmlAttributeValue("", 3633, abledoc.Utility.CommonHelper.Version, 3633, 37, false);

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
