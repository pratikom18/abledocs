using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ContactsController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {

            ClientsContacts objClientAlpha = new ClientsContacts();
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "contacts").FirstOrDefault();
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
            var databasename = HttpContext.Session.GetString("Database");

            ViewBag.databasename = databasename;



            if (pagefound == "No")
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                ViewBag.AlphabetList = objClientAlpha.GetAlphabetList();
                return View();
            }





            //ViewBag.AlphabetList = objClientAlpha.GetAlphabetList();
            //return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ContactList(JQueryDataTableParamModel param, string AlphaSearch)
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
            List<ClientsContacts> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            ClientsContacts objContactMaster = new ClientsContacts();
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
            allRecord = objContactMaster.GetContactListForTable(startIndex, endIndex, OrderBy, SearchKey, AlphaSearch, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                var totalFilterRecord = objContactMaster.GetDataTableFiltered(SearchKey, AlphaSearch);
                filteredRecords = Convert.ToInt32(totalFilterRecord);//allRecord.Count();
            }

            string code = HttpContext.Session.GetString("Country");

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),                                                                                                       //0
                             c.Code == null ? "" : c.Code.ToString(),                                                                               //1
                             c.Company == null ? "" : c.Company.ToString(),                                                                         //2
                             c.FirstName == null ? "" : c.databasename == databasename? c.FirstName.ToString()+" "+c.LastName.ToString():"XXXXX",   //3
                             c.Email == null ? "" : c.databasename == databasename?c.Email.ToString():"XXXXX",                                      //4
                             c.City == null ? "" :c.City.ToString(),                                                                                //5
                             c.Country == null ? "" : c.Country.ToString(),                                                                         //6
                             c.Year == null ? "" : c.Year.ToString(),                                                                               //7
                             c.CountryCode == null ? "" :c.CountryCode.ToString(),                                                                  //8
                             c.databasename.ToString(),                                                                                             //9
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                                                                              //10

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
        public ActionResult Create()
        {
            var loginuserCountry = HttpContext.Session.GetString("Country");
            ViewBag.loginuserCountry = "";
            if (!string.IsNullOrEmpty(loginuserCountry))
            {
                ViewBag.loginuserCountry = loginuserCountry;
            }

            var databasename = HttpContext.Session.GetString("Database");

            ViewBag.databasename = databasename;

            ClientsContacts model = new ClientsContacts();
            string flag = databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ? "1" : "0";
            model.flag = flag;
            return View(model);
        }

        [HttpPost, LoginAuthorized]
        public ActionResult Create(ClientsContacts model)
        {
            try
            {
                var databasename = Utility.CommonHelper.Getabledocs(model.flag);
                model.databasename = databasename;
                Clients objClientMaster = new Clients();
                objClientMaster.databasename = databasename;
                objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(model.ClientID));
                model.Language = objClientMaster.Language;
                var id = model.InsertUpdate();
              
                return RedirectToAction("edit", new { id = Convert.ToInt32(id),flag = model.flag, save = 1});
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AuthorizedAction]
        [HttpGet]
        //[Route("edit/{id}")]
        // GET: ContactsController/Edit/5
        public ActionResult Edit(int id,string flag="",int save = 0)
        {
            var databasename = HttpContext.Session.GetString("Database");
            var databasename1 = Utility.CommonHelper.Getabledocs(flag);
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "contacts").FirstOrDefault();
            
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
                ClientsContacts objContactMaster = new ClientsContacts();
                objContactMaster.databasename = databasename1;
                objContactMaster = objContactMaster.GetContactById(Convert.ToInt32(id));
                AdscanUser objAdscanUser = new AdscanUser();
                objAdscanUser.databasename = Utility.CommonHelper.Getadscan(flag);
                objAdscanUser = objAdscanUser.GetAdscanUserById(objContactMaster.ADScan_UserID);
                if (objAdscanUser != null)
                {
                    ViewBag.AdscanUser = objAdscanUser.ID;
                }
                else
                {
                    ViewBag.AdscanUser = 0;
                }
                ViewBag.Save = save;

                string code = HttpContext.Session.GetString("Country");
                if (objContactMaster.Country != null)
                {
                    if (databasename != databasename1)
                    {
                        ViewBag.SameCountryUser = false;
                    }
                    else {
                        ViewBag.SameCountryUser = true;
                    }
                }
                else {
                    ViewBag.SameCountryUser = true;
                }
               
                ViewBag.databasename = databasename1;

                var loginuserCountry = HttpContext.Session.GetString("Country");
                ViewBag.loginuserCountry = "";
                if (!string.IsNullOrEmpty(loginuserCountry))
                {
                    ViewBag.loginuserCountry = loginuserCountry;
                }
                return View(objContactMaster);
            }
        }
        [HttpPost, LoginAuthorized]
        //[ValidateAntiForgeryToken]

        public ActionResult Edit(ClientsContacts model)
        {
            try
            {
                var databasename = Utility.CommonHelper.Getabledocs(model.flag);
                model.databasename = databasename;
                var message = model.InsertUpdate();
                return RedirectToAction("edit", new { id = Convert.ToInt32(model.ID),flag= model.flag, save = 1 });
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SetContactDatabase()
        {
            var databasename = Request.Form["databasename"];

            //HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename));
            var jsonResult = Json(new

            {
                result = true
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetCountryCode()
        {
            var country_code = Request.Form["country_code"];
            Countries model = new Countries();
            model.code = country_code;
            var result = model.GetCountryByCode();
            var jsonResult = Json(new

            {
                result = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public JsonResult GetClientAddress(int ClientID,string flag)
        {

            Clients model = new Clients();
            model.databasename = Utility.CommonHelper.Getabledocs(flag);
            var clientData = model.GetClientById(ClientID);
            
            var jsonResult = Json(new

            {
                result = clientData
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult createadscanuser(string Email,int CompanyID,string First_Name,string Last_Name,string Company_Address,string Phone_Num,string Postal_Code,string City,string Province,string Country,int ADO_Contact_ID,string Lang,string Role,string flag)
        {
            string CompanyName = "";
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            Clients objClients = new Clients();
            objClients.databasename = databasename;
            objClients = objClients.GetClientById(CompanyID);
            if (objClients != null)
            {
                CompanyName = objClients.Company;
            }
            string tmppassword = DateTime.Now.ToString("yyyyMMddHHmmss"); 
            AdscanUser objAdscanUser = new AdscanUser();
            objAdscanUser.databasename = Utility.CommonHelper.Getadscan(flag);
            objAdscanUser.Email = Email;
            objAdscanUser.CompanyID = CompanyID;
            objAdscanUser.CompanyName = CompanyName;
            objAdscanUser.First_Name = First_Name;
            objAdscanUser.Last_Name = string.IsNullOrEmpty(Last_Name)?"":Last_Name;
            objAdscanUser.Company_Address = Company_Address;
            objAdscanUser.Postal_Code = Postal_Code;
            objAdscanUser.City = City;
            objAdscanUser.Province = Province;
            objAdscanUser.Country = Country;
            objAdscanUser.Phone_Num = Phone_Num;
            objAdscanUser.ADO_Contact_ID = ADO_Contact_ID;
            objAdscanUser.Lang = Lang;
            objAdscanUser.Role = Role;
            objAdscanUser.Pwd = Utility.CommonHelper.GetMD5Hash(tmppassword);
            string retval = objAdscanUser.Insert();

            ClientsContacts objClientsContacts = new ClientsContacts();
            objClientsContacts.databasename = databasename;
            objClientsContacts.ID = ADO_Contact_ID;
            objClientsContacts.ADScan_UserID = Utility.CommonHelper.GetDBInt(retval);
            objClientsContacts.UpdateADScanUserID();
            //Email
            // email the user with their temp password
            //$msg = "Hello ".  $_POST["FirstName"].",\n\nYour ADScan account has been created.  Your temporary password is: ".$tempPass."\n\n";
            //      emailThis($_POST["Email"], $_POST["FirstName"].' '. $_POST["LastName"], 'ADScan Account Created',$msg);

            //
            var message = "Hello " + First_Name + ",\n\nYour ADScan account has been created.  Your temporary password is: "+ tmppassword +"\n\n";
            Utility.EmailHelper.EmailThis(Email, "", "", "ADScan Account Created", message, "", false, null);
            return Json(new { msg = "Your ADScan account has been created." });
        }

        [HttpPost, LoginAuthorized]
        public ActionResult resetadscanuser(int ID,string flag)
        {
            string tmppassword = DateTime.Now.ToString("yyyyMMddHHmmss");
            AdscanUser objAdscanUser = new AdscanUser();
            objAdscanUser.databasename = Utility.CommonHelper.Getadscan(flag);
            objAdscanUser.ADO_Contact_ID = ID;
            objAdscanUser.Pwd = Utility.CommonHelper.GetMD5Hash(tmppassword);
            objAdscanUser.UpdatePwd();

           var modelAdscanUser =  objAdscanUser.GetAdscanUserByContactId(ID);
            var Email = modelAdscanUser.Email;
            var FirstName = modelAdscanUser.First_Name;
            //Email
            // email the user with their temp password
            //$msg = "Hello ".  $_POST["FirstName"].",\n\nYour ADScan password has been reset.  Your temporary password is: ".$tempPass."\n\n";
            //emailThis($_POST["Email"], $_POST["FirstName"].' '. $_POST["LastName"], 'ADScan Password Reset',$msg);
            var message = "Hello "+ FirstName + ",\n\nYour ADScan password has been reset.  Your temporary password is: "+ tmppassword + "\n\n";
            Utility.EmailHelper.EmailThis(Email, "", "", "ADScan Password Reset", message, "", false, null);
            return Json(new { msg = "Your ADScan password has been reset." });
        }

        [HttpPost, LoginAuthorized]
        public JsonResult DeleteContact()
        {
            var flag = Request.Form["flag"];
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            var id = Request.Form["id"];

            //HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename));

            ERPClass objERP = new ERPClass();
            objERP.databasename = databasename;
           var modelERPJob = objERP.CheckRecordUsed("jobs", "ContactID", Convert.ToInt32(id));
           
            var msg = "";
            bool success = false;
            if (modelERPJob.Count == 0)
            {
                ClientsContacts model = new ClientsContacts();
                model.ID = Convert.ToInt32(id);
                model.databasename = databasename;
                model.Deleted = 1;
                model.DeleteContact();
                msg = "Deleted Successfully!";

                success = true;
            }
            else
            {
                msg = "Record deletion failed. Already assigned.";
                success = false;
            }

            var jsonResult = Json(new

            {
                result = success,
                msg = msg
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }
    }
}
