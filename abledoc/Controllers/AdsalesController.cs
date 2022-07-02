using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class AdsalesController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {

            ClientsContacts objClientAlpha = new ClientsContacts();
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "adsales").FirstOrDefault();
            string pagefound = "No";
            string isAdd = "No";
            string isEdit = "No";
            string isDelete = "No";

            if (objAssignedMenu != null)
            {
                if (objAssignedMenu.isview)
                {
                    pagefound = "yes";

                }
                if (objAssignedMenu.isadd)
                {
                    isAdd = "yes";

                }
                if (objAssignedMenu.isupdate)
                {
                    isEdit = "yes";

                }
                if (objAssignedMenu.isdelete)
                {
                    isDelete = "yes";

                }
            }
            ViewBag.isAdd = isAdd;
            ViewBag.isEdit = isEdit;
            ViewBag.isDelete = isDelete;



            if (pagefound == "No")
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                ADSales adSales = new ADSales();

                CommonMaster commonMaster = new CommonMaster();
                List<CommonMaster> commonMasterList = commonMaster.GetCommonMasterList("Pipeline");
                string typecode = commonMasterList.Select(x => x.typecode).FirstOrDefault();

                commonMasterList = new List<CommonMaster>();
                adSales.Pipelinelist = commonMaster.GetCommonMasterList(typecode);

                adSales.adsaleslist = adSales.GetADSalesList();

                string code = HttpContext.Session.GetString("Country");
                Countries countries = new Countries();
                countries.code = code;
                countries = countries.GetCountryByCode();

                if (countries.currency_code.Trim() == "USD")
                {
                    ViewBag.Code = "$";
                }
                else {
                    ViewBag.Code = countries.currency_code;
                }
               

                //ConversionRate conversionRate = new ConversionRate();
                //conversionRate = ConvertAmountAPI(countries.currency_code.Trim());
                if(adSales.adsaleslist != null)
                {
                    foreach (ADSales item in adSales.adsaleslist)
                    {
                        item.flag = item.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ? "1" : "0";
                        if (item.currencycode != countries.currency_code)
                        {
                            //ConversionRateAPI1(countries.currency_code.Trim(), item.currencycode.Trim(), item.value);
                            Rates rates = new Rates();
                            item.currencycode = item.currencycode.Trim();
                            //item.value = Math.Round(decimal.Parse(rates.ConvertGOOG(item.value, item.currencycode.Trim(), conversionRate).ToString()), 2);
                            item.value = Math.Round(ConversionRateAPI1(countries.currency_code.Trim(), item.currencycode.Trim(), item.value), 2);

                        }
                        // ConvertGOOG(float.Parse(item.value.ToString()), item.currencycode, countries.currency_code);
                    }
                }
                

                return View(adSales);
            }





            //ViewBag.AlphabetList = objClientAlpha.GetAlphabetList();
            //return View();
        }
        [LoginAuthorized]
        public  decimal ConversionRateAPI1(string fromCurrency,string to_Currency,decimal amount)
        {
            try
            {
                try
                {
                    string query = fromCurrency + "_" + to_Currency;
                    String URLString = "https://free.currconv.com/api/v7/convert?q="+ query + "&compact=ultra&apiKey=b8c493c2b903637553b5";
                    //using (var webClient = new System.Net.WebClient())
                    //{
                    //    var json = webClient.DownloadString(URLString);
                    //    API_Obj Test = JsonConvert.DeserializeObject<API_Obj>(json);

                    //    return Test.conversion_rates;

                    //}

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var client = new RestClient(URLString);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AlwaysMultipartFormData = true;
                    IRestResponse response = client.Execute(request);
                    string jsonContent = string.Empty;

                    if (response.StatusCode.ToString() == "OK")
                    {
                        jsonContent = response.Content;
                        if (!string.IsNullOrWhiteSpace(jsonContent))
                        {
                            dynamic Test = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                            if (Test != null)
                            {
                                string amt = Test[query];
                                return decimal.Parse(amt) * amount;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
           
        }
        [LoginAuthorized]
        public ConversionRate ConvertAmountAPI(string fromCurrency)
        {
   
            try
            {

                String URLString = "https://v6.exchangerate-api.com/v6/1be343c87e8418218a8fe259/latest/" + fromCurrency;
                //using (var webClient = new System.Net.WebClient())
                //{
                //    var json = webClient.DownloadString(URLString);
                //    API_Obj Test = JsonConvert.DeserializeObject<API_Obj>(json);

                //    return Test.conversion_rates;

                //}

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var client = new RestClient(URLString);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);
                string jsonContent = string.Empty;

                if (response.StatusCode.ToString() == "OK")
                {
                    jsonContent = response.Content;
                    if (!string.IsNullOrWhiteSpace(jsonContent))
                    {
                        API_Obj Test = JsonConvert.DeserializeObject<API_Obj>(jsonContent);
                        if (Test != null)
                        {
                            return Test.conversion_rates;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        

        [HttpGet, LoginAuthorized]
        public ActionResult Create()
        {
            var databasename = HttpContext.Session.GetString("Database");
            ViewBag.databasename = databasename;
            ViewBag.loginusercountry = HttpContext.Session.GetString("Country");

            ADSales model = new ADSales();
            CommonMaster commonMaster = new CommonMaster();
            List<CommonMaster> commonMasterList = commonMaster.GetCommonMasterList("Pipeline");
            ViewBag.Type = commonMasterList.Select(x => x.typecode).FirstOrDefault();

            commonMasterList = new List<CommonMaster>();
            commonMasterList = commonMaster.GetCommonMasterList(ViewBag.Type);
            model.stage = commonMasterList.Select(x => x.typecode).FirstOrDefault();
            model.ownerid = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            model.flag = databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ? "1" : "0";
            return View(model);
        }

        [HttpGet, LoginAuthorized]
        public ActionResult Edit(int id,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;
            ViewBag.loginusercountry = HttpContext.Session.GetString("Country");

            ADSales model = new ADSales();
            model.adsalesid = id;
            model.databasename = databasename;
            model = model.GetADSalesbyId();
            model.flag = flag;
            if (model == null)
            {
                CommonMaster commonMaster = new CommonMaster();
                List<CommonMaster> commonMasterList = commonMaster.GetCommonMasterList("Pipeline");
                ViewBag.Type = commonMasterList.Select(x => x.typecode).FirstOrDefault();

                commonMasterList = new List<CommonMaster>();
                commonMasterList = commonMaster.GetCommonMasterList(ViewBag.Type);
                model.stage = commonMasterList.Select(x => x.typecode).FirstOrDefault();
            }
            else
            {
                ViewBag.Type = model.pipeline;
                model.stage = model.stage;
                ViewBag.id = model.clientid.ToString();
                ADSalesPhone aDSalesPhone = new ADSalesPhone();
                model.adsalesphonelist = new List<ADSalesPhone>();
                model.adsalesphonelist = aDSalesPhone.GetList(model.adsalesid);
                ADSalesEmail aDSalesEmail = new ADSalesEmail();
                model.adsalesemaillist = new List<ADSalesEmail>();
                aDSalesEmail.databasename = databasename;
                model.adsalesemaillist = aDSalesEmail.GetList(model.adsalesid);
            }
            
            return View("Create", model);
        }
        [LoginAuthorized]
        public ActionResult adsales1()
        {
            return View("~/Views/ADsales/IndexNew.cshtml");
        }
        [LoginAuthorized]
        public PartialViewResult contactdropdown(int id,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag) ;
            ViewBag.databasename = databasename;
            ViewBag.loginusercountry = HttpContext.Session.GetString("Country");
            if (id == null)
            {
                ViewBag.id = "";
            }
            else
            {
                ViewBag.id = id.ToString();
            }

            return PartialView("_ContactDropdown");
        }
        [LoginAuthorized]
        public PartialViewResult currencydropdown(int id)
        {
            var databasename = HttpContext.Session.GetString("Database");

            if (id == null)
            {
                ViewBag.code = "";
            }
            else
            {
                Clients clients = new Clients();
                clients.databasename = databasename;

                clients = clients.GetClientById(id);
                if (clients.Country != null)
                {
                    ViewBag.code = clients.Country.ToString();
                }
                else
                {
                    ViewBag.code = "";
                }

            }

            return PartialView("_CurrencyDropdown");
        }
        [LoginAuthorized]
        public PartialViewResult pipelinestage(string typecode)
        {
            ViewBag.Type = typecode;
            return PartialView("_Pipelinestage");
        }

        [HttpPost,LoginAuthorized]
        //[ValidateAntiForgeryToken]
        public ActionResult upsert(ADSales model)
        {
            try
            {
                var databasename = Utility.CommonHelper.Getabledocs(model.flag);
                model.userid = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                model.databasename = databasename;
                var retval = model.Upsert();



                ADSalesPhone modeJobsDeliveryContact = new ADSalesPhone();
                ADSalesEmail modeJobsDeliveryContact1 = new ADSalesEmail();
                if (model.adsalesid != 0)
                {
                    modeJobsDeliveryContact.adsalesid = Utility.CommonHelper.GetDBInt(retval);
                    modeJobsDeliveryContact.databasename = databasename;
                    modeJobsDeliveryContact.Delete();
                    modeJobsDeliveryContact1.adsalesid = Utility.CommonHelper.GetDBInt(retval);
                    modeJobsDeliveryContact1.databasename = databasename;
                    modeJobsDeliveryContact1.Delete();
                }
                

                List<string> ContactIDs = new List<string>();
                bool isempty = false;
                foreach (string key in Request.Form.Keys)
                {
                    if (key.Contains("TypeFilter"))
                    {
                        //if (Request.Form[key].ToString() != "")
                        //{
                        modeJobsDeliveryContact.adsalesid = Utility.CommonHelper.GetDBInt(retval);
                        if (key.Contains("Phone1"))
                        {
                            
                            if (string.IsNullOrEmpty(Request.Form[key].ToString()))
                            {
                                isempty = true;
                            }
                            else
                            {
                                ContactIDs.Add("'" + Request.Form[key].ToString() + "'");
                                modeJobsDeliveryContact.phone = Request.Form[key].ToString();

                            }
                            continue;
                        }
                        if (key.Contains("PhoneType"))
                        {
                            modeJobsDeliveryContact.type = Request.Form[key].ToString();
                        }
                        if (isempty)
                        {
                            isempty = false;
                        }
                        else
                        {
                            modeJobsDeliveryContact.databasename = databasename;
                            modeJobsDeliveryContact.Insert();
                        }

                        //}
                    }

                    if (key.Contains("TypeEmail"))
                    {
                        //if (Request.Form[key].ToString() != "")
                        //{
                        modeJobsDeliveryContact1.adsalesid = Utility.CommonHelper.GetDBInt(retval);
                        if (key.Contains("Email1"))
                        {

                            if (string.IsNullOrEmpty(Request.Form[key].ToString()))
                            {
                                isempty = true;
                            }
                            else
                            {
                                ContactIDs.Add("'" + Request.Form[key].ToString() + "'");
                                modeJobsDeliveryContact1.email = Request.Form[key].ToString();
                            }
                            continue;
                        }
                        if (key.Contains("EmailType"))
                        {
                            modeJobsDeliveryContact1.type = Request.Form[key].ToString();
                        }

                        if (isempty)
                        {
                            isempty = false;
                        }
                        else
                        {
                            modeJobsDeliveryContact1.databasename = databasename;
                            modeJobsDeliveryContact1.Insert();
                        }
                        //}
                    }


                }




                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}
