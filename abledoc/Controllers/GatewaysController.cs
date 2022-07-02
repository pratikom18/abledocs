using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class GatewaysController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public static string successmsg { get; set; }
        public GatewaysController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, LoginAuthorized]
        public ActionResult Create(Gateways model)
        {
            try
            {
                model.databasename = Utility.CommonHelper.Getgateway(model.flag);
                var subdomain = model.InsertGateway();

                return RedirectToAction("edit", new { subdomain = subdomain.ToString(),flag=model.flag });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet, LoginAuthorized]
        public ActionResult Create()
        {

            ViewBag.Type = "Add";
            var databasename = HttpContext.Session.GetString("database_gateway");
            ViewBag.databasename = databasename;
            //var databasename1 = HttpContext.Session.GetString("database_gateway_edit");
            //if (databasename != databasename1)
            //{
            //    ViewBag.Disebled = "True";
            //}
            //else
            //{

            //    ViewBag.Disebled = "Fase";
            //}

            Gateways objGatewayMaster = new Gateways();

            GatewaysLogo objGatewaysLogo = new GatewaysLogo();
            ViewBag.ImageUrl = "";

            var objLanguageList = new Dictionary<string, string>(); 
            var objClientNamelist = new Dictionary<string, string>(); 
            var objLogoAltlist = new Dictionary<string, string>(); 
            var objFieldslist = new Dictionary<string, string>();
            
            var objTextWelcomelist = new Dictionary<string, string>();
            
            var objTextThankyoulist = new Dictionary<string, string>();
           
            var objTextEmaillist = new Dictionary<string, string>();
            
            var objFieldsCustom = new List<CustomField>();
           

            // ViewBag.LanguageList = objLanguageList;
            ViewBag.ClientNameList = objClientNamelist;

            var langus = new Dictionary<string, string>();
            langus.Add("EN", "English");
            langus.Add("FR", "French");
            langus.Add("DA", "Danish");
            langus.Add("DE", "German");
            langus.Add("ES", "Spanish");
            langus.Add("SV", "Swedish");
            langus.Add("IS", "Icelandic");
            langus.Add("FI", "Finnish");
            langus.Add("NN", "Norwegian");

            ViewBag.LanguageList = langus;
            ViewBag.LangList = objLanguageList;
            ViewBag.LogoAltlist = objLogoAltlist;

            ViewBag.FieldsList = objFieldslist;
            ViewBag.TextWelcomelist = objTextWelcomelist;
            ViewBag.TextThankyoulist = objTextThankyoulist;
            ViewBag.TextEmaillist = objTextEmaillist;
            ViewBag.FieldsCustomlist = objFieldsCustom;


            var standardFields = new Dictionary<string, string>();
            standardFields.Add("FirstName", "First Name");
            standardFields.Add("LastName", "Last Name");
            standardFields.Add("Phone", "Phone Number");
            standardFields.Add("Email", "Email Address");
            standardFields.Add("Extension", "Extension");
            standardFields.Add("AdditionalContact", "Additional Contact");
            standardFields.Add("BillingContact", "Billing Contact");
            standardFields.Add("Quote", "Quote");
            standardFields.Add("Notes", "Notes");
            ViewBag.StandardFieldList = standardFields;

            var states = new Dictionary<string, string>();
            states.Add("Show", "Show");
            states.Add("Required", "Required");
            states.Add("Hide", "Hide");
            ViewBag.SatesList = states;

            var fieldtype = new Dictionary<string, string>();
            fieldtype.Add("input", "Short text");
            fieldtype.Add("textarea", "Long text");
            fieldtype.Add("checkbox", "Checkbox");
            fieldtype.Add("select", "Dropdown");
            fieldtype.Add("radio", "Radio");
            ViewBag.FieldTypelist = fieldtype;
            ViewBag.Save = successmsg;
            return View("edit",objGatewayMaster);


        }

        [HttpGet, LoginAuthorized]
        public ActionResult Edit(string subdomain,string flag)
        {
            ViewBag.Type = "Edit";
            var databasename = Utility.CommonHelper.Getgateway(flag);
            ViewBag.databasename = databasename;
            //var databasename1 = HttpContext.Session.GetString("database_gateway_edit");
            //if (databasename != databasename1)
            //{
            //    ViewBag.Disebled = "True";
            //}
            //else
            //{

            //    ViewBag.Disebled = "Fase";
            //}

            Gateways objGatewayMaster = new Gateways();
            objGatewayMaster.databasename = databasename;
            objGatewayMaster = objGatewayMaster.GetewayById(subdomain);

            GatewaysLogo objGatewaysLogo = new GatewaysLogo();
            objGatewaysLogo.databasename = databasename;
            objGatewaysLogo = objGatewaysLogo.GetewayById(subdomain);
            //  Languages obj = JsonConvert.DeserializeObject<Languages>(objGatewayMaster.Languages);

            //Console.WriteLine(obj.Country);
            if (objGatewaysLogo != null)
            {
                byte[] bytes = (byte[])objGatewaysLogo.logo;
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                ViewBag.ImageUrl = "data:image/png;base64," + base64String;
            }
            else
            {
                ViewBag.ImageUrl = "";
            }
            var objLanguageList = JsonConvert.DeserializeObject(objGatewayMaster.Languages);
            var objClientNamelist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objGatewayMaster.ClientName);
            var objLogoAltlist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objGatewayMaster.logo_alt);
            var objFieldslist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objGatewayMaster.fields))
            {
                objFieldslist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objGatewayMaster.fields);
            }

            var objTextWelcomelist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objGatewayMaster.text_Welcome))
            {
                objTextWelcomelist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objGatewayMaster.text_Welcome);
            }

            var objTextThankyoulist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objGatewayMaster.text_Thankyou))
            {
                objTextThankyoulist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objGatewayMaster.text_Thankyou);
            }

            var objTextEmaillist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objGatewayMaster.text_Email))
            {
                objTextEmaillist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objGatewayMaster.text_Email);
            }

            var objFieldsCustom = new List<CustomField>();
            if (!string.IsNullOrEmpty(objGatewayMaster.fields_custom) && objGatewayMaster.fields_custom != "null")
            {
                objFieldsCustom = JsonConvert.DeserializeObject<List<CustomField>>(objGatewayMaster.fields_custom);
                if (objFieldsCustom != null)
                {
                    foreach (CustomField item in objFieldsCustom)
                    {
                        if (item.Label == null)
                        {
                            item.Label = new Dictionary<string, string>();
                            item.Label.Add("EN", "");
                        }
                        if (item.Values == null)
                        {
                            item.Values = new Dictionary<string, string>();
                            item.Values.Add("EN", "");
                        }
                        if (item.Pattern == null)
                        {
                            item.Pattern = new Dictionary<string, string>();
                            item.Pattern.Add("EN", "");
                        }
                    }
                }

            }


            // ViewBag.LanguageList = objLanguageList;
            ViewBag.ClientNameList = objClientNamelist;

            var langus = new Dictionary<string, string>();
            langus.Add("EN", "English");
            langus.Add("FR", "French");
            langus.Add("DA", "Danish");
            langus.Add("DE", "German");
            langus.Add("ES", "Spanish");
            langus.Add("SV", "Swedish");
            langus.Add("IS", "Icelandic");
            langus.Add("FI", "Finnish");
            langus.Add("NN", "Norwegian");

            ViewBag.LanguageList = langus;
            ViewBag.LangList = objLanguageList;
            ViewBag.LogoAltlist = objLogoAltlist;

            ViewBag.FieldsList = objFieldslist;
            ViewBag.TextWelcomelist = objTextWelcomelist;
            ViewBag.TextThankyoulist = objTextThankyoulist;
            ViewBag.TextEmaillist = objTextEmaillist;
            ViewBag.FieldsCustomlist = objFieldsCustom;


            var standardFields = new Dictionary<string, string>();
            standardFields.Add("FirstName", "First Name");
            standardFields.Add("LastName", "Last Name");
            standardFields.Add("Phone", "Phone Number");
            standardFields.Add("Email", "Email Address");
            standardFields.Add("Extension", "Extension");
            standardFields.Add("AdditionalContact", "Additional Contact");
            standardFields.Add("BillingContact", "Billing Contact");
            standardFields.Add("Quote", "Quote");
            standardFields.Add("Notes", "Notes");
            ViewBag.StandardFieldList = standardFields;

            var states = new Dictionary<string, string>();
            states.Add("Show", "Show");
            states.Add("Required", "Required");
            states.Add("Hide", "Hide");
            ViewBag.SatesList = states;

            var fieldtype = new Dictionary<string, string>();
            fieldtype.Add("input", "Short text");
            fieldtype.Add("textarea", "Long text");
            fieldtype.Add("checkbox", "Checkbox");
            fieldtype.Add("select", "Dropdown");
            fieldtype.Add("radio", "Radio");
            ViewBag.FieldTypelist = fieldtype;
            ViewBag.Save = successmsg;
            return View(objGatewayMaster);


        }
        [HttpPost, LoginAuthorized]
        public ActionResult Edit(Gateways model)
        {
            try
            {
                Dictionary<string, string> ClientName = new Dictionary<string, string>();
                Dictionary<string, string> logo_alt = new Dictionary<string, string>();
                List<string> Languages = new List<string>();
                List<CustomField> CustomFieldList = new List<CustomField>();
                Dictionary<string, string> LabelList = new Dictionary<string, string>();
                Dictionary<string, string> ValueList = new Dictionary<string, string>();
                Dictionary<string, string> PatternList = new Dictionary<string, string>();
                Dictionary<string, string> TextWelcomeList = new Dictionary<string, string>();
                Dictionary<string, string> TextThankYouList = new Dictionary<string, string>();
                Dictionary<string, string> TextEmailList = new Dictionary<string, string>();
                Dictionary<string, string> FieldList = new Dictionary<string, string>();
                List<string> fields = new List<string>();

                //GatewaysLogo objGatewaysLogo = new GatewaysLogo();
                //objGatewaysLogo.databasename = HttpContext.Session.GetString("database_gateway");
                //var files = HttpContext.Request.Form.Files;


                //foreach (var Image in files)
                //{
                //    if (Image != null && Image.Length > 0)
                //    {
                //        using (var fileStream = new MemoryStream())
                //        {
                //            Image.CopyTo(fileStream);
                //            objGatewaysLogo.contentType = Image.ContentType;
                //            objGatewaysLogo.subdomain = model.subdomain;
                //            objGatewaysLogo.logo = fileStream.ToArray();
                //        }

                //        objGatewaysLogo.databasename = HttpContext.Session.GetString("database_gateway");
                //        objGatewaysLogo.UpdateGateway();
                //    }
                //}
                fields.Add("FirstName");
                fields.Add("LastName");
                fields.Add("Phone");
                fields.Add("Email");
                fields.Add("Extension");
                fields.Add("AdditionalContact");
                fields.Add("BillingContact");
                fields.Add("Quote");
                fields.Add("Notes");

                int Length = 0;
                foreach (string key in Request.Form.Keys)
                {
                    if (key == "Languages[]")
                    {
                        Languages = Request.Form[key].ToString().Split(',').ToList();
                    }
                    else if (key == "fields_custom")
                    {
                        Length = Utility.CommonHelper.GetDBInt(Request.Form[key].ToString());
                    }
                }



                foreach (string item in fields)
                {
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "fields[" + item + "]")
                        {
                            FieldList.Add(item, Request.Form[key]);
                        }
                    }
                }

                for (int i = 0; i < Length; i++)
                {
                    LabelList = new Dictionary<string, string>();
                    ValueList = new Dictionary<string, string>();
                    PatternList = new Dictionary<string, string>();
                    CustomField CustomField = new CustomField();
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "fields_custom[" + i + "][Type]")
                        {
                            CustomField.Type = Request.Form[key].ToString();
                        }
                        else if (key == "fields_custom[" + i + "][State]")
                        {
                            CustomField.State = Request.Form[key].ToString();
                        }
                    }
                    foreach (string lang in Languages)
                    {
                        foreach (string key in Request.Form.Keys)
                        {
                            if (key == "fields_custom[" + i + "][Label][" + lang + "]")
                            {
                                LabelList.Add(lang, Request.Form[key]);
                            }
                            else if (key == "fields_custom[" + i + "][Values][" + lang + "]")
                            {
                                ValueList.Add(lang, Request.Form[key]);
                            }
                            else if (key == "fields_custom[" + i + "][Pattern][" + lang + "]")
                            {
                                PatternList.Add(lang, Request.Form[key]);
                            }
                        }
                    }
                    CustomField.Label = LabelList;
                    CustomField.Values = ValueList;
                    CustomField.Pattern = PatternList;
                    CustomFieldList.Add(CustomField);
                }

                foreach (string key in Request.Form.Keys)
                {
                    if (key == "fieldscustom")
                    {
                        Length = Utility.CommonHelper.GetDBInt(Request.Form[key].ToString());

                        CustomField CustomField = new CustomField();
                        CustomField.Label = LabelList;
                        CustomField.Values = ValueList;
                        CustomField.Pattern = PatternList;
                        CustomFieldList.Add(CustomField);
                    }
                }

                foreach (string lang in Languages)
                {
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "ClientNames[" + lang + "]")
                        {
                            ClientName.Add(lang, Request.Form[key]);
                        }
                        else if (key == "logo_alts[" + lang + "]")
                        {
                            logo_alt.Add(lang, Request.Form[key]);
                        }
                        else if (key == "text_Welcome[" + lang + "]")
                        {
                            TextWelcomeList.Add(lang, Request.Form[key]);
                        }
                        else if (key == "text_Thankyou[" + lang + "]")
                        {
                            TextThankYouList.Add(lang, Request.Form[key]);
                        }
                        else if (key == "text_Email[" + lang + "]")
                        {
                            TextEmailList.Add(lang, Request.Form[key]);
                        }
                    }
                }
                foreach (string lang in Languages)
                {
                    if (!(ClientName.ContainsKey(lang)))
                    {
                        ClientName.Add(lang, "");
                    }
                    if (!(logo_alt.ContainsKey(lang)))
                    {
                        logo_alt.Add(lang, "");
                    }
                    if (!(TextWelcomeList.ContainsKey(lang)))
                    {
                        TextWelcomeList.Add(lang, "");
                    }
                    if (!(TextThankYouList.ContainsKey(lang)))
                    {
                        TextThankYouList.Add(lang, "");
                    }
                    if (!(TextEmailList.ContainsKey(lang)))
                    {
                        TextEmailList.Add(lang, "");
                    }
                }

                foreach (CustomField item in CustomFieldList)
                {
                    foreach (string lang in Languages)
                    {
                        if (!(item.Label.ContainsKey(lang)))
                        {
                            item.Label.Add(lang, "");
                        }
                        if (!(item.Values.ContainsKey(lang)))
                        {
                            item.Values.Add(lang, "");
                        }
                        if (!(item.Pattern.ContainsKey(lang)))
                        {
                            item.Pattern.Add(lang, "");
                        }

                    }
                }

                model.ClientName = JsonConvert.SerializeObject(ClientName);
                model.logo_alt = JsonConvert.SerializeObject(logo_alt);
                model.Languages = JsonConvert.SerializeObject(Languages);
                model.fields_custom = JsonConvert.SerializeObject(CustomFieldList);
                model.text_Welcome = JsonConvert.SerializeObject(TextWelcomeList);
                model.text_Thankyou = JsonConvert.SerializeObject(TextThankYouList);
                model.text_Email = JsonConvert.SerializeObject(TextEmailList);
                if (FieldList.Count > 0)
                {
                    model.fields = JsonConvert.SerializeObject(FieldList);
                }
                else
                {
                    model.fields = string.Empty;
                }
                model.databasename = Utility.CommonHelper.Getgateway(model.flag); 
                var message = model.UpdateGateway();

                successmsg = "true";
                return RedirectToAction("edit", new { subdomain = message.ToString() });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult CreateGateway(Gateways model)
        {
            try
            {
                Dictionary<string, string> ClientName = new Dictionary<string, string>();
                Dictionary<string, string> logo_alt = new Dictionary<string, string>();
                List<string> Languages = new List<string>();
                List<CustomField> CustomFieldList = new List<CustomField>();
                Dictionary<string, string> LabelList = new Dictionary<string, string>();
                Dictionary<string, string> ValueList = new Dictionary<string, string>();
                Dictionary<string, string> PatternList = new Dictionary<string, string>();
                Dictionary<string, string> TextWelcomeList = new Dictionary<string, string>();
                Dictionary<string, string> TextThankYouList = new Dictionary<string, string>();
                Dictionary<string, string> TextEmailList = new Dictionary<string, string>();
                Dictionary<string, string> FieldList = new Dictionary<string, string>();
                List<string> fields = new List<string>();

                //GatewaysLogo objGatewaysLogo = new GatewaysLogo();
                //objGatewaysLogo.databasename = HttpContext.Session.GetString("database_gateway");
                //var files = HttpContext.Request.Form.Files;


                //foreach (var Image in files)
                //{
                //    if (Image != null && Image.Length > 0)
                //    {
                //        using (var fileStream = new MemoryStream())
                //        {
                //            Image.CopyTo(fileStream);
                //            objGatewaysLogo.contentType = Image.ContentType;
                //            objGatewaysLogo.subdomain = model.subdomain;
                //            objGatewaysLogo.logo = fileStream.ToArray();
                //        }

                //        objGatewaysLogo.databasename = HttpContext.Session.GetString("database_gateway");
                //        objGatewaysLogo.UpdateGateway();
                //    }
                //}
                fields.Add("FirstName");
                fields.Add("LastName");
                fields.Add("Phone");
                fields.Add("Email");
                fields.Add("Extension");
                fields.Add("AdditionalContact");
                fields.Add("BillingContact");
                fields.Add("Quote");
                fields.Add("Notes");

                int Length = 0;
                foreach (string key in Request.Form.Keys)
                {
                    if (key == "Languages[]")
                    {
                        Languages = Request.Form[key].ToString().Split(',').ToList();
                    }
                    else if (key == "fields_custom")
                    {
                        Length = Utility.CommonHelper.GetDBInt(Request.Form[key].ToString());
                    }
                }



                foreach (string item in fields)
                {
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "fields[" + item + "]")
                        {
                            FieldList.Add(item, Request.Form[key]);
                        }
                    }
                }

                for (int i = 0; i < Length; i++)
                {
                    LabelList = new Dictionary<string, string>();
                    ValueList = new Dictionary<string, string>();
                    PatternList = new Dictionary<string, string>();
                    CustomField CustomField = new CustomField();
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "fields_custom[" + i + "][Type]")
                        {
                            CustomField.Type = Request.Form[key].ToString();
                        }
                        else if (key == "fields_custom[" + i + "][State]")
                        {
                            CustomField.State = Request.Form[key].ToString();
                        }
                    }
                    foreach (string lang in Languages)
                    {
                        foreach (string key in Request.Form.Keys)
                        {
                            if (key == "fields_custom[" + i + "][Label][" + lang + "]")
                            {
                                LabelList.Add(lang, Request.Form[key]);
                            }
                            else if (key == "fields_custom[" + i + "][Values][" + lang + "]")
                            {
                                ValueList.Add(lang, Request.Form[key]);
                            }
                            else if (key == "fields_custom[" + i + "][Pattern][" + lang + "]")
                            {
                                PatternList.Add(lang, Request.Form[key]);
                            }
                        }
                    }
                    CustomField.Label = LabelList;
                    CustomField.Values = ValueList;
                    CustomField.Pattern = PatternList;
                    CustomFieldList.Add(CustomField);
                }

                foreach (string key in Request.Form.Keys)
                {
                    if (key == "fieldscustom")
                    {
                        Length = Utility.CommonHelper.GetDBInt(Request.Form[key].ToString());

                        CustomField CustomField = new CustomField();
                        CustomField.Label = LabelList;
                        CustomField.Values = ValueList;
                        CustomField.Pattern = PatternList;
                        CustomFieldList.Add(CustomField);
                    }
                }

                foreach (string lang in Languages)
                {
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "ClientNames[" + lang + "]")
                        {
                            ClientName.Add(lang, Request.Form[key]);
                        }
                        else if (key == "logo_alts[" + lang + "]")
                        {
                            logo_alt.Add(lang, Request.Form[key]);
                        }
                        else if (key == "text_Welcome[" + lang + "]")
                        {
                            TextWelcomeList.Add(lang, Request.Form[key]);
                        }
                        else if (key == "text_Thankyou[" + lang + "]")
                        {
                            TextThankYouList.Add(lang, Request.Form[key]);
                        }
                        else if (key == "text_Email[" + lang + "]")
                        {
                            TextEmailList.Add(lang, Request.Form[key]);
                        }
                    }
                }
                foreach (string lang in Languages)
                {
                    if (!(ClientName.ContainsKey(lang)))
                    {
                        ClientName.Add(lang, "");
                    }
                    if (!(logo_alt.ContainsKey(lang)))
                    {
                        logo_alt.Add(lang, "");
                    }
                    if (!(TextWelcomeList.ContainsKey(lang)))
                    {
                        TextWelcomeList.Add(lang, "");
                    }
                    if (!(TextThankYouList.ContainsKey(lang)))
                    {
                        TextThankYouList.Add(lang, "");
                    }
                    if (!(TextEmailList.ContainsKey(lang)))
                    {
                        TextEmailList.Add(lang, "");
                    }
                }

                foreach (CustomField item in CustomFieldList)
                {
                    foreach (string lang in Languages)
                    {
                        if (!(item.Label.ContainsKey(lang)))
                        {
                            item.Label.Add(lang, "");
                        }
                        if (!(item.Values.ContainsKey(lang)))
                        {
                            item.Values.Add(lang, "");
                        }
                        if (!(item.Pattern.ContainsKey(lang)))
                        {
                            item.Pattern.Add(lang, "");
                        }

                    }
                }

                model.ClientName = JsonConvert.SerializeObject(ClientName);
                model.logo_alt = JsonConvert.SerializeObject(logo_alt);
                model.Languages = JsonConvert.SerializeObject(Languages);
                model.fields_custom = JsonConvert.SerializeObject(CustomFieldList);
                model.text_Welcome = JsonConvert.SerializeObject(TextWelcomeList);
                model.text_Thankyou = JsonConvert.SerializeObject(TextThankYouList);
                model.text_Email = JsonConvert.SerializeObject(TextEmailList);
                if (FieldList.Count > 0)
                {
                    model.fields = JsonConvert.SerializeObject(FieldList);
                }
                else
                {
                    model.fields = string.Empty;
                }
                model.databasename = HttpContext.Session.GetString("database_gateway");
                var message = model.Insert();

                successmsg = "true";
                var databasename = HttpContext.Session.GetString("Database");
                string flag = databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ? "1" : "0";
                return RedirectToAction("edit", new { subdomain = message.ToString(),flag = flag });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SetContactDatabase()
        {
            var databasename = Request.Form["databasename"];

            HttpContext.Session.SetString("database_gateway", Utility.CommonHelper.GetDBString(databasename));
            var jsonResult = Json(new

            {
                result = true
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GatewayList(JQueryDataTableParamModel param, string AlphaSearch)
        {
            var databasename = HttpContext.Session.GetString("Database");
           // var databasename1 = HttpContext.Session.GetString("Database_edit");
            //if (databasename != databasename1)
            //{
            //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
            //}

            //var databasename2 = HttpContext.Session.GetString("database_gateway");
            //var databasename3 = HttpContext.Session.GetString("database_gateway_edit");
            //if (databasename2 != databasename3)
            //{
            //    HttpContext.Session.SetString("database_gateway", Utility.CommonHelper.GetDBString(databasename3));
            //}

            int totalRecords = 0;
            //List<Gateways> allRecord = null;
            List<Gateways> allRecord = new List<Gateways>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Gateways objClientsMaster = new Gateways();
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                SearchKey = param.sSearch;
            }
            string[] colunms = param.sColumns.Split(',');
            string sortcolunm = Request.Form["iSortCol_0"];
            string sortColunmName = colunms[Convert.ToInt32(sortcolunm)].ToString();
            string sortdir = Request.Form["sSortDir_0"];
            if (!string.IsNullOrEmpty(sortColunmName) && !string.IsNullOrEmpty(sortdir))
            {
                OrderBy = sortColunmName + " " + sortdir;
            }

            allRecord = objClientsMaster.GetGatewayList(startIndex, endIndex, OrderBy, SearchKey, AlphaSearch, out totalRecords);
            // allRecords = objClientMaster.GetCompanyList();

            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    if (allRecord != null)
            //    {
            //        filteredRecords = allRecord.Count();
            //    }
            //    else
            //    {
            //        filteredRecords = 0;
            //    }

            //}
            //if (allRecord != null)
            //{
            foreach (Gateways item in allRecord)
            {
                Clients objClientMaster = new Clients();
                objClientMaster.databasename = databasename;
                objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(item.ClientID));
                if (objClientMaster != null) {
                    item.Code = objClientMaster.Code;
                    item.ClientName = objClientMaster.Company;
                }
                
            }


            var result = from c in allRecord
                         select new[] {

                         c.ClientID == null ? "" : c.ClientID.ToString(),
                             c.Code == null ? "" : c.Code.ToString(),
                             c.subdomain == null ? "" : c.subdomain.ToString(),
                             c.ClientName == null ? "" : c.ClientName.ToString(),
                             c.PortalName == null ? "" : c.PortalName.ToString(),
                             c.databasename.ToString(),
                             c.databasename.Contains("_eu")?Models.Utility.Constants.ABLEDOCS_EU_DB:Models.Utility.Constants.ABLEDOCS_DB,
                             c.databasename.Contains("_eu")?"1":"0",
                         };
            //}
            var jsonResult = Json(new

            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
                //bbData = results
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult DeleteGateways()
        {
            var subdomain = Request.Form["subdomain"];
            var ClientID = Request.Form["ClientID"];
            var flag= Request.Form["flag"];


            Gateways model = new Gateways();
            model.subdomain = subdomain;
            model.ClientID = ClientID;
            model.databasename = Utility.CommonHelper.Getgateway(flag);
            model.DeleteGateway();

            var jsonResult = Json(new

            {
                result = true
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult isExistsubdomain()
        {
            var subdomain = Request.Form["subdomain"];
            var flag = Request.Form["flag"];

            Gateways gateways = new Gateways();
            gateways.databasename = Utility.CommonHelper.Getgateway(flag);
            gateways = gateways.GetewayById(subdomain);
            bool isExist = false;
            if (gateways != null)
            {
                isExist = true;
            }

            var jsonResult = Json(new
            {
                result = isExist
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult Upload(string subdomain,string flag, string FileTypeReference)
        {
            var databasename = Utility.CommonHelper.Getgateway(flag);
            GatewaysLogo objGatewaysLogo = new GatewaysLogo();
            objGatewaysLogo.databasename = databasename;
            var files = HttpContext.Request.Form.Files;


            foreach (var Image in files)
            {
                if (Image != null && Image.Length > 0)
                {
                    using (var fileStream = new MemoryStream())
                    {
                        Image.CopyTo(fileStream);
                        objGatewaysLogo.contentType = Image.ContentType;
                        objGatewaysLogo.subdomain = subdomain;
                        objGatewaysLogo.logo = fileStream.ToArray();
                    }

                    objGatewaysLogo.databasename = databasename;
                    objGatewaysLogo.UpdateGateway();
                }
            }

            objGatewaysLogo = new GatewaysLogo();
            objGatewaysLogo.databasename = databasename;
            objGatewaysLogo = objGatewaysLogo.GetewayById(subdomain);
            //  Languages obj = JsonConvert.DeserializeObject<Languages>(objGatewayMaster.Languages);

            //Console.WriteLine(obj.Country);

            if (objGatewaysLogo != null)
            {
                byte[] bytes = (byte[])objGatewaysLogo.logo;
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                ViewBag.ImageUrl = "data:image/png;base64," + base64String;
            }
            else
            {
                ViewBag.ImageUrl = "";
            }

            return Json(new { data = ViewBag.ImageUrl });

        }


    }
}
