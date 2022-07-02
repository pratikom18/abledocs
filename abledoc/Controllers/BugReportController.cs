using abledoc.Models;
using abledoc.Utility.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Controllers
{
    public class BugReportController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View("Index");
        }
        [LoginAuthorized]
        public IActionResult Create(BugReport model)
        {
            try
            {
                bool isError = false;
                if (string.IsNullOrEmpty(model.bugText.Steps_To_Reproduce))
                {
                    model.bugText.Steps_To_Reproduce = string.Empty;
                }
                if (string.IsNullOrEmpty(model.bugText.Description))
                {
                    model.bugText.Description = string.Empty;
                }
                if (string.IsNullOrEmpty(model.bug.Summary))
                {
                    model.bug.Summary = string.Empty;
                }

                if (model.bugText.Steps_To_Reproduce.ToString().Length < 20)
                {
                    ViewBag.Message = "You must provide DETAILED steps to reproduce your issue";
                    ViewBag.isError = true;
                    isError = true;
                }
                else if (model.bugText.Description.ToString().Length < 20)
                {
                    ViewBag.Message = "You must provide a DETAILED description issue";
                    ViewBag.isError = true;
                    isError = true;
                }
                else if (model.bug.Summary.ToString().Length < 20)
                {
                    ViewBag.Message = "You must provide a summary of the issue";
                    ViewBag.isError = true;
                    isError = true;
                }
                if (!isError)
                {
                    //ViewBag.field_steps_to_reproduce ="";
                    Users objUsers = new Users();
                    objUsers.ID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                    objUsers.FirstName = HttpContext.Session.GetString("FirstName");
                    objUsers.LastName = HttpContext.Session.GetString("LastName");
                    objUsers.Username = HttpContext.Session.GetString("UserName");
                    objUsers.Email = HttpContext.Session.GetString("EmailID");
                    objUsers.RealName = HttpContext.Session.GetString("LastName");
                    objUsers.Password = HttpContext.Session.GetString("UserName");
                    objUsers.Cookie_String = HttpContext.Session.GetString("UserName");
                    
                    string UserID = objUsers.InsertUpdateUserBugReport();
                    //  var ids = objUsers.InsertUpdateUserBugReport();

                    BugText objBugText = new BugText();
                    objBugText = model.bugText;
                   
                    string bugTextID = objBugText.InsertUpdateBugText();

                    Bug objBug = new Bug();
                    objBug = model.bug;
                    objBug.Reporter_Id = Utility.CommonHelper.GetDBInt(UserID);
                    objBug.Bug_Text_Id = Utility.CommonHelper.GetDBInt(bugTextID);
                    string currentStatus = string.Empty;
                    foreach (string key in Request.Form.Keys)
                    {
                        if (key == "Priority")
                        {
                            currentStatus = Request.Form[key].ToString();
                            objBug.Priority = Convert.ToInt32(currentStatus);
                        }
                        if (key == "Severity")
                        {
                            currentStatus = Request.Form[key].ToString();
                            objBug.Severity = Convert.ToInt32(currentStatus);
                        }
                    }
                    string BugId = objBug.InsertUpdateBug();

                    BugHistory objBugHistory = new BugHistory();
                    objBugHistory.User_Id = Utility.CommonHelper.GetDBInt(UserID);
                    objBugHistory.Bug_Id = Utility.CommonHelper.GetDBInt(BugId);
                    
                    objBugHistory.InsertUpdateBugHistory();
                    ViewBag.Message = "Thank you - your bug has been submitted";
                    ViewBag.isError = false;
                }

                return View("Index");
            }
            catch (Exception exp)
            {
                return View("Index");
            }
        }




    }
}
