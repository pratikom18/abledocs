using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class SearchController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index(string search)
        {
            ViewBag.Search = search;
            
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SearchList(JQueryDataTableParamModel param, string searchstr,string filter)
        {
            var databasename = HttpContext.Session.GetString("Database");
            var databasename1 = HttpContext.Session.GetString("Database_edit");
            if (databasename != databasename1)
            {
                HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
            }

            int totalRecords = 0;
            List<Search> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Search objSearch = new Search();
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
            allRecord = objSearch.GetSearchListForTable(startIndex, endIndex, OrderBy, SearchKey,searchstr , filter, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

          

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),                                           //0
                             c.JobID.ToString(),                                        //1
                             c.Client == null ? "" : c.Client,                          //2
                             c.Contact == null? "":c.Contact,                           //3
                             c.EngagementNum == null ? "" :c.EngagementNum,             //4
                             c.EntityType == null ? "" : c.EntityType,                  //5
                             c.Status == null ? "" : Models.Utility.getStatus(c.Status),//6
                             c.FileName == null ? "":c.FileName,                        //7
                             c.databasename.ToString(),                                 //8
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                  //9
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

        //[HttpGet]
        //public JsonResult SearchList(string searchstr) 
        //{
        //    Search objSearch = new Search();
        //    var result = objSearch.GetSearchList(searchstr);
        //    var jsonResult = Json(new

        //    {
        //        data = result
        //    });
        //    //jsonResult.m = int.MaxValue;
        //    return jsonResult;
        //}
    }
}
