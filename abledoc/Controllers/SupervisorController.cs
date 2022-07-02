using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class SupervisorController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SupervisorList(JQueryDataTableParamModel param, string AlphaSearch)
        {

            int totalRecords = 0;
            List<TimesheetTracking> allRecord = new List<TimesheetTracking>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            TimesheetTracking objClientsMaster = new TimesheetTracking();
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
            var databasename = HttpContext.Session.GetString("Database");
            objClientsMaster.databasename = databasename;
            allRecord = objClientsMaster.GetSupervisorList(startIndex, endIndex, OrderBy, SearchKey, Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"), out totalRecords);

            int filteredRecords = totalRecords;
           
            double totalHourRounded = 0.00;
            objClientsMaster.TotalHourRounded = Utility.CommonHelper.GetDBDouble(totalHourRounded);

            var result = from c in allRecord
                         select new[] {
                             c.TimesheetID == null ? "" : c.TimesheetID.ToString(),
                             c.FullName == null ? "" : c.FullName.ToString(),
                             c.BillableDuration == null ? "" : c.BillableDuration.ToString(),
                             c.TotalHourRounded == null ? "" :c.TotalHourRounded.ToString(),
                             c.ID.ToString(),
                             c.UserID.ToString(),
                             c.OriginalHour == null?"":c.OriginalHour.ToString(),
                             c.TotalHour == null?"":c.TotalHour.ToString(),
        };
            var jsonResult = Json(new

            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult UpdateSupervisor(string flag, string Comment,double OverrideTime,string SupervisorComment,int ID,string flagdn)
        {
            var databasename = flagdn;

            if (flag == "jobsfilescheckouts")
            {
                TimesheetJobs timesheetJobs = new TimesheetJobs();
                timesheetJobs.ID = ID;
                timesheetJobs.Comment = Comment;
                timesheetJobs.SupervisorComment = SupervisorComment;
                timesheetJobs.OverrideTime = OverrideTime;
                timesheetJobs.databasename = databasename;
                timesheetJobs.UpdateSupervisor();
            }
            else if (flag == "secondtimer")
            {
                SecondTimer secondTimer = new SecondTimer();
                secondTimer.ID = ID;
                secondTimer.Comment = Comment;
                secondTimer.SupervisorComment = SupervisorComment;
                secondTimer.OverrideTime = OverrideTime;
                secondTimer.databasename = databasename;
                secondTimer.UpdateSupervisor();
            }
            
            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SendBackTimesheet(string timesheetID)
        {
            var databasename = HttpContext.Session.GetString("Database");
            TimesheetTracking timesheetTracking = new TimesheetTracking();
            timesheetTracking.ID = Utility.CommonHelper.GetDBInt(timesheetID);
            timesheetTracking.databasename = databasename;
            timesheetTracking.UpdateSentBack();

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ApproveTimesheet(string timesheetID)
        {
            var databasename = HttpContext.Session.GetString("Database");
            TimesheetTracking timesheetTracking = new TimesheetTracking();
            timesheetTracking.ID = Utility.CommonHelper.GetDBInt(timesheetID);
            timesheetTracking.databasename = databasename;
            timesheetTracking.UpdateApproved();

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }
    }
}
