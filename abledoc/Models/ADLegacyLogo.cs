using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ADLegacyLogo
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ClientID { get; set; }
        public byte[] logo { get; set; }
        public string contentType { get; set; }
        public string databasename { get; set; }
        #endregion

        public ADLegacyLogo GetLegacyLogoById(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adlegacy_logo WHERE ClientID= " + id;
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
                    return ConvertDataTable<ADLegacyLogo>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string UpdateLegacyLogo()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                if (!IsExist())
                {
                    try
                    {
                        string query = "INSERT INTO adlegacy_logo (ClientID, contentType, logo) VALUES (@ClientID, @contentType, @logo)";


                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        int a = cmd.ExecuteNonQuery();

                        ExecutionString = "Replace done";
                    }
                    catch (Exception ex)
                    {
                        ExecutionString = ex.Message;
                    }
                }
                else
                {
                    try
                    {
                        string query = "UPDATE adlegacy_logo SET logo=@logo,contentType=@contentType" +
                            " WHERE ClientID=@ClientID";


                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        int a = cmd.ExecuteNonQuery();

                        ExecutionString = "Update legacy logo";
                    }
                    catch (Exception ex)
                    {
                        ExecutionString = ex.Message;
                    }
                }

               
                

                conn.Close();
            }
            return ExecutionString;
        }

        public bool IsExist()
        {
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);
            bool isexist = false;
            //Password = abledoc.Utility.CommonHelper.EncryptString(Password);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ClientID FROM adlegacy_logo WHERE ClientID='" + ClientID + "'";
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
                {
                    isexist = true;
                }
            }
            return isexist;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientID",
                DbType = DbType.Int32,
                Value = ClientID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@logo",
                MySqlDbType = MySqlDbType.Blob,
                Value = logo,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@contentType",
                DbType = DbType.String,
                Value = contentType,
            });

        }

    }
}
