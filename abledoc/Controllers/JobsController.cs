using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO.Compression;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using Microsoft.Extensions.Configuration;
using System.Data.OleDb;
using MySqlConnector;

namespace abledoc.Controllers
{
    public class JobsController : Controller
    {

        //private readonly IHostingEnvironment _hostingEnvironment;

        //public JobsController(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IViewLocalizer Localizer;
        private IConfiguration Configuration;
        public JobsController(IWebHostEnvironment hostEnvironment, IViewLocalizer _Localizer, IConfiguration _configuration)
        {
            webHostEnvironment = hostEnvironment;
            Localizer = _Localizer;
            Configuration = _configuration;
        }

        [AuthorizedAction]
        public IActionResult Index(string currenttab = "")
        {
            Jobs objJobMaster = new Jobs();
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "jobs").FirstOrDefault();
            string pagefound = "No";
            string isAdd = "No";
            string isEdit = "No";
            string isDelete = "No";

            if (objAssignedMenu != null)
            {
                if (objAssignedMenu.isview)
                {
                    pagefound = "yes";

                }
                if (objAssignedMenu.isadd)
                {
                    isAdd = "yes";

                }
                if (objAssignedMenu.isupdate)
                {
                    isEdit = "yes";

                }
                if (objAssignedMenu.isdelete)
                {
                    isDelete = "yes";

                }
            }
            ViewBag.isAdd = isAdd;
            ViewBag.isEdit = isEdit;
            ViewBag.isDelete = isDelete;
            if (pagefound == "No")
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                var databasename = HttpContext.Session.GetString("Database");
                objJobMaster.databasename = databasename;
                ViewBag.ClientList = objJobMaster.GetClientsCountryList();
                ViewBag.StatusList = objJobMaster.GetStatusList();
                ViewBag.Status = currenttab;
                return View();
            }
        }
        [HttpPost, LoginAuthorized]
        public JsonResult JobList(JQueryDataTableParamModel param, string Status, int TodayTask)
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
            allRecord = objJobMaster.GetJobListForTable(startIndex, endIndex, OrderBy, SearchKey, Status, out totalRecords, TodayTask);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                var totalFilterRecord = objJobMaster.GetDataTableFiltered(SearchKey, Status, TodayTask);
                filteredRecords = Convert.ToInt32(totalFilterRecord);//allRecord.Count();
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
                             c.FinalPercent == 0 ? c.P1.ToString()+"/"+c.P2.ToString()+"/"+c.P4.ToString()+"/"+c.P5.ToString()+"<br/>"+c.FinalFiles.ToString()+"/"+c.Files.ToString()+"(0%)": c.P1.ToString()+"/"+c.P2.ToString()+"/"+c.P4.ToString()+"/"+c.P5.ToString()+"<br/>"+c.FinalFiles.ToString()+"/"+c.Files.ToString()+ "("+c.FinalPercent.ToString("#.##")+"%)",
                             c.ClientID.ToString(),
                             c.FinalPercent.ToString(),
                             c.Deadline == null ? "" : c.Deadline.ToString(),
                             c.Year == null ? "" : c.Year.ToString(),
                             c.CountryCode == null ? "" :c.CountryCode.ToString(),
                             c.Notes == null ? "" :c.Notes.ToString(),
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",  

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
        public IActionResult AddJobforClient(string ClientCode)
        {
            var databasename = HttpContext.Session.GetString("Database");

            Jobs model = new Jobs();
            Clients modelClients = new Clients();
            modelClients.Code = ClientCode;
            modelClients.databasename = databasename;
            modelClients = modelClients.GetClientByCode();

            model.databasename = databasename;
            model.ClientID = modelClients.ID;
            string id = model.CreateJob();
            return RedirectToAction("JobsInitial", new { id = Convert.ToInt32(id) });

        }

        [HttpGet]
        [LoginAuthorized]
        public IActionResult JobsInitial(int id, string flag = "", int save = 0)
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }



            ViewBag.databasename = databasename;
            ViewBag.flag = flag;

            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "jobs").FirstOrDefault();


            string isEdit = "No";

            if (objAssignedMenu != null)
            {

                if (objAssignedMenu.isupdate)
                {
                    isEdit = "yes";

                }

            }

            ViewBag.isEdit = isEdit;

            if (isEdit == "No")
            {
                return RedirectToAction("NoAccess", "Error");
            }
            else
            {
                string code = HttpContext.Session.GetString("Country");
                ViewBag.loginusercountry = code;

                List<UserRoles> UserRolesList = new List<UserRoles>();
                UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
                Dictionary<string, string> StatusList = new Dictionary<string, string>();

                ViewBag.UserRolesList = UserRolesList;


                var secBillingContactF = "";
                var justContactF = "";
                var deliveryF = "";
                var billingContactEmail = "";
                var billingContactName = "";
                var justContactEmail = "";
                var justContactName = "";
                var billingContactID = "";
                var deliveryContactID2 = "";
                var deliveryEmail = "";
                var deliveryName = "";
                var deliveryLang = "";



                Jobs objJobMaster = new Jobs();

                objJobMaster.databasename = databasename;
                objJobMaster = objJobMaster.GetJobsById(Convert.ToInt32(id));

                JobsFiles objJobsFiles = new JobsFiles();
                objJobsFiles.databasename = databasename;
                //objJobsFiles = objJobsFiles.GetJobFilesByID(id);
                var getFilesList = objJobsFiles.GetFilesListByJobID(id);

                JobsDeliveryContact objJobDeliveryMaster = new JobsDeliveryContact();
                objJobDeliveryMaster.JobID = id;
                objJobDeliveryMaster.databasename = databasename;
                ViewBag.JobDeliveryContactData = objJobDeliveryMaster.GetJobDeliveryContactList();

                JobsCustomFields objJobsCustomFields = new JobsCustomFields();
                objJobsCustomFields.JobID = id;
                objJobsCustomFields.databasename = databasename;
                ViewBag.JobsCustomFields = objJobsCustomFields.GetJobsCustomFieldById();

                List<JobsFiles> jobsfilesList = new List<JobsFiles>();

                objJobsFiles.databasename = databasename;
                jobsfilesList = objJobsFiles.GetJobFileListIDByJobID(Utility.CommonHelper.GetDBInt(id));

                ClientsContacts modelClientContact = new ClientsContacts();

                modelClientContact.databasename = databasename;
                var clientContactDetails = modelClientContact.GetContactById(Utility.CommonHelper.GetDBInt(objJobMaster.ContactID));
                var BillingContactDetails = modelClientContact.GetContactById(Utility.CommonHelper.GetDBInt(objJobMaster.BillingContactID));

                Clients modelClient = new Clients();
                modelClient.databasename = databasename;
                var clientData = modelClient.GetClientById(objJobMaster.ClientID);
                objJobMaster.loginusercountry = code;
                if (clientData.Country == code)
                {
                    ViewBag.SameCountryUser = true;
                }
                else
                {
                    ViewBag.SameCountryUser = false;
                }

                if (objJobMaster.BillingContactID == "" || objJobMaster.BillingContactID == "0")
                {
                    billingContactID = clientData.BillingContactID;
                }
                else
                {
                    billingContactID = objJobMaster.BillingContactID;
                }

                if (objJobMaster.SecDeliveryContactID == "" || objJobMaster.SecDeliveryContactID == "0")
                {
                    deliveryContactID2 = clientData.SecDeliveryContactID;
                }
                else
                {
                    deliveryContactID2 = objJobMaster.SecDeliveryContactID;
                }


                if (BillingContactDetails != null)
                {
                    billingContactEmail = BillingContactDetails.Email;
                    billingContactName = BillingContactDetails.FirstName + " " + BillingContactDetails.LastName;
                    secBillingContactF = BillingContactDetails.FirstName;
                }
                if (clientContactDetails != null)
                {
                    deliveryLang = clientContactDetails.Language;
                    justContactEmail = clientContactDetails.Email;
                    justContactName = clientContactDetails.FirstName + " " + clientContactDetails.LastName;
                    justContactF = clientContactDetails.FirstName;

                    ViewBag.selectedFirstName = clientContactDetails.FirstName;

                    EmailTemplate modelTemplate = new EmailTemplate();
                    objJobMaster.DeliveryEmailTemplate = modelTemplate.GetTemplateByLang(clientContactDetails.Language, "Files_List_To_Be_Delivered");


                }

                if (deliveryContactID2 != "" && deliveryContactID2 != "0")
                {
                    modelClientContact.databasename = databasename;//HttpContext.Session.GetString("Database");
                    var deliverContactDetails = modelClientContact.GetContactById(Utility.CommonHelper.GetDBInt(deliveryContactID2));
                    if (deliverContactDetails != null)
                    {
                        deliveryEmail = deliverContactDetails.Email;
                        deliveryName = deliverContactDetails.FirstName + " " + deliverContactDetails.LastName;
                        deliveryF = deliverContactDetails.FirstName;
                    }
                }

                ViewBag.deliveryLang = deliveryLang;
                ViewBag.justContactEmail = justContactEmail;
                ViewBag.justContactName = justContactName;
                ViewBag.justContactF = justContactF;
                ViewBag.secBillingContactF = secBillingContactF;
                ViewBag.billingContactName = billingContactName;
                ViewBag.billingContactEmail = billingContactEmail;
                ViewBag.billingContactID = billingContactID;
                ViewBag.deliveryContactID2 = deliveryContactID2;
                ViewBag.deliveryEmail = deliveryEmail;
                ViewBag.deliveryName = deliveryName;
                ViewBag.deliveryF = deliveryF;
                objJobMaster.PORequired = clientData.PORequired;

                if (jobsfilesList != null)
                {
                    //int[] arrays = jobsfilesList.Select(x => x.ID).ToArray();

                    //List<JobsFiles> jobfilesTreeList = new List<JobsFiles>();
                    //jobfilesTreeList = objJobsFiles.GetJobfilesFileTree(arrays);

                    //JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
                    //List<JobsFilesVersions> JobsFilesVersionsList = objJobsFilesVersions.GetUserListByJobID(id);
                    //List<JobsFilesVersions> getFileVersionOtherStateList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "OTHER");
                    //List<JobsFilesVersions> getFileVersionReferenceList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "REFERENCE");

                    //List<JobsFilesVersions> jobfileversionTreeList = new List<JobsFilesVersions>();
                    //jobfileversionTreeList = objJobsFilesVersions.GetJobfilesFileTree(arrays);

                    //objJobMaster.JobFileTree = jobfilesTreeList;
                    //objJobMaster.JobsFilesVersionsTree = jobfileversionTreeList;

                    //objJobMaster.OtherFilesList = getFileVersionOtherStateList;

                    //objJobMaster.ReferenceFilesList = getFileVersionReferenceList;

                    objJobMaster.FilesList = getFilesList;
                }
                ViewBag.Save = save;
                objJobMaster.databasename = databasename;
                return View(objJobMaster);
            }
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult FileTree(int id, int CurrentPage, int inEndIndex, int checkbox = 0,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }


            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");


            ViewBag.UserRolesList = UserRolesList;

            Models.File objFile = new Models.File();
            JobsFiles objJobsFiles = new JobsFiles();
            List<JobsFiles> jobsfilesList = new List<JobsFiles>();
            objJobsFiles.databasename = databasename;
            jobsfilesList = objJobsFiles.GetJobFileListIDByJobID(Utility.CommonHelper.GetDBInt(id));

            if (jobsfilesList != null)
            {
                int[] arrays = jobsfilesList.Select(x => x.ID).ToArray();

                int inStartIndex = (CurrentPage - 1) * inEndIndex;
                int TotalRecord = 0;

                List<JobsFiles> jobfilesTreeList = new List<JobsFiles>();

                objJobsFiles.databasename = databasename;
                jobfilesTreeList = objJobsFiles.GetJobfilesFileTreePaging(arrays, inStartIndex, inEndIndex, out TotalRecord);

                JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
                objJobsFilesVersions.databasename = databasename;
                List<JobsFilesVersions> JobsFilesVersionsList = objJobsFilesVersions.GetUserListByJobID(id);
                List<JobsFilesVersions> getFileVersionOtherStateList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "OTHER");
                List<JobsFilesVersions> getFileVersionReferenceList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "REFERENCE");

                List<JobsFilesVersions> jobfileversionTreeList = new List<JobsFilesVersions>();
                jobfileversionTreeList = objJobsFilesVersions.GetJobfilesFileTree(arrays);
                objFile.jobs = new Jobs();
                objFile.jobs.ID = id;
                objFile.JobFileTree = jobfilesTreeList;
                objFile.JobsFilesVersionsTree = jobfileversionTreeList;
                objFile.CurrentPage = CurrentPage;
                objFile.PageCount = Convert.ToInt32(Math.Ceiling(TotalRecord / (double)inEndIndex));
            }
            ViewBag.checkbox = checkbox;
            return PartialView("_FileTree", objFile);
        }
        [HttpGet, LoginAuthorized]
        public PartialViewResult FileTreeDataTable(int id)
        {

            ViewBag.JobID = id;
            return PartialView("_FileTree");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult FileTreeDataTableDeliverd(int id)
        {

            ViewBag.JobID = id;
            return PartialView("_FileTreeToBeDeliverd");
        }

        [HttpPost, LoginAuthorized]
        public JsonResult FileTreeFirstLevel(JQueryDataTableParamModel param, int ID,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            JobsFiles objJobsFiles = new JobsFiles();
            List<JobsFiles> jobsfilesList = new List<JobsFiles>();
            objJobsFiles.databasename = databasename;
            jobsfilesList = objJobsFiles.GetJobFileListIDByJobID(Utility.CommonHelper.GetDBInt(ID));
            List<FileTree> allRecord = new List<FileTree>();
            if (jobsfilesList != null)
            {
                int[] arrays = jobsfilesList.Select(x => x.ID).ToArray();

                int totalRecords = 0;


                string SearchKey = "";
                string OrderBy = "";
                int startIndex = param.iDisplayStart;
                int endIndex = param.iDisplayLength;

                FileTree objFileTree = new FileTree();
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
                objFileTree.databasename = databasename;
                allRecord = objFileTree.GetJobListForFirstLevel(startIndex, endIndex, OrderBy, SearchKey, arrays, out totalRecords);
                int filteredRecords = totalRecords;
                if (SearchKey != "")
                {
                    filteredRecords = allRecord.Count();
                }

                var result = from c in allRecord
                             select new[] {
                             c.ID.ToString(),
                             c.FileID.ToString(),
                             c.Filename == null?string.Empty:c.Filename,
                             c.Deadline == null?"":c.Deadline.ToString(),
                             c.State == null?string.Empty:c.State,
                             c.Physical_Path == null?string.Empty:c.Physical_Path,
                             c.QCType == null?"0":c.QCType.ToString(),



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
            else
            {
                var jsonResult = Json(new

                {
                    sEcho = param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = allRecord
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }




        }

        [HttpPost, LoginAuthorized]
        public JsonResult FileTreeSecondLevel(JQueryDataTableParamModel param, int ID, bool ReferenceNotAllow = false,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            int totalRecords = 0;
            List<FileTree> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            FileTree objFileTree = new FileTree();
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

            objFileTree.databasename = databasename;
            allRecord = objFileTree.GetJobListForSecondLevel(startIndex, endIndex, OrderBy, SearchKey, ID, out totalRecords, ReferenceNotAllow);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),
                             c.FileID.ToString(),
                             c.Filename == null?string.Empty:c.Filename,
                             c.State == null?string.Empty:c.State,
                             c.Physical_Path == null?string.Empty:c.Physical_Path,
                             c.QCType == null?"0":c.QCType.ToString()

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

        [HttpPost, LoginAuthorized]
        public JsonResult FileTreeThiredLevel(JQueryDataTableParamModel param, int ID, string State, int QcType,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            int totalRecords = 0;
            List<FileTree> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            FileTree objFileTree = new FileTree();
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
            objFileTree.databasename = databasename;
            allRecord = objFileTree.GetJobListForThiredLevel(startIndex, endIndex, OrderBy, SearchKey, ID, State, QcType, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),
                             c.FileID.ToString(),
                             c.Filename == null?string.Empty:c.Filename,
                             c.State == null?string.Empty:c.State,
                             c.Physical_Path == null?string.Empty:c.Physical_Path,
                             c.QCType == null?"0":c.QCType.ToString()

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


        [HttpPost, LoginAuthorized]
        public JsonResult FileTreeComplianceReportFirstLevel(JQueryDataTableParamModel param, int ID,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            int totalRecords = 0;
            List<FileTree> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            FileTree objFileTree = new FileTree();
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
            objFileTree.databasename = databasename;
            allRecord = objFileTree.GetJobListForComplianceReportFirstLevel(startIndex, endIndex, OrderBy, SearchKey, ID, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord.Where(x => x.QCType != 0)
                         select new[] {
                             c.ID.ToString(),
                             c.FileID.ToString(),
                             c.Filename == null?string.Empty:c.Filename,
                             c.State == null?string.Empty:c.State,
                             c.Physical_Path == null?string.Empty:c.Physical_Path,
                             c.QCType == null?"0":c.QCType.ToString()

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

        [HttpPost, LoginAuthorized]
        public JsonResult FileTreeComplianceReportSecondLevel(JQueryDataTableParamModel param, int ID, string State, int QcType,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            int totalRecords = 0;
            List<FileTree> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            FileTree objFileTree = new FileTree();
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
            objFileTree.databasename = databasename;
            allRecord = objFileTree.GetJobListForComplianceReportSecondLevel(startIndex, endIndex, OrderBy, SearchKey, ID, State, QcType, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),
                             c.FileID.ToString(),
                             c.Filename == null?string.Empty:c.Filename,
                             c.State == null?string.Empty:c.State,
                             c.Physical_Path == null?string.Empty:c.Physical_Path,
                             c.QCType == null?"0":c.QCType.ToString()

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
        public JsonResult OtherFilesList(int JobID,string flag = "")
        {


            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            objJobsFilesVersions.databasename = databasename;
            var result = objJobsFilesVersions.GetFileVersionStateByJobID(JobID, "OTHER");
            var jsonResult = Json(new
            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public JsonResult ReferenceFilesList(int JobID,string flag = "")
        {
            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            objJobsFilesVersions.databasename = databasename;
            var result = objJobsFilesVersions.GetFileVersionStateByJobID(JobID, "REFERENCE");
            var jsonResult = Json(new
            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public JsonResult SourceFilesList(int JobID,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.databasename = databasename;
            var result = objJobsFiles.GetFilesListByJobID(JobID);

            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult JobsInitial(Jobs model)
        {

            List<string> Notes = new List<string>();

            if (model.QuoteOnly == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Quote");
                Notes.Add(modelFlag.detail);
            }
            if (model.DaxsFlag == "1")
            {
                var modelFlag = new Flags().GetFlagByName("ADLegacy");
                Notes.Add(modelFlag.detail);
            }
            if (model.JobFromADapi == "1")
            {
                var modelFlag = new Flags().GetFlagByName("JobFromADapi");
                Notes.Add(modelFlag.detail);
            }
            if (model.SourceFileTagging == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Non-PDF Files");
                Notes.Add(modelFlag.detail);
            }
            if (model.AltProvided == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Alt provided");
                Notes.Add(modelFlag.detail);
            }
            if (model.MultiLanguage == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Multi-language");
                Notes.Add(modelFlag.detail);
            }
            if (model.Sample == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Sample");
                Notes.Add(modelFlag.detail);
            }
            if (model.FixUpdate == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Update");
                Notes.Add(modelFlag.detail);
            }

            if (model.MultiDeadline == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Multi-Deadline");
                Notes.Add(modelFlag.detail);
            }
            if (model.ADScan == "1")
            {
                var modelFlag = new Flags().GetFlagByName("ADScan");
                Notes.Add(modelFlag.detail);
            }
            if (model.ADStream == "1")
            {
                var modelFlag = new Flags().GetFlagByName("ADStream");
                Notes.Add(modelFlag.detail);
            }
            if (model.NonProductionRelated == "1")
            {
                var modelFlag = new Flags().GetFlagByName("Non-Production Related");
                Notes.Add(modelFlag.detail);
            }


            string NotesDetails = String.Join(",", Notes);
            var databasename = HttpContext.Session.GetString("Database");
            var flag = Request.Form["flag"];
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            model.databasename = databasename;
            model.Notes = NotesDetails;
            var id = model.UpdateJob();
            JobsDeliveryContact modeJobsDeliveryContact = new JobsDeliveryContact();

            List<string> ContactIDs = new List<string>();



            foreach (string key in Request.Form.Keys)
            {
                if (key.Contains("JobDelivery"))
                {
                    if (Request.Form[key].ToString() != "")
                    {


                        modeJobsDeliveryContact.JobID = model.ID;
                        if (key.Contains("ContactID"))
                        {
                            ContactIDs.Add(Request.Form[key].ToString());
                            modeJobsDeliveryContact.ContactID = Convert.ToInt32(Request.Form[key].ToString());
                        }

                        modeJobsDeliveryContact.databasename = databasename;
                        modeJobsDeliveryContact.InsertContact();
                    }
                }
            }
            if (ContactIDs != null)
            {
                string conIDs = String.Join(",", ContactIDs);
                int jobid = model.ID;
                modeJobsDeliveryContact.databasename = databasename;
                modeJobsDeliveryContact.DeleteContact(jobid, conIDs);
            }

            var currenttab = Request.Form["currenttab"];
           
            if (currenttab == "open")
            {
                return RedirectToAction("Index", new { currenttab = currenttab });
            }
            else
            {

                return RedirectToAction("JobsInitial", new { id = Convert.ToInt32(id), flag = flag, save = 1 });
            }
        }

        [HttpGet, LoginAuthorized]
        public IActionResult AddClient(int param1,string param2 = "")
        {

            var model = new Clients();
            ViewBag.flag = param2;
            return PartialView(model);
        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult AddClient([Bind("Company,Code")] Clients model)
        {
            var databasename = HttpContext.Session.GetString("Database");
            var flag = Request.Form["flag"];
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            ViewBag.flag = flag;

            if (ModelState.IsValid)
            {
                model.databasename = databasename;
                string id = model.InsertUpdate();

                ViewBag.id = id.ToString();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ClientDropdown"), message = "Company added successfull.", divId = "client-list", reload = true });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddClient", model) });
        }

        [HttpGet, LoginAuthorized]
        public IActionResult AddContact(int param1,string param2 = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (param2 == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(param2);

            }
            else if (param2 == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(param2);
            }

            ViewBag.flag = param2;

            var model = new ClientsContacts();
            model.ClientID = param1;

            var modelClients = new Clients();
            modelClients.databasename = databasename;
            ViewBag.Client = modelClients.GetClientById(param1);
            ViewBag.databasename = databasename;


            // return PartialView("~/Views/Contacts/_edit.cshtml",model);
            return PartialView(model);
        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult AddContact(ClientsContacts model)
        {
            var databasename = HttpContext.Session.GetString("Database");
            var flag = Request.Form["flag"];
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            ViewBag.flag = flag;

            var modelClients = new Clients();
            modelClients.databasename = databasename;
            ViewBag.Client = modelClients.GetClientById(Convert.ToInt32(model.ClientID));
            ViewBag.databasename = databasename;

            if (ModelState.IsValid)
            {
                model.databasename = databasename;

                string id = model.InsertUpdate();

                ViewBag.id = id.ToString();
                model.databasename = databasename;
                model.loginusercountry = HttpContext.Session.GetString("Country");
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ContactDropdown", model), message = "Contact added successfull.", divId = "contact-list", reload = true });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddContact", model) });
        }

        [HttpGet, LoginAuthorized]
        //[ValidateAntiForgeryToken]
        public IActionResult ClientChange(int ClientID, int JobID,string flag = "")
        {

            Jobs model = new Jobs();

            model.ClientID = ClientID;
            model.ID = JobID;
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            model.databasename = databasename;
            var id = model.UpdateClient();


            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ContactDropdown", model), html2 = Helper.RenderRazorViewToString(this, "_deliverycontact", model), message = "Contact update successfull.", divId = "contact-list", reload = true });


        }


        [HttpGet, LoginAuthorized]
        public IActionResult CancelJob(int param1,string flag = "")
        {

            Jobs objJobMaster = new Jobs();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            objJobMaster.databasename = databasename;
            objJobMaster = objJobMaster.GetJobsById(Convert.ToInt32(param1));
            return PartialView(objJobMaster);
        }

        [HttpPost, LoginAuthorized]
        public IActionResult CancelJob(string State, string Params,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            string[] data = Params.ToString().Split("|").ToArray();
            Jobs model = new Jobs();
            if (State == "CopyAndCreateJob")
            {
                int ClientID = Convert.ToInt32(Params);

                model.databasename = databasename;
                model.ClientID = ClientID;
                var lastid = model.CreateJob();

                model = new Jobs();
                model.databasename = databasename;
                model.Deadline = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                model.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                model.ID = Convert.ToInt32(lastid);
                model.UpdateCopyJob();

                return Json(new { Status = State, jobidnew = Convert.ToInt32(lastid) });

            }
            else if (State == "CopyAndCreateJobStep2")
            {
                string pathCopy = this.webHostEnvironment.WebRootPath;
                string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ERP");

                var paramsExplode = Params.ToString().Split(" | ").ToArray();

                int oldJobID = Convert.ToInt32(paramsExplode[0]);
                int newJobID = Convert.ToInt32(paramsExplode[1]);
                string[] unCheckedFilesArray = new string[paramsExplode.Length - 1];

                int j = 0;
                for (var i = 2; i < paramsExplode.Length - 1; i++)
                {

                    unCheckedFilesArray[j] = paramsExplode[i];
                    j++;
                }
                JobsFiles modelJobFiles = new JobsFiles();
                modelJobFiles.databasename = databasename;
                var JobFileList = modelJobFiles.GetJobFilesListByJobID(oldJobID, 1);
                var created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Dictionary<int, int> matchFileID = new Dictionary<int, int>();
                //int[] matchFileID = new int[JobFileList.Count];
                if (JobFileList.Count > 0)
                {

                    foreach (var row in JobFileList)
                    {
                        modelJobFiles.JobID = newJobID;

                        modelJobFiles.Pages = row.Pages;
                        modelJobFiles.Filename = row.Filename;
                        modelJobFiles.Deleted = row.Deleted;
                        modelJobFiles.databasename = databasename;
                        int lastjobfileid = modelJobFiles.InsertFilesByJobCopy(created);
                        matchFileID.Add(row.ID, lastjobfileid);

                        var jobID = newJobID;
                        var fileID = lastjobfileid;



                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string path1 = Path.Combine(path, jobID.ToString());
                        string physicalPathJobFolder = path1;
                        string physicalPathFileFolder = "";
                        string filePath1 = "";
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1);
                            filePath1 = "/ERP/" + jobID;
                        }

                        if (fileID != 0)
                        {
                            path1 = Path.Combine(path1, fileID.ToString());
                            filePath1 = "/ERP/" + jobID + "/" + fileID;
                            physicalPathFileFolder = path1;
                        }
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1);
                        }

                    }
                }
                // Copy jobs_files_versions
                JobsFilesVersions modelJobFileVersion = new JobsFilesVersions();
                modelJobFileVersion.databasename = databasename;
                var objJobfileVer = modelJobFileVersion.GetFileVersionByJobID(oldJobID);
                if (objJobfileVer.Count > 0)
                {
                    foreach (var row in objJobfileVer)
                    {
                        int deleted = 0;
                        if (unCheckedFilesArray.Contains(Convert.ToString(row.ID)))
                        {
                            deleted = 1;
                            if (row.State == "SOURCE")
                            {

                                //modelJobFiles.ID = row.FileID;//Utility.CommonHelper.GetDBInt(matchFileID[row.FileID]);
                                modelJobFiles.ID = Utility.CommonHelper.GetDBInt(matchFileID[row.FileID]);

                                modelJobFiles.databasename = databasename;
                                modelJobFiles.FileDeleteQuote();
                            }
                        }

                        var fileName = (row.Filename != "") ? row.Filename : "demo.pdf";
                        var fileNameExplode = fileName.ToString().Split("_").ToArray();
                        var physicalPath = row.Physical_Path;

                        // insert new jobs_files_versions table
                        modelJobFileVersion.JobID = newJobID;
                        modelJobFileVersion.FileID = Utility.CommonHelper.GetDBInt(matchFileID[row.FileID]);
                        modelJobFileVersion.State = row.State;
                        modelJobFileVersion.Filename = fileName;
                        modelJobFileVersion.Type = row.Type;
                        modelJobFileVersion.Size = row.Size;
                        modelJobFileVersion.Physical_Path = row.Physical_Path;
                        modelJobFileVersion.MetaDataID = row.MetaDataID;
                        modelJobFileVersion.Deleted = deleted;
                        modelJobFileVersion.databasename = databasename;
                        var lastjobfileversionid = modelJobFileVersion.InsertByCopyJob();

                        var newFileName = lastjobfileversionid;

                        for (int i = 1; i < fileNameExplode.Length; i++)
                        {
                            newFileName += "_" + fileNameExplode[i];
                        }

                        //Folder Structure
                        int JobID = newJobID;
                        int FileID = Utility.CommonHelper.GetDBInt(matchFileID[row.FileID]);

                        string physicalPathJobFolder = "/ERP/" + JobID.ToString();//Path.Combine("/ERP", JobID.ToString());

                        string physicalPathFileFolder = physicalPathJobFolder + "/" + FileID.ToString(); //Path.Combine(physicalPathJobFolder, FileID.ToString());

                        string physicalPathFileName = Path.Combine(physicalPathFileFolder, newFileName.ToString());

                        // Update Jobs_Files_version
                        modelJobFileVersion.ID = Convert.ToInt32(lastjobfileversionid);
                        modelJobFileVersion.Filename = newFileName;
                        modelJobFileVersion.Physical_Path = physicalPathFileName;
                        modelJobFileVersion.databasename = databasename;
                        modelJobFileVersion.UpdateJobFileVersionByCopyJob();

                        pathCopy = this.webHostEnvironment.WebRootPath;
                        // Copy the files in actual directories now
                        string pathCopy2 = pathCopy + physicalPathFileName;

                        pathCopy = pathCopy + row.Physical_Path;
                        string fullPath = pathCopy;// Path.Combine(pathCopy, row.Physical_Path);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Copy(pathCopy, pathCopy2);

                            //System.IO.File.Copy(Path.Combine(this.webHostEnvironment.WebRootPath, row.Physical_Path), physicalPathFileName);
                        }

                        model.databasename = databasename;
                        model.JobCancelByCopyJob(newJobID, oldJobID);

                    }
                }
            }
            else if (State == "JustCancelJob")
            {
                model.databasename = databasename;
                model.ID = Convert.ToInt32(data[0]);
                string id = model.JobCancel();

            }
            return Json(new { Status = State });

        }

        [HttpGet, LoginAuthorized]
        public IActionResult FilesToBeCopied(int JobID,string flag = "")
        {
            Jobs model = new Jobs();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);

            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            model.databasename = databasename;
            var modelJob = model.GetJobsById(JobID);
            return PartialView(modelJob);
        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult CancelJobold(Jobs model)
        {
            string status = Request.Form["CancelOptions"];
            if (status == "1")
            {
                var databasename = HttpContext.Session.GetString("Database");
                model.databasename = databasename;
                string id = model.JobCancel();
            }


            return Json(new { isValid = true, reload = true });

        }

        [HttpGet, LoginAuthorized]
        public IActionResult Quote(int JobID, int updateVal = 0, int QTID = 0,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            ViewBag.dflag = flag;

            ViewBag.databasename = databasename;
            var model = new Jobs();
            model.ID = JobID;

            if (QTID > 0)
            {
                ViewBag.QuoteFile = QTID;
            }

            Clients modelClient = new Clients();
            modelClient.databasename = databasename;
            modelClient = modelClient.GetClientByJobId(JobID);
            ClientsContacts modelClientContact = new ClientsContacts();
            modelClientContact.databasename = databasename;//HttpContext.Session.GetString("Database");
            modelClientContact = modelClientContact.GetContactById(modelClient.ContactID);
            var language = (String.IsNullOrEmpty(modelClient.Language)) ? "EN" : modelClient.Language;

            ViewBag.modelClient = modelClient;
            ViewBag.modelClientContact = modelClientContact;

            var clientName = modelClient.Company;
            var billingCompany = modelClient.Billing_Company;
            var displayBillingContact = "";
            if (modelClientContact != null)
            {
                displayBillingContact = modelClientContact.FirstName + " " + modelClientContact.LastName + " - ";
            }

            billingCompany = (billingCompany == "") ? clientName : billingCompany;

            clientName = displayBillingContact + billingCompany;

            var clientID = modelClient.ClientID;


            QuoteTemp quoteTmpModel = new QuoteTemp();
            quoteTmpModel.databasename = databasename;
            var quoteTmp = quoteTmpModel.GetQuoteTmpDefaultInsert(JobID);
            var quoteDefaultFlag = 0;
            if (quoteTmp != null)
            {
                foreach (var row in quoteTmp)
                {
                    quoteDefaultFlag = 1;

                    quoteTmpModel.Information = clientName + "|" + clientID;
                    quoteTmpModel.databasename = databasename;
                    quoteTmpModel.UpdateQuoteTmpClientName();
                }
            }

            if (quoteDefaultFlag == 0)
            {

                quoteTmpModel.JobID = JobID;
                quoteTmpModel.Information = clientName + "|" + clientID;
                quoteTmpModel.TypeTmp = "client_name";
                quoteTmpModel.databasename = databasename;
                quoteTmpModel.QuoteTmpInsert();

                JobQuoteAutopopulate modelAutopopu = new JobQuoteAutopopulate();
                modelAutopopu.Language = language;
                var clients = modelAutopopu.GetJobPopulateByLanguage();

                foreach (var row in clients)
                {
                    int sub_title_added = 0;
                    if (row.Type == "sub_title")
                    {
                        //row.Information = str_replace("[client name]",clientName,row.Information);
                        var clientCountry = modelClient.Country;
                        var countrywiseSubtitle = modelAutopopu.GetSubtitleByCountry(clientCountry, language, "sub_title");

                        if (countrywiseSubtitle.Information != null)
                        {

                            row.Information = countrywiseSubtitle.Information;
                            quoteTmpModel.databasename = databasename;
                            var GetInfo = quoteTmpModel.GetInformation(row.Information);
                            if (GetInfo.Information == countrywiseSubtitle.Information)
                            {
                                sub_title_added = 1;
                            }
                        }
                        else
                        {

                            string actual = row.Information;
                            quoteTmpModel.databasename = databasename;
                            var GetInfo = quoteTmpModel.GetInformation(row.Information);
                            if (GetInfo.Information == row.Information)
                            {
                                sub_title_added = 1;
                            }

                            row.Information = actual.Replace("[client name]", clientName);
                            //sub_title_added = 1;
                        }




                    }
                    if (sub_title_added == 0)
                    {
                        quoteTmpModel.JobID = JobID;
                        quoteTmpModel.Information = row.Information;
                        quoteTmpModel.TypeTmp = row.Type;
                        quoteTmpModel.databasename = databasename;
                        quoteTmpModel.QuoteTmpInsert();
                    }


                }
                quoteTmpModel.JobID = JobID;
                quoteTmpModel.Information = "Yes";
                quoteTmpModel.TypeTmp = "DefaultInsert";
                quoteTmpModel.databasename = databasename;
                quoteTmpModel.QuoteTmpInsert();

            }

            QuoteTemp quoteTemp = new QuoteTemp();
            quoteTemp.databasename = databasename;
            ViewBag.modelQuote = quoteTemp.GetQuoteTmpListByJobID(model.ID);

            JobsFiles jobsFiles = new JobsFiles();

            jobsFiles.databasename = databasename;
            ViewBag.modelJobFiles = jobsFiles.GetJobFilesListByJobID(model.ID);

            List<QuoteTemp> subtitle = new List<QuoteTemp>();
            List<QuoteTemp> offering = new List<QuoteTemp>();
            List<QuoteTemp> notes = new List<QuoteTemp>();
            if (ViewBag.modelQuote != null)
            {
                foreach (var row in ViewBag.modelQuote)
                {
                    if (row.TypeTmp == "sub_title")
                    {
                        if (row.Deleted == 0)
                        {
                            subtitle.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }
                    }

                    if (row.TypeTmp == "offering")
                    {
                        if (row.Deleted == 0)
                        {
                            offering.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }

                    }

                    if (row.TypeTmp == "notes")
                    {
                        if (row.Deleted == 0)
                        {
                            notes.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }

                    }

                }
            }

            ViewBag.SubTitle = subtitle;
            ViewBag.Offering = offering;
            ViewBag.Notes = notes;
            ViewBag.updateVal = updateVal;
            ViewBag.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            model.databasename = databasename;
            return PartialView(model);
        }


        [HttpPost, LoginAuthorized]
        public IActionResult Job()
        {
           

            var Status = Request.Form["State"];
            var Params = Request.Form["Params"];
            var flag = Request.Form["flag"];

            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            var paramsExplodes = Params.ToString().Split(" ").ToArray();
            int JobID = Convert.ToInt32(Request.Form["JobID"]);
            string[] data = null;

            if (Status == "AddOffering")
            {
                QuoteTemp modelQuote = new QuoteTemp();
                data = Params.ToString().Split('|').ToArray();

                modelQuote.JobID = Convert.ToInt32(data[0]);
                modelQuote.Information = data[1];
                modelQuote.TypeTmp = "offering";
                modelQuote.databasename = databasename;
                modelQuote.QuoteTmpInsert();

                return Json(new { Status = Status, jobid = JobID });

            }
            if (Status == "AddNotes")
            {
                QuoteTemp modelQuote = new QuoteTemp();
                data = Params.ToString().Split('|').ToArray();

                modelQuote.JobID = Convert.ToInt32(data[0]);
                modelQuote.Information = data[1];
                modelQuote.TypeTmp = "notes";
                modelQuote.databasename = databasename;
                modelQuote.QuoteTmpInsert();

                return Json(new { Status = Status, jobid = JobID });

            }
            if (Status == "DeleteQuoteText")
            {
                QuoteTemp modelQuote = new QuoteTemp();

                data = Params.ToString().Split('|').ToArray();

                modelQuote.ID = Convert.ToInt32(data[0]);
                modelQuote.databasename = databasename;
                modelQuote.QuoteTmpDelete();

                return Json(new { Status = Status, jobid = JobID });

            }

            if (Status == "SaveQuoteText")
            {
                QuoteTemp modelQuote = new QuoteTemp();

                data = Params.ToString().Split('|').ToArray();

                modelQuote.ID = Convert.ToInt32(data[0]);
                modelQuote.Information = data[1];
                modelQuote.databasename = databasename;
                modelQuote.SaveQuoteText();
                return Json(new { Status = Status, message = "Update Successfully." });
            }

            if (Status == "AddEmptyFileState")
            {
                data = Params.ToString().Split('|').ToArray();
                JobsFiles modelJobsFiles = new JobsFiles();
                modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(data[0]);
                modelJobsFiles.Description = Utility.CommonHelper.GetDBString(data[1]);
                modelJobsFiles.Quantity = Utility.CommonHelper.GetDBDecimal(data[2]);
                modelJobsFiles.PricePer = Utility.CommonHelper.GetDBString(data[3]);
                modelJobsFiles.Price = Utility.CommonHelper.GetDBDecimal(data[4]);
                modelJobsFiles.Filename = Utility.CommonHelper.GetDBString(data[1]);

                modelJobsFiles.databasename = databasename;
                modelJobsFiles.InsertByQuote();

                return Json(new { Status = Status, jobid = JobID });
            }
            if (Status == "FileDeleteQuote")
            {
                data = Params.ToString().Split('|').ToArray();
                JobsFiles modelJobsFiles = new JobsFiles();
                modelJobsFiles.ID = Utility.CommonHelper.GetDBInt(data[0]);
                modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(data[1]);

                modelJobsFiles.databasename = databasename;
                modelJobsFiles.FileDeleteQuote();

                return Json(new { Status = Status, jobid = JobID });

            }
            if (Status == "AllFileDeleteQuote")
            {
                data = Params.ToString().Split('|').ToArray();
                JobsFiles modelJobsFiles = new JobsFiles();
                modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(data[0]);

                modelJobsFiles.databasename = databasename;
                modelJobsFiles.FileDeleteQuote("AllFileDeleteQuote");

                return Json(new { Status = Status, jobid = JobID });

            }

            if (Status == "DeleteAllOffering")
            {
                QuoteTemp modelQuote = new QuoteTemp();
                modelQuote.JobID = Utility.CommonHelper.GetDBInt(Params.ToString());
                modelQuote.TypeTmp = "offering";
                modelQuote.databasename = databasename;
                modelQuote.DeleteAll();
                return Json(new { Status = Status, jobid = JobID });
            }
            if (Status == "DeleteAllNotes")
            {
                QuoteTemp modelQuote = new QuoteTemp();
                modelQuote.JobID = Utility.CommonHelper.GetDBInt(Params.ToString());
                modelQuote.TypeTmp = "notes";
                modelQuote.databasename = databasename;
                modelQuote.DeleteAll();
                return Json(new { Status = Status, jobid = JobID });
            }

            if (Status == "DefaultUnitPriceSet")
            {
                Jobs model = new Jobs();
                data = Params.ToString().Split('|').ToArray();
                var jobID = data[0];
                var defaultUnit = data[1];
                var defaultPrice = data[2];
                var defaultQuantity = data[3];
                var unitTypePrice = "";
                if (defaultUnit == "Hour")
                {
                    unitTypePrice = "HourlyRate";
                }
                else if (defaultUnit == "Page")
                {
                    unitTypePrice = "PageRate";
                }
                else if (defaultUnit == "Document")
                {
                    unitTypePrice = "DocumentRate";
                }
                else
                {
                    unitTypePrice = "";
                }


                model.DefaultUnit = defaultUnit;
                model.DefaultPrice = Utility.CommonHelper.GetDBDecimal(defaultPrice);
                model.ID = Utility.CommonHelper.GetDBInt(jobID);
                model.databasename = databasename;
                model.UpdateDefaultPrice(unitTypePrice);

                JobsFiles modelJobsFiles = new JobsFiles();

                modelJobsFiles.PricePer = defaultUnit;
                modelJobsFiles.Price = Utility.CommonHelper.GetDBDecimal(defaultPrice);
                modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(jobID);
                modelJobsFiles.Quantity = Utility.CommonHelper.GetDBDecimal(defaultQuantity);
                modelJobsFiles.databasename = databasename;
                modelJobsFiles.DefaultPriceUpdate();

                if (defaultUnit == "Document")
                {
                    modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(jobID);
                    modelJobsFiles.databasename = databasename;
                    modelJobsFiles.DefaultDocUpdate();
                }
                return Json(new { Status = Status, jobid = JobID });

            }

            if (Status == "JobPanelUpdate")
            {
                var paramsSplit = Params.ToString().Split("[]").ToArray();
                var paramjobId = paramsSplit[0].ToString().Split("|").ToArray();
                // var JobID = paramjobId[6];
                for (var i = 0; i < (paramsSplit.Length) - 1; i++)
                {
                    var paramsExplode = paramsSplit[i].ToString().Split("|").ToArray();
                    int fileID = Convert.ToInt32(paramsExplode[0]);
                    decimal unitPrice = Convert.ToDecimal(paramsExplode[1]);
                    var pricePer = paramsExplode[2];
                    decimal quantity = Convert.ToDecimal(paramsExplode[3]);
                    var tax = paramsExplode[4];
                    var description = paramsExplode[5];
                    var description_id = Convert.ToInt32(paramsExplode[7]);

                    var oldUnitPrice = 0;
                    var oldPricePer = 0;
                    var oldQuantity = 0;
                    var oldTax = 0;
                    var oldDescription = 0;

                    JobsFiles modelJobsFiles = new JobsFiles();

                    // Get the current stored data

                    modelJobsFiles.databasename = databasename;
                    var row = modelJobsFiles.GetJobFilesByID(fileID);

                    if (row.Price != unitPrice)
                    {
                        oldUnitPrice = 1;
                    }
                    else if (row.OldPrice == 1)
                    {
                        oldUnitPrice = 1;
                    }

                    if (row.PricePer != pricePer)
                    {
                        oldPricePer = 1;
                    }
                    else if (row.OldPricePer == 1)
                    {
                        oldPricePer = 1;
                    }

                    if (row.Quantity != quantity)
                    {
                        oldQuantity = 1;
                    }
                    else if (row.OldQuantity == 1)
                    {
                        oldQuantity = 1;
                    }


                    if (row.Tax != tax)
                    {
                        oldTax = 1;
                    }
                    else if (row.OldTax == 1)
                    {
                        oldTax = 1;
                    }


                    if (row.Description != description)
                    {
                        oldDescription = 1;
                    }
                    else if (row.OldDescription == 1)
                    {
                        oldDescription = 1;
                    }



                    modelJobsFiles.PricePer = pricePer;
                    modelJobsFiles.Price = unitPrice;
                    modelJobsFiles.Quantity = quantity;
                    modelJobsFiles.Tax = tax;
                    modelJobsFiles.Description = description;
                    modelJobsFiles.OldPricePer = oldPricePer;
                    modelJobsFiles.OldPrice = oldUnitPrice;
                    modelJobsFiles.OldQuantity = oldQuantity;
                    modelJobsFiles.OldTax = oldTax;
                    modelJobsFiles.OldDescription = oldDescription;
                    modelJobsFiles.ID = fileID;
                    modelJobsFiles.databasename = databasename;
                    modelJobsFiles.description_id = description_id;
                    modelJobsFiles.JobPanelUpdate();



                }
                return Json(new { Status = Status, jobid = JobID });
            }
            if (Status == "QuoteDone")
            {
                Jobs model = new Jobs();
                model.ID = Convert.ToInt32(JobID);
                model.databasename = databasename;
                model.QuoteDone();
                // return Json(new { Status = Status, jobid = JobID });
                return Json(new { Status = Status, jobid = Convert.ToInt32(JobID) });
            }

            if (Status == "JobPanelUpdateInvoice")
            {
                var paramsSplit = Params.ToString().Split("[]").ToArray();
                var paramjobId = paramsSplit[0].ToString().Split("|").ToArray();
                //var JobID = paramjobId[6];
                InvoiceTmp modelInvoiceTmp = new InvoiceTmp();
                for (var i = 0; i < (paramsSplit.Length) - 1; i++)
                {
                    var paramsExplode = paramsSplit[i].ToString().Split("|").ToArray();

                    modelInvoiceTmp.PricePer = paramsExplode[2];
                    modelInvoiceTmp.Price = Convert.ToDouble(paramsExplode[1]);
                    modelInvoiceTmp.Quantity = Convert.ToDouble(paramsExplode[3]);
                    modelInvoiceTmp.Tax = paramsExplode[4];
                    modelInvoiceTmp.Description = paramsExplode[5];
                    modelInvoiceTmp.ID = Convert.ToInt32(paramsExplode[0]);
                    modelInvoiceTmp.databasename = databasename;
                    modelInvoiceTmp.JobPanelUpdateInvoice();

                }
                return Json(new { Status = Status, jobid = JobID });
            }

            if (Status == "JobInvoiceContactUpdate")
            {
                var paramsExplode = Params.ToString().Split("|").ToArray();
                InvoiceInstance modelInstance = new InvoiceInstance();

                modelInstance.CompanyName = paramsExplode[1];
                modelInstance.Address1 = paramsExplode[2];
                modelInstance.Address2 = paramsExplode[3];
                modelInstance.ClientFirstName = paramsExplode[4];
                modelInstance.ClientLastName = paramsExplode[5];
                modelInstance.Email = paramsExplode[6];
                modelInstance.Telephone = paramsExplode[7];
                modelInstance.City = paramsExplode[8];
                modelInstance.Province = paramsExplode[9];
                modelInstance.Country = paramsExplode[10];
                modelInstance.PostalCode = paramsExplode[11];
                modelInstance.ID = Convert.ToInt32(paramsExplode[0]);
                modelInstance.databasename = databasename;
                modelInstance.JobInvoiceContactUpdate();

                Jobs model = new Jobs();
                model.ContractDate = paramsExplode[12];
                model.ID = Convert.ToInt32(paramsExplode[13]);
                model.databasename = databasename;
                model.JobInvoiceContactUpdate();

                return Json(new { Status = Status, jobid = JobID });

            }

            if (Status == "FileDeleteInvoice")
            {
                var paramsExplode = Params.ToString().Split("|").ToArray();
                InvoiceTmp modelInvoiceTmp = new InvoiceTmp();

                modelInvoiceTmp.ID = Convert.ToInt32(paramsExplode[0]);
                modelInvoiceTmp.databasename = databasename;
                modelInvoiceTmp.FileDeleteInvoice();

                return Json(new { Status = Status, jobid = JobID });

            }

            if (Status == "SaveVarianceComment")
            {
                Jobs model = new Jobs();
                var paramsExplode = Params.ToString().Split("|").ToArray();
                model.ID = Convert.ToInt32(paramsExplode[0]);
                model.VarianceComment = paramsExplode[1];
                model.databasename = databasename;
                model.SaveVarianceComment();
                return Json(new { Status = Status, jobid = JobID });
            }

            if (Status == "DeleteFileVersion" || Status == "DeleteFileFromOtherList" || Status == "DeleteFileFromReferenceList" || Status == "DeleteFileFromFileList")
            {
                var paramsExplode = Params.ToString().Split("|").ToArray();

                JobsFilesVersions modelfilever = new JobsFilesVersions();
                JobsFiles objJobsFiles = new JobsFiles();

                modelfilever.databasename = databasename;
                if (Status == "DeleteFileFromFileList")
                {

                    modelfilever.DeleteFileVersionSource(Convert.ToInt32(paramsExplode[0]));
                    objJobsFiles.ID = Convert.ToInt32(paramsExplode[0]);

                    objJobsFiles.databasename = databasename;
                    objJobsFiles.FileDeleteQuote();
                }
                else
                {
                    modelfilever.DeleteFileVersion(Convert.ToInt32(paramsExplode[0]));
                }




                Jobs model = new Jobs();
                int id = JobID;

                model.databasename = databasename;
                Jobs modelJob = model.GetJobsById(id);
                objJobsFiles.databasename = databasename;
                var getFilesList = objJobsFiles.GetFilesListByJobID(id);

                JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
                objJobsFilesVersions.databasename = databasename;
                List<JobsFilesVersions> getFileVersionOtherStateList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "OTHER");
                List<JobsFilesVersions> getFileVersionReferenceList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "REFERENCE");

                modelJob.OtherFilesList = getFileVersionOtherStateList;

                modelJob.ReferenceFilesList = getFileVersionReferenceList;

                modelJob.FilesList = getFilesList;
                modelJob.databasename = databasename;

                Clients modelClient = new Clients();
                modelClient.databasename = databasename;
                var clientData = modelClient.GetClientById(modelJob.ClientID);
                modelJob.PORequired = clientData.PORequired;

                return Json(new { Status = Status, jobid = JobID, html = Helper.RenderRazorViewToString(this, "_files", modelJob) });

            }

            if (Status == "PanelSave")
            {
                var paramsExplode = Params.ToString().Split(" ").ToArray();
                int FileID = Convert.ToInt32(paramsExplode[0]);

                var olddate = paramsExplode[7];
                JobsFiles modelJobfiles = new JobsFiles();

                modelJobfiles.Deadline = paramsExplode[1];
                modelJobfiles.DeadlineTime = paramsExplode[2];
                modelJobfiles.Pages = Convert.ToInt64(paramsExplode[3]);
                modelJobfiles.PricePer = paramsExplode[5];
                modelJobfiles.Price = Convert.ToDecimal(paramsExplode[4]);
                modelJobfiles.ID = FileID;
                modelJobfiles.Quantity = Convert.ToDecimal(paramsExplode[6]);
                modelJobfiles.databasename = databasename;
                modelJobfiles.UpdatePanelSave();

                modelJobfiles.JobID = JobID;
                modelJobfiles.Deadline = paramsExplode[1];
                modelJobfiles.databasename = databasename;
                modelJobfiles.UpdateDeadlineDateByJobID(olddate);


                JobsFilesVersions objfilever = new JobsFilesVersions();
                objfilever.FileID = FileID;
                objfilever.databasename = databasename;
                objfilever = objfilever.GetFileVersionStateByFileID(FileID, "SOURCE");
                if (objfilever != null)
                {
                    MetaDataFiles objMetadata = new MetaDataFiles();
                    objMetadata.VersionID = objfilever.ID;
                    objMetadata.Pages = Convert.ToInt32(paramsExplode[3]);
                    objMetadata.databasename = databasename;
                    objMetadata.UpdateMetaDataPages();
                }

                Jobs model = new Jobs();
                model.databasename = databasename;
                var modelJob = model.GetJobsById(JobID);
                modelJob.databasename = databasename;

                Clients modelClient = new Clients();
                modelClient.databasename = databasename;
                var clientData = modelClient.GetClientById(modelJob.ClientID);
                modelJob.PORequired = clientData.PORequired;

                return Json(new { Status = Status, jobid = JobID, html = Helper.RenderRazorViewToString(this, "_files", modelJob) });
            }

            if (Status == "UpdateAltText")
            {
                var paramsExplode = Params.ToString().Split("|").ToArray();
                JobsFiles modelJobfiles = new JobsFiles();
                modelJobfiles.AltTxt = Convert.ToInt32(paramsExplode[1]);
                modelJobfiles.ID = Convert.ToInt32(paramsExplode[0]);
                modelJobfiles.databasename = databasename;
                modelJobfiles.UpdateAltText();
                return Json(new { Status = Status, jobid = JobID });
            }
            if(Status == "UpdateJobDeadline")
            {
                int jobID = Convert.ToInt32(Params);
                JobsFiles objJobFiles = new JobsFiles();
                objJobFiles.databasename = databasename;
               var deadlineQuery = objJobFiles.GetMaximumDeadlineDateByJob(jobID);
               var maxDeadline = deadlineQuery.MaximizedDeadline;

                Jobs objJob = new Jobs();
                objJob.databasename = databasename;
                objJob.Deadline = maxDeadline;
                objJob.ID = jobID;
                objJob.UpdateJobDeadlineDate();

               var modelJob = objJob.GetJobsById(jobID);
                modelJob.databasename = databasename;
                var getFilesList = objJobFiles.GetFilesListByJobID(jobID);
                modelJob.FilesList = getFilesList;

                if (modelJob.JobType != "MULTI")
                {
                    objJobFiles.Deadline = maxDeadline;
                    objJobFiles.JobID = jobID;
                    objJobFiles.UpdateDeadlineDateByJobID();
                }
                Clients modelClient = new Clients();
                modelClient.databasename = databasename;
                var clientData = modelClient.GetClientById(modelJob.ClientID);
                modelJob.PORequired = clientData.PORequired;

                return Json(new { Status = Status, jobid = JobID, html = Helper.RenderRazorViewToString(this, "_files", modelJob) });


            }

            if(Status == "UpdateDeadlineManual")
            {
                var deadline = Params.ToString();
                Jobs objJob = new Jobs();
                objJob.databasename = databasename;
                objJob.Deadline = deadline;
                objJob.ID = JobID;
                objJob.UpdateJobDeadlineDate(1);

                var modelJob = objJob.GetJobsById(JobID);
                modelJob.databasename = databasename;
                JobsFiles objJobFiles = new JobsFiles();
                objJobFiles.databasename = databasename;
                var getFilesList = objJobFiles.GetFilesListByJobID(JobID);
                modelJob.FilesList = getFilesList;

                if (modelJob.JobType != "MULTI")
                {
                    objJobFiles.Deadline = deadline;
                    objJobFiles.JobID = JobID;
                    objJobFiles.UpdateDeadlineDateByJobID();
                }
                Clients modelClient = new Clients();
                modelClient.databasename = databasename;
                var clientData = modelClient.GetClientById(modelJob.ClientID);
                modelJob.PORequired = clientData.PORequired;

                return Json(new { Status = Status, jobid = JobID, html = Helper.RenderRazorViewToString(this, "_files", modelJob) });

            }

            if (Status == "ToProduction")
            {
                var paramsExplode = Params.ToString().Split("|").ToArray();

                var currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"); //date("Y-m-d H:i:s");

                Dictionary<int,string> refFileArray = new Dictionary<int,string>();
                Dictionary<int,string> refSourceArray = new Dictionary<int,string>();

                var assignedTo = "";
                var today = DateTime.Today.ToString("yyyy-MM-dd");  //date("Y -m-d");
                int jobIDForRef = Convert.ToInt32(paramsExplode[0]);

                ReferenceLinking objRef = new ReferenceLinking();
                objRef.CJobID = jobIDForRef;
                objRef.databasename = databasename;
                var refTable = objRef.GetReferenceLinkingByJob();
                if(refTable != null)
                {
                    int cnt = 0;
                    foreach(var item in refTable)
                    {

                        if (!refFileArray.ContainsValue(item.RFileID.ToString()))
                        {
                            refFileArray.Add(cnt, item.RFileID.ToString());
                        }
                        
                        if (!refSourceArray.ContainsValue(item.CFileID.ToString())) {
                            refSourceArray.Add(cnt, item.CFileID.ToString());
                        }
                        
                        cnt++;
                    }
                }
                //var refFileArray = refTable.RFileID.ToString().Split(",").ToArray();
                //var refSourceArray = refTable.CFileID.ToString().Split(",").ToArray();
                Jobs objJob = new Jobs();
                for (int i = 0; i < paramsExplode.Length; i++)
                {
                    if (i == 0)
                    {
                        objJob.ID = Convert.ToInt32(paramsExplode[i]);
                        objJob.Status = "OPEN";
                        objJob.ContractDate = today;
                        objJob.databasename = databasename;
                        objJob.UpdateJobStatusWithContractDate();
                    }
                    else if (i == 1)
                    {
                        assignedTo = paramsExplode[i];
                    }
                    else
                    {
                        // Get the Deadline of the current file
                        
                       
                        if (paramsExplode[i] != "")
                        {
                            JobsFiles objJobFiles = new JobsFiles();
                            objJobFiles.databasename = databasename;
                            
                           var modelJobFiles = objJobFiles.GetJobFilesByID(Convert.ToInt32(paramsExplode[i]));

                        
                            var deadline = (modelJobFiles.Deadline != "" || modelJobFiles.Deadline != null) ? modelJobFiles.Deadline : "";



                        objJobFiles.UpdateFilePosition("TAGGING", "PENDING", 0, Convert.ToInt32(paramsExplode[i]), deadline);


                        objJobFiles.AssignedTo = assignedTo;
                        objJobFiles.ID = Convert.ToInt32(paramsExplode[i]);
                        objJobFiles.LastTagger = assignedTo;
                        objJobFiles.UpdateJobFilesBySendToProduction();

                        if (!refSourceArray.ContainsValue(paramsExplode[i]))
                        {
                                int j = 0;
                                foreach(var itm in refFileArray)
                                {
                                objRef.CFileID = Convert.ToInt32(paramsExplode[i]);
                                objRef.CJobID = jobIDForRef;
                                objRef.RFileID = Convert.ToInt32(itm.Value);
                                objRef.InsertBySendToProduction();
                                    j++;

                            }
                        }

                    }

                
                    }
                }

                return Json(new { Status = Status, jobid = JobID});
            }

                   
                




            return Json(new { Status = Status });
        }

        [HttpGet, LoginAuthorized]
        public IActionResult Testpage()
        {
            return PartialView("_generatequote");
        }

        [HttpGet, LoginAuthorized]
        //public IActionResult GenerateQuote(string submit)
        //{
        //    HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
        //    WebKitConverterSettings settings = new WebKitConverterSettings();
        //    settings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinariesWindows");
        //    htmlToPdfConverter.ConverterSettings = settings;

        //    PdfDocument document = htmlToPdfConverter.Convert("http://localhost:28251/jobs/testpage");

        //    MemoryStream ms = new MemoryStream();
        //    document.Save(ms);
        //    document.Close(true);

        //    ms.Position = 0;

        //    FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
        //    fileStreamResult.FileDownloadName = "Quote.pdf";


        //    return fileStreamResult;
        //}

        [LoginAuthorized]
        public IActionResult GenerateQuoteExcel(int JobID, int Flag, int InsertFlag,string dflag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");

            if (dflag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(dflag);
            }
            else if (dflag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(dflag);
            }


            int downloadQuote = 0;
            int emailQuote = 0;
            int insertQuoteTracking = 1;
            if (InsertFlag == 1)
            {
                insertQuoteTracking = 0;
            }

            if (Flag == 1)
            {
                downloadQuote = 1;
            }
            else if (Flag == 2)
            {
                emailQuote = 1;
            }

            Jobs objJobs = new Jobs();

            Clients modelClient = new Clients();
            modelClient.databasename = databasename;
            modelClient = modelClient.GetClientByJobId(JobID);
            ClientsContacts modelClientContact = new ClientsContacts();
            modelClientContact.databasename = databasename;//HttpContext.Session.GetString("Database");
            modelClientContact = modelClientContact.GetContactById(modelClient.ContactID);
            var language = (modelClient.Language == "") ? "EN" : modelClient.Language;

            ViewBag.modelClient = modelClient;
            ViewBag.modelClientContact = modelClientContact;

            var clientName = modelClient.Company;
            var billingCompany = modelClient.Billing_Company;
            var displayBillingContact = "";
            if (modelClientContact != null)
            {
                displayBillingContact = modelClientContact.FirstName + " " + modelClientContact.LastName + " - ";
            }

            billingCompany = (billingCompany == "") ? clientName : billingCompany;


            clientName = displayBillingContact + billingCompany;

            var clientID = modelClient.ClientID;



            Countries objCounntry = new Countries();
            objCounntry.code = modelClient.Country;
            var modelCountry = objCounntry.GetCountryByCode();
            var country_currency_symbol = "$";
            var country_currency_code = "USD";
            if (modelCountry != null)
            {
                country_currency_symbol = modelCountry.currency;
                country_currency_code = modelCountry.currency_code;
            }



            QuoteTemp quoteTmpModel = new QuoteTemp();
            quoteTmpModel.databasename = databasename;
            var quoteTmp = quoteTmpModel.GetQuoteTmpDefaultInsert(JobID);
            var quoteDefaultFlag = 0;
            if (quoteTmp != null)
            {
                foreach (var row in quoteTmp)
                {
                    quoteDefaultFlag = 1;

                    quoteTmpModel.Information = clientName + "|" + clientID;
                    quoteTmpModel.databasename = databasename;
                    quoteTmpModel.UpdateQuoteTmpClientName();
                }
            }

            if (quoteDefaultFlag == 0)
            {

                quoteTmpModel.JobID = JobID;
                quoteTmpModel.Information = clientName + "|" + clientID;
                quoteTmpModel.TypeTmp = "client_name";
                quoteTmpModel.databasename = databasename;
                quoteTmpModel.QuoteTmpInsert();

                JobQuoteAutopopulate modelAutopopu = new JobQuoteAutopopulate();
                modelAutopopu.Language = language;
                var clients = modelAutopopu.GetJobPopulateByLanguage();

                foreach (var row in clients)
                {

                    if (row.Type == "sub_title")
                    {
                        //row.Information = str_replace("[client name]",clientName,row.Information);
                        string actual = row.Information;

                        row.Information = actual.Replace("[client name]", clientName);
                    }

                    quoteTmpModel.JobID = JobID;
                    quoteTmpModel.Information = row.Information;
                    quoteTmpModel.TypeTmp = row.Type;
                    quoteTmpModel.databasename = databasename;
                    quoteTmpModel.QuoteTmpInsert();


                }
                quoteTmpModel.JobID = JobID;
                quoteTmpModel.Information = "Yes";
                quoteTmpModel.TypeTmp = "DefaultInsert";
                quoteTmpModel.databasename = databasename;
                quoteTmpModel.QuoteTmpInsert();

            }

            QuoteTemp quoteTemp = new QuoteTemp();
            quoteTemp.databasename = databasename;
            ViewBag.modelQuote = quoteTemp.GetQuoteTmpListByJobID(JobID);

            JobsFiles jobsFiles = new JobsFiles();
            jobsFiles.databasename = databasename;
            ViewBag.modelJobFiles = jobsFiles.GetJobFilesListByJobID(JobID);

            List<QuoteTemp> subtitle = new List<QuoteTemp>();
            List<QuoteTemp> offering = new List<QuoteTemp>();
            List<QuoteTemp> notes = new List<QuoteTemp>();
            if (ViewBag.modelQuote != null)
            {
                foreach (var row in ViewBag.modelQuote)
                {
                    if (row.TypeTmp == "sub_title")
                    {
                        if (row.Deleted == 0)
                        {
                            subtitle.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }
                    }

                    if (row.TypeTmp == "offering")
                    {
                        if (row.Deleted == 0)
                        {
                            offering.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }

                    }

                    if (row.TypeTmp == "notes")
                    {
                        if (row.Deleted == 0)
                        {
                            notes.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }

                    }

                }
            }

            ViewBag.SubTitle = subtitle;
            ViewBag.Offering = offering;
            ViewBag.Notes = notes;
            ViewBag.updateVal = 0;

            //start excel file here



            string pathXL = Path.Combine(this.webHostEnvironment.WebRootPath, "JobsQuote");
            if (!Directory.Exists(pathXL))
            {
                Directory.CreateDirectory(pathXL);
            }



            Jobs modelJobs = new Jobs();
            modelJobs.databasename = databasename;
            modelJobs = modelJobs.GetJobsById(JobID);
            var currentYear = DateTime.Now.ToString("yyyy");
            // filename = currentYear + "-" + modelJobs.Code + "-" + JobID + "-" + ID + "-Quote.pdf";


            string filenameXL = currentYear + "-" + modelJobs.Code + "-" + JobID + "-Excel.xlsx";
            string fullpathXL = Path.Combine(pathXL, filenameXL);


            //Excel.Application xlApp = new Excel.Application();

            //Microsoft.Office.Interop.Excel.Workbook excelWorkBook =
            //   xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);

            //Microsoft.Office.Interop.Excel.Worksheet workSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();

            var stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("QuoteXL");
                workSheet.Cells.AutoFitColumns();
                var currentRow = 1;

                if (subtitle.Count > 0)
                {
                    
                    workSheet.Cells[currentRow, 1].Value = subtitle[0].Information;

                    using (ExcelRange Rng = workSheet.Cells[currentRow,1, currentRow, 26])
                    {
                        Rng.Merge = true;
                    }
                    currentRow = currentRow + 2;
                }

                if (offering.Count > 0)
                {
                    ExcelRange rng = workSheet.Cells[currentRow, 1];
                    rng.Value = "Offering:";

                    foreach (var item in ViewBag.Offering)
                    {
                        currentRow++;
                        using (ExcelRange Rng = workSheet.Cells[currentRow, 2, currentRow, 10])
                        {
                            Rng.Value = "- " + item.Information;
                            Rng.Merge = true;
                            Rng.Style.WrapText = true;
                        }
                    }
                    currentRow++;
                }

                if (notes.Count > 0)
                {
                    workSheet.Cells[currentRow, 1].Value = "Notes:";

                    foreach (var item in ViewBag.Notes)
                    {
                        currentRow++;
                        using (ExcelRange Rng = workSheet.Cells[currentRow, 2, currentRow, 10])
                        {
                            Rng.Value = "- " + item.Information;
                            Rng.Merge = true;
                            Rng.Style.WrapText = true;
                        }
                    }
                }

                

                GenerateQuotePdf modelGenerateQuotePdf = new GenerateQuotePdf();

                if (insertQuoteTracking == 1)
                {
                    modelGenerateQuotePdf.currentDate = DateTime.Today;
                }
                else
                {
                    QuoteTracking objQuoteTracking = new QuoteTracking();
                    objQuoteTracking.databasename = databasename;
                    objQuoteTracking = objQuoteTracking.GetQuoteTrackingByJobID(JobID);
                    if (objQuoteTracking != null)
                    {
                        modelGenerateQuotePdf.currentDate = objQuoteTracking.LastUpdated;
                    }
                }
                modelGenerateQuotePdf.modelClient = modelClient;
                modelGenerateQuotePdf.modelClientContact = modelClientContact;
                modelGenerateQuotePdf.subtitle = subtitle;
                modelGenerateQuotePdf.offering = offering;
                modelGenerateQuotePdf.notes = notes;
                JobsFiles jobsFiles1 = new JobsFiles();

                jobsFiles1.databasename = databasename;
                modelGenerateQuotePdf.modelJobFiles = jobsFiles1.GetJobFilesListByJobID(JobID);




                // return PartialView("GenerateQuotePdf",modelGenerateQuotePdf);

                decimal subTotal = 0;
                var taxDB = "-1";
                decimal taxTotal = 0;
                Int64 taxSelected = 0;
                var unitType = "";
                var taxSelectedString = "";
                var fileCount = 0;

                currentRow++;
                currentRow++;
                int toprow = currentRow;

               
                workSheet.Cells[currentRow, 1].Value = "Description";
                workSheet.Cells[currentRow, 2].Value = "Quantity";
                workSheet.Cells[currentRow, 3].Value = "Unit";
                workSheet.Cells[currentRow, 4].Value = "Unit Price" + "(" + country_currency_symbol + ")";
                workSheet.Cells[currentRow, 5].Value = "Total" + "(" + country_currency_symbol + ")";
                using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                {
                    Rng.Style.Font.Bold = true;
                    //Rng.Style.Font.Color.SetColor(Color.Red);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    Rng.AutoFitColumns();
                }

                if (modelGenerateQuotePdf.modelJobFiles != null)
                {
                    foreach (var row in modelGenerateQuotePdf.modelJobFiles)
                    {
                        taxSelectedString = row.Tax;
                        subTotal = subTotal + (row.Price * row.Quantity);

                        currentRow++;
                        workSheet.Cells[currentRow, 1].Value = row.Description;
                        workSheet.Cells[currentRow, 2].Value = row.Quantity;
                        workSheet.Cells[currentRow, 3].Value = row.PricePer;
                        workSheet.Cells[currentRow, 4].Value = country_currency_symbol + row.Price.ToString("#,##0.00");
                        workSheet.Cells[currentRow, 5].Value = country_currency_symbol + (row.Price * row.Quantity).ToString("#,##0.00");
                    }
                }


                string provinceid = null;
                if (modelClient.stateid.ToString() != "" || modelClient.stateid.ToString() != null)
                {
                    provinceid = modelClient.stateid.ToString();
                }
                string Information = string.Empty;
                var modelJobPopulate = new JobQuoteAutopopulate().GetJobPopulateList(modelClient.Country, provinceid);
                if (modelJobPopulate != null)
                {

                    foreach (var row in new JobQuoteAutopopulate().GetJobPopulateList(modelClient.Country, provinceid))
                    {
                        if (taxSelectedString == row.Information)
                        {
                            Information = row.Information;
                            taxSelected = row.tax;
                        }
                    }
                }
                taxTotal = (taxSelected * subTotal) / 100;
                var totalAmount = taxTotal + subTotal;
                totalAmount.ToString("#,##0.00");

                //string s= Localizer["Total"].ToString();

                //workSheet.Cells[currentRow + 1, 1].Value = Localizer["TermsText"].ToString();
                //using (ExcelRange Rng = workSheet.Cells[currentRow + 1, 1, currentRow + 1, 3])
                //{
                //    Rng.Merge = true;
                //}
                // currentRow++;
                using (ExcelRange Rng = workSheet.Cells[currentRow + 1, 1, currentRow + 1, 5])
                {
                    Rng.Style.Font.Bold = true;
                    //Rng.Style.Font.Color.SetColor(Color.Red);
                    //Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                workSheet.Cells[currentRow + 1, 4].Value = "Sub-Total";
                workSheet.Cells[currentRow + 1, 5].Value = country_currency_symbol + totalAmount.ToString("#,##0.00");
                currentRow++;

                //workSheet.Cells[currentRow + 1, 1].Value = Localizer["PricesInCurrency"].ToString() + " " + country_currency_code;
                //using (ExcelRange Rng = workSheet.Cells[currentRow + 1, 1, currentRow + 1, 3])
                //{
                //    Rng.Merge = true;
                //}
                workSheet.Cells[currentRow + 1, 4].Value = Information;
                workSheet.Cells[currentRow + 1, 5].Value = country_currency_symbol + taxTotal.ToString("#,##0.00");
                currentRow++;
                using (ExcelRange Rng = workSheet.Cells[currentRow + 1, 1, currentRow + 1, 5])
                {
                    Rng.Style.Font.Bold = true;
                    //Rng.Style.Font.Color.SetColor(Color.Red);
                    //Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                workSheet.Cells[currentRow + 1, 4].Value = "Total";
                workSheet.Cells[currentRow + 1, 5].Value = country_currency_symbol + totalAmount.ToString("#,##0.00");

                using (ExcelRange Rng = workSheet.Cells[toprow, 4, currentRow + 1, 5])
                {
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (ExcelRange Rng = workSheet.Cells[toprow, 1, currentRow + 1, 5])
                {
                    Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Top.Color.SetColor(Color.Black);
                    Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Left.Color.SetColor(Color.Black);
                    Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Right.Color.SetColor(Color.Black);
                    Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Bottom.Color.SetColor(Color.Black);
                }

                package.Save();
            }

            // End excel file






            //workSheet.SaveAs(fullpathXL);

            //xlApp.Visible = false;
            //xlApp.UserControl = false;


            //xlApp.Quit();  //MainWindowTitle will become empty afer being close

            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkBook);

            //Process[] excelProcesses = Process.GetProcessesByName("excel");
            //foreach (Process p in excelProcesses)
            //{
            //    if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes 
            //    {
            //        p.Kill();
            //    }
            //}


            //using (var ms = new MemoryStream())
            //{
            //    byte[] fileContents = null;
            //    var sw = new FileStream(fullpathXL, FileMode.Open, FileAccess.Read);
            //    ms.Position = 0;
            //    sw.CopyTo(ms);
            //    fileContents = ms.ToArray();
            //    return File(fileContents, "application/vnd.ms-excel", filenameXL);
            //}

            stream.Position = 0;
            var contentType = "application/octet-stream";
            var fileName = filenameXL;
            return File(stream, contentType, fileName);
        }

        [LoginAuthorized]
        public async Task<IActionResult> GenerateQuotePdf(int JobID, int Flag, int InsertFlag,string dflag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (dflag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(dflag);
            }
            else if (dflag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(dflag);
            }

            int downloadQuote = 0;
            int emailQuote = 0;
            int insertQuoteTracking = 1;
            if (InsertFlag == 1)
            {
                insertQuoteTracking = 0;
            }

            if (Flag == 1)
            {
                downloadQuote = 1;
            }
            else if (Flag == 2)
            {
                emailQuote = 1;
            }

            Jobs objJobs = new Jobs();

            Clients modelClient = new Clients();
            modelClient.databasename = databasename;
            modelClient = modelClient.GetClientByJobId(JobID);
            ClientsContacts modelClientContact = new ClientsContacts();
            modelClientContact.databasename = HttpContext.Session.GetString("Database");
            modelClientContact = modelClientContact.GetContactById(modelClient.ContactID);
            var language = (modelClient.Language == "") ? "EN" : modelClient.Language;

            ViewBag.modelClient = modelClient;
            ViewBag.modelClientContact = modelClientContact;

            var clientName = modelClient.Company;
            var billingCompany = modelClient.Billing_Company;
            var displayBillingContact = "";
            if (modelClientContact != null)
            {
                displayBillingContact = modelClientContact.FirstName + " " + modelClientContact.LastName + " - ";
            }

            billingCompany = (billingCompany == "") ? clientName : billingCompany;


            clientName = displayBillingContact + billingCompany;

            var clientID = modelClient.ClientID;


            QuoteTemp quoteTmpModel = new QuoteTemp();
            quoteTmpModel.databasename = databasename;
            var quoteTmp = quoteTmpModel.GetQuoteTmpDefaultInsert(JobID);
            var quoteDefaultFlag = 0;
            if (quoteTmp != null)
            {
                foreach (var row in quoteTmp)
                {
                    quoteDefaultFlag = 1;

                    quoteTmpModel.Information = clientName + "|" + clientID;
                    quoteTmpModel.databasename = databasename;
                    quoteTmpModel.UpdateQuoteTmpClientName();
                }
            }

            if (quoteDefaultFlag == 0)
            {

                quoteTmpModel.JobID = JobID;
                quoteTmpModel.Information = clientName + "|" + clientID;
                quoteTmpModel.TypeTmp = "client_name";
                quoteTmpModel.databasename = databasename;
                quoteTmpModel.QuoteTmpInsert();

                JobQuoteAutopopulate modelAutopopu = new JobQuoteAutopopulate();
                modelAutopopu.Language = language;
                var clients = modelAutopopu.GetJobPopulateByLanguage();

                foreach (var row in clients)
                {

                    if (row.Type == "sub_title")
                    {
                        //row.Information = str_replace("[client name]",clientName,row.Information);
                        string actual = row.Information;

                        row.Information = actual.Replace("[client name]", clientName);
                    }

                    quoteTmpModel.JobID = JobID;
                    quoteTmpModel.Information = row.Information;
                    quoteTmpModel.TypeTmp = row.Type;
                    quoteTmpModel.databasename = databasename;
                    quoteTmpModel.QuoteTmpInsert();


                }
                quoteTmpModel.JobID = JobID;
                quoteTmpModel.Information = "Yes";
                quoteTmpModel.TypeTmp = "DefaultInsert";
                quoteTmpModel.databasename = databasename;
                quoteTmpModel.QuoteTmpInsert();

            }

            QuoteTemp quoteTemp = new QuoteTemp();
            quoteTemp.databasename = databasename;
            ViewBag.modelQuote = quoteTemp.GetQuoteTmpListByJobID(JobID);

            JobsFiles jobsFiles = new JobsFiles();
            jobsFiles.databasename = databasename;
            ViewBag.modelJobFiles = jobsFiles.GetJobFilesListByJobID(JobID);

            List<QuoteTemp> subtitle = new List<QuoteTemp>();
            List<QuoteTemp> offering = new List<QuoteTemp>();
            List<QuoteTemp> notes = new List<QuoteTemp>();
            if (ViewBag.modelQuote != null)
            {
                foreach (var row in ViewBag.modelQuote)
                {
                    if (row.TypeTmp == "sub_title")
                    {
                        if (row.Deleted == 0)
                        {
                            subtitle.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }
                    }

                    if (row.TypeTmp == "offering")
                    {
                        if (row.Deleted == 0)
                        {
                            offering.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }

                    }

                    if (row.TypeTmp == "notes")
                    {
                        if (row.Deleted == 0)
                        {
                            notes.Add(new QuoteTemp { Information = row.Information, ID = row.ID });
                        }

                    }

                }
            }

            ViewBag.SubTitle = subtitle;
            ViewBag.Offering = offering;
            ViewBag.Notes = notes;
            ViewBag.updateVal = 0;

            GenerateQuotePdf modelGenerateQuotePdf = new GenerateQuotePdf();

            if (insertQuoteTracking == 1)
            {
                modelGenerateQuotePdf.currentDate = DateTime.Today;
            }
            else
            {
                QuoteTracking objQuoteTracking = new QuoteTracking();
                objQuoteTracking.databasename = databasename;
                objQuoteTracking = objQuoteTracking.GetQuoteTrackingByJobID(JobID);
                if (objQuoteTracking != null)
                {
                    modelGenerateQuotePdf.currentDate = objQuoteTracking.LastUpdated;
                }
            }
            modelGenerateQuotePdf.modelClient = modelClient;
            modelGenerateQuotePdf.modelClientContact = modelClientContact;
            modelGenerateQuotePdf.subtitle = subtitle;
            modelGenerateQuotePdf.offering = offering;
            modelGenerateQuotePdf.notes = notes;
            JobsFiles jobsFiles1 = new JobsFiles();
            jobsFiles1.databasename = databasename;
            modelGenerateQuotePdf.modelJobFiles = jobsFiles1.GetJobFilesListByJobID(JobID);
            //return PartialView("GenerateQuotePdf",modelGenerateQuotePdf);

            decimal subTotal = 0;
            var taxDB = "-1";
            decimal taxTotal = 0;
            Int64 taxSelected = 0;
            var unitType = "";
            var taxSelectedString = "";
            var fileCount = 0;
            if (modelGenerateQuotePdf.modelJobFiles != null)
            {
                foreach (var row in modelGenerateQuotePdf.modelJobFiles)
                {
                    taxSelectedString = row.Tax;
                    subTotal = subTotal + (row.Price * row.Quantity);
                }
            }

            string provinceid = null;
            if (modelClient.stateid.ToString() != "" || modelClient.stateid.ToString() != null)
            {
                provinceid = modelClient.stateid.ToString();
            }

            var modelJobPopulate = new JobQuoteAutopopulate().GetJobPopulateList(modelClient.Country, provinceid);
            if (modelJobPopulate != null)
            {

                foreach (var row in new JobQuoteAutopopulate().GetJobPopulateList(modelClient.Country, provinceid))
                {
                    if (taxSelectedString == row.Information)
                    {
                        taxSelected = row.tax;
                    }
                }
            }
            taxTotal = (taxSelected * subTotal) / 100;
            var totalAmount = taxTotal + subTotal;
            totalAmount.ToString("#,##0.00");
            string ID = "";
            string filename = "";
            string path1 = "";
            string fullpath = "";
            if (insertQuoteTracking == 1)
            {
                QuoteTracking modelTracking = new QuoteTracking();
                modelTracking.JobID = JobID;
                modelTracking.QuoteAmount = Convert.ToDecimal(totalAmount.ToString("#,##0.00"));
                modelTracking.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                modelTracking.OperationType = "QuoteGenerated";
                modelTracking.databasename = databasename;
                ID = modelTracking.InsertQuoteTracking();

                string path = Path.Combine(this.webHostEnvironment.WebRootPath, "QuoteInvoices");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path1 = Path.Combine(path, JobID.ToString());
                string filePath1 = "";
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                    filePath1 = "/QuoteInvoices/" + JobID.ToString();
                }
                Jobs modelJobs = new Jobs();
                modelJobs.databasename = databasename;
                modelJobs = modelJobs.GetJobsById(JobID);
                var currentYear = DateTime.Now.ToString("yyyy");
                filename = currentYear + "-" + modelJobs.Code + "-" + JobID + "-" + ID + "-Quote.pdf";
                fullpath = Path.Combine(path1, filename);
                modelTracking = new QuoteTracking();
                modelTracking.ID = Utility.CommonHelper.GetDBInt(ID);
                modelTracking.PhysicalLocation = Path.Combine(filePath1, filename);
                modelTracking.databasename = databasename;
                modelTracking.UpdatePhysicalLocation();

                objJobs.ID = JobID;
                objJobs.QuoteFlag = "1";
                objJobs.databasename = databasename;
                objJobs.UpdateQuoteFlag();

                ViewAsPdf pdf = new ViewAsPdf("GenerateQuotePdf", modelGenerateQuotePdf)
                {
                    FileName = filename,
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = await pdf.BuildFile(ControllerContext);

                using (var fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(pdfData, 0, pdfData.Length);
                }
            }
            if (downloadQuote == 1)
            {

                return File(new FileStream(fullpath, FileMode.Open), "application/pdf", filename);
            }
            else if (emailQuote == 1)
            {
                string sTo = "";
                objJobs = new Jobs();
                objJobs.databasename = databasename;
                objJobs = objJobs.GetJobsById(JobID);
                ClientsContacts objClientsContacts = new ClientsContacts();
                objClientsContacts = objClientsContacts.GetContactById(objJobs.ClientID);

                string bcEmail = "";// objClientsContacts.Email;
                sTo = objClientsContacts.Email;
                string bcName = objClientsContacts.FirstName + " " + objClientsContacts.LastName;

                //objClientsContacts = new ClientsContacts();
                //objClientsContacts = objClientsContacts.GetContactById(Utility.CommonHelper.GetDBInt(objJobs.SecDeliveryContactID));

                string ccEmail = "";// objClientsContacts.Email;
                //string ccName = objClientsContacts.FirstName + " " + objClientsContacts.LastName;

                string message = string.Empty;

                //string BillingEmail = Localizer["BillingEmail"].ToString();
                //string CompanyName = Localizer["CompanyName"].ToString();

                ViewAsPdf pdf = new ViewAsPdf("GenerateQuotePdf", modelGenerateQuotePdf)
                {
                    FileName = filename,
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter
                    //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = await pdf.BuildFile(ControllerContext);

                Attachment attachment = Utility.EmailHelper.GetAttachFile(pdfData, filename);

                EmailTemplate modelTemplate = new EmailTemplate();
                modelTemplate = modelTemplate.GetTemplateByLang(objClientsContacts.Language, "Quote_Send_To_Client");
                string deliveryMessage = string.Empty;
                string actual = modelTemplate.DeliveryEmail;
                deliveryMessage = actual.Replace("[FirstName]", bcName);
                deliveryMessage = actual.Replace("[ID]", ID);
                deliveryMessage = Utility.CommonHelper.Nl2br(deliveryMessage);
                message = deliveryMessage;

                string subject = modelTemplate.subject;

                Utility.EmailHelper.Email(objClientsContacts.Country, sTo, ccEmail, bcEmail, subject, message, attachment);

                //return new ViewAsPdf("GenerateQuotePdf", modelGenerateQuotePdf);
                return Json(new { Status = "Success"});
            }
            else
            {
                return new ViewAsPdf("GenerateQuotePdf", modelGenerateQuotePdf);
            }

        }

        [HttpGet, LoginAuthorized]
        public IActionResult Invoice(int JobID, int updateVal = 0,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            ViewBag.databasename = databasename;
            ViewBag.dflag = flag;

            var model = new Jobs();
            model.ID = JobID;
            model.databasename = databasename;
            var modelJob = model.GetJobsById(JobID);
            InvoiceInstance modelInvoiceInstance = new InvoiceInstance();

            Clients modelClient = new Clients();
            modelClient.databasename = databasename;
            modelClient = modelClient.GetClientByJobId(JobID);
            ClientsContacts modelClientContact = new ClientsContacts();
            modelClientContact.databasename = databasename;//HttpContext.Session.GetString("Database");
            modelClientContact = modelClientContact.GetContactById(modelClient.ContactID);

            ViewBag.modelClient = modelClient;
            ViewBag.modelRegularContact = modelClientContact;
            if (modelClient.BillingContactID != "")
            {
                ClientsContacts modelBillingContact = new ClientsContacts();
                modelBillingContact.databasename = databasename;//HttpContext.Session.GetString("Database");
                modelBillingContact = modelBillingContact.GetContactById(Convert.ToInt32(modelClient.BillingContactID));
                ViewBag.modelBillingContact = modelBillingContact;
            }
            else {
                ClientsContacts modelBillingContact = new ClientsContacts();
                ViewBag.modelBillingContact = modelBillingContact;
            }
            


            var billingFN = "";
            var billingLN = "";

            var clientName = modelClient.Company;
            var clientProvince = modelClient.Province;
            var currency = modelClient.Currency;
            var hourlyRate = modelClient.HourlyRate;
            var pageRate = modelClient.PageRate;
            var billingCompany = modelClient.Billing_Company;
            var billingContact = modelClient.Billing_Contact;
            var billingTelephone = modelClient.Billing_Telephone;
            var billingEmail = modelClient.Billing_Email;
            var clientID = modelClient.ClientID;
            var address1 = modelClient.Address1;
            var address2 = modelClient.Address2;
            var city = modelClient.City;
            var country = modelClient.Country;
            var postalCode = modelClient.PostalCode;
            string[] contactExplode = (billingContact != null) ? billingContact.ToString().Split(" ") : 0.ToString().Split(" ");
            billingFN = contactExplode[0];
            var jobQuotedAs = modelClient.JobQuotedAs;

            for (var i = 1; i < contactExplode.Length; i++)
            {
                billingLN = billingLN + " " + contactExplode[i];
            }
            var billingContactID = modelClient.BillingContactID;
            var contactID = modelClient.ContactID;

            if (billingCompany == "")
            {
                billingCompany = clientName;
            }


            Int64 invID = 1000; // Starting Invoice ID
                                // here quote means invoice
            InvoiceTmp modelInvoiceTmp = new InvoiceTmp();
            modelInvoiceTmp.JobID = JobID;
            modelInvoiceTmp.databasename = databasename;
            var quoteTmp = modelInvoiceTmp.GetInvoiceTmpByJobID();

            var quoteDefaultFlag = 0;
            if (quoteTmp != null)
            {
                foreach (var row in quoteTmp)
                {
                    invID = row.InvoiceID;
                    quoteDefaultFlag = 1;
                }
            }
            JobsFiles modelJobFiles = new JobsFiles();
            if (quoteDefaultFlag == 0)
            {
                modelInvoiceTmp.databasename = databasename;
                var row = modelInvoiceTmp.GetMaxInvoiceID();


                if (row.maxInvoiceID == 0)
                {
                    invID = 1000;
                }
                else
                {
                    invID = row.maxInvoiceID;
                }
                invID = invID + 1;

                modelJobFiles.databasename = databasename;
                var clients = modelJobFiles.GetJobFilesListByJobID(JobID);
                if (clients != null)
                {
                    foreach (var rows in clients)
                    {
                        var descriptionText = "";
                        if (rows.Description == "")
                        {
                            descriptionText = rows.Filename;
                        }
                        else
                        {
                            descriptionText = rows.Description;
                        }



                        modelInvoiceTmp.InvoiceID = invID;
                        modelInvoiceTmp.FileID = rows.ID;
                        modelInvoiceTmp.JobID = JobID;
                        modelInvoiceTmp.PricePer = rows.PricePer;
                        modelInvoiceTmp.Price = Convert.ToDouble(rows.Price);
                        modelInvoiceTmp.Quantity = Convert.ToDouble(rows.Quantity);
                        modelInvoiceTmp.Tax = rows.Tax;
                        modelInvoiceTmp.Description = descriptionText;
                        modelInvoiceTmp.databasename = databasename;
                        modelInvoiceTmp.InvoiceTmpInsert();



                        modelJobFiles.OldPricePer = 0;
                        modelJobFiles.OldPrice = 0;
                        modelJobFiles.OldQuantity = 0;
                        modelJobFiles.OldTax = 0;
                        modelJobFiles.OldDescription = 0;
                        modelJobFiles.ID = row.ID;
                        modelJobFiles.databasename = databasename;
                        modelJobFiles.JobPanelOldPriceUpdate();


                    }
                }

            }
            else
            {

                modelJobFiles.databasename = databasename;
                var clients = modelJobFiles.GetJobFilesListByJobID(JobID);
                if (clients != null)
                {
                    foreach (var rows in clients)
                    {
                        var descriptionText = "";
                        if (rows.Description == "")
                        {
                            descriptionText = rows.Filename;
                        }
                        else
                        {
                            descriptionText = rows.Description;
                        }


                        var oldPricePer = rows.OldPricePer;
                        var oldPrice = rows.OldPrice;
                        var oldQuantity = rows.OldQuantity;
                        var oldTax = rows.OldTax;
                        var oldDescription = rows.OldDescription;




                        modelInvoiceTmp.PricePer = rows.PricePer;
                        modelInvoiceTmp.Price = Convert.ToDouble(rows.Price);
                        modelInvoiceTmp.Quantity = Convert.ToDouble(rows.Quantity);
                        modelInvoiceTmp.Tax = rows.Tax;
                        modelInvoiceTmp.Description = descriptionText;
                        modelInvoiceTmp.OldPricePer = oldPricePer;
                        modelInvoiceTmp.OldPrice = oldPrice;
                        modelInvoiceTmp.OldQuantity = oldQuantity;
                        modelInvoiceTmp.OldTax = oldTax;
                        modelInvoiceTmp.OldDescription = oldDescription;
                        modelInvoiceTmp.InvoiceID = invID;
                        modelInvoiceTmp.FileID = rows.ID;
                        modelInvoiceTmp.JobID = JobID;

                        modelInvoiceTmp.databasename = databasename;
                        modelInvoiceTmp.InvoiceTmpUpdate();


                        modelJobFiles.OldPricePer = 0;
                        modelJobFiles.OldPrice = 0;
                        modelJobFiles.OldQuantity = 0;
                        modelJobFiles.OldTax = 0;
                        modelJobFiles.OldDescription = 0;
                        modelJobFiles.ID = rows.ID;
                        modelJobFiles.databasename = databasename;
                        modelJobFiles.JobPanelOldPriceUpdate();
                    }
                }
            }

            int currentInvoiceRowID = 0;
            // Check if the Invoice Instance table has already been populated
            modelInvoiceInstance.databasename = databasename;
            var invoiceTable = modelInvoiceInstance.GetInvoiceInstanceAlreadyPopulated(invID);

            int invoiceDefaultFlag = 0;
            if (invoiceTable != null)
            {
                foreach (var row in invoiceTable)
                {
                    currentInvoiceRowID = row.rowID;
                    invoiceDefaultFlag = 1;
                }
            }

            if (invoiceDefaultFlag == 0)
            {

                modelInvoiceInstance.InvoiceID = invID;
                modelInvoiceInstance.JobID = JobID;
                modelInvoiceInstance.CompanyName = billingCompany;
                modelInvoiceInstance.Address1 = address1;
                modelInvoiceInstance.Address2 = address2;
                modelInvoiceInstance.ClientFirstName = billingFN;
                modelInvoiceInstance.ClientLastName = billingLN;
                modelInvoiceInstance.Email = billingEmail;
                modelInvoiceInstance.Telephone = billingTelephone;
                modelInvoiceInstance.City = city;
                modelInvoiceInstance.Province = clientProvince;
                modelInvoiceInstance.Country = country;
                modelInvoiceInstance.PostalCode = postalCode;
                modelInvoiceInstance.BillingContactID = billingContactID;
                modelInvoiceInstance.ContactID = Convert.ToString(contactID);
                modelInvoiceInstance.databasename = databasename;
                string id = modelInvoiceInstance.InvoiceInstanceInsert();

                currentInvoiceRowID = Convert.ToInt32(id);
            }
            else
            {

                modelInvoiceInstance.CompanyName = billingCompany;
                modelInvoiceInstance.Address1 = address1;
                modelInvoiceInstance.Address2 = address2;
                modelInvoiceInstance.ClientFirstName = billingFN;
                modelInvoiceInstance.ClientLastName = billingLN;
                modelInvoiceInstance.Email = billingEmail;
                modelInvoiceInstance.Telephone = billingTelephone;
                modelInvoiceInstance.City = city;
                modelInvoiceInstance.Province = clientProvince;
                modelInvoiceInstance.Country = country;
                modelInvoiceInstance.PostalCode = postalCode;
                modelInvoiceInstance.BillingContactID = billingContactID;
                modelInvoiceInstance.ContactID = Convert.ToString(contactID);
                modelInvoiceInstance.InvoiceID = invID;
                modelInvoiceInstance.JobID = JobID;

                modelInvoiceInstance.databasename = databasename;
                modelInvoiceInstance.InvoiceInstanceUpdate();

            }

            modelInvoiceInstance.ID = currentInvoiceRowID;
            modelInvoiceInstance.databasename = databasename;
            ViewBag.GetInvoiceInstance = modelInvoiceInstance.GetInvoiceInstanceByID();

            modelInvoiceTmp.JobID = JobID;
            modelInvoiceTmp.databasename = databasename;
            var invoiceTmpList = modelInvoiceTmp.GetInvoiceTmpByJobID();
            ViewBag.invoiceTmpList = invoiceTmpList;
            ViewBag.updateVal = updateVal;
            ViewBag.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));


            return PartialView(modelJob);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult Upload()
        {
            var databasename = HttpContext.Session.GetString("Database");

            var post = Request.Form;
            var uploadFlag = post["uploadFlag"];
            string POText = post["POText"];
            var flag = post["flag"];

            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            if (POText == "")
            {
                POText = null;

            }
            int jobID = Convert.ToInt32(post["Add_JobID"]);
            int fileID = (post["Add_FileID"] == "") ? 0 : Convert.ToInt32(post["Add_FileID"]);

            var Add_FileType = post["Add_FileType"];
            var upl = post["upl"];
            string qcType = "";
            string state = "SOURCE";
            if (uploadFlag == "referenceFile")
            {
                state = "REFERENCE";
            }
            else if (uploadFlag == "otherFile")
            {
                state = "OTHER";
            }

            Utility.UploadFile uploadFile = new Utility.UploadFile(this.webHostEnvironment);
            var files = HttpContext.Request.Form.Files;

            //if (state != "REFERENCE" && state != "OTHER")
            if (fileID == 0 || state == "SOURCE")
            {

                uploadFile.AddFile2Job(databasename, jobID, files, state);

            }
            else
            {
                uploadFile.ProcessRequest(databasename, jobID, fileID, state, qcType, files);
            }

            Jobs model = new Jobs();
            int id = jobID;
            model.databasename = databasename;
            var modelJob = model.GetJobsById(id);

            Jobs model2 = new Jobs();

            model2.ID = id;
            model2.POText = POText;
            model2.databasename = databasename;
            model2.UpdateJobPOText();

            Clients modelClient = new Clients();
            modelClient.databasename = databasename;
            var clientData = modelClient.GetClientById(modelJob.ClientID);
            modelJob.PORequired = clientData.PORequired;


            JobsFiles objJobsFiles = new JobsFiles();

            objJobsFiles.databasename = databasename;
            var getFilesList = objJobsFiles.GetFilesListByJobID(id);

            //JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            //List<JobsFilesVersions> getFileVersionOtherStateList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "OTHER");
            //List<JobsFilesVersions> getFileVersionReferenceList = objJobsFilesVersions.GetFileVersionStateByJobID(id, "REFERENCE");

            //modelJob.OtherFilesList = getFileVersionOtherStateList;

            //modelJob.ReferenceFilesList = getFileVersionReferenceList;

            modelJob.FilesList = getFilesList;
            modelJob.databasename = databasename;
            var msg = "";
            if (uploadFlag == "sourceFile")
            {
                        
                msg = "Source File Uploaded successfully.";
            }
            else if (uploadFlag == "otherFile")
            {
                        
                msg = "Other File Uploaded successfully.";
            }
            else if (uploadFlag == "referenceFile")
            {
                        
                msg = "Reference File Uploaded successfully.";
            }
            return Json(new { Status = uploadFlag, jobid = jobID, html = Helper.RenderRazorViewToString(this, "_files", modelJob), potext = POText, msg = msg });

            //         var files = HttpContext.Request.Form.Files;

            //         Utility.CommonHelper.MakeFolder(JobID);

            //         foreach (var file in files)
            //         {
            //             if (file.Length > 0)
            //             {
            //                 //Getting FileName
            //                 var fileName = Path.GetFileName(file.FileName);

            //                 //Assigning Unique Filename (Guid)
            //                 var myUniqueFileName = Convert.ToString(Guid.NewGuid());

            //                 //Getting file Extension
            //                 var fileExtension = Path.GetExtension(fileName);

            //                 // concatenating  FileName + FileExtension
            //                 var newFileName = String.Concat(myUniqueFileName, fileExtension);

            //                 // Combines two strings into a path.
            //                 var filepath =
            //new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ERP", JobID)).Root + $@"\{newFileName}";

            //                 using (FileStream fs = System.IO.File.Create(filepath))
            //                 {
            //                     file.CopyTo(fs);
            //                     fs.Flush();
            //                 }

            //             }
            //         }



        }

        [HttpGet, LoginAuthorized]
        public IActionResult Addurlstoscan(int param1)
        {
            ViewBag.jobID = param1;
            return PartialView();

        }
        [HttpPost, LoginAuthorized]
        public async Task<JsonResult> AddUrl(int jobID, string files,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            Utility.UploadFile uploadFile = new Utility.UploadFile(this.webHostEnvironment);
            await uploadFile.AddUrlFileToJob(databasename, jobID, files);
            var jsonResult = Json(new
            {
                aaData = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public IActionResult FilePreview(int FileID, int pdfH,string flag = "")
        {
            ViewBag.pdfH = pdfH * 0.9;
            ViewBag.FileJFV = FileID;
            
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            ViewBag.databasename = databasename;
            ViewBag.flag = flag;
            return PartialView();

        }

        [HttpGet, LoginAuthorized]
        public IActionResult FileQuoteState(int FileID, int pdfH,string flag = "")
        {

            ViewBag.pdfH = pdfH * 0.9;
            ViewBag.File = FileID;
            JobsFiles model = new JobsFiles();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            model.databasename = databasename;
            var modelJobfiles = model.GetJobFilesByID(FileID);
            ViewBag.databasename = databasename;
            ViewBag.flag = flag;
            modelJobfiles.databasename = databasename;
            return PartialView(modelJobfiles);

        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult Downloadall(int JobID,string flag = "")
        {
            Jobs model = new Jobs();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            model.databasename = databasename;
            var objJobmaster = model.GetJobsById(JobID);
            return PartialView("_Downloadall", objJobmaster);

        }

        [HttpPost, LoginAuthorized]
        public ActionResult ZipPDFFiles()
        {
            var databasename = HttpContext.Session.GetString("Database");

            string JobID = Request.Form["ID"];
            string State = Request.Form["DownloadStates"];
            string Mode = Request.Form["DownloadMode"];
            string flag = Request.Form["flag"];
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            JobsFilesVersions objADscanFiles = new JobsFilesVersions();

            objADscanFiles.databasename = databasename;
            var ADscanFileslist = objADscanFiles.GetFileVersionByJobID(Convert.ToInt32(JobID));
            string folder = string.Empty;

            var listState = State.Split(",").ToArray();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (JobsFilesVersions item in ADscanFileslist.Where(x => listState.Contains(x.State)))
                    {
                        try
                        {
                            if (Mode == "File")
                            {
                                folder = Path.Combine(item.FileID.ToString(), item.State);
                            }
                            else
                            {
                                folder = item.State;
                            }
                            string fullFileName = Path.Combine(folder, item.Filename);
                            var path = this.webHostEnvironment.WebRootPath + "/" + item.Physical_Path;
                            ziparchive.CreateEntryFromFile($"{path}", fullFileName, System.IO.Compression.CompressionLevel.Fastest);
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                return File(memoryStream.ToArray(), "application/zip", JobID + ".zip");
            }
        }


        [HttpGet, LoginAuthorized]
        public ActionResult DownloadFiles(string ID, string IDS,string flag = "")
        {
            return downloadFile(ID, IDS,flag);
        }
        [LoginAuthorized]
        public FileResult downloadFile(string ID, string IDS,string flag = "")
        {
            JobsFilesVersions jobsFilesVersions = new JobsFilesVersions();
            List<JobsFilesVersions> FileVersionList = new List<JobsFilesVersions>();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            jobsFilesVersions.databasename = databasename;
            FileVersionList = jobsFilesVersions.GetFileVersionByIDS(IDS);

            string folder = string.Empty;

            //var listState = State.Split(",").ToArray();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (JobsFilesVersions item in FileVersionList)
                    {
                        try
                        {

                            folder = item.State;

                            string fullFileName = Path.Combine(folder, item.Filename);
                            var path = this.webHostEnvironment.WebRootPath + "/" + item.Physical_Path;
                            ziparchive.CreateEntryFromFile($"{path}", fullFileName, System.IO.Compression.CompressionLevel.Fastest);
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                return File(memoryStream.ToArray(), "application/zip", ID + ".zip");
            }
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult POUpload(int JobID, string Status,string flag = "")
        {
            Jobs model = new Jobs();
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            model.databasename = databasename;
            var objJobmaster = model.GetJobsById(JobID);
            ViewBag.Status = Status;
            return PartialView("_POUpload", objJobmaster);

        }

        [HttpPost, LoginAuthorized]
        public JsonResult GenerateDelivery()
        {
            var databasename = HttpContext.Session.GetString("Database");
            string flag = Request.Form["flag"];
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            string Params = Request.Form["Params"];
            int JobID = Convert.ToInt32(Request.Form["JobID"]);
            var paramsExplode = Params.ToString().Split("|").ToArray();

            var subject = Request.Form["subject"];

            //var mailsubject = !string.IsNullOrEmpty(subject)? subject : "Files Delivery";

            var emailContacts = paramsExplode[1];
            var emailContactsExplode = emailContacts.ToString().Split(",").ToArray();

            var billingEmailInfo = emailContactsExplode[0];
            var contactEmailInfo = emailContactsExplode[1];
            var deliveryEmailInfo = emailContactsExplode[2];

            var billingEmailInfoExplode = billingEmailInfo.ToString().Split("=").ToArray();
            var contactEmailInfoExplode = contactEmailInfo.ToString().Split("=").ToArray();
            var deliveryEmailInfoExplode = deliveryEmailInfo.ToString().Split("=").ToArray();

            var billingEmail = billingEmailInfoExplode[0];
            var billingName = billingEmailInfoExplode[1];
            var contactEmail = contactEmailInfoExplode[0];
            var contactName = contactEmailInfoExplode[1];
            var deliveryEmail = deliveryEmailInfoExplode[0];
            var deliveryName = deliveryEmailInfoExplode[1];

            if (billingEmail == "")
            {
                billingEmail = contactEmail;
                billingName = contactName;
            }

            var deliveryMessage = paramsExplode[2];
            var filePath = "";
            var allFileID = "";
            var message = "";

            var randomkey = abledoc.Utility.ComboHelper.MD5Hash("abcdefghijklmnopqrstuvwxyz1234567890");//"1f2gsfd2df1d";//substr(md5(rand()), 0, 32);

            JobsFiles modelJobFiles = new JobsFiles();

            for (var i = 3; i < paramsExplode.Length - 1; i++)
            {
                var fileSpecificIdExplode = paramsExplode[i].ToString().Split("_").ToArray();

                var fileSpecificId = fileSpecificIdExplode[1];
                allFileID = allFileID + fileSpecificId + "|";

                // update delivery count as +1 for the files
                modelJobFiles.ID = Convert.ToInt32(fileSpecificId);
                modelJobFiles.databasename = databasename;
                modelJobFiles.UpdateJobsFilesDeliveryCount();

            }

            // Inserting into generate_delivery
            GenerateDelivery modelGD = new GenerateDelivery();
            modelGD.JobID = JobID;
            modelGD.AllJFVID = allFileID;
            modelGD.DownloadKey = randomkey;
            modelGD.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            modelGD.DeliveryMessage = deliveryMessage;
            modelGD.LinkExpiry = Convert.ToDateTime(DateTime.Today.AddDays(30).ToString("yyyy-MM-dd"));
            modelGD.databasename = databasename;
            var lastDeliveryId = modelGD.InsertGenerateDelivery();

            Users modelUser = new Users();
            var sessionUserData = modelUser.GetUserById(Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID")));

            var urlToClients = "https://download.abledocs.com/" + lastDeliveryId + "/" + randomkey;
            string actual = deliveryMessage;
            deliveryMessage = actual.Replace("[Download Link]", urlToClients);
            deliveryMessage = Utility.CommonHelper.Nl2br(deliveryMessage);
            message = deliveryMessage;
            message += "\n\n<br><br><strong>" + sessionUserData.FirstName + " " + sessionUserData.LastName + "</strong>";
            message += "\n<br>" + sessionUserData.Title;

            message += "\n<br>P: 905.467.7984";
            message += "\n<br>http://www.abledocs.com ";

            var from = sessionUserData.Email;
            var fromName = sessionUserData.FirstName + " " + sessionUserData.LastName;
            var ccEmail = "";
            var ccName = "";
            if (deliveryEmail != "" && deliveryEmail != "0")
            {
                ccEmail = deliveryEmail;
                ccName = deliveryName;
            }
            // EMAIL SEND TO CLIENT SECTION
            //emailThis($contactEmail, $contactName, 'Files Delivery', $message, $from, $fromName, "HTML", 'none', 'none', '', 0, $ccEmail, $ccName);
            Utility.EmailHelper.EmailThis(contactEmail, ccEmail, "", subject, message,from,false,null);
            // If the job is multi
            var deliveryCountFlag = 0;
            Jobs modelJob = new Jobs();
            modelJob.databasename = databasename;
            var jobStatus = modelJob.GetJobsById(JobID);


            if (jobStatus.JobType == "MULTI")
            {
                // Then check if all the jobs_files has been delivered

                modelJobFiles.databasename = databasename;
                var allFilesStatus = modelJobFiles.GetJobFilesListByJobID(JobID, 1);

                foreach (var row1 in allFilesStatus)
                {
                    if (row1.DeliveryCount == 0)
                    {
                        deliveryCountFlag++;
                    }
                }
                if (deliveryCountFlag == 0)
                {
                    // Update the State of the job to - Delivered
                    modelJob.ID = JobID;
                    modelJob.Status = "DELIVERED";
                    modelJob.databasename = databasename;
                    modelJob.UpdateJobStatus();

                }
                else
                {
                    // Update the State of the job to - Open
                    modelJob.ID = JobID;
                    modelJob.Status = "OPEN";
                    modelJob.databasename = databasename;
                    modelJob.UpdateJobStatus();
                }
            }
            else
            {
                // Update the State of the job to - Delivered
                modelJob.ID = JobID;
                modelJob.Status = "DELIVERED";
                modelJob.databasename = databasename;
                modelJob.UpdateJobStatus();
            }

            // make it a function
            // Check if the status of job has been set to Delivered, then if the invoice has been approved, and if so then move the job to Closed

            ERPClass erpObj = new ERPClass();
            var databasename1 = databasename;//HttpContext.Session.GetString("Database");
            var response = erpObj.JobStatusUpdate(JobID, "CheckToClosed", databasename1);

            return Json(new { isValid = true, reload = true });

        }

        [HttpPost, LoginAuthorized]
        public JsonResult clientdetails()
        {
            var flag = Request.Form["flag"];
            var databasename = Utility.CommonHelper.Getabledocs(flag); 
            var databasename1 = HttpContext.Session.GetString("Database");
            
            //if (flag == "1")
            //{
            //    databasename = Utility.CommonHelper.Getabledocs(flag);
            //}
            //else if (flag == "0")
            //{
            //    databasename = Utility.CommonHelper.Getabledocs(flag);
            //}

            var id = Request.Form["id"];
            Clients objClient = new Clients();
            objClient.databasename = databasename;
            var modelClient = objClient.GetClientById(Convert.ToInt32(id));
            CommonMaster objCommon = new CommonMaster();

            var modelCommon = objCommon.GetCommonByTypeCode(modelClient.Language);

            var html = "<p>";
            html += "Client Name: " + modelClient.Company + "<br>";
            if (modelClient.ClientSince != null)
            {
                html += "Since Year: " + DateTime.Parse(modelClient.ClientSince).Year + "<br>";
            }
            else
            {
                html += "Since Year: <br>";
            }
            html += "Country: " + modelClient.CountryName + "<br>";
            string code = HttpContext.Session.GetString("Country");
            if (databasename1 != databasename)
            {
                html += "Email: XXXXX <br>";
            }
            else
            {
                html += "Email: " + modelClient.Email + "<br>";
            }

            html += "Language: " + modelCommon.typename + "<br>";
            html += "</p>";

            return Json(new { status = true, details = html });
        }

        [HttpPost, LoginAuthorized]
        public JsonResult contactdetails()
        {
            var id = Request.Form["id"];
            ClientsContacts objClient = new ClientsContacts();

            var flag = Request.Form["flag"];
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            var databasename1 = HttpContext.Session.GetString("Database");

            objClient.databasename = databasename;
            var modelClient = objClient.GetContactById(Convert.ToInt32(id));
            CommonMaster objCommon = new CommonMaster();

            var modelCommon = objCommon.GetCommonByTypeCode(modelClient.Language);


            var html = "<p>";
            string code = HttpContext.Session.GetString("Country");
            if (databasename != databasename1)
            {
                html += "Contact Name: XXXXX <br>";
                html += "Country: " + modelClient.CountryName + "<br>";
                html += "Email: XXXXX <br>";
                html += "Language: " + modelCommon.typename + "<br>";
            }
            else
            {
                html += "Contact Name: " + (modelClient.FirstName + " " + modelClient.LastName) + "<br>";
                html += "Country: " + modelClient.CountryName + "<br>";
                html += "Email: " + modelClient.Email + "<br>";
                html += "Language: " + modelCommon.typename + "<br>";
            }

            html += "</p>";

            return Json(new { status = true, details = html });
        }

        [HttpPost, LoginAuthorized]
        public JsonResult uploadexcel(int jobID,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            try
            {
                var postedFile1 = HttpContext.Request.Form.Files;

                if (postedFile1 != null)
                {
                    foreach (var postedFile in postedFile1)
                    {
                        //Create a Folder.
                        string path = Path.Combine(this.webHostEnvironment.WebRootPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //Save the uploaded Excel file.
                        string e = Path.GetExtension(postedFile.FileName);
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+e;//Path.GetFileName(postedFile.FileName);
                        string filePath = Path.Combine(path, fileName);
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                        }

                        ////Read the connection string for the Excel file.
                        //string conString = this.Configuration.GetConnectionString("ExcelConString");
                        //DataTable dt = new DataTable();
                        //conString = string.Format(conString, filePath);

                        //using (OleDbConnection connExcel = new OleDbConnection(conString))
                        //{
                        //    using (OleDbCommand cmdExcel = new OleDbCommand())
                        //    {
                        //        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        //        {
                        //            cmdExcel.Connection = connExcel;

                        //            //Get the name of First Sheet.
                        //            connExcel.Open();
                        //            DataTable dtExcelSchema;
                        //            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        //            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        //            connExcel.Close();

                        //            //Read Data from First Sheet.
                        //            connExcel.Open();
                        //            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        //            odaExcel.SelectCommand = cmdExcel;
                        //            odaExcel.Fill(dt);
                        //            connExcel.Close();
                        //        }
                        //    }
                        //}

                        DataTable dt1 = GetDataTableFromExcel(filePath, true);

                        foreach (DataRow data in dt1.Rows)
                        {
                            if (string.IsNullOrEmpty(data[0].ToString()))
                            {
                                JobsFiles modelJobsFiles = new JobsFiles();
                                modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(jobID);
                                modelJobsFiles.Description = Utility.CommonHelper.GetDBString(data[1]);
                                modelJobsFiles.Quantity = Utility.CommonHelper.GetDBDecimal(data[2]);
                                modelJobsFiles.PricePer = Utility.CommonHelper.GetDBString(data[3]);
                                modelJobsFiles.Price = Utility.CommonHelper.GetDBDecimal(data[4]);
                                modelJobsFiles.Filename = Utility.CommonHelper.GetDBString(data[1]);

                                modelJobsFiles.databasename = databasename;
                                modelJobsFiles.InsertByQuote();
                            }
                            else {
                                JobsFiles modelJobsFiles = new JobsFiles();
                                modelJobsFiles.JobID = Utility.CommonHelper.GetDBInt(jobID);
                                modelJobsFiles.Description = Utility.CommonHelper.GetDBString(data[1]);
                                modelJobsFiles.Quantity = Utility.CommonHelper.GetDBDecimal(data[2]);
                                modelJobsFiles.PricePer = Utility.CommonHelper.GetDBString(data[3]);
                                modelJobsFiles.Price = Utility.CommonHelper.GetDBDecimal(data[4]);
                                modelJobsFiles.ID = Utility.CommonHelper.GetDBInt(data[0]);

                                modelJobsFiles.databasename = databasename;
                                modelJobsFiles.UpdateByQuote();
                                
                            }
                            
                            //jobsFiles
                        }
                        //    Insert the Data read from the Excel file to Database Table.
                        //conString = this.Configuration.GetConnectionString("abledocs");
                        //using (MySqlConnection con = new MySqlConnection(conString))
                        //{
                        //    MySqlBulkCopy sqlBulkCopy = new MySqlBulkCopy(con);

                        //    //Set the database table name.
                        //    sqlBulkCopy.DestinationTableName = "jobs_files";

                        //    //[OPTIONAL]: Map the Excel columns with that of the database table
                        //    MySqlBulkCopyColumnMapping mySqlBulkCopyColumnMapping = new MySqlBulkCopyColumnMapping();
                        //    mySqlBulkCopyColumnMapping.DestinationColumn = "JobId";
                        //    mySqlBulkCopyColumnMapping.SourceOrdinal = "JobId";
                        //    sqlBulkCopy.ColumnMappings.Add("JobId", jobID);
                        //    sqlBulkCopy.ColumnMappings.Add("Description", "Description");
                        //    sqlBulkCopy.ColumnMappings.Add("FileName", "Description");
                        //    sqlBulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                        //    sqlBulkCopy.ColumnMappings.Add("PricePer", "Unit");
                        //    sqlBulkCopy.ColumnMappings.Add("Price", "Unit Price");
                        //    con.Open();
                        //    sqlBulkCopy.WriteToServer(dt);
                        //    con.Close();

                        //}
                    }

                }
                return Json(new { uploadfile = "" });

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message });
            }

        }
         [LoginAuthorized]
        public FileResult DownloadSampleFile_old()
        {

            string filename = "Quote_Sample.xlsx";
            string path = Path.Combine(this.webHostEnvironment.WebRootPath, filename);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", filename);

            ////var path = Server.MapPath("~/Content/Uploads");
            //var path = Path.Combine(this.webHostEnvironment.WebRootPath , filename);
            //return File(path, "application/vnd.ms-excel", filename);
        }
        [LoginAuthorized]
        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = System.IO.File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
        [LoginAuthorized]
        public IActionResult DownloadSampleFile(int JobID,string flag = "")
        {
            var databasename = HttpContext.Session.GetString("Database");
            if (flag == "1")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }
            else if (flag == "0")
            {
                databasename = Utility.CommonHelper.Getabledocs(flag);
            }

            Jobs objJobs = new Jobs();

            Clients modelClient = new Clients();
            modelClient.databasename = databasename;
            modelClient = modelClient.GetClientByJobId(JobID);

            Countries objCounntry = new Countries();
            objCounntry.code = modelClient.Country;
            var modelCountry = objCounntry.GetCountryByCode();
            var country_currency_symbol = "$";
            var country_currency_code = "USD";
            if (modelCountry != null)
            {
                country_currency_symbol = modelCountry.currency;
                country_currency_code = modelCountry.currency_code;
            }

            string pathXL = Path.Combine(this.webHostEnvironment.WebRootPath, "JobsQuote");
            if (!Directory.Exists(pathXL))
            {
                Directory.CreateDirectory(pathXL);
            }



            Jobs modelJobs = new Jobs();
            modelJobs.databasename = databasename;
            modelJobs = modelJobs.GetJobsById(JobID);
            var currentYear = DateTime.Now.ToString("yyyy");

            string filenameXL = "Quote_Sample.xlsx";//currentYear + "-" + modelJobs.Code + "-" + JobID + "-Excel.xlsx";
            string fullpathXL = Path.Combine(pathXL, filenameXL);


            var stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("QuoteXL");
                workSheet.Cells.AutoFitColumns();
                var currentRow = 1;


                GenerateQuotePdf modelGenerateQuotePdf = new GenerateQuotePdf();

                JobsFiles jobsFiles1 = new JobsFiles();

                jobsFiles1.databasename = databasename;
                modelGenerateQuotePdf.modelJobFiles = jobsFiles1.GetJobFilesListByJobID(JobID);

                 int toprow = currentRow;

                workSheet.Cells[currentRow, 1].Value = "File ID";
                workSheet.Cells[currentRow, 2].Value = "Description";
                workSheet.Cells[currentRow, 3].Value = "Quantity";
                workSheet.Cells[currentRow, 4].Value = "Unit";
                workSheet.Cells[currentRow, 5].Value = "Unit Price";
                using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                {
                    Rng.Style.Font.Bold = true;
                    //Rng.Style.Font.Color.SetColor(Color.Red);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    Rng.AutoFitColumns();
                }

                if (modelGenerateQuotePdf.modelJobFiles != null)
                {
                    foreach (var row in modelGenerateQuotePdf.modelJobFiles)
                    {
                        currentRow++;
                        workSheet.Cells[currentRow, 1].Value = row.ID;
                        workSheet.Cells[currentRow, 2].Value = row.Description;
                        workSheet.Cells[currentRow, 3].Value = row.Quantity;
                        workSheet.Cells[currentRow, 4].Value = row.PricePer;
                        workSheet.Cells[currentRow, 5].Value = row.Price.ToString("#,##0.00");
                    }
                }


                using (ExcelRange Rng = workSheet.Cells[toprow, 4, currentRow + 1, 5])
                {
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                using (ExcelRange Rng = workSheet.Cells[toprow, 1, currentRow + 1, 5])
                {
                    Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Top.Color.SetColor(Color.Black);
                    Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Left.Color.SetColor(Color.Black);
                    Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Right.Color.SetColor(Color.Black);
                    Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Bottom.Color.SetColor(Color.Black);
                }

                package.Save();
            }


            stream.Position = 0;
            var contentType = "application/octet-stream";
            var fileName = filenameXL;
            return File(stream, contentType, fileName);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult getdescriptionbyname(string name)
        {
            string ID = "0";

            DiscriptionMaster discriptionMaster = new DiscriptionMaster();
            discriptionMaster = discriptionMaster.GetContentByName(name.Replace("_", " "));

            var jsonResult = Json(new
            {
                data = discriptionMaster
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult getunitlist(string name)
        {
            
            DiscriptionMaster discriptionMaster = new DiscriptionMaster();
            discriptionMaster = discriptionMaster.GetContentByName(name.Replace("_", " "));
            CommonMaster objCommon = new CommonMaster();
           var unitMaster = objCommon.GetCommonMasterByMultiId(discriptionMaster.unit.ToString());

            var jsonResult = Json(new
            {
                data = unitMaster
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

    }
}
