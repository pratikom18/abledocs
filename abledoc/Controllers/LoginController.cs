using abledoc.Models;
using abledoc.Utility.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class LoginController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public ActionResult Index()
        {
            Users objUsers = new Users();

            if (string.IsNullOrEmpty(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID")))
            {
                ViewBag.Remember = _httpContextAccessor.HttpContext.Request.Cookies["Remember"];
                ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["UserName"];
                ViewBag.Password = _httpContextAccessor.HttpContext.Request.Cookies["Password"];

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        #region Login
        [HttpPost]
        public JsonResult Signin(string UserName, string Password, bool remember)
        {
            //  HttpContext.Session.SetString("Location", Utility.CommonHelper.GetDBString(Location));
          string pwd =  Utility.CommonHelper.GetMD5Hash(Password);
            Users objUsers = new Users();
            objUsers.Username = UserName;
            objUsers.Password = pwd;
            objUsers = objUsers.GetDataByEmailPassword();

            if (objUsers != null)
            {
                if (objUsers.ID != 0)
                {

                    HttpContext.Session.SetString("ID", Utility.CommonHelper.GetDBString(objUsers.ID));
                    HttpContext.Session.SetString("FirstName", Utility.CommonHelper.GetDBString(objUsers.FirstName));
                    HttpContext.Session.SetString("LastName", Utility.CommonHelper.GetDBString(objUsers.LastName));
                    HttpContext.Session.SetString("UserName", Utility.CommonHelper.GetDBString(objUsers.Username));
                    HttpContext.Session.SetString("EmailID", Utility.CommonHelper.GetDBString(objUsers.Email));
                    HttpContext.Session.SetString("UserRoleId", Utility.CommonHelper.GetDBString(objUsers.UserRoleId));
                    HttpContext.Session.SetString("Password", Utility.CommonHelper.GetDBString(objUsers.Password));
                    HttpContext.Session.SetString("Country", Utility.CommonHelper.GetDBString(objUsers.Country));
                    Countries objCountry = new Countries();
                    objCountry.code = objUsers.Country;
                    var modelCountry = objCountry.GetCountryByCode();
                    var databasename = Models.Utility.Constants.ABLEDOCS_DB;
                    var database_adscan = Models.Utility.Constants.ADSCAN_DB;
                    var database_gateway = Models.Utility.Constants.GATEWAY_DB;
                    var database_mantis = Models.Utility.Constants.Mantis_DB;
                    var database_legacy = Models.Utility.Constants.LEGACY_DB;
                    if (modelCountry != null)
                    {
                        if (modelCountry.EUcountry == Utility.CommonHelper.GetDBBoolean(1))
                        {
                            databasename = Models.Utility.Constants.ABLEDOCS_EU_DB;
                            database_adscan = Models.Utility.Constants.ADSCAN_EU_DB;
                            database_gateway = Models.Utility.Constants.GATEWAY_EU_DB;
                            database_mantis = Models.Utility.Constants.Mantis_EU_DB;
                            database_legacy = Models.Utility.Constants.LEGACY_EU_DB;

                        }
                    }
                    

                    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename));
                    HttpContext.Session.SetString("CommonDatabase", Utility.CommonHelper.GetDBString(databasename));
                    HttpContext.Session.SetString("Database_edit", Utility.CommonHelper.GetDBString(databasename));
                    HttpContext.Session.SetString("database_adscan", Utility.CommonHelper.GetDBString(database_adscan));
                    HttpContext.Session.SetString("database_gateway", Utility.CommonHelper.GetDBString(database_gateway));
                    HttpContext.Session.SetString("database_mantis", Utility.CommonHelper.GetDBString(database_mantis));
                    HttpContext.Session.SetString("database_legacy", Utility.CommonHelper.GetDBString(database_legacy));

                    //HttpContext.Session.SetString("database_adscan_edit", Utility.CommonHelper.GetDBString(database_adscan));
                    //HttpContext.Session.SetString("database_gateway_edit", Utility.CommonHelper.GetDBString(database_gateway));
                    //HttpContext.Session.SetString("database_mantis_edit", Utility.CommonHelper.GetDBString(database_mantis));
                    //HttpContext.Session.SetString("database_legacy_edit", Utility.CommonHelper.GetDBString(database_legacy));

                    HttpContext.Session.GetString("FirstName");
                    HttpContext.Session.GetString("LastName");
                    HttpContext.Session.GetString("UserName");
                    HttpContext.Session.GetString("EmailID");
                    HttpContext.Session.GetString("UserRoleId");
                    UserRoles objUserRoles = new UserRoles();
                    List<UserRoles> UserRoleList = new List<UserRoles>();
                    var databasename1 = HttpContext.Session.GetString("Database");
                    objUserRoles.databasename = databasename1;
                    UserRoleList = objUserRoles.GetUserRoleByUserId(objUsers.ID);

                    objUsers.Password = HttpContext.Session.GetString("Password");

                    CookieOptions option = new CookieOptions();

                    if (!remember)
                    {
                        option.Expires = DateTime.Now.AddDays(1);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Remember", remember.ToString(), option);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("UserName", "", option);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Password", "", option);
                    }
                    else
                    {
                        option.Expires = DateTime.Now.AddDays(365);

                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Remember", remember.ToString(), option);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("UserName", objUsers.Username, option);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Password", Password, option);
                    }


                    Utility.Session.SessionExtensions.SetComplexData(HttpContext.Session, "UserRoles", UserRoleList);

                    AssignedMenu objAssignedMenu = new AssignedMenu();
                    List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
                    
                    AssignedMenuList = objAssignedMenu.GetAssignedMenuList(objUsers.ID);

                    List<AssignedMenu> AssignedMenuList2 = new List<AssignedMenu>();
                    
                    AssignedMenuList2 = objAssignedMenu.GetAssignedMenuList1(objUsers.ID);

                    if (AssignedMenuList != null)
                    {
                        List<AssignedMenu> AssignedMenuList1 = new List<AssignedMenu>();
                        AssignedMenuList1 = AssignedMenuList.Where(x => x.IsHeaderMenu == false).ToList();
                        AssignedMenuList2 = AssignedMenuList2.Where(x => x.IsHeaderMenu == true).ToList();
                        foreach (var item in AssignedMenuList1)
                        {
                            if (item.menuname == "Profile")
                            {
                                HttpContext.Session.SetString("Profile", "1");
                            }
                            else if (item.menuname == "User Setting")
                            {
                                HttpContext.Session.SetString("Setting", "1");
                            }

                        }

                    }
                    Utility.Session.SessionExtensions.SetComplexData(HttpContext.Session, "AssignedMenu1", AssignedMenuList2);
                    Utility.Session.SessionExtensions.SetComplexData(HttpContext.Session, "AssignedMenu", AssignedMenuList);

                    objUsers = null;
                    var jsonResult = Json(new
                    {
                        messageKey = "True",
                        message = ""
                    });
                    return jsonResult;
                }
                else
                {
                    var jsonResult = Json(new
                    {
                        messageKey = "False",
                        message = "Your username or password is not correct."
                    });
                    return jsonResult;

                }

            }
            else
            {
                var jsonResult = Json(new
                {
                    messageKey = "False",
                    message = "Your username or password is not correct."
                });
                return jsonResult;

            }
        }
        #endregion

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Login");
        }


    }
}
