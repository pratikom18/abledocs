using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class FinalFilesController : Controller
    {
        //[AuthorizedAction]
        [LoginAuthorized]
        public IActionResult Index(int ID)
        {
            ViewBag.State = "FINAL";
            var databasename = HttpContext.Session.GetString("Database");
            ViewBag.databasename = databasename;
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;

            FinalFile finalFile = new FinalFile();
            JobsFiles objJobsFiles = new JobsFiles();
            
            objJobsFiles.databasename = databasename;
            finalFile.jobsFiles = objJobsFiles.GetJobFilesByID(ID);

            Jobs objJobs = new Jobs();
            objJobs.databasename = databasename;
            finalFile.jobs = objJobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(finalFile.jobsFiles.JobID));
            if (finalFile.jobs.JobQuotedAs != "Timed")
            {
                if (finalFile.jobsFiles.PricePer == "Hour")
                {
                    finalFile.jobs.JobQuotedAs = finalFile.jobsFiles.Quantity.ToString() + " Hrs";
                }
                else
                {
                    finalFile.jobs.JobQuotedAs = "Timed";
                }
            }
            else
            {
                finalFile.jobs.JobQuotedAs = "Timed";
            }

            Clients objClients = new Clients();
            objClients.databasename = databasename;
            finalFile.clients = objClients.GetClientById(finalFile.jobs.ClientID);

            

            AllTimers objAllTimers = new AllTimers();
            objAllTimers.JobID = Utility.CommonHelper.GetDBInt(finalFile.jobsFiles.JobID);
            objAllTimers.FileID = ID;
            objAllTimers.State = "FINAL";
            objAllTimers.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objAllTimers.databasename = databasename;
            objAllTimers = objAllTimers.GetAllTimersByIds();
            finalFile.allTimers = objAllTimers;

            //if (finalFile.jobsFiles.Status == "FINAL" || finalFile.jobsFiles.Status == "QC")
            //{
            //    JobsFilesFinal objjobsFilesFinal = new JobsFilesFinal();
            //    finalFile.jobsFilesFinalList = objjobsFilesFinal.GetFLC(Utility.CommonHelper.GetDBInt(finalFile.jobsFiles.JobID), ID);
            //}

            //JobsFilesQC objJobsFilesQC = new JobsFilesQC();
            //finalFile.jobsFilesQCList = objJobsFilesQC.GetQCLC(Utility.CommonHelper.GetDBInt(finalFile.jobsFiles.JobID), ID);

            if (finalFile.jobsFiles.Status == "FINAL")
            {
                AltTexts objAltTexts = new AltTexts();
                objAltTexts.databasename = databasename;
                finalFile.altTextsList = objAltTexts.GetAltTextListByFileId(ID);

                JobsFilesReviews jobsFilesReviews = new JobsFilesReviews();
                jobsFilesReviews.databasename = databasename;
                finalFile.jobsFilesReviewsList = jobsFilesReviews.GetRLC(Utility.CommonHelper.GetDBInt(finalFile.jobsFiles.JobID), ID);
            }
            

            JobsFilesReviews objJobsFilesReviews = new JobsFilesReviews();
            objJobsFilesReviews.databasename = databasename;
            finalFile.jobsFilesCommentsList = objJobsFilesReviews.GetCommentsPhase4or5(Utility.CommonHelper.GetDBInt(finalFile.jobsFiles.JobID), ID, "finalfile", finalFile.jobsFiles.Status);


            return View(finalFile);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult FinalFileCheckin(int File, string Comments, int Job, string AltTextStatus, string Timer)
        {
            var databasename = HttpContext.Session.GetString("Database");

            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            objJobsFilesVersions = objJobsFilesVersions.GetJobsFilesVersionsLastData();
            string LastId = string.Empty;
            if (Comments != string.Empty)
            {
                JobsFilesFinal jobsFilesFinal = new JobsFilesFinal();
                jobsFilesFinal.VersionID = objJobsFilesVersions.ID;
                jobsFilesFinal.FinalizerID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                jobsFilesFinal.FileID = File;
                jobsFilesFinal.JobID = Job;
                jobsFilesFinal.LastUpdated = DateTime.Now;
                LastId = jobsFilesFinal.Insert();
            }

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = File;
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Timer = Timer;
            objJobsFilesCheckouts.State = "FINAL";
            objJobsFilesCheckouts.Checkin = DateTime.Now;
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.UpdateJobsFilesCheckOutByFileIdUserId();

            if (Comments != string.Empty)
            {
                JobsFilesFinal jobsFilesFinal = new JobsFilesFinal();
                jobsFilesFinal.VersionID = objJobsFilesVersions.ID;
                jobsFilesFinal.ID = Utility.CommonHelper.GetDBInt(LastId);
                jobsFilesFinal.Comments = Comments;
                jobsFilesFinal.UpdateJobsFilesFinalCommentByVesrionIdLastId();
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
    }
}
