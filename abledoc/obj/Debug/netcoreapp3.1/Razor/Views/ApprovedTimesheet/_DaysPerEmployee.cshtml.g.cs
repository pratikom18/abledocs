#pragma checksum "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eebf5253d1e9ecc1a7785eba0880784a6fb40b65"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ApprovedTimesheet__DaysPerEmployee), @"mvc.1.0.view", @"/Views/ApprovedTimesheet/_DaysPerEmployee.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"eebf5253d1e9ecc1a7785eba0880784a6fb40b65", @"/Views/ApprovedTimesheet/_DaysPerEmployee.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_ApprovedTimesheet__DaysPerEmployee : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ApprovedTimesheet>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""modal fade"" id=""modal-daysperemaployee"" role=""dialog"">
    <div class=""modal-dialog"" style="" max-width: 40%;"">
        <div class=""modal-content"">

            <!-- Modal Header -->
            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"">&times;</button>
            </div>

            <!-- Modal body -->
            <div class=""modal-body"" style=""max-height: calc(100vh - 200px); overflow-y: auto;"">

                <div class=""card"">
                    <div class=""card-body"">
                        <div class=""row col-md-12"">
                            <div class=""form-group col-sm-3"">
                                <label for=""empname"" class=""col-form-label"">");
#nullable restore
#line 20 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                       Write(Localizer["Employee Name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            </div>\r\n                            <div class=\"form-group col-sm-9\">\r\n                                <input type=\"text\" name=\"empname\" class=\"form-control\" id=\"empname\"");
            BeginWriteAttribute("value", " value=\"", 1099, "\"", 1120, 1);
#nullable restore
#line 23 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 1107, ViewBag.name, 1107, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("placeholder", " placeholder=", 1121, "", 1161, 1);
#nullable restore
#line 23 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 1134, Localizer["Employee Name"], 1134, 27, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" readonly>
                            </div>
                        </div>
                        <div class=""row col-md-12"">
                            <div class=""form-group col-sm-3"">
                                <label for=""week"" class=""col-form-label"">");
#nullable restore
#line 28 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                    Write(Localizer["Week"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            </div>\r\n                            <div class=\"form-group col-sm-9\">\r\n                                <input type=\"text\" name=\"week\" class=\"form-control\" id=\"week\"");
            BeginWriteAttribute("value", " value=\"", 1651, "\"", 1684, 1);
#nullable restore
#line 31 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 1659, Model.timesheetWeekRange, 1659, 25, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("placeholder", " placeholder=", 1685, "", 1716, 1);
#nullable restore
#line 31 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 1698, Localizer["Week"], 1698, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" readonly>\r\n                            </div>\r\n                        </div>\r\n\r\n                        <div class=\"row\">\r\n                            <div class=\"col-md-12\">\r\n");
#nullable restore
#line 37 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                 for (int i = 0; i < Model.weeklyHourList.Count && i < 14; i++)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <div");
            BeginWriteAttribute("class", " class=\"", 2066, "\"", 2074, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                                        <div");
            BeginWriteAttribute("class", " class=\"", 2122, "\"", 2130, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                                            <div class=\"card-collapse\">\r\n                                                <div class=\"card-header\" role=\"tab\"");
            BeginWriteAttribute("id", " id=\"", 2290, "\"", 2310, 1);
#nullable restore
#line 42 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 2295, "heading_"+i, 2295, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                    <h5 class=\"mb-0\">\r\n                                                        <a data-toggle=\"collapse\"");
            BeginWriteAttribute("href", " href=\"", 2466, "\"", 2507, 1);
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 2473, "#collapseApprovedTimesheet_"+i, 2473, 34, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" aria-expanded=\"false\" aria-controls=\"collapseTwo\" class=\"collapsed accordion\">\r\n                                                            <label class=\"col-sm-6 ApprovedTimesheet-label text-left\">");
#nullable restore
#line 45 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                                                                 Write(Model.weekDays[i]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                                                            <label class=\"col-sm-5 ApprovedTimesheet-label text-right\">");
#nullable restore
#line 46 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                                                                   Write(Model.weeklyHourList[i] == "0" ? "0.00": String.Format("{0:0.00}", abledoc.Utility.CommonHelper.GetDBDouble(Model.weeklyHourList[i])));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                                                            <i class=""material-icons"">keyboard_arrow_down</i>
                                                        </a>
                                                    </h5>
                                                </div>
                                                <div");
            BeginWriteAttribute("id", " id=\"", 3340, "\"", 3378, 1);
#nullable restore
#line 51 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 3345, "collapseApprovedTimesheet_"+i, 3345, 33, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"collapse\" role=\"tabpanel\"");
            BeginWriteAttribute("aria-labelledby", " aria-labelledby=\"", 3412, "\"", 3445, 1);
#nullable restore
#line 51 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
WriteAttributeValue("", 3430, "heading_"+i, 3430, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-parent=\"#accordion\"");
            BeginWriteAttribute("style", " style=\"", 3471, "\"", 3479, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                    <div class=\"card-body\">\r\n");
#nullable restore
#line 53 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                         if (Model.weeklyHourList[i] != "0")
                                                        {


#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                                            <div class=""material-datatables"">
                                                                <table id=""datatables1"" class=""table table-striped table-no-bordered table-hover"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                                                    <tbody>
");
#nullable restore
#line 59 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                         if (Model.clubbedFileList1[i] != null)
                                                                        {
                                                                            List<clubbedFile> ListclubbedFile = Model.clubbedFileList1[i];

                                                                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 63 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                             foreach (clubbedFile item in ListclubbedFile)
                                                                            {
                                                                                double textValTime = 0.00;
                                                                                if (item.OverrideTime != 0.00)
                                                                                {
                                                                                    textValTime = item.OverrideTime;
                                                                                }
                                                                                else
                                                                                {
                                                                                    textValTime = item.textValTime;
                                                                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                                                                <tr>
                                                                                    <td>
                                                                                        ");
#nullable restore
#line 76 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                                   Write(item.QueryType);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                                                    </td>\r\n                                                                                    <td class=\"text-right\">");
#nullable restore
#line 78 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                                                       Write(String.Format("{0:0.00}", abledoc.Utility.CommonHelper.GetDBDouble(textValTime)));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                                                                </tr>\r\n");
#nullable restore
#line 80 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 80 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                             
                                                                        }
                                                                        

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                    </tbody>\r\n                                                                </table>\r\n                                                            </div>\r\n");
#nullable restore
#line 113 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
");
#nullable restore
#line 120 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("                            </div>\r\n\r\n\r\n                        </div>\r\n\r\n\r\n                    </div>\r\n                </div>\r\n            </div>\r\n            <div class=\"modal-footer\">\r\n                <div");
            BeginWriteAttribute("class", " class=\"", 41343, "\"", 41351, 0);
            EndWriteAttribute();
            WriteLiteral("><button type=\"button\" class=\"btn btn-danger modal-cls\" data-dismiss=\"modal\">");
#nullable restore
#line 490 "D:\Project\abledocs\abledoc\Views\ApprovedTimesheet\_DaysPerEmployee.cshtml"
                                                                                                     Write(Localizer["Close"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</button></div>\r\n            </div>\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ApprovedTimesheet> Html { get; private set; }
    }
}
#pragma warning restore 1591