#pragma checksum "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "146589abf801deec0b1011ee4689c8d83085cafb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Timesheet__addTime), @"mvc.1.0.view", @"/Views/Timesheet/_addTime.cshtml")]
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
#line 2 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"146589abf801deec0b1011ee4689c8d83085cafb", @"/Views/Timesheet/_addTime.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Timesheet__addTime : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("frmAddTime"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("<div class=\"modal-dialog\">\r\n    <div class=\"modal-content\">\r\n\r\n        <!-- Modal Header -->\r\n        <div class=\"modal-header\">\r\n            <h4 class=\"modal-title\">");
#nullable restore
#line 9 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
                               Write(Localizer["Add Time"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n            <button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>\r\n        </div>\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "146589abf801deec0b1011ee4689c8d83085cafb4538", async() => {
                WriteLiteral("\r\n            <!-- Modal body -->\r\n            <div class=\"modal-body\">\r\n                <div class=\"form-group\">\r\n                    ");
#nullable restore
#line 16 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
               Write(Html.DropDownList("queryTypeSelect", abledoc.Utility.ComboHelper.GetTimeTypeNew(), Localizer["Select"].Value, new { @id = "queryTypeSelect", @class = "col-md-12 form-control selectpicker", @datastyle = "btn btn-link", @name = "queryTypeSelect" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <input type=\"number\" name=\"addTimeNumber\" id=\"addTimeNumber\" class=\"form-control\"");
                BeginWriteAttribute("placeholder", " placeholder=", 1040, "", 1075, 1);
#nullable restore
#line 19 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
WriteAttributeValue("", 1053, Localizer["Hours..."], 1053, 22, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <textarea type=\"url\" name=\"messageAddTimer\" id=\"messageAddTimer\" class=\"form-control\"");
                BeginWriteAttribute("placeholder", " placeholder=", 1251, "", 1288, 1);
#nullable restore
#line 22 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
WriteAttributeValue("", 1264, Localizer["Comment..."], 1264, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral("></textarea>\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <input type=\"text\" name=\"addTimerDateExtra\" id=\"addTimerDateExtra\" class=\"form-control addTimerDateExtra\"");
                BeginWriteAttribute("placeholder", " placeholder=", 1493, "", 1527, 1);
#nullable restore
#line 25 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
WriteAttributeValue("", 1506, Localizer["Date..."], 1506, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                </div>\r\n\r\n            </div>\r\n\r\n            <!-- Modal footer -->\r\n            <div class=\"modal-footer\">\r\n                <button type=\"button\" class=\"btn btn-primary-1\" id=\"saveBtn\" onclick=\"AddTimeToTimesheet();\">");
#nullable restore
#line 32 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
                                                                                                        Write(Localizer["Save"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                &nbsp;&nbsp;\r\n                <button type=\"button\" class=\"btn btn-danger\" data-dismiss=\"modal\">");
#nullable restore
#line 34 "D:\Project\abledocs\abledoc\Views\Timesheet\_addTime.cshtml"
                                                                             Write(Localizer["Close"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n            </div>\r\n        ");
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
            WriteLiteral("\r\n\r\n    </div>\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>

                //$(document).ready(function () {

                //    $('.datepicker').datetimepicker({
                //        multidate: true,
                //        format: ""YYYY-MM-DD"",
                //        icons: {
                //            time: ""fa fa-clock-o"",
                //            date: ""fa fa-calendar"",
                //            up: ""fa fa-chevron-up"",
                //            down: ""fa fa-chevron-down"",
                //            previous: ""fa fa-chevron-left"",
                //            next: ""fa fa-chevron-right"",
                //            today: ""fa fa-screenshot"",
                //            clear: ""fa fa-trash"",
                //            close: ""fa fa-remove"",
                //        },
                //    });
                //});

    </script>
");
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