using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class QcFilesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public QcFilesController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;

        }
        [LoginAuthorized]
        public IActionResult Index(int ID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;

            ViewBag.State = "QC";

            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;

            QcFile objQcFile = new QcFile();
            JobsFiles objJobsFiles = new JobsFiles();
            
            objJobsFiles.databasename = databasename;
            objQcFile.jobsFiles = objJobsFiles.GetJobFilesByID(ID);
            Jobs objJobs = new Jobs();
            objJobs.databasename = databasename;
            objQcFile.jobs = objJobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(objQcFile.jobsFiles.JobID));

            if (objQcFile.jobs.JobQuotedAs != "Timed")
            {
                if (objQcFile.jobsFiles.PricePer == "Hour")
                {
                    objQcFile.jobs.JobQuotedAs = objQcFile.jobsFiles.Quantity.ToString() + " Hrs";
                }
                else
                {
                    objQcFile.jobs.JobQuotedAs = "Timed";
                }
            }
            else
            {
                objQcFile.jobs.JobQuotedAs = "Timed";
            }
            Clients objClients = new Clients();
            objClients.databasename = databasename;
            objQcFile.clients = objClients.GetClientById(objQcFile.jobs.ClientID);
            AllTimers objAllTimers = new AllTimers();
            objAllTimers.JobID = Utility.CommonHelper.GetDBInt(objQcFile.jobsFiles.JobID);
            objAllTimers.FileID = ID;
            objAllTimers.State = "QC";
            objAllTimers.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objAllTimers.databasename = databasename;
            objAllTimers = objAllTimers.GetAllTimersByIds();
            objQcFile.allTimers = objAllTimers;
            if (objQcFile.jobsFiles.Status == "FINAL" || objQcFile.jobsFiles.Status == "QC")
            {
                AltTexts objAltTexts = new AltTexts();
                objAltTexts.databasename = databasename;
                objQcFile.altTextsList = objAltTexts.GetAltTextListByFileId(ID);
            }
            //JobsFilesFinal objjobsFilesFinal = new JobsFilesFinal();
            //objQcFile.jobsFilesFinalList = objjobsFilesFinal.GetFLC(Utility.CommonHelper.GetDBInt(objQcFile.jobsFiles.JobID), ID);
            //JobsFilesQC objJobsFilesQC = new JobsFilesQC();
            //objQcFile.jobsFilesQCList = objJobsFilesQC.GetQCLC(Utility.CommonHelper.GetDBInt(objQcFile.jobsFiles.JobID), ID);
            JobsFilesReviews objjobsFilesFinal = new JobsFilesReviews();
            objjobsFilesFinal.databasename = databasename;
            objQcFile.jobsFilesCommentsList = objjobsFilesFinal.GetCommentsPhase4or5(Utility.CommonHelper.GetDBInt(objQcFile.jobsFiles.JobID), ID, "qc", "");


            objQcFile.flag = flag;
            return View(objQcFile);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult UpdateAltText(string Params,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
           
            var paramsExplode = Params.ToString().Split("|").ToArray();
            JobsFiles modelJobfiles = new JobsFiles();
            modelJobfiles.AltTxt = Convert.ToInt32(paramsExplode[1]);
            modelJobfiles.ID = Convert.ToInt32(paramsExplode[0]);
            modelJobfiles.databasename = databasename;
            modelJobfiles.UpdateAltText();

            var jsonResult = Json(new

            {
                message = "Success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }
        [HttpPost, LoginAuthorized]
        public JsonResult SecondTimer(string Params,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            var paramsExplode = Params.ToString().Split("|").ToArray();
            SecondTimer objSecondTimer = new SecondTimer();
            objSecondTimer.Timer = paramsExplode[0].ToString();
            objSecondTimer.Message = paramsExplode[1].ToString();
            objSecondTimer.QueryType = paramsExplode[2].ToString();
            objSecondTimer.JobID = Utility.CommonHelper.GetDBInt(paramsExplode[3].ToString());
            objSecondTimer.FileID = Utility.CommonHelper.GetDBInt(paramsExplode[4].ToString());
            objSecondTimer.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objSecondTimer.databasename = databasename;
            objSecondTimer.Insert();

            var jsonResult = Json(new

            {
                message = "Success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult DownloadFileTreeFirstLevel(JQueryDataTableParamModel param, int ID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

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
            string[] colunms = param.sColumns.Split(",");
            string sortcolunm = Request.Form["iSortCol_0"];
            string sortColunmName = colunms[Convert.ToInt32(sortcolunm)].ToString();
            string sortdir = Request.Form["sSortDir_0"];
            if (!string.IsNullOrEmpty(sortColunmName) && !string.IsNullOrEmpty(sortdir))
            {
                OrderBy = sortColunmName + " " + sortdir;
            }
            objFileTree.databasename = databasename;
            allRecord = objFileTree.GetJobListForSecondLevel(startIndex, endIndex, OrderBy, SearchKey, ID, out totalRecords);
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
        public JsonResult DownloadFileTreeSecondLevel(JQueryDataTableParamModel param, int ID, string State,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag); 

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
            string[] colunms = param.sColumns.Split(",");
            string sortcolunm = Request.Form["iSortCol_0"];
            string sortColunmName = colunms[Convert.ToInt32(sortcolunm)].ToString();
            string sortdir = Request.Form["sSortDir_0"];
            if (!string.IsNullOrEmpty(sortColunmName) && !string.IsNullOrEmpty(sortdir))
            {
                OrderBy = sortColunmName + " " + sortdir;
            }
            objFileTree.databasename = databasename;
            allRecord = objFileTree.GetJobListForDownloadFileTreeLevel(startIndex, endIndex, OrderBy, SearchKey, ID, State, out totalRecords);
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
        public JsonResult QcFileCheckin(int File, string Comments, int Job, string AltTextStatus, string Timer,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            objJobsFilesVersions = objJobsFilesVersions.GetJobsFilesVersionsByFileID(File);
            string LastId = string.Empty;
            if (Comments != string.Empty)
            {
                JobsFilesQC objJobsFilesQC = new JobsFilesQC();
                objJobsFilesQC.VersionID = objJobsFilesVersions.ID;
                objJobsFilesQC.QualityControllerID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                objJobsFilesQC.FileID = File;
                objJobsFilesQC.JobID = Job;
                objJobsFilesQC.LastUpdated = DateTime.Now;
                objJobsFilesQC.databasename = databasename;
                LastId = objJobsFilesQC.Insert();
            }

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = File;
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Timer = Timer;
            objJobsFilesCheckouts.State = "QC";
            objJobsFilesCheckouts.Checkin = DateTime.Now;
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.UpdateJobsFilesCheckOutByFileIdUserId();

            if (Comments != string.Empty)
            {
                JobsFilesQC objJobsFilesQC = new JobsFilesQC();
                objJobsFilesQC.VersionID = objJobsFilesVersions.ID;
                objJobsFilesQC.ID = Utility.CommonHelper.GetDBInt(LastId);
                objJobsFilesQC.Comments = Comments;
                objJobsFilesQC.databasename = databasename;
                objJobsFilesQC.UpdateJobsFilesQcCommentByVesrionIdLastId();
            }

            HttpContext.Session.SetString("LastCheckedInTimer", Utility.CommonHelper.GetDBString(Timer));
            string FullName = HttpContext.Session.GetString("FirstName") + " " + HttpContext.Session.GetString("LastName");

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.Touches = FullName;
            objJobsFiles.AltTxt = Utility.CommonHelper.GetDBInt(AltTextStatus);
            objJobsFiles.ID = File;
            objJobsFiles.databasename = databasename;
            objJobsFiles.UpdateJobsFilesTouches();

            var jsonResult = Json(new
            {
                message = "Checked in file " + File
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult AjaxTimeState(string State, string Params,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFiles objJobsFiles = new JobsFiles();
            JobsFilesCheckouts ObjJobsFilesCheckouts = new JobsFilesCheckouts();
            if (State == "TAGGINGTime" || State == "REVIEWTime" || State == "FINALTime" || State == "QCTime" || State == "ALTTime")
            {

                string[] paramsExplode = Params.Split("|");
                string fileID = paramsExplode[0];
                string timer = paramsExplode[1];
                string phase = paramsExplode[2];

                string realClockTimer = Utility.CommonHelper.TimerServerCompare(fileID, phase, databasename);


                if (phase != "ALT")
                {
                    objJobsFiles.ID = Utility.CommonHelper.GetDBInt(fileID);
                    objJobsFiles.LastState = phase;
                    objJobsFiles.LastStateTimer = realClockTimer;

                }

                var jsonResult3 = Json(new
                {
                    data = realClockTimer
                });
                return jsonResult3;
            }
            else if (State == "FormsFlag")
            {
                objJobsFiles.ID = Utility.CommonHelper.GetDBInt(Params);
                objJobsFiles.databasename = databasename;
                objJobsFiles.UpdateJobsFilesFormFlagByID();
            }
            else if (State == "LinkingFlag")
            {
                objJobsFiles.ID = Utility.CommonHelper.GetDBInt(Params);
                objJobsFiles.databasename = databasename;
                objJobsFiles.UpdateJobsLinkingFlagFlagByID();
            }
            else if (State == "ReviewFlag")
            {
                objJobsFiles.ID = Utility.CommonHelper.GetDBInt(Params);
                objJobsFiles.databasename = databasename;
                objJobsFiles.UpdateJobsReviewFlagByID();
            }
            else if (State == "Assign" || State == "ToBeReviewed" || State == "ToBeFinalized" || State == "ToBeQualityControlled" || State == "Complete")
            {
                int nextActionFileID = Utility.CommonHelper.GetDBInt(Params);
                string status = "";
                if (State == "Assign")
                {
                    status = "TAGGING";
                }
                if (State == "ToBeReviewed")
                {
                    status = "REVIEW";
                }
                else if (State == "ToBeFinalized")
                {
                    status = "FINAL";
                }
                else if (State == "ToBeQualityControlled")
                {
                    status = "QC";
                }
                else if (State == "Complete")
                {
                    status = "TOBEDELIVERED";
                }

                // Update the NextState in jobs_files_checkouts
                ObjJobsFilesCheckouts.FileID = nextActionFileID;
                ObjJobsFilesCheckouts.NextState = status;
                ObjJobsFilesCheckouts.databasename = databasename;
                ObjJobsFilesCheckouts.UpdateJobsNextStateByFileID();

                // Getting the old Status
                objJobsFiles.databasename = databasename;
                objJobsFiles = objJobsFiles.GetJobFilesByID(nextActionFileID);
                string prevStatus = objJobsFiles.Status;
                string returnData = "";
                if (objJobsFiles.Status != status)
                {
                    FilePosition(status, objJobsFiles.Status, objJobsFiles.Priority, nextActionFileID, objJobsFiles.Deadline,flag);
                }

                int fix = 0;
                int reviewFix = 0;
                int finalFix = 0;
                if (prevStatus != "TAGGING" && status == "TAGGING")
                {
                    fix = 1;
                }

                if ((prevStatus == "FINAL" || prevStatus == "QC") && status == "REVIEW")
                {
                    reviewFix = 1;
                    objJobsFiles = new JobsFiles();
                    objJobsFiles.ID = nextActionFileID;
                    objJobsFiles.ReviewFix = reviewFix;
                    objJobsFiles.databasename = databasename;
                    objJobsFiles.UpdateJobsReviewFixByID();
                }

                if (prevStatus == "QC" && status == "FINAL")
                {
                    finalFix = 1;
                    objJobsFiles = new JobsFiles();
                    objJobsFiles.ID = nextActionFileID;
                    objJobsFiles.FinalFix = reviewFix;
                    objJobsFiles.databasename = databasename;
                    objJobsFiles.UpdateJobsFinalFixByID();
                }

                if (State == "Assign")
                {
                    objJobsFiles = new JobsFiles();
                    
                    objJobsFiles.databasename = databasename;
                    objJobsFiles = objJobsFiles.GetJobFilesByID(nextActionFileID);
                    string assignedTo = objJobsFiles.LastTagger;

                    objJobsFiles = new JobsFiles();
                    objJobsFiles.AssignedTo = assignedTo;
                    objJobsFiles.LastTagger = assignedTo;
                    objJobsFiles.ID = nextActionFileID;
                    objJobsFiles.Fix = fix;
                    objJobsFiles.databasename = databasename;
                    objJobsFiles.UpdateJobsLastTaggerByID();
                }
                else
                {
                    objJobsFiles = new JobsFiles();
                    objJobsFiles.Status = status;
                    objJobsFiles.AssignedTo = "";
                    objJobsFiles.ID = nextActionFileID;
                    objJobsFiles.Fix = fix;
                    objJobsFiles.databasename = databasename;
                    objJobsFiles.UpdateJobsAssignedToByID();
                }

                if (status == "TOBEDELIVERED")
                {
                    int jobStateDeliverFlag = 0;
                    string jobIDOfDoc = "";

                    Jobs objJobs = new Jobs();
                    objJobs.databasename = databasename;
                    objJobs = objJobs.GetJobsByJobFilesID(nextActionFileID);
                    int jobID = objJobs.ID;
                    if (objJobs.JobType == "MULTI")
                    {
                        // If all the files are in to be delivered state
                        // Checking if all the other files are in ToBeDelivered state for the current job or not
                        objJobsFiles = new JobsFiles();
                        objJobsFiles.ID = jobID;
                        objJobsFiles.databasename = databasename;
                        bool isDelivery = objJobsFiles.CheckReadyForDelivery();
                        if (!isDelivery)
                        {

                            List<string> deliveryArray = new List<string>(); ;
                            int deliveryStateCount = 0;
                            // Checking if any batch of file is ready to be delivered and has not yet been delivered atleast once

                            objJobsFiles = new JobsFiles();
                            objJobsFiles.JobID = jobID;
                            objJobsFiles.databasename = databasename;
                            List<JobsFiles> jobsFilesList = objJobsFiles.GetJobFileListIDByJobID();


                            foreach (JobsFiles row in jobsFilesList)
                            {

                                if (!deliveryArray.Contains(row.Deadline))
                                {

                                    if (deliveryArray.Count != 0 && deliveryStateCount == 0)
                                    {
                                        objJobs = new Jobs();
                                        objJobs.JobID = jobID;
                                        objJobs.databasename = databasename;
                                        objJobs.UpdateJobForDelivery();
                                        break;
                                    }
                                    deliveryArray.Add(row.Deadline);
                                    deliveryStateCount = 0;
                                    if (row.Status == "TOBEDELIVERED" && row.DeliveryCount == 0)
                                    {

                                    }
                                    else
                                    {
                                        deliveryStateCount++;
                                    }
                                }
                                else
                                {

                                    if (row.Status == "TOBEDELIVERED" && row.DeliveryCount == 0)
                                    {

                                    }
                                    else
                                    {
                                        deliveryStateCount++;
                                    }
                                }
                            }
                            if (deliveryArray.Count != 0 && deliveryStateCount == 0)
                            {
                                objJobs = new Jobs();
                                objJobs.JobID = jobID;
                                objJobs.databasename = databasename;
                                objJobs.UpdateJobForDelivery();
                            }

                        }
                        else
                        {
                            objJobs = new Jobs();
                            objJobs.JobID = jobID;
                            objJobs.databasename = databasename;
                            objJobs.UpdateJobForDelivery();
                        }

                    }
                    else
                    {
                        objJobsFiles = new JobsFiles();
                        objJobsFiles.ID = jobID;
                        bool isDelivery = objJobsFiles.CheckReadyForDelivery();
                        if (isDelivery)
                        {
                            objJobs = new Jobs();
                            objJobs.JobID = jobID;
                            
                            objJobs.databasename = databasename;
                            objJobs.UpdateJobForDelivery();
                        }
                    }
                }
            }
            else if (State == "UpdateAltText")
            {
                string[] paramsExplode = Params.Split("|");
                objJobsFiles = new JobsFiles();
                objJobsFiles.ID = Utility.CommonHelper.GetDBInt(paramsExplode[0].ToString());
                objJobsFiles.AltTxt = Utility.CommonHelper.GetDBInt(paramsExplode[1].ToString());
                objJobsFiles.databasename = databasename;
                objJobsFiles.UpdateAltText();
            }
            else if (State == "TimerCompareStart")
            {
                string[] paramsExplode = Params.Split("|");
                string fileID = paramsExplode[0];
                string phase = paramsExplode[1];
                string storedTimer = paramsExplode[2];
                string currentTimeCompare = "";
                int tableRowID = 0;
                // Get the TimeCompare current stored value

                ObjJobsFilesCheckouts = new JobsFilesCheckouts();
                ObjJobsFilesCheckouts.FileID = Utility.CommonHelper.GetDBInt(fileID);
                ObjJobsFilesCheckouts.State = phase;
                ObjJobsFilesCheckouts.databasename = databasename;
                ObjJobsFilesCheckouts = ObjJobsFilesCheckouts.GetJobsFilesCheckoutsByFileIDState();
                tableRowID = ObjJobsFilesCheckouts.ID;
                currentTimeCompare = ObjJobsFilesCheckouts.TimerCompare.ToString();

                // Get the time
                var format = "yyyy-MM-dd H:mm:ss";
                var stringDate = DateTime.Now.ToString(format);
                int datetime2 = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));

                // Store the current timestamp in the table
                try
                {
                    ObjJobsFilesCheckouts = new JobsFilesCheckouts();
                    ObjJobsFilesCheckouts.ID = tableRowID;
                    ObjJobsFilesCheckouts.Timer = storedTimer;
                    ObjJobsFilesCheckouts.TimerCompare = datetime2;
                    ObjJobsFilesCheckouts.databasename = databasename;
                    ObjJobsFilesCheckouts.UpdateJobsFilesCheckOutTimerTimerCompareById();
                }
                catch (Exception e) { }


                if (currentTimeCompare == "" || currentTimeCompare == "00:00:00")
                {

                }
                else
                {

                }
            }

            else if (State == "TimerCompareStop")
            {
                string[] paramsExplode = Params.Split(" | ");
                string fileID = paramsExplode[0];
                string phase = paramsExplode[1];
                string storedTimer = paramsExplode[2];
                string currentTimeCompare = "";
                string tableRowID = "";

                string realClockTimer = Utility.CommonHelper.TimerServerCompare(fileID, phase, databasename);


                if (phase != "ALT")
                {
                    try
                    {
                        objJobsFiles = new JobsFiles();
                        objJobsFiles.ID = Utility.CommonHelper.GetDBInt(fileID);
                        objJobsFiles.LastState = phase;
                        objJobsFiles.LastStateTimer = realClockTimer;
                        objJobsFiles.databasename = databasename;
                        objJobsFiles.UpdateJobsFilesLastStateLastStateTimerByID();
                    }
                    catch (Exception e) { }
                }

            }

            else if (State == "TimerStartedState")
            {

                string[] paramsExplode = Params.Split("|");

                string jobID = paramsExplode[0];
                string fileID = paramsExplode[1];
                string fileState = paramsExplode[2];
                string fixFile = paramsExplode[3];
                string lastTimerState = paramsExplode[4];
                int userID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID")); ;
                var format = "yyyy-MM-dd H:mm:ss";
                var stringDate = DateTime.Now.ToString(format);
                int datetime2 = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));

                Int64 tStart = datetime2;
                Int64 tStop = datetime2;

                AllTimers objallTimers = new AllTimers();
                objallTimers.JobID = Utility.CommonHelper.GetDBInt(jobID);
                objallTimers.FileID = Utility.CommonHelper.GetDBInt(fileID);
                objallTimers.State = fileState;
                objallTimers.UserID = userID;
                objallTimers.databasename = databasename;
                objallTimers = objallTimers.GetAllTimersByIds();
                string lastTimerTotal = "00:00:00";
                if (objallTimers != null)
                {
                    lastTimerTotal = objallTimers.TotalTimerNow;
                }

                if (lastTimerTotal == "")
                {
                    lastTimerTotal = "00:00:00";
                }
                string lastInsertID = string.Empty;
                try
                {
                    objallTimers = new AllTimers();
                    objallTimers.JobID = Utility.CommonHelper.GetDBInt(jobID);
                    objallTimers.FileID = Utility.CommonHelper.GetDBInt(fileID);
                    objallTimers.State = fileState;
                    objallTimers.UserID = userID;
                    objallTimers.TStart = tStart;
                    objallTimers.TStop = tStop;
                    objallTimers.Fix = Utility.CommonHelper.GetDBInt(fixFile);
                    objallTimers.TotalTimerNow = lastTimerTotal;
                    objallTimers.databasename = databasename;
                    lastInsertID = objallTimers.CreateAllTimers();
                }
                catch (Exception e)
                {

                }
                var jsonResult1 = Json(new
                {
                    data = lastInsertID
                });
                return jsonResult1;

            }

            else if (State == "TimerCompareState")
            {

                string[] paramsExplode = Params.Split("|");
                string jobID = paramsExplode[0];
                string fileID = paramsExplode[1];
                string fileState = paramsExplode[2];
                string fixFile = paramsExplode[3];
                int userID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));

                Int64 tStart = 0;
                int allTimersID = 0;
                string timerNowInstance = string.Empty;

                AllTimers objallTimers = new AllTimers();
                objallTimers.JobID = Utility.CommonHelper.GetDBInt(jobID);
                objallTimers.FileID = Utility.CommonHelper.GetDBInt(fileID);
                objallTimers.State = fileState;
                objallTimers.UserID = userID;
                objallTimers.databasename = databasename;
                objallTimers = objallTimers.GetAllTimersByIds();
                bool isExist = false;
                if (objallTimers != null)
                {
                    allTimersID = objallTimers.ID;
                    tStart = objallTimers.TStart;
                    timerNowInstance = objallTimers.TimerNow;
                    isExist = true;


                }

                var format = "yyyy-MM-dd H:mm:ss";
                var stringDate = DateTime.Now.ToString(format);
                int datetime2 = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));

                // Current Timestamp
                Int64 tStop = datetime2;

                int totalSeconds = Utility.CommonHelper.GetDBInt(tStop - tStart);
                double totalHours = totalSeconds / 3600;
                totalHours = Math.Round(totalHours, 2);
                string realClockTimer = Utility.CommonHelper.TimerCompareStateServer(Utility.CommonHelper.GetDBInt(tStart), Utility.CommonHelper.GetDBInt(tStop));


                try
                {
                    objallTimers = new AllTimers();
                    objallTimers.ID = allTimersID;
                    objallTimers.TStop = tStop;
                    objallTimers.TimerNow = realClockTimer;
                    objallTimers.TotalSeconds = totalSeconds;
                    objallTimers.TotalHours = totalHours;
                    objallTimers.databasename = databasename;
                    objallTimers.UpdateAllTimers();

                }
                catch (Exception e)
                {
                }


                totalSeconds = 0;
                int timerSeconds = 0;
                if (isExist)
                {
                    timerSeconds = Utility.CommonHelper.TimeFormatToSeconds(timerNowInstance);
                    totalSeconds = totalSeconds + timerSeconds;
                }

                // Seconds to time format
                string realClockTimerTotal = Utility.CommonHelper.SecondsToTimeFormat(totalSeconds);

                try
                {
                    objallTimers = new AllTimers();
                    objallTimers.TotalTimerNow = realClockTimerTotal;
                    objallTimers.ID = allTimersID;
                    objallTimers.databasename = databasename;
                    objallTimers.UpdateTotalTimerNow();
                }
                catch (Exception e)
                { }
                var jsonResult2 = Json(new
                {
                    data = realClockTimerTotal
                });
                return jsonResult2;
            }

            else if (State == "DownloadFileCheckoutScreen")
            {
                //    $params = $_POST["Params"];
                //    $fileID = $params;
                //    $response = $erpObj->DownloadFileCheckoutScreen($fileID);
                //echo $response;
            }
            var jsonResult = Json(new
            {
                message = "success"
            });
            return jsonResult;
        }
        [LoginAuthorized]
        public string FilePosition(string newStatus, string oldStatus, Int64 oldPriority, int fileId, string deadline,string flag)
        {
            string retval = string.Empty;
            string prevStatus = oldStatus;
            int fileID = fileId;
            if (prevStatus != newStatus)
            {
                int maxP = 0;
                int filePosFlag = 1;
                JobsFiles objJobsFiles = new JobsFiles();
                var databasename = Utility.CommonHelper.Getabledocs(flag);
                objJobsFiles.databasename = databasename;
                retval = objJobsFiles.UpdateFilePosition(newStatus, oldStatus, oldPriority, fileId, deadline);
            }
            return retval;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult Upload(int jobID,int ID,string FileType,string uploadFlag,string FileTypeReference,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            List<UploadFiles> uploadFilesList = new List<UploadFiles>();
            //var post = Request.Form;
            //var uploadFlag = post["uploadFlag"];
            //int jobID = Convert.ToInt32(post["JobID"]);
            int fileID = ID;//(post["ID"] == "") ? 0 : Convert.ToInt32(post["ID"]);
            //var FileType = post["FileType"];
            //string FileTypeReference= post["FileTypeReference"];
            string qcType = "";
            string state = "";
            Utility.UploadFile uploadFile = new Utility.UploadFile(this.webHostEnvironment);
            var files = HttpContext.Request.Form.Files;
            if (uploadFlag == "wcFile" || uploadFlag == "pacFile" || uploadFlag == "unsecuredFile" || uploadFlag == "securedFile")
            {
                if (uploadFlag == "wcFile")
                {
                    qcType = "1";
                }
                else if (uploadFlag == "pacFile")
                {
                    qcType = "2";
                }
                else if (uploadFlag == "unsecuredFile")
                {
                    qcType = "3";
                }
                else if (uploadFlag == "securedFile")
                {
                    qcType = "4";
                }
                state = "QC";
                uploadFilesList = uploadFile.AddJobFileVersion(databasename,jobID, fileID, state, qcType, files);
            }
            else if (uploadFlag == "referenceFile")
            {
                state = "REFERENCE";
                uploadFilesList=  uploadFile.AddJobFileVersion(databasename,jobID, fileID, state, qcType, files);
            }
            else if (uploadFlag == "error")
            {
                uploadFilesList =  uploadFile.ErrorRequest(jobID, fileID, files);
            }
            else if (uploadFlag == "otherFile")
            {
                state = "OTHER";
                uploadFilesList = uploadFile.AddJobFileVersion(databasename,jobID, fileID, state, qcType, files);
            }
            else if (uploadFlag == "p4File")
            {
                state = "FINAL";
                uploadFilesList = uploadFile.AddJobFileVersion(databasename,jobID, fileID, state, qcType, files);
            }
            else {
                if (string.IsNullOrEmpty(FileType))
                {
                    if (FileTypeReference == "REFERENCE")
                    {
                        FileTypeReference = "REFERENCE";
                    }
                    else
                    {
                        FileTypeReference = "SOURCE";
                    }
                }
                else {
                    state = FileType;
                }
               
                // if the job ID has been passed, then we just need to add a file version
                if (fileID > 0)
                {
                    uploadFilesList = uploadFile.AddJobFileVersion(databasename,jobID, fileID, state, FileTypeReference, files);
                }
                else if(FileTypeReference != "REFERENCE" && FileTypeReference != "OTHER")  {
                    
                    uploadFile.AddFile2Job(databasename,jobID, files);
                }
            }

            return Json(new { uploadfile = uploadFilesList });

        }

        [HttpPost, LoginAuthorized]
        public JsonResult Updatecsrnotes(string code, string notes,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ClientSpecificRequirements clientSpecificRequirements = new ClientSpecificRequirements();
            clientSpecificRequirements.client_code = code;
            clientSpecificRequirements.notes = notes;
            clientSpecificRequirements.databasename = databasename;
            clientSpecificRequirements.UpdateNote();

            return Json(new { data = "Sucess" });
        }
    }
}
