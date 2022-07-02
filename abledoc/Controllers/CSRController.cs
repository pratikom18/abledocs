using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class CSRController : Controller
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        public CSRController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetCSRList(JQueryDataTableParamModel param)
        {
            var databasename = HttpContext.Session.GetString("Database");
            //var databasename1 = HttpContext.Session.GetString("Database_edit");
            //if (databasename != databasename1)
            //{
            //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
            //}

            int totalRecords = 0;
            List<ClientSpecificRequirements> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            ClientSpecificRequirements objJobMaster = new ClientSpecificRequirements();
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
            allRecord = objJobMaster.GetCSRList(startIndex, endIndex, OrderBy, SearchKey, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}


            var result = from c in allRecord
                         select new[] {
                             c.ClientID.ToString(),                                                             //0
                             c.client_code == null ? "" :c.client_code,                                         //1
                             c.Company == null ? "" : c.Company.ToString(),                                     //2
                             c.remediation_requirements == null ? "" : c.remediation_requirements.ToString(),   //3
                             c.common_alt== null ? "" : c.common_alt.ToString(),                                //4
                             c.author == null?"":c.author.ToString(),                                           //5
                             c.unsecured1 == null?"":c.unsecured1.ToString(),                                   //6
                             c.secured1== null ? "" : c.secured1.ToString(),                                    //7
                             c.pdf1== null ? "" :  c.pdf1.ToString(),                                           //8
                             c.pac_reports1== null ? "" : c.pac_reports1.ToString(),                            //9
                             c.notes == null?"":c.notes.ToString(),                                             //10 
                             c.databasename.ToString(),                                                         //11
                             c.databasename == Models.Utility.Constants.ABLEDOCS_EU_DB ?"1":"0",                                          //12

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
        public JsonResult uploadcsr()
        {
            try
            {
                var databasename = HttpContext.Session.GetString("Database");

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
                        string fileName = "Quote_Sample" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"; //Path.GetFileName(postedFile.FileName);
                        string filePath = Path.Combine(path, fileName);
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                        }

                        DataTable dt1 = GetDataTableFromExcel(filePath, true);

                        foreach (DataRow data in dt1.Rows)
                        {
                            ClientSpecificRequirements modelJobsFiles = new ClientSpecificRequirements();
                            modelJobsFiles.client_code = Utility.CommonHelper.GetDBString(data[0]);
                            modelJobsFiles.remediation_requirements = Utility.CommonHelper.GetDBString(data[1]);
                            modelJobsFiles.common_alt = Utility.CommonHelper.GetDBString(data[2]);
                            modelJobsFiles.author = Utility.CommonHelper.GetDBString(data[3]);
                            modelJobsFiles.unsecured = Utility.CommonHelper.GetDBString(data[4]) == "1" ? true : false;
                            modelJobsFiles.secured = Utility.CommonHelper.GetDBString(data[5]) == "1" ? true : false;
                            modelJobsFiles.pdf = Utility.CommonHelper.GetDBString(data[6]) == "1" ? true : false;
                            modelJobsFiles.pac_reports = Utility.CommonHelper.GetDBString(data[7]) == "1" ? true : false;
                            modelJobsFiles.notes = Utility.CommonHelper.GetDBString(data[8]);
                            modelJobsFiles.databasename = databasename;
                            modelJobsFiles.Insert();
                            //jobsFiles
                        }

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
    }
}
