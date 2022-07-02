using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace abledoc.Controllers
{
    public class PendingCreditNotesController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }
        [LoginAuthorized]
        public JsonResult GetPendingCreditNotesList(JQueryDataTableParamModel param,string State, string param1)
        {

            int totalRecords = 0;
            List<InvoiceInstance> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            InvoiceInstance objJobMaster = new InvoiceInstance();
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
            allRecord = objJobMaster.GetPendingCreditNotesListForTable(startIndex, endIndex, OrderBy, SearchKey, out totalRecords, State, param1);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             
                             c.InvoiceID.ToString(),                                            //0
                             c.JobID.ToString(),                                                //1
                             c.CreditMemoID.ToString(),                                         //2
                             c.CreditMemoAmount.ToString(),                                     //3
                             c.LastUpdated == null ? "" :c.LastUpdated.ToString("MM/dd/yyyy"),  //4   
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                          //5

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
