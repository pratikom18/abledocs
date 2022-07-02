using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class BugReport
    {
        DataContext context = new DataContext();
        public Bug bug { get; set; }
        public BugText bugText { get; set; }
        public BugHistory bugHistory { get; set; }

        public Users users { get; set; }


        //public string InsertUpdateBugText()
        //{
        //    string ExecutionString = "";
        //    using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))
        //    {
        //        conn.Open();

        //        string query = "INSERT INTO bug_text (description,steps_to_reproduce,additional_information)" +
        //            " VALUES(@description,@steps_to_reproduce,@additional_information)";


        //            MySqlCommand cmd = new MySqlCommand(query, conn);
        //            BindBugTextParams(cmd);

        //            cmd.ExecuteNonQuery();
        //            int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

        //            ExecutionString = LastID.ToString();

        //        conn.Close();
        //    }
        //    return ExecutionString;
        //}

        //private void BindBugTextParams(MySqlCommand cmd)
        //{

        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@description",
        //        DbType = DbType.String,
        //        Value = bugText.Description,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@steps_to_reproduce",
        //        DbType = DbType.String,
        //        Value = bugText.Steps_To_Reproduce,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@additional_information",
        //        DbType = DbType.String,
        //        Value = bugText.Additional_Information,
        //    });

        //}




        //public string InsertUpdateBug()
        //{
        //    string ExecutionString = "";
        //    using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))
        //    {
        //        conn.Open();

        //        string query = "INSERT INTO bug (priority, severity, project_id, category_id, reporter_id, bug_text_id, summary, date_submitted, last_updated)" +
        //            " VALUES(@priority, @severity, @project_id, @category_id, @reporter_id, @bug_text_id, @summary, @date_submitted, @last_updated)";


        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        BindBugParams(cmd);

        //        cmd.ExecuteNonQuery();
        //        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

        //        ExecutionString = LastID.ToString();

        //        conn.Close();
        //    }
        //    return ExecutionString;
        //}

        //private void BindBugParams(MySqlCommand cmd)
        //{

        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@priority",
        //        DbType = DbType.String,
        //        Value = bug.Priority,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@severity",
        //        DbType = DbType.String,
        //        Value = bug.Severity,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@project_id",
        //        DbType = DbType.Int32,
        //        Value = bug.Project_Id,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@category_id",
        //        DbType = DbType.Int32,
        //        Value = bugText.Additional_Information,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@reporter_id",
        //        DbType = DbType.Int32,
        //        Value = bug.Reporter_Id,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@bug_text_id",
        //        DbType = DbType.Int32,
        //        Value = bug.Bug_Text_Id,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@summary",
        //        DbType = DbType.String,
        //        Value = bug.Summary,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@date_submitted",
        //        DbType = DbType.DateTime,
        //        Value = bug.Date_Submitted,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@last_updated",
        //        DbType = DbType.DateTime,
        //        Value = bug.Last_Updated,
        //    });

        //}



        //public string InsertUpdateBugHistory()
        //{
        //    string ExecutionString = "";
        //    using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))
        //    {
        //        conn.Open();

        //        string query = "INSERT INTO bug_history  (user_id, bug_id, date_modified, type, field_name, old_value, new_value)" +
        //            " VALUES(@user_id, @bug_id, @date_modified,1, '','', '')";


        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        BindBugHistoryParams(cmd);

        //        cmd.ExecuteNonQuery();
        //        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

        //        ExecutionString = LastID.ToString();

        //        conn.Close();
        //    }
        //    return ExecutionString;
        //}

        //private void BindBugHistoryParams(MySqlCommand cmd)
        //{

        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@user_id",
        //        DbType = DbType.Int32,
        //        Value = bugHistory.User_Id,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@bug_id",
        //        DbType = DbType.Int32,
        //        Value = bugHistory.Bug_Id,
        //    });
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@date_modified",
        //        DbType = DbType.DateTime,
        //        Value = bugHistory.Date_Modified,
        //    });


        //}
    }
}