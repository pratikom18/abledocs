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
    public class BugText
    {
        DataContext context = new DataContext();

        public int Id { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string Description  { get; set; }
        [Required(ErrorMessage = "Steps_To_Reproduce is Required")]
        public string Steps_To_Reproduce  { get; set; }
        public string Additional_Information  { get; set; }
        public string databasename { get; set; }


        public string InsertUpdateBugText()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))
            {
                conn.Open();

                string query = "INSERT INTO bug_text (description,steps_to_reproduce,additional_information)" +
                    " VALUES(@description,@steps_to_reproduce,'')";

                //string query = "INSERT INTO bug_text (description,steps_to_reproduce,additional_information)" +
                //    " VALUES(@description,@steps_to_reproduce,@additional_information)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindBugTextParams(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindBugTextParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = Description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@steps_to_reproduce",
                DbType = DbType.String,
                Value = Steps_To_Reproduce,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@additional_information",
                DbType = DbType.String,
                Value = Additional_Information,
            });

        }
    }
}
