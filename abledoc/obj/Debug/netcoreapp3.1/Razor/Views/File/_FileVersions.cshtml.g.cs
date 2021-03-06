#pragma checksum "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "925206864bc4f5815cd170b02c8a3bbdec24d473"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_File__FileVersions), @"mvc.1.0.view", @"/Views/File/_FileVersions.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"925206864bc4f5815cd170b02c8a3bbdec24d473", @"/Views/File/_FileVersions.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_File__FileVersions : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
  
    JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
    List<JobsFilesVersions> FileVersionList = new List<JobsFilesVersions>();
    objJobsFilesVersions.databasename = ViewBag.databasename;
    FileVersionList = objJobsFilesVersions.GetFileVersionByFileID(ViewBag.ID);


#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""col-md-12"">
    <div class=""card"">
        <div class=""card-body"">
            <div class=""card-collapse"">
                <div class=""card-header"" role=""tab"" id=""headingEleven"">
                    <h5 class=""mb-0"">
                        <a data-toggle=""collapse"" href=""#collapseFileVersions"" aria-expanded=""false"" aria-controls=""collapseEleven"" class=""collapsed accordion"">
                            ");
#nullable restore
#line 17 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                       Write(Localizer["File Versions"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            <i class=""material-icons"">keyboard_arrow_down</i>
                        </a>
                    </h5>
                </div>
                <div id=""collapseFileVersions"" class=""collapse show"" role=""tabpanel"" aria-labelledby=""headingEleven"" data-parent=""#accordion""");
            BeginWriteAttribute("style", " style=\"", 1136, "\"", 1144, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                    <div class=\"card-body\">\r\n");
            WriteLiteral("\r\n\r\n");
            WriteLiteral(@"                        <div class=""material-datatables"">
                            <table id=""datatables7"" class=""table table-striped table-no-bordered table-hover FileVersionsTable"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                <thead class=""thin-border-bottom"">
                                    <tr>
                                        <th style=""width:3%""></th>
                                        <th style=""width:73%"">");
#nullable restore
#line 40 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                         Write(Localizer["File Name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th style=\"width:8%\">");
#nullable restore
#line 41 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                        Write(Localizer["State"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th style=\"width:8%\">");
#nullable restore
#line 42 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                        Write(Localizer["Date"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th style=\"width:8%\">");
#nullable restore
#line 43 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                        Write(Localizer["Time"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                    </tr>\r\n                                </thead>\r\n                                <tbody>\r\n");
#nullable restore
#line 47 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                     if (FileVersionList != null)
                                    {
                                        foreach (JobsFilesVersions item in FileVersionList)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <tr>\r\n                                                <td>\r\n                                                    <input type=\"checkbox\" name=\"assignfiles[]\" class=\"assignfiles\" data-id=\"");
#nullable restore
#line 53 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                                                                                        Write(item.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"");
            BeginWriteAttribute("value", " value=\"", 3226, "\"", 3242, 1);
#nullable restore
#line 53 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
WriteAttributeValue("", 3234, item.ID, 3234, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" form=\"multiassign\">\r\n");
            WriteLiteral("                                                </td>\r\n                                                <td>\r\n                                                    <a");
            BeginWriteAttribute("href", " href=\"", 3499, "\"", 3545, 4);
            WriteAttributeValue("", 3506, "/fileget?ID=", 3506, 12, true);
#nullable restore
#line 57 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
WriteAttributeValue("", 3518, item.ID, 3518, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3526, "&flag=", 3526, 6, true);
#nullable restore
#line 57 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
WriteAttributeValue("", 3532, ViewBag.flag, 3532, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\" class=\"a-1\">");
#nullable restore
#line 57 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                                                                                             Write(item.Filename);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                                                </td>\r\n                                                <td>\r\n                                                    ");
#nullable restore
#line 60 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                Write(item.State == "TAGGING"?"Phase 1": item.State == "REVIEW"? "Phase 2": item.State == "FINAL" ? "Phase 4" : item.State == "QC"? "Phase 3": item.State == "COMPLETE"? "To Be Delivered": item.State == "TOBEDELIVERED"? "To Be Delivered":"");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                </td>\r\n                                                <td>\r\n                                                    ");
#nullable restore
#line 63 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                               Write(item.LastUpdated.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                </td>\r\n                                                <td>\r\n                                                    ");
#nullable restore
#line 66 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                               Write(item.LastUpdated.ToString("hh:mm tt"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                </td>\r\n                                            </tr>\r\n");
#nullable restore
#line 69 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                        }
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                </tbody>\r\n                            </table>\r\n                        </div>\r\n");
            WriteLiteral("                    </div>\r\n                    <div class=\"row\">\r\n                        <div class=\"col-sm-2\">\r\n                            <button type=\"button\" class=\"btn btn-primary btn-sm btn-primary-1 col-sm-12\"");
            BeginWriteAttribute("onclick", " onclick=\'", 4953, "\'", 4990, 3);
            WriteAttributeValue("", 4963, "AddFiles(\"", 4963, 10, true);
#nullable restore
#line 78 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
WriteAttributeValue("", 4973, ViewBag.Status, 4973, 15, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4988, "\")", 4988, 2, true);
            EndWriteAttribute();
            WriteLiteral(" data-toggle=\"modal\" data-target=\"#myModal\">\r\n                                <i class=\"material-icons\">add</i> ");
#nullable restore
#line 79 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                             Write(Localizer["Add Files"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </button>\r\n                        </div>\r\n                        <div class=\"col-sm-2\">\r\n                            <a");
            BeginWriteAttribute("href", " href=\"", 5277, "\"", 5325, 3);
            WriteAttributeValue("", 5284, "/File/downloadFile?ID=", 5284, 22, true);
#nullable restore
#line 83 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
WriteAttributeValue("", 5306, ViewBag.ID, 5306, 11, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5317, "&IDS=all", 5317, 8, true);
            EndWriteAttribute();
            WriteLiteral(" type=\"button\" class=\"btn btn-primary btn-sm btn-primary-1 col-sm-12\">\r\n                                <i class=\"material-icons\">download</i> ");
#nullable restore
#line 84 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                                  Write(Localizer["Download All"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            </a>
                        </div>
                        <div class=""col-sm-2"">
                            <button type=""button"" class=""btn btn-primary btn-sm btn-primary-1 col-sm-12"" onclick='download()'>
                                <i class=""material-icons"">download</i> ");
#nullable restore
#line 89 "D:\Project\abledocs\abledoc\Views\File\_FileVersions.cshtml"
                                                                  Write(Localizer["Download"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </button>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <div>\r\n        </div>\r\n    </div>\r\n</div>");
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
