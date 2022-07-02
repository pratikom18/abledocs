using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class CancelledJobsController : Controller
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        public CancelledJobsController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost,LoginAuthorized]
        public JsonResult GetCancelJobStatusList(JQueryDataTableParamModel param, string Status, int TodayTask)
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
            List<Jobs> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Jobs objJobMaster = new Jobs();
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
            allRecord = objJobMaster.GetJobStatusListForTable(startIndex, endIndex, OrderBy, SearchKey, "CANCELLEDJOBS", out totalRecords, TodayTask);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {                             
                             //c.JobID.ToString(),
                             c.ID.ToString(),                                                               //0
                             c.EngagementNum == null ? "":c.EngagementNum,                                  //1
                             c.Deadline == null ? "" : c.Deadline.ToString()+" "+c.DeadlineTime.ToString(), //2
                             //c.JobType == null ? "" : c.JobType.ToString(),       
                             c.JobType == null ? "" : c.JobType.ToString(),                                 //3
                             c.databasename.ToString(),                                                     //4
                             c.databasename== Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                                      //5

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
    }
}

