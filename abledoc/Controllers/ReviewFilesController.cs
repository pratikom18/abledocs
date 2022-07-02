using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ReviewFilesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ReviewFilesController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;

        }

        [LoginAuthorized]
        public IActionResult Index(int ID,string flag)
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            ViewBag.databasename = databasename;

            ViewBag.State = "REVIEW";

            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;

            ReviewFiles reviewFiles = new ReviewFiles();
            JobsFiles jobsFiles = new JobsFiles();
            
            jobsFiles.databasename = databasename;
            reviewFiles.jobsFiles = jobsFiles.GetJobFilesByID(ID);

            Jobs objJobs = new Jobs();
            objJobs.databasename = databasename;
            reviewFiles.jobs = objJobs.GetJobsByJobID(Utility.CommonHelper.GetDBInt(reviewFiles.jobsFiles.JobID));
            if (reviewFiles.jobs.JobQuotedAs != "Timed")
            {
                if (reviewFiles.jobsFiles.PricePer == "Hour")
                {
                    reviewFiles.jobs.JobQuotedAs = reviewFiles.jobsFiles.Quantity.ToString() + " Hrs";
                }
                else
                {
                    reviewFiles.jobs.JobQuotedAs = "Timed";
                }
            }
            else
            {
                reviewFiles.jobs.JobQuotedAs = "Timed";
            }

            Clients clients = new Clients();
            clients.databasename = databasename;
            reviewFiles.clients = clients.GetClientById(reviewFiles.jobs.ClientID);

            Users users = new Users();
            reviewFiles.users = users.GetUserById(Utility.CommonHelper.GetDBInt(reviewFiles.jobsFiles.LastTagger));

            AllTimers allTimers = new AllTimers();
            allTimers.JobID = Utility.CommonHelper.GetDBInt(reviewFiles.jobsFiles.JobID);
            allTimers.FileID = ID;
            allTimers.State = "REVIEW";
            allTimers.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            allTimers.databasename = databasename;
            reviewFiles.allTimers = allTimers.GetAllTimersByIds();

            if (reviewFiles.jobsFiles.Status == "TAGGING" || reviewFiles.jobsFiles.Status == "REVIEW")
            {
                JobsFilesReviews jobsFilesReviews = new JobsFilesReviews();
                jobsFilesReviews.databasename = databasename;
                reviewFiles.jobsFilesReviewsList = jobsFilesReviews.GetRLC(Utility.CommonHelper.GetDBInt(reviewFiles.jobsFiles.JobID), ID);
            }

            ErrorReport objErrorReport = new ErrorReport();
            objErrorReport.databasename = databasename;
            reviewFiles.errorReportsList = objErrorReport.GetErrorReportList(reviewFiles.jobsFiles.ID);
            reviewFiles.flag = flag;
            return View(reviewFiles);
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ReviewCheckin()
        {
            string flag = Request.Form["flag"];
            var databasename = Utility.CommonHelper.Getabledocs(flag);

            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            objJobsFilesVersions = objJobsFilesVersions.GetJobsFilesVersionsLastData();
            int versionID = objJobsFilesVersions.ID;
            DateTime lastUpdated = objJobsFilesVersions.LastUpdated;

            string timer = Request.Form["timer"];
            string comments = Request.Form["comments"];
            string file = Request.Form["file"];
            string jobId = Request.Form["jobId"];
            string lastTagger = Request.Form["lastTagger"];
            string tagOrder = Request.Form["tagOrder"];
            string contentOrder = Request.Form["contentOrder"];
            string tabbingOrder = Request.Form["tabbingOrder"];
            string incorrectTags = Request.Form["incorrectTags"];
            string assembly = Request.Form["assembly"];
            string figuresText = Request.Form["figuresText"];
            string figuresMissed = Request.Form["figuresMissed"];
            string headings = Request.Form["headings"];
            string referencesMissed = Request.Form["referencesMissed"];
            string referencesType = Request.Form["referencesType"];
            string spaceIssues = Request.Form["spaceIssues"];
            string formFieldProperties = Request.Form["formFieldProperties"];
            string formFieldTooltips = Request.Form["formFieldTooltips"];
            string formFieldType = Request.Form["formFieldType"];
            string artifact = Request.Form["artifact"];
            string tableStructure = Request.Form["tableStructure"];
            string listStructure = Request.Form["listStructure"];
            string links = Request.Form["links"];
            string languageAttributes = Request.Form["languageAttributes"];
            string sideBySide = Request.Form["sideBySide"];
            string csr = Request.Form["csr"];
            string documentNaming = Request.Form["documentNaming"];
            string addComments = Request.Form["addComments"];
            string LastId = string.Empty;
            if (addComments == "1")
            {
                JobsFilesReviews jobsFilesReviews = new JobsFilesReviews();
                jobsFilesReviews.VersionID = versionID;
                jobsFilesReviews.ReviewerID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                jobsFilesReviews.FileID = Utility.CommonHelper.GetDBInt(file);
                jobsFilesReviews.JobID = Utility.CommonHelper.GetDBInt(jobId);
                jobsFilesReviews.LastUpdated = DateTime.Now;
                jobsFilesReviews.databasename = databasename;
                LastId = jobsFilesReviews.Insert();
            }


            int errorCountOff = 0;
            if (tagOrder == "0") { errorCountOff++; }
            if (contentOrder == "0") { errorCountOff++; }
            if (tabbingOrder == "0") { errorCountOff++; }
            if (incorrectTags == "0") { errorCountOff++; }
            if (assembly == "0") { errorCountOff++; }
            if (figuresText == "0") { errorCountOff++; }
            if (figuresMissed == "0") { errorCountOff++; }
            if (headings == "0") { errorCountOff++; }
            if (referencesMissed == "0") { errorCountOff++; }
            if (referencesType == "0") { errorCountOff++; }
            if (spaceIssues == "0") { errorCountOff++; }
            if (formFieldProperties == "0") { errorCountOff++; }
            if (formFieldTooltips == "0") { errorCountOff++; }
            if (formFieldType == "0") { errorCountOff++; }
            if (artifact == "0") { errorCountOff++; }
            if (tableStructure == "0") { errorCountOff++; }
            if (listStructure == "0") { errorCountOff++; }
            if (links == "0") { errorCountOff++; }
            if (languageAttributes == "0") { errorCountOff++; }
            if (sideBySide == "0") { errorCountOff++; }
            if (csr == "0") { errorCountOff++; }
            if (documentNaming == "0") { errorCountOff++; }

            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = Utility.CommonHelper.GetDBInt(file);
            objJobsFilesCheckouts.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            objJobsFilesCheckouts.Timer = timer;
            objJobsFilesCheckouts.State = "REVIEW";
            objJobsFilesCheckouts.Checkin = DateTime.Now;
            objJobsFilesCheckouts.ExtraData = errorCountOff.ToString();
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.UpdateJobsFilesExtraDataByFileIdUserId();



            HttpContext.Session.SetString("LastCheckedInTimer", Utility.CommonHelper.GetDBString(timer));
            string FullName = HttpContext.Session.GetString("FirstName") + " " + HttpContext.Session.GetString("LastName");

            if (addComments == "1")
            {
                JobsFilesReviews jobsFilesReviews = new JobsFilesReviews();
                jobsFilesReviews.ID = Utility.CommonHelper.GetDBInt(LastId);
                jobsFilesReviews.VersionID = versionID;
                jobsFilesReviews.Comments = comments;
                jobsFilesReviews.TagOrderE = Utility.CommonHelper.GetDBInt(tagOrder);
                jobsFilesReviews.ContentOrderE = Utility.CommonHelper.GetDBInt(contentOrder);
                jobsFilesReviews.TabbingOrderE = Utility.CommonHelper.GetDBInt(tabbingOrder);
                jobsFilesReviews.IncorrectTagsE = Utility.CommonHelper.GetDBInt(incorrectTags);
                jobsFilesReviews.AssemblyE = Utility.CommonHelper.GetDBInt(assembly);
                jobsFilesReviews.FiguresTextE = Utility.CommonHelper.GetDBInt(figuresText);
                jobsFilesReviews.FiguresMissedE = Utility.CommonHelper.GetDBInt(figuresMissed);
                jobsFilesReviews.HeadingsE = Utility.CommonHelper.GetDBInt(headings);
                jobsFilesReviews.ReferencesMissedE = Utility.CommonHelper.GetDBInt(referencesMissed);
                jobsFilesReviews.ReferencesTypeE = Utility.CommonHelper.GetDBInt(referencesType);
                jobsFilesReviews.SpaceIssuesE = Utility.CommonHelper.GetDBInt(spaceIssues);
                jobsFilesReviews.FormFieldPropertiesE = Utility.CommonHelper.GetDBInt(formFieldProperties);
                jobsFilesReviews.FormFieldTooltipsE = Utility.CommonHelper.GetDBInt(formFieldTooltips);
                jobsFilesReviews.FormFieldTypeE = Utility.CommonHelper.GetDBInt(formFieldType);
                jobsFilesReviews.ArtifactE = Utility.CommonHelper.GetDBInt(artifact);
                jobsFilesReviews.TableStructureE = Utility.CommonHelper.GetDBInt(tableStructure);
                jobsFilesReviews.ListStructureE = Utility.CommonHelper.GetDBInt(listStructure);
                jobsFilesReviews.LinksE = Utility.CommonHelper.GetDBInt(links);
                jobsFilesReviews.LanguageAttributesE = Utility.CommonHelper.GetDBInt(languageAttributes);
                jobsFilesReviews.SideBySideE = Utility.CommonHelper.GetDBInt(sideBySide);
                jobsFilesReviews.CSRE = Utility.CommonHelper.GetDBInt(csr);
                jobsFilesReviews.DocumentNamingE = Utility.CommonHelper.GetDBInt(documentNaming);
                jobsFilesReviews.TaggedBy = Utility.CommonHelper.GetDBInt(lastTagger);
                jobsFilesReviews.databasename = databasename;
                jobsFilesReviews.Update();
            }

            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.Touches = FullName;
            objJobsFiles.ID = Utility.CommonHelper.GetDBInt(file);
            objJobsFiles.databasename = databasename;
            objJobsFiles.UpdateJobsFilesTouchesReview();

            var jsonResult = Json(new
            {
                message = "Checked in file " + file
            });
            return jsonResult;
        }
    }
}
