using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ADscanCrawls
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public Int64 clientID { get; set; }
        public string starturl { get; set; }
        public string status { get; set; }
        public string ScanType { get; set; }
        public string accesskey { get; set; }
        public DateTime crawl_time_start { get; set; }
        public DateTime crawl_time_end { get; set; }
        public DateTime adscan_time_start { get; set; }
        public DateTime adscan_time_end { get; set; }
        public DateTime LastActivity { get; set; }
        public string OffLimits { get; set; }
        public string AllowedDomains { get; set; }
        public string Additional_starturls { get; set; }
        public Int64 Files_Found { get; set; }
        public Int64 Files_Scanned { get; set; }
        public Int64 Files_Error { get; set; }
        public Int64 ADO_jobID { get; set; }
        public Int64 compliant { get; set; }
        public Int64 nonCompliant { get; set; }
        public Int64 untagged { get; set; }
        public Int64 offsiteFiles { get; set; }

        public string Code { get; set; }

        public string Company { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
        #endregion




        public List<ADscanCrawls> GetADscanList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string AlphaSearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ADSCAN_DB))
            {
                conn.Open();
                string myqry = "SELECT Id,clientID,status,starturl,Files_Found,Files_Scanned,Files_Error,'DBNAME' as databasename " +
                                " FROM DBNAME.adscan_crawls " +
                                " WHERE 1 ";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (clientID LIKE '%" + SearchKey + "%'" +
                        " OR starturl LIKE '%" + SearchKey + "%'" +
                        " OR adscan_crawls.Id LIKE '%" + SearchKey + "%'" +
                        " OR status LIKE '%" + SearchKey + "%'" +
                    ")";
                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " ORDER BY " + OrderBy;
                }
                else
                {
                    myqry3 = myqry3 + " ORDER BY ID DESC";

                }
               
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                string myqry_oth = myqry.Replace("DBNAME", Constants.ADSCAN_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ADSCAN_EU_DB);

                myqry = "select * from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " + myqry3;

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;

                var countQry = "SELECT COUNT(*) as TotalCount FROM DBNAME.adscan_crawls ";
                                
                string countQry_oth = countQry.Replace("DBNAME", Constants.ADSCAN_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ADSCAN_EU_DB);

                countQry = "select SUM(TotalCount) from ( " + countQry_oth + " UNION ALL " + countQry_eu + " ) as a ";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<ADscanCrawls>(dt);
                }
                else
                {
                    return Utility.ConvertDataTable<ADscanCrawls>(dt);
                }
            }
        }

    public List<ADscanCrawls> GetADscanCrawlsListByClientId(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adscan_crawls WHERE clientID = "+ID;
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
                    return Utility.ConvertDataTable<ADscanCrawls>(dt);
                else
                    return null;
            }
        }

        public ADscanCrawls GetADscanById(int Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adscan_crawls WHERE ID= " + Id;
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
                    return ConvertDataTable<ADscanCrawls>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string UpdateadcsanCrawl(int jobid,int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "UPDATE adscan_crawls SET ADO_jobID=" + jobid + " WHERE ID=" + ID;
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateCrawl(int id)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "UPDATE adscan_crawls SET status = 'start' WHERE ID=" + id ;
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                   
                    cmd.ExecuteNonQuery();
                    ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string DeleteCrawl(int id)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "DELETE FROM adscan_crawls WHERE ID=" + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string InsertUpdate()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                if (ID == 0)
                {
                    accesskey = abledoc.Utility.ComboHelper.RandomString();

                    string query = "INSERT INTO adscan_crawls(clientID,starturl,accesskey) VALUES(@clientID,@starturl,@accesskey)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE adscan_crawls SET clientID=@clientID,starturl=@starturl," +
                        "status=@status," +
                        "ScanType=@ScanType,accesskey=@accesskey,OffLimits=@OffLimits,AllowedDomains=@AllowedDomains," +
                        "Additional_starturls=@Additional_starturls,Files_Found=@Files_Found," +
                        "Files_Scanned=@Files_Scanned,Files_Error=@Files_Error,ADO_jobID=@ADO_jobID WHERE ID=@ID";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);
                    cmd.ExecuteNonQuery();

                    ExecutionString = "Update Succefully";
                }
                conn.Close();
            }
            return ExecutionString;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientID",
                DbType = DbType.String,
                Value = clientID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@status",
                DbType = DbType.String,
                Value = status,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@accesskey",
                DbType = DbType.String,
                Value = accesskey,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@starturl",
                DbType = DbType.String,
                Value = starturl,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ScanType",
                DbType = DbType.String,
                Value = ScanType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OffLimits",
                DbType = DbType.String,
                Value = OffLimits,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AllowedDomains",
                DbType = DbType.String,
                Value = AllowedDomains,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Additional_starturls",
                DbType = DbType.String,
                Value = Additional_starturls,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Files_Found",
                DbType = DbType.String,
                Value = Files_Found,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Files_Scanned",
                DbType = DbType.String,
                Value = Files_Scanned,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Files_Error",
                DbType = DbType.String,
                Value = Files_Error,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ADO_jobID",
                DbType = DbType.String,
                Value = ADO_jobID,
            });
            
        }
    }
}
