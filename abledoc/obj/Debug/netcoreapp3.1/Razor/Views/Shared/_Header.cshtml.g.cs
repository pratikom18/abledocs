#pragma checksum "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "012ebe99ff7632e71fb8c9d2a19eaebe8fd691fa"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Header), @"mvc.1.0.view", @"/Views/Shared/_Header.cshtml")]
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
#line 1 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"012ebe99ff7632e71fb8c9d2a19eaebe8fd691fa", @"/Views/Shared/_Header.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4541238ecace9825e26c3b3c9bf67b64ce13ac7", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Header : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("logo-mini-1"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Resources/Images/Logo/logo-positive-sm.svg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("AbleDocs"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("logo-normal-1"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Resources/Images/Logo/logo-negative.svg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 5 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
  
    string firstid = string.Empty;
    string secondid = string.Empty;
    string thiredid = string.Empty;
    string FirstName = string.Empty;
    string LastName = string.Empty;
    try
    {
        FirstName = HttpContextAccessor.HttpContext.Session.GetString("FirstName");
        if (FirstName == null)
        {
            FirstName = "";
        }
    }
    catch (Exception ex)
    {

    }

    try
    {
        LastName = HttpContextAccessor.HttpContext.Session.GetString("LastName");
        if (LastName == null)
        {
            LastName = "";
        }
    }
    catch (Exception ex)
    {

    }

    try
    {
        firstid = HttpContextAccessor.HttpContext.Session.GetString("firstid");
        if (firstid == null)
        {
            firstid = "0";
        }
    }
    catch (Exception ex)
    {

    }
    try
    {
        secondid = HttpContextAccessor.HttpContext.Session.GetString("secondid");
        if (secondid == null)
        {
            secondid = "0";
        }
    }
    catch (Exception ex)
    {

    }
    try
    {
        thiredid = HttpContextAccessor.HttpContext.Session.GetString("thiredid");
        if (thiredid == null)
        {
            thiredid = "0";
        }
    }
    catch (Exception ex)
    {

    }

    List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();

    AssignedMenuList = abledoc.Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContextAccessor.HttpContext.Session, "AssignedMenu1");
    string url1 = @Context.Request.Path;
    int parentid = 0;
    if (url1.Split('/').Count() > 1)
    {

        if (url1 == "/")
        {

            var h = AssignedMenuList.Where(s => s.pageurl.Contains("home"));

            var i = AssignedMenuList.Where(s => s.pageurl.Contains("home")).Select(t => t.menumasterid);

            parentid = abledoc.Utility.CommonHelper.GetDBInt(i.FirstOrDefault());

        }
        else
        {

            AssignedMenu p = AssignedMenuList.Where(a => a.pageurl.Contains(url1)).FirstOrDefault();

            var q = AssignedMenuList.Where(a => a.pageurl.Contains(url1)).Select(b => b.ParentID);
            if (abledoc.Utility.CommonHelper.GetDBInt(q.FirstOrDefault()) != 0)
            {
                parentid = abledoc.Utility.CommonHelper.GetDBInt(q.FirstOrDefault());
            }
            else
            {
                if (p != null)
                {
                    parentid = p.menumasterid;
                }

            }


            var m = AssignedMenuList.Where(x => x.menumasterid == parentid).Select(n => n.ParentID);

            if (abledoc.Utility.CommonHelper.GetDBInt(m.FirstOrDefault()) != 0)
            {
                parentid = abledoc.Utility.CommonHelper.GetDBInt(m.FirstOrDefault());
            }


        }





        //  var s1 = AssignedMenuList.Where(x => x.pageurl.Contains(url1.Split('/')[1])).Select(x => x.ParentID);

        // int parentID = abledoc.Utility.CommonHelper.GetDBInt(AssignedMenuList.Where(j => j.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(AssignedMenuList.Where(x => x.pageurl.Contains(url1.Split('/')[1])).Select(x => x.ParentID))).Select(i => i.menumasterid));

        //  string s = "";
    }



#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""sidebar sidebar-1"" data-color=""azure"" data-background-color=""blue"">
    <!--
        Tip 1: You can change the color of the sidebar using: data-color=""purple | azure | green | orange | danger""

        Tip 2: you can also add an image using data-image tag
    -->
");
            WriteLiteral("\r\n    <div class=\"logo\">\r\n        <a href=\"/\" class=\"simple-text logo-mini\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "012ebe99ff7632e71fb8c9d2a19eaebe8fd691fa9110", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </a>\r\n\r\n        <a href=\"/\" class=\"simple-text logo-normal\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "012ebe99ff7632e71fb8c9d2a19eaebe8fd691fa10399", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </a>\r\n    </div>\r\n    <div class=\"sidebar-wrapper\">\r\n        <div class=\"user\">\r\n            <div class=\"photo\">\r\n                <i class=\"material-icons\" style=\"font-size:36px;color:white;\">account_circle</i>\r\n");
            WriteLiteral("            </div>\r\n            <div class=\"user-info\">\r\n                <a data-toggle=\"collapse\" href=\"#collapseExample\" class=\"username\">\r\n                    <span style=\"color:white;\">\r\n                        ");
#nullable restore
#line 163 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                   Write(FirstName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 163 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                              Write(LastName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </span>\r\n                </a>\r\n            </div>\r\n        </div>\r\n        <ul class=\"nav\">\r\n");
#nullable restore
#line 169 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
             if (AssignedMenuList != null)
            {
                if (AssignedMenuList.Count > 0)
                {
                    foreach (var parentMenu in AssignedMenuList)
                    {
                        if (parentMenu.ParentID == 0)
                        {
                            

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li");
            BeginWriteAttribute("class", " class=\"", 5402, "\"", 5610, 2);
            WriteAttributeValue("", 5410, "nav-item", 5410, 8, true);
#nullable restore
#line 178 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 5418, (parentMenu.menumasterid == parentid ||(abledoc.Utility.CommonHelper.GetDBInt(firstid)!=0?parentMenu.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(firstid):false)) ? "active" : "" , 5419, 191, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <a");
            BeginWriteAttribute("href", " href=\"", 5648, "\"", 5796, 1);
#nullable restore
#line 179 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 5655, AssignedMenuList.Where(x=>x.ParentID == parentMenu.menumasterid && x.ParentID != 0).Count() > 0?"#"+parentMenu.menuname:parentMenu.pageurl, 5655, 141, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 5797, "\"", 5928, 2);
            WriteAttributeValue("", 5805, "nav-link", 5805, 8, true);
#nullable restore
#line 179 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 5813, AssignedMenuList.Where(x=>x.ParentID == parentMenu.menumasterid && x.ParentID != 0).Count() > 0?" collapse ":"", 5814, 114, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" ");
#nullable restore
#line 179 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                                                                                                                                                                                                                                        Write(AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid && x.ParentID != 0).Count() > 0 ? " data-toggle=collapse" : "");

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 179 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                                                            Write((AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid).ToList().Count > 0) && (parentMenu.menumasterid == parentid || (abledoc.Utility.CommonHelper.GetDBInt(firstid) != 0 ? parentMenu.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(firstid) : false)) ? "aria-expanded=true" : "aria-expanded=false");

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 179 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            Write(AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid && x.ParentID != 0).Count() > 0 ? "" : "onclick=setMenu(" + parentMenu.menumasterid + ",0,0)");

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n");
            WriteLiteral("                                    <i class=\"material-icons\">");
#nullable restore
#line 181 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                         Write(parentMenu.IconName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</i>\r\n                                    <p class=\"p-menuname\">\r\n          \r\n\r\n                                        ");
#nullable restore
#line 185 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                   Write(Localizer[parentMenu.menuname]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 187 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                         if (AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid && x.ParentID != 0).Count() > 0)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <b class=\"caret\"></b>\r\n");
#nullable restore
#line 190 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    </p>\r\n                                </a>\r\n");
#nullable restore
#line 193 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                 if (AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid).ToList().Count > 0)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <div");
            BeginWriteAttribute("id", " id=\"", 7453, "\"", 7478, 1);
#nullable restore
#line 195 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 7458, parentMenu.menuname, 7458, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 7479, "\"", 7775, 2);
            WriteAttributeValue("", 7487, "collapse", 7487, 8, true);
#nullable restore
#line 195 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 7495, (AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid).ToList().Count > 0) && (parentMenu.menumasterid == parentid ||(abledoc.Utility.CommonHelper.GetDBInt(firstid)!=0?parentMenu.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(firstid):false)) ? "show" : "", 7496, 279, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                        <ul class=\"nav\" ");
#nullable restore
#line 196 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                    Write(ViewBag.Pin == "true" ? "" : "style=padding-left:7%;");

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n");
#nullable restore
#line 197 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                             foreach (var subMenu in AssignedMenuList.Where(x => x.ParentID == parentMenu.menumasterid))
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <li");
            BeginWriteAttribute("class", " class=\"", 8130, "\"", 8358, 2);
            WriteAttributeValue("", 8138, "nav-item", 8138, 8, true);
#nullable restore
#line 199 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 8146, (url1.Contains(subMenu.pageurl.ToLower().Replace(" ", "")) ||(abledoc.Utility.CommonHelper.GetDBInt(secondid)!=0?subMenu.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(secondid):false)) ? "active" : "", 8147, 211, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                    <a");
            BeginWriteAttribute("href", " href=\"", 8416, "\"", 8558, 1);
#nullable restore
#line 200 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 8423, AssignedMenuList.Where(x=>x.ParentID == @subMenu.menumasterid && x.ParentID != 0).Count() > 0?"#"+@subMenu.menuname:@subMenu.pageurl, 8423, 135, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 8559, "\"", 8688, 2);
            WriteAttributeValue("", 8567, "nav-link", 8567, 8, true);
#nullable restore
#line 200 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 8575, AssignedMenuList.Where(x=>x.ParentID == @subMenu.menumasterid && x.ParentID != 0).Count() > 0?" collapse ":"", 8576, 112, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" ");
#nullable restore
#line 200 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                                                                                                                                                                                                                                                    Write(AssignedMenuList.Where(x => x.ParentID == @subMenu.menumasterid && x.ParentID != 0).Count() > 0 ? " data-toggle=collapse" : "");

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 200 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                                                                      Write((AssignedMenuList.Where(x => x.ParentID == @subMenu.menumasterid).ToList().Count > 0) && (url1.Contains(subMenu.menuname.ToLower().Replace(" ", "")) || (abledoc.Utility.CommonHelper.GetDBInt(secondid) != 0 ? subMenu.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(secondid) : false)) ? "aria-expanded=true" : "aria-expanded=false");

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 200 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          Write(AssignedMenuList.Where(x => x.ParentID == subMenu.menumasterid && x.ParentID != 0).Count() > 0 ? "" : "onclick=setMenu(" + parentMenu.menumasterid + "," + subMenu.menumasterid + ",0)");

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n");
            WriteLiteral("                                                        <i class=\"material-icons\">");
#nullable restore
#line 202 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                             Write(subMenu.IconName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</i>\r\n                                                        <p class=\"p-menuname\">\r\n                                                            ");
#nullable restore
#line 204 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                       Write(Localizer[subMenu.menuname]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 205 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                             if (AssignedMenuList.Where(x => x.ParentID == subMenu.menumasterid && x.ParentID != 0).Count() > 0)
                                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <b class=\"caret\"></b>\r\n");
#nullable restore
#line 208 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                        </p>\r\n                                                    </a>\r\n");
#nullable restore
#line 211 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                     if (AssignedMenuList.Where(x => x.ParentID == subMenu.menumasterid).ToList().Count > 0)
                                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                        <div");
            BeginWriteAttribute("id", " id=\"", 10484, "\"", 10506, 1);
#nullable restore
#line 213 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 10489, subMenu.menuname, 10489, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 10507, "\"", 10822, 2);
            WriteAttributeValue("", 10515, "collapse", 10515, 8, true);
#nullable restore
#line 213 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 10523, (AssignedMenuList.Where(x => x.ParentID == subMenu.menumasterid).ToList().Count > 0) && (url1.Contains(subMenu.menuname.ToLower().Replace(" ", "")) ||(abledoc.Utility.CommonHelper.GetDBInt(secondid)!=0?subMenu.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(secondid):false)) ? "show" : "", 10524, 298, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                            <ul class=\"nav\" ");
#nullable restore
#line 214 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                        Write(ViewBag.Pin == "true" ? "" : "style=padding-left:7%;");

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n");
#nullable restore
#line 215 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                 foreach (var subMenu1 in AssignedMenuList.Where(x => x.ParentID == subMenu.menumasterid))
                                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                    <li");
            BeginWriteAttribute("class", " class=\"", 11255, "\"", 11486, 2);
            WriteAttributeValue("", 11263, "nav-item", 11263, 8, true);
#nullable restore
#line 217 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue(" ", 11271, (url1.Contains(subMenu1.pageurl.ToLower().Replace(" ", "")) ||(abledoc.Utility.CommonHelper.GetDBInt(thiredid)!=0?subMenu1.menumasterid == abledoc.Utility.CommonHelper.GetDBInt(thiredid):false)) ? "active" : "" , 11272, 214, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                                        <a");
            BeginWriteAttribute("href", " href=\"", 11564, "\"", 11588, 1);
#nullable restore
#line 218 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 11571, subMenu1.pageurl, 11571, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"nav-link\"");
            BeginWriteAttribute("onclick", " onclick=\"", 11606, "\"", 11694, 7);
            WriteAttributeValue("", 11616, "setMenu(", 11616, 8, true);
#nullable restore
#line 218 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 11624, parentMenu.menumasterid, 11624, 24, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 11648, ",", 11648, 1, true);
#nullable restore
#line 218 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 11649, subMenu.menumasterid, 11649, 21, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 11670, ",", 11670, 1, true);
#nullable restore
#line 218 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
WriteAttributeValue("", 11671, subMenu1.menumasterid, 11671, 22, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 11693, ")", 11693, 1, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n");
            WriteLiteral("                                                                            <i class=\"material-icons\">");
#nullable restore
#line 220 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                                                 Write(subMenu1.IconName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</i>\r\n                                                                            <p class=\"p-menuname\">\r\n                                                                                ");
#nullable restore
#line 222 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                           Write(Localizer[subMenu1.menuname]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                                                                            </p>\r\n                                                                        </a>\r\n\r\n                                                                    </li>\r\n");
#nullable restore
#line 228 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                            </ul>\r\n                                                        </div>\r\n");
#nullable restore
#line 231 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                </li>\r\n");
#nullable restore
#line 233 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        </ul>\r\n                                    </div>\r\n");
#nullable restore
#line 236 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </li>\r\n");
#nullable restore
#line 239 "D:\Project\abledocs\abledoc\Views\Shared\_Header.cshtml"
                        }
                    }
                }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </ul>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; private set; }
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