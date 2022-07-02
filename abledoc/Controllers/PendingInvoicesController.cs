using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using MySql.Data.MySqlClient;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class PendingInvoicesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IViewLocalizer Localizer;
        public PendingInvoicesController(IWebHostEnvironment hostEnvironment, IViewLocalizer _Localizer)
        {
            webHostEnvironment = hostEnvironment;
            Localizer = _Localizer;

        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetPendingInvoicesList(JQueryDataTableParamModel param, string State, string param1)
        {
            var databasename = HttpContext.Session.GetString("Database");
            

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
            allRecord = objJobMaster.GetPendingInvoicedListForTable(startIndex, endIndex, OrderBy, SearchKey, out totalRecords, State, param1);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}


            foreach (InvoiceInstance item in allRecord)
            {

                if (item.Tax != null)
                {
                    string taxString = item.Tax;
                    string[] taxStringExplode = taxString.Split(" (");
                    string[] taxString1 = taxStringExplode[1].Split("%)");
                    decimal tax = Utility.CommonHelper.GetDBDecimal(taxString1[0]);
                    decimal preTaxAmount = Math.Round(item.InvoiceAmount / (1 + (tax / 100)), 2);
                    item.PretaxAmount = Math.Round(preTaxAmount, 2);
                    item.justTax = Math.Round(item.InvoiceAmount - preTaxAmount, 2);
                }

            }


            var result = from c in allRecord
                         select new[] {

                             c.InvoiceID.ToString(),                                                                                             //0       
                             c.JobID.ToString(),                                                                                                 //1
                             c.EngagementNum == null ? "":c.EngagementNum,                                                                       //2
                             c.LastUpdated == null ? "" :c.LastUpdated.ToString("MM/dd/yyyy"),                                                   //3
                             c.PretaxAmount == null?"":c.PretaxAmount.ToString(),                                                                //4
                             c.justTax == null?"":c.justTax.ToString(),                                                                          //5
                             c.InvoiceAmount == null?"": c.InvoiceAmount.ToString(),                                                             //6
                             string.IsNullOrEmpty(c.QuoteAmount.ToString())?"0.00" :Utility.CommonHelper.GetDBDecimal(c.QuoteAmount).ToString(), //7
                             c.VarianceApproved== 0?"":c.VarianceApproved.ToString(),                                                            //8
                             c.currency==null ? "":c.currency,                                                                                   //9
                             c.databasename.ToString() == databasename.ToString() ? "true":"false",                                              //10
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                                                                           //11

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

        [HttpGet, LoginAuthorized]
        public PartialViewResult VarianceState(string Params)
        {
            

            string[] paramsExplode = Params.Split("|");
            string jobID = paramsExplode[0];
            string pdfH = paramsExplode[1];
            string quoteAmount = paramsExplode[2];
            string invoiceAmount = paramsExplode[3];
            string invoiceID = paramsExplode[4];
            string flag = paramsExplode[5];

            var databasename = Utility.CommonHelper.Getabledocs(flag);

            int displayQuote = 1;
            double timedValueERPRecorded = 0.00;
            double timedValueERPAltered = 0.00;
            string varianceMessage = "";


            Jobs jobs = new Jobs();
            AllTimers allTimers = new AllTimers();
            TimesheetJobs timesheet = new TimesheetJobs();
            
            jobs.databasename = databasename;
            jobs = jobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(jobID));
            if (jobs != null)
            {
                varianceMessage = jobs.VarianceComment;

                if (jobs.JobQuotedAs == "Timed")
                {
                    displayQuote = 0;
                    allTimers.databasename = databasename;
                    timedValueERPRecorded = Math.Round(Convert.ToDouble(allTimers.FetchJobERPRecordedHours(Utility.CommonHelper.GetDBInt(jobID))),2);
                    timesheet.databasename = databasename;
                    timedValueERPAltered = timesheet.FetchJobERPAlteredHours(Utility.CommonHelper.GetDBInt(jobID));

                }
            }
            int invoiceIDQB = 0;
            InvoiceInstance invoiceInstance = new InvoiceInstance();
            invoiceInstance.ID = Utility.CommonHelper.GetDBInt(invoiceID);
            invoiceInstance.databasename = databasename;
            invoiceInstance = invoiceInstance.GetInvoiceInstanceByID();
            if (invoiceInstance != null)
            {
                invoiceIDQB = Utility.CommonHelper.GetDBInt(invoiceInstance.InvoiceIDQB);
            }
            VarianceState varianceState = new VarianceState();
            varianceState.jobID = Utility.CommonHelper.GetDBInt(jobID);
            varianceState.displayQuote = displayQuote;
            varianceState.timedValueERPRecorded = timedValueERPRecorded;
            varianceState.timedValueERPAltered = timedValueERPAltered;
            varianceState.varianceAmt = Utility.CommonHelper.GetDBDouble(invoiceAmount) - Utility.CommonHelper.GetDBDouble(quoteAmount);
            varianceState.varianceMessage = varianceMessage;
            varianceState.invoiceIDQB = Utility.CommonHelper.GetDBInt(invoiceIDQB);
            varianceState.invoiceID = Utility.CommonHelper.GetDBInt(invoiceID);
            varianceState.flag = flag;


            return PartialView("~/Views/PendingInvoices/_VarianceState.cshtml", varianceState);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult AjaxState(string state, string param,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            if (state == "SaveVarianceComment")
            {
                Jobs jobs = new Jobs();
                var paramsExplode = param.ToString().Split("|").ToArray();
                jobs.ID = Convert.ToInt32(paramsExplode[0]);
                jobs.VarianceComment = paramsExplode[1];
               
                jobs.databasename = databasename;
                jobs.SaveVarianceComment();
            }
            else if (state == "VarianceSendBack")
            {
                var paramsExplode = param.ToString().Split("|").ToArray();
                InvoiceInstance invoiceInstance = new InvoiceInstance();
                invoiceInstance.Status = "TOBEINVOICED";
                invoiceInstance.InvoiceSentBack = 1;
                invoiceInstance.JobID = Convert.ToInt32(paramsExplode[0]);
                invoiceInstance.InvoiceID = Convert.ToInt32(paramsExplode[1]);
                invoiceInstance.databasename = databasename;
                invoiceInstance.InvoiceInstanceUpdate();
            }

            var jsonResult = Json(new

            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        //[HttpPost]
        //public JsonResult AjaxInvoice(string state, string param)
        //{
        //    if (state == "InvoiceEmailSend")
        //    {

        //    }
        //    else if (state == "InvoiceEmail")
        //    {

        //    }

        //    var jsonResult = Json(new

        //    {
        //        message = "success"
        //    });
        //    //jsonResult.m = int.MaxValue;
        //    return jsonResult;
        //}

        [HttpGet, LoginAuthorized]
        public async Task<IActionResult> GenerateInvoicePdf(string State, string Params, int ID, int InsertFlag, int CreditNoteFlag, string CreditMemoID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            string fullPath = string.Empty;
            GenerateInvoicePdf generateInvoicePdf = new GenerateInvoicePdf();
            generateInvoicePdf.State = State;
            //string state = "";
            string invoiceID = "";
            string jobID = "";
            int usInvoiceFlag = 0;
            int creditNoteFlag = 0;
            string creditMemoID = "0";
            string creditMemoIDQB = "0";
            string invoiceIDQB = "0";
            string companyIDQB = "";
            //string[] bccEmailList = Array;

            if (!string.IsNullOrEmpty(State))
            {
                if (State == "DownloadInvoice")
                {
                    string[] invAndJobID = Params.Split("|");
                    invoiceID = invAndJobID[0];
                    jobID = invAndJobID[1];
                    invoiceIDQB = invAndJobID[2];
                }
                else if (State == "CreditNoteEmailSend")
                {
                    string[] explodeString = Params.Split("|");
                    creditMemoID = explodeString[0];
                    jobID = explodeString[1];
                    creditMemoIDQB = explodeString[2];
                    if (!string.IsNullOrEmpty(creditMemoIDQB))
                    {
                        string[] creditMemoIDExplode = creditMemoIDQB.Split("<");
                        creditMemoIDQB = creditMemoIDExplode[0];
                    }
                }
                else if (State == "InvoiceEmailSend")
                {
                    string[] invAndJobID = Params.Split("|");
                    invoiceID = invAndJobID[0];
                    jobID = invAndJobID[1];
                    invoiceIDQB = invAndJobID[2];
                }
                else if (State == "GenerateInvoiceJobSetupPage" || State == "GenerateDownloadInvoiceJobSetupPage")
                {
                    jobID = ID.ToString();
                }
                else
                {
                    string[] invAndJobID = Params.Split("|");
                    invoiceID = invAndJobID[0];
                    jobID = invAndJobID[1];
                }
            }
            else
            {
                jobID = ID.ToString();
            }
            int insertInvoiceTracking = 1;

            if (InsertFlag == 1)
            {
                insertInvoiceTracking = 0;
                InvoiceTracking invoiceTracking = new InvoiceTracking();
                invoiceTracking.databasename = databasename;
                invoiceTracking = invoiceTracking.GetInvoiceTrackingByJobID(Utility.CommonHelper.GetDBInt(jobID));
                generateInvoicePdf.currentDate = invoiceTracking.LastUpdated;
            }
            else
            {
                generateInvoicePdf.currentDate = DateTime.Today;
            }

            Jobs jobs = new Jobs();
            
            jobs.databasename = databasename;
            jobs = jobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(jobID));
            generateInvoicePdf.EngagementNum = jobs.EngagementNum;

            generateInvoicePdf.CreditNoteFlag = CreditNoteFlag;
            generateInvoicePdf.creditMemoIDQB = creditMemoIDQB;


            string currency = "";
            string contractDate = "";
            string poText = "";

            Clients clients = new Clients();
            clients.databasename = databasename;
            clients = clients.GetClientByJobId(Utility.CommonHelper.GetDBInt(jobID));
            generateInvoicePdf.clients = clients;

            InvoiceInstance invoiceInstance = new InvoiceInstance();
            invoiceInstance.databasename = databasename;
            invoiceInstance = invoiceInstance.GetInvoiceInstanceByJobID(Utility.CommonHelper.GetDBInt(jobID));
            generateInvoicePdf.invoiceInstance = invoiceInstance;


            ClientsContacts clientsContacts = new ClientsContacts();
            clientsContacts.databasename = databasename;
            generateInvoicePdf.BillingContact = clientsContacts.GetContactByContactID(Utility.CommonHelper.GetDBInt(invoiceInstance.BillingContactID));
            generateInvoicePdf.MainContact = clientsContacts.GetContactByContactID(Utility.CommonHelper.GetDBInt(invoiceInstance.ContactID));

            string LangPack;

            var langus = new Dictionary<string, string>();
            langus.Add("EN", "English");
            langus.Add("FR", "French");
            langus.Add("DA", "Danish");
            langus.Add("DE", "German");
            langus.Add("ES", "Spanish");
            langus.Add("SV", "Swedish");
            langus.Add("IS", "Icelandic");
            langus.Add("FI", "Finnish");
            langus.Add("NN", "Norwegian");

            if (generateInvoicePdf.BillingContact != null)
            {
                if (generateInvoicePdf.BillingContact.Language != "")
                { LangPack = langus[generateInvoicePdf.BillingContact.Language]; }
                else if (generateInvoicePdf.MainContact.Language != "")
                { LangPack = langus[generateInvoicePdf.MainContact.Language]; }
                else { LangPack = langus["EN"]; }
            }
            else {
                LangPack = langus["EN"];
            }

            string currencyInfo = string.Empty;
            decimal subTotal = 0;
            string str = string.Empty;
            decimal taxRate = 0;
            string taxText = string.Empty;

            if (generateInvoicePdf.invoiceTmpList != null)
            {

                foreach (var row in generateInvoicePdf.invoiceTmpList)
                {
                    if (generateInvoicePdf.CreditNoteFlag == 1 && row.Quantity == 0)
                    {
                        continue;
                    }

                    currencyInfo = row.Tax;
                    taxText = row.Tax;
                    subTotal = subTotal + abledoc.Utility.CommonHelper.GetDBDecimal((row.Price * row.Quantity));
                }
            }
            var modelJobPopulate = new JobQuoteAutopopulate().GetJobPopulateList();
            if (modelJobPopulate.Count > 0)
            {

                foreach (var row in new JobQuoteAutopopulate().GetJobPopulateList())
                {

                    if (taxText == row.Information)
                    {
                        taxRate = @row.tax;
                    }
                }
            }
            decimal taxTotal = abledoc.Utility.CommonHelper.GetDBDecimal((abledoc.Utility.CommonHelper.GetDBDecimal(taxRate) * subTotal) / 100);
            decimal totalAmount = taxTotal + subTotal;

            if (creditNoteFlag == 1)
            {

                if (insertInvoiceTracking == 1)
                {
                    InvoiceTmp creditMemoTmp = new InvoiceTmp();
                    creditMemoTmp.JobID = Utility.CommonHelper.GetDBInt(jobID);
                    creditMemoTmp.databasename = databasename;
                    generateInvoicePdf.invoiceTmpList = creditMemoTmp.GetCreditMemoTmpLockedByJobID();
                }
                else
                {
                    InvoiceTmp creditMemoTmp = new InvoiceTmp();
                    creditMemoTmp.JobID = Utility.CommonHelper.GetDBInt(jobID);
                    creditMemoTmp.CreditMemoID = Utility.CommonHelper.GetDBInt(creditMemoID);
                    creditMemoTmp.databasename = databasename;
                    generateInvoicePdf.invoiceTmpList = creditMemoTmp.GetCreditMemoTmpByJobIDCreditMemoID();
                }
            }
            else
            {
                InvoiceTmp invoiceTmp = new InvoiceTmp();
                invoiceTmp.JobID = Utility.CommonHelper.GetDBInt(jobID);
                invoiceTmp.databasename = databasename;
                generateInvoicePdf.invoiceTmpList = invoiceTmp.GetInvoiceTmpByJobID();
            }
            string PDFName = string.Empty;

            if (insertInvoiceTracking == 1)
            {
                if (creditNoteFlag == 1)
                {
                    int creditNoteID = 0;
                    CreditMemoInstance creditMemoInstance = new CreditMemoInstance();
                    creditMemoInstance.JobID = Utility.CommonHelper.GetDBInt(jobID);
                    creditMemoInstance.databasename = databasename;
                    creditMemoInstance = creditMemoInstance.GetCreditMemoInstanceByJobID();
                    creditNoteID = creditMemoInstance.ID;
                    string lastID = string.Empty;
                    try
                    {
                        CreditMemoTracking creditMemoTracking = new CreditMemoTracking();
                        creditMemoTracking.JobID = Utility.CommonHelper.GetDBInt(jobID);
                        creditMemoTracking.CreditMemoAmount = totalAmount;
                        creditMemoTracking.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                        creditMemoTracking.CreditMemoID = creditNoteID;
                        creditMemoTracking.databasename = databasename;
                        lastID = creditMemoTracking.Insert();
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        creditMemoInstance = new CreditMemoInstance();
                        creditMemoInstance.JobID = Utility.CommonHelper.GetDBInt(jobID);
                        creditMemoInstance.databasename = databasename;
                        creditMemoInstance.UpdateByJobID();
                    }
                    catch (Exception ex)
                    {

                    }
                    string path = Path.Combine(this.webHostEnvironment.WebRootPath, "QuoteInvoices");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string path1 = Path.Combine(path, jobID.ToString());
                    string physicalPathJobFolder = path1;
                    string filePath1 = "";
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                        filePath1 = "/QuoteInvoices/" + jobID;
                    }
                    PDFName = lastID + "_CreditMemo.pdf";
                    string filename = Path.Combine(filePath1, lastID + "_CreditMemo.pdf");
                    fullPath = Path.Combine(physicalPathJobFolder, lastID + "_CreditMemo.pdf");

                    CreditMemoTracking creditMemoTracking1 = new CreditMemoTracking();
                    creditMemoTracking1.ID = Utility.CommonHelper.GetDBInt(lastID);
                    creditMemoTracking1.PhysicalLocation = filename;
                    creditMemoTracking1.databasename = databasename;
                    creditMemoTracking1.UpdatePhysicalLocationByID();

                    ViewAsPdf pdf = new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf)
                    {
                        FileName = PDFName,
                        //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    };
                    byte[] pdfData = await pdf.BuildFile(ControllerContext);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(pdfData, 0, pdfData.Length);
                    }
                }
                else
                {
                    string retvalID = string.Empty;
                    try
                    {
                        InvoiceTracking invoiceTracking = new InvoiceTracking();
                        invoiceTracking.JobID = Utility.CommonHelper.GetDBInt(jobID);
                        invoiceTracking.InvoiceAmount = Convert.ToDecimal(totalAmount.ToString("#,##0.00"));
                        invoiceTracking.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                        invoiceTracking.OperationType = "InvoiceGenerated";
                        invoiceTracking.databasename = databasename;
                        retvalID = invoiceTracking.InsertInvoiceTracking();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        InvoiceInstance invoiceInstance1 = new InvoiceInstance();
                        invoiceInstance1.Status = "INVOICEPENDING";
                        invoiceInstance1.InvoiceSentBack = 0;
                        invoiceInstance1.InvoiceID = generateInvoicePdf.invoiceInstance.InvoiceID;
                        invoiceInstance1.databasename = databasename;
                        retvalID = invoiceInstance1.UpdateStatusByInvoiceID();
                    }
                    catch (Exception ex)
                    {

                    }
                    string path = Path.Combine(this.webHostEnvironment.WebRootPath, "QuoteInvoices");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string path1 = Path.Combine(path, jobID.ToString());
                    string physicalPathJobFolder = path1;
                    string filePath1 = "";
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                        filePath1 = "/QuoteInvoices/" + jobID;
                    }
                    PDFName = retvalID + "_Invoice.pdf";
                    string filename = Path.Combine(filePath1, retvalID + "_Invoice.pdf");
                    fullPath = Path.Combine(physicalPathJobFolder, retvalID + "_Invoice.pdf");

                    InvoiceTracking invoiceTracking1 = new InvoiceTracking();
                    invoiceTracking1.ID = Utility.CommonHelper.GetDBInt(retvalID);
                    invoiceTracking1.PhysicalLocation = filename;
                    invoiceTracking1.databasename = databasename;
                    invoiceTracking1.UpdatePhysicalLocationByID();

                    ViewAsPdf pdf = new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf)
                    {
                        FileName = PDFName,
                        //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                    };
                    byte[] pdfData = await pdf.BuildFile(ControllerContext);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(pdfData, 0, pdfData.Length);
                    }
                }
            }

            if (State == "CreditNoteEmailSend")
            {
                CreditMemoInstance creditMemoInstance = new CreditMemoInstance();
                creditMemoInstance.ID = Utility.CommonHelper.GetDBInt(CreditMemoID);
                creditMemoInstance.databasename = databasename;
                creditMemoInstance = creditMemoInstance.GetCreditMemoInstanceByID();

                string bcName = "";
                string bcEmail = "";
                string rcName = "";
                string rcEmail = "";
                string sTo = "";
                ClientsContacts clientsContacts1 = new ClientsContacts();
                clientsContacts1.databasename = databasename;
                clientsContacts1 = clientsContacts1.GetContactByContactID(Utility.CommonHelper.GetDBInt(creditMemoInstance.BillingContactID));
                sTo = clientsContacts1.Email;
                //bcEmail = clientsContacts1.Email;
                //rcEmail = clientsContacts1.Email;
                //bcName = clientsContacts1.FirstName + " " + clientsContacts1.LastName;
                //rcName = clientsContacts1.FirstName + " " + clientsContacts1.LastName;

                string BillingEmail = Localizer["BillingEmail"].Value;
                string CompanyName = Localizer["CompanyName"].Value;

                ViewAsPdf pdf = new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf)
                {
                    FileName = PDFName,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = await pdf.BuildFile(ControllerContext);

                Attachment attachment = Utility.EmailHelper.GetAttachFile(pdfData, PDFName);
                string message1 = "<br><br>Please find your Credit Note #" + creditMemoIDQB + " attached. ";
                Utility.EmailHelper.Email(clientsContacts1.Country,sTo, rcEmail, bcEmail, "Credit Note #" + creditMemoIDQB, message1, attachment);

                //return new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf);
                var jsonResult = Json(new

                {
                   messge="Successfully send email."
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;

            }
            if (State == "CreditNoteEmail")
            {
                InvoiceInstance invoiceInstance1 = new InvoiceInstance();
                invoiceInstance1.databasename = databasename;
                invoiceInstance1 = invoiceInstance1.GetInvoiceInstanceCompanyName();
            }
            if (State == "InvoiceEmailSend")
            {
                InvoiceInstance invoiceInstance1 = new InvoiceInstance();
                invoiceInstance1.databasename = databasename;
                invoiceInstance1 = invoiceInstance1.GetInvoiceInstanceByInvoiceID(Utility.CommonHelper.GetDBInt(invoiceID));

                string bcName = "";
                string bcEmail = "";
                string rcName = "";
                string rcEmail = "";
                string sTo = "";

                ClientsContacts clientsContacts1 = new ClientsContacts();
                clientsContacts1.databasename = databasename;
                clientsContacts1 = clientsContacts1.GetContactByContactID(Utility.CommonHelper.GetDBInt(invoiceInstance1.BillingContactID));
                sTo = clientsContacts1.Email;
                //bcEmail = clientsContacts1.Email;
                //rcEmail = clientsContacts1.Email;
               // bcName = clientsContacts1.FirstName + " " + clientsContacts1.LastName;
               // rcName = clientsContacts1.FirstName + " " + clientsContacts1.LastName;

                //string BillingEmail = Localizer["BillingEmail"].ToString();
                //string CompanyName = Localizer["CompanyName"].ToString();

                ViewAsPdf pdf = new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf)
                {
                    FileName = PDFName,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = await pdf.BuildFile(ControllerContext);

                Attachment attachment = Utility.EmailHelper.GetAttachFile(pdfData, PDFName);
                string message1 = "<br><br>Attached is our invoice # " + invoiceIDQB + " for accessibility services.";
                Utility.EmailHelper.Email(clientsContacts1.Country,sTo, rcEmail, bcEmail, "Invoice #" + invoiceIDQB, message1, attachment);

                //return new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf);
                var jsonResult = Json(new

                {
                    messge = "Successfully send email."
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }

            if (State == "InvoiceEmail")
            {
                string result = "";
                int proceedToQB = 0;
                // Check if invoice has already been posted to quick books for this engagement before
                InvoiceInstance invoiceInstance1 = new InvoiceInstance();
                invoiceInstance1.InvoiceID = Utility.CommonHelper.GetDBInt(invoiceID);
                invoiceInstance1.JobID = Utility.CommonHelper.GetDBInt(jobID);
                invoiceInstance1.databasename = databasename;
                invoiceInstance1 = invoiceInstance1.GetInvoiceInstanceByInvoiceIDJobID();
                if (invoiceInstance1 != null)
                {
                    if (invoiceInstance1.InvoiceIDQB == 0)
                    {
                        proceedToQB = 1;
                    }
                }


                if (proceedToQB == 1)
                {
                    // Create Invoice
                    //$qbInvObject = new QuickBooks();
                    //$qbInvAuthObj = new QuickBooks_IPP_OAuth($consumerKey,$consumerSecret);
                    //$qbInvAuthObj->signature("HMAC-SHA1");
                    //$invoiceCreateURLQuery = $_SESSION["QBURL"]."/v3/company/".$_SESSION["RealmID"]."/invoice?minorversion=4";
                    //$result = $qbInvAuthObj->sign("POST",$invoiceCreateURLQuery,$accessToken,$accessTokenSecret);
                    //$authorizeHeader = $result[3];
                    //$createInvResponse = $qbInvObject->CreateInvoice($invoiceCreateURLQuery,$authorizeHeader,$companyIDQB,$invoiceRowFiles,$invoiceSubTotal,$invoiceTaxPercent,$invoiceTax,$invoiceTotal);

                    //$idVal = "";
                    //$dom = new DOMDocument;
                    //$dom->loadXML($createInvResponse);
                    //$ids = $dom->getElementsByTagName('DocNumber');
                    //            foreach ($ids as $id) {
                    //    $idVal = $id->nodeValue;
                    //            }
                    //echo $idVal;
                    //$invIDQB = $idVal;


                    //$my_file = 'invoice_data.txt';
                    //$handle = fopen($my_file, 'a') or die('Cannot open file:  '.$my_file);
                    //$current.= "Invoice Create URL Query: ". $invoiceCreateURLQuery. " \n";
                    //$current.= "Authorize Header: ". $authorizeHeader. " \n";
                    //$current.= "Company ID QB: ". $companyIDQB. " \n";



                    //            foreach ($invoiceRowFiles as $rowInv)
                    //{
                    //    $current.= "Invoice Row Files: ". $rowInv. " \n";
                    //            }
                    //$current.= "Invoice Sub Total: ". $invoiceSubTotal. " \n";
                    //$current.= "Invoice Tax Percent: ". $invoiceTaxPercent. " \n";
                    //$current.= "Invoice Tax: ". $invoiceTax. " \n";
                    //$current.= "Invoice Total: ". $invoiceTotal. " \n";
                    //$current.= "Invoice ID QB: ". $invIDQB. " \n";

                    //$current.= "Response: ". $createInvResponse. " \n";


                    //$current.= " \n";
                    //            fwrite($handle, $current);
                    //            fclose($handle);

                    int invIDQB = 0;
                    //if (invIDQB != 0)
                    //{
                        // Update the invoice_instance table
                        try
                        {
                            var format = "yyyy-MM-dd H:mm:ss";
                            var stringDate = DateTime.Now.ToString(format);

                            invoiceInstance1 = new InvoiceInstance();
                            invoiceInstance1.InvoiceIDQB = invIDQB;
                            invoiceInstance1.InvoiceSentBack = 0;
                            invoiceInstance1.Approved = 1;
                            invoiceInstance1.ApprovedUID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                            invoiceInstance1.ApprovedTimestamp = DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture);
                            invoiceInstance1.VarianceApproved = 1;
                            invoiceInstance1.VarianceApprovedUID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                            invoiceInstance1.VarianceApprovedTimestamp = DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture);
                            invoiceInstance1.Status = "CLOSED";
                            invoiceInstance1.InvoiceID = Utility.CommonHelper.GetDBInt(invoiceID);
                            invoiceInstance1.databasename = databasename;
                            invoiceInstance1.InvoiceInstanceUpdateByInvoiceID();

                        }
                        catch (Exception e)
                        {
                            //echo "Invoice ID QB: (". $invIDQB. ") ";
                            //echo "Invoice ID: (".$invoiceID. ") ";
                            //exit;
                        }

                        // Update the invoice_tmp table
                        try
                        {
                            InvoiceTmp invoiceTmp = new InvoiceTmp();
                            invoiceTmp.InvoiceIDQB = invIDQB;
                            invoiceTmp.InvoiceID = Utility.CommonHelper.GetDBInt(invoiceID);
                            invoiceTmp.databasename = databasename;
                            invoiceTmp.UpdateInvoiceIDQBByInvoiceID();

                        }
                        catch (Exception e)
                        {
                            //echo $e->getMessage();
                            //exit;
                        }

                        //string jobStatusFlag = "CheckToClosed";
                        //$responseJobStatusUpdate = $erpObj->JobStatusUpdate($jobID, $jobStatusFlag);
                    //}
                    result = invoiceID + "|" + jobID + "|" + invIDQB;

                }
                else
                {
                    result = "0|0|0";
                }
                var jsonResult = Json(new

                {
                    data = result
                });
                return jsonResult;
            }
            else if (insertInvoiceTracking == 0 && State != "DownloadInvoice")
            {
                return new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf);
            }

            else if (State == "DownloadInvoice")
            {
                ViewAsPdf pdf = new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf)
                {
                    FileName = PDFName,
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = await pdf.BuildFile(ControllerContext);
                //echo $invoiceID ."|" . $jobID ."|". $invoiceIDQB;
                if (invoiceIDQB != "0" && invoiceIDQB != "")
                {
                    return File(pdfData, "application/pdf", invoiceIDQB + "_QB_Invoice.pdf");
                }
                else
                {
                    return File(pdfData, "application/pdf", jobID + "_JobID_Invoice.pdf");
                }
            }
            else
            {
                return new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf);
            }
            //if (downloadInvoice == 1)
            //{
            //    return File(new FileStream(fullpath, FileMode.Open), "application/pdf", filename);
            //}
            //else
            //{
            //    return new ViewAsPdf("GenerateInvoicePdf", generateInvoicePdf);
            //}

            // return View(generateInvoicePdf);
        }

        [HttpGet, LoginAuthorized]
        public IActionResult QuoteInvoiceShowFile(int JobID, string Type,string flag)
        {
            ViewBag.Type = Type;
            ViewBag.ID = JobID;
            ViewBag.flag = flag;
            return PartialView("~/Views/Jobs/_IFramePdf.cshtml");
        }
    }
}
