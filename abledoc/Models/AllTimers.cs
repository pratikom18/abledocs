using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class AllTimers
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public int FileID { get; set; }
        public int UserID { get; set; }
        public string State { get; set; }
        public Int64 TStart { get; set; }
        public Int64 TStop { get; set; }
        public int Fix { get; set; }
        public string TimerNow { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeStop { get; set; }
        public string TotalTimerNow { get; set; }
        public int TimesheetID { get; set; }
        public int TotalSeconds { get; set; }
        public double TotalHours { get; set; }
        public double OverrideTime { get; set; }
        public string Comment { get; set; }
        public string SupervisorComment { get; set; }
        public int Deleted { get; set; }
        public decimal SumTotalSeconds { get; set; }
        public int Pages { get; set; }
        public string Filename { get; set; }
        public string databasename { get; set; }
        #endregion
        public AllTimers GetAllTimersByIds()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * From all_timers Where JobID=@JobID AND FileID=@FileID AND State=@State AND UserID=@UserID Order By ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                BindParams(cmd);
                MySqlDataReader dr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<AllTimers>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }
        public string CreateAllTimers()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "INSERT INTO all_timers(JobID, FileID, UserID, State, TStart, TStop, Fix, TotalTimerNow) Values(@JobID, @FileID, @UserID, @State, @TStart, @TStop, @Fix, @TotalTimerNow)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindInsertParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateAllTimers()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE all_timers SET TStop = @TStop, TimerNow = @TimerNow, TotalSeconds = @TotalSeconds, TotalHours = @TotalHours WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindUpdateParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateTotalTimerNow()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE all_timers SET TotalTimerNow = @TotalTimerNow WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindUpdateTotalTimerNowParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@State",
                DbType = DbType.String,
                Value = State,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });

        }
        private void BindInsertParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@State",
                DbType = DbType.String,
                Value = State,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TStart",
                DbType = DbType.Int64,
                Value = TStart,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TStop",
                DbType = DbType.Int64,
                Value = TStop,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Fix",
                DbType = DbType.Int32,
                Value = Fix,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TotalTimerNow",
                DbType = DbType.String,
                Value = TotalTimerNow,
            });


        }
        private void BindUpdateParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TStop",
                DbType = DbType.Int32,
                Value = TStop,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TimerNow",
                DbType = DbType.String,
                Value = TimerNow,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TotalSeconds",
                DbType = DbType.Int32,
                Value = TotalSeconds,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TotalHours",
                DbType = DbType.Double,
                Value = TotalHours,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });

        }
        private void BindUpdateTotalTimerNowParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TotalTimerNow",
                DbType = DbType.String,
                Value = TotalTimerNow,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });

        }
        public List<AllTimers> GetAllTimersByJobId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            AllTimers objAllTimers = new AllTimers();
            decimal sumHour = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT TimerNow From all_timers Where JobID=@JobID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                BindTimerParams(cmd);
                MySqlDataReader dr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<AllTimers>(dt);
                else
                    return null;
            }
        }
        private void BindTimerParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
        }
        public decimal FetchJobERPRecordedHours(int jobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            decimal sumHour = 0;

            AllTimers allTimers = new AllTimers();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select SUM(TotalSeconds) as SumTotalSeconds from all_timers Where JobID=" + jobID;
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                BindParams(cmd);
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
                    allTimers = ConvertDataTable<AllTimers>(dt.Rows[0].Table).FirstOrDefault();
                    sumHour = allTimers.SumTotalSeconds / 3600;
                    return sumHour;
                }
                else
                    return sumHour;
            }
        }

        public List<AllTimers> getUserWorkedDataWeekly(string mode, string empID, string startDate, string endDate, string timesheetID)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string query = string.Empty;
                if (mode == "")
                {
                    query = "Select at.TimerNow, at.JobID, at.DateTimeStart, at.State, jf.Pages, jf.ID as FileID, jf.Filename as Filename, at.ID, at.Comment, at.SupervisorComment, at.TotalHours, at.OverrideTime,'DBNAME' as databasename FROM DBNAME.all_timers at Inner Join DBNAME.jobs_files jf ON at.FileID = jf.ID WHERE 1 AND at.UserID=" + empID+"  AND at.DateTimeStart>='"+startDate+"' AND at.DateTimeStart<='"+endDate+"' AND at.TimesheetID=0 AND at.Deleted=0";
                }
                else if (mode == "Supervision")
                {
                    query = "Select at.TimerNow, at.JobID, at.DateTimeStart, at.State, jf.Pages, jf.ID as FileID, jf.Filename as Filename, at.ID, at.Comment, at.SupervisorComment, at.TotalHours, at.OverrideTime,'DBNAME' as databasename FROM DBNAME.all_timers at Inner Join DBNAME.jobs_files jf ON at.FileID = jf.ID WHERE at.UserID=" + empID+" AND at.DateTimeStart>='"+startDate+"' AND at.DateTimeStart<='"+endDate+"' AND at.TimesheetID="+timesheetID+" AND at.Deleted=0";
                }
                else if (mode == "SentBack")
                {
                    query = "Select at.TimerNow, at.JobID, at.DateTimeStart, at.State, jf.Pages, jf.ID as FileID, jf.Filename as Filename, at.ID, at.Comment, at.SupervisorComment, at.TotalHours, at.OverrideTime,'DBNAME' as databasename FROM DBNAME.all_timers at Inner Join DBNAME.jobs_files jf ON at.FileID = jf.ID WHERE at.UserID=" + empID+" AND at.DateTimeStart>='"+startDate+"' AND at.DateTimeStart<='"+endDate+"' AND at.TimesheetID="+timesheetID+" AND at.Deleted=0";
                }


                string myqry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                query = " Select TimerNow, JobID, DateTimeStart, State, Pages, FileID, Filename, ID, Comment, SupervisorComment, TotalHours, OverrideTime,databasename  from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a ORDER BY DateTimeStart";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                              

                MySqlDataReader dr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<AllTimers>(dt);
                else
                    return null;
            }
        }

        public string UpdateTimeSheetID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update all_timers SET TimesheetID= @TimesheetID WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@TimesheetID",
                    DbType = DbType.Int32,
                    Value = TimesheetID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
    }
}
