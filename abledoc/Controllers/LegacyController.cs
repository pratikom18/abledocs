using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class LegacyController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public LegacyController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        //[AuthorizedAction]
        [LoginAuthorized]
        [HttpGet]
        public ActionResult Edit(int id,string flag, int save = 0)
        {
            var legacy_databasename = Utility.CommonHelper.Getlegacy(flag);
            ADLegacyCustom objLegacyCustom = new ADLegacyCustom();
            objLegacyCustom.databasename = legacy_databasename;
            objLegacyCustom = objLegacyCustom.GetADlegacyCustomById(Convert.ToInt32(id));

            Clients objClientMaster = new Clients();
            objClientMaster.databasename = Utility.CommonHelper.Getabledocs(flag); 
            objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(id));

            ADLegacyLogo objLegacyLogo = new ADLegacyLogo();
            objLegacyLogo.databasename = legacy_databasename;
            objLegacyLogo = objLegacyLogo.GetLegacyLogoById(id);
           
            if (objLegacyLogo != null)
            {
                byte[] bytes = (byte[])objLegacyLogo.logo;
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                ViewBag.ImageUrl = "data:image/png;base64," + base64String;
            }
            else
            {
                ViewBag.ImageUrl = "";
            }
            var objLanguageList = JsonConvert.DeserializeObject(objLegacyCustom.Languages);
            var objClientNamelist = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(objLegacyCustom.logo_alt))
            {
                 objClientNamelist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objLegacyCustom.ClientName);
            }
            else
            {
                 objClientNamelist = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"EN\":\"\"}");
            }

            var objLogoAltlist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objLegacyCustom.logo_alt))
            {
                 objLogoAltlist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objLegacyCustom.logo_alt);
            }
            else
            {
                objLogoAltlist = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"EN\":\"\"}");

            }

            var objStampTextlist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objLegacyCustom.StampText))
            {
                objStampTextlist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objLegacyCustom.StampText);
            }
            else
            {
                objStampTextlist = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"EN\":\"\"}");
                
            }

            
            

            var objTextWelcomelist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objLegacyCustom.text_Welcome))
            {
                objTextWelcomelist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objLegacyCustom.text_Welcome);
            }
            else
            {
                objTextWelcomelist = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"EN\":\"\"}");

            }

            var objTextThankyoulist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objLegacyCustom.text_Thankyou))
            {
                objTextThankyoulist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objLegacyCustom.text_Thankyou);
            }
            else
            {
                objTextThankyoulist = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"EN\":\"\"}");

            }

            var objTextEmaillist = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(objLegacyCustom.text_Email))
            {
                objTextEmaillist = JsonConvert.DeserializeObject<Dictionary<string, string>>(objLegacyCustom.text_Email);
            }
            else
            {
                objTextEmaillist = JsonConvert.DeserializeObject<Dictionary<string, string>>("{\"EN\":\"\"}");

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
            ViewBag.StampTextlist = objStampTextlist;

            ViewBag.TextWelcomelist = objTextWelcomelist;
            ViewBag.TextThankyoulist = objTextThankyoulist;
            ViewBag.TextEmaillist = objTextEmaillist;

            ViewBag.ClientDetails = objClientMaster;
            ViewBag.Save = save;

            objLegacyCustom.flag = flag;

            return View(objLegacyCustom);
        }

        [HttpPost, LoginAuthorized]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ADLegacyCustom model)
        {
            try
            {
                Dictionary<string, string> ClientName = new Dictionary<string, string>();
                Dictionary<string, string> logo_alt = new Dictionary<string, string>();
                Dictionary<string, string> StampText = new Dictionary<string, string>();
                List<string> Languages = new List<string>();
               
               
                Dictionary<string, string> TextWelcomeList = new Dictionary<string, string>();
                Dictionary<string, string> TextThankYouList = new Dictionary<string, string>();
                Dictionary<string, string> TextEmailList = new Dictionary<string, string>();
               
               

                ADLegacyLogo objLegacyLogo = new ADLegacyLogo();
                var legacy_databasename = Utility.CommonHelper.Getlegacy(model.flag);
                var files = HttpContext.Request.Form.Files;


                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        if (Image.ContentType == "application/pdf")
                        {
                            Utility.UploadFile uploadFile = new Utility.UploadFile(this.webHostEnvironment);
                            uploadFile.databasename = legacy_databasename;
                            uploadFile.updateLegacyIcon(Convert.ToInt32(model.ClientID), Image);
                        }
                        else {
                            using (var fileStream = new MemoryStream())
                            {
                                Image.CopyTo(fileStream);
                                objLegacyLogo.contentType = Image.ContentType;
                                objLegacyLogo.ClientID = Convert.ToInt32(model.ClientID);
                                objLegacyLogo.logo = fileStream.ToArray();
                            }
                            objLegacyLogo.databasename = legacy_databasename;
                            objLegacyLogo.UpdateLegacyLogo();
                        }
                        
                    }
                }


        //        if ($_FILES["IconToUpload"]["size"] > 0) {
        //$path = WebRoot.'/includes/SetaPdf/Stamps/'.$_POST['ClientID'].'.pdf';
        //            file_put_contents($path, file_get_contents($_FILES['IconToUpload']['tmp_name']));
        //            query_legacy("UPDATE adlegacy_custom SET icon = :icon WHERE  ClientID = :ClientID", array(
        //                ':ClientID' => $_POST['ClientID'],
        //                ':icon' => $path,
            
                //    ));
                //}



                foreach (string key in Request.Form.Keys)
                {
                    if (key == "Languages[]")
                    {
                        Languages = Request.Form[key].ToString().Split(',').ToList();
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
                        else if (key == "StampText[" + lang + "]")
                        {
                            StampText.Add(lang, Request.Form[key]);
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
                    if (!(StampText.ContainsKey(lang)))
                    {
                        StampText.Add(lang, "");
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

                model.ClientName = (ClientName.Count > 0) ?JsonConvert.SerializeObject(ClientName) : null;
                model.logo_alt = (logo_alt.Count > 0)?JsonConvert.SerializeObject(logo_alt) : null;
                model.Languages = (Languages.Count > 0) ? JsonConvert.SerializeObject(Languages):"[\"EN\"]";
                //model.fields_custom = JsonConvert.SerializeObject(CustomFieldList);
                model.text_Welcome = (TextWelcomeList.Count > 0) ? JsonConvert.SerializeObject(TextWelcomeList) : null;
                model.text_Thankyou = (TextThankYouList.Count > 0) ? JsonConvert.SerializeObject(TextThankYouList) : null;
                model.text_Email = (TextEmailList.Count > 0) ? JsonConvert.SerializeObject(TextEmailList) : null;
                model.StampText = (StampText.Count > 0) ? JsonConvert.SerializeObject(StampText) : null;
                model.databasename = legacy_databasename;
                var message = model.UpdateLegacyCustom();
                return RedirectToAction("edit", new { id = Convert.ToInt32(model.ClientID),flag = model.flag, save = 1 });
                // return RedirectToAction("Index");
            }
            catch
            {
                
            }
            return View();
        }
    }
}
