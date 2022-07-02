#pragma checksum "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2f4d5ac19abff27a6a086f63bba260fed82e2e27"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_FinalFiles_Index), @"mvc.1.0.view", @"/Views/FinalFiles/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2f4d5ac19abff27a6a086f63bba260fed82e2e27", @"/Views/FinalFiles/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_FinalFiles_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<FinalFile>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/js/pages/timestartstop.js?v=1.2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/js/pages/CommonFiles.js?v=1.2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/js/pages/DownLoadFileTree.js?v=1.2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/js/pages/NoneEngagementTimer.js?v=1.2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 2 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
  
    ViewData["PageTitle"] = "Phase 4 Checked Out";
    ViewData["Title"] = "File #" + Model.jobsFiles.ID + " - " + Model.jobsFiles.Filename;//;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
  
    string lastTimerTotal = "00:00:00";
    if (Model != null)
    {
        ViewBag.ID = Model.jobsFiles.ID;
        ViewBag.FileID = Model.jobsFiles.ID;
        ViewBag.JobID = Model.jobsFiles.JobID;
        ViewBag.CurrentVersionFileID = Model.jobsFiles.CurrentVersionFileID;
        if (Model.allTimers != null)
        {
            lastTimerTotal = Model.allTimers.TotalTimerNow;

            if (lastTimerTotal == "")
            {
                lastTimerTotal = "00:00:00";
            }
        }
        ViewBag.Code = Model.clients.Code;
    }

    ViewBag.State = "FINAL";


#line default
#line hidden
#nullable disable
            DefineSection("Styles", async() => {
                WriteLiteral(@"
    <style type=""text/css"">
        #uploadText {
            width: 100%;
            position: absolute;
            left: 0px;
        }

        #upload ul li, #upload ul li {
            top: 0px;
            height: 75px;
            /*  overflow-y: scroll;*/
        }
    </style>
");
            }
            );
            WriteLiteral("<input type=\"hidden\" id=\"FileID\"");
            BeginWriteAttribute("value", " value=\"", 1142, "\"", 1161, 1);
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
WriteAttributeValue("", 1150, ViewBag.ID, 1150, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n");
#nullable restore
#line 45 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
 if (Model.jobsFiles != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <input type=\"hidden\" name=\"JobID\" id=\"JobID\"");
            BeginWriteAttribute("value", " value=\"", 1249, "\"", 1279, 1);
#nullable restore
#line 47 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
WriteAttributeValue("", 1257, Model.jobsFiles.JobID, 1257, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    <input type=\"hidden\" name=\"ID\" id=\"ID\"");
            BeginWriteAttribute("value", " value=\"", 1325, "\"", 1352, 1);
#nullable restore
#line 48 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
WriteAttributeValue("", 1333, Model.jobsFiles.ID, 1333, 19, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    <input type=\"hidden\" name=\"State\" id=\"State\" value=\"FINAL\" />\r\n    <input type=\"hidden\" name=\"FileType\" id=\"FileType\"");
            BeginWriteAttribute("value", " value=\"", 1477, "\"", 1508, 1);
#nullable restore
#line 50 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
WriteAttributeValue("", 1485, Model.jobsFiles.Status, 1485, 23, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"LastTimerTotal\" ID=\"LastTimerTotal\"");
            BeginWriteAttribute("value", " value=\"", 1580, "\"", 1603, 1);
#nullable restore
#line 51 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
WriteAttributeValue("", 1588, lastTimerTotal, 1588, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    <div class=\"row\">\r\n        <div class=\"col-md-8\">\r\n            <div class=\"row\">\r\n\r\n                ");
#nullable restore
#line 56 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
           Write(await Html.PartialAsync("/views/qcfiles/_DragDropFile.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 57 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
           Write(await Html.PartialAsync("~/Views/QcFiles/_CSRNotes.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral(@"            </div>
            <div class=""row"">
                <div class=""col-md-12"">
                    <div class=""row"">
                        <div class=""col-md-12"">
                            <div class=""card"">
                                <div class=""card-body"">
                                    <div class=""card-collapse"">
                                        <div class=""card-header"" role=""tab"" id=""headingOne"">
                                            <h5 class=""mb-0"">
                                                <a data-toggle=""collapse"" href=""#collapseDownloadFiles"" aria-expanded=""true"" aria-controls=""collapseOne"" class=""collapsed accordion"">
                                                    Download Files
                                                    <i class=""material-icons"">keyboard_arrow_down</i>
                                                </a>
                                            </h5>
                                        </div>
          ");
            WriteLiteral("                              <div id=\"collapseDownloadFiles\" class=\"collapse show\" role=\"tabpanel\" aria-labelledby=\"headingOne\" data-parent=\"#accordion\"");
            BeginWriteAttribute("style", " style=\"", 7270, "\"", 7278, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                                            <div class=\"card-body DownloadFiles\">\r\n                                                ");
#nullable restore
#line 134 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                           Write(await Html.PartialAsync("~/Views/QcFiles/_DownloadFiles.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""row"">
                        <div class=""col-md-12"">
                            <div class=""card"">
                                <div class=""card-body"">
                                    <div class=""card-collapse"">
                                        <div class=""card-header"" role=""tab"" id=""headingPhase5Last"">
                                            <h5 class=""mb-0"">
                                                <a data-toggle=""collapse"" href=""#collapsePhase5Last"" aria-expanded=""true"" aria-controls=""collapsePhase5Last"" class=""collapsed accordion"">
                                                    Comments
                                                    <i class=""material-");
            WriteLiteral(@"icons"">keyboard_arrow_down</i>
                                                </a>
                                            </h5>
                                        </div>
                                        <div id=""collapsePhase5Last"" class=""collapse show"" role=""tabpanel"" aria-labelledby=""headingPhase5Last"" data-parent=""#accordion""");
            BeginWriteAttribute("style", " style=\"", 8854, "\"", 8862, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                                            <div class=""card-body"">
                                                <table id=""datatables3"" class=""example1 table table-striped table-bordered table-hover responsive"" width=""100%"">
                                                    <thead class=""thin-border-bottom"">
                                                        <tr>
");
            WriteLiteral(@"                                                            <th>Comments</th>
                                                            <th>User</th>
                                                            <th>Status</th>
                                                            <th>Date</th>
                                                            <th>Time</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
");
#nullable restore
#line 169 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                         if (Model.jobsFilesCommentsList != null)
                                                        {
                                                            foreach (JobsFilesReviews item1 in Model.jobsFilesCommentsList)
                                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <tr>\r\n");
            WriteLiteral("                                                                    <td>\r\n                                                                        ");
#nullable restore
#line 269 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                   Write(item1.Comments);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                                    </td>\r\n                                                                    <td>\r\n                                                                        ");
#nullable restore
#line 272 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                   Write(item1.FullName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                                    </td>\r\n                                                                    <td>\r\n                                                                        ");
#nullable restore
#line 275 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                   Write(item1.Status);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                                    </td>\r\n                                                                    <td>\r\n                                                                        ");
#nullable restore
#line 278 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                   Write(item1.LastUpdated.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                                    </td>\r\n                                                                    <td>\r\n                                                                        ");
#nullable restore
#line 281 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                   Write(item1.LastUpdated.ToString("hh:mm tt"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                                    </td>\r\n\r\n                                                                </tr>\r\n");
#nullable restore
#line 285 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                            }
                                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

");
#nullable restore
#line 297 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                     if (Model.jobsFiles.Status == "FINAL")
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                        <div class=""row"">
                            <div class=""col-md-12"">
                                <div class=""card"">
                                    <div class=""card-body"">
                                        <div class=""card-collapse"">
                                            <div class=""card-header"" role=""tab"" id=""headingALTText"">
                                                <h5 class=""mb-0"">
                                                    <a data-toggle=""collapse"" href=""#collapseALTText"" aria-expanded=""true"" aria-controls=""collapseALTText"" class=""collapsed accordion"">
                                                        ALT Text
                                                        <i class=""material-icons"">keyboard_arrow_down</i>
                                                    </a>
                                                </h5>
                                            </div>
                                            <div id=""c");
            WriteLiteral("ollapseALTText\" class=\"collapse show\" role=\"tabpanel\" aria-labelledby=\"headingALTText\" data-parent=\"#accordion\"");
            BeginWriteAttribute("style", " style=\"", 23115, "\"", 23123, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                                                <div class=""card-body"">
                                                    <table id=""datatables1"" class=""example1 table table-striped table-bordered table-hover responsive"" width=""100%"">
                                                        <thead class=""thin-border-bottom"">
                                                            <tr>
                                                                <th>Page Num</th>
                                                                <th>Alt-Text</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
");
#nullable restore
#line 322 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                             if (Model.altTextsList != null)
                                                            {
                                                                

#line default
#line hidden
#nullable disable
#nullable restore
#line 324 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                 foreach (AltTexts item1 in Model.altTextsList)
                                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                    <tr>\r\n                                                                        <td>\r\n                                                                            ");
#nullable restore
#line 328 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                       Write(item1.PageNum);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                                        </td>
                                                                        <td>
                                                                            <textarea id=""copyAltText"" name=""copyAltText"" class=""form-control"" readonly>");
#nullable restore
#line 331 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                                                                                                   Write(item1.AltText);

#line default
#line hidden
#nullable disable
            WriteLiteral("</textarea>\r\n                                                                        </td>\r\n\r\n                                                                    </tr>\r\n");
#nullable restore
#line 335 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 335 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                 
                                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
");
#nullable restore
#line 497 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                    
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"row Conversation\">\r\n                        ");
#nullable restore
#line 500 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                   Write(await Html.PartialAsync("/views/file/_Conversation.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                    </div>
                </div>
            </div>


        </div>
        <div class=""col-md-4"">
            <div class=""card"">
                <div class=""card-body"">
                    <div class=""form-group row"">
                        <button type=""button"" class=""btn btn-success btn-primary-1"" data-toggle=""modal"" data-target=""#secondTimer"" onclick=""nonEngagement();"">
                            <i class=""material-icons"">watch_later</i> Non-Engagement Timer
                        </button>
                    </div>
                    <div class=""form-group row"">
                        <label class=""col-md-6 col-form-label text-left"">Client:</label>
                        <label class=""col-md-6 col-form-label text-left"">");
#nullable restore
#line 517 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                    Write(Model.clients.Company);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">Client & Job:</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">");
#nullable restore
#line 519 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                    Write(Model.jobs.EngagementNum);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">File Quoted As:</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">");
#nullable restore
#line 521 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                    Write(Model.jobs.JobQuotedAs);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">DeadLine:</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">");
#nullable restore
#line 523 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                     Write(Model.jobsFiles.Deadline + " @ " + Model.jobsFiles.DeadlineTime  );

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">Pages:</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">");
#nullable restore
#line 525 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                    Write(Model.jobsFiles.Pages);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">Instructions:</label>\r\n                        <label class=\"col-md-6 col-form-label text-left\">");
#nullable restore
#line 527 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                    Write(Model.jobs.Tagging_Instructions);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    </div>\r\n                    <div class=\"form-group row\">\r\n                        <label class=\"col-md-6 col-form-label text-left\" id=\"stopWatch\">");
#nullable restore
#line 530 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                                                                                   Write(lastTimerTotal);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                        <button type=""button"" class=""btn btn-success btn-primary-1"" data-toggle=""modal"" id=""start"" onclick=""start()"">
                            Start
                        </button>
                        <button type=""button"" class=""btn btn-danger"" data-toggle=""modal"" id=""stop"" onclick=""stop()"" style=""display:none;"">
                            Stop
                        </button>
                    </div>
                    <div class=""form-group row"">
                        <div class=""col-md-4"">
                            <label for=""altText"" class=""col-form-label"">Finalizer Comments</label>
                        </div>
                        <div class=""col-md-8"">
                            <textarea class=""form-control"" name=""altText"" rows=""5"" id=""PhaseComments"" placeholder=""Finalizer Comments""></textarea>
                        </div>

                    </div>
                    <div class=""form-group row"">
                        <label for");
            WriteLiteral("=\"altText\" class=\"col-md-4 col-form-label\">ALT-Text</label>\r\n                        <div class=\"col-md-8\">\r\n                            ");
#nullable restore
#line 550 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                       Write(Html.DropDownList("altTextStatusID", abledoc.Utility.ComboHelper.GetAltTextStatusList(Model.jobsFiles.AltTxt.ToString()), "Select", new { @id = "altTextStatusID", @class = "form-control selectpicker", @datastyle = "btn btn-link", @name = "altTextStatusID", @onchange = "UpdateAlt();" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                    <div id=\"phaseButton\" class=\"form-group row\">\r\n");
#nullable restore
#line 554 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                           ViewBag.status = Model.jobsFiles.Status;
                            ViewBag.altTextVal = Model.jobsFiles.AltTxt;
                            ViewBag.p1ToP4 = Model.jobs.P1ToP4;
                            ViewBag.p4ToDelivery = Model.jobs.P4ToDelivery;
                        

#line default
#line hidden
#nullable disable
            WriteLiteral("                        ");
#nullable restore
#line 559 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
                   Write(await Html.PartialAsync("~/views/qcfiles/_PhaseButton.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 565 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("<div id=\"secondTimer\" class=\"modal\">\r\n    ");
#nullable restore
#line 566 "D:\Project\abledocs\abledoc\Views\FinalFiles\Index.cshtml"
Write(await Html.PartialAsync("~/views/qcfiles/_NonEngagementTimer.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"

    <script>
        $(document).ready(function () {

            $('#queryTypeSelect').selectpicker('setStyle', 'btn btn-link');
            $('#altTextStatusID').selectpicker('setStyle', 'btn btn-link');
            $('.filter-option').addClass('filter-option-1');
            $('#datatables1').dataTable();
            $('#datatables2').dataTable();
            $('#datatables3').dataTable();
        });
    </script>

    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2f4d5ac19abff27a6a086f63bba260fed82e2e2730920", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2f4d5ac19abff27a6a086f63bba260fed82e2e2732020", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2f4d5ac19abff27a6a086f63bba260fed82e2e2733120", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2f4d5ac19abff27a6a086f63bba260fed82e2e2734220", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n    <!-- END PAGE LEVEL PLUGINS -->\r\n");
            }
            );
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<FinalFile> Html { get; private set; }
    }
}
#pragma warning restore 1591
