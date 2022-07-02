using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ADSalesPhone
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int adsalesphoneid { get; set; }
        public int adsalesid { get; set; }
        public string phone { get; set; }
        public string type { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<ADSalesPhone> GetList(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT adsalesphoneid, adsalesid, phone,type FROM adsales_phone WHERE adsalesid =" + id + " ORDER BY adsalesphoneid ASC";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                // BindId(cmd);
                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<ADSalesPhone>(dt);
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

                    string query = "INSERT INTO adsales_phone(adsalesid, phone,type) VALUES(@adsalesid, @phone,@type)";

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

        public string Delete()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "DELETE FROM adsales_phone WHERE adsalesid = " + adsalesid;

                MySqlCommand cmd = new MySqlCommand(query, conn);
                
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@adsalesid",
                    DbType = DbType.Int32,
                    Value = adsalesid
                });

                cmd.ExecuteNonQuery();
                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@adsalesid",
                DbType = DbType.Int32,
                Value = adsalesid
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phone",
                DbType = DbType.String,
                Value = phone,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@type",
                DbType = DbType.String,
                Value = type,
            });

        }

    }
}
