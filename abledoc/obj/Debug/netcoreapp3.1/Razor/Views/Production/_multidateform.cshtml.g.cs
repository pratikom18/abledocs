#pragma checksum "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b30674368773061dc59548ad0803c60f3efd3e9c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Production__multidateform), @"mvc.1.0.view", @"/Views/Production/_multidateform.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b30674368773061dc59548ad0803c60f3efd3e9c", @"/Views/Production/_multidateform.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Production__multidateform : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<abledoc.Models.Production>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("assigndateform"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"modal fade\" id=\"bootstrap-modal\" role=\"dialog\">\r\n    <div class=\"modal-dialog\">\r\n        <div class=\"modal-content\">\r\n\r\n            <!-- Modal Header -->\r\n            <div class=\"modal-header\">\r\n                <h4 class=\"modal-title\">");
#nullable restore
#line 12 "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml"
                                   Write(Localizer["Update Multi Assign Date"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>\r\n            </div>\r\n\r\n            <!-- Modal body -->\r\n            <div class=\"modal-body\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b30674368773061dc59548ad0803c60f3efd3e9c4820", async() => {
                WriteLiteral("\r\n\r\n");
                WriteLiteral("                    <div class=\"row\">\r\n                        <div class=\"form-group col-md-4\">\r\n                            <label for=\"Deadline\" class=\"col-form-label\">");
#nullable restore
#line 23 "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml"
                                                                    Write(Localizer["DeadLine Date"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</label>
                        </div>
                        <div class=""form-group col-md-8"">
                            <input type=""text"" class=""form-control datepicker"" name=""Deadline"" id=""Deadline"" autocomplete=""off"" aria-describedby=""Deadline Date"" required>
                        </div>

                    </div>
                    <div class=""row"">
                        <div class=""form-group col-md-4"">
                            <label for=""DeadlineTime"" class=""col-form-label"">");
#nullable restore
#line 32 "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml"
                                                                        Write(Localizer["Deadline Time"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</label>
                        </div>
                        <div class=""form-group col-md-8"">
                            <input type=""text"" class=""form-control timepicker"" name=""DeadlineTime"" id=""DeadlineTime"" autocomplete=""off"" aria-describedby=""Deadline Time"" required>
                        </div>

                    </div>

                    <!-- Modal footer -->
                    <div class=""modal-footer"">
                        <button type=""button"" class=""btn btn-primary-1"" id=""saveDate"">");
#nullable restore
#line 42 "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml"
                                                                                 Write(Localizer["Save"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                        &nbsp;&nbsp;\r\n                        <button type=\"button\" class=\"btn btn-danger\" data-dismiss=\"modal\">");
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\Production\_multidateform.cshtml"
                                                                                     Write(Localizer["Close"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                    </div>\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<abledoc.Models.Production> Html { get; private set; }
    }
}
#pragma warning restore 1591
