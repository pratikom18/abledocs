using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Bug
    {
        DataContext context = new DataContext();
        public int Id { get; set; }
        [Required(ErrorMessage = "Priority is Required")]
        public int Priority { get; set; }
        [Required(ErrorMessage = "Severity is Required")]
        public int Severity { get; set; }
        public int Project_Id  { get; set; }
        public int Category_Id  { get; set; }
        public int Reporter_Id  { get; set; }
        public int Bug_Text_Id { get; set; }
        [Required(ErrorMessage = "Summary is Required")]
        public string Summary  { get; set; }
        public DateTime Date_Submitted  { get; set; }
        public DateTime Last_Updated  { get; set; }
        public string databasename { get; set; }
        public string InsertUpdateBug()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))
            {
                conn.Open();
                string query = "INSERT INTO bug (priority, severity, project_id, category_id, reporter_id, bug_text_id, summary, date_submitted, last_updated)" +
                    " VALUES(@priority, @severity, 1, 1, @reporter_id, @bug_text_id, @summary,@date_submitted, @last_updated)";

               // string query = "INSERT INTO bug (priority, severity, project_id, category_id, reporter_id, bug_text_id, summary, date_submitted, last_updated)" +
               //" VALUES(@priority, @severity, 1, @category_id, @reporter_id, @bug_text_id, @summary, '', '')";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindBugParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindBugParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@priority",
                DbType = DbType.Int32,
                Value = Priority,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@severity",
                DbType = DbType.Int32,
                Value = Severity,
            });
            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    ParameterName = "@project_id",
            //    DbType = DbType.Int32,
            //    Value = Project_Id,
            //});
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@category_id",
                DbType = DbType.Int32,
                Value = Category_Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reporter_id",
                DbType = DbType.Int32,
                Value = Reporter_Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@bug_text_id",
                DbType = DbType.Int32,
                Value = Bug_Text_Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@summary",
                DbType = DbType.String,
                Value = Summary,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date_submitted",
                DbType = DbType.Int32,
                Value = abledoc.Utility.CommonHelper.ConvertToTimestamp(DateTime.Today),
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_updated",
                DbType = DbType.Int32,
                Value = abledoc.Utility.CommonHelper.ConvertToTimestamp(DateTime.Today),
            });

        }
    }
}
