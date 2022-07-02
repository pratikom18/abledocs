using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesCheckouts
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int UserID { get; set; }
        public int FileID { get; set; }
        public DateTime Checkout { get; set; }
        public DateTime Checkin { get; set; }
        public int Checkout_PageNumber { get; set; }
        public int Checkin_PageNumber { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }
        public string State { get; set; }
        public string NextState { get; set; }
        public string ExtraData { get; set; }
        public int TimesheetID { get; set; }
        public string Comment { get; set; }
        public double ActualTime { get; set; }
        public double OverrideTime { get; set; }
        public string SupervisorComment { get; set; }
        public int TimerCompare { get; set; }
        public string FullName { get; set; }
        public string databasename { get; set; }
        public List<JobsFilesCheckouts> GetUserListByJobID(long id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT DISTINCT UserID,CONCAT(u.firstname,' ',u.lastname) AS FullName FROM jobs_files_checkouts jfc " +
                              "INNER JOIN COMMONDBNAME.users u ON jfc.UserID = u.ID " +
                              "LEFT JOIN jobs_files jf ON jfc.FileID = jf.ID " +
                               "WHERE jf.JobID = " + id;

                qry = qry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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
                    return Utility.ConvertDataTable<JobsFilesCheckouts>(dt);
                else
                    return null;
            }
        }

        public string CreateJobsFilesCheckouts()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "INSERT INTO jobs_files_checkouts (FileID, UserID, Checkout, Checkout_PageNumber, State) VALUES (@FileID,@UserID,@Checkout,@Checkout_PageNumber,@State)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);
                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Checkout",
                DbType = DbType.DateTime,
                Value = Checkout,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Checkout_PageNumber",
                DbType = DbType.Int32,
                Value = Checkout_PageNumber,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@State",
                DbType = DbType.String,
                Value = State,
            });

        }

        public List<JobsFilesCheckouts> GetJobsFilesCheckoutsByFileID(int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * from jobs_files_checkouts Where FileID=" + fileID + " AND State<>'ALT' Order By ID DESC Limit 2";
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
                    return Utility.ConvertDataTable<JobsFilesCheckouts>(dt);
                else
                    return null;
            }
        }

        public string UpdateJobsFilesCheckOut()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_checkouts SET Timer = @Timer, Checkin = @Checkin WHERE Checkin IS NULL AND FileID = @FileID AND UserID = @UserID ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UserID",
                    DbType = DbType.Int32,
                    Value = UserID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Timer",
                    DbType = DbType.String,
                    Value = Timer,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Checkin",
                    DbType = DbType.DateTime,
                    Value = Checkin,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesCheckOutByFileIdUserId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_checkouts SET Checkin = @Checkin, Timer = @Timer, State = @State WHERE Checkin IS NULL AND FileID = @FileID AND UserID = @UserID ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UserID",
                    DbType = DbType.Int32,
                    Value = UserID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Timer",
                    DbType = DbType.String,
                    Value = Timer,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Checkin",
                    DbType = DbType.DateTime,
                    Value = Checkin,
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@State",
                    DbType = DbType.String,
                    Value = State,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesByFileIdUserId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_checkouts SET Checkin = @Checkin, Timer = @Timer, State = @State,Checkin_PageNumber=@Checkin_PageNumber WHERE Checkin IS NULL AND FileID = @FileID AND UserID = @UserID ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UserID",
                    DbType = DbType.Int32,
                    Value = UserID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Timer",
                    DbType = DbType.String,
                    Value = Timer,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Checkin",
                    DbType = DbType.DateTime,
                    Value = Checkin,
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@State",
                    DbType = DbType.String,
                    Value = State,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Checkin_PageNumber",
                    DbType = DbType.Int32,
                    Value = Checkin_PageNumber,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public JobsFilesCheckouts GetJobsFilesCheckoutsByFileIDState()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * From jobs_files_checkouts Where FileID=@FileID AND State=@State Order By ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@State",
                    DbType = DbType.String,
                    Value = State
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
                    return Utility.ConvertDataTable<JobsFilesCheckouts>(dt).FirstOrDefault();
                else
                    return null;
            }
        }

        public string UpdateJobsFilesCheckOutTimerTimerCompareById()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_checkouts SET Timer = @Timer, TimerCompare = @TimerCompare WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Timer",
                    DbType = DbType.String,
                    Value = Timer,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@TimerCompare",
                    DbType = DbType.Int32,
                    Value = TimerCompare,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsNextStateByFileID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files_checkouts SET NextState = @NextState WHERE FileID = @FileID ORDER BY ID DESC LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@NextState",
                    DbType = DbType.String,
                    Value = NextState
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesExtraDataByFileIdUserId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_checkouts SET Timer = @Timer, State = @State, Checkin = @Checkin, ExtraData = @ExtraData WHERE Checkin IS NULL AND FileID = @FileID AND UserID = @UserID ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UserID",
                    DbType = DbType.Int32,
                    Value = UserID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Timer",
                    DbType = DbType.String,
                    Value = Timer,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Checkin",
                    DbType = DbType.DateTime,
                    Value = Checkin,
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@State",
                    DbType = DbType.String,
                    Value = State,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ExtraData",
                    DbType = DbType.String,
                    Value = ExtraData,
                });
                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }
    }
}
