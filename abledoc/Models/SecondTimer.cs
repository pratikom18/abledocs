using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class SecondTimer
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Timer { get; set; }
        public string State { get; set; }
        public DateTime DateStart { get; set; }
        public string QueryType { get; set; }
        public double ActualTime { get; set; }
        public double OverrideTime { get; set; }
        public string Comment { get; set; }
        public int TimesheetID { get; set; }
        public string SupervisorComment { get; set; }
        public int JobID { get; set; }
        public int FileID { get; set; }
        public int Deleted { get; set; }
        public string databasename { get; set; }
        #endregion

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "Insert into second_timer(UserID, Message, Timer, QueryType, JobID, FileID, Comment) Values(@UserID, @Message, @Timer, @QueryType, @JobID, @FileID, @Comment)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                BindParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public List<SecondTimer> getUserWorkedDataWeekly(string mode, string empID, string startDate, string endDate, string timesheetID)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string query = string.Empty;
                if (mode == "")
                {
                    query = "Select *,'DBNAME' as databasename From DBNAME.second_timer where DateStart>='" + startDate + "' AND DateStart<='" + endDate + "' AND UserID=" + empID + " AND TimesheetID=0 AND Deleted=0 ";
                }
                else if (mode == "Supervision")
                {
                    query = "Select *,'DBNAME' as databasename From DBNAME.second_timer where DateStart>='" + startDate + "' AND DateStart<='" + endDate + "' AND UserID=" + empID + " AND TimesheetID=" + timesheetID + " AND Deleted=0 ";
                }
                else if (mode == "SentBack")
                {
                    query = "Select *,'DBNAME' as databasename From DBNAME.second_timer where DateStart>='" + startDate + "' AND DateStart<='" + endDate + "' AND UserID=" + empID + " AND TimesheetID=" + timesheetID + " AND Deleted=0 ";
                }


                string myqry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                query = " Select *  from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a ORDER BY DateStart";


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
                    return ConvertDataTable<SecondTimer>(dt);
                else
                    return null;
            }
        }

        public List<SecondTimer> getUserWorkedDataWeekly1(string mode, string empID, string startDate, string timesheetID)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string query = string.Empty;
                if (mode == "")
                {
                    query = "Select *,'DBNAME' as databasename From DBNAME.second_timer where DateStart<'" + startDate + "' AND UserID=" + empID + " AND TimesheetID=0 AND Deleted=0 ";
                }
                else if (mode == "Supervision")
                {
                    query = "Select *,'DBNAME' as databasename From DBNAME.second_timer where DateStart<'" + startDate + "' AND UserID=" + empID + " AND TimesheetID=" + timesheetID + " AND Deleted=0 ";
                }
                else if (mode == "SentBack")
                {
                    query = "Select *,'DBNAME' as databasename From DBNAME.second_timer where DateStart<'" + startDate + "' AND UserID=" + empID + " AND TimesheetID=" + timesheetID + " AND Deleted=0 ";
                }

                
                string myqry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                query = " Select *  from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a ORDER BY DateStart";

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
                    return ConvertDataTable<SecondTimer>(dt);
                else
                    return null;
            }
        }
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Message",
                DbType = DbType.String,
                Value = Message,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Timer",
                DbType = DbType.String,
                Value = Timer,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@QueryType",
                DbType = DbType.String,
                Value = QueryType,
            });
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
                ParameterName = "@Comment",
                DbType = DbType.String,
                Value = Comment,
            });

        }

        public string UpdateApprovedFinal()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update second_timer SET ActualTime=@actualTime WHERE ID=@ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@actualTime",
                    DbType = DbType.Double,
                    Value = ActualTime,
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

        public string UpdateSupervisor()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update second_timer SET Comment = @Comment, OverrideTime = @OverrideTime, SupervisorComment = @SupervisorComment WHERE ID = @ID";


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

        public string InsertTimeSheet()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "INSERT INTO second_timer (UserID, QueryType, ActualTime, OverrideTime, Comment, DateStart, TimesheetID) VALUES (@UserID, @QueryType, @ActualTime, @OverrideTime, @Comment, @DateStart, @TimesheetID)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                BindParams1(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParams1(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@QueryType",
                DbType = DbType.String,
                Value = QueryType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ActualTime",
                DbType = DbType.Double,
                Value = ActualTime,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OverrideTime",
                DbType = DbType.Double,
                Value = OverrideTime,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Comment",
                DbType = DbType.String,
                Value = Comment,
            });
            
            
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DateStart",
                DbType = DbType.DateTime,
                Value = DateStart,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TimesheetID",
                DbType = DbType.Int32,
                Value = TimesheetID,
            });
          
        }

        public string UpdateTimeSheet()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update second_timer SET TimesheetID = @TimesheetID WHERE ID = @ID";


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
    }
}
