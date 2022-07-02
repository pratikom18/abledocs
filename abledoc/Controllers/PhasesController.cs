using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class PhasesController : Controller
    {
        //[AuthorizedAction]
        [LoginAuthorized]
        public IActionResult tagging()
        {
            return View();
        }
        [LoginAuthorized]
        public IActionResult review()
        {
            return View();
        }
        [LoginAuthorized]
        public IActionResult alttxt()
        {
            return View();
        }
        [LoginAuthorized]
        public IActionResult final()
        {
            return View();
        }
        [LoginAuthorized]
        public IActionResult qc()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult Phase2List(JQueryDataTableParamModel param, string searchstr)
        {
            var databasename = HttpContext.Session.GetString("Database");
            
            int totalRecords = 0;
            List<JobsFiles> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            JobsFiles objSearch = new JobsFiles();
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
            allRecord = objSearch.GetPhase2List(startIndex, endIndex, OrderBy, SearchKey, searchstr, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            var result = from c in allRecord
                         select new[] {
                                 c.ID.ToString(),                               //0
                                 c.JobID.ToString(),                            //1
                                 c.Filename == null ? "":c.Filename,            //2
                                 c.EngagementNum == null ? "" :c.EngagementNum, //3
                                 c.Deadline == null ? "":c.Deadline,            //4
                                 c.AssignedTo == null ? "":c.AssignedTo,        //5
                                 c.databasename.ToString(),                     //6
                                 c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",      //7
                                 c.CurrentVersionFileID.ToString()              //8
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
        public JsonResult Phase1List(JQueryDataTableParamModel param, string searchstr)
        {
            var databasename = HttpContext.Session.GetString("Database");
            

            int totalRecords = 0;
            List<JobsFiles> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            JobsFiles objSearch = new JobsFiles();
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
            
            objSearch.databasename = databasename;
            allRecord = objSearch.GetPhase1List(startIndex, endIndex, OrderBy, SearchKey, searchstr, out totalRecords, Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID")));
            int filteredRecords = totalRecords;

            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            var result = from c in allRecord
                         select new[] {
                                 c.ID.ToString(),                                   //0
                                 c.JobID.ToString(),                                //1
                                 c.Filename == null ? "":c.Filename,                //2
                                 c.EngagementNum == null ? "" :c.EngagementNum,     //3
                                 c.Deadline == null ? "":c.Deadline,                //4
                                 c.AssignedTo == null ? "":c.AssignedTo,            //5
                                 c.databasename.ToString(),                         //6 
                                  c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",         //7
                                  c.CurrentVersionFileID.ToString()                 //8 
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
        public JsonResult Phase4List(JQueryDataTableParamModel param, string searchstr)
        {
            var databasename = HttpContext.Session.GetString("Database");
           

            int totalRecords = 0;
            List<JobsFiles> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            //  string Havingclaue = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            JobsFiles objSearch = new JobsFiles();
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
            allRecord = objSearch.GetFinalSearchListForTable(startIndex, endIndex, OrderBy, SearchKey, searchstr, out totalRecords);  //GetSearchListForTable(startIndex, endIndex, OrderBy, SearchKey, searchstr, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),
                             c.Filename == null ? "":c.Filename,
                             c.EngagementNum == null ? "" :c.EngagementNum,
                             c.Deadline == null ? "":c.Deadline,
                             c.AltTxt.ToString() == null ? "" :c.AltTxt.ToString(),
                             c.FinalFix.ToString() == null ? "" :c.FinalFix.ToString(),
                             c.CurrentCheckout.ToString() == null ? "" :c.CurrentCheckout.ToString(),
                             c.JobID.ToString(),
                             c.AssignedTo == null?"":c.AssignedTo.ToString()

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
        public JsonResult alttextList(JQueryDataTableParamModel param, string searchstr)
        {
            var databasename = HttpContext.Session.GetString("Database");
           

            int totalRecords = 0;
            List<JobsFiles> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            JobsFiles objSearch = new JobsFiles();
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
            allRecord = objSearch.GetalttextList(startIndex, endIndex, OrderBy, SearchKey, searchstr, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            var result = from c in allRecord
                         select new[] {
                                 c.ID.ToString(),                                                   //0
                                 c.JobID.ToString(),                                                //1
                                 c.Filename == null ? "":c.Filename,                                //2
                                 c.EngagementNum == null ? "" :c.EngagementNum,                     //3
                                 c.Deadline == null ? "":c.Deadline,                                //4
                                 c.AssignedTo == null ? "":c.AssignedTo,                            //5
                                 c.databasename.ToString() == databasename.ToString()?"true":"false",//6
                                 c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                          //7
                                 c.CurrentVersionFileID.ToString()                                  //8
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
        public JsonResult Phase3List(JQueryDataTableParamModel param, string searchstr)
        {
            var databasename = HttpContext.Session.GetString("Database");
           

            int totalRecords = 0;
            List<JobsFiles> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            JobsFiles objSearch = new JobsFiles();
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
            allRecord = objSearch.GetPhase3List(startIndex, endIndex, OrderBy, SearchKey, searchstr, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            var result = from c in allRecord
                         select new[] {
                                 c.ID.ToString(),                               //0
                                 c.JobID.ToString(),                            //1
                                 c.Filename == null ? "":c.Filename,            //2
                                 c.EngagementNum == null ? "" :c.EngagementNum, //3
                                 c.Deadline == null ? "":c.Deadline,            //4
                                 c.AssignedTo == null ? "":c.AssignedTo,        //5
                                 c.databasename.ToString(),                     //6
                                 c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",      //7
                                 c.CurrentVersionFileID.ToString()              //8
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
        public PartialViewResult Confirm(int fileID,string flag)
        {
            ViewBag.FileID = fileID;
            ViewBag.flag = flag;
            return PartialView("_Confirm");
        }
        [HttpGet, LoginAuthorized]
        public PartialViewResult Delete(int fileID,string flag)
        {
            ViewBag.FileID = fileID;
            ViewBag.flag = flag;
            return PartialView("_Delete");
        }
        [HttpGet, LoginAuthorized]
        public PartialViewResult referencefile(int fileID)
        {
            ViewBag.FileID = fileID;

            return PartialView("_ReferenceFile");
        }
        [HttpPost, LoginAuthorized]
        public JsonResult InsertAltText(int FileID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
         

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = FileID;
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Checkout = DateTime.Now;
            objJobsFilesCheckouts.Checkout_PageNumber = 0;
            objJobsFilesCheckouts.State = "ALT";
            objJobsFilesCheckouts.databasename = databasename;
            string ID = objJobsFilesCheckouts.CreateJobsFilesCheckouts();

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.ID = FileID;
            objJobsFiles.ALTCheckout = 1;
            objJobsFiles.ALTCheckoutUser = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFiles.databasename = databasename;
            objJobsFiles.UpdateALTCheckout();

            var jsonResult = Json(new
            {
                aaData = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }
        [LoginAuthorized]
        public IActionResult AltTxtFile(int FileID, int LastID = 0,string flag="0")
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            ViewBag.FileID = FileID;
            ViewBag.File = FileID;
            if (LastID != 0)
            {
                ViewBag.LastFile = LastID;
            }
            ViewBag.IsSecure = false;
            AltTxtFile oAltTxtFile = new AltTxtFile();
            oAltTxtFile.databasename = databasename;

            Jobs objJobs = new Jobs();
            objJobs.databasename = databasename;
            oAltTxtFile.jobsfileid = objJobs.GetJobsByFileID(FileID);
            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            oAltTxtFile.jobsFilesVersionsList = objJobsFilesVersions.GetJobsFilesVersionsReferenceListByFileID(FileID);
            JobsFiles objJobsFiles = new JobsFiles();

            objJobsFiles.databasename = databasename;
            oAltTxtFile.jobsFiles = objJobsFiles.GetJobFilesByID(FileID);
            oAltTxtFile.jobsjobid = objJobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(oAltTxtFile.jobsFiles.JobID));
            if (oAltTxtFile.jobsjobid.JobQuotedAs != "Timed")
            {
                if (oAltTxtFile.jobsFiles.PricePer == "Hour")
                {
                    oAltTxtFile.jobsFiles.HourValue = oAltTxtFile.jobsFiles.Quantity + " Hrs";
                }
                else
                {
                    oAltTxtFile.jobsFiles.HourValue = "Timed";
                }
            }
            else
            {
                oAltTxtFile.jobsFiles.HourValue = "Timed";
            }

            Clients objClients = new Clients();
            objClients.databasename = databasename;
            oAltTxtFile.clients = objClients.GetClientById(oAltTxtFile.jobsjobid.ClientID);
            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.databasename = databasename;
            oAltTxtFile.jobsfilescheckoutslist = objJobsFilesCheckouts.GetJobsFilesCheckoutsByFileID(FileID);
            AllTimers objAllTimers = new AllTimers();
            objAllTimers.JobID = Utility.CommonHelper.GetDBInt(oAltTxtFile.jobsFiles.JobID);
            objAllTimers.FileID = FileID;
            objAllTimers.State = "ALT";
            objAllTimers.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objAllTimers.databasename = databasename;
            oAltTxtFile.allTimers = objAllTimers.GetAllTimersByIds();
            AltTexts objAltTexts = new AltTexts();
            objAltTexts.databasename = databasename;
            oAltTxtFile.alttextslistbyclients = objAltTexts.GetAltTextListByClientID(oAltTxtFile.jobsjobid.ClientID);
            oAltTxtFile.alttextslistbyfile = objAltTexts.GetAltTextListByFileId(FileID);
            BrandImage objBrandImage = new BrandImage();
            objBrandImage.databasename = databasename;
            oAltTxtFile.brandimagelist = objBrandImage.GetBrandImageByClientID(oAltTxtFile.jobsjobid.ClientID);
            objJobsFilesVersions.databasename = databasename;
            oAltTxtFile.JobsFilesVersionsSource = objJobsFilesVersions.GetFileVersionStateByFileID(FileID, "SOURCE");
            oAltTxtFile.JobsFilesVersionsTaging = objJobsFilesVersions.GetFileVersionStateByFileID(FileID, "TAGGING");
            oAltTxtFile.flag = flag;
            return View(oAltTxtFile);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult UpdateAltText(int FileID, int PageNum, string AltText,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            AltTexts objAltTexts = new AltTexts();
            objAltTexts.FileID = FileID;
            objAltTexts.PageNum = PageNum;
            objAltTexts.AltText = AltText;
            objAltTexts.databasename = databasename;
            objAltTexts.UpdateAltTextByFileId();

            var jsonResult = Json(new

            {
                message = "Success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult DeleteAltText(int FileID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            AltTexts objAltTexts = new AltTexts();
            objAltTexts.FileID = FileID;
            objAltTexts.databasename = databasename;
            objAltTexts.DeleteByFileId();

            var jsonResult = Json(new

            {
                message = "Success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }
        [HttpPost, LoginAuthorized]
        public JsonResult StateTimer(string State, string Params1,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            if (State == "TimerStartedState")
            {
                AllTimers objAllTimers = new AllTimers();

                string[] paramsExplode = Params1.Split("|");
                int jobID = Utility.CommonHelper.GetDBInt(paramsExplode[0]);
                int fileID = Utility.CommonHelper.GetDBInt(paramsExplode[1]);
                string fileState = Utility.CommonHelper.GetDBString(paramsExplode[2]);
                int fixFile = Utility.CommonHelper.GetDBInt(paramsExplode[3]);
                string lastTimerState = Utility.CommonHelper.GetDBString(paramsExplode[4]);
                int userID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                var format = "yyyy-MM-dd H:mm:ss";
                var stringDate = DateTime.Now.ToString(format);
                int tStart = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));
                int tStop = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));
                AllTimers objAllTimers1 = new AllTimers();
                objAllTimers.JobID = jobID;
                objAllTimers.FileID = fileID;
                objAllTimers.State = fileState;
                objAllTimers.UserID = userID;
                objAllTimers.databasename = databasename;
                objAllTimers1 = objAllTimers.GetAllTimersByIds();
                string lastTimerTotal = "00:00:00";
                if (objAllTimers1 != null)
                {
                    lastTimerTotal = objAllTimers1.TotalTimerNow;
                }

                if (lastTimerTotal == "")
                {
                    lastTimerTotal = "00:00:00";
                }
                objAllTimers.TStart = tStart;
                objAllTimers.TStop = tStop;
                objAllTimers.Fix = fixFile;
                objAllTimers.TotalTimerNow = lastTimerTotal;
                objAllTimers.databasename = databasename;
                string ID = objAllTimers.CreateAllTimers();

                var jsonResult = Json(new

                {
                    message = ID
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }
            else if (State == "TimerCompareState")
            {
                AllTimers objAllTimers = new AllTimers();

                string[] paramsExplode = Params1.Split("|");
                int jobID = Utility.CommonHelper.GetDBInt(paramsExplode[0]);
                int fileID = Utility.CommonHelper.GetDBInt(paramsExplode[1]);
                string fileState = Utility.CommonHelper.GetDBString(paramsExplode[2]);
                int fixFile = Utility.CommonHelper.GetDBInt(paramsExplode[3]);
                int userID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                int tStart = 0;
                int allTimersID = 0;
                var format = "yyyy-MM-dd H:mm:ss";
                var stringDate = DateTime.Now.ToString(format);
                int tStop = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));
                AllTimers objAllTimers1 = new AllTimers();
                objAllTimers.JobID = jobID;
                objAllTimers.FileID = fileID;
                objAllTimers.State = fileState;
                objAllTimers.UserID = userID;
                objAllTimers.databasename = databasename;
                objAllTimers1 = objAllTimers.GetAllTimersByIds();

                if (objAllTimers1 != null)
                {
                    allTimersID = objAllTimers1.ID;
                    tStart = Utility.CommonHelper.GetDBInt(objAllTimers1.TStart);
                }

                int totalSeconds = tStop - tStart;
                double totalHours = totalSeconds / 3600;
                totalHours = Math.Round(totalHours, 2);
                string realClockTimer = Utility.CommonHelper.TimerCompareStateServer(tStart, tStop);
                objAllTimers = new AllTimers();
                objAllTimers.ID = allTimersID;
                objAllTimers.TStop = tStop;
                objAllTimers.TimerNow = realClockTimer;
                objAllTimers.TotalSeconds = totalSeconds;
                objAllTimers.TotalHours = totalHours;
                objAllTimers.databasename = databasename;
                objAllTimers.UpdateAllTimers();

                objAllTimers1 = new AllTimers();
                objAllTimers = new AllTimers();
                objAllTimers.JobID = jobID;
                objAllTimers.FileID = fileID;
                objAllTimers.State = fileState;
                objAllTimers.UserID = userID;
                objAllTimers.databasename = databasename;
                objAllTimers1 = objAllTimers.GetAllTimersByIds();

                string timerNowInstance = "";
                int timerSeconds = 0;
                if (objAllTimers1 != null)
                {
                    timerNowInstance = objAllTimers1.TimerNow;
                    timerSeconds = Utility.CommonHelper.TimeFormatToSeconds(timerNowInstance);
                    totalSeconds = totalSeconds + timerSeconds;
                }

                string realClockTimerTotal = Utility.CommonHelper.SecondsToTimeFormat(totalSeconds);
                objAllTimers = new AllTimers();
                objAllTimers.ID = allTimersID;
                objAllTimers.TotalTimerNow = realClockTimerTotal;
                objAllTimers.databasename = databasename;
                objAllTimers.UpdateTotalTimerNow();
                var jsonResult = Json(new

                {
                    message = realClockTimerTotal
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }
            else
            {
                var jsonResult = Json(new

                {
                    message = ""
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }
        }

        [HttpPost, LoginAuthorized]
        public JsonResult Create(int FileID, int ClientID, string AltText, int PageNum, bool saveAsFrequent,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            AltTexts objAltTexts = new AltTexts();
            objAltTexts.FileID = FileID;
            objAltTexts.PageNum = PageNum;
            objAltTexts.AltText = AltText;
            objAltTexts.SaveAsFrequent = saveAsFrequent ? 1 : 0;
            objAltTexts.FrequentText = AltText;
            objAltTexts.FrequentPage = PageNum;
            objAltTexts.ClientID = ClientID;
            objAltTexts.databasename = databasename;
            objAltTexts.CreateAltText();

            var jsonResult = Json(new

            {
                message = ""
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }
        [HttpPost, LoginAuthorized]
        public JsonResult DoneAltText(string param1,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            string[] param = param1.Split("|");
            int id = Utility.CommonHelper.GetDBInt(param[0]);
            int alttxt = Utility.CommonHelper.GetDBInt(param[1]);
            string Timer = Utility.CommonHelper.GetDBString(param[2]);
            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.ID = id;
            objJobsFiles.AltTxt = alttxt;

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = id;
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Timer = Timer;
            objJobsFilesCheckouts.Checkin = DateTime.Now;
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.UpdateJobsFilesCheckOut();
            var jsonResult = Json(new

            {
                message = ""
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SetContactDatabase()
        {
            var databasename = Request.Form["databasename"];

            HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename));
            var jsonResult = Json(new

            {
                result = true
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }
    }
}
