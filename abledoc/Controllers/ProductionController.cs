using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ProductionController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IViewLocalizer Localizer;
        public ProductionController(IWebHostEnvironment hostEnvironment, IViewLocalizer _Localizer)
        {
            webHostEnvironment = hostEnvironment;
            Localizer = _Localizer;

        }
        [AuthorizedAction]
        public IActionResult Index()
        {
            Users model = new Users();
            ViewBag.AssigneUserList = model.GetAssignedUsersList();

            
            string[] statusArray = { "Phase 1", "Phase 2", "Phase 3", "ALT Text" , "Deliverables" };

            Production modelProduction = new Production();
            Dictionary<string, string> StatusCountList = new Dictionary<string, string>();
            foreach (var status in statusArray)
            {
                  var TotalCount = modelProduction.GetStatusList(status);
                  StatusCountList.Add(status, TotalCount);

            }
            ViewBag.StatusCountList = StatusCountList;
            //ViewBag.TotalPhase1 = modelProduction.GetStatusList("Phase 1");
            //ViewBag.TotalPhase2 = modelProduction.GetStatusList("Phase 2");
            //ViewBag.TotalPhase3 = modelProduction.GetStatusList("Phase 3");
            //ViewBag.TotalPhase4 = modelProduction.GetStatusList("Phase 4");
            //ViewBag.TotalPhase5 = modelProduction.GetStatusList("Phase 5");
            //ViewBag.TotalDeliverables = modelProduction.GetStatusList("Deliverables");
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ProductionList(JQueryDataTableParamModel param, string Status,string SearchBy, string txtSearch)
        {
            var databasename = HttpContext.Session.GetString("Database");
           
            int totalRecords = 0;
            List<Production> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Production objProdMaster = new Production();
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
            
            objProdMaster.databasename = databasename;

            allRecord = objProdMaster.GetProdListForTable(startIndex, endIndex, OrderBy, SearchKey, Status, out totalRecords, SearchBy, txtSearch);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                var totalFilterRecord = objProdMaster.GetDataTableFiltered(SearchKey, Status);
                filteredRecords = Convert.ToInt32(totalFilterRecord);//allRecord.Count();
            }
            Users objUserModel = new Users();
            
            var result = from c in allRecord
                         select new[] {
                             c.FileID.ToString(),                                                           //0
                             c.Code == null ? "" : c.Code.ToString(),                                       //1
                             c.JobID.ToString(),                                                            //2
                             c.Filename == null ? "" : c.Filename.ToString(),                               //3
                             c.Pages.ToString(),                                                            //4
                             c.Deadline == null ? "" : c.Deadline.ToString()+" "+c.DeadlineTime.ToString(), //5
                             c.JobType == null ? "" : c.JobType.ToString(),                                 //6
                             c.CurrentPage.ToString(),                                                      //7
                             c.AssemblyPage.ToString(),                                                     //8
                             c.CurrentCheckout == 0 ? "" : objUserModel.GetUsernameById(Utility.CommonHelper.GetDBInt(c.CurrentCheckout)),//9
                             c.AssignedTo == null ? "" : c.AssignedTo.ToString(),                           //10
                             c.ClientID.ToString(),                                                         //11
                             c.FormsFlag == null ? "" : c.FormsFlag.ToString(),                             //12
                             c.LinkingFlag == null ? "" : c.LinkingFlag.ToString(),                         //13
                             c.ReviewFlag == null ? "" : c.ReviewFlag.ToString(),                           //14
                             c.Lastcheckin == 0 ? "" : objUserModel.GetUsernameById(Utility.CommonHelper.GetDBInt(c.Lastcheckin)),//15
                             c.multiassign == "0" ? "0":c.multiassign.ToString(),       //16
                             c.databasename.ToString(),                                 //17
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB?"1":"0",                    //18
                             c.CurrentVersionFileID.ToString(), //19
                             c.Deadline == null ? "" : c.Deadline.ToString()//20

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
        public JsonResult GetAssignedUsersList()
        {
            Users model = new Users();
            var result = model.GetAssignedUsersList();
            var jsonResult = Json(new

            {
                data = result
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult AssignUpdate()
        {
            
            string assignedUser = null;
            string FileID = null;
           
            assignedUser = Request.Form["assignedUser"];
            FileID = Request.Form["FileID"];
            string flag = Request.Form["flag"];
            Production model = new Production();
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            model.databasename = databasename;
            var result = model.AssignUpdate(FileID, assignedUser);
            var jsonResult = Json(new

            {
                 message = result
            });
            return jsonResult;
        }

        //[HttpPost]
        //public JsonResult MultiAssignUpdate()
        //{

        //    string assignedUser = null;
        //    string[] assignfiles = null;

        //    assignedUser = Request.Form["assignedUser"];
        //    assignfiles = Request.Form["assignfiles[]"];

        //    Production model = new Production();

        //    var FileID = String.Join(",", assignfiles);
        //    var databasename = HttpContext.Session.GetString("Database");
        //    model.databasename = databasename;
        //    var result = model.MultiAssignUpdate(FileID, assignedUser);
        //    var jsonResult = Json(new

        //    {
        //        message = result
        //    });
        //    return jsonResult;
        //}

        [HttpPost, LoginAuthorized]
        public JsonResult MultiAssignUpdate()
        {

            string assignedUser = null;
            string[] assignfiles = null;

            assignedUser = Request.Form["assignedUser"];
            assignfiles = Request.Form["assignfiles"];
            string flag = Request.Form["flag"];

            Production model = new Production();

            var FileID = String.Join(",", assignfiles);
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            model.databasename = databasename;
            var result = model.MultiAssignUpdate(FileID, assignedUser);
            var jsonResult = Json(new

            {
                message = result
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public IActionResult DeadlineDate()
        {
            string[] assignfiles = null;
            Production model = new Production();
            assignfiles = Request.Form["assignfiles[]"];

            ViewBag.FileIDS = String.Join(",", assignfiles);

            return PartialView("_multidateform", model);

        }

        [HttpPost, LoginAuthorized]
        public JsonResult DeadlineDateUpdate(Production model)
        {
            string[] assignfiles = null;

            //assignfiles = Request.Form["assignfiles[]"];
            assignfiles = Request.Form["assignfiles"];
            string flag = Request.Form["flag"];

            var FileID = String.Join(",", assignfiles);
            //var databasename = HttpContext.Session.GetString("Database");
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            model.databasename = databasename;
            var result = model.MultiAssignDateUpdate(FileID);

            var jsonResult = Json(new

            {
                message = "Success"
            });
            return jsonResult;

        }

        [HttpGet, LoginAuthorized]
        public ActionResult DownloadFiles(string ID, string IDS, string flag = "")
        {
            return downloadFile(ID, IDS, flag);
        }
        [LoginAuthorized]
        public FileResult downloadFile(string ID, string IDS, string flag)
        {
            JobsFilesVersions jobsFilesVersions = new JobsFilesVersions();
            List<JobsFilesVersions> FileVersionList = new List<JobsFilesVersions>();
            var databasename = Utility.CommonHelper.Getabledocs(flag);
                       
            
            jobsFilesVersions.databasename = databasename;
            FileVersionList = jobsFilesVersions.GetFileVersionByFileIDS(IDS);

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
        public ActionResult DownloadFilesMulti(string IDS, string flag = "")
        {
            return downloadFileMulti(IDS, flag);
        }
        [LoginAuthorized]
        public FileResult downloadFileMulti(string IDS, string flag)
        {
            var fileIDSarray = IDS.ToString().Split(",").ToArray();
            var flagarray = flag.ToString().Split(",").ToArray();


            JobsFilesVersions jobsFilesVersions = new JobsFilesVersions();
            List<JobsFilesVersions> FileVersionList = new List<JobsFilesVersions>();
            int count = 0;

            

            
         
            string folder = string.Empty;

            //var listState = State.Split(",").ToArray();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var fileids in fileIDSarray)
                    {
                        flag = flagarray[count];
                        var databasename = Utility.CommonHelper.Getabledocs(flag);

                        var filesList = new Dictionary<string, string>();
                        jobsFilesVersions.databasename = databasename;
                        FileVersionList = jobsFilesVersions.GetFileVersionByFileIDS(fileids);

                        count++;




                        foreach (JobsFilesVersions item in FileVersionList)
                        {
                            try
                            {
                                folder = Path.Combine(Convert.ToString(item.JobID), item.State);
                                //folder = ;

                                string fullFileName = Path.Combine(folder, item.Filename);
                                var path = this.webHostEnvironment.WebRootPath + "/" + item.Physical_Path;
                                ziparchive.CreateEntryFromFile($"{path}", fullFileName, System.IO.Compression.CompressionLevel.Fastest);
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            }
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ProductionCurrentUserList(JQueryDataTableParamModel param, string Status, string SearchBy, string txtSearch)
        {

            int totalRecords = 0;
            List<Production> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Production objProdMaster = new Production();
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
            var databasename = HttpContext.Session.GetString("Database");
            allRecord = objProdMaster.GetProdListForCurrentUserTable(startIndex, endIndex, OrderBy, SearchKey, Status, Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"), out totalRecords, SearchBy, txtSearch);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                var totalFilterRecord = objProdMaster.GetDataTableFiltered(SearchKey, Status);
                filteredRecords = Convert.ToInt32(totalFilterRecord);//allRecord.Count();
            }
            Users objUserModel = new Users();
            var result = from c in allRecord
                         select new[] {
                             c.FileID.ToString(),                                                           //0
                             c.Filename == null ? "" : c.Filename.ToString(),                               //1
                             c.EngagementNum == null?"":c.EngagementNum,                                    //2
                             c.Deadline == null ? "" : c.Deadline.ToString()+" "+c.DeadlineTime.ToString(), //3
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB?"1":"0"                                        //4

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
