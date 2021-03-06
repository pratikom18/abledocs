#pragma checksum "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7babd8bdf432a6dddf4f7262653c26e0cb270f52"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_QcFiles__PhaseButton), @"mvc.1.0.view", @"/Views/QcFiles/_PhaseButton.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7babd8bdf432a6dddf4f7262653c26e0cb270f52", @"/Views/QcFiles/_PhaseButton.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_QcFiles__PhaseButton : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
  
    int fixVal = (Convert.ToInt32(ViewBag.fixVal) > 0) ? Convert.ToInt32(ViewBag.fixVal) : 0;
    string status = string.IsNullOrEmpty(ViewBag.status) ? "" : ViewBag.status;
    int p1ToP4 = (Convert.ToInt32(ViewBag.p1ToP4) > 0) ? Convert.ToInt32(ViewBag.p1ToP4) : 0;
    int altTextVal = (Convert.ToInt32(ViewBag.altTextVal) > 0) ? Convert.ToInt32(ViewBag.altTextVal) : 0;
    int p4ToDelivery = (Convert.ToInt32(ViewBag.p4ToDelivery) > 0) ? Convert.ToInt32(ViewBag.p4ToDelivery) : 0;
    List<UserRoles> UserList = ViewBag.UserRolesList;
    bool Reviewer = false;
    if (UserList.Where(x => x.RoleName == "Reviewer").Count() > 0)
    {
        Reviewer = true;
    }
    bool QC = false;
    if (UserList.Where(x => x.RoleName == "QC").Count() > 0)
    {
        QC = true;
    }


#line default
#line hidden
#nullable disable
            WriteLiteral("<input type=\"hidden\" id=\"hdnstatus\"");
            BeginWriteAttribute("value", " value=\"", 924, "\"", 939, 1);
#nullable restore
#line 22 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
WriteAttributeValue("", 932, status, 932, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n<input type=\"hidden\" id=\"hdnQC\"");
            BeginWriteAttribute("value", " value=\"", 976, "\"", 987, 1);
#nullable restore
#line 23 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
WriteAttributeValue("", 984, QC, 984, 3, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n<input type=\"hidden\" id=\"hdnaltTextVal\"");
            BeginWriteAttribute("value", " value=\"", 1032, "\"", 1051, 1);
#nullable restore
#line 24 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
WriteAttributeValue("", 1040, altTextVal, 1040, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n");
#nullable restore
#line 25 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
 if (fixVal == 1)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"form-group row\">\r\n        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"Assign\" name=\"Assign\" onclick=\"CheckIn(1);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 29 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 1"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"ToBeReviewed\" name=\"ToBeReviewed\" onclick=\"CheckIn(2);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 32 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 2"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n");
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 38 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
}
else if (status == "TAGGING")
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <div class=""form-group row"">
        <a class=""btn btn-success btn-primary-1"" id=""messagePopUpLinkID"" href=""#messagePopUpLinkID"" style=""display:none;""></a>
        <button type=""button"" class=""btn btn-success btn-primary-1"" id=""Assign"" name=""Assign"" onclick=""CheckIn(1);"">
            <i class=""fa fa-save""></i> ");
#nullable restore
#line 44 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 1"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"ToBeReviewed\" name=\"ToBeReviewed\" onclick=\"CheckIn(2);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 47 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 2"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n");
#nullable restore
#line 49 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
         if (Reviewer || p1ToP4 == 1)
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 53 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                           
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 55 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
         if (QC)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"phase5Activated\" name=\"ToBeQualityControlled\" onclick=\"CheckIn(5);\">\r\n                <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 58 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                      Write(Localizer["Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </button>\r\n            <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"phase5Locked\" name=\"ToBeQualityControlled\" onclick=\"ShowPopUpMessage();\">\r\n                <i class=\"fa fa-lock\"></i> ");
#nullable restore
#line 61 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                      Write(Localizer["Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </button>\r\n            <div class=\"row\">\r\n                <label class=\"col-md-12 col-form-label\">");
#nullable restore
#line 64 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                                   Write(Localizer["Need Appropriate ALT Status to Activate Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("!</label>\r\n            </div>\r\n");
#nullable restore
#line 66 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 68 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
}
else if (status == "REVIEW")
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"form-group row\">\r\n        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"Assign\" name=\"Assign\" onclick=\"CheckIn(1);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 73 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 1"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"ToBeReviewed\" name=\"ToBeReviewed\" onclick=\"CheckIn(2);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 76 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 2"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n");
            WriteLiteral("        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"phase5Activated\" name=\"ToBeQualityControlled\" onclick=\"CheckIn(5);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 82 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n    </div>\r\n");
#nullable restore
#line 85 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
}
else if (status == "FINAL")
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <div class=""form-group row"">
        <a class=""btn btn-success btn-primary-1"" id=""messagePopUpLinkID"" href=""#messagePopUpLinkID"" style=""display:none;""></a>
        <button type=""button"" class=""btn btn-success btn-primary-1"" id=""Assign"" name=""Assign"" onclick=""CheckIn(1);"">
            <i class=""fa fa-save""></i> ");
#nullable restore
#line 91 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 1"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n");
            WriteLiteral("        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"phase5Activated\" name=\"ToBeQualityControlled\" onclick=\"CheckIn(5);\">\r\n            <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 97 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n        <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"phase5Locked\" name=\"ToBeQualityControlled\" onclick=\"ShowPopUpMessage();\">\r\n            <i class=\"fa fa-lock\"></i> ");
#nullable restore
#line 100 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                  Write(Localizer["Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n        <div class=\"row\">\r\n            <label class=\"col-md-12 col-form-label\">");
#nullable restore
#line 103 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                               Write(Localizer["Need Appropriate ALT Status to Activate Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("!</label>\r\n        </div>\r\n");
#nullable restore
#line 105 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
         if (p4ToDelivery == 1)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"Complete\" name=\"Complete\" onclick=\"CheckIn(6);\">\r\n                <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 108 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                                      Write(Localizer["To Be Delivered"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </button>\r\n");
#nullable restore
#line 110 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 112 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
}
else if (status == "QC")
{
    

#line default
#line hidden
#nullable disable
            WriteLiteral("    <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"ToBeReviewed\" name=\"ToBeReviewed\" onclick=\"CheckIn(2);\">\r\n        <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 119 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                              Write(Localizer["Phase 2"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </button>\r\n    <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"phase5Activated\" name=\"ToBeQualityControlled\" onclick=\"CheckIn(5);\">\r\n        <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 122 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                              Write(Localizer["Phase 3"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </button>\r\n    <button type=\"button\" class=\"btn btn-success btn-primary-1\" id=\"Complete\" name=\"Complete\" onclick=\"CheckIn(6);\">\r\n        <i class=\"fa fa-save\"></i> ");
#nullable restore
#line 125 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
                              Write(Localizer["To Be Delivered"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </button>\r\n");
#nullable restore
#line 127 "D:\Project\abledocs\abledoc\Views\QcFiles\_PhaseButton.cshtml"
}

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>
        $(document).ready(function () {
            if ($(""#hdnstatus"").val() == ""TAGGING"") {
                if ($(""#hdnQC"").val() == ""true"") {
                    var altTextVal = parseInt($(""#hdnaltTextVal"").val());
                    if (altTextVal == 1 || altTextVal == 3 || altTextVal == 4 || altTextVal == 5 || altTextVal == 7) {
                        $(""#phase5Locked"").parent().hide();
                        $(""#phase5LockedMessage"").hide();
                        $(""#phase5Activated"").parent().show();
                    } else {
                        $(""#phase5Activated"").parent().hide();
                        $(""#phase5Locked"").parent().show();
                        $(""#phase5LockedMessage"").show();
                    }
                }
            }
        });
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
