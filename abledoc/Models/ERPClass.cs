using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class dataArray
    {
        public int jobid { get; set; }
        public double ActualTime { get; set; }
        public string ID { get; set; }
        public string databasename { get; set; }
    }

    public class carriedForwardFileString
    {
        public string id { get; set; }
        public string QueryType { get; set; }
        public double textValTime { get; set; }
        public string Comment { get; set; }
        public double ActualTime { get; set; }
        public double OverrideTime { get; set; }
        public string justDate { get; set; }
        public string SupervisorComment { get; set; }
        public string databasename { get; set; }
    }

    public class clubbedFile
    {
        public string id { get; set; }
        public string QueryType { get; set; }
        public double textValTime { get; set; }
        public string Comment { get; set; }
        public double ActualTime { get; set; }
        public double OverrideTime { get; set; }
        public string SupervisorComment { get; set; }
        public string databasename { get; set; }
    }

    public static class UpdateExtensions
    {
        public delegate void Func<TArg0>(TArg0 element);

        public static int Update<TSource>(this IEnumerable<TSource> source, Func<TSource> update)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (update == null) throw new ArgumentNullException("update");
            if (typeof(TSource).IsValueType)
                throw new NotSupportedException("value type elements are not supported by update.");

            int count = 0;
            foreach (TSource element in source)
            {
                update(element);
                count++;
            }
            return count;
        }
    }
    public class ERPClass
    {
        #region "Properties"
        DataContext context = new DataContext();
        public decimal SumTotalSeconds { get; set; }
        public Int64 FileID { get; set; }
        public decimal sumHour { get; set; }
        public Int64 TotalSeconds { get; set; }
        public double ActualTime { get; set; }
        public double OverrideTime { get; set; }
        public string databasename { get; set; }
        #endregion

        public decimal FileHoursWorked(Int64 FileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            decimal sumHour = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select SUM(TotalSeconds) as SumTotalSeconds from all_timers Where FileID=@FileID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int64,
                    Value = FileID,
                });

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    ERPClass u = ConvertDataTable<ERPClass>(dt.Rows[0].Table).FirstOrDefault();
                    sumHour = u.SumTotalSeconds / 3600;
                }
            }
            return sumHour;
        }

        public decimal FetchJobERPRecordedHours(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            decimal sumHour = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select SUM(TotalSeconds) as SumTotalSeconds from all_timers Where JobID=@JobID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    ERPClass u = ConvertDataTable<ERPClass>(dt.Rows[0].Table).FirstOrDefault();
                    sumHour = u.SumTotalSeconds / 3600;
                }
            }
            return sumHour;
        }

        public List<ERPClass> FetchJobERPAlteredHours(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * from timesheet_jobs Where JobID=@JobID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<ERPClass>(dt);
                else
                    return null;
            }

        }

        public List<ERPClass> CheckRecordUsed(string tablename,string Columnname,int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * from " + tablename + " Where " + Columnname +" = " + id;
                 MySqlCommand cmd = new MySqlCommand(qry, conn);
                

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<ERPClass>(dt);
                else
                    return Utility.ConvertDataTable<ERPClass>(dt);
            }

        }

        public int JobStatusUpdate(int JobID, string flag,string databasename1)
        {
            if (flag == "CheckToClosed")
            {
                // Check if the status of job has been set to Delivered, then if the invoice has been approved, and if so then move the job to Closed
                Jobs modelJob = new Jobs();
                modelJob.ID = JobID;
                modelJob.databasename = databasename1;
                var query = modelJob.CheckJobStatus();
                if (query != null)
                {
                    foreach (var row in query)
                    {
                        if (row.Status == "DELIVERED" && row.Approved == 1 && row.VarianceApproved == 1)
                        {
                            // Move the job to closed
                            modelJob.ID = JobID;
                            modelJob.Status = "CLOSED";
                            modelJob.databasename = databasename1;
                            modelJob.UpdateJobStatus();
                        }
                    }
                }

            }
            return 0;
        }

        public static List<string> GetDatesFromRange(DateTime start, DateTime end, string format = "yyyy-MM-d")
        {

            //var stringDate = start.ToString(format);

            var dates = new List<string>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                DateTime dt1 = dt;
                dates.Add(dt1.ToString(format));
            }
            return dates;
        }

        public List<string> GetDatesFromRange1(DateTime start, DateTime end, string format = "yyyy-MM-d")
        {

            //var stringDate = start.ToString(format);

            var dates = new List<string>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                DateTime dt1 = dt;
                dates.Add(dt1.ToString(format));
            }
            return dates;
        }

        public static double TimeToMinute(string timeString)
        {
            double totalMinute = 0;
            if (timeString != null)
            {
                string[] timeStringExplode = timeString.Split(':');
                double hour = abledoc.Utility.CommonHelper.GetDBInt(timeStringExplode[0]);
                double minute = abledoc.Utility.CommonHelper.GetDBInt(timeStringExplode[1]);
                double second = abledoc.Utility.CommonHelper.GetDBInt(timeStringExplode[2]);
                double secondToMinute = 0;
                if (second > 0)
                {
                    secondToMinute = 1;
                }
                double hourToMinute = hour * 60;
                totalMinute = hourToMinute + minute + secondToMinute;
            }
            
            return totalMinute;
        }

        public static string UserWorkedDataWeekly(string databasename, string loggedInUser, string dateRange, string timesheetID, string mode,ref ApprovedTimesheet approvedTimesheet)
        {
           
            List<carriedForwardFileString> carriedForwardFileStringList = new List<carriedForwardFileString>();
            List<clubbedFile> clubbedFileList = new List<clubbedFile>();
            approvedTimesheet.clubbedFileList1 = new List<List<clubbedFile>>();
            //string[] weekDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            //approvedTimesheet.weekDays = weekDays.ToList();
            string[] eachDate = dateRange.Split(",");

            if (mode == "Supervision")
            {
                List<string> dateArray = GetDatesFromRange(abledoc.Utility.CommonHelper.GetDBDate(dateRange.Split(",").GetValue(0)), abledoc.Utility.CommonHelper.GetDBDate(dateRange.Split(",").GetValue(1)));
                approvedTimesheet.dateRange = dateArray;
                eachDate = dateArray.ToArray();
            }

            string startDate = eachDate[0] + " 00:00:00";
            string endDate = eachDate[eachDate.Length - 1] + " 23:59:59";
            string clubbedFile = "";
            List<string> dayTimeVal = new List<string>();

            for (int i = 0; i < 14; i++)
            {
                dayTimeVal.Add("0");
            }
            AllTimers allTimers = new AllTimers();
            List<AllTimers> allTimersList = new List<AllTimers>();
            allTimers.databasename = databasename;
            allTimersList = allTimers.getUserWorkedDataWeekly(mode, loggedInUser, startDate, endDate, timesheetID);
            string empID = loggedInUser;
            for (int countDate = 0; countDate < 14; countDate++)
            {
                clubbedFileList = new List<clubbedFile>();
                // Jobs Checkout Files
                startDate = eachDate[countDate] + " 00:00:00";
                endDate = eachDate[countDate] + " 23:59:59";

                clubbedFile = NormalTimesheetDataFormulation(databasename,1, startDate, endDate, empID, clubbedFile, countDate,ref dayTimeVal, "",ref clubbedFileList,ref carriedForwardFileStringList, mode, timesheetID);
                // Second Timer Hours
                SecondTimer secondTimer = new SecondTimer();
                List<SecondTimer> secondTimersList = new List<SecondTimer>();
                secondTimer.databasename = databasename;
                secondTimersList = secondTimer.getUserWorkedDataWeekly(mode, loggedInUser, startDate, endDate, timesheetID);
                if (secondTimersList != null)
                {
                    foreach (SecondTimer row in secondTimersList)
                    {
                        double actualTime = 0;
                        if (row.DateStart >= abledoc.Utility.CommonHelper.GetDBDate(eachDate[countDate] + " 00:00:00") && row.DateStart <= abledoc.Utility.CommonHelper.GetDBDate(eachDate[countDate] + " 23:59:59"))
                        {
                            string timer = row.Timer;
                            double minute = 0;
                            if (!string.IsNullOrEmpty(timer))
                            {
                                minute = TimeToMinute(timer);
                            }

                            double hour = minute / 60;
                            actualTime = Math.Round(hour, 2);
                            if (row.OverrideTime == null || row.OverrideTime == 0)
                            {
                                dayTimeVal[countDate] = (abledoc.Utility.CommonHelper.GetDBDouble(dayTimeVal[countDate]) + Math.Round(hour, 2)).ToString();
                            }
                            else
                            {
                                dayTimeVal[countDate] = (abledoc.Utility.CommonHelper.GetDBDouble(dayTimeVal[countDate]) + Math.Round(row.OverrideTime, 2)).ToString();
                            }
                            clubbedFile clubbedFile1 = new clubbedFile();
                            clubbedFile1.id = "secondtimer-" + row.ID;
                            clubbedFile1.QueryType = row.QueryType;
                            clubbedFile1.textValTime = Math.Round(hour, 2);
                            clubbedFile1.Comment = row.Comment;
                            clubbedFile1.ActualTime = row.ActualTime;
                            clubbedFile1.OverrideTime = row.OverrideTime;
                            clubbedFile1.SupervisorComment = row.SupervisorComment;
                            clubbedFile1.databasename = row.databasename;
                            clubbedFileList.Add(clubbedFile1);
                            clubbedFile = clubbedFile + "secondtimer-" + row.ID + " | " + row.QueryType + " | " + Math.Round(hour, 2) + " | " + row.Comment + " | " + row.ActualTime + " | " + row.OverrideTime + " | " + row.SupervisorComment + " || ";
                        }
                        if (row.ActualTime == null)
                        {
                            secondTimer = new SecondTimer();
                            secondTimer.ActualTime = actualTime;
                            secondTimer.ID = row.ID;
                            secondTimer.databasename = row.databasename;
                            secondTimer.UpdateApprovedFinal();
                        }
                    }
                }
                approvedTimesheet.clubbedFileList1.Add(clubbedFileList);
                clubbedFile = clubbedFile + " |||| ";
            }

            startDate = eachDate[0] + " 00:00:00";
            endDate = eachDate[eachDate.Length - 1] + " 23:59:59";



            string dayTime = "";
            approvedTimesheet.weeklyHourList = new List<string>();
            for (int i = 0; i < 14; i++)
            {
                approvedTimesheet.weeklyHourList.Add(dayTimeVal[i]);
                dayTime = dayTime + dayTimeVal[i] + " | ";
            }

            //******************************************** Carried forward ************************************
            clubbedFileList = new List<clubbedFile>();
            string carriedForwardString = "";
            string carriedForwardFileString = "";
            int countDate1 = 0;
            carriedForwardFileString = NormalTimesheetDataFormulation(databasename,2, startDate, endDate, empID, "", countDate1,ref dayTimeVal, carriedForwardFileString, ref clubbedFileList, ref carriedForwardFileStringList, mode, timesheetID);


            // Second Timer Hours

            SecondTimer secondTimer1 = new SecondTimer();
            List<SecondTimer> secondTimersList1 = new List<SecondTimer>();
            secondTimer1.databasename = databasename;
            secondTimersList1 = secondTimer1.getUserWorkedDataWeekly1(mode, loggedInUser, startDate, timesheetID);
            string lastDate = "";
            double timeHour = 0;
            if (secondTimersList1 != null)
            {
                foreach (SecondTimer row in secondTimersList1)
                {
                    double actualTime = 0;
                    string justDate = row.DateStart.Date.ToString();
                    if (justDate != lastDate && lastDate != "")
                    {
                        carriedForwardString = carriedForwardString + lastDate + " | " + timeHour + " || ";
                        timeHour = 0;
                        lastDate = justDate;
                    }

                    if (justDate == lastDate || lastDate == "")
                    {
                        string timer = row.Timer;
                        double minute = TimeToMinute(timer);
                        double hour = minute / 60;
                        actualTime = Math.Round(hour, 2);
                        timeHour = timeHour + Math.Round(hour, 2);
                        carriedForwardFileString carriedForwardFileString1 = new carriedForwardFileString();
                        carriedForwardFileString1.id = "secondtimer-" + row.ID;
                        carriedForwardFileString1.QueryType = row.QueryType;
                        carriedForwardFileString1.textValTime = Math.Round(hour, 2);
                        carriedForwardFileString1.Comment = row.Comment;
                        carriedForwardFileString1.ActualTime = row.ActualTime;
                        carriedForwardFileString1.OverrideTime = row.OverrideTime;
                        carriedForwardFileString1.justDate = justDate;
                        carriedForwardFileString1.SupervisorComment = row.SupervisorComment;
                        carriedForwardFileString1.databasename = row.databasename;
                        carriedForwardFileStringList.Add(carriedForwardFileString1);
                        carriedForwardFileString = carriedForwardFileString + "secondtimer-" + row.ID + " | " + row.QueryType + " | " + Math.Round(hour, 2) + " | " + row.Comment + " | " + row.ActualTime + " | " + row.OverrideTime + " | " + justDate + " | " + row.SupervisorComment + " || ";
                    }

                    lastDate = justDate;

                    if (row.ActualTime == null)
                    {
                        secondTimer1 = new SecondTimer();
                        secondTimer1.ActualTime = actualTime;
                        secondTimer1.ID = row.ID;
                        secondTimer1.databasename = row.databasename;
                        secondTimer1.UpdateApprovedFinal();
                    }
                }
            }

            approvedTimesheet.carriedForwardFileStringsList = carriedForwardFileStringList;
            //approvedTimesheet.clubbedFileList = clubbedFileList;
            string fullTimesheet = dayTime + " ||| " + carriedForwardFileString + " ||| " + clubbedFile + " ||| " + dateRange;
            return fullTimesheet;
        }

        public static string NormalTimesheetDataFormulation(string databasename,int timesheetType, string startDate, string endDate, string empID, string clubbedFile, int countDate,ref List<string> dayTimeVal, string carriedForwardFileString,ref List<clubbedFile> clubbedFileList,ref List<carriedForwardFileString> carriedForwardFileStringList, string timesheetMode = "", string timesheetID = "")
        {

            if (timesheetType == 1)
            {

                AllTimers allTimers = new AllTimers();
                List<AllTimers> allTimersList = new List<AllTimers>();
                allTimers.databasename = databasename;
                allTimersList = allTimers.getUserWorkedDataWeekly(timesheetMode, empID, startDate, endDate, timesheetID);

                List<dataArray> dataArray1 = new List<dataArray>();
                if (allTimersList != null)
                {
                    foreach (AllTimers row in allTimersList)
                    {
                        if (dataArray1.Where(x => x.jobid == row.JobID).Count() == 1)
                        {
                            var query = (from data in dataArray1
                                         where data.jobid == row.JobID
                                         select data)
                            .Update(st => { st.ActualTime = st.ActualTime + row.TotalHours; st.ID = st.ID + " | " + row.ID; });
                        }
                        else
                        {
                            dataArray dataArray = new dataArray();
                            dataArray.jobid = row.JobID;
                            dataArray.ActualTime = 0.00;
                            dataArray.ID = "";
                            dataArray.databasename = row.databasename;
                            dataArray1.Add(dataArray);
                        }
                    }
                }


                TimesheetJobs timesheetJobs = new TimesheetJobs();
                TimesheetJobs timesheetJobs1 = new TimesheetJobs();

                // Store each job data in the timesheet_jobs table
                foreach (dataArray dataArrayKey in dataArray1)
                {
                    double actualTime = dataArrayKey.ActualTime;
                    string allIDs = dataArrayKey.ID;


                    timesheetJobs.JobID = dataArrayKey.jobid;
                    timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                    timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(startDate);
                    timesheetJobs.TimesheetID = abledoc.Utility.CommonHelper.GetDBInt(timesheetID);
                    timesheetJobs.TimesheetType = timesheetType;
                    timesheetJobs.Deleted = 0;
                    timesheetJobs.databasename = dataArrayKey.databasename;
                    timesheetJobs1 = timesheetJobs.getUserWorkedDataWeekly();

                    if (timesheetJobs1 != null)
                    {
                        timesheetJobs = new TimesheetJobs();
                        timesheetJobs.JobID = dataArrayKey.jobid;
                        timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                        timesheetJobs.ActualTime = dataArrayKey.ActualTime;
                        timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(startDate);
                        timesheetJobs.TimesheetType = timesheetType;
                        timesheetJobs.ID = timesheetJobs1.ID;
                        timesheetJobs.AllTimersID = dataArrayKey.ID;
                        timesheetJobs.databasename = dataArrayKey.databasename;
                        timesheetJobs.UpdateTimesheetJobs();
                    }
                    else
                    {
                        timesheetJobs = new TimesheetJobs();
                        timesheetJobs.JobID = dataArrayKey.jobid;
                        timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                        timesheetJobs.ActualTime = dataArrayKey.ActualTime;
                        timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(startDate);
                        timesheetJobs.AllTimersID = dataArrayKey.ID;
                        timesheetJobs.TimesheetType = timesheetType;
                        timesheetJobs.databasename = dataArrayKey.databasename;
                        timesheetJobs.Insert();
                    }

                    timesheetJobs = new TimesheetJobs();
                    timesheetJobs1 = new TimesheetJobs();
                    timesheetJobs.JobID = dataArrayKey.jobid;
                    timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                    timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(startDate);
                    timesheetJobs.TimesheetID = abledoc.Utility.CommonHelper.GetDBInt(timesheetID);
                    timesheetJobs.TimesheetType = timesheetType;
                    timesheetJobs.Deleted = 0;
                    timesheetJobs.databasename = dataArrayKey.databasename;
                    timesheetJobs1 = timesheetJobs.getUserWorkedDataWeekly();


                    int foundRowID = 0;
                    double overrideTime = 0.00;
                    string comment = "";
                    string supervisorComment = "";

                    if (timesheetJobs1 != null)
                    {

                        foundRowID = timesheetJobs1.ID;
                        actualTime = timesheetJobs1.ActualTime;
                        overrideTime = timesheetJobs1.OverrideTime;
                        comment = timesheetJobs1.Comment;
                        supervisorComment = timesheetJobs1.SupervisorComment;
                    }

                    if (overrideTime == 0.00)
                    {
                        dayTimeVal[countDate] = (abledoc.Utility.CommonHelper.GetDBDouble(dayTimeVal[countDate].ToString()) + Math.Round(actualTime, 2)).ToString();
                    }
                    else
                    {
                        dayTimeVal[countDate] = (abledoc.Utility.CommonHelper.GetDBDouble(dayTimeVal[countDate].ToString()) + Math.Round(overrideTime, 2)).ToString();
                    }

                    Search search = new Search();
                    string engagementNum = search.EngagementNumberJobID(dataArrayKey.jobid, dataArrayKey.databasename);
                    clubbedFile clubbedFile1 = new clubbedFile();
                    clubbedFile1.id = "jobsfilescheckouts-" + foundRowID;
                    clubbedFile1.QueryType = engagementNum;
                    clubbedFile1.textValTime = actualTime;
                    clubbedFile1.Comment = comment;
                    clubbedFile1.ActualTime = actualTime;
                    clubbedFile1.OverrideTime = overrideTime;
                    clubbedFile1.SupervisorComment = supervisorComment;
                    clubbedFile1.databasename = dataArrayKey.databasename;
                    clubbedFileList.Add(clubbedFile1);
                    clubbedFile = clubbedFile + "jobsfilescheckouts-" + foundRowID + " | " + engagementNum + " | " + actualTime + " | " + comment + " | " + actualTime + " | " + overrideTime + " | " + supervisorComment + " || ";
                }
            }
            else if (timesheetType == 2)
            {

                AllTimers allTimers = new AllTimers();
                List<AllTimers> allTimersList = new List<AllTimers>();
                allTimers.databasename = databasename;
                allTimersList = allTimers.getUserWorkedDataWeekly(timesheetMode, empID, startDate, endDate, timesheetID);


                // Get the data sorted in jobs according to their dates
                List<DateTime> dataArrayDate = new List<DateTime>();
                if (allTimersList != null)
                {
                    foreach (AllTimers row in allTimersList)
                    {
                        if (!dataArrayDate.Contains(row.DateTimeStart.Date))
                        {
                            dataArrayDate.Add(row.DateTimeStart.Date);
                        }
                    }
                }

                if (dataArrayDate != null)
                {
                    foreach (DateTime value in dataArrayDate)
                    {

                        string currentDateStart = value.Date.ToString("yyyy-MM-d") + " 00:00:00";
                        string currentDateStop = value.Date.ToString("yyyy-MM-d") + " 23:59:59";

                        allTimers = new AllTimers();
                        allTimersList = new List<AllTimers>();
                        allTimers.databasename = databasename;
                        allTimersList = allTimers.getUserWorkedDataWeekly(timesheetMode, empID, currentDateStart, currentDateStop, timesheetID);

                        List<dataArray> dataArray1 = new List<dataArray>();
                        foreach (AllTimers row in allTimersList)
                        {
                            if (dataArray1.Where(x => x.jobid == row.JobID).Count() == 1)
                            {
                                var query = (from data in dataArray1
                                             where data.jobid == row.JobID
                                             select data)
                                .Update(st => { st.ActualTime = st.ActualTime + row.TotalHours; st.ID = st.ID + " | " + row.ID; });
                            }
                            else
                            {
                                dataArray dataArray = new dataArray();
                                dataArray.jobid = row.JobID;
                                dataArray.ActualTime = 0.00;
                                dataArray.ID = "";
                                dataArray.databasename = row.databasename;
                                dataArray1.Add(dataArray);
                            }
                        }

                        TimesheetJobs timesheetJobs = new TimesheetJobs();
                        TimesheetJobs timesheetJobs1 = new TimesheetJobs();

                        // Store each job data in the timesheet_jobs table
                        foreach (dataArray dataArrayKey in dataArray1)
                        {
                            double actualTime = dataArrayKey.ActualTime;
                            string allIDs = dataArrayKey.ID;


                            timesheetJobs.JobID = dataArrayKey.jobid;
                            timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                            timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(currentDateStart);
                            timesheetJobs.TimesheetID = abledoc.Utility.CommonHelper.GetDBInt(timesheetID);
                            timesheetJobs.TimesheetType = timesheetType;
                            timesheetJobs.Deleted = 0;
                            timesheetJobs.databasename = dataArrayKey.databasename;
                            timesheetJobs1 = timesheetJobs.getUserWorkedDataWeekly();

                            if (timesheetJobs1 != null)
                            {
                                timesheetJobs = new TimesheetJobs();
                                timesheetJobs.JobID = dataArrayKey.jobid;
                                timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                                timesheetJobs.ActualTime = dataArrayKey.ActualTime;
                                timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(currentDateStart);
                                timesheetJobs.TimesheetType = timesheetType;
                                timesheetJobs.ID = timesheetJobs1.ID;
                                timesheetJobs.AllTimersID = dataArrayKey.ID;
                                timesheetJobs.databasename = dataArrayKey.databasename;
                                timesheetJobs.UpdateTimesheetJobs();
                            }
                            else
                            {
                                timesheetJobs = new TimesheetJobs();
                                timesheetJobs.JobID = dataArrayKey.jobid;
                                timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                                timesheetJobs.ActualTime = dataArrayKey.ActualTime;
                                timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(currentDateStart);
                                timesheetJobs.AllTimersID = dataArrayKey.ID;
                                timesheetJobs.TimesheetType = timesheetType;
                                timesheetJobs.databasename = dataArrayKey.databasename;
                                timesheetJobs.Insert();
                            }

                            timesheetJobs = new TimesheetJobs();
                            List<TimesheetJobs> timesheetJobsList = new List<TimesheetJobs>();
                            timesheetJobs.JobID = dataArrayKey.jobid;
                            timesheetJobs.UserID = abledoc.Utility.CommonHelper.GetDBInt(empID);
                            timesheetJobs.DateTimeStart = abledoc.Utility.CommonHelper.GetDBDate(currentDateStart);
                            timesheetJobs.TimesheetID = abledoc.Utility.CommonHelper.GetDBInt(timesheetID);
                            timesheetJobs.TimesheetType = timesheetType;
                            timesheetJobs.Deleted = 0;
                            timesheetJobs.databasename = dataArrayKey.databasename;
                            timesheetJobsList = timesheetJobs.getUserWorkedDataWeeklyList();

                            int rowFound = 0;
                            int foundRowID = 0;
                            double overrideTime = 0.00;
                            string comment = "";
                            string supervisorComment = "";

                            double timeHour = 0;
                            string lastDate = "";
                            if (timesheetJobsList != null)
                            {
                                foreach (TimesheetJobs row1 in timesheetJobsList)
                                {
                                    foundRowID = row1.ID;
                                    actualTime = row1.ActualTime;
                                    overrideTime = row1.OverrideTime;
                                    comment = row1.Comment;
                                    supervisorComment = row1.SupervisorComment;
                                    string carriedForwardString = "";
                                    string justDate = row1.DateTimeStart.Date.ToString();
                                    if (justDate != lastDate && lastDate != "")
                                    {
                                        carriedForwardString = carriedForwardString + lastDate + " | " + timeHour + " || ";
                                        timeHour = 0;
                                        lastDate = justDate;
                                    }

                                    if (justDate == lastDate || lastDate == "")
                                    {
                                        timeHour = timeHour + actualTime;
                                        Search search = new Search();
                                        string engagementNum = search.EngagementNumberJobID(dataArrayKey.jobid, dataArrayKey.databasename);
                                        carriedForwardFileString carriedForwardFileString1 = new carriedForwardFileString();
                                        carriedForwardFileString1.id = "jobsfilescheckouts-" + foundRowID;
                                        carriedForwardFileString1.QueryType = engagementNum;
                                        carriedForwardFileString1.textValTime = actualTime;
                                        carriedForwardFileString1.Comment = comment;
                                        carriedForwardFileString1.ActualTime = actualTime;
                                        carriedForwardFileString1.OverrideTime = overrideTime;
                                        carriedForwardFileString1.justDate = justDate;
                                        carriedForwardFileString1.SupervisorComment = supervisorComment;
                                        carriedForwardFileString1.databasename = dataArrayKey.databasename;
                                        carriedForwardFileStringList.Add(carriedForwardFileString1);
                                        carriedForwardFileString = carriedForwardFileString + "jobsfilescheckouts-" + foundRowID + " | " + engagementNum + " | " + actualTime + " | " + comment + " | " + actualTime + " | " + overrideTime + " | " + justDate + " | " + supervisorComment + " || ";
                                    }

                                    lastDate = justDate;
                                }
                            }

                        }


                    }
                }
                
               
            }

            if (timesheetType == 1)
            {
                return clubbedFile;
            }
            else if (timesheetType == 2)
            {
                return carriedForwardFileString;
            }
            else
            {
                return "";
            }
        }



    }
}

