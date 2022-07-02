using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using abledoc.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace abledoc.Controllers
{
    public class ClientsController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            Clients objClientAlpha = new Clients();
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "clients").FirstOrDefault();
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
                ViewBag.AlphabetList = objClientAlpha.GetAlphabetList();
                return View();
            }

        }
        [HttpPost, LoginAuthorized]
        public JsonResult ClientList(JQueryDataTableParamModel param, string AlphaSearch)
        {
            var databasename = HttpContext.Session.GetString("Database");
            //var databasename1 = HttpContext.Session.GetString("Database_edit");

            //var database_adscan_edit = HttpContext.Session.GetString("database_adscan_edit");
            //var database_gateway_edit = HttpContext.Session.GetString("database_gateway_edit");
            //var database_mantis_edit = HttpContext.Session.GetString("database_mantis_edit");
            //var database_legacy_edit = HttpContext.Session.GetString("database_legacy_edit");

            //if (databasename != databasename1)
            //{
            //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
            //    HttpContext.Session.SetString("database_adscan", Utility.CommonHelper.GetDBString(database_adscan_edit));
            //    HttpContext.Session.SetString("database_gateway", Utility.CommonHelper.GetDBString(database_gateway_edit));
            //    HttpContext.Session.SetString("database_mantis", Utility.CommonHelper.GetDBString(database_mantis_edit));
            //    HttpContext.Session.SetString("database_legacy", Utility.CommonHelper.GetDBString(database_legacy_edit));
            //}

            int totalRecords = 0;
            List<Clients> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Clients objClientsMaster = new Clients();
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
            allRecord = objClientsMaster.GetClientListForTable(startIndex, endIndex, OrderBy, SearchKey, AlphaSearch, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                var totalFilterRecord = objClientsMaster.GetDataTableFiltered(SearchKey, AlphaSearch);
                filteredRecords = Convert.ToInt32(totalFilterRecord);//allRecord.Count();
            }

            string code = HttpContext.Session.GetString("Country");

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),
                             c.Code == null ? "" : c.Code.ToString(),
                             c.Company == null ? "" : c.Company.ToString(),
                             c.Email == null ? "" :c.databasename == databasename ?c.Email.ToString():"XXXXX",
                             c.City == null ? "" :c.City.ToString(),
                             c.Country == null ? "" : c.Country.ToString(),
                             c.Year == null ? "" : c.Year.ToString(),
                             c.CountryCode == null ? "" :c.CountryCode.ToString(),
                             c.databasename.ToString(),
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",

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
        public ActionResult Create(int id = 0)
        {
            Clients model = new Clients();
            var loginuserCountry = HttpContext.Session.GetString("Country");
            ViewBag.loginuserCountry = "";
            if (!string.IsNullOrEmpty(loginuserCountry))
            {
                ViewBag.loginuserCountry = loginuserCountry;
            }

            var databasename = HttpContext.Session.GetString("Database");

            States objStateMaster = new States();
            model.databasename = HttpContext.Session.GetString("CommonDatabase");
            ViewBag.StateList = objStateMaster.GetStateList(ViewBag.loginuserCountry);

            ClientsContacts objContactMaster = new ClientsContacts();
            objContactMaster.databasename = databasename;
            ViewBag.ContactList = objContactMaster.GetClientContactList(Convert.ToInt32(id));

            ADscanCrawls objADscanMaster = new ADscanCrawls();
            objADscanMaster.databasename = HttpContext.Session.GetString("database_adscan");
            ViewBag.ADscanList = objADscanMaster.GetADscanCrawlsListByClientId(Convert.ToInt32(id));

            Gateways objGatewaysMaster = new Gateways();
            objGatewaysMaster.databasename = HttpContext.Session.GetString("database_gateway");
            ViewBag.GatewaysList = objGatewaysMaster.GatewaysListByClientId(Convert.ToInt32(id));

            ClientSpecificRequirements objCSR = new ClientSpecificRequirements();
            objCSR.databasename = databasename;
            ViewBag.CSRList = objCSR.GetCSRById(Convert.ToInt32(id));

            model.flag = databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ? "1" : "0";
            return View(model);

        }


        [HttpPost, LoginAuthorized]
        public ActionResult Upsert(ClientSpecificRequirements model)
        {
            try
            {
                model.databasename = Utility.CommonHelper.Getabledocs(model.flag);
                var id = model.InsertUpdate();
                return RedirectToAction("edit", new { id = Convert.ToInt32(model.ClientID),flag= model.flag, save = 1 });
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        [HttpPost, LoginAuthorized]
        public ActionResult Create(Clients model)
        {
            try
            {
                var databasename = Utility.CommonHelper.Getabledocs(model.flag);
                model.databasename = databasename;

                var id = model.InsertUpdate();

                ClientsMultirate modeJobsDeliveryContact = new ClientsMultirate();
                modeJobsDeliveryContact.databasename = databasename;
                List<string> ContactIDs = new List<string>();

                foreach (string key in Request.Form.Keys)
                {
                    if (key.Contains("TypeFilter"))
                    {
                        if (Request.Form[key].ToString() != "")
                        {


                            modeJobsDeliveryContact.clientid = Utility.CommonHelper.GetDBInt(id);
                            if (key.Contains("RateType"))
                            {
                                ContactIDs.Add("'" + Request.Form[key].ToString() + "'");
                                modeJobsDeliveryContact.typecode = Request.Form[key].ToString();
                                continue;
                            }
                            if (key.Contains("Rate"))
                            {
                                modeJobsDeliveryContact.rate = Utility.CommonHelper.GetDBDecimal(Request.Form[key].ToString());
                            }



                            modeJobsDeliveryContact.Insert();
                        }
                    }
                }

                if (ContactIDs != null)
                {
                    string conIDs = String.Join(",", ContactIDs);
                    string clientid = id;

                    modeJobsDeliveryContact.Deleterate(clientid, conIDs);
                }
               // string flag = databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ? "1" : "0";
                return RedirectToAction("edit", new { id = Convert.ToInt32(id),flag= model.flag, save = 1 });
                //return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        [HttpGet, LoginAuthorized]
        public ActionResult Edit(int id, string flag, int save = 0)
        {
            var databasename1 = HttpContext.Session.GetString("Database");
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            

            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "clients").FirstOrDefault();

            string isEdit = "No";


            if (objAssignedMenu != null)
            {


                if (objAssignedMenu.isupdate)
                {
                    isEdit = "yes";

                }

            }

            ViewBag.isEdit = isEdit;



            if (isEdit == "No")
            {
                return RedirectToAction("NoAccess", "Error");
            }
            else
            {

                Clients objClientMaster = new Clients();

                objClientMaster.databasename = databasename;
                objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(id));

                States objStateMaster = new States();
                ViewBag.StateList = objStateMaster.GetStateList(objClientMaster.Country);

                ClientsContacts objContactMaster = new ClientsContacts();
                objContactMaster.databasename = databasename;
                List<ClientsContacts> clientsContactsList = objContactMaster.GetClientContactList(Convert.ToInt32(id));
                if (clientsContactsList != null)
                {
                    if (databasename != databasename1)
                    {
                        foreach (ClientsContacts item in clientsContactsList)
                        {
                            if (item.Country != null)
                            {
                                item.FullName = "XXXXX";
                                item.Email = "XXXXX";
                            }
                        }
                    }

                }

                ViewBag.ContactList = clientsContactsList;
                ADscanCrawls objADscanMaster = new ADscanCrawls();
                objADscanMaster.databasename = Utility.CommonHelper.Getadscan(flag);
                ViewBag.ADscanList = objADscanMaster.GetADscanCrawlsListByClientId(Convert.ToInt32(id));

                Gateways objGatewaysMaster = new Gateways();
                objGatewaysMaster.databasename = Utility.CommonHelper.Getgateway(flag);
                ViewBag.GatewaysList = objGatewaysMaster.GatewaysListByClientId(Convert.ToInt32(id));

                ADLegacyCustom objLegacyCustom = new ADLegacyCustom();
                objLegacyCustom.databasename = Utility.CommonHelper.Getlegacy(flag);
                ViewBag.ADLegacyCustom = objLegacyCustom.GetADlegacyCustomById(Convert.ToInt32(id));

                ClientSpecificRequirements objCSR = new ClientSpecificRequirements();
                objCSR.databasename = databasename;
                ViewBag.CSRList = objCSR.GetCSRByClientCode(objClientMaster.Code);

                ViewBag.Save = save;

                ClientsMultirate objJobDeliveryMaster = new ClientsMultirate();
                objJobDeliveryMaster.databasename = databasename;
                ViewBag.JobDeliveryContactData = objJobDeliveryMaster.GetList(id);


                if (objClientMaster.Country != null)
                {
                    if (databasename != databasename1)
                    {
                        ViewBag.SameCountryUser = false;
                    }
                    else
                    {
                        ViewBag.SameCountryUser = true;
                    }
                }
                else
                {
                    ViewBag.SameCountryUser = true;
                }
                objClientMaster.databasename = databasename;
                objClientMaster.flag = flag;
                return View(objClientMaster);

            }
        }

        [HttpPost, LoginAuthorized]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Clients model)
        {
            try
            {
                var legacy = Utility.CommonHelper.Getlegacy(model.flag);
                var databasename = Utility.CommonHelper.Getabledocs(model.flag);
              

                if (Request.Form["AddLegacy"] == "1")
                {
                    if (string.IsNullOrEmpty(model.Language))
                    {
                        model.Language = "EN";
                    }

                    ADLegacyCustom objLegacyCustom = new ADLegacyCustom();
                    objLegacyCustom.databasename = legacy;
                    objLegacyCustom.ClientID = Convert.ToString(model.ID);
                    objLegacyCustom.ClientName = model.Company;
                    objLegacyCustom.Languages = model.Language;
                    var id = objLegacyCustom.InsertLegacyCustom();
                    return RedirectToAction("edit", "legacy", new { id = Convert.ToInt32(model.ID), flag = model.flag, save = 1 });
                }
                else
                {
                    
                    model.databasename = databasename;

                    var message = model.InsertUpdate();


                    return RedirectToAction("edit", new { id = Convert.ToInt32(model.ID), flag = model.flag, save = 1 });
                }
                // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult AddUrl(int id,string flag)
        {
            Clients objClientMaster = new Clients();
            objClientMaster.databasename = Utility.CommonHelper.Getabledocs(flag);
            if (id != 0)
            {

                objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(id));
                ViewBag.PageMode = "Edit";
            }
            else
            {
                ViewBag.PageMode = "Add";
            }
            objClientMaster.flag = flag;
            return PartialView("_addurl", objClientMaster);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult AddGateway(int id,string flag)
        {
            Clients objClientMaster = new Clients();
            objClientMaster.databasename = Utility.CommonHelper.Getabledocs(flag);
            if (id != 0)
            {
                objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(id));
                ViewBag.PageMode = "Edit";
            }
            else
            {
                ViewBag.PageMode = "Add";
            }
            objClientMaster.flag = flag;
            return PartialView("_addgateway", objClientMaster);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult AddContact(int id,string flag)
        {
            Clients objClientMaster = new Clients();
            objClientMaster.databasename = Utility.CommonHelper.Getabledocs(flag);
            if (id != 0)
            {
                objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(id));
                ViewBag.PageMode = "Edit";
            }
            else
            {
                ViewBag.PageMode = "Add";
            }
            objClientMaster.flag = flag;
            ViewBag.modelClient = objClientMaster;

            return PartialView("_addcontact");
        }


        [HttpGet, LoginAuthorized]
        public PartialViewResult AddCSR(int id, string code, int clientid,string flag)
        {
            ClientSpecificRequirements objcsr = new ClientSpecificRequirements();
            objcsr.databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.client_code = code;
            ViewBag.clientid = clientid;

            if (id != 0)
            {
                objcsr = objcsr.GetCSRById(Convert.ToInt32(id));
                ViewBag.PageMode = "Edit";
            }
            else
            {
                ViewBag.PageMode = "Add";
            }
            objcsr.flag = flag;
            return PartialView("_addcsr", objcsr);
        }
        [LoginAuthorized]
        public JsonResult DeleteCSR(int id,string flag)
        {
            ClientSpecificRequirements objcsr = new ClientSpecificRequirements();
            objcsr.databasename = Utility.CommonHelper.Getabledocs(flag);
            objcsr.Delete(id);

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetStateList()
        {
            var country_code = Request.Form["country_code"];
            States model = new States();
            var result = model.GetStateList(country_code);
            var jsonResult = Json(new

            {
                result = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult UpdateMultirate(int multiID, int clientID, string RateType, decimal MultiRate,string flag)
        {
            string ID = "0";

            if (clientID != 0)
            {
                ClientsMultirate clientsMultirate = new ClientsMultirate();
                clientsMultirate.databasename = Utility.CommonHelper.Getabledocs(flag);
                
                if (multiID == 0)
                {
                    clientsMultirate.clientid = clientID;
                    clientsMultirate.typecode = RateType;
                    clientsMultirate.rate = MultiRate;
                    ID = clientsMultirate.Insert();
                }
                else
                {
                    clientsMultirate.clientsmultirate_id = multiID;
                    clientsMultirate.clientid = clientID;
                    clientsMultirate.typecode = RateType;
                    clientsMultirate.rate = MultiRate;
                    clientsMultirate.Update();
                }
            }
           

            var jsonResult = Json(new
            {
                ID = ID
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult DeleteMultirate(int multiID,string flag)
        {

            if (multiID != 0)
            {
                ClientsMultirate clientsMultirate = new ClientsMultirate();
                clientsMultirate.databasename = Utility.CommonHelper.Getabledocs(flag);
                clientsMultirate.clientsmultirate_id = multiID;
                clientsMultirate.Delete();
            }
           

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        //[HttpPost]
        //public JsonResult SetContactDatabase()
        //{
        //    var databasename = Request.Form["databasename"];


        //    var database_adscan = "adscan";
        //    var database_gateway = "gateway";
        //    var database_mantis = "mantis";
        //    var database_legacy = "legacy";
        //    if (databasename == Models.Utility.Constants.ABLEDOCS_EU_DB)
        //    {
        //        database_adscan = "adscan_eu";
        //        database_gateway = "gateway_eu";
        //        database_mantis = "mantis_eu";
        //        database_legacy = "legacy_eu";
        //    }
        //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename));
        //    HttpContext.Session.SetString("database_adscan", Utility.CommonHelper.GetDBString(database_adscan));
        //    HttpContext.Session.SetString("database_gateway", Utility.CommonHelper.GetDBString(database_gateway));
        //    HttpContext.Session.SetString("database_mantis", Utility.CommonHelper.GetDBString(database_mantis));
        //    HttpContext.Session.SetString("database_legacy", Utility.CommonHelper.GetDBString(database_legacy));

        //    var jsonResult = Json(new

        //    {
        //        result = true
        //    });
        //    //jsonResult.m = int.MaxValue;
        //    return jsonResult;
        //}
    }
}
