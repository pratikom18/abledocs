#pragma checksum "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9631cf3d9f0893d60080b2abc8644e44118e2d39"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_EmailTemplate_Create), @"mvc.1.0.view", @"/Views/EmailTemplate/Create.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9631cf3d9f0893d60080b2abc8644e44118e2d39", @"/Views/EmailTemplate/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_EmailTemplate_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<EmailTemplate>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "text", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("subject"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("emailtemplateform"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.TextAreaTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_TextAreaTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""modal fade"" id=""bootstrap-modal"" role=""dialog"">
    <div class=""modal-dialog"">
        <!-- Modal content-->
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"">×</button>
                <h4 class=""modal-title"">
");
#nullable restore
#line 12 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                     if (ViewBag.PageMode == "Create")
                    {
                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                   Write(Localizer["Create"]);

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                            
                    }
                    else if (ViewBag.PageMode == "Update")
                    {
                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                   Write(Localizer["Update"]);

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                            
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </h4>\r\n            </div>\r\n            <div id=\"demo-modal\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9631cf3d9f0893d60080b2abc8644e44118e2d397211", async() => {
                WriteLiteral("\r\n                    <div class=\"card\">\r\n                        <div class=\"card-body\">\r\n                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "9631cf3d9f0893d60080b2abc8644e44118e2d397594", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 26 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.id);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n\r\n\r\n                            <div class=\"row language-div\">\r\n                                <label for=\"Language\" class=\"col-form-label col-sm-3\">");
#nullable restore
#line 31 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                                                                 Write(Localizer["Language"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" <span class=\"text-danger\">*</span></label>\r\n                                <div class=\"col-md-9\">\r\n                                    <div class=\"form-group\">\r\n\r\n                                        ");
#nullable restore
#line 35 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                   Write(Html.DropDownList("Language", abledoc.Utility.ComboHelper.GetCommonMasterList("Language", Model.language), Localizer["Select"].Value, new { @id = "Language", @class = "form-control select2", @datastyle = "btn btn-link", @name = "Language", @placeholder = "Language" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"

                                    </div>
                                </div>
                            </div>
                            <div class=""row"">
                                <label for=""subject"" class=""col-form-label col-sm-3"">");
#nullable restore
#line 41 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                                                                Write(Localizer["Subject"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" <span class=\"text-danger\">*</span></label>\r\n                                <div class=\"col-md-9\">\r\n                                    <div class=\"form-group\">\r\n\r\n                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "9631cf3d9f0893d60080b2abc8644e44118e2d3911209", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
#nullable restore
#line 45 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.subject);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"

                                    </div>
                                </div>
                            </div>
                            <div class=""row"">
                                <label for=""email_for"" class=""col-form-label col-sm-3"">");
#nullable restore
#line 51 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                                                                  Write(Localizer["Email For"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" <span class=\"text-danger\">*</span></label>\r\n                                <div class=\"col-md-9\">\r\n                                    <div class=\"form-group\">\r\n\r\n                                        ");
#nullable restore
#line 55 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                   Write(Html.DropDownList("email_for", abledoc.Utility.ComboHelper.GetCommonMasterList("EmailFor", Model.email_for), Localizer["Select"].Value, new { @id = "email_for", @class = "form-control select2", @datastyle = "btn btn-link", @name = "email_for", @placeholder = "Email For" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"

                                    </div>
                                </div>
                            </div>

                            <div class=""row"">
                                <label for=""Information"" class=""col-form-label col-sm-3"">");
#nullable restore
#line 62 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                                                                    Write(Localizer["Delivery Email"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" <span class=\"text-danger\">*</span></label>\r\n                                <div class=\"col-md-9\">\r\n                                    <div class=\"form-group\">\r\n\r\n                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("textarea", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9631cf3d9f0893d60080b2abc8644e44118e2d3915123", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_TextAreaTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.TextAreaTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_TextAreaTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#nullable restore
#line 66 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_TextAreaTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.DeliveryEmail);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_TextAreaTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"

                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                    <!-- Modal footer -->
                    <div class=""modal-footer"">
                        <div class=""col-sm-2""><button type=""button"" class=""btn btn-primary-1"" id=""saveContent"">");
#nullable restore
#line 77 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                                                                                          Write(Localizer["Save"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button></div>\r\n                        <div class=\"col-sm-2\"><button type=\"button\" class=\"btn btn-danger\" data-dismiss=\"modal\">");
#nullable restore
#line 78 "D:\Project\abledocs\abledoc\Views\EmailTemplate\Create.cshtml"
                                                                                                           Write(Localizer["Close"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button></div>\r\n\r\n                    </div>\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
            WriteLiteral(@"    <script>
        $(""#emailtemplateform"").validate({
            rules: {
                Language: ""required"",
                email_for: ""required"",
                DeliveryEmail: ""required"",
                subject: ""required""

            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                if (element.hasClass('select2') && element.next('.select2-container').length) {
                    error.insertAfter(element.next('.select2-container'));
                } else {
                    error.insertAfter(element.closest('.form-control'));
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });
    </script>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<EmailTemplate> Html { get; private set; }
    }
}
#pragma warning restore 1591