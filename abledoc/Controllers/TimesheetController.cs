using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class TimesheetController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.timesheetIDParam = "";
                ViewBag.currentTimesheetGlobal = 0;
            }
            else
            {
                ViewBag.timesheetIDParam = id.ToString();
                ViewBag.currentTimesheetGlobal = id;
            }

            Users users = new Users();
            users = users.GetUserById(Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID")));
            ViewBag.supervisorID = users.CurrentSupervisor;

            return View();
        }
        [HttpGet, LoginAuthorized]
        public PartialViewResult AddTime()
        {

            return PartialView("_addTime");
        }
        [HttpGet, LoginAuthorized]
        public PartialViewResult sendForProcessing()
        {

            return PartialView("_sendForProcessing");
        }

        [HttpPost, LoginAuthorized]
        public JsonResult AddTimeToTimesheet(string param)
        {
            var databasename = HttpContext.Session.GetString("Database");

            string[] paramsExplode = param.Split(" | ");
            string typeAddition = paramsExplode[0];
            string hours = paramsExplode[1];
            string message = paramsExplode[2];
            string dates = paramsExplode[3];
            string currentTimesheetGlobal = paramsExplode[4];

            string[] datesExplode = dates.Split(", ");

            for (int i = 0; i < datesExplode.Length; i++)
            {
                SecondTimer secondTimer = new SecondTimer();
                secondTimer.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                secondTimer.QueryType = typeAddition;
                secondTimer.ActualTime = 0.00;
                secondTimer.OverrideTime = Utility.CommonHelper.GetDouble(hours);
                secondTimer.Comment = message;
                secondTimer.DateStart = Utility.CommonHelper.GetDBDate(datesExplode[i]);
                secondTimer.TimesheetID = Utility.CommonHelper.GetDBInt(currentTimesheetGlobal);
                secondTimer.databasename = databasename;
                secondTimer.InsertTimeSheet();
            }

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult SendTimesheetForProcessing(string param)
        {
            var databasename = HttpContext.Session.GetString("Database");

            string[] paramsExplode = param.Split(" | ");
            double totalHour = Utility.CommonHelper.GetDBDouble(paramsExplode[0].ToString());
            string billableDuration = paramsExplode[1];
            string userID = paramsExplode[2];
            string supervisorID = paramsExplode[3];
            string originalHour = paramsExplode[4];
            double totalHourRounded = Utility.CommonHelper.RoundedOffHour(totalHour);

            TimesheetTracking timesheetTracking = new TimesheetTracking();
            var format = "yyyy-MM-dd H:mm:ss";
            timesheetTracking.CreatedTime = Utility.CommonHelper.GetDBDate(DateTime.Now.ToString(format));
            timesheetTracking.UserID = Utility.CommonHelper.GetDBInt(userID);
            timesheetTracking.TotalHour = totalHour;
            timesheetTracking.BillableDuration = billableDuration;
            timesheetTracking.SupervisorID = Utility.CommonHelper.GetDBInt(supervisorID);
            timesheetTracking.OriginalHour = Utility.CommonHelper.GetDBDouble(originalHour);
            timesheetTracking.TotalHourRounded = totalHourRounded;
            timesheetTracking.databasename = databasename;
            string lastID = timesheetTracking.Insert();

            var jsonResult = Json(new
            {
                message = lastID
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult UpdateTimeSheet(string flag,  int ID,int LastID,string flagdn)
        {
            var databasename = flagdn;

            if (flag == "jobsfilescheckouts")
            {
                TimesheetJobs timesheetJobs = new TimesheetJobs();
                timesheetJobs.ID = ID;
                timesheetJobs.TimesheetID = LastID;
                timesheetJobs.databasename = databasename;
                timesheetJobs.UpdateTimeSheet();

                timesheetJobs = new TimesheetJobs();
                timesheetJobs.databasename = databasename;
                timesheetJobs = timesheetJobs.getDetailByID(ID);

                string[] AllIDS = timesheetJobs.AllTimersID.Split(" | ");
                string query = string.Empty;
                foreach (string item in AllIDS)
                {
                    AllTimers allTimers = new AllTimers();
                    allTimers.TimesheetID = LastID;
                    allTimers.ID = Utility.CommonHelper.GetDBInt(item);
                    allTimers.databasename = databasename;
                    allTimers.UpdateTimeSheetID();
                }

            }
            else if (flag == "secondtimer")
            {
                SecondTimer secondTimer = new SecondTimer();
                secondTimer.ID = ID;
                secondTimer.TimesheetID = LastID;
                secondTimer.databasename = databasename;
                secondTimer.UpdateTimeSheet();
            }

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SendTimesheetForProcessingAgain(string param)
        {
            var databasename = HttpContext.Session.GetString("Database");

            string[] paramsExplode = param.Split(" | ");
            string timesheetID = paramsExplode[0];
            double totalHour = Utility.CommonHelper.GetDBDouble(paramsExplode[1].ToString());
            double totalHourRounded = Utility.CommonHelper.RoundedOffHour(totalHour);

            TimesheetTracking timesheetTracking = new TimesheetTracking();
            timesheetTracking.ID = Utility.CommonHelper.GetDBInt(timesheetID);
            timesheetTracking.TotalHour = totalHour;
            timesheetTracking.TotalHourRounded = totalHourRounded;
            timesheetTracking.databasename = databasename;
            timesheetTracking.SendTimesheetForProcessingAgain();

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }
    }
}
