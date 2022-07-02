using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class TimesheetTracking
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int UserID { get; set; }
        public int SupervisorID { get; set; }
        public double OriginalHour { get; set; }
        public double TotalHour { get; set; }
        public string BillableDuration { get; set; }
        public int Approved { get; set; }
        public int SentBack { get; set; }
        public double TotalHourRounded { get; set; }
        public int ApprovedFinal { get; set; }
        public int Deleted { get; set; }
        public double OT1 { get; set; }
        public double OT2 { get; set; }
        public double OT3 { get; set; }
        public double OT4 { get; set; }
        public double OT5 { get; set; }
        public double OT6 { get; set; }
        public double OT7 { get; set; }
        public string ERPID { get; set; }
        public string FullName { get; set; }
        public string TimesheetID { get; set; }
        public double plainHourLimit { get; set; }
        public double overTimeHours { get; set; }
        public string databasename { get;set; }

        #endregion

        public List<TimesheetTracking> GetSearchListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int supervisorID, string monthVal, string yearVal,int ApprovedFinal, int userID,bool show, out int TotalRecord)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = string.Empty;
                if (monthVal == "0" || yearVal == "0")
                {
                    if (show) //(userID == 8)
                    {
                        myqry = "Select tt.ID,tt.UserID,tt.BillableDuration,tt.TotalHourRounded,CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) as TimesheetID,CONCAT(U.FirstName,' ',U.LastName) AS FullName, st1.OT1, st2.OT2, st3.OT3 from COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from DBNAME.second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from DBNAME.second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from DBNAME.second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE tt.Approved = 1 AND tt.Deleted = 0 AND tt.ApprovedFinal="+ ApprovedFinal + "";
                    }
                    else
                    {
                        myqry = "Select tt.ID,tt.UserID,tt.BillableDuration,tt.TotalHourRounded,CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) as TimesheetID,CONCAT(U.FirstName,' ',U.LastName) AS FullName, st1.OT1, st2.OT2, st3.OT3 from COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from DBNAME.second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from DBNAME.second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from DBNAME.second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE (tt.SupervisorID = " + supervisorID + " OR tt.UserID = " + supervisorID + ") AND tt.Approved = 1 AND tt.Deleted = 0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                }
                else
                {
                    if (show) //(userID == 8)
                    {
                        myqry = "Select tt.ID,tt.UserID,tt.BillableDuration,tt.TotalHourRounded,CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) as TimesheetID,CONCAT(U.FirstName,' ',U.LastName) AS FullName, st1.OT1, st2.OT2, st3.OT3 from COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from DBNAME.second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from DBNAME.second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from DBNAME.second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE tt.BillableDuration LIKE '%" + yearVal + "-" + monthVal + "%' AND tt.Approved=1 AND tt.Deleted=0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                    else
                    {
                        myqry = "Select tt.ID,tt.UserID,tt.BillableDuration,tt.TotalHourRounded,CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) as TimesheetID,CONCAT(U.FirstName,' ',U.LastName) AS FullName, st1.OT1, st2.OT2, st3.OT3 from COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from DBNAME.second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from DBNAME.second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from DBNAME.second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE (tt.SupervisorID = " + supervisorID + " OR tt.UserID = " + supervisorID + ") AND tt.BillableDuration LIKE '%" + yearVal + "-" + monthVal + "%' AND tt.Approved=1 AND tt.Deleted=0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                }


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) Like '%" + SearchKey + "%'" +
                     " OR CONCAT(U.FirstName,' ',U.LastName) Like '%" + SearchKey + "%'" +
                     " OR tt.BillableDuration Like '%" + SearchKey + "%'" +
                    ")";
                }

                string myqry3 = string.Empty;
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = " Select ID,UserID,BillableDuration,TotalHourRounded, TimesheetID,FullName, sum(COALESCE(OT1,0)) as OT1, sum(COALESCE(OT2,0)) as OT2, SUM(COALESCE(OT3,0)) as OT3  from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a group by ID,UserID,BillableDuration,TotalHourRounded, TimesheetID,FullName" + myqry3;

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry = string.Empty;

                if (monthVal == "0" || yearVal == "0")
                {
                    if (show)
                    {
                        countQry = "Select COUNT(*) as total from DBNAME.timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from DBNAME.second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from DBNAME.second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from DBNAME.second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE tt.Approved = 1 AND tt.Deleted = 0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                    else
                    {
                        countQry = "Select COUNT(*) as total from DBNAME.timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from DBNAME.second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from DBNAME.second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from DBNAME.second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE (tt.SupervisorID = " + supervisorID + " OR tt.UserID = " + supervisorID + ") AND tt.Approved = 1 AND tt.Deleted = 0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                }
                else
                {
                    if (show)
                    {
                        countQry = "Select COUNT(*) as total from timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE tt.BillableDuration LIKE '%" + yearVal + "-" + monthVal + "%' AND tt.Approved=1 AND tt.Deleted=0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                    else
                    {
                        countQry = "Select COUNT(*) as total from timesheet_tracking tt LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT1 from second_timer Where QueryType = 'Vacation' Group BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT2 from second_timer Where QueryType = 'Statutory' Group BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                                "(Select TimesheetID, SUM(OverrideTime) as OT3 from second_timer Where QueryType = 'Personal' Group BY TimesheetID) st3 ON tt.ID = st3.TimesheetID " +
                                "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                "WHERE (tt.SupervisorID = " + supervisorID + " OR tt.UserID = " + supervisorID + ") AND tt.BillableDuration LIKE '%" + yearVal + "-" + monthVal + "%' AND tt.Approved=1 AND tt.Deleted=0 AND tt.ApprovedFinal=" + ApprovedFinal + "";
                    }
                }


                if (SearchKey != "")
                {
                    countQry = countQry + " AND (CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) Like '%" + SearchKey + "%'" +
                     " OR CONCAT(U.FirstName,' ',U.LastName) Like '%" + SearchKey + "%'" +
                     " OR tt.BillableDuration Like '%" + SearchKey + "%'" +
                    ")";
                }

                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select sum(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<TimesheetTracking>(dt);
                }
                else
                {
                    return new List<TimesheetTracking>();
                }
            }
        }

        public string UpdateApprovedFinal(string valFlag,string ID)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string query = "UPDATE timesheet_tracking SET ApprovedFinal="+valFlag+" WHERE ID in ("+ID+")";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                
                cmd.ExecuteNonQuery();
                int LastID = 0;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public List<TimesheetTracking> GetSupervisorList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SupervisorID, out int TotalRecord)
        {
           // var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT tt.*,CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) as TimesheetID,CONCAT(U.FirstName,' ',U.LastName) AS FullName FROM COMMONDBNAME.timesheet_tracking tt" +
                                " INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                                " WHERE SupervisorID=" + SupervisorID + " AND Approved=0 AND SentBack=0 AND Deleted=0";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR CONCAT(U.FirstName,' ',U.LastName) LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR TotalHourRounded LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR BillableDuration LIKE '%" + SearchKey.Trim() + "%'" +
                        ")";
                }

                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {
                    myqry = myqry + " ORDER BY TimesheetID ASC";
                }
                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }
                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
               
                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;

                var countQry = string.Empty;


                countQry = "SELECT COUNT(*) FROM COMMONDBNAME.timesheet_tracking tt" +
                               " INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                               " WHERE SupervisorID=" + SupervisorID + " AND Approved=0 AND SentBack=0 AND Deleted=0";

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (CONCAT(YEAR(tt.CreatedTime),'-',COALESCE(U.ERPID,''),'-',tt.ID) LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR CONCAT(U.FirstName,' ',U.LastName) LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR TotalHourRounded LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR BillableDuration LIKE '%" + SearchKey.Trim() + "%'" +
                        ")";
                }
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
               
                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<TimesheetTracking>(dt);
                }
                else
                {
                    return Utility.ConvertDataTable<TimesheetTracking>(dt);
                }
            }
        }

        public string UpdateSentBack()
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string query = "UPDATE timesheet_tracking SET SentBack = 1 WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

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

        public string UpdateApproved()
        {
            string ExecutionString = "";
           // var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string query = "UPDATE timesheet_tracking SET Approved = 1 WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

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

        public string UpdateTotalHour()
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string query = "UPDATE timesheet_tracking SET TotalHour = @TotalHour, TotalHourRounded = @TotalHourRounded WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@TotalHour",
                    DbType = DbType.Double,
                    Value = TotalHour,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@TotalHourRounded",
                    DbType = DbType.Double,
                    Value = TotalHourRounded,
                });

                cmd.ExecuteNonQuery();
                int LastID = 0;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string SendTimesheetForProcessingAgain()
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string query = "UPDATE timesheet_tracking SET SentBack = 0, TotalHour = @TotalHour, TotalHourRounded = @TotalHourRounded WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@TotalHour",
                    DbType = DbType.Double,
                    Value = TotalHour,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@TotalHourRounded",
                    DbType = DbType.Double,
                    Value = TotalHourRounded,
                });

                cmd.ExecuteNonQuery();
                int LastID = 0;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string Insert()
        {
           // var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string query = "INSERT INTO timesheet_tracking (CreatedTime, UserID, TotalHour, BillableDuration, SupervisorID, OriginalHour, TotalHourRounded) VALUES (NOW(), @UserID, @TotalHour, @BillableDuration, @SupervisorID, @OriginalHour, @TotalHourRounded)";

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

            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    ParameterName = "@CreatedTime",
            //    DbType = DbType.DateTime,
            //    Value = CreatedTime,
            //});
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TotalHour",
                DbType = DbType.Double,
                Value = TotalHour,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@BillableDuration",
                DbType = DbType.String,
                Value = BillableDuration,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SupervisorID",
                DbType = DbType.Int32,
                Value = SupervisorID,
            });


            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OriginalHour",
                DbType = DbType.Double,
                Value = OriginalHour,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TotalHourRounded",
                DbType = DbType.Double,
                Value = TotalHourRounded,
            });

        }

        public List<TimesheetTracking> GetTotalHoursTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int supervisorID, string monthVal, string yearVal, int ApprovedFinal, int userID, bool show, out int TotalRecord)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = string.Empty;
                //if (monthVal == "0" || yearVal == "0")
                //{
                //    if (show) //(userID == 8)
                //    {
                myqry = "SELECT tt.UserID,CONCAT(U.FirstName,' ',U.LastName) AS FullName, SUM(COALESCE(st1.OT1,0)) AS OT1, SUM(COALESCE(st2.OT2,0)) AS OT2, SUM(COALESCE(st3.OT3,0))AS OT3,SUM(COALESCE(st4.OT4,0)) AS OT4,SUM(COALESCE(st5.OT5,0)) AS OT5,SUM(COALESCE(st6.OT6,0) ) AS OT6 "+
                        " FROM COMMONDBNAME.timesheet_tracking tt" +
                        " LEFT JOIN(SELECT TimesheetID, SUM(COALESCE(OverrideTime, 0)) AS OT1 FROM DBNAME.second_timer" +
                            " WHERE QueryType = 'Contract' GROUP BY TimesheetID) st1 ON tt.ID = st1.TimesheetID"+
                            " LEFT JOIN(SELECT TimesheetID, SUM(COALESCE(OverrideTime,0)) AS OT2 FROM DBNAME.second_timer" +
                            " WHERE QueryType = 'Personal' GROUP BY TimesheetID) st2 ON tt.ID = st2.TimesheetID"+
                            " LEFT JOIN(SELECT TimesheetID, SUM(COALESCE(OverrideTime,0)) AS OT3 FROM DBNAME.second_timer" +
                            " WHERE QueryType = 'Statutory' GROUP BY TimesheetID) st3 ON tt.ID = st3.TimesheetID"+
                            " LEFT JOIN(SELECT TimesheetID, SUM(COALESCE(OverrideTime,0)) AS OT4 FROM DBNAME.second_timer" +
                            " WHERE QueryType = 'Training' GROUP BY TimesheetID) st4 ON tt.ID = st4.TimesheetID"+
                            " LEFT JOIN(SELECT TimesheetID, SUM(COALESCE(OverrideTime,0)) AS OT5 FROM DBNAME.second_timer" +
                            " WHERE QueryType = 'Meetings' GROUP BY TimesheetID) st5 ON tt.ID = st5.TimesheetID"+
                            " LEFT JOIN(SELECT TimesheetID, SUM(COALESCE(OverrideTime,0)) AS OT6 FROM DBNAME.second_timer" +
                            " WHERE QueryType = 'Others' GROUP BY TimesheetID) st6 ON tt.ID = st6.TimesheetID"+
                            " INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID WHERE tt.Approved = 1 AND tt.Deleted = 0";


               

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (" +
                     " CONCAT(U.FirstName,' ',U.LastName) Like '%" + SearchKey + "%'" +
                    ")";
                }

                myqry = myqry + " GROUP BY tt.UserID";

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = " Select UserID, FullName, SUM(COALESCE(OT1,0)) AS OT1, SUM(COALESCE(OT2,0)) AS OT2, SUM(COALESCE(OT3,0))AS OT3,SUM(COALESCE(OT4,0)) AS OT4,SUM(COALESCE(OT5,0)) AS OT5,SUM(COALESCE(OT6,0) ) AS OT6  from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a group by UserID " + myqry3;


                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry = string.Empty;

                //if (monthVal == "0" || yearVal == "0")
                //{
                //    if (show)
                //    {
                countQry = "SELECT COUNT(*) as total FROM ("+
                            " SELECT tt.userid,U.FirstName,U.LastName FROM COMMONDBNAME.timesheet_tracking tt " +
                            "LEFT JOIN(SELECT TimesheetID, SUM(OverrideTime) AS OT1 FROM DBNAME.second_timer WHERE QueryType = 'Contract' GROUP BY TimesheetID) st1 ON tt.ID = st1.TimesheetID "+
                            "LEFT JOIN(SELECT TimesheetID, SUM(OverrideTime) AS OT2 FROM DBNAME.second_timer WHERE QueryType = 'Personal' GROUP BY TimesheetID) st2 ON tt.ID = st2.TimesheetID "+
                            "LEFT JOIN(SELECT TimesheetID, SUM(OverrideTime) AS OT3 FROM DBNAME.second_timer WHERE QueryType = 'Statutory' GROUP BY TimesheetID) st3 ON tt.ID = st3.TimesheetID "+
                            "LEFT JOIN(SELECT TimesheetID, SUM(OverrideTime) AS OT4 FROM DBNAME.second_timer WHERE QueryType = 'Training' GROUP BY TimesheetID) st4 ON tt.ID = st4.TimesheetID "+
                            "LEFT JOIN(SELECT TimesheetID, SUM(OverrideTime) AS OT5 FROM DBNAME.second_timer WHERE QueryType = 'Meetings' GROUP BY TimesheetID) st5 ON tt.ID = st5.TimesheetID "+
                            "LEFT JOIN(SELECT TimesheetID, SUM(OverrideTime) AS OT6 FROM DBNAME.second_timer WHERE QueryType = 'Others' GROUP BY TimesheetID) st6 ON tt.ID = st6.TimesheetID "+
                            "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                            "WHERE tt.Approved = 1 AND tt.Deleted = 0 GROUP BY CONCAT(U.FirstName, ' ', U.LastName)) AS temp";

               
                if (SearchKey != "")
                {
                    countQry = countQry + " where (" +
                     " CONCAT(FirstName,' ',LastName) Like '%" + SearchKey + "%'" +
                    ")";
                }


                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select sum(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<TimesheetTracking>(dt);
                }
                else
                {
                    return new List<TimesheetTracking>();
                }
            }
        }

        public List<TimesheetTracking> GetTotalWeeksTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int supervisorID, string monthVal, string yearVal, int ApprovedFinal, int userID, bool show, out int TotalRecord)
        {
           // var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = string.Empty;
                //if (monthVal == "0" || yearVal == "0")
                //{
                //    if (show) //(userID == 8)
                //    {
                myqry = "SELECT tt.ID,tt.billableduration,tt.UserID ,CONCAT(U.FirstName,' ',U.LastName) AS FullName, st1.OT1, st2.OT2, st3.OT3,st4.OT4,st5.OT5,st6.OT6 " +
                        "FROM COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT1 FROM DBNAME.second_timer WHERE QueryType = 'Contract' GROUP BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT2 FROM DBNAME.second_timer WHERE QueryType = 'Personal' GROUP BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT3 FROM DBNAME.second_timer WHERE QueryType = 'Statutory' GROUP BY TimesheetID) st3 ON tt.ID = st3.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT4 FROM DBNAME.second_timer WHERE QueryType = 'Training' GROUP BY TimesheetID) st4 ON tt.ID = st4.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT5 FROM DBNAME.second_timer WHERE QueryType = 'Meetings' GROUP BY TimesheetID) st5 ON tt.ID = st5.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT6 FROM DBNAME.second_timer WHERE QueryType = 'Others' GROUP BY TimesheetID) st6 ON tt.ID = st6.TimesheetID " +
                        "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                        "WHERE tt.Approved = 1 AND tt.Deleted = 0 ";
                        


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (" +
                     " CONCAT(U.FirstName,' ',U.LastName) Like '%" + SearchKey + "%'" +
                    ")";
                }

                string myqry3 = "";
                myqry3 = myqry3 + " ORDER BY FullName,ID"; 
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = " Select ID,billableduration,UserID ,FullName, SUM(COALESCE(OT1,0)) AS OT1, SUM(COALESCE(OT2,0)) AS OT2, SUM(COALESCE(OT3,0))AS OT3,SUM(COALESCE(OT4,0)) AS OT4,SUM(COALESCE(OT5,0)) AS OT5,SUM(COALESCE(OT6,0) ) AS OT6  from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a group by FullName " + myqry3;


                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry = string.Empty;

                //if (monthVal == "0" || yearVal == "0")
                //{
                //    if (show)
                //    {
                countQry = "SELECT COUNT(*) as total " +
                        "FROM COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT1 FROM DBNAME.second_timer WHERE QueryType = 'Contract' GROUP BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT2 FROM DBNAME.second_timer WHERE QueryType = 'Personal' GROUP BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT3 FROM DBNAME.second_timer WHERE QueryType = 'Statutory' GROUP BY TimesheetID) st3 ON tt.ID = st3.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT4 FROM DBNAME.second_timer WHERE QueryType = 'Training' GROUP BY TimesheetID) st4 ON tt.ID = st4.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT5 FROM DBNAME.second_timer WHERE QueryType = 'Meetings' GROUP BY TimesheetID) st5 ON tt.ID = st5.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT6 FROM DBNAME.second_timer WHERE QueryType = 'Others' GROUP BY TimesheetID) st6 ON tt.ID = st6.TimesheetID " +
                        "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                        "WHERE tt.Approved = 1 AND tt.Deleted = 0 ";

                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select sum(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";


                if (SearchKey != "")
                {
                    countQry = countQry + " where (" +
                     " CONCAT(FirstName,' ',LastName) Like '%" + SearchKey + "%'" +
                    ")";
                }



                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<TimesheetTracking>(dt);
                }
                else
                {
                    return new List<TimesheetTracking>();
                }
            }
        }
       
        public List<TimesheetTracking> GetTotalWeeksByUsersTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int supervisorID, string monthVal, string yearVal, int ApprovedFinal, int userID, bool show, out int TotalRecord)
        {
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = string.Empty;
                //if (monthVal == "0" || yearVal == "0")
                //{
                //    if (show) //(userID == 8)
                //    {
                myqry = "SELECT tt.ID,tt.billableduration,tt.UserID ,CONCAT(U.FirstName,' ',U.LastName) AS FullName, st1.OT1, st2.OT2, st3.OT3,st4.OT4,st5.OT5,st6.OT6 " +
                        "FROM COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT1 FROM DBNAME.second_timer WHERE QueryType = 'Contract' GROUP BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT2 FROM DBNAME.second_timer WHERE QueryType = 'Personal' GROUP BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT3 FROM DBNAME.second_timer WHERE QueryType = 'Statutory' GROUP BY TimesheetID) st3 ON tt.ID = st3.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT4 FROM DBNAME.second_timer WHERE QueryType = 'Training' GROUP BY TimesheetID) st4 ON tt.ID = st4.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT5 FROM DBNAME.second_timer WHERE QueryType = 'Meetings' GROUP BY TimesheetID) st5 ON tt.ID = st5.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT6 FROM DBNAME.second_timer WHERE QueryType = 'Others' GROUP BY TimesheetID) st6 ON tt.ID = st6.TimesheetID " +
                        "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                        "WHERE tt.Approved = 1 AND tt.Deleted = 0 "+
                        "AND tt.userid = " + userID.ToString();


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (" +
                     " CONCAT(U.FirstName,' ',U.LastName) Like '%" + SearchKey + "%'" +
                    ")";
                }

                string myqry3 = "";
                myqry3 = myqry3 + " ORDER BY FullName,ID";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = " Select ID,billableduration,UserID , FullName,sum(COALESCE(OT1,0)) as OT1, sum(COALESCE(OT2,0)) as OT2, SUM(COALESCE(OT3,0)) as OT3 ,SUM(COALESCE(OT4,0)) as OT4,SUM(COALESCE(OT5,0)) as OT5,SUM(COALESCE(OT6,0)) as OT6 from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a group by ID,billableduration,UserID , FullName " + myqry3;


                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();TotalHourRounded

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry = string.Empty;

                //if (monthVal == "0" || yearVal == "0")
                //{
                //    if (show)
                //    {
                countQry = "SELECT COUNT(*) as total " +
                        "FROM COMMONDBNAME.timesheet_tracking tt LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT1 FROM DBNAME.second_timer WHERE QueryType = 'Contract' GROUP BY TimesheetID) st1 ON tt.ID = st1.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT2 FROM DBNAME.second_timer WHERE QueryType = 'Personal' GROUP BY TimesheetID) st2 ON tt.ID = st2.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT3 FROM DBNAME.second_timer WHERE QueryType = 'Statutory' GROUP BY TimesheetID) st3 ON tt.ID = st3.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT4 FROM DBNAME.second_timer WHERE QueryType = 'Training' GROUP BY TimesheetID) st4 ON tt.ID = st4.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT5 FROM DBNAME.second_timer WHERE QueryType = 'Meetings' GROUP BY TimesheetID) st5 ON tt.ID = st5.TimesheetID LEFT JOIN " +
                        "(SELECT TimesheetID, SUM(OverrideTime) AS OT6 FROM DBNAME.second_timer WHERE QueryType = 'Others' GROUP BY TimesheetID) st6 ON tt.ID = st6.TimesheetID " +
                        "INNER JOIN COMMONDBNAME.users U ON U.ID = tt.UserID " +
                        "WHERE tt.Approved = 1 AND tt.Deleted = 0 "+
                         "AND tt.userid = " + userID.ToString();
                

                if (SearchKey != "")
                {
                    countQry = countQry + " where (" +
                     " CONCAT(FirstName,' ',LastName) Like '%" + SearchKey + "%'" +
                    ")";
                }


                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select sum(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";


                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<TimesheetTracking>(dt);
                }
                else
                {
                    return new List<TimesheetTracking>();
                }
            }
        }
    }
}
