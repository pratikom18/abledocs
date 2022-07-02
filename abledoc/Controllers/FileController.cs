using abledoc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace abledoc.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public FileController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

       // //[AuthorizedAction]
       [LoginAuthorized]
        public IActionResult Index(int id,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;
           
            Models.File objFile = new Models.File();
            
            objFile.databasename = databasename;

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.databasename = databasename;
            objJobsFiles = objJobsFiles.GetJobFilesByID(id);
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;

            Jobs objJobs = new Jobs();
            objJobs.databasename = databasename;
            objJobs = objJobs.GetJobsById(objJobsFiles.JobID);

            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            List<JobsFilesVersions> JobsFilesVersionsList = objJobsFilesVersions.GetUserListByJobID(objJobsFiles.JobID);

            List<JobsFilesVersions> FileVersionList = new List<JobsFilesVersions>();
            objJobsFilesVersions.databasename = databasename;
            FileVersionList = objJobsFilesVersions.GetFileVersionByFileID(objJobsFiles.ID);

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.databasename = databasename;
            List<JobsFilesCheckouts> JobsFilesCheckoutsList = objJobsFilesCheckouts.GetUserListByJobID(objJobsFiles.JobID);

            Dictionary<int, string> UserList = new Dictionary<int, string>();
            if (JobsFilesVersionsList != null)
            {
                foreach (JobsFilesVersions item in JobsFilesVersionsList)
                {
                    if (!UserList.ContainsKey(item.AssignedTo))
                    {
                        UserList.Add(item.AssignedTo, item.FullName);
                    }
                }
            }

            if (JobsFilesCheckoutsList != null)
            {
                foreach (JobsFilesCheckouts item in JobsFilesCheckoutsList)
                {
                    if (!UserList.ContainsKey(item.UserID))
                    {
                        UserList.Add(item.UserID, item.FullName);
                    }
                }
            }

            AltTexts objAltTexts = new AltTexts();
            List<AltTexts> AltTextList = new List<AltTexts>();
            objAltTexts.databasename = databasename;
            AltTextList = objAltTexts.GetAllAltTextListByFileId(objJobsFiles.ID);

            List<JobsFiles> jobsfilesList = new List<JobsFiles>();
           
            objJobsFiles.databasename = databasename;
            jobsfilesList = objJobsFiles.GetJobFileListIDByJobID(Utility.CommonHelper.GetDBInt(objJobsFiles.JobID));
            int[] arrays = jobsfilesList.Select(x => x.ID).ToArray();

            List<JobsFiles> jobfilesTreeList = new List<JobsFiles>();
            
            objJobsFiles.databasename = databasename;
            jobfilesTreeList = objJobsFiles.GetJobfilesFileTree(arrays);

            List<JobsFilesVersions> jobfileversionTreeList = new List<JobsFilesVersions>();
            objJobsFilesVersions.databasename = databasename;
            jobfileversionTreeList = objJobsFilesVersions.GetJobfilesFileTree(arrays);

            List<JobsFilesVersions> ReferenceList = new List<JobsFilesVersions>();
            objJobsFilesVersions.databasename = databasename;
            ReferenceList = objJobsFilesVersions.GetReferenceFile(objJobsFiles.ID);

            

            ErrorReport objErrorReport = new ErrorReport();
            List<ErrorReport> ErrorReportList = new List<ErrorReport>();
            objErrorReport.databasename = databasename;
            ErrorReportList = objErrorReport.GetErrorReportList(objJobsFiles.ID);

            JobsFilesReviews jobsFilesReviews = new JobsFilesReviews();
            if (objJobsFiles.Status == "TOBEDELIVERED")
            {
                objFile.JobsFilesCommentsList = null;
            }
            else {
                jobsFilesReviews.databasename = databasename;
                objFile.JobsFilesCommentsList = jobsFilesReviews.GetCommentsFiles(Utility.CommonHelper.GetDBInt(objJobsFiles.JobID), objJobsFiles.ID, objJobsFiles.Status);
            }
            
            ViewBag.AltTextList = AltTextList;
            ViewBag.UserList = UserList;
            ViewBag.FileVersionList = FileVersionList;
            ViewBag.ID = objJobsFiles.ID;
            objFile.jobsFiles = objJobsFiles;
            objFile.jobs = objJobs;
            objFile.JobFileTree = jobfilesTreeList;
            objFile.JobsFilesVersionsTree = jobfileversionTreeList;
            objFile.ReferenceFile = ReferenceList;
            //objFile.QCLCList = JobsFilesQCList;
            ////objFile.ConversationsList = ConversationsList;
            //objFile.FLCList = JobsFilesFinalList;
            //objFile.RLCList = JobsFilesReviewsList;
            objFile.ErrorReportList = ErrorReportList;
            objFile.flag = flag;
            return View(objFile);
        }

        [HttpPost, LoginAuthorized]
        public IActionResult EDIT(Models.File model)
        {
            var databasename = Utility.CommonHelper.Getabledocs(model.flag);

            JobsFiles oldJobsFiles = new JobsFiles();
            
            oldJobsFiles.databasename = databasename;
            oldJobsFiles = oldJobsFiles.GetJobFilesByID(model.jobsFiles.ID);
            string currentStatus = string.Empty;
            string AssignedTo = "";
            string AltTxt = "";
            foreach (string key in Request.Form.Keys)
            {
                if (key == "Status")
                {
                    currentStatus = Request.Form[key].ToString();
                }
                else if (key == "AssignedTo")
                {
                    AssignedTo = Request.Form[key].ToString();
                }
                else if (key == "AltTxt")
                {
                    AltTxt = Request.Form[key].ToString();
                }
            }

            if (oldJobsFiles.Status != currentStatus)
            {
                FilePosition(currentStatus, oldJobsFiles.Status, oldJobsFiles.Priority, model.jobsFiles.ID, model.jobsFiles.Deadline,model.flag);
            }
            JobsFiles updateJobsFiles = new JobsFiles();
            updateJobsFiles = model.jobsFiles;
            updateJobsFiles.Status = currentStatus;
            updateJobsFiles.AssignedTo = AssignedTo;
            updateJobsFiles.AltTxt = Utility.CommonHelper.GetDBInt(AltTxt);
            updateJobsFiles.databasename = databasename;
            updateJobsFiles.UpdateJobFilesByID();
            if (model.jobsFiles.Status == "TOBEDELIVERED")
            {
                int jobID = 0;
                int jobStateDeliverFlag = 0;

                Jobs objJobs = new Jobs();
                
                objJobs.databasename = databasename;
                objJobs = objJobs.GetJobsByJobFilesID(model.jobsFiles.ID);

                if (objJobs.JobType == "MULTI")
                {
                    List<JobsFiles> objList = new List<JobsFiles>();
                    oldJobsFiles.databasename = databasename;
                    objList = oldJobsFiles.GetJobFileListIDByID(model.jobsFiles.ID, "");

                    foreach (JobsFiles item in objList)
                    {
                        jobID = abledoc.Utility.CommonHelper.GetDBInt(item.JobID);
                        if (item.Status == "TOBEDELIVERED")
                        {
                        }
                        else
                        {
                            jobStateDeliverFlag = 1;
                        }
                    }

                    if (jobStateDeliverFlag == 0)
                    {
                        Jobs objJobs1 = new Jobs();
                        objJobs1.databasename = databasename;
                        objJobs1.UpdateJobsStatusByID(jobID);
                    }
                    else
                    {
                        List<string> deliveryArray = new List<string>();
                        int deliveryStateCount = 0;

                        Jobs objJobs1 = new Jobs();

                        List<JobsFiles> objList1 = new List<JobsFiles>();
                        oldJobsFiles.databasename = databasename;
                        objList1 = oldJobsFiles.GetJobFileListIDByID(model.jobsFiles.ID, "OrderBy");
                        foreach (JobsFiles item in objList1)
                        {
                            if (!deliveryArray.Contains(item.Deadline))
                            {
                                if (deliveryArray.Count != 0 && deliveryStateCount == 0)
                                {
                                    objJobs1.databasename = databasename;
                                    objJobs1.UpdateJobsStatusByID(abledoc.Utility.CommonHelper.GetDBInt(item.JobID));
                                    break;
                                }
                                deliveryArray.Add(item.Deadline);
                                deliveryStateCount = 0;

                                if (item.Status == "TOBEDELIVERED" && item.DeliveryCount == 0)
                                {

                                }
                                else
                                {
                                    deliveryStateCount++;
                                }

                            }
                            else
                            {
                                if (item.Status == "TOBEDELIVERED" && item.DeliveryCount == 0)
                                {

                                }
                                else
                                {
                                    deliveryStateCount++;
                                }
                            }
                        }

                    }
                }
                else
                {
                    List<JobsFiles> objList1 = new List<JobsFiles>();
                    oldJobsFiles.databasename = databasename;
                    objList1 = oldJobsFiles.GetJobFileListIDByID(model.jobsFiles.ID, "Delete");

                    foreach (JobsFiles item in objList1)
                    {
                        jobID = abledoc.Utility.CommonHelper.GetDBInt(item.JobID);
                        if (item.Status == "TOBEDELIVERED")
                        {
                        }
                        else
                        {
                            jobStateDeliverFlag = 1;
                        }
                    }

                    if (jobStateDeliverFlag == 0)
                    {
                        Jobs objJobs1 = new Jobs();
                        objJobs1.databasename = databasename;
                        objJobs1.UpdateJobsStatusByID(jobID);
                    }

                }
            }
            //return RedirectToAction("Index", new { id = model.jobsFiles.ID.ToString() });
            return Redirect("/file?ID="+model.jobsFiles.ID.ToString()+"&flag="+model.flag);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult InsertCheckout(int FileID, int Checkout_PageNumber, string State,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
           
            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = FileID;
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Checkout = DateTime.Now;
            objJobsFilesCheckouts.Checkout_PageNumber = Checkout_PageNumber;
            objJobsFilesCheckouts.State = State;
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.CreateJobsFilesCheckouts();

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.CurrentCheckout = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFiles.ID = FileID;
            objJobsFiles.databasename = databasename;
            objJobsFiles.UpdateCurrentCheckout();

            var jsonResult = Json(new

            {
                message = "Success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult InsertConversions(int FileID, int JobID, string Conversation,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            Conversations objConversations = new Conversations();
            objConversations.FileID = FileID;
            objConversations.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objConversations.JobID = JobID;
            objConversations.Conversation = Conversation;
            objConversations.databasename = databasename;
            objConversations.Insert();

            List<Conversations> ConversationsList = new List<Conversations>();
            objConversations.databasename = databasename;
            ConversationsList = objConversations.GetConversationsList(FileID);

            var result = from c in ConversationsList
                         select new[] {
                             c.ID.ToString(),
                             c.Conversation == null ? "" : c.Conversation,
                             c.FullName == null? "":c.FullName,
                             c.LastUpdated == null ? "" :c.LastUpdated.ToString(),
                         };

            var jsonResult = Json(new
            {
                aaData = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult InsertClientinstruction(int FileID, int JobID, string Clientinstruction, string Status,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            Clientinstruction objClientinstruction = new Clientinstruction();
            objClientinstruction.FileID = FileID;
            objClientinstruction.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objClientinstruction.JobID = JobID;
            objClientinstruction.Status = Status;
            objClientinstruction.Clientinstructions = Clientinstruction;
            objClientinstruction.databasename = databasename;
            objClientinstruction.Insert();

            List<Clientinstruction> ClientinstructionList = new List<Clientinstruction>();
            objClientinstruction.databasename = databasename;
            ClientinstructionList = objClientinstruction.GetClientinstructionList(FileID);

            var result = from c in ClientinstructionList
                         select new[] {
                             c.ID.ToString(),
                             c.Clientinstructions == null ? "" : c.Clientinstructions,
                             c.FullName == null? "":c.FullName,
                             c.LastUpdated == null ? "" :c.LastUpdated.ToString(),
                             c.Status==null?"" :c.Status,
                         };

            var jsonResult = Json(new
            {
                aaData = result
            });
           
            return jsonResult;

        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult Conversations(int id,string flag)
        {
            ViewBag.ID = id;
            ViewBag.Open = "true";
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;
            return PartialView("~/views/file/_Conversation.cshtml");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult FileVersions(int id,string status,string flag)
        {
            ViewBag.ID = id;
            ViewBag.Status = status;
            ViewBag.flag = flag;
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;
            return PartialView("~/views/file/_FileVersions.cshtml");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult ClientInstruction(int id,string flag)
        {
            ViewBag.ID = id;
            ViewBag.Open = "true";
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;
            return PartialView("~/views/file/_ClientInstuction.cshtml");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult SendToFile(int id, string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.databasename = databasename;
            objJobsFiles = objJobsFiles.GetJobFilesByID(id);
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;
            objJobsFiles.flag = flag;
            return PartialView("_SendTo", objJobsFiles);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult ConfirmStatus(string oldStatus,string newStatus, string oldStatus1, string newStatus1)
        {
            ViewBag.oldStatus = oldStatus;
            ViewBag.newStatus = newStatus;
            ViewBag.oldStatus1 = oldStatus1.Replace("-"," ");
            ViewBag.newStatus1 = newStatus1.Replace("-", " ");

            return PartialView("_ConfirmStatus");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult DragDropFile(int id,string type)
        {
            ViewBag.Type = type;
            return PartialView("_DragDropFile");
        }

        [HttpPost, LoginAuthorized]
        public IActionResult sendto(JobsFiles model)
        {
            var databasename = Utility.CommonHelper.Getabledocs(model.flag);

            JobsFiles oldJobsFiles = new JobsFiles();
            
            oldJobsFiles.databasename = databasename;
            oldJobsFiles = oldJobsFiles.GetJobFilesByID(model.ID);

            JobsFiles objJobsFiles1 = new JobsFiles();
            string currentStatus = string.Empty;
            foreach (string key in Request.Form.Keys)
            {
                if (key == "SendTo")
                {
                    currentStatus = Request.Form[key].ToString();
                    objJobsFiles1.Status = Request.Form[key].ToString();
                    objJobsFiles1.ID = model.ID;
                    
                    objJobsFiles1.databasename = databasename;
                    objJobsFiles1.UpdateStatus();
                }
            }

            if (oldJobsFiles.Status != currentStatus)
            {
                FilePosition(currentStatus, oldJobsFiles.Status, oldJobsFiles.Priority, model.ID, oldJobsFiles.Deadline,model.flag);
            }

            //return RedirectToAction("Index", new { id = model.ID.ToString() });
            return Redirect("/file?ID=" + model.ID.ToString()+"&flag="+model.flag);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult LinkPopupFile(int jobID, int fileID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            List<JobsFilesVersions> JobsFilesVersionsList = new List<JobsFilesVersions>();
            objJobsFilesVersions.databasename = databasename;
            JobsFilesVersionsList = objJobsFilesVersions.GetLinkPopupFile(jobID, fileID);


            ViewBag.LinkPopupFileList = JobsFilesVersionsList;
            ViewBag.JobID = jobID;
            ViewBag.FileID = fileID;
            ViewBag.flag = flag;
            return PartialView("_LinkPopupFile");
        }

        public void FilePosition(string currentStatus, string OldStatus, long OldPriority, int ID, string OldDeadline,string flag)
        {
            JobsFiles objJobsFiles = new JobsFiles();
            if (currentStatus != OldStatus)
            {
                var databasename = Utility.CommonHelper.Getabledocs(flag);
                objJobsFiles.databasename = databasename;
                objJobsFiles.UpdateFilePosition(currentStatus, OldStatus, OldPriority, ID, OldDeadline);
            }

        }

        [HttpGet, LoginAuthorized]
        public JsonResult FileTree(int JobID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFiles objJobsFiles = new JobsFiles();
            List<JobsFiles> jobsfilesList = new List<JobsFiles>();
            objJobsFiles.databasename = databasename;
            jobsfilesList = objJobsFiles.GetJobFileListIDByJobID(Utility.CommonHelper.GetDBInt(JobID));
            int[] arrays = jobsfilesList.Select(x => x.ID).ToArray();

            List<JobsFiles> jobfilesTreeList = new List<JobsFiles>();
            objJobsFiles.databasename = databasename;
            jobfilesTreeList = objJobsFiles.GetJobfilesFileTree(arrays);

            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            List<JobsFilesVersions> jobfileversionTreeList = new List<JobsFilesVersions>();
            objJobsFilesVersions.databasename = databasename;
            jobfileversionTreeList = objJobsFilesVersions.GetJobfilesFileTree(arrays);
            var FilteList = from c in jobfilesTreeList
                            select new[] {
                             c.ID.ToString(),
                             c.Filename == null ? "" : c.Filename
                         };

            var TreeList = from c in jobfileversionTreeList
                           select new[] {
                             c.ID.ToString(),
                             c.FileID.ToString(),
                             c.Filename == null ? "" : c.Filename,
                              c.State == null ? "" : c.State,
                               c.QCType == null ? "" : c.QCType.ToString()

                         };
            var jsonResult = Json(new
            {
                FileList = FilteList,
                TreeList= TreeList
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult InsertLinkPopupFile(int FileID, int JobID, string RefFileID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            ReferenceLinking objReferenceLinking = new ReferenceLinking();
            objReferenceLinking.databasename = databasename;
            objReferenceLinking.Insert(JobID,FileID,RefFileID);

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult UpdateTagging(int JobID, string TaggingInstructions,string flag)
        {
            Jobs objJobs = new Jobs();
            var databasename1 = Utility.CommonHelper.Getabledocs(flag);
            objJobs.databasename = databasename1;
            objJobs.UpdateTagging(JobID, TaggingInstructions);

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult DeleteFileVersion(int ID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            objJobsFilesVersions.DeleteFileVersion(ID);

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }
     
        [HttpPost, LoginAuthorized]
        public ActionResult ProcessRequest(int jobID,int fileID,string state= "SOURCE",string qcType= "",string flag="0")
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            Utility.UploadFile uploadFile = new Utility.UploadFile(this.webHostEnvironment);
            var files = HttpContext.Request.Form.Files;
            string name = string.Empty;
            foreach (var pdf in files)
            {
                name = pdf.FileName;
            }
                
            string path = uploadFile.ProcessRequest(databasename,jobID, fileID, state, qcType, files);
            var data = new
            {
                name = name,
                path = path,
                date= DateTime.Today
            };
            return Json(data);
        }

        [HttpPost, LoginAuthorized]
        public ActionResult ZipPDFFiles(string ID,string IDS,string flag)
        {
            //string i = Request.Form["ID"];
            //string j = Request.Form["IDS"];
            return downloadFile(ID, IDS,flag);
        }

        public FileResult downloadFile(string ID, string IDS,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
           
            JobsFilesVersions jobsFilesVersions = new JobsFilesVersions();
            List<JobsFilesVersions> FileVersionList = new List<JobsFilesVersions>();
            jobsFilesVersions.databasename = databasename;
            if (IDS == "all")
            {
                FileVersionList = jobsFilesVersions.GetFileVersionByFileID(Utility.CommonHelper.GetDBInt(ID));
            }
            else
            {
                FileVersionList = jobsFilesVersions.GetFileVersionByID(IDS);
            }
            string folder = ID;
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (JobsFilesVersions item in FileVersionList)
                    {
                        try
                        {
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
    }
}
