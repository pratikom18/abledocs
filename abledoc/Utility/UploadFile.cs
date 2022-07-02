using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace abledoc.Utility
{
    public class UploadFile
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public UploadFile(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public string databasename { get; set; }

        public string ProcessRequest(string databasename,int jobID, int fileID, string state = "SOURCE", string qcType = "", IFormFileCollection files = null, int UploadMetaData = 0)
        {
            //var files = HttpContext.Request.Form.Files;
            string uploadFilePath = string.Empty;
            string wwwPath = this.webHostEnvironment.WebRootPath;
            string contentPath = this.webHostEnvironment.ContentRootPath;

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ERP");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path1 = Path.Combine(path, jobID.ToString());
            string filePath1 = "";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
                filePath1 = "/ERP/" + jobID;
            }

            if (fileID != 0)
            {
                path1 = Path.Combine(path1, fileID.ToString());
                filePath1 = "/ERP/" + jobID + "/" + fileID;
            }
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            foreach (var pdf in files)
            {
                string fileName = pdf.FileName;
                string type = pdf.ContentType;
                string size = pdf.Length.ToString();
                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
                objJobsFilesVersions.FileID = fileID;
                objJobsFilesVersions.JobID = jobID;
                objJobsFilesVersions.State = state;
                objJobsFilesVersions.QCType = Utility.CommonHelper.GetDBInt(qcType);
                objJobsFilesVersions.Filename = fileName;
                objJobsFilesVersions.Type = type;
                objJobsFilesVersions.Size = size;
                objJobsFilesVersions.LastUpdated = DateTime.Now;
                objJobsFilesVersions.databasename = databasename;
                string id = objJobsFilesVersions.Insert();
                string filePath = Path.Combine(path1, id + fileExtension);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    pdf.CopyTo(fileStream);
                }

                objJobsFilesVersions = new JobsFilesVersions();
                objJobsFilesVersions.ID = Utility.CommonHelper.GetDBInt(id);
                objJobsFilesVersions.Physical_Path = Path.Combine(filePath1, id + fileExtension);
                objJobsFilesVersions.databasename = databasename;
                objJobsFilesVersions.UpdatePhysicalPath();
                JobsFiles objJobsFiles = new JobsFiles();
                objJobsFiles.ID = fileID;
                objJobsFiles.CurrentVersionFileID = Utility.CommonHelper.GetDBInt(id);
                
                objJobsFiles.databasename = databasename;
                objJobsFiles.Update();
                uploadFilePath = id;
                if (UploadMetaData == 1)
                {
                    //Dictionary<string, string> metadata = Utility.CommonHelper.PDFMetadata(@"E:\Projects\abledoc.abledocs\ADO\ADO\wwwroot\QuoteInvoices\1285\test.pdf");

                    if (fileExtension == ".pdf")
                    {
                        Dictionary<string, string> metadata = Utility.CommonHelper.PDFMetadata(Path.Combine(path1, id + fileExtension));
                        if (metadata.Count > 0)
                        {
                            int PdfPages = 0;
                            MetaDataFiles objMetaDataFiles = new MetaDataFiles();
                            foreach (KeyValuePair<string, string> item in metadata)
                            {
                                if (item.Key == "Title")
                                {
                                    objMetaDataFiles.Title = item.Value;
                                }
                                else if (item.Key == "Author")
                                {
                                    objMetaDataFiles.Author = item.Value;
                                }
                                else if (item.Key == "Subject")
                                {
                                    objMetaDataFiles.Subject = item.Value;
                                }
                                else if (item.Key == "Keywords")
                                {
                                    objMetaDataFiles.Keywords = item.Value;
                                }
                                else if (item.Key == "CreationDate")
                                {
                                    objMetaDataFiles.FileCreationDate = item.Value;
                                }
                                else if (item.Key == "ModDate")
                                {
                                    objMetaDataFiles.FileModificationDate = item.Value;
                                }
                                else if (item.Key == "Pages")
                                {
                                    PdfPages = Utility.CommonHelper.GetDBInt(item.Value);
                                    objMetaDataFiles.Pages = PdfPages;
                                }
                            }
                            objMetaDataFiles.VersionID = Utility.CommonHelper.GetDBInt(id);
                            objMetaDataFiles.databasename = databasename;
                            string MetadataID = objMetaDataFiles.Insert();

                            objJobsFilesVersions = new JobsFilesVersions();
                            objJobsFilesVersions.ID = Utility.CommonHelper.GetDBInt(id);
                            objJobsFilesVersions.MetaDataID = Utility.CommonHelper.GetDBInt(MetadataID);
                            objJobsFilesVersions.databasename = databasename;
                            objJobsFilesVersions.UpdateMetaDataID();

                            objJobsFiles = new JobsFiles();
                            objJobsFiles.ID = Utility.CommonHelper.GetDBInt(fileID);
                            objJobsFiles.Pages = Utility.CommonHelper.GetDBInt(PdfPages);
                            objJobsFiles.databasename = databasename;
                            objJobsFiles.UpdatePages();
                        }
                    }

                }
            }


            return uploadFilePath;
        }

        public List<UploadFiles> AddJobFileVersion(string databasename,int jobID, int fileID, string state = "SOURCE", string qcType = "", IFormFileCollection files = null, int UploadMetaData = 0)
        {
            List<UploadFiles> uploadFilesList = new List<UploadFiles>();
            //var files = HttpContext.Request.Form.Files;
            string uploadFilePath = string.Empty;
            string wwwPath = this.webHostEnvironment.WebRootPath;
            string contentPath = this.webHostEnvironment.ContentRootPath;

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ERP");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path1 = Path.Combine(path, jobID.ToString());
            string filePath1 = "";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
                filePath1 = "/ERP/" + jobID;
            }

            if (fileID != 0)
            {
                path1 = Path.Combine(path1, fileID.ToString());
                filePath1 = "/ERP/" + jobID + "/" + fileID;
            }
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            foreach (var pdf in files)
            {
                string fileName = pdf.FileName;
                string type = pdf.ContentType;
                string size = pdf.Length.ToString();
                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
                objJobsFilesVersions.FileID = fileID;
                objJobsFilesVersions.JobID = jobID;
                objJobsFilesVersions.State = state;
                objJobsFilesVersions.QCType = Utility.CommonHelper.GetDBInt(qcType);
                objJobsFilesVersions.Filename = fileName;
                objJobsFilesVersions.Type = type;
                objJobsFilesVersions.Size = size;
                objJobsFilesVersions.LastUpdated = DateTime.Now;
                objJobsFilesVersions.databasename = databasename;
                string id = objJobsFilesVersions.Insert();
                string filePath = Path.Combine(path1, id + fileExtension);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    pdf.CopyTo(fileStream);
                }
                
                UploadFiles uploadFile = new UploadFiles();
                uploadFile.FileName = fileName;
                uploadFile.Size = size;
                uploadFile.ID = id;
                uploadFilesList.Add(uploadFile);

                objJobsFilesVersions = new JobsFilesVersions();
                objJobsFilesVersions.ID = Utility.CommonHelper.GetDBInt(id);
                objJobsFilesVersions.Physical_Path = Path.Combine(filePath1, id + fileExtension);
                objJobsFilesVersions.databasename = databasename;
                objJobsFilesVersions.UpdatePhysicalPath();
                JobsFiles objJobsFiles = new JobsFiles();
                objJobsFiles.ID = fileID;
                objJobsFiles.CurrentVersionFileID = Utility.CommonHelper.GetDBInt(id);
                objJobsFiles.databasename = databasename;
                objJobsFiles.Update();
                uploadFilePath = id;
                if (UploadMetaData == 1)
                {
                    //Dictionary<string, string> metadata = Utility.CommonHelper.PDFMetadata(@"E:\Projects\abledoc.abledocs\ADO\ADO\wwwroot\QuoteInvoices\1285\test.pdf");

                    if (fileExtension == ".pdf")
                    {
                        Dictionary<string, string> metadata = Utility.CommonHelper.PDFMetadata(Path.Combine(path1, id + fileExtension));
                        if (metadata.Count > 0)
                        {
                            int PdfPages = 0;
                            MetaDataFiles objMetaDataFiles = new MetaDataFiles();
                            foreach (KeyValuePair<string, string> item in metadata)
                            {
                                if (item.Key == "Title")
                                {
                                    objMetaDataFiles.Title = item.Value;
                                }
                                else if (item.Key == "Author")
                                {
                                    objMetaDataFiles.Author = item.Value;
                                }
                                else if (item.Key == "Subject")
                                {
                                    objMetaDataFiles.Subject = item.Value;
                                }
                                else if (item.Key == "Keywords")
                                {
                                    objMetaDataFiles.Keywords = item.Value;
                                }
                                else if (item.Key == "CreationDate")
                                {
                                    objMetaDataFiles.FileCreationDate = item.Value;
                                }
                                else if (item.Key == "ModDate")
                                {
                                    objMetaDataFiles.FileModificationDate = item.Value;
                                }
                                else if (item.Key == "Pages")
                                {
                                    PdfPages = Utility.CommonHelper.GetDBInt(item.Value);
                                    objMetaDataFiles.Pages = PdfPages;
                                }
                            }
                            objMetaDataFiles.VersionID = Utility.CommonHelper.GetDBInt(id);
                            objMetaDataFiles.databasename = databasename;
                            string MetadataID = objMetaDataFiles.Insert();

                            objJobsFilesVersions = new JobsFilesVersions();
                            objJobsFilesVersions.ID = Utility.CommonHelper.GetDBInt(id);
                            objJobsFilesVersions.MetaDataID = Utility.CommonHelper.GetDBInt(MetadataID);
                            objJobsFilesVersions.databasename = databasename;
                            objJobsFilesVersions.UpdateMetaDataID();

                            objJobsFiles = new JobsFiles();
                            objJobsFiles.ID = Utility.CommonHelper.GetDBInt(fileID);
                            objJobsFiles.Pages = Utility.CommonHelper.GetDBInt(PdfPages);
                            objJobsFiles.databasename = databasename;
                            objJobsFiles.UpdatePages();
                        }
                    }

                }
            }


            return uploadFilesList;
        }

        public List<UploadFiles> ErrorRequest(int jobID, int fileID, IFormFileCollection files = null)
        {
            //var files = HttpContext.Request.Form.Files;
            List<UploadFiles> uploadFilesList = new List<UploadFiles>();
            string uploadFilePath = string.Empty;
            string wwwPath = this.webHostEnvironment.WebRootPath;
            string contentPath = this.webHostEnvironment.ContentRootPath;

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, jobID.ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path1 = Path.Combine(path, fileID.ToString());
            string filePath1 = "";
            
            Directory.CreateDirectory(path1);
            filePath1 = "/" + jobID + "/" + fileID;

            path1 = Path.Combine(path1, "ErrorReports");
            filePath1 = "/" + jobID + "/" + fileID + "/ErrorReports";

            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            foreach (var pdf in files)
            {
                string fileName = pdf.FileName;
                string type = pdf.ContentType;
                string size = pdf.Length.ToString();
                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                ErrorReport errorReport = new ErrorReport();
                errorReport.JobID = Convert.ToUInt32(jobID);
                errorReport.FileID = Convert.ToUInt32(fileID);
                errorReport.ErrorReportName = fileName;
                errorReport.databasename = databasename;
                string retval = errorReport.Insert();

                string filePath = Path.Combine(path1, retval + fileExtension);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    pdf.CopyTo(fileStream);
                }

                UploadFiles uploadFile = new UploadFiles();
                uploadFile.FileName = fileName;
                uploadFile.Size = size;
                uploadFile.ID = retval;
                uploadFilesList.Add(uploadFile);

                errorReport = new ErrorReport();
                errorReport.ID = Utility.CommonHelper.GetDBInt(retval);
                errorReport.PhysicalPath = Path.Combine(filePath1, errorReport + fileExtension);
                errorReport.databasename = databasename;
                errorReport.UpdatePhysicalPath();
                
                
            }


            return uploadFilesList;
        }

        public string AddFile2Job(string databasename,int jobID, IFormFileCollection files = null, string state = "SOURCE")
        {

            int fileID = 0;
            foreach (var pdf in files)
            {
                string fileName = pdf.FileName;
                string type = pdf.ContentType;
                string size = pdf.Length.ToString();
                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                JobsFiles modelJobfiles = new JobsFiles();
                modelJobfiles.JobID = jobID;
                modelJobfiles.Filename = fileName;
                modelJobfiles.databasename = databasename;
                fileID = modelJobfiles.InsertFilesByJob();

            }

            ProcessRequest(databasename, jobID, fileID, state, "", files, 1);

            //upload file metadata

            return "file uploaded successfully";
        }
        public async Task<string> AddUrlFileToJob(string databasename,int jobID, string files = null, string state = "SOURCE")
        {

            int fileID = 0;

            string fileName = System.IO.Path.GetFileName(files);

            JobsFiles modelJobfiles = new JobsFiles();
            modelJobfiles.JobID = jobID;
            modelJobfiles.Filename = fileName;
            modelJobfiles.databasename = databasename;
            fileID = modelJobfiles.InsertFilesByJob();

            await FileSaveToUrl(databasename,jobID, fileID, "SOURCE", "", files, 1);

            //upload file metadata

            return "file uploaded successfully";
        }
        public async Task<string> FileSaveToUrl(string databasename,int jobID, int fileID, string state = "SOURCE", string qcType = "", string files = null, int UploadMetaData = 0)
        {
            //var files = HttpContext.Request.Form.Files;

            string wwwPath = this.webHostEnvironment.WebRootPath;
            string contentPath = this.webHostEnvironment.ContentRootPath;

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ERP");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path1 = Path.Combine(path, jobID.ToString());
            string filePath1 = "";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
                filePath1 = "/ERP/" + jobID;
            }

            if (fileID != 0)
            {
                path1 = Path.Combine(path1, fileID.ToString());
                filePath1 = "/ERP/" + jobID + "/" + fileID;
            }
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            string fileName = System.IO.Path.GetFileName(files);//pdf.FileName;
            string fileExtension = Path.GetExtension(files);
            var fileProvider = new FileExtensionContentTypeProvider();
            // Figures out what the content type should be based on the file name. 
            string contentType = "";
            if (!fileProvider.TryGetContentType(fileName, out contentType))
            {
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {fileName}.");
            }

            string type = contentType;//.ContentType;
            string size = GetFileSize(new Uri(files));// info.Length.ToString();

            //Getting file Extension


            JobsFilesVersions objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.FileID = fileID;
            objJobsFilesVersions.JobID = jobID;
            objJobsFilesVersions.State = state;
            objJobsFilesVersions.QCType = Utility.CommonHelper.GetDBInt(qcType);
            objJobsFilesVersions.Filename = fileName;
            objJobsFilesVersions.Type = type;
            objJobsFilesVersions.Size = size;
            objJobsFilesVersions.LastUpdated = DateTime.Now;
            objJobsFilesVersions.databasename = databasename;
            string id = objJobsFilesVersions.Insert();
            string filePath = Path.Combine(path1, id + fileExtension);

            string download = await downloadUrlFile(filePath, files);
            //if (!System.IO.File.Exists(filePath))
            //{
            //    WebClient webClient = new WebClient();
            //    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadFileCallback2);
            //    webClient.DownloadFileAsync(new Uri(files), filePath);
            //}

            objJobsFilesVersions = new JobsFilesVersions();
            objJobsFilesVersions.ID = Utility.CommonHelper.GetDBInt(id);
            objJobsFilesVersions.Physical_Path = Path.Combine(filePath1, id + fileExtension);
            objJobsFilesVersions.databasename = databasename;
            objJobsFilesVersions.UpdatePhysicalPath();
            JobsFiles objJobsFiles = new JobsFiles();
            objJobsFiles.ID = fileID;
            objJobsFiles.CurrentVersionFileID = Utility.CommonHelper.GetDBInt(id);
            objJobsFiles.databasename = databasename;
            objJobsFiles.Update();

            if (UploadMetaData == 1)
            {
                //Dictionary<string, string> metadata = Utility.CommonHelper.PDFMetadata(@"E:\Projects\abledoc.abledocs\ADO\ADO\wwwroot\QuoteInvoices\1285\test.pdf");

                if (fileExtension == ".pdf")
                {
                    Dictionary<string, string> metadata = Utility.CommonHelper.PDFMetadata(Path.Combine(path1, id + fileExtension));
                    if (metadata.Count > 0)
                    {
                        int PdfPages = 0;
                        MetaDataFiles objMetaDataFiles = new MetaDataFiles();
                        foreach (KeyValuePair<string, string> item in metadata)
                        {
                            if (item.Key == "Title")
                            {
                                objMetaDataFiles.Title = item.Value;
                            }
                            else if (item.Key == "Author")
                            {
                                objMetaDataFiles.Author = item.Value;
                            }
                            else if (item.Key == "Subject")
                            {
                                objMetaDataFiles.Subject = item.Value;
                            }
                            else if (item.Key == "Keywords")
                            {
                                objMetaDataFiles.Keywords = item.Value;
                            }
                            else if (item.Key == "CreationDate")
                            {
                                objMetaDataFiles.FileCreationDate = item.Value;
                            }
                            else if (item.Key == "ModDate")
                            {
                                objMetaDataFiles.FileModificationDate = item.Value;
                            }
                            else if (item.Key == "Pages")
                            {
                                PdfPages = Utility.CommonHelper.GetDBInt(item.Value);
                                objMetaDataFiles.Pages = PdfPages;
                            }
                        }
                        objMetaDataFiles.VersionID = Utility.CommonHelper.GetDBInt(id);
                        objMetaDataFiles.databasename = databasename;
                        string MetadataID = objMetaDataFiles.Insert();

                        objJobsFilesVersions = new JobsFilesVersions();
                        objJobsFilesVersions.ID = Utility.CommonHelper.GetDBInt(id);
                        objJobsFilesVersions.MetaDataID = Utility.CommonHelper.GetDBInt(MetadataID);
                        objJobsFilesVersions.databasename = databasename;
                        objJobsFilesVersions.UpdateMetaDataID();

                        objJobsFiles = new JobsFiles();
                        objJobsFiles.ID = Utility.CommonHelper.GetDBInt(fileID);
                        objJobsFiles.Pages = Utility.CommonHelper.GetDBInt(PdfPages);
                        objJobsFiles.databasename = databasename;
                        objJobsFiles.UpdatePages();
                    }
                }

            }



            return "file uploaded successfully";
        }

        private async Task<string> downloadUrlFile(string filePath, string files)
        {
            if (!System.IO.File.Exists(filePath))
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadFileCallback2);
                await webClient.DownloadFileTaskAsync(new Uri(files), filePath);
                return "";
            }
            else
            {
                return "";
            }
        }

        private static string GetFileSize(Uri uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                var fileSizeInMegaByte = Math.Round(Convert.ToDouble(fileSize) / 1024.0 / 1024.0, 2);
                return fileSize.ToString();// + " MB";
            }
        }
        public static string FileSaveToUrl()
        {
            string folderName = @"C:\";
            string pathString = System.IO.Path.Combine(folderName, "storefolder");
            Directory.CreateDirectory(pathString);
            string Extension = System.IO.Path.GetExtension(@"http://www.africau.edu/images/default/sample.pdf");
            string fileName = System.IO.Path.GetRandomFileName() + Extension;
            pathString = System.IO.Path.Combine(pathString, fileName);

            if (!System.IO.File.Exists(pathString))
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadFileCallback2);
                webClient.DownloadFileAsync(new Uri(@"http://www.africau.edu/images/default/sample.pdf"), pathString);
                return "";
            }
            else
            {
                return "File " + fileName + " already exists.";
            }
        }

        public string updateLegacyIcon(int clientid, IFormFile files)
        {
            string uploadFilePath = string.Empty;
            string wwwPath = this.webHostEnvironment.WebRootPath;
            string contentPath = this.webHostEnvironment.ContentRootPath;

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, "includes");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string path1 = Path.Combine(path, "SetaPdf");
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            string path2 = Path.Combine(path1, "Stamps");
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
            string filename = clientid.ToString() + ".pdf";

            if (System.IO.File.Exists(Path.Combine(path2, filename)))
            {
                System.IO.File.Delete(Path.Combine(path2, filename));
            }
            using (FileStream stream = new FileStream(Path.Combine(path2, filename), FileMode.Create))
            {
                files.CopyTo(stream);

            }
            ADLegacyCustom objADLegacyCustom = new ADLegacyCustom();
            objADLegacyCustom.ClientID = clientid.ToString();
            objADLegacyCustom.icon = Path.Combine(path2, filename);
            objADLegacyCustom.databasename = databasename;
            objADLegacyCustom.UpdateIconLegacyCustom();
            return "";
        }
        private static void DownloadFileCallback2(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("File download cancelled.");
            }
            else
            {

            }

            if (e.Error != null)
            {
                Console.WriteLine(e.Error.ToString());
            }
        }

    }
}
