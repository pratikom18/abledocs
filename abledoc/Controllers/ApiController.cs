using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    
    public class ApiController : ControllerBase
    {
        [HttpPost]
        public dynamic GetJobsList(
        JQueryDataTableParamModel param, string Status,int TodayTask)
        {
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
            allRecord = objJobMaster.GetJobListForTable(startIndex, endIndex, OrderBy, SearchKey, Status, out totalRecords, TodayTask);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.Code == null ? "" : c.Code.ToString(),
                             c.JobID.ToString(),
                             c.Deadline == null ? "" : c.Deadline.ToString()+" "+c.DeadlineTime.ToString(),
                             c.JobType == null ? "" : c.JobType.ToString(),
                             c.Files.ToString(),
                             c.Pages.ToString(),
                             c.Currency == null ? "" : c.Currency.ToString(),
                             c.QuotedValue.ToString("#,##0.00"),
                             c.QuotedHours.ToString("#,##0.00"),
                             c.hours.ToString("#,##0.00"),
                             c.FinalPercent == 0 ? c.FinalFiles.ToString()+"/"+c.Files.ToString()+"(0%)": c.FinalFiles.ToString()+"/"+c.Files.ToString()+ "("+c.FinalPercent.ToString("#.##")+"%)",
                             c.ClientID.ToString(),
                             c.FinalPercent.ToString(),
                             c.Deadline == null ? "" : c.Deadline.ToString(),
                         };

            return new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            };
        }


        [HttpPost]
        public dynamic GetClientsList(
        JQueryDataTableParamModel param, string Status)
        {
            int totalRecords = 0;
            List<Clients> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Clients objClientMaster = new Clients();
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
            allRecord = objClientMaster.GetClientListForTable(startIndex, endIndex, OrderBy, SearchKey, Status, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.Company == null ? "" : c.Company.ToString(),
                             c.Email == null ? "" : c.Email.ToString(),
                             c.City == null ? "" : c.City.ToString(),
                             c.Country == null ? "" : c.Country.ToString(),
                             c.ID.ToString(),
                         };

            return new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            };
        }

    }
}
