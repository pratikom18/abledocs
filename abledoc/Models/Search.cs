using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Search
    {
        #region "Properties"
        DataContext context = new DataContext();

        public int ID { get; set; }
        public int JobID { get; set; }
        public string EngagementNum { get; set; }
        public string EntityType { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public string Client { get; set; }
        public string Contact { get; set; }
        public string databasename { get; set; }
        #endregion


        public List<Search> GetSearchListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SearchStr,string filter, out int TotalRecord)
        {
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "Select ID,JOBID,Code,Country,ClientCreated,Company as Client,concat(FirstName, ' ',LastName) as Contact,FileName,Status,'File' as EntityType,";
                myqry += " (Select CONCAT(YEAR(ClientCreated), '-', Country, '-', Code, '-', jobs.ID)";
                myqry += " from DBNAME.clients";
                myqry += " RIGHT JOIN DBNAME.jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum,'DBNAME' as databasename ";
                myqry += " from DBNAME.jobs_files a";
                myqry += " LEFT JOIN(Select jobs.ID as JID, jobs.ClientID,clients.Country, clients.Code, clients.Company, clients.ClientCreated,clients_contacts.FirstName,clients_contacts.LastName, CONCAT(SUBSTRING_INDEX(clients.ClientCreated, '-', 1), '-', clients.Country, '-', clients.Code) as HalfEngagement";
                myqry += " from DBNAME.jobs";
                myqry += " LEFT JOIN DBNAME.clients ON jobs.ClientID = clients.ID";
                myqry += " LEFT JOIN DBNAME.clients_contacts ON jobs.ID = clients_contacts.ID ) b ON a.JobID = b.JID";
                myqry += " WHERE ";
                //if(SearchKey != "")
                //{
                //    SearchStr = SearchKey;
                //}
                if (filter == "ID")
                {
                    myqry += " a.ID Like '%" + SearchStr + "%'";
                }
                else if (filter == "JobID")
                {
                    myqry += " a.JobID Like '%" + SearchStr + "%'";
                }
                else if (filter == "Client")
                {
                    myqry += " b.Company Like '%" + SearchStr + "%'";
                }
                else if (filter == "Contact")
                {
                    myqry += " CONCAT(b.FirstName, ' ', b.LastName) Like '%" + SearchStr + "%'";
                }
                else if (filter == "EngagementNum")
                {
                    //  myqry += " a.EngagementNum Like '%" + SearchStr + "%'"; 

                    myqry += " CONCAT(YEAR(ClientCreated), '-', Country, '-', Code, '-', JOBID) Like '%" + SearchStr + "%'";

                }
                else if (filter == "Status")
                {

                    string mystatus = Regex.Replace(SearchStr.ToLower(), @"\s+", " ");

                    string strstatus = Utility.getSearchStatus(mystatus);


                    myqry += " lower(a.Status) Like '%" + strstatus + "%'";
                }
                else if (filter == "FileName")
                {
                    myqry += " a.Filename Like '%" + SearchStr + "%'";
                }
                else
                {
                    myqry = myqry + " (a.ID Like '%" + SearchStr + "%'" +
                        " OR a.JobID Like '%" + SearchStr + "%'" +
                        " OR a.Filename Like '%" + SearchStr + "%'" +
                        " OR b.Code Like '%" + SearchStr + "%'" +
                        " OR b.Company Like '%" + SearchStr + "%'" +
                        " OR CONCAT(YEAR(ClientCreated), '-', COALESCE(Country,''), '-', Code, '-', JOBID) Like '%" + SearchStr + "%'" +
                        " OR CONCAT(b.FirstName, ' ', b.LastName) Like '%" + SearchStr + "%'" +
                        " OR lower(a.Status) Like '%" + SearchStr + "%'" +
                    ")";
                }

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (a.ID Like '%" + SearchKey + "%'"+
                     " OR a.JobID Like '%" + SearchKey + "%'"+
                     " OR a.Filename Like '%" + SearchKey + "%'"+
                     " OR b.Code Like '%" + SearchKey + "%'"+
                     " OR b.Company Like '%" + SearchKey + "%'"+
                     " OR CONCAT(YEAR(ClientCreated), '-', COALESCE(Country,''), '-', Code, '-', JOBID) Like '%" + SearchKey + "%'"+
                     " OR CONCAT(b.FirstName, ' ', b.LastName) Like '%" + SearchKey + "%'" +
                      " OR lower(a.Status) Like '%" + SearchKey + "%'" +
                    ")";
                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }


                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

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
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry = "Select COUNT(*) as TotalCount ";
                countQry += " from DBNAME.jobs_files a";
                countQry += " LEFT JOIN(Select jobs.ID as JID, jobs.ClientID,clients.Country, clients.Code, clients.Company, clients.ClientCreated,clients_contacts.FirstName,clients_contacts.LastName, CONCAT(SUBSTRING_INDEX(clients.ClientCreated, '-', 1), '-', clients.Country, '-', clients.Code) as HalfEngagement";
                countQry += " from DBNAME.jobs";
                countQry += " LEFT JOIN DBNAME.clients ON jobs.ClientID = clients.ID";
                countQry += " LEFT JOIN DBNAME.clients_contacts ON jobs.ID = clients_contacts.ID ) b ON a.JobID = b.JID";
                countQry += " WHERE "; 

                if (filter == "ID")
                {
                    countQry += " a.ID Like '%" + SearchStr + "%'";
                }
                else if (filter == "JobID")
                {
                    countQry += " a.JobID Like '%" + SearchStr + "%'";
                }
                else if (filter == "Client")
                {
                    countQry += " b.Company Like '%" + SearchStr + "%'";
                }
                else if (filter == "Contact")
                {
                    countQry += " CONCAT(b.FirstName, ' ', b.LastName) Like '%" + SearchStr + "%'";
                }
                else if (filter == "EngagementNum")
                {
                    //  myqry += " a.EngagementNum Like '%" + SearchStr + "%'"; 

                    countQry += " CONCAT(YEAR(ClientCreated), '-', COALESCE(Country,''), '-', Code, '-', JOBID) Like '%" + SearchStr + "%'";

                }
                else if (filter == "Status")
                {

                    string mystatus = Regex.Replace(SearchStr.ToLower(), @"\s+", " ");

                    string strstatus = Utility.getSearchStatus(mystatus);


                    countQry += " lower(a.Status) Like '%" + strstatus + "%'";
                }
                else if (filter == "FileName")
                {
                    countQry += " a.Filename Like '%" + SearchStr + "%'";
                }
                else
                {
                    countQry = countQry + " (a.ID Like '%" + SearchStr + "%'" +
                        " OR a.JobID Like '%" + SearchStr + "%'" +
                        " OR a.Filename Like '%" + SearchStr + "%'" +
                        " OR b.Code Like '%" + SearchStr + "%'" +
                        " OR b.Company Like '%" + SearchStr + "%'" +
                        " OR CONCAT(YEAR(ClientCreated), '-', COALESCE(Country,''), '-', Code, '-', JOBID) Like '%" + SearchStr + "%'" +
                        " OR CONCAT(b.FirstName, ' ', b.LastName) Like '%" + SearchStr + "%'" +
                        " OR lower(a.Status) Like '%" + SearchStr + "%'" +
                    ")";
                }

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (a.ID Like '%" + SearchKey + "%'" +
                     " OR a.JobID Like '%" + SearchKey + "%'" +
                     " OR a.Filename Like '%" + SearchKey + "%'" +
                     " OR b.Code Like '%" + SearchKey + "%'" +
                     " OR b.Company Like '%" + SearchKey + "%'" +
                     " OR CONCAT(YEAR(ClientCreated), '-', COALESCE(Country,''), '-', Code, '-', JOBID) Like '%" + SearchKey + "%'" +
                     " OR CONCAT(b.FirstName, ' ', b.LastName) Like '%" + SearchStr + "%'" +
                      " OR lower(a.Status) Like '%" + SearchKey + "%'" +
                    ")";
                }

              
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(TotalCount) from ( " + countQry_oth + " UNION ALL " + countQry_eu + " ) as a ";

                //countQry += " WHERE a.ID Like '%" + SearchStr + "%'";
                //countQry += " OR a.JobID Like '%" + SearchStr + "%'";
                //countQry += " OR a.Filename Like '%" + SearchStr + "%'";
                //countQry += " OR b.Code Like '%" + SearchStr + "%'";
                //countQry += " OR b.Company Like '%" + SearchStr + "%'";
                //countQry += " OR b.HalfEngagement Like '%" + SearchStr + "%'";
                //countQry += " OR b.FirstName Like '%" + SearchStr + "%'";
                //countQry += " OR b.LastName Like '%" + SearchStr + "%'";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Search>(dt);
                }
                else
                {
                    return new List<Search>();
                }
            }
        }


        public string EngagementNumberJobID(int jobid,string databasename)
        {
		//Return string of engagement number based on jobID
            string engagementNumber = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select ClientCreated,Country,Code from clients RIGHT JOIN jobs ON jobs.ClientID=clients.ID Where jobs.ID = " + jobid;
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
                    engagementNumber = dt.Rows[0]["ClientCreated"] + "-"+dt.Rows[0]["Country"] + "-" + dt.Rows[0]["Code"] + "-"+ jobid;
                }

            }
            return engagementNumber;
        }
    }
}

