using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class TagController : Controller
    {
        [LoginAuthorized]
        public IActionResult Index(int ID,string flag)
        {
            ViewBag.State = "TAGGING";
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;

            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;

            TagFiles tagFiles = new TagFiles();
            JobsFiles jobsFiles = new JobsFiles();
            
            jobsFiles.databasename = databasename;
            tagFiles.jobsFiles = jobsFiles.GetJobFilesByID(ID);

            Jobs objJobs = new Jobs();
            objJobs.databasename = databasename;
            tagFiles.jobs = objJobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(tagFiles.jobsFiles.JobID));
            if (tagFiles.jobs.JobQuotedAs != "Timed")
            {
                if (tagFiles.jobsFiles.PricePer == "Hour")
                {
                    tagFiles.jobs.JobQuotedAs = tagFiles.jobsFiles.Quantity.ToString() + " Hrs";
                }
                else
                {
                    tagFiles.jobs.JobQuotedAs = "Timed";
                }
            }
            else
            {
                tagFiles.jobs.JobQuotedAs = "Timed";
            }

            Clients clients = new Clients();
            clients.databasename = databasename;
            tagFiles.clients = clients.GetClientById(tagFiles.jobs.ClientID);

            AllTimers allTimers = new AllTimers();
            allTimers.JobID = Utility.CommonHelper.GetDBInt(tagFiles.jobsFiles.JobID);
            allTimers.FileID = ID;
            allTimers.State = "TAGGING";
            allTimers.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            allTimers.databasename = databasename;
            tagFiles.allTimers = allTimers.GetAllTimersByIds();

            JobsFilesReviews objjobsFilesFinal = new JobsFilesReviews();
            objjobsFilesFinal.databasename = databasename;
            tagFiles.jobsFilesCommentsList = objjobsFilesFinal.GetCommentsPhase4or5(Utility.CommonHelper.GetDBInt(tagFiles.jobsFiles.JobID), ID, "qc", "");
            tagFiles.flag = flag;
            return View(tagFiles);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult FileProgress(int Page, int File, string Cat,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFilesProgress jobsFilesProgress = new JobsFilesProgress();
            jobsFilesProgress.FileID = File;
            jobsFilesProgress.PageNumber = Page;
            jobsFilesProgress.Progress = Cat;
            jobsFilesProgress.Submitted = DateTime.Now;
            jobsFilesProgress.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            jobsFilesProgress.Insert();

            if (Cat == "TAGGING")
            {
                JobsFiles jobsFiles = new JobsFiles();
                jobsFiles.ID = File;
                jobsFiles.CurrentPage = Page;
                jobsFiles.databasename = databasename;
                jobsFiles.UpdateCurrentPage();
            }
            else if (Cat == "ASSEMBLY")
            {
                JobsFiles jobsFiles = new JobsFiles();
                jobsFiles.ID = File;
                jobsFiles.CurrentPage = Page;
                jobsFiles.databasename = databasename;
                jobsFiles.UpdateAssemblyPage();
            }




            var jsonResult = Json(new

            {
                message = "Updated progress to page " + Page + " for file " + File
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult FileCheckin(string Timer, int File, int Page, int Assembly, int AltTextStatus,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = File;
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Timer = Timer;
            objJobsFilesCheckouts.State = "TAGGING";
            objJobsFilesCheckouts.Checkin = DateTime.Now;
            objJobsFilesCheckouts.Checkin_PageNumber = Page;
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.UpdateJobsFilesByFileIdUserId();
            
            HttpContext.Session.SetString("LastCheckedInTimer", Utility.CommonHelper.GetDBString(Timer));

            string FullName = HttpContext.Session.GetString("FirstName") + " " + HttpContext.Session.GetString("LastName");

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.Touches = FullName;
            objJobsFiles.LastTagger = Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID");
            objJobsFiles.AltTxt = Utility.CommonHelper.GetDBInt(AltTextStatus);
            objJobsFiles.ID = File;
            objJobsFiles.CurrentPage = Page;
            objJobsFiles.databasename = databasename;
            objJobsFiles.UpdateJobsFilesCheckOut();

            var jsonResult = Json(new
            {
                message = "Checked in file " + File
            });
            return jsonResult;
        }
    }
}
