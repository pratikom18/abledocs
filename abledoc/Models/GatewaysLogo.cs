using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class GatewaysLogo
    {
        #region "Properties"
        DataContext context = new DataContext();
        public string subdomain { get; set; }
        public string contentType { get; set; }
        public byte[] logo { get; set; }
        public string databasename { get; set; }
        #endregion

        public GatewaysLogo GetewayById(string subdomain)
        {
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM gateways_logo WHERE subdomain= '" + subdomain + "'";
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
                    return ConvertDataTable<GatewaysLogo>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }
        public string UpdateGateway()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                if (!IsExist())
                {
                    try
                    {
                        string query = "INSERT INTO gateways_logo(subdomain,logo,contentType) Values (@subdomain,@logo,@contentType)";


                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        int a = cmd.ExecuteNonQuery();

                        ExecutionString = subdomain;
                    }
                    catch (Exception ex)
                    {
                        ExecutionString = ex.Message;
                    }
                }
                else {
                    try
                    {
                        string query = "UPDATE gateways_logo SET logo=@logo,contentType=@contentType" +
                            " WHERE subdomain=@subdomain";


                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        int a = cmd.ExecuteNonQuery();

                        ExecutionString = subdomain;
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
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@subdomain",
                DbType = DbType.String,
                Value = subdomain,
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
        public bool IsExist()
        {
            bool isexist = false;
            //Password = abledoc.Utility.CommonHelper.EncryptString(Password);
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT subdomain FROM gateways_logo WHERE subdomain='"+subdomain+"'";
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
    }
}
