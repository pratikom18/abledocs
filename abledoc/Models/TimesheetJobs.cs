using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class TimesheetJobs
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public int UserID { get; set; }
        public int TimesheetID { get; set; }
        public double ActualTime { get; set; }
        public double OverrideTime { get; set; }
        public string Comment { get; set; }
        public string SupervisorComment { get; set; }
        public DateTime Created { get; set; }
        public int SupervisorID { get; set; }
        public DateTime DateTimeStart { get; set; }
        public int TimesheetType { get; set; }
        public string AllTimersID { get; set; }
        public int Deleted { get; set; }
        public string databasename { get; set; }

        public double FetchJobERPAlteredHours(int jobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            double sumHour = 0.00;

            TimesheetJobs timesheetJobs = new TimesheetJobs();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * from timesheet_jobs Where JobID=" + jobID;
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
                {
                    timesheetJobs = ConvertDataTable<TimesheetJobs>(dt.Rows[0].Table).FirstOrDefault();
                    if (timesheetJobs.OverrideTime == 0.00)
                    {
                        sumHour = timesheetJobs.ActualTime;
                    }
                    else
                    {
                        sumHour = timesheetJobs.OverrideTime;
                    }

                    return sumHour;
                }
                else
                    return sumHour;
            }
        }

        public TimesheetJobs getUserWorkedDataWeekly()
        {
             var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = string.Empty;
               
                    query = "SELECT * FROM timesheet_jobs WHERE DateTimeStart=@DateTimeStart AND UserID=@UserID AND JobID=@JobID AND TimesheetID=@TimesheetID AND TimesheetType=@TimesheetType AND Deleted=@Deleted";

               
                MySqlCommand cmd = new MySqlCommand(query, conn);

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
                    return ConvertDataTable<TimesheetJobs>(dt).FirstOrDefault();
                else
                    return null;
            }
        }

        public List<TimesheetJobs> getUserWorkedDataWeeklyList()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = string.Empty;

                query = "SELECT * FROM timesheet_jobs WHERE DateTimeStart=@DateTimeStart AND UserID=@UserID AND JobID=@JobID AND TimesheetID=@TimesheetID AND TimesheetType=@TimesheetType AND Deleted=@Deleted";

                MySqlCommand cmd = new MySqlCommand(query, conn);

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
                    return ConvertDataTable<TimesheetJobs>(dt);
                else
                    return null;
            }
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DateTimeStart",
                DbType = DbType.DateTime,
                Value = DateTimeStart,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TimesheetID",
                DbType = DbType.Int32,
                Value = TimesheetID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TimesheetType",
                DbType = DbType.Int32,
                Value = TimesheetType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Deleted",
                DbType = DbType.Int32,
                Value = Deleted,
            });

        }

        private void BindParamsInsert(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DateTimeStart",
                DbType = DbType.DateTime,
                Value = DateTimeStart,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AllTimersID",
                DbType = DbType.String,
                Value = AllTimersID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TimesheetType",
                DbType = DbType.Int32,
                Value = TimesheetType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ActualTime",
                DbType = DbType.Double,
                Value = ActualTime,
            });

        }

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "INSERT INTO timesheet_jobs (JobID, UserID, ActualTime, DateTimeStart, TimesheetType, AllTimersID) VALUES (@JobID, @UserID, @ActualTime, @DateTimeStart, @TimesheetType, @AllTimersID)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                BindParamsInsert(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateTimesheetJobs()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update timesheet_jobs Set ActualTime=@ActualTime, AllTimersID=@AllTimersID WHERE DateTimeStart=@DateTimeStart AND UserID=@UserID AND JobID=@JobID AND TimesheetType=@TimesheetType AND ID=@ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindUpdateParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        private void BindUpdateParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ActualTime",
                DbType = DbType.Double,
                Value = ActualTime,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AllTimersID",
                DbType = DbType.String,
                Value = AllTimersID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DateTimeStart",
                DbType = DbType.DateTime,
                Value = DateTimeStart,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TimesheetType",
                DbType = DbType.Int32,
                Value = TimesheetType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });

        }

        public string UpdateSupervisor()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update timesheet_jobs SET Comment = @Comment, OverrideTime = @OverrideTime, SupervisorComment = @SupervisorComment WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Comment",
                    DbType = DbType.String,
                    Value = Comment,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@OverrideTime",
                    DbType = DbType.Double,
                    Value = OverrideTime,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@SupervisorComment",
                    DbType = DbType.String,
                    Value = SupervisorComment,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });

                cmd.ExecuteNonQuery();
                int LastID = 0;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateTimeSheet()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update timesheet_jobs SET TimesheetID = @TimesheetID WHERE ID = @ID";


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
                int LastID = 0;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public TimesheetJobs getDetailByID(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = string.Empty;

                query = "SELECT * FROM timesheet_jobs WHERE ID ="+id;

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
                    return ConvertDataTable<TimesheetJobs>(dt).FirstOrDefault();
                else
                    return null;
            }
        }
    }
}
