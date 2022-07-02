using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Conversations
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public string Conversation { get; set; }
        public int FileID { get; set; }
        public int JobID { get; set; }
        public int UserID { get; set; }
        public DateTime LastUpdated { get; set; }
        public string FullName { get; set; }
        public string databasename { get; set; }
        #endregion

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "INSERT INTO conversation (Conversation, JobID, FileID, UserID) VALUES (@Conversation,@JobID,@FileID,@UserID)";

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
            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    ParameterName = "@ClientID",
            //    DbType = DbType.Int32,
            //    Value = ClientID,
            //});

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Conversation",
                DbType = DbType.String,
                Value = Conversation,
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
                ParameterName = "@UserID",
                DbType = DbType.Int32,
                Value = UserID,
            });
            
        }

        public List<Conversations> GetConversationsList( int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT c.ID, c.Conversation AS conversation, c.UserID, c.LastUpdated , CONCAT(u.FirstName,' ', u.LastName) AS FullName "+
                                "FROM conversation c "+
                                "LEFT JOIN abledocs.users u ON c.UserID = u.ID " +
                                "WHERE c.FileID = "+ fileID + " ORDER BY c.LastUpdated DESC";
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
                    return Utility.ConvertDataTable<Conversations>(dt);
                else
                    return null;
            }
        }
    }
}
