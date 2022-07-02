using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesInteractions
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public int FileID { get; set; }
        public int FileVersionID { get; set; }
        public int UserID { get; set; }
        public string Activity { get; set; }
        public DateTime LastUpdated { get; set; }
        public string databasename { get; set; }

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = "INSERT INTO jobs_files_interactions (FileID, FileVersionID, JobID, UserID, LastUpdated, Activity) VALUES (@FileID, @FileVersionID, @JobID, @UserID, @LastUpdated, @Activity)";

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

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileVersionID",
                DbType = DbType.Int32,
                Value = FileVersionID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Activity",
                DbType = DbType.String,
                Value = Activity,
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
