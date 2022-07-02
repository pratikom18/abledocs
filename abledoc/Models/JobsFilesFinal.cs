using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesFinal
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int VersionID { get; set; }
        public int FinalizerID { get; set; }
        public int FileID { get; set; }
        public int JobID { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public string FullName { get; set; }
 
        #endregion

        public List<JobsFilesFinal> GetFLC(int jobID, int fileID)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT j.*,CONCAT(u.FirstName,' ',u.LastName) AS FullName FROM jobs_files_final j " +
                                "LEFT JOIN COMMONDBNAME.users u ON j.FinalizerID = u.ID " +
                                "WHERE JobID = " + jobID + " AND FileID = " + fileID + " ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesFinal>(dt);
                else
                    return null;
            }
        }

        public string Insert()
        {
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();

                    string query = "INSERT INTO jobs_files_final (VersionID,FinalizerID,FileID,JobID,LastUpdated) VALUES (@VersionID,@FinalizerID,@FileID,@JobID,@LastUpdated)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());
                    ExecutionString = LastID.ToString();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesFinalCommentByVesrionIdLastId()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {

                conn.Open();
                string query = "UPDATE jobs_files_final SET Comments = @Comments WHERE VersionID = @VersionID AND ID = @lastId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Comments",
                    DbType = DbType.String,
                    Value = Comments,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@VersionID",
                    DbType = DbType.Int32,
                    Value = VersionID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@lastId",
                    DbType = DbType.Int32,
                    Value = ID,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VersionID",
                DbType = DbType.Int32,
                Value = VersionID
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FinalizerID",
                DbType = DbType.Int32,
                Value = FinalizerID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastUpdated",
                DbType = DbType.DateTime,
                Value = LastUpdated,
            });
        }

        
    }
}
