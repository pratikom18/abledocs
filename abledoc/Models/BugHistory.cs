using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class BugHistory
    {
        DataContext context = new DataContext();
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Bug_Id { get; set; }
        public DateTime Date_Modified { get; set; }
        public int Type { get; set; }
        public string Field_Name { get; set; }
        public string Old_Value { get; set; }
        public string New_Value { get; set; }
        public string databasename { get; set; }

        public string InsertUpdateBugHistory()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))
            {
                conn.Open();

                //string query = "INSERT INTO bug_history(user_id, bug_id, date_modified, type, field_name, old_value, new_value)" +
                //    " VALUES('', '', @date_modified,1, '','', '')";

                //     string query = "INSERT INTO bug_history(user_id, bug_id, date_modified, type, field_name, old_value, new_value)" +
                //" VALUES(@user_id, @bug_id, @date_modified,1, '','', '') select last_insert_id()";


                string query = "INSERT INTO bug_history(user_id, bug_id, date_modified, type, field_name, old_value, new_value)" +
           " VALUES(@user_id, @bug_id, @date_modified,1, '','', '')";

                //select scope_identity();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindBugHistoryParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindBugHistoryParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@user_id",
                DbType = DbType.Int32,
                Value = User_Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@bug_id",
                DbType = DbType.Int32,
                Value = Bug_Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date_modified",
                DbType = DbType.Int32,
                Value = abledoc.Utility.CommonHelper.ConvertToTimestamp(DateTime.Today),
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@field_name",
                DbType = DbType.String,
                Value = Field_Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@old_value",
                DbType = DbType.String,
                Value = Old_Value,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@new_value",
                DbType = DbType.String,
                Value = New_Value,
            });

        }
    }
}
