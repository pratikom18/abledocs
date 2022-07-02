using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesQC
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int VersionID {get;set;}
        public int QualityControllerID { get; set; }
        public int FileID { get; set; }
        public int JobID { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public string FullName { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<JobsFilesQC> GetQCLC(int jobID, int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT j.*,CONCAT(u.FirstName, ' ' ,u.LastName) AS FullName FROM jobs_files_qc j "+
                                "Left JOIN COMMONDBNAME.users u ON u.ID = j.QualityControllerID " +
                                "WHERE j.JobID = "+jobID+" AND j.FileID = "+fileID+" "+
                                "ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesQC>(dt);
                else
                    return null;
            }
        }

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = "INSERT INTO jobs_files_qc (VersionID,QualityControllerID,FileID,JobID,LastUpdated) VALUES (@VersionID,@QualityControllerID,@FileID,@JobID,@LastUpdated)";

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

        public string UpdateJobsFilesQcCommentByVesrionIdLastId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_qc SET Comments = @Comments WHERE VersionID = @VersionID AND ID = @lastId";
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
                ParameterName = "@QualityControllerID",
                DbType = DbType.Int32,
                Value = QualityControllerID,
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
