using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsDeliveryContact
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public Int64 JobID { get; set; }
        public Int64 ContactID { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<JobsDeliveryContact> GetJobDeliveryContactList()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_delivery_contact where JobID=@JobID";
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
                    return Utility.ConvertDataTable<JobsDeliveryContact>(dt);
                else
                    return null;
            }
        }


        public string InsertContact()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                int count = 0;
                int TotalRecord = 0;
                var countQry = "SELECT COUNT(*) FROM jobs_delivery_contact where JobID=@JobID and ContactID=@ContactID";
                MySqlCommand cou = new MySqlCommand(countQry, conn);
                BindParams(cou);
                count = Convert.ToInt32(cou.ExecuteScalar());
                TotalRecord = count;
                if (TotalRecord == 0)
                {
                    string query = "INSERT INTO jobs_delivery_contact(JobID,ContactID) VALUES(@JobID,@ContactID)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    cmd.ExecuteNonQuery();
                    ExecutionString = "Update Succefully";
                }
                conn.Close();
            }
            return ExecutionString;
        }

        public string DeleteContact(int JobID, string ContactID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "DELETE FROM jobs_delivery_contact WHERE JobID = " + JobID;
                if (ContactID.ToString() != "") {
                    query += " AND ContactID NOT IN (" + ContactID + ")";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                

                cmd.ExecuteNonQuery();

                ExecutionString = "Delete Succefully";

                conn.Close();
            }
            return ExecutionString;
        }
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int64,
                Value = JobID,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ContactID",
                DbType = DbType.Int64,
                Value = ContactID,
            });
           
            
            
            
        }

    }
}
