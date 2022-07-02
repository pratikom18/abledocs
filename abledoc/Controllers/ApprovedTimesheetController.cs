using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
//using Excel = Microsoft.Office.Interop.Excel;

namespace abledoc.Controllers
{
    public class ApprovedTimesheetController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public ApprovedTimesheetController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            // ViewBag.UserID = "8";
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            Dictionary<string, string> StatusList = new Dictionary<string, string>();

            ViewBag.UserRolesList = UserRolesList;

            ViewBag.UserID = Utility.Session.SessionExtensions.GetComplexData<string>(HttpContext.Session, "ID");
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult getApprovedTimesheet(JQueryDataTableParamModel param, int supervisorID, string monthVal, string yearVal, int ApprovedFinal)
        {
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            bool show = false;
            if (UserRolesList.Where(x => x.RoleName == "ApprovedTimesheet").Count() > 0)
            {
                show = true;
            }

            int totalRecords = 0;
            List<TimesheetTracking> allRecord = new List<TimesheetTracking>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            TimesheetTracking objSearch = new TimesheetTracking();
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
            var databasename = HttpContext.Session.GetString("Database");
            objSearch.databasename = databasename;
            allRecord = objSearch.GetSearchListForTable(startIndex, endIndex, OrderBy, SearchKey, supervisorID, monthVal, yearVal, ApprovedFinal, supervisorID, show, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}


            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString().Trim(),
                             c.UserID.ToString(),
                             c.BillableDuration == null ? "" : c.BillableDuration,
                             c.TimesheetID == null? "": c.TimesheetID,
                             c.FullName == null ? "" : c.FullName,
                             c.plainHourLimit == null? Utility.CommonHelper.GetDBString(0.00) :(c.TotalHourRounded - (c.OT3+c.OT2+c.OT1)) > 80 ? Utility.CommonHelper.GetDBString(80) : Utility.CommonHelper.GetDBString(c.TotalHourRounded - (c.OT3+c.OT2+c.OT1)),
                             c.overTimeHours == null ? "0.00" : (c.TotalHourRounded - (c.OT3+c.OT2+c.OT1)) > 80 ?((c.TotalHourRounded - (c.OT3+c.OT2+c.OT1)) - 80).ToString() : "0.00",
                             c.OT1 == null ? "0.00" : c.OT1.ToString(),
                             c.OT3 == null ? "0.00" :c.OT3.ToString(),
                             c.OT2 == null ? "0.00":c.OT2.ToString(),
                             c.TotalHourRounded == null?"0.00":c.TotalHourRounded.ToString()
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
        public JsonResult updateapprovedFinal(string State, string Params)
        {
            int valFlag = 0;
            if (State == "ToPending")
            {
                valFlag = 0;
            }
            else if (State == "ToResolved")
            {
                valFlag = 1;
            }
            //var databasename = HttpContext.Session.GetString("Database");
            TimesheetTracking timesheetTracking = new TimesheetTracking();
            //timesheetTracking.databasename = databasename;
            timesheetTracking.UpdateApprovedFinal(valFlag.ToString(), Params);
            var jsonResult = Json(new
            {
                message = "success"
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult totalweek(int ID) {
            ViewBag.UserID = ID;
            Users users = new Users();
            users = users.GetUserById(ID);
            ViewBag.name = users.FirstName + " " + users.LastName;
            return PartialView("_TotalWeeks");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult DaysPerEmployee()
        {

            return PartialView("_DaysPerEmployee");
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult UserWorkedDataWeekly(string loggedInUser, string dateRange, string timesheetID, string mode, string flag, int updatedSupervisionFlag)
        {
            var databasename = HttpContext.Session.GetString("Database");

            ViewBag.loggedInUser = loggedInUser;
            ViewBag.dateRange = dateRange;
            ViewBag.timesheetID = timesheetID;
            ViewBag.mode = mode;


            ViewBag.flag = flag;
            ApprovedTimesheet approvedTimesheet = new ApprovedTimesheet();

            if (flag == "timesheet")
            {
                mode = "";
                DateTime dt = Utility.CommonHelper.GetDBDate(dateRange);
                ERPClass eRPClass1 = new ERPClass();
                List<string> dateArray = eRPClass1.GetDatesFromRange1(abledoc.Utility.CommonHelper.GetDBDate(dateRange), dt.AddDays(13));
                approvedTimesheet.dateRange = dateArray;
                dateRange = string.Empty;
                foreach (string str in dateArray)
                {
                    if (dateRange == "")
                    {
                        dateRange = str;
                    }
                    else {
                        dateRange += "," + str;
                    }
                }

            }



            
            ERPClass eRPClass = new ERPClass();
            string retval = ERPClass.UserWorkedDataWeekly(databasename, loggedInUser, dateRange, timesheetID, mode, ref approvedTimesheet);

            string state = string.Empty;
            var fullTimesheet = retval;
            var splitFullTimesheet = retval.Split(" ||| ");
            var weeklyHourList = splitFullTimesheet[0];
            var carriedForwardString = splitFullTimesheet[1];
            var clubbedFile = splitFullTimesheet[2];
            var dateRange1 = splitFullTimesheet[3];


            var eachCarriedForward = carriedForwardString.Split(" || ");
            var eachDayClubbedFile = clubbedFile.Split(" |||| ");

            double carriedForwardHour = 0;
            double actualTimeGlobal = 0;
            string[] weeklyHourEach = approvedTimesheet.weeklyHourList.ToArray();
            var dateRangeSplit = dateRange.Split(", ");
            string startDate = dateRangeSplit[0];
            string endDate = dateRangeSplit[dateRangeSplit.Length - 1];
            string dayString = "";

            if (flag == "timesheet")
            {
                var dateRangeSplit1 = dateRange.Split(",");
                string startDate1 = dateRangeSplit1[0];
                string endDate1 = dateRangeSplit1[dateRangeSplit1.Length - 1];
                approvedTimesheet.timesheetWeekRange = startDate1 + " to " + endDate1;
            }
            else {
                approvedTimesheet.timesheetWeekRange = dateRange.Replace(",", " to ");
            }

                
            var totalBillableHour = 0.00;
            //string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            approvedTimesheet.weekDays = weekDays.ToList();
            if (approvedTimesheet.carriedForwardFileStringsList.Count > 0)
            {
                foreach (carriedForwardFileString item in approvedTimesheet.carriedForwardFileStringsList)
                {

                    var dbActualTime = item.ActualTime;
                    var dbOverrideTime = item.OverrideTime.ToString();
                    double textValTime = 0;
                    if (dbOverrideTime != "0.00")
                    {
                        textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(dbOverrideTime), 2);
                    }
                    else
                    {
                        textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(item.textValTime), 2);
                    }


                    totalBillableHour = totalBillableHour + textValTime;
                    actualTimeGlobal = actualTimeGlobal + Math.Round(Utility.CommonHelper.GetDBDouble(item.textValTime), 2);
                }
            }
            approvedTimesheet.carriedForwardTotal = totalBillableHour.ToString();


            for (var i = 0; i < weeklyHourEach.Length - 1; i++)
            {
                if (weeklyHourEach[i] != "0")
                {

                    List<List<clubbedFile>> eachFileData = approvedTimesheet.clubbedFileList1;
                    //for (var j = 0; j < eachFileData.Count(); j++)
                    //{

                        List<clubbedFile> eachFile = eachFileData[i];
                        for (var l = 0; l < eachFile.Count() ; l++)
                        {
                            var dbActualTime = eachFile[l].ActualTime;
                            var dbOverrideTime = eachFile[l].OverrideTime;
                            double textValTime = 0;
                            if (dbOverrideTime != 0.00)
                            {
                                textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(dbOverrideTime), 2);
                            }
                            else
                            {
                                textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(eachFile[l].textValTime), 2);
                            }


                            totalBillableHour = totalBillableHour + textValTime;
                            actualTimeGlobal = actualTimeGlobal + Math.Round(Utility.CommonHelper.GetDBDouble(eachFile[l].textValTime), 2);
                        }
                           
                    //}

                }
            }


            double floorNum = Math.Floor(totalBillableHour);
            var afterDecimal = totalBillableHour - floorNum;
            double validHour = 0;
            afterDecimal = Math.Round(afterDecimal, 2);
            //alert(afterDecimal);
            if (afterDecimal == 0.00)
            {
                validHour = floorNum;
            }
            else if (afterDecimal >= 0.01 && afterDecimal < 0.13)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.13 && afterDecimal < 0.37)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.37 && afterDecimal < 0.63)
            {
                validHour = floorNum + 0.50;
            }
            else if (afterDecimal >= 0.63 && afterDecimal < 0.87)
            {
                validHour = floorNum + 0.75;
            }
            else
            {
                validHour = floorNum + 1;
            }

            approvedTimesheet.billableHours = Math.Round(totalBillableHour, 2).ToString();
            approvedTimesheet.equivalentHours = Math.Round(validHour, 2).ToString();

            if (updatedSupervisionFlag == 1)
            {
                TimesheetTracking timesheetTracking = new TimesheetTracking();
                timesheetTracking.ID = Utility.CommonHelper.GetDBInt(timesheetID);
                double hour = totalBillableHour;
                double totalHourRounded = RoundedOffHour(hour);
                timesheetTracking.databasename = databasename;
                timesheetTracking.UpdateTotalHour();
            }
            if (flag == "timesheet")
            {
                return PartialView("_TimesheetDetail", approvedTimesheet);
            }
            else {
                return PartialView("_ApprovedTimesheet", approvedTimesheet);
            }

        }

        // [HttpPost]
        [LoginAuthorized]
        public IActionResult TimesheetView()
        {
            
            return View();
        }
        [LoginAuthorized]
        public IActionResult Downloadxl(string loggedInUser, string dateRange, string timesheetID, string mode,string flag)
        {
            var databasename = HttpContext.Session.GetString("Database");

            ApprovedTimesheet approvedTimesheet = new ApprovedTimesheet();

            if (flag == "timesheet")
            {
                mode = "";
                DateTime dt = Utility.CommonHelper.GetDBDate(dateRange);
                ERPClass eRPClass1 = new ERPClass();
                List<string> dateArray = eRPClass1.GetDatesFromRange1(abledoc.Utility.CommonHelper.GetDBDate(dateRange), dt.AddDays(13));
                approvedTimesheet.dateRange = dateArray;
                dateRange = string.Empty;
                foreach (string str in dateArray)
                {
                    if (dateRange == "")
                    {
                        dateRange = str;
                    }
                    else
                    {
                        dateRange += "," + str;
                    }
                }

            }

            ERPClass eRPClass = new ERPClass();
            string retval = ERPClass.UserWorkedDataWeekly(databasename,loggedInUser, dateRange, timesheetID, mode, ref approvedTimesheet);

            string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            approvedTimesheet.weekDays = weekDays.ToList();

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ApprovedTimesheet");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filename = "Timesheet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
           // string fullpath = Path.Combine(path, filename);
             var stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("Timesheet");
               
                
                var currentRow = 1;

                //using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                //{
                //    Rng.Style.Font.Bold = true;
                //    //Rng.Style.Font.Color.SetColor(Color.Red);
                //    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                //    Rng.AutoFitColumns();
                //}

                if (approvedTimesheet.carriedForwardFileStringsList.Count() > 0)
                {
                    workSheet.Cells[currentRow, 1].Value = "Carried Forward";
                    using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                    {
                        Rng.Style.Font.Bold = true;
                        Rng.Merge = true;
                        Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }
                    currentRow = currentRow + 1;
                    
                    workSheet.Cells[currentRow, 1].Value = "Date";
                    workSheet.Cells[currentRow, 2].Value = "Engagement #";
                    workSheet.Cells[currentRow, 3].Value = "Hours";
                    workSheet.Cells[currentRow, 4].Value = "Staff Comments";
                    workSheet.Cells[currentRow, 5].Value = "Supervisor Comments";
                    using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                    {
                        Rng.Style.Font.Bold = true;
                        //Rng.Style.Font.Color.SetColor(Color.Red);
                        Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        Rng.AutoFitColumns();
                    }

                    foreach (carriedForwardFileString item in approvedTimesheet.carriedForwardFileStringsList.OrderBy(x => x.justDate).ToList())
                    {
                        currentRow++;

                        double textValTime = 0.00;
                        if (item.OverrideTime != 0.00)
                        {
                            textValTime = item.OverrideTime;
                        }
                        else
                        {
                            textValTime = item.textValTime;
                        }

                        workSheet.Cells[currentRow, 1].Value = item.justDate;
                        workSheet.Cells[currentRow, 2].Value = item.QueryType;
                        workSheet.Cells[currentRow, 3].Value = textValTime;
                        workSheet.Cells[currentRow, 4].Value = item.Comment;
                        workSheet.Cells[currentRow, 5].Value = item.SupervisorComment;
                    }

                    using (ExcelRange Rng = workSheet.Cells[1, 1, currentRow, 5])
                    {
                        Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Top.Color.SetColor(Color.Black);
                        Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Left.Color.SetColor(Color.Black);
                        Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Right.Color.SetColor(Color.Black);
                        Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                }
                
                
                

                int toprow = currentRow + 2;
                currentRow = currentRow + 2;
                workSheet.Cells[currentRow, 1].Value = "Current Selected";
                using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                {
                    Rng.Style.Font.Bold = true;
                    Rng.Merge = true;
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                //currentRow = currentRow + 1;
                for (int i = 0; i < 14 && i < 15; i++)
                {
                    currentRow++;
                    workSheet.Cells[currentRow, 1].Value = approvedTimesheet.dateRange[i] + "-" + approvedTimesheet.weekDays[i];
                    using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                    {
                        Rng.Style.Font.Bold = true;
                        //Rng.Style.Font.Color.SetColor(Color.Red);
                        Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        Rng.Merge = true;
                    }

                    if (approvedTimesheet.clubbedFileList1[i].Count() > 0 )//  != null)
                    {
                        List<clubbedFile> ListclubbedFile = approvedTimesheet.clubbedFileList1[i];

                        if (ListclubbedFile.Count > 0)
                        {

                            currentRow++;
                            
                            workSheet.Cells[currentRow, 2].Value = "Engagement #";
                            workSheet.Cells[currentRow, 3].Value = "Hours";
                            workSheet.Cells[currentRow, 4].Value = "Comment";
                            workSheet.Cells[currentRow, 5].Value = "Supervisor Comment";
                            using (ExcelRange Rng = workSheet.Cells[currentRow, 1, currentRow, 5])
                            {
                                Rng.AutoFitColumns();
                            }

                            foreach (clubbedFile item in ListclubbedFile)
                            {
                                currentRow++;

                                double textValTime = 0.00;
                                if (item.OverrideTime != 0.00)
                                {
                                    textValTime = item.OverrideTime;
                                }
                                else
                                {
                                    textValTime = item.textValTime;
                                }

                                workSheet.Cells[currentRow, 2].Value = item.QueryType;
                                workSheet.Cells[currentRow, 3].Value = textValTime;
                                workSheet.Cells[currentRow, 4].Value = item.Comment;
                                workSheet.Cells[currentRow, 5].Value = item.SupervisorComment;
                            }
                        }

                    }

                }
                using (ExcelRange Rng = workSheet.Cells[toprow, 1, currentRow, 5])
                {
                    Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Top.Color.SetColor(Color.Black);
                    Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Left.Color.SetColor(Color.Black);
                    Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Right.Color.SetColor(Color.Black);
                    Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                package.Save();
                

            }

            stream.Position = 0;
            var contentType = "application/octet-stream";
            var fileName = filename;
            return File(stream, contentType, fileName);

        }
        [LoginAuthorized]
        public IActionResult SendforApproval(string loggedInUser, string dateRange, string timesheetID, string mode)
        {
            var databasename = HttpContext.Session.GetString("Database");
            ApprovedTimesheet approvedTimesheet = new ApprovedTimesheet();

            ERPClass eRPClass = new ERPClass();
            string retval = ERPClass.UserWorkedDataWeekly(databasename,loggedInUser, dateRange, timesheetID, mode, ref approvedTimesheet);

            string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            approvedTimesheet.weekDays = weekDays.ToList();

            string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ApprovedTimesheet");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filename = "ApprovedTimesheet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            // string fullpath = Path.Combine(path, filename);
            var stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("Timesheet");
                workSheet.Cells.AutoFitColumns();

                workSheet.Cells[1, 1].Value = "Current Selected";

                var currentRow = 2;
                for (int i = 0; i < 14 && i < 15; i++)
                {
                    currentRow++;
                    workSheet.Cells[currentRow, 1].Value = approvedTimesheet.dateRange[i] + "-" + approvedTimesheet.weekDays[i];


                    if (approvedTimesheet.clubbedFileList1[i] != null)
                    {
                        List<clubbedFile> ListclubbedFile = approvedTimesheet.clubbedFileList1[i];

                        if (ListclubbedFile.Count > 0)
                        {

                            currentRow++;
                            workSheet.Cells[currentRow, 2].Value = "Engagement #";
                            workSheet.Cells[currentRow, 3].Value = "Hours";
                            workSheet.Cells[currentRow, 4].Value = "Comment";
                            workSheet.Cells[currentRow, 5].Value = "Supervisor Comment";

                            foreach (clubbedFile item in ListclubbedFile)
                            {
                                currentRow++;

                                double textValTime = 0.00;
                                if (item.OverrideTime != 0.00)
                                {
                                    textValTime = item.OverrideTime;
                                }
                                else
                                {
                                    textValTime = item.textValTime;
                                }

                                workSheet.Cells[currentRow, 2].Value = item.QueryType;
                                workSheet.Cells[currentRow, 3].Value = textValTime;
                                workSheet.Cells[currentRow, 4].Value = item.Comment;
                                workSheet.Cells[currentRow, 5].Value = item.SupervisorComment;
                            }
                        }

                    }

                }

                package.Save();


            }


            string bcEmail = "";
            string bcName = "Test Test";

            string ccEmail = "";
            string ccName = "Test Test";

            string sTo = "Test";
           
            byte[] fileContents = null;
            stream.Position = 0;
            fileContents = stream.ToArray();


            Attachment attachment = Utility.EmailHelper.GetAttachFile(fileContents, filename);

            EmailTemplate modelTemplate = new EmailTemplate();
            modelTemplate = modelTemplate.GetTemplateByLang("EN", "Send_For_Approval");
            string deliveryMessage = string.Empty;
            string actual = modelTemplate.DeliveryEmail;
            deliveryMessage = actual.Replace("[FirstName]", bcName);
            deliveryMessage = Utility.CommonHelper.Nl2br(deliveryMessage);

            string message = string.Empty;
            message = deliveryMessage;

            string subject = modelTemplate.subject;

            Utility.EmailHelper.Email("US", sTo, ccEmail, bcEmail, subject, message, attachment);

            //Utility.EmailHelper.EmailTimeSheet(ccEmail, bcEmail, ccName, bcName, timesheetID, attachment);

            string msg = "Sucess";


            return Json(new {msg});

            //stream.Position = 0;
            //var contentType = "application/octet-stream";
            //var fileName = filename;
            //return File(stream, contentType, fileName);

        }


        //public IActionResult Downloadxl1(string loggedInUser, string dateRange, string timesheetID, string mode)
        //{
        //    ApprovedTimesheet approvedTimesheet = new ApprovedTimesheet();

        //    //   ERPClass eRPClass = new ERPClass();
        //    string retval = ERPClass.UserWorkedDataWeekly(loggedInUser, dateRange, timesheetID, mode, ref approvedTimesheet);

        //    string path = Path.Combine(this.webHostEnvironment.WebRootPath, "ApprovedTimesheet");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string filename = "ApprovedTimesheet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        //    string fullpath = Path.Combine(path, filename);


        //    Excel.Application xlApp = new Excel.Application();

        //    Microsoft.Office.Interop.Excel.Workbook excelWorkBook =
        //       xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);

        //    Microsoft.Office.Interop.Excel.Worksheet workSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
        //    workSheet.Name = Convert.ToString("Timesheet");
        //    workSheet.Columns.AutoFit();

        //    workSheet.Name = "download"; //name of excel file

        //    Microsoft.Office.Interop.Excel.Range rng;
        //    rng = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[1, 1];
        //    rng.Font.Bold = true;
        //    rng.Font.Color= System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
        //    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGray);
        //    rng.BorderAround();

        //    workSheet.Cells[1, 1] = "Current Selected";

        //    var currentRow = 2;
        //    for (int i = 0; i < 14 && i < 15; i++)
        //    {
        //        currentRow++;
        //        rng = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[currentRow, 1];
        //        rng.Font.Bold = true;
        //        rng.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
        //        rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGray);
        //        rng.BorderAround();

        //        workSheet.Cells[currentRow, 1] = approvedTimesheet.dateRange[i] + "-" + approvedTimesheet.weekDays[i];


        //        if (approvedTimesheet.clubbedFileList1[i] != null)
        //        {
        //            List<clubbedFile> ListclubbedFile = approvedTimesheet.clubbedFileList1[i];

        //            if (ListclubbedFile.Count > 0)
        //            {

        //                currentRow++;
        //                workSheet.Cells[currentRow, 2] = "Engagement #";
        //                workSheet.Cells[currentRow, 3] = "Hours";
        //                workSheet.Cells[currentRow, 4] = "Comment";
        //                workSheet.Cells[currentRow, 5] = "Supervisor Comment";

        //                foreach (clubbedFile item in ListclubbedFile)
        //                {
        //                    currentRow++;

        //                    double textValTime = 0.00;
        //                    if (item.OverrideTime != 0.00)
        //                    {
        //                        textValTime = item.OverrideTime;
        //                    }
        //                    else
        //                    {
        //                        textValTime = item.textValTime;
        //                    }

        //                    workSheet.Cells[currentRow, 2] = item.QueryType;
        //                    workSheet.Cells[currentRow, 3] = textValTime;
        //                    workSheet.Cells[currentRow, 4] = item.Comment;
        //                    workSheet.Cells[currentRow, 5] = item.SupervisorComment;
        //                }
        //            }

        //        }

        //    }


        //    workSheet.SaveAs(fullpath);

        //    xlApp.Visible = false;
        //    xlApp.UserControl = false;
        //    //excelWorkBook.SaveAs(fullpath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
        //    //        false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
        //    //        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

        //    //excelWorkBook.Close(null, null, null);

        //    xlApp.Quit();  //MainWindowTitle will become empty afer being close

        //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
        //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkBook);

        //    Process[] excelProcesses = Process.GetProcessesByName("excel");
        //    foreach (Process p in excelProcesses)
        //    {
        //        if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes 
        //        {
        //            p.Kill();
        //        }
        //    }

        //    using (var ms = new MemoryStream())
        //    {
        //        byte[] fileContents = null;
        //        var sw = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
        //        ms.Position = 0;
        //        sw.CopyTo(ms);
        //        fileContents = ms.ToArray();
        //        return File(fileContents, "application/vnd.ms-excel", filename);
        //    }

        //    //var stream = new MemoryStream();

        //    //using (FileStream fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
        //    //{
        //    //    fileStream.CopyTo(stream);
        //    //}

        //    //   stream.Position = 0;
        //    ////   var contentType = "application/octet-stream";
        //    ////   var fileName = filename;
        //    ////  return File(stream, contentType, fileName);

        //    //   using (MemoryStream ms = new MemoryStream())
        //    //   {

        //    //      // excelWorkBook.SaveAs(ms);
        //    //      // excelWorkBook.Close();

        //    //       //Here you delete the saved file
        //    //       //File.Delete(filename);
        //    //       ms.Position = 0;
        //    //       return File(ms, "application/vnd.ms-excel", filename);

        //    //   }

        //}
        [LoginAuthorized]
        public double RoundedOffHour(double num)
        {
            double floorNum = num;
            double afterDecimal = num - floorNum;

            afterDecimal = Math.Round(afterDecimal, 2);

            double validHour = 0;
            if (afterDecimal == 0.00)
            {
                validHour = floorNum;
            }
            else if (afterDecimal >= 0.01 && afterDecimal < 0.13)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.13 && afterDecimal < 0.37)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.37 && afterDecimal < 0.63)
            {
                validHour = floorNum + 0.50;
            }
            else if (afterDecimal >= 0.63 && afterDecimal < 0.87)
            {
                validHour = floorNum + 0.75;
            }
            else
            {
                validHour = floorNum + 1;
            }

            return validHour;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult getTotalHours(JQueryDataTableParamModel param, int supervisorID, string monthVal, string yearVal, int ApprovedFinal)
        {
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            bool show = false;
            if (UserRolesList.Where(x => x.RoleName == "ApprovedTimesheet").Count() > 0)
            {
                show = true;
            }

            int totalRecords = 0;
            List<TimesheetTracking> allRecord = new List<TimesheetTracking>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            TimesheetTracking objSearch = new TimesheetTracking();
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
            var databasename = HttpContext.Session.GetString("Database");

            objSearch.databasename = databasename;
            allRecord = objSearch.GetTotalHoursTable(startIndex, endIndex, OrderBy, SearchKey, supervisorID, monthVal, yearVal, ApprovedFinal, supervisorID, show, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}


            var result = from c in allRecord
                         select new[] {
                             //c.ID.ToString().Trim(),
                             c.UserID.ToString(),
                             //c.BillableDuration == null ? "" : c.BillableDuration,
                            // c.TimesheetID == null? "": c.TimesheetID,
                             c.FullName == null ? "" : c.FullName,
                             //c.plainHourLimit == null? Utility.CommonHelper.GetDBString(0.00) :(c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) > 80 ? Utility.CommonHelper.GetDBString(80) : Utility.CommonHelper.GetDBString(c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)),
                             c.overTimeHours == null ? "0.00" : (c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) > 80 ?((c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) - 80).ToString() : "0.00",
                             c.OT1 == 0 ? "0.00" : c.OT1.ToString(),
                             c.OT2 == 0 ? "0.00" :c.OT2.ToString(),
                             c.OT3 == 0 ? "0.00":c.OT3.ToString(),
                             c.OT4 == 0 ? "0.00":c.OT4.ToString(),
                             c.OT5 == 0 ? "0.00":c.OT5.ToString(),
                             c.OT6 == 0 ? "0.00":c.OT6.ToString(),
                            // c.TotalHourRounded == null?"0.00":c.TotalHourRounded.ToString()
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
        public JsonResult getTotalWeeks(JQueryDataTableParamModel param, int supervisorID, string monthVal, string yearVal, int ApprovedFinal)
        {
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            bool show = false;
            if (UserRolesList.Where(x => x.RoleName == "ApprovedTimesheet").Count() > 0)
            {
                show = true;
            }

            int totalRecords = 0;
            List<TimesheetTracking> allRecord = new List<TimesheetTracking>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            TimesheetTracking objSearch = new TimesheetTracking();
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
            var databasename = HttpContext.Session.GetString("Database");
            objSearch.databasename = databasename;
            allRecord = objSearch.GetTotalWeeksTable(startIndex, endIndex, OrderBy, SearchKey, supervisorID, monthVal, yearVal, ApprovedFinal, supervisorID, show, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}


            var result = from c in allRecord
                         select new[] {
                             //c.ID.ToString().Trim(),
                             c.UserID.ToString(),
                            // c.TimesheetID == null? "": c.TimesheetID,
                             c.FullName == null ? "" : c.FullName,
                             //c.plainHourLimit == null? Utility.CommonHelper.GetDBString(0.00) :(c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) > 80 ? Utility.CommonHelper.GetDBString(80) : Utility.CommonHelper.GetDBString(c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)),
                             c.overTimeHours == null ? "0.00" : (c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) > 80 ?((c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) - 80).ToString() : "0.00",
                             c.OT1 == 0 ? "0.00" : c.OT1.ToString(),
                             c.OT2 == 0 ? "0.00" :c.OT2.ToString(),
                             c.OT3 == 0 ? "0.00":c.OT3.ToString(),
                             c.OT4 == 0 ? "0.00":c.OT4.ToString(),
                             c.OT5 == 0 ? "0.00":c.OT5.ToString(),
                             c.OT6 == 0 ? "0.00":c.OT6.ToString(),
                             c.BillableDuration == null ? "" : c.BillableDuration
                            // c.TotalHourRounded == null?"0.00":c.TotalHourRounded.ToString()
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
        public JsonResult getTotalWeeksbyUser(JQueryDataTableParamModel param, int supervisorID, string monthVal, string yearVal, int ApprovedFinal,int userID)
        {
            List<UserRoles> UserRolesList = new List<UserRoles>();
            UserRolesList = Utility.Session.SessionExtensions.GetComplexData<List<UserRoles>>(HttpContext.Session, "UserRoles");
            bool show = false;
            if (UserRolesList.Where(x => x.RoleName == "ApprovedTimesheet").Count() > 0)
            {
                show = true;
            }

            int totalRecords = 0;
            List<TimesheetTracking> allRecord = new List<TimesheetTracking>();

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            TimesheetTracking objSearch = new TimesheetTracking();
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
            var databasename = HttpContext.Session.GetString("Database");
            objSearch.databasename = databasename;
            allRecord = objSearch.GetTotalWeeksByUsersTable(startIndex, endIndex, OrderBy, SearchKey, supervisorID, monthVal, yearVal, ApprovedFinal, userID, show, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}


            var result = from c in allRecord
                         select new[] {
                             //c.ID.ToString().Trim(),
                             c.UserID.ToString(),
                            // c.TimesheetID == null? "": c.TimesheetID,
                             c.FullName == null ? "" : c.FullName,
                             //c.plainHourLimit == null? Utility.CommonHelper.GetDBString(0.00) :(c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) > 80 ? Utility.CommonHelper.GetDBString(80) : Utility.CommonHelper.GetDBString(c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)),
                             c.overTimeHours == null ? "0.00" : (c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) > 80 ?((c.TotalHourRounded - (c.OT1+c.OT2+c.OT3+c.OT4+c.OT5+c.OT6)) - 80).ToString() : "0.00",
                             c.OT1 == 0 ? "0.00" : c.OT1.ToString(),
                             c.OT2 == 0 ? "0.00" :c.OT2.ToString(),
                             c.OT3 == 0 ? "0.00":c.OT3.ToString(),
                             c.OT4 == 0 ? "0.00":c.OT4.ToString(),
                             c.OT5 == 0 ? "0.00":c.OT5.ToString(),
                             c.OT6 == 0 ? "0.00":c.OT6.ToString(),
                             c.BillableDuration == null ? "" : c.BillableDuration,
                             c.ID.ToString().Trim()
                            // c.TotalHourRounded == null?"0.00":c.TotalHourRounded.ToString()
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
        public PartialViewResult UserWorkedDataWeeklyNew(string loggedInUser, string dateRange, string timesheetID, string mode, string flag, int updatedSupervisionFlag)
        {
            var databasename = HttpContext.Session.GetString("Database");

            ViewBag.loggedInUser = loggedInUser;
            ViewBag.dateRange = dateRange;
            ViewBag.timesheetID = timesheetID;
            ViewBag.mode = mode;


            ViewBag.flag = flag;
            ApprovedTimesheet approvedTimesheet = new ApprovedTimesheet();

            if (flag == "timesheet")
            {
                mode = "";
                DateTime dt = Utility.CommonHelper.GetDBDate(dateRange);
                ERPClass eRPClass1 = new ERPClass();
                List<string> dateArray = eRPClass1.GetDatesFromRange1(abledoc.Utility.CommonHelper.GetDBDate(dateRange), dt.AddDays(6));
                approvedTimesheet.dateRange = dateArray;
                dateRange = string.Empty;
                foreach (string str in dateArray)
                {
                    if (dateRange == "")
                    {
                        dateRange = str;
                    }
                    else
                    {
                        dateRange += "," + str;
                    }
                }

            }

            ERPClass eRPClass = new ERPClass();
            string retval = ERPClass.UserWorkedDataWeekly(databasename,loggedInUser, dateRange, timesheetID, mode, ref approvedTimesheet);

            string state = string.Empty;
            var fullTimesheet = retval;
            var splitFullTimesheet = retval.Split(" ||| ");
            var weeklyHourList = splitFullTimesheet[0];
            var carriedForwardString = splitFullTimesheet[1];
            var clubbedFile = splitFullTimesheet[2];
            var dateRange1 = splitFullTimesheet[3];


            var eachCarriedForward = carriedForwardString.Split(" || ");
            var eachDayClubbedFile = clubbedFile.Split(" |||| ");

            double carriedForwardHour = 0;
            double actualTimeGlobal = 0;
            var weeklyHourEach = weeklyHourList.Split(" | ");
            var dateRangeSplit = dateRange.Split(", ");
            string startDate = dateRangeSplit[0];
            string endDate = dateRangeSplit[dateRangeSplit.Length - 1];
            string dayString = "";

            if (flag == "timesheet")
            {
                var dateRangeSplit1 = dateRange.Split(",");
                string startDate1 = dateRangeSplit1[0];
                string endDate1 = dateRangeSplit1[dateRangeSplit1.Length - 1];
                approvedTimesheet.timesheetWeekRange = startDate1 + " to " + endDate1;
            }
            else
            {
                approvedTimesheet.timesheetWeekRange = dateRange.Replace(",", " to ");
            }


            var totalBillableHour = 0.00;
            string[] weekDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            //string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            approvedTimesheet.weekDays = weekDays.ToList();
            if (approvedTimesheet.carriedForwardFileStringsList.Count > 0)
            {
                foreach (carriedForwardFileString item in approvedTimesheet.carriedForwardFileStringsList)
                {

                    var dbActualTime = item.ActualTime;
                    var dbOverrideTime = item.OverrideTime.ToString();
                    double textValTime = 0;
                    if (dbOverrideTime != "0.00")
                    {
                        textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(dbOverrideTime), 2);
                    }
                    else
                    {
                        textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(item.textValTime), 2);
                    }


                    totalBillableHour = totalBillableHour + textValTime;
                    actualTimeGlobal = actualTimeGlobal + Math.Round(Utility.CommonHelper.GetDBDouble(item.textValTime), 2);
                }
            }



            for (var i = 0; i < weeklyHourEach.Length - 1; i++)
            {
                if (weeklyHourEach[i] != "0")
                {

                    var eachFileData = eachDayClubbedFile[i].Split(" || ");
                    for (var j = 0; j < eachFileData.Length - 1; j++)
                    {

                        var eachFile = eachFileData[j].Split(" | ");
                        var dbActualTime = eachFile[4];
                        var dbOverrideTime = eachFile[5];
                        double textValTime = 0;
                        if (dbOverrideTime != "0.00")
                        {
                            textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(dbOverrideTime), 2);
                        }
                        else
                        {
                            textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(eachFile[2]), 2);
                        }


                        totalBillableHour = totalBillableHour + textValTime;
                        actualTimeGlobal = actualTimeGlobal + Math.Round(Utility.CommonHelper.GetDBDouble(eachFile[2]), 2);
                    }

                }
            }


            double floorNum = Math.Floor(totalBillableHour);
            var afterDecimal = totalBillableHour - floorNum;
            double validHour = 0;
            afterDecimal = Math.Round(afterDecimal, 2);
            //alert(afterDecimal);
            if (afterDecimal == 0.00)
            {
                validHour = floorNum;
            }
            else if (afterDecimal >= 0.01 && afterDecimal < 0.13)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.13 && afterDecimal < 0.37)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.37 && afterDecimal < 0.63)
            {
                validHour = floorNum + 0.50;
            }
            else if (afterDecimal >= 0.63 && afterDecimal < 0.87)
            {
                validHour = floorNum + 0.75;
            }
            else
            {
                validHour = floorNum + 1;
            }

            approvedTimesheet.billableHours = Math.Round(totalBillableHour, 2).ToString();
            approvedTimesheet.equivalentHours = Math.Round(validHour, 2).ToString();

            if (updatedSupervisionFlag == 1)
            {
                TimesheetTracking timesheetTracking = new TimesheetTracking();
                timesheetTracking.ID = Utility.CommonHelper.GetDBInt(timesheetID);
                double hour = totalBillableHour;
                double totalHourRounded = RoundedOffHour(hour);
                timesheetTracking.databasename = databasename;
                timesheetTracking.UpdateTotalHour();
            }
            if (flag == "timesheet")
            {
                return PartialView("_TimesheetDetail", approvedTimesheet);
            }
            else
            {
                return PartialView("_ApprovedTimesheet", approvedTimesheet);
            }

        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult UserWorkedDataWeeklyNew1(string loggedInUser, string dateRange, string timesheetID, string mode, string flag, int updatedSupervisionFlag)
        {
            var databasename = Utility.CommonHelper.Getabledocs("0");

            Users users = new Users();
            users = users.GetUserById(Utility.CommonHelper.GetDBInt(loggedInUser));
            ViewBag.name = users.FirstName + " " + users.LastName;

            ViewBag.loggedInUser = loggedInUser;
            ViewBag.dateRange = dateRange;
            ViewBag.timesheetID = timesheetID;
            ViewBag.mode = mode;


            ViewBag.flag = flag;
            ApprovedTimesheet approvedTimesheet = new ApprovedTimesheet();

             ERPClass eRPClass = new ERPClass();
            string retval = ERPClass.UserWorkedDataWeekly(databasename,loggedInUser, dateRange, timesheetID, mode, ref approvedTimesheet);

            string state = string.Empty;
            var fullTimesheet = retval;
            var splitFullTimesheet = retval.Split(" ||| ");
            var weeklyHourList = splitFullTimesheet[0];
            var carriedForwardString = splitFullTimesheet[1];
            var clubbedFile = splitFullTimesheet[2];
            var dateRange1 = splitFullTimesheet[3];


            var eachCarriedForward = carriedForwardString.Split(" || ");
            var eachDayClubbedFile = clubbedFile.Split(" |||| ");

            double carriedForwardHour = 0;
            double actualTimeGlobal = 0;
            string[] weeklyHourEach = approvedTimesheet.weeklyHourList.ToArray();
            var dateRangeSplit = dateRange.Split(", ");
            string startDate = dateRangeSplit[0];
            string endDate = dateRangeSplit[dateRangeSplit.Length - 1];
            string dayString = "";

            
            approvedTimesheet.timesheetWeekRange = dateRange.Replace(",", " to ");
            
            var totalBillableHour = 0.00;
           // string[] weekDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            approvedTimesheet.weekDays = weekDays.ToList();
            if (approvedTimesheet.carriedForwardFileStringsList.Count > 0)
            {
                foreach (carriedForwardFileString item in approvedTimesheet.carriedForwardFileStringsList)
                {

                    var dbActualTime = item.ActualTime;
                    var dbOverrideTime = item.OverrideTime.ToString();
                    double textValTime = 0;
                    if (dbOverrideTime != "0.00")
                    {
                        textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(dbOverrideTime), 2);
                    }
                    else
                    {
                        textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(item.textValTime), 2);
                    }


                    totalBillableHour = totalBillableHour + textValTime;
                    actualTimeGlobal = actualTimeGlobal + Math.Round(Utility.CommonHelper.GetDBDouble(item.textValTime), 2);
                }
            }



            for (var i = 0; i < weeklyHourEach.Length - 1; i++)
            {
                if (weeklyHourEach[i] != "0")
                {

                    List<List<clubbedFile>> eachFileData = approvedTimesheet.clubbedFileList1;
                    //for (var j = 0; j < eachFileData.Count(); j++)
                    //{

                    List<clubbedFile> eachFile = eachFileData[i];
                    for (var l = 0; l < eachFile.Count(); l++)
                    {
                        var dbActualTime = eachFile[l].ActualTime;
                        var dbOverrideTime = eachFile[l].OverrideTime;
                        double textValTime = 0;
                        if (dbOverrideTime != 0.00)
                        {
                            textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(dbOverrideTime), 2);
                        }
                        else
                        {
                            textValTime = Math.Round(Utility.CommonHelper.GetDBDouble(eachFile[l].textValTime), 2);
                        }


                        totalBillableHour = totalBillableHour + textValTime;
                        actualTimeGlobal = actualTimeGlobal + Math.Round(Utility.CommonHelper.GetDBDouble(eachFile[l].textValTime), 2);
                    }

                    //}

                }
            }


            double floorNum = Math.Floor(totalBillableHour);
            var afterDecimal = totalBillableHour - floorNum;
            double validHour = 0;
            afterDecimal = Math.Round(afterDecimal, 2);
            //alert(afterDecimal);
            if (afterDecimal == 0.00)
            {
                validHour = floorNum;
            }
            else if (afterDecimal >= 0.01 && afterDecimal < 0.13)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.13 && afterDecimal < 0.37)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.37 && afterDecimal < 0.63)
            {
                validHour = floorNum + 0.50;
            }
            else if (afterDecimal >= 0.63 && afterDecimal < 0.87)
            {
                validHour = floorNum + 0.75;
            }
            else
            {
                validHour = floorNum + 1;
            }

            approvedTimesheet.billableHours = Math.Round(totalBillableHour, 2).ToString();
            approvedTimesheet.equivalentHours = Math.Round(validHour, 2).ToString();

            if (updatedSupervisionFlag == 1)
            {
                TimesheetTracking timesheetTracking = new TimesheetTracking();
                timesheetTracking.ID = Utility.CommonHelper.GetDBInt(timesheetID);
                double hour = totalBillableHour;
                double totalHourRounded = RoundedOffHour(hour);
                timesheetTracking.databasename = databasename;
                timesheetTracking.UpdateTotalHour();
            }
           
                return PartialView("_DaysPerEmployee", approvedTimesheet);
           
        }

    }
}
