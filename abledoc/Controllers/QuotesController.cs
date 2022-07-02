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
    public class QuotesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public QuotesController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        //[AuthorizedAction]
        [LoginAuthorized]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetQuotesList(JQueryDataTableParamModel param, string AlphaSearch)
        {
            var databasename = HttpContext.Session.GetString("Database");
            //var databasename1 = HttpContext.Session.GetString("Database_edit");
            //if (databasename != databasename1)
            //{
            //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
            //}

            int totalRecords = 0;
            // List<InvoiceInstance> allRecord = new List<InvoiceInstance>();
            List<Clients> allRecord = null;
            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            InvoiceInstance objClientsMaster = new InvoiceInstance();
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

            Clients objClients = new Clients();
            allRecord = objClients.GetQuotesClientList(startIndex, endIndex, OrderBy, SearchKey, out totalRecords);

            // allRecords = objClientMaster.GetCompanyList();

            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    if (allRecord != null)
            //    {
            //        filteredRecords = allRecord.Count();
            //    }
            //    else
            //    {
            //        filteredRecords = 0;
            //    }

            //}
         
            var result = from c in allRecord
                         select new[] {

                                 //c.ID.ToString(),
                                 c.JobID.ToString(),                                                    //0
                                 c.EngagementNum == null ? "":c.EngagementNum,                          //1
                                 c.FirstName == null ?"":c.FirstName,                                   //2
                                 c.LastName == null?"":c.LastName,                                      //3
                                 c.LastUpdated==null? "" :c.LastUpdated.ToString("yyyy-MM-dd hh:mm"),   //4
                                 c.QTID.ToString(),                                                     //5
                                 c.databasename.ToString() == databasename ?"true":"false",             //6
                                  c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                             //7
                         };

            var jsonResult = Json(new

            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
                //bbData = results
            });
            return jsonResult;
        }
    }
}
