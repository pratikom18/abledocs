using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class ADscanController : Controller
    {
        
        private readonly IWebHostEnvironment webHostEnvironment;

        public ADscanController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        [AuthorizedAction]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult SetContactDatabase()
        {
            var databasename = Request.Form["databasename"];

            HttpContext.Session.SetString("database_adscan", Utility.CommonHelper.GetDBString(databasename));
            var jsonResult = Json(new

            {
                result = true
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ADScanLists(JQueryDataTableParamModel param, string AlphaSearch)
        {
            var databasename = HttpContext.Session.GetString("Database");
            //var databasename1 = HttpContext.Session.GetString("Database_edit");
            //if (databasename != databasename1)
            //{
            //    HttpContext.Session.SetString("Database", Utility.CommonHelper.GetDBString(databasename1));
            //}

            //var databasename2 = HttpContext.Session.GetString("database_adscan");
            //var databasename3 = HttpContext.Session.GetString("database_adscan_edit");
            //if (databasename2 != databasename3)
            //{
            //    HttpContext.Session.SetString("database_adscan", Utility.CommonHelper.GetDBString(databasename3));
            //}

            int totalRecords = 0;
            List<ADscanCrawls> allRecord = new List<ADscanCrawls>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            ADscanCrawls objAdscanMaster = new ADscanCrawls();
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

            allRecord = objAdscanMaster.GetADscanList(startIndex, endIndex, OrderBy, SearchKey, AlphaSearch, out totalRecords);

            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                if (allRecord != null)
                {
                    filteredRecords = allRecord.Count();
                }
                else
                {
                    filteredRecords = 0;
                }

            }
            if (allRecord != null)
            {

                foreach (ADscanCrawls item in allRecord)
                {
                    Clients objClientMaster = new Clients();
                    objClientMaster.databasename = item.databasename.Contains("_eu") ? Models.Utility.Constants.ABLEDOCS_EU_DB : Models.Utility.Constants.ABLEDOCS_DB;
                    objClientMaster = objClientMaster.GetClientById(Convert.ToInt32(item.clientID));
                    if (objClientMaster != null)
                    {
                        item.Code = objClientMaster.Code;
                        item.Company = objClientMaster.Company;
                    }
                }
            }
           
            var result = from c in allRecord
                         select new[] {

                             c.ID.ToString(),                                           //0
                             c.clientID.ToString(),                                     //1
                             c.Code == null ? "" : c.Code.ToString(),                   //2
                             c.Company == null ? "" : c.Company.ToString(),             //3
                             c.status == null ? "" : c.status.ToString(),               //4            
                             c.starturl == null ? "" : c.starturl.ToString(),           //5
                             c.Files_Found.ToString(),                                  //6
                             c.Files_Scanned.ToString(),                                //7
                             c.Files_Error.ToString(),                                  //8
                             c.databasename.ToString(),                                 //9
                             c.databasename.Contains("_eu")?Models.Utility.Constants.ABLEDOCS_EU_DB:Models.Utility.Constants.ABLEDOCS_DB,   //10
                             c.databasename.Contains("_eu")?"1":"0",                    //11
                         };

            var jsonResult = Json(new

            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
                //bbData = results
            });
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        //[ValidateAntiForgeryToken]

        public ActionResult Create(ADscanCrawls model)
        {
            try
            {
                model.databasename = Utility.CommonHelper.Getadscan(model.flag);
                var id = model.InsertUpdate();
                return RedirectToAction("edit", new { id = Convert.ToInt32(id),flag= model.flag });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet, LoginAuthorized]
        public ActionResult Edit(int id, string flag)
        {
            var databasename = Utility.CommonHelper.Getadscan(flag);
            ViewBag.databasename = databasename;
            //var databasename1 = HttpContext.Session.GetString("database_adscan_edit");
            //if (databasename != databasename1)
            //{
            //    ViewBag.Disebled = "True";
            //}
            //else
            //{

            //    ViewBag.Disebled = "Fase";
            //}

            ADscanCrawls objADscanMaster = new ADscanCrawls();
            objADscanMaster.databasename = databasename;
            objADscanMaster = objADscanMaster.GetADscanById(Convert.ToInt32(id));

            ADscanFiles objADscanFiles = new ADscanFiles();
            objADscanFiles.databasename = databasename;
            List<ADscanFiles> ADscanFileslist = objADscanFiles.GetADscanfileslise(Convert.ToInt32(id));
            ViewBag.ADscanfiles = ADscanFileslist;
            objADscanMaster.flag = flag;
            return View(objADscanMaster);
        }
        [HttpPost, LoginAuthorized]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ADscanCrawls model)
        {
            try
            {
                var databasename = Utility.CommonHelper.Getadscan(model.flag);
                model.databasename = databasename;
                var message = model.InsertUpdate();
                //return RedirectToAction("Index");
                //ADscanCrawls objADscanMaster = new ADscanCrawls();
                //objADscanMaster.databasename = databasename;
                //objADscanMaster = objADscanMaster.GetADscanById(Convert.ToInt32(model.ID));
                // return View(objADscanMaster);
                return RedirectToAction("edit", new { id = Convert.ToInt32(model.ID), flag = model.flag });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost, LoginAuthorized]
        public JsonResult updateCrawl(int ID,string flag)
        {
            ADscanCrawls objaDscanCrawls = new ADscanCrawls();
            objaDscanCrawls.databasename = Utility.CommonHelper.Getadscan(flag);

            objaDscanCrawls.UpdateCrawl(ID);

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public JsonResult deleteCrawl(int ID,string flag)
        {
            var databasename = Utility.CommonHelper.Getadscan(flag);
            ADscanCrawls objaDscanCrawls = new ADscanCrawls();
            objaDscanCrawls.databasename = databasename;
            objaDscanCrawls.DeleteCrawl(ID);
            ADscanFiles objadfile = new ADscanFiles();
            objadfile.databasename = databasename;
            objadfile.Deleteadcsanfile(ID);

            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpPost, LoginAuthorized]
        public async Task<JsonResult> adscanCreatejob(int clientID,int ID,string flag)
        {
            var databasename = Utility.CommonHelper.Getadscan(flag);
            var databasename1 = Utility.CommonHelper.Getabledocs(flag);

            Jobs jobs = new Jobs();
            ADscanCrawls objaDscanCrawls = new ADscanCrawls();
            
            jobs.databasename = databasename1;
            jobs.ClientID = clientID;
            var lastjobid = jobs.CreateJob();
            objaDscanCrawls.databasename = databasename;
            objaDscanCrawls.UpdateadcsanCrawl(Convert.ToInt32(lastjobid),ID);
            jobs = new Jobs();
            jobs.Deadline = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            jobs.UID = Utility.CommonHelper.GetDBInt(Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID"));
            jobs.databasename = databasename1;
            jobs.ID = Convert.ToInt32(lastjobid);
            jobs.UpdateCopyJob();

            if ("ADO_INTERNAL_SCRIPTS_KEY" == "ADO_INTERNAL_SCRIPTS_KEY")
            {
                ADscanFiles aDscanFiles = new ADscanFiles();
                aDscanFiles.databasename = databasename;
                List<ADscanFiles> aDscanControllerslist = new List<ADscanFiles>();
                aDscanControllerslist= aDscanFiles.GetADscanfilesliset(ID);
                if (aDscanControllerslist.Count > 0)
                {
                    foreach (ADscanFiles item in aDscanControllerslist)
                    {
                        Utility.UploadFile uploadFile = new Utility.UploadFile(this.webHostEnvironment);
                        await uploadFile.AddUrlFileToJob(databasename,Utility.CommonHelper.GetDBInt(lastjobid), item.url);
                    }
                    
                }
            }

            var jsonResult = Json(new
            {
               // message = "success"
               jobid= lastjobid
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;

        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult AdscanFileUrl(int id,string flag)
        {
            ADscanFiles objADscanFiles = new ADscanFiles();
            if (id != 0)
            {
                objADscanFiles.crawl_id = id;
                objADscanFiles.offsite = 0;
            }
            objADscanFiles.flag = flag;
            return PartialView("_AdscanFileUrl", objADscanFiles);
        }

        [HttpPost, LoginAuthorized]
        public ActionResult Insert(ADscanFiles model)
        {

            try
            {
                model.databasename = Utility.CommonHelper.Getadscan(model.flag);
                var id = model.Insert();
                return RedirectToAction("edit", new { id = Convert.ToInt32(model.crawl_id),flag=model.flag });
            }
            catch
            {
                return RedirectToAction();
            }
        }

        [HttpPost, LoginAuthorized]
        public ActionResult ZipPDFFiles(int id)
        {
            ADscanFiles objADscanFiles = new ADscanFiles();
            objADscanFiles.databasename = HttpContext.Session.GetString("database_adscan");
            List<ADscanFiles> ADscanFileslist = objADscanFiles.GetADscanfileslise(Convert.ToInt32(id));

            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < ADscanFileslist.Count; i++)
                    {
                        try
                        {
                            ziparchive.CreateEntryFromFile($"{ADscanFileslist[i].url}", ADscanFileslist[i].filename, System.IO.Compression.CompressionLevel.Fastest);
                        }
                        catch (Exception ex)
                        { 
                        }
                        
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", id + ".zip");
            }
        }

        [LoginAuthorized]
        public async Task<FileResult> ZipPDFFiles2(int id,string flag)
        {
            ADscanFiles objADscanFiles = new ADscanFiles();
            objADscanFiles.databasename = Utility.CommonHelper.Getadscan(flag);
            List<ADscanFiles> ADscanFileslist = objADscanFiles.GetADscanfileslise(Convert.ToInt32(id));

            var contentType = "application/zip";

            string zippedFolderName = id + ".zip";

            var downloadResults = ADscanFileslist
            .Select(uri => (uri: uri, response: HttpClientFactory.Create().SendAsync(new HttpRequestMessage(HttpMethod.Get, uri.url))))
            .ToArray();

            await Task.WhenAll(downloadResults.Select(v => v.response));

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var download in downloadResults)
                    {
                        if (!string.IsNullOrEmpty(download.uri.filename))
                        {
                            var entry = archive.CreateEntry(download.uri.filename, CompressionLevel.Fastest);

                            using (var zipStream = entry.Open())
                            {
                                var data = await download.response.Result.Content.ReadAsByteArrayAsync();

                                zipStream.Write(data, 0, data.Length);
                            }
                        }
                        
                    }
                }

                return File(ms.ToArray(), contentType, $"{zippedFolderName}");
            }
                
        }

        //public FileResult DownloadZipFile(int id)
        //{

        //    var fileName = string.Format("{0}_Files.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_1");
        //    var tempOutPutPath = Path.Combine(webHostEnvironment.WebRootPath, "download/"+ fileName) ;

        //    using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
        //    {
        //        s.SetLevel(9); // 0-9, 9 being the highest compression  

        //        byte[] buffer = new byte[4096];

        //        ADscanFiles objADscanFiles = new ADscanFiles();
        //        List<ADscanFiles> ADscanFileslist = objADscanFiles.GetADscanfileslise(Convert.ToInt32(id));

        //        for (int i = 0; i < ADscanFileslist.Count; i++)
        //        {
        //            ZipEntry entry = new ZipEntry(ADscanFileslist[i].filename);
        //            entry.DateTime = DateTime.Now;
        //            entry.IsUnicodeText = true;
        //            s.PutNextEntry(entry);

        //            using (FileStream fs = System.IO.File.OpenRead(ADscanFileslist[i].url))
        //            {
        //                int sourceBytes;
        //                do
        //                {
        //                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
        //                    s.Write(buffer, 0, sourceBytes);
        //                } while (sourceBytes > 0);
        //            }
        //        }
        //        s.Finish();
        //        s.Flush();
        //        s.Close();

        //    }

        //    byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
        //    if (System.IO.File.Exists(tempOutPutPath))
        //        System.IO.File.Delete(tempOutPutPath);

        //    if (finalResult == null || !finalResult.Any())
        //        throw new Exception(String.Format("No Files found with Image"));

        //    return File(finalResult, "application/zip", fileName);

        //}
    }
}
