using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class FileGetController : Controller
    {
        //[AuthorizedAction]
        [LoginAuthorized]
        public IActionResult Index(int Id,string error="",string flag="0")
        {
            var databasename = Utility.CommonHelper.Getabledocs(flag);
            

            ErrorReport objErrorReport = new ErrorReport();
            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.databasename = databasename;
            if (!string.IsNullOrWhiteSpace(error))
            {
                objErrorReport.databasename = databasename;
                objErrorReport = objErrorReport.GetErrorReportListByID(Id);
            }
            else {   
                objJobsFilesVersions = objJobsFilesVersions.GetFileVersionByID(Id);
            }
            if (objErrorReport != null || objJobsFilesVersions != null)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    JobsFilesInteractions objJobsFilesInteractions = new JobsFilesInteractions();
                    objJobsFilesInteractions.FileID = Utility.CommonHelper.GetDBInt(objErrorReport.FileID);
                    objJobsFilesInteractions.JobID = Utility.CommonHelper.GetDBInt(objErrorReport.JobID);
                    objJobsFilesInteractions.FileVersionID = objErrorReport.ID;
                    objJobsFilesInteractions.UserID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
                    objJobsFilesInteractions.Activity = "DOWNLOAD";
                    var format = "yyyy-MM-dd H:mm:ss";
                    objJobsFilesInteractions.LastUpdated = Utility.CommonHelper.GetDBDate(DateTime.Now.ToString(format));
                    objJobsFilesInteractions.databasename = databasename;
                    objJobsFilesInteractions.Insert();
                    ViewBag.Physical_Path = objErrorReport.PhysicalPath;
                }
                else {
                    ViewBag.Physical_Path = objJobsFilesVersions.Physical_Path;
                }
            }
            return View();
        }
    }
}
