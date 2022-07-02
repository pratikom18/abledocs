#pragma checksum "D:\Project\abledocs\abledoc\Views\Jobs\_IFramePdf.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b30323dae41e8128e7be8937569c3c96514743cc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Jobs__IFramePdf), @"mvc.1.0.view", @"/Views/Jobs/_IFramePdf.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b30323dae41e8128e7be8937569c3c96514743cc", @"/Views/Jobs/_IFramePdf.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Jobs__IFramePdf : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Project\abledocs\abledoc\Views\Jobs\_IFramePdf.cshtml"
  
    int File = (Convert.ToInt32(ViewBag.File) > 0) ? Convert.ToInt32(ViewBag.File) : 0;
    int FileJFV = (Convert.ToInt32(ViewBag.FileJFV) > 0) ? Convert.ToInt32(ViewBag.FileJFV) : 0;
    int LastFile = (Convert.ToInt32(ViewBag.LastFile) > 0) ? Convert.ToInt32(ViewBag.LastFile) : 0;
    int QuoteFile = (Convert.ToInt32(ViewBag.QuoteFile) > 0) ? Convert.ToInt32(ViewBag.QuoteFile) : 0;
    string Type = (!string.IsNullOrEmpty(Convert.ToString(ViewBag.QuoteType))) ? Convert.ToInt32(ViewBag.QuoteType) : "";
    int ID = (Convert.ToInt32(ViewBag.QuoteID) > 0) ? Convert.ToInt32(ViewBag.QuoteID) : 0;

    string Physical_Path = string.Empty;
    ViewBag.databasename = abledoc.Utility.CommonHelper.Getabledocs(ViewBag.flag);
    if (FileJFV > 0)
    {
        JobsFilesVersions modelFile = new JobsFilesVersions();
        modelFile.databasename = ViewBag.databasename;
        modelFile = modelFile.GetFileVersionByID(FileJFV);
        if (modelFile != null)
        {
            Physical_Path = modelFile.Physical_Path;
        }
    }
    else if (File > 0)
    {
        JobsFilesVersions modelFile = new JobsFilesVersions();
        modelFile.databasename = ViewBag.databasename;
        modelFile = modelFile.GetFileVersionStateByFileID(File, "SOURCE");
        if (modelFile != null)
        {
            Physical_Path = modelFile.Physical_Path;
        }

    }
    else if (LastFile > 0)
    {
        JobsFilesVersions modelFile = new JobsFilesVersions();
        modelFile.databasename = ViewBag.databasename;
        modelFile = modelFile.GetFileVersionByID(LastFile);
        if (modelFile != null)
        {
            Physical_Path = modelFile.Physical_Path;
        }
    }
    else if (QuoteFile > 0)
    {
        QuoteTracking modelFile = new QuoteTracking();
        modelFile.databasename = ViewBag.databasename;
        modelFile = modelFile.GetQuoteTrackingByID(QuoteFile);
        if (modelFile != null)
        {
            Physical_Path = modelFile.PhysicalLocation;
        }
    }
    else if (Type != "Quote")
    {
        QuoteTracking quoteTracking = new QuoteTracking();
        quoteTracking.databasename = ViewBag.databasename;
        quoteTracking = quoteTracking.GetQuoteTrackingByJobID(ID);
        if (quoteTracking != null)
        {
            Physical_Path = quoteTracking.PhysicalLocation;
        }

    }
    else if (Type != "Invoice")
    {
        InvoiceTracking invoiceTracking = new InvoiceTracking();
        invoiceTracking.databasename = ViewBag.databasename;
        invoiceTracking = invoiceTracking.GetInvoiceTrackingByJobID(ID);
        if (invoiceTracking != null)
        {
            Physical_Path = invoiceTracking.PhysicalLocation;
        }
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 74 "D:\Project\abledocs\abledoc\Views\Jobs\_IFramePdf.cshtml"
 if (Physical_Path != string.Empty)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"card-body pdfView\">\r\n\r\n\r\n        <embed");
            BeginWriteAttribute("src", " src=\"", 2893, "\"", 2938, 2);
#nullable restore
#line 79 "D:\Project\abledocs\abledoc\Views\Jobs\_IFramePdf.cshtml"
WriteAttributeValue("", 2899, Physical_Path, 2899, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2915, "#navpanes=0&scrollbar=0", 2915, 23, true);
            EndWriteAttribute();
            WriteLiteral(" width=\"100%\" height=\"400px\">\r\n\r\n    </div>\r\n");
#nullable restore
#line 82 "D:\Project\abledocs\abledoc\Views\Jobs\_IFramePdf.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591