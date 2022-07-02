using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesProgress
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int FileID { get; set; }
        public int UserID { get; set; }
        public int PageNumber { get; set; }
        public string Progress { get; set; }
        public DateTime Submitted { get; set; }
        public string databasename { get; set; }
        #endregion

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "INSERT INTO jobs_files_progress (FileID, Progress, PageNumber, Submitted, UserID) VALUES (@FileID, @Progress, @PageNumber, @Submitted, @UserID)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
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
                ParameterName = "@Progress",
                DbType = DbType.String,
                Value = Progress,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PageNumber",
                DbType = DbType.Int32,
                Value = PageNumber,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Submitted",
                DbType = DbType.DateTime,
                Value = Submitted,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });

        }
    }
}
