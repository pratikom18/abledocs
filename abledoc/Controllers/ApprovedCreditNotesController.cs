using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ApprovedCreditNotesController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, LoginAuthorized]
        public JsonResult GetApprovedCreditNotesList(JQueryDataTableParamModel param, string searchstr, string State,string param1, string startdate1, string enddate1)
        {

            int totalRecords = 0;
            List<CreditMemoInstance> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            CreditMemoInstance objCreditMemoInstance = new CreditMemoInstance();
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
            allRecord = objCreditMemoInstance.GetApprovedCreditNotesListForTable(startIndex, endIndex, OrderBy, SearchKey, out totalRecords, searchstr, State, param1, startdate1, enddate1);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            foreach (CreditMemoInstance item in allRecord)
            {
                if (item.Tax != null)
                {
                    string taxString = item.Tax;
                    string[] taxStringExplode = taxString.Split(" (");
                    string[] taxString1 = taxStringExplode[1].Split("%)");
                    decimal tax = Utility.CommonHelper.GetDBDecimal(taxString1[0]);

                    decimal preTaxAmount = item.CreditMemoAmount / (1 + (tax / 100));

                    var justTax = item.CreditMemoAmount - preTaxAmount;

                    item.Tax = tax.ToString();
                    item.PretaxAmount = preTaxAmount;
                }
              
               
            }


            var result = from c in allRecord
                         select new[] {

                            c.InvoiceID.ToString(),                                             //0
                            c.JobID.ToString(),                                                 //1
                            c.Tax == null ? "" : c.Tax.ToString(),                              //2
                            c.CreditMemoAmount.ToString(),                                      //3
                            c.Company,                                                          //4
                            c.CreditMemoAmount.ToString(),                                      //5
                            c.LastUpdated == null ? "" :c.LastUpdated.ToString("MM/dd/yyyy"),   //6
                            c.PretaxAmount==null?"":c.PretaxAmount.ToString(),                  //7
                            c.CreditMemoID.ToString(),                                          //8
                            c.CreditMemoIDQB.ToString()                                         //9
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
