#pragma checksum "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c8876cadb50f8693ae3d1c269f2c829e87d4fdc6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ApprovedCreditNotes_Index), @"mvc.1.0.view", @"/Views/ApprovedCreditNotes/Index.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c8876cadb50f8693ae3d1c269f2c829e87d4fdc6", @"/Views/ApprovedCreditNotes/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_ApprovedCreditNotes_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
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
#line 3 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
  
    ViewData["Title"] = Localizer["ApprovedCreditNotes"];
    ViewData["PageTitle"] = Localizer["ApprovedCreditNotes"];

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""content"">
    <div class=""container-fluid"">
        <div class=""row"">
            <div class=""col-md-12"">
                <div class=""card"">

                    <div class=""row col-md-12"">

                        <div class=""form-group col-md-4"">
                            <input type=""text"" id=""startdate"" class=""form-control datepicker"" autocomplete=""off""");
            BeginWriteAttribute("placeholder", " placeholder=\"", 593, "\"", 631, 1);
#nullable restore
#line 17 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
WriteAttributeValue("", 607, Localizer["Start Date"], 607, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                        </div>\r\n\r\n                        <div class=\"form-group col-md-4\">\r\n                            <input type=\"text\" id=\"enddate\" class=\"form-control datepicker\" autocomplete=\"off\"");
            BeginWriteAttribute("placeholder", " placeholder=\"", 840, "\"", 876, 1);
#nullable restore
#line 21 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
WriteAttributeValue("", 854, Localizer["End Date"], 854, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                        </div>\r\n                        <div class=\"form-group col-md-4\">\r\n");
            WriteLiteral("                            <input type=\"button\" id=\"setdate\" class=\"btn btn-success btn-sm float-sm-right   btn-primary-1\"");
            BeginWriteAttribute("value", " value=", 1260, "", 1287, 1);
#nullable restore
#line 25 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
WriteAttributeValue("", 1267, Localizer["Search"], 1267, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                        </div>
                    </div>

                    <div class=""card-body"">
                        <div class=""toolbar"">

                            <!--        Here you can write extra buttons/actions for the toolbar              -->
                        </div>
                        <input type=""hidden"" id=""currentDate""");
            BeginWriteAttribute("value", " value=\"", 1656, "\"", 1700, 1);
#nullable restore
#line 34 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
WriteAttributeValue("", 1664, DateTime.Now.ToString("yyyy-MM-dd"), 1664, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                        <input type=\"hidden\" id=\"forDue2Days\"");
            BeginWriteAttribute("value", " value=\"", 1767, "\"", 1824, 1);
#nullable restore
#line 35 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
WriteAttributeValue("", 1775, DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"), 1775, 49, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                        <div class=""material-datatables"">
                            <table id=""ApprovedCreditNotesTable"" class=""table table-striped table-no-bordered table-hover"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                <thead>
                                    <tr>
                                        <th width=""10%"">");
#nullable restore
#line 40 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["InvoiceID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th width=\"15%\">");
#nullable restore
#line 41 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["JobID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th width=\"10%\">");
#nullable restore
#line 42 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["Date"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th width=\"20%\">");
#nullable restore
#line 43 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["Client"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th width=\"10%\">");
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["PreTaxAmount"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th width=\"20%\">");
#nullable restore
#line 45 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["Tax"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th width=\"15%\">");
#nullable restore
#line 46 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
                                                   Write(Localizer["Full Amount"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n\r\n");
            WriteLiteral(@"                                    </tr>

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
");
            DefineSection("scripts", async() => {
                WriteLiteral("\r\n\r\n\r\n    <!-- BEGIN PAGE LEVEL PLUGINS -->\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c8876cadb50f8693ae3d1c269f2c829e87d4fdc610060", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 3406, "~/assets/js/pages/ApprovedCreditNotes.js?v=", 3406, 43, true);
#nullable restore
#line 78 "D:\Project\abledocs\abledoc\Views\ApprovedCreditNotes\Index.cshtml"
AddHtmlAttributeValue("", 3449, abledoc.Utility.CommonHelper.Version, 3449, 37, false);

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
