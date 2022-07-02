using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ApprovedInvoicesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public ApprovedInvoicesController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, LoginAuthorized]
        public JsonResult GetApprovedInvoicesList(JQueryDataTableParamModel param, string searchstr, string State, string startdate1, string enddate1)
        {

            int totalRecords = 0;
            List<InvoiceInstance> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            InvoiceInstance objInvoiceInstance = new InvoiceInstance();
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
            allRecord = objInvoiceInstance.GetApprovedInvoicesListForTable(startIndex, endIndex, OrderBy, SearchKey, out totalRecords, searchstr, startdate1, enddate1);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            foreach (InvoiceInstance item in allRecord)
            {
                QuoteTracking quoteTracking = new QuoteTracking();
                decimal rowQuoteAmount = Utility.CommonHelper.GetDBDecimal(0.00);
                quoteTracking.QuoteAmount = Utility.CommonHelper.GetDBDecimal(0.00);
                if (string.IsNullOrEmpty(item.QuoteAmount.ToString()))
                {
                    rowQuoteAmount = Utility.CommonHelper.GetDBDecimal(0.00);
                }
                else
                {
                    quoteTracking.QuoteAmount = item.QuoteAmount;
                }

                if (item.Tax != null)
                {
                    string taxString = item.Tax;
                    string[] taxStringExplode = taxString.Split(" (");
                    string[] taxString1 = taxStringExplode[1].Split("%)");
                    decimal tax = Utility.CommonHelper.GetDBDecimal(taxString1[0]);

                    decimal preTaxAmount = item.InvoiceAmount / (1 + (tax / 100));

                    var justTax = item.InvoiceAmount - preTaxAmount;

                    item.Tax = tax.ToString();
                    item.PretaxAmount = preTaxAmount;

                }

            }


            var result = from c in allRecord
                         select new[] {

                            c.InvoiceID.ToString(),                                             //0
                            c.JobID.ToString(),                                                 //1
                            c.Tax == null ? "" : c.Tax.ToString(),                              //2
                            c.InvoiceAmount.ToString(),                                         //3
                            c.CompanyName,                                                      //4
                            c.InvoiceAmount.ToString(),                                         //5
                            c.EngagementNum == null ? "":c.EngagementNum,                       //6
                            c.LastUpdated == null ? "" :c.LastUpdated.ToString("MM/dd/yyyy"),   //7
                            c.PretaxAmount==null?"":c.PretaxAmount.ToString()                   //8

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
