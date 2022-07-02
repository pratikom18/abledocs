using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ADscanFiles
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int crawl_id { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public int NumPages { get; set; }
        public string Lang { get; set; }
        public string Tagged { get; set; }
        public string Fillable { get; set; }
        public string Producer { get; set; }
        public string Author { get; set; }
        public string Creator { get; set; }
        public string OpenPassword { get; set; }
        public string Title { get; set; }
        public int FileSize { get; set; }
        public string ModDate { get; set; }
        public string Keywords { get; set; }
        public string CreationDate { get; set; }
        public string Subject { get; set; }
        public string ___possibly_ocr { get; set; }
        public string ___rotate { get; set; }
        public string ___crop_box_top { get; set; }
        public string ___crop_box_bottom { get; set; }
        public string ___crop_box_left { get; set; }
        public string ___crop_box_right { get; set; }
        public string ISO { get; set; }
        public int offsite { get; set; }
        public int adscan_success { get; set; }
        public string errortext { get; set; }
        public string Error { get; set; }
        public int ADO_FileID { get; set; }
        public int passed_check { get; set; }
        public int warned_check { get; set; }
        public int failed_check { get; set; }
        public string UA_Index { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
        #endregion

        public List<ADscanFiles> GetADscanfileslise(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, url, filename FROM adscan_files WHERE 1 AND offsite != 1 AND crawl_id ="+id+" ORDER BY ID ASC";
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
                    return Utility.ConvertDataTable<ADscanFiles>(dt);
                else
                    return null;
            }
        }

        public List<ADscanFiles> GetADscanfilesliset(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adscan_files WHERE 1 AND crawl_id =" + id + " AND adscan_success=1 ORDER BY ID ASC";
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
                    return Utility.ConvertDataTable<ADscanFiles>(dt);
                else
                    return new List<ADscanFiles>();
            }
        }

        public string Insert()
        {
            string ExecutionString = "";
            try
            {
                var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();
                    
                        string query = "INSERT INTO adscan_files(crawl_id,url,filename,offsite) VALUES(@crawl_id,@url,@filename,@offsite)";

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

        public string Deleteadcsanfile(int id)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "DELETE FROM adscan_files WHERE ID=" + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

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
                ParameterName = "@crawl_id",
                DbType = DbType.Int32,
                Value = crawl_id
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@url",
                DbType = DbType.String,
                Value = url,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@filename",
                DbType = DbType.String,
                Value = filename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@offsite",
                DbType = DbType.Int32,
                Value = offsite,
            });
           
        }
    }
}
