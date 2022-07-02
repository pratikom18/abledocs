using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ToBeInvoicedController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public ToBeInvoicedController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetToBeInvoicedList(JQueryDataTableParamModel param, string Status, int TodayTask)
        {
            var databasename = HttpContext.Session.GetString("Database");
            //var databasename1 = HttpContext.Session.GetString("Database_edit");
            //if (databasename != databasename1)
            //{
            //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
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
            allRecord = objJobMaster.GetToBeInvoicedListForTable(startIndex, endIndex, OrderBy, SearchKey, Status, out totalRecords, TodayTask);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            double timerSeconds = 0.00;
            double timeInHour = 0.00;

            foreach (Jobs item in allRecord)
            {
                AllTimers allTimers = new AllTimers();
                List<AllTimers> AllTimersList = new List<AllTimers>();
                allTimers.JobID = item.ID;
                allTimers.databasename = item.databasename;
                AllTimersList = allTimers.GetAllTimersByJobId();
                if (AllTimersList != null)
                {
                    foreach (AllTimers item1 in AllTimersList)
                    {
                        timerSeconds += Utility.CommonHelper.TimeFormatToSeconds(item1.TimerNow);
                    }
                }

                timeInHour = Math.Round(timerSeconds / 3600, 2);
                item.hours = Utility.CommonHelper.GetDBDecimal(timeInHour);
                JobsFiles jobsFiles = new JobsFiles();
                
                jobsFiles.databasename = item.databasename;
                jobsFiles = jobsFiles.GetJobIdQuotedAsValue(item.ID);

                if (item.JobQuotedAs == "Hour")
                {
                    if (jobsFiles.PricePer == "Hour")
                    {
                        item.JobQuotedAs = jobsFiles.totalQuantity.ToString();
                    }
                    else
                    {
                        item.JobQuotedAs = "Timed";
                    }
                }

                item.fileC = jobsFiles.fileC;
                item.totalPages = jobsFiles.totalPages;
                item.totalQuantity = jobsFiles.totalQuantity;
                item.pricePer = jobsFiles.PricePer;

                //var format = "Y-m-d";
                DateTime startDate = item.Created;//.ToString(format);
                DateTime endDate = DateTime.Now;//.ToString(format);
                TimeSpan age = endDate.Subtract(startDate);
                item.Age = Utility.CommonHelper.GetDBInt(age.Days.ToString());
            }


            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),                                                               //0
                             c.EngagementNum == null ? "" :c.EngagementNum,                                 //1
                             c.Deadline == null ? "" : c.Deadline.ToString()+" "+c.DeadlineTime.ToString(), //2
                             c.JobType == null ? "" : c.JobType.ToString(),                                 //3
                             c.Status== null ? "" : c.Status.ToString(),                                    //4
                             c.fileC == null?"":c.fileC.ToString(),                                         //5
                             c.totalPages == null?"":c.totalPages.ToString(),                               //6
                             c.Notes== null ? "" : c.Notes.ToString(),                                      //7
                              c.Age.ToString(),                                                             //8
                               c.JobQuotedAs== null ? "" : c.JobQuotedAs.ToString(),                        //9
                             c.hours == null?"":c.hours.ToString(),                                         //10
                             c.databasename.ToString(),                                                     //11
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                                      //12


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
