using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsCustomFields
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int JobID { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<JobsCustomFields> GetJobsCustomFieldById()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT FieldName, FieldValue FROM jobs_custom_fields WHERE 1 AND JobID= @JobID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
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
                    return Utility.ConvertDataTable<JobsCustomFields>(dt);
                else
                    return null;
            }
        }
    }
}
