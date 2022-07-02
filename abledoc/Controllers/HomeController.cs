using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IViewLocalizer Localizer;
        public HomeController(ILogger<HomeController> logger, IViewLocalizer _Localizer)
        {
            _logger = logger;
            Localizer = _Localizer;
        }

        //[AuthorizedAction]
        [LoginAuthorized]
        public IActionResult Index()
        {

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("ID")))
            {
                Jobs objJobMaster = new Jobs();
                InvoiceInstance objinvoiceInstance = new InvoiceInstance();
                
                //var databasename = HttpContext.Session.GetString("Database");
                //objJobMaster.databasename = databasename;
                
                List<Jobs> StatusList = objJobMaster.GetStatusList();

                ViewBag.TotalPENDING = Utility.CommonHelper.GetDBInt(StatusList.Where(x => x.Status == "PENDING").Select(x => x.Count).Sum());
                ViewBag.TotalQUOTE = Utility.CommonHelper.GetDBInt(StatusList.Where(x => x.Status == "QUOTE").Select(x => x.Count).Sum());
                ViewBag.TotalOPEN = Utility.CommonHelper.GetDBInt(StatusList.Where(x => x.Status == "OPEN").Select(x => x.Count).Sum());
                ViewBag.TotalTOBEDELIVERED = Utility.CommonHelper.GetDBInt(StatusList.Where(x => x.Status == "TOBEDELIVERED").Select(x => x.Count).Sum());
                ViewBag.TotalDELIVERED = Utility.CommonHelper.GetDBInt(StatusList.Where(x => x.Status == "DELIVERED").Select(x => x.Count).Sum());

                ViewBag.TotalJobs = Utility.CommonHelper.GetDBInt(StatusList.Select(x => x.Count).Sum());

                
                string[] statusArray = { "Phase 1", "Phase 2", "Phase 3", "ALT Text", "Deliverables" };

                Production modelProduction = new Production();
                int TotalProduction = 0;
                foreach (var status in statusArray)
                {
                    TotalProduction += Utility.CommonHelper.GetDBInt(modelProduction.GetStatusList(status));
                }
                ViewBag.TotalProduction = TotalProduction;

                Dictionary<string, int> phasesList = new Dictionary<string, int>();
                JobsFiles objJobsFiles = new JobsFiles();
                var databasename = HttpContext.Session.GetString("Database");
                objJobsFiles.databasename = databasename;
                phasesList = objJobsFiles.PhasesCount(Utility.CommonHelper.GetDBInt(HttpContext.Session.GetString("ID")));
                ViewBag.PhasesList = phasesList;

                UserPermissions userPermissions = new UserPermissions();

                ViewBag.SettingList = userPermissions.GetSettingList(Utility.CommonHelper.GetDBInt(HttpContext.Session.GetString("ID")), "Dashboard");
                if (ViewBag.SettingList != null)
                {
                    foreach (Setting item in ViewBag.SettingList)
                    {
                        if (item.SectionName == "Global Sales")
                        {
                            List<InvoiceInstance> invoiceInstancesList = objinvoiceInstance.GetGlobalSales();
                            ViewBag.CountryList = invoiceInstancesList.OrderByDescending(x => x.InvoiceAmount);
                            ViewBag.Totalamount = invoiceInstancesList.Select(x => x.InvoiceAmount).Sum();
                        }
                        else if (item.SectionName == "Visualization")
                        {
                            ViewBag.InvoiceAmount = objinvoiceInstance.GetGlobalInvoice();
                            int i = 0;
                            long years1 = DateTime.Today.Year - 1; ;
                            long years2 = DateTime.Today.Year;
                            decimal amount1 = 0;
                            decimal amount2 = 0;
                            foreach (InvoiceInstance item1 in ViewBag.InvoiceAmount)
                            {
                                if (i == 0)
                                {
                                    years1 = item1.Years;
                                    amount1 = item1.InvoiceAmount;
                                }
                                else
                                {
                                    years2 = item1.Years;
                                    amount2 = item1.InvoiceAmount;
                                }
                                i++;


                            }
                            ViewBag.years1 = years1;
                            ViewBag.amount1 = amount1;
                            ViewBag.years2 = years2;
                            ViewBag.amount2 = amount2;

                            ViewBag.PrvDailyMaxJob = objinvoiceInstance.GetPreviousDailyMaxJobs();
                            ViewBag.DailyMaxJob = objinvoiceInstance.GetDailyMaxJobs();
                            ViewBag.WeeklyMaxJob = objinvoiceInstance.GetWeeklyMaxJobs();
                            ViewBag.DailyMaxFiles = objinvoiceInstance.GetDailyMaxFile();
                            ViewBag.QUARTER = objinvoiceInstance.GetGlobalInvoiceQUARTER();
                            ViewBag.QUARTER1 = objinvoiceInstance.GetGlobalInvoiceQUARTERPre();
                            ViewBag.Billing = objinvoiceInstance.GetGlobalInvoiceBilling();
                            //ViewBag.WeeklySummary = objinvoiceInstance.GetGlobalWeeklySummary();

                            ViewBag.JobWaterMarks = objinvoiceInstance.GetJobWaterMarks();

                            decimal q1 = 0;
                            decimal q2 = 0;
                            decimal q3 = 0;
                            decimal q4 = 0;
                            foreach (InvoiceInstance item1 in ViewBag.QUARTER)
                            {
                                if (item1.Years == 1)
                                {
                                    q1 = item1.InvoiceAmount;
                                }
                                if (item1.Years == 2)
                                {
                                    q2 = item1.InvoiceAmount;
                                }
                                if (item1.Years == 3)
                                {
                                    q3 = item1.InvoiceAmount;
                                }
                                if (item1.Years == 4)
                                {
                                    q4 = item1.InvoiceAmount;
                                }
                            }
                            ViewBag.q1 = q1;
                            ViewBag.q2 = q2;
                            ViewBag.q3 = q3;
                            ViewBag.q4 = q4;

                            decimal q5 = 0;
                            decimal q6 = 0;
                            decimal q7 = 0;
                            decimal q8 = 0;
                            foreach (InvoiceInstance item1 in ViewBag.QUARTER1)
                            {
                                if (item1.Years == 1)
                                {
                                    q5 = item1.InvoiceAmount;
                                }
                                if (item1.Years == 2)
                                {
                                    q6 = item1.InvoiceAmount;
                                }
                                if (item1.Years == 3)
                                {
                                    q7 = item1.InvoiceAmount;
                                }
                                if (item1.Years == 4)
                                {
                                    q8 = item1.InvoiceAmount;
                                }
                            }
                            ViewBag.q5 = q5;
                            ViewBag.q6 = q6;
                            ViewBag.q7 = q7;
                            ViewBag.q8 = q8;
                            // ViewBag.total = q1 + q2 + q3 + q4;
                        }
                        else if (item.SectionName == "Billing Office")
                        {
                            ViewBag.TotalOffice = objinvoiceInstance.GetGlobalTotalOffice();
                        }
                        else if (item.SectionName == "Clients")
                        {
                            List<Jobs> ClientList = objJobMaster.GetClientsList();
                            ViewBag.TotalClients = ClientList.Count();
                        }
                        else if (item.SectionName == "Contacts")
                        {
                            ClientsContacts objClientAlpha = new ClientsContacts();
                            List<ClientsContacts> objClients = objClientAlpha.GetContactList();
                            ViewBag.TotalContacts = objClients.Count();
                        }
                       
                    }
                }
                

                    return View();
            }
            else
            {
                return View("~/Views/Login/Index.cshtml");
            }

        }
        [LoginAuthorized]
        public IActionResult Privacy()
        {
            return View();
        }
        [LoginAuthorized]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //public JsonResult GetPinmenuvalue(string pinvalue)
        //{

        //    HttpContext.Session.SetString("pinvalue", pinvalue);

        //    var jsonResult = Json(new
        //    {
        //        messageKey = "true",
        //        message = ""
        //    });
        //    return jsonResult;

        //}

        [HttpPost, LoginAuthorized]
        public IActionResult GetPinmenuvalue(string pinvalue)
        {
            HttpContext.Session.SetString("pinvalue", pinvalue);
            string test = HttpContext.Session.GetString("pinvalue");
            var jsonResult = Json(new
            {
                message = "Success"
            });
            return jsonResult;
        }
        [HttpPost, LoginAuthorized]
        public IActionResult SetMenuID(string firstid, string secondid, string thiredid)
        {
            HttpContext.Session.SetString("firstid", firstid);
            string test = HttpContext.Session.GetString("firstid");
            HttpContext.Session.SetString("secondid", secondid);
            string test1 = HttpContext.Session.GetString("secondid");
            HttpContext.Session.SetString("thiredid", thiredid);
            string test2 = HttpContext.Session.GetString("thiredid");
            var jsonResult = Json(new
            {
                message = "Success"
            });
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult SettingPermissionList()
        {
            int userid = Utility.CommonHelper.GetDBInt(HttpContext.Session.GetString("ID"));
            string roleid = HttpContext.Session.GetString("UserRoleId");
            ViewBag.UserID = userid;

            UserPermissions settingPermissions = new UserPermissions();
            settingPermissions.UserPermissionsList = settingPermissions.GetUserPermissionList(userid, roleid);

            int total = settingPermissions.UserPermissionsList.Count();
            int isView = 0;
            foreach (UserPermissions item in settingPermissions.UserPermissionsList)
            {
                if (item.IsView)
                {
                    isView = isView + 1;
                }
            }
            bool view = false;
            if (isView == total)
            {
                view = true;
            }
            ViewBag.isView = view;

            return PartialView("~/views/shared/_userPermissions.cshtml", settingPermissions);
        }


        [HttpPost, LoginAuthorized]
        public JsonResult UpsertSettingPermission(int userid, int settingid, bool view)
        {
            var databasename = HttpContext.Session.GetString("Database");

            UserPermissions settingPermissions = new UserPermissions();

            settingPermissions.UserID = userid;
            settingPermissions.SettingID = settingid;
            settingPermissions.IsView = view;
            var result = settingPermissions.Upsert();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ClientVisualizationByCountry()
        {

            Clients clients = new Clients();
            var result = clients.GetClientVisualizationbyCountry();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }
        [HttpPost, LoginAuthorized]
        public JsonResult JobsVisualizationByCountry()
        {

            Clients clients = new Clients();
            var result = clients.GetJobsVisualizationbyCountry();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ProductVisualizationByCountry()
        {

            string[] statusArray = { "Phase 1", "Phase 2", "Phase 3", "ALT Text", "Deliverables" };

            Production modelProduction = new Production();
            Dictionary<string, string> StatusCountList = new Dictionary<string, string>();
            foreach (var status in statusArray)
            {
                var TotalCount = modelProduction.GetStatusList(status);
                StatusCountList.Add(status, TotalCount);

            }

            var jsonResult = Json(new

            {
                data = StatusCountList
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult JobsVisualizationByStatus()
        {
            Jobs objJobMaster = new Jobs();
            List<Jobs> StatusList = objJobMaster.GetStatusList();

            var jsonResult = Json(new
            {
                data = StatusList
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }





        [HttpPost, LoginAuthorized]
        public JsonResult GetWeeklySummaryList(JQueryDataTableParamModel param)
        {

            int totalRecords = 0;
            List<InvoiceInstance> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            InvoiceInstance objInvoiceInstance = new InvoiceInstance();
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
            allRecord = objInvoiceInstance.GetGlobalWeeklySummary(startIndex, endIndex, OrderBy, SearchKey, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }


            foreach (InvoiceInstance item in allRecord)
            {
                item.foregincurrencyvalue = Math.Round(ConversionRateAPI1(item.currency.Trim(), "USD", Convert.ToDecimal(item.InvoiceAmount)), 2);
            }

            //foreach (InvoiceInstance item in allRecord)
            //{
            //    QuoteTracking quoteTracking = new QuoteTracking();
            //    decimal rowQuoteAmount = Utility.CommonHelper.GetDBDecimal(0.00);
            //    quoteTracking.QuoteAmount = Utility.CommonHelper.GetDBDecimal(0.00);
            //    if (string.IsNullOrEmpty(item.QuoteAmount.ToString()))
            //    {
            //        rowQuoteAmount = Utility.CommonHelper.GetDBDecimal(0.00);
            //    }
            //    else
            //    {
            //        quoteTracking.QuoteAmount = item.QuoteAmount;
            //    }

            //    if (item.Tax != null)
            //    {
            //        string taxString = item.Tax;
            //        string[] taxStringExplode = taxString.Split(" (");
            //        string[] taxString1 = taxStringExplode[1].Split("%)");
            //        decimal tax = Utility.CommonHelper.GetDBDecimal(taxString1[0]);

            //        decimal preTaxAmount = item.InvoiceAmount / (1 + (tax / 100));

            //        var justTax = item.InvoiceAmount - preTaxAmount;

            //        item.Tax = tax.ToString();
            //        item.PretaxAmount = preTaxAmount;

            //    }

            //}


            var result = from c in allRecord
                         select new[] {

                           c.InvoiceID.ToString(),                              //0
                           c.Created.ToString("MM/dd/yyyy"),                    //1
                           c.Notes== null?"":c.Notes.ToString(),                //2
                           c.Datepaid == null ?"":c.Datepaid.ToString(),         //3
                           c.EngagementNum,                                      //4
                           c.fullname==null?"":c.fullname,                       //5
                           c.Product==null?"":c.Product,                        //6
                           c.documents==null?"":c.documents.ToString(),         //7
                           c.totalPages == null?"":c.totalPages.ToString(),     //8
                            c.InvoiceAmount == null?"":c.InvoiceAmount.ToString(),//9
                            c.netwithhst == null?"":c.netwithhst.ToString(),    //10
                            c.hstamount==null?"":c.hstamount.ToString(),        //11
                            c.currency == null?"":c.currency.ToString(),        //12
                            c.foregincurrencyvalue == null?"":c.foregincurrencyvalue.ToString()//13

    };
            var jsonResult = Json(new

            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult globalsales()
        {
            InvoiceInstance objinvoiceInstance = new InvoiceInstance();
            List<InvoiceInstance> invoiceInstancesList = objinvoiceInstance.GetGlobalSales();

            ViewBag.CountryList = invoiceInstancesList.OrderByDescending(x => x.InvoiceAmount);
            ViewBag.Totalamount = invoiceInstancesList.Select(x => x.InvoiceAmount).Sum();

            return PartialView("_GlobalSales");
        }
        [HttpGet, LoginAuthorized]
        public PartialViewResult visualization()
        {
            InvoiceInstance objinvoiceInstance = new InvoiceInstance();
            ViewBag.InvoiceAmount = objinvoiceInstance.GetGlobalInvoice();
            int i = 0;
            long years1 = DateTime.Today.Year - 1; ;
            long years2 = DateTime.Today.Year;
            decimal amount1 = 0;
            decimal amount2 = 0;
            foreach (InvoiceInstance item in ViewBag.InvoiceAmount)
            {
                if (i == 0)
                {
                    years1 = item.Years;
                    amount1 = item.InvoiceAmount;
                }
                else
                {
                    years2 = item.Years;
                    amount2 = item.InvoiceAmount;
                }
                i++;


            }
            ViewBag.years1 = years1;
            ViewBag.amount1 = amount1;
            ViewBag.years2 = years2;
            ViewBag.amount2 = amount2;

            ViewBag.PrvDailyMaxJob = objinvoiceInstance.GetPreviousDailyMaxJobs();
            ViewBag.DailyMaxJob = objinvoiceInstance.GetDailyMaxJobs();
            ViewBag.WeeklyMaxJob = objinvoiceInstance.GetWeeklyMaxJobs();
            ViewBag.DailyMaxFiles = objinvoiceInstance.GetDailyMaxFile();
            ViewBag.QUARTER = objinvoiceInstance.GetGlobalInvoiceQUARTER();
            ViewBag.QUARTER1 = objinvoiceInstance.GetGlobalInvoiceQUARTERPre();
            ViewBag.Billing = objinvoiceInstance.GetGlobalInvoiceBilling();
            //ViewBag.WeeklySummary = objinvoiceInstance.GetGlobalWeeklySummary();
            ViewBag.JobWaterMarks = objinvoiceInstance.GetJobWaterMarks();

            decimal q1 = 0;
            decimal q2 = 0;
            decimal q3 = 0;
            decimal q4 = 0;
            foreach (InvoiceInstance item in ViewBag.QUARTER)
            {
                if (item.Years == 1)
                {
                    q1 = item.InvoiceAmount;
                }
                if (item.Years == 2)
                {
                    q2 = item.InvoiceAmount;
                }
                if (item.Years == 3)
                {
                    q3 = item.InvoiceAmount;
                }
                if (item.Years == 4)
                {
                    q4 = item.InvoiceAmount;
                }
            }
            ViewBag.q1 = q1;
            ViewBag.q2 = q2;
            ViewBag.q3 = q3;
            ViewBag.q4 = q4;

            decimal q5 = 0;
            decimal q6 = 0;
            decimal q7 = 0;
            decimal q8 = 0;
            foreach (InvoiceInstance item in ViewBag.QUARTER1)
            {
                if (item.Years == 1)
                {
                    q5 = item.InvoiceAmount;
                }
                if (item.Years == 2)
                {
                    q6 = item.InvoiceAmount;
                }
                if (item.Years == 3)
                {
                    q7 = item.InvoiceAmount;
                }
                if (item.Years == 4)
                {
                    q8 = item.InvoiceAmount;
                }
            }
            ViewBag.q5 = q5;
            ViewBag.q6 = q6;
            ViewBag.q7 = q7;
            ViewBag.q8 = q8;
            // ViewBag.total = q1 + q2 + q3 + q4;

            return PartialView("_Visualization");
        }

        public decimal ConversionRateAPI1(string fromCurrency, string to_Currency, decimal amount)
        {
            try
            {
                try
                {
                    string query = fromCurrency + "_" + to_Currency;
                    String URLString = "https://free.currconv.com/api/v7/convert?q=" + query + "&compact=ultra&apiKey=b8c493c2b903637553b5";
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

        //[HttpGet]
        //public PartialViewResult billingoffice()
        //{
        //    InvoiceInstance objinvoiceInstance = new InvoiceInstance();



        //    return PartialView("_GlobalSales");
        //}
    }
}
