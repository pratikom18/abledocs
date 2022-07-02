using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;
//using MySqlConnector =  MySqlConnector;

namespace abledoc.Models
{
    public class JobsFiles
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public Int64 JobID { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public Int64 Pages { get; set; }
        public Int64 Priority { get; set; }
        public string PriorityManual { get; set; }
        public string Deadline { get; set; }
        public string MaximizedDeadline { get; set; }
        public string DeadlineTime { get; set; }
        public Int64 CurrentVersionFileID { get; set; }
        public Int64 CurrentPage { get; set; }
        public Int64 AssemblyPage { get; set; }
        public Int64 CurrentCheckout { get; set; }
        public string AssignedTo { get; set; }
        public string Language { get; set; }
        public string Filename { get; set; }
        public string ReferenceFileID { get; set; }
        public DateTime LastUpdated { get; set; }
        public Int64 FolderFlagged { get; set; }
        public Int64 AltTxt { get; set; }
        public string LastTagger { get; set; }
        public Int64 Fix { get; set; }
        public Int64 ReviewedAltTxt { get; set; }
        public string PricePer { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string Tax { get; set; }
        public string Description { get; set; }
        public string SourceFileTagging { get; set; }
        public string DaxsFlag { get; set; }
        public string FormsFlag { get; set; }
        public string LinkingFlag { get; set; }
        public string ReviewFlag { get; set; }
        public string Touches { get; set; }
        public string LastState { get; set; }
        public string LastStateTimer { get; set; }
        public Int64 DeliveryCount { get; set; }
        public Int64 Deleted { get; set; }
        public Int64 ReviewFix { get; set; }
        public Int64 FinalFix { get; set; }
        public DateTime SendToProduction { get; set; }
        public Int64 ALTCheckout { get; set; }
        public Int64 ALTCheckoutUser { get; set; }
        public Int64 OldPricePer { get; set; }
        public Int64 OldPrice { get; set; }
        public Int64 OldQuantity { get; set; }
        public Int64 OldTax { get; set; }
        public Int64 OldDescription { get; set; }
        public string EngagementNum { get; set; }
        public decimal PagesTotal { get; set; }
        public string HourValue { get; set; }
        public decimal totalQuantity { get; set; }
        public decimal totalPages { get; set; }
        public Int64 fileC { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
        public Int64 description_id { get; set; }
        #endregion

        // public string FileName { get; set; }

        public List<JobsFiles> GetPhase2List(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SearchStr, out int TotalRecord)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "SELECT jobs_files.ID, jobs_files.JobID, jobs_files.Filename, jobs_files.Status, jobs_files.Pages,CONCAT(jobs_files.Deadline,' ',jobs_files.DeadlineTime) as Deadline,jobs_files.DeadlineTime,jobs.JobType , jobs_files.CurrentPage, jobs_files.CurrentCheckout, jobs_files.ReviewFix, clients.Company,CONCAT(assign.FirstName,' ',assign.LastName) AS AssignedTo,checkout.FirstName AS CheckedOutTo ,  ";
                myqry += " (Select CONCAT(YEAR(ClientCreated),'-',COALESCE(Country,''),'-',Code,'-',jobs.ID) from DBNAME.clients ";
                myqry += " RIGHT JOIN DBNAME.jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum,'DBNAME' AS databasename,jobs_files.CurrentVersionFileID ";
                myqry += " FROM DBNAME.jobs_files  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout ";
                myqry += " WHERE 1 ";
                myqry += " AND jobs_files.Deleted = 0 ";
                myqry += " AND jobs_files.Status = 'REVIEW' ";
                myqry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs_files.ID Like '%" + SearchKey + "%'  OR jobs_files.JobID Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%')";
                }
                //myqry += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 += " ORDER BY Deadline desc, DeadlineTime, FIELD(JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";
                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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

                string countQry = "SELECT COUNT(*) as TotalCount";
                countQry += " FROM DBNAME.jobs_files  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout ";
                countQry += " WHERE 1 ";
                countQry += " AND jobs_files.Deleted = 0 ";
                countQry += " AND jobs_files.Status = 'REVIEW' ";
                countQry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs_files.ID Like '%" + SearchKey + "%'  OR jobs_files.JobID Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%')";
                }

          
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(TotalCount) from ( " + myqry_oth1 + " UNION ALL " + myqry_eu1 + " ) as a " ;

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                }
                else
                {
                    return new List<JobsFiles>();
                }
            }
        }

        public List<JobsFiles> GetPhase1List(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SearchStr, out int TotalRecord, int userId)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string myqry = "SELECT jobs_files.ID, jobs_files.JobID, jobs_files.Filename, jobs_files.Status, jobs_files.Pages, CONCAT(jobs_files.Deadline,' ',jobs_files.DeadlineTime) as Deadline, jobs_files.CurrentPage, jobs_files.Fix, jobs_files.CurrentCheckout, clients.Company, CONCAT(assign.FirstName,' ',assign.LastName) AS AssignedTo, checkout.FirstName AS CheckedOutTo, ";
                myqry += " (Select CONCAT(YEAR(ClientCreated),'-',COALESCE(Country,''),'-',Code,'-',jobs.ID) from DBNAME.clients ";
                myqry += " RIGHT JOIN DBNAME.jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum,'DBNAME' AS databasename,jobs_files.FinalFix,jobs_files.DeadlineTime,jobs.JobType,jobs_files.CurrentVersionFileID ";
                myqry += " FROM DBNAME.jobs_files LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID  ";
                myqry += " LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout ";
                myqry += " WHERE 1 ";
                myqry += " AND jobs_files.Status = 'TAGGING' ";
                myqry += " AND jobs_files.Deleted = 0 AND jobs_files.AssignedTo = " + userId + " ";
                myqry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%'  OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%')";
                }
                //myqry += " ORDER BY jobs_files.FinalFix Desc, jobs_files.Fix Desc, jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 += " ORDER BY FinalFix Desc, Fix Desc, Deadline desc, DeadlineTime, FIELD(JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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

                string countQry = "SELECT COUNT(*) as total";
                countQry += " FROM DBNAME.jobs_files LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID  ";
                countQry += " LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout ";
                countQry += " WHERE 1 ";
                countQry += " AND jobs_files.Status = 'TAGGING' ";
                countQry += " AND jobs_files.Deleted = 0 AND jobs_files.AssignedTo = " + userId + " ";
                countQry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%'  OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";
                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%')";
                }

        
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(total) from ( " + countQry_oth + " UNION ALL " + countQry_eu + " ) as a ";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                }
                else
                {
                    return new List<JobsFiles>();
                }
            }
        }

        public List<JobsFiles> GetalttextList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SearchStr, out int TotalRecord)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "SELECT jobs_files.ID, jobs_files.Filename, jobs_files.Pages,jobs_files.DeadlineTime, CONCAT(jobs_files.Deadline,' ',jobs_files.DeadlineTime) as Deadline, jobs_files.AltTxt, jobs_files.JobID, jobs.JobType, ";
                myqry += " (Select CONCAT(YEAR(ClientCreated),'-',COALESCE(Country,''),'-',Code,'-',jobs.ID) from DBNAME.clients ";
                myqry += " RIGHT JOIN DBNAME.jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum,'DBNAME' AS databasename,jobs_files.CurrentVersionFileID ";
                myqry += "  FROM DBNAME.jobs_files";
                myqry += "  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID";
                myqry += " LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID";
                myqry += " WHERE 1";
                myqry += " AND jobs.Status = 'OPEN'";
                myqry += " AND jobs_files.Status <> 'CLOSED'";
                myqry += " AND jobs_files.Status <> 'PENDING'";
                myqry += " AND (jobs_files.AltTxt IS NULL OR jobs_files.AltTxt = 0 OR jobs_files.AltTxt = 2 OR jobs_files.AltTxt = 6)";
                myqry += " AND jobs_files.FolderFlagged <> 2";
                myqry += " AND jobs_files.ALTCheckout = 0";
                myqry += " AND jobs_files.Deleted = 0";
                myqry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%' OR CONCAT(YEAR(ClientCreated), '-', COALESCE(clients.Country,''), '-', Code, '-', jobs.ID) like '%" + SearchStr + "%')";


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated), '-', COALESCE(clients.Country,''), '-', Code, '-', jobs.ID) like '%" + SearchKey + "%')";
                }
                //myqry += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 += " ORDER BY Deadline desc, DeadlineTime, FIELD(JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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
               
                string countQry = "SELECT count(*) as TotalCount";
                countQry += "  FROM DBNAME.jobs_files";
                countQry += "  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID";
                countQry += " LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID";
                countQry += " WHERE 1";
                countQry += " AND jobs.Status = 'OPEN'";
                countQry += " AND jobs_files.Status <> 'CLOSED'";
                countQry += " AND jobs_files.Status <> 'PENDING'";
                countQry += " AND(jobs_files.AltTxt IS NULL OR jobs_files.AltTxt = 0 OR jobs_files.AltTxt = 2 OR jobs_files.AltTxt = 6)";
                countQry += " AND jobs_files.FolderFlagged <> 2";
                countQry += " AND jobs_files.ALTCheckout = 0";
                countQry += " AND jobs_files.Deleted = 0";
                countQry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%' OR CONCAT(YEAR(ClientCreated), '-', COALESCE(clients.Country,''), '-', Code, '-', jobs.ID) like '%" + SearchStr + "%')";


                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated), '-', COALESCE(clients.Country,''), '-', Code, '-', jobs.ID) like '%" + SearchKey + "%')";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(TotalCount) from ( " + myqry_oth1 + " UNION ALL " + myqry_eu1 + " ) as a ";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                }
                else
                {
                    return new List<JobsFiles>();
                }
            }
        }

        public List<JobsFiles> GetPhase3List(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SearchStr, out int TotalRecord)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "SELECT jobs_files.ID, jobs_files.JobID, jobs_files.Filename, jobs_files.Status,jobs_files.DeadlineTime,jobs.JobType, jobs_files.Pages, CONCAT(jobs_files.Deadline,' ',jobs_files.DeadlineTime) as Deadline, jobs_files.CurrentPage, jobs_files.CurrentCheckout";
                myqry += "  , clients.Company,";
                myqry += " CONCAT(assign.FirstName,' ',assign.LastName) AS AssignedTo,";
                myqry += " checkout.FirstName AS CheckedOutTo,";
                myqry += " (Select CONCAT(YEAR(ClientCreated),'-',COALESCE(Country,''),'-',Code,'-',jobs.ID) from DBNAME.clients ";
                myqry += " RIGHT JOIN DBNAME.jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum,'DBNAME' AS databasename,jobs_files.CurrentVersionFileID ";
                myqry += "  FROM DBNAME.jobs_files";
                myqry += "  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID";
                myqry += " LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID";
                myqry += " LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo";
                myqry += "  LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout";
                myqry += "  WHERE 1";
                myqry += " AND jobs_files.Deleted = 0 AND jobs_files.Status = 'QC' ";
                myqry += " AND (jobs_files.ID Like '%" + SearchStr + "%'  OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' " +
                        "OR CONCAT(YEAR(ClientCreated), '-', COALESCE(clients.Country,''), '-', Code, '-', jobs.ID) like '%" + SearchKey + "%'" +
                        "OR jobs_files.Filename Like '%" + SearchKey + "%')";
                }
                //myqry += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 += " ORDER BY Deadline desc, DeadlineTime, FIELD(JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                }

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }


                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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
               
                string countQry = "SELECT count(*) as TotalCount";
                countQry += "  FROM DBNAME.jobs_files";
                countQry += "  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID";
                countQry += " LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID";
                countQry += " LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo";
                countQry += "  LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout";
                countQry += "  WHERE 1";
                countQry += " AND jobs_files.Deleted = 0 AND jobs_files.Status = 'QC' ";
                countQry += " AND (jobs_files.ID Like '%" + SearchStr + "%'  OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";
                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' " +
                        "OR CONCAT(YEAR(ClientCreated), '-', COALESCE(clients.Country,''), '-', Code, '-', jobs.ID) like '%" + SearchKey + "%'" +
                        "OR jobs_files.Filename Like '%" + SearchKey + "%')";
                }

                   countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(TotalCount) from ( " + myqry_oth1 + " UNION ALL " + myqry_eu1 + " ) as a ";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                }
                else
                {
                    return new List<JobsFiles>();
                }
            }
        }

        public List<JobsFiles> GetJobFilesListByJobID(int JobID, int all = 0)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "";
                if (all == 1)
                {
                    qry = "SELECT * FROM jobs_files WHERE JobID=@JobID";
                }
                else
                {
                    qry = "SELECT * FROM jobs_files WHERE JobID=@JobID AND Deleted=0 Order By Filename";
                }
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
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                else
                    return null;
            }
        }
        public List<JobsFiles> GetFilesListByJobID(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, Status, Deadline, DeadlineTime, AssignedTo, Filename, Pages FROM jobs_files WHERE JobID = @JobID AND Deleted=0 ORDER BY Deadline ASC";
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

                return Utility.ConvertDataTable<JobsFiles>(dt);
            }
        }

        public JobsFiles GetJobFilesByID(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * from jobs_files Where ID=@ID LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
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
                    return ConvertDataTable<JobsFiles>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        //sarju 11-01-21 |GetFinalSearchListForTable

        public List<JobsFiles> GetFinalSearchListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string SearchStr, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "SELECT jobs_files.ID, jobs_files.JobID, jobs_files.Filename, jobs_files.Status, jobs_files.Pages, CONCAT(jobs_files.Deadline,' ',jobs_files.DeadlineTime) as Deadline, jobs_files.CurrentPage, jobs_files.AltTxt, jobs_files.CurrentCheckout, jobs_files.FinalFix ,clients.Company,";
                myqry += " assign.FirstName AS AssignedTo,checkout.FirstName AS CheckedOutTo,";
                myqry += " (Select CONCAT(YEAR(ClientCreated),'-',COALESCE(Country,''),'-',Code,'-',jobs.ID) from clients ";
                myqry += " RIGHT JOIN jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum";
                myqry += " FROM jobs_files";
                myqry += " LEFT JOIN jobs ON jobs.ID = jobs_files.JobID";
                myqry += " LEFT JOIN clients ON clients.ID = jobs.ClientID";
                myqry += " LEFT JOIN users AS assign ON assign.ID = jobs_files.AssignedTo";
                myqry += " LEFT JOIN users AS checkout ON checkout.ID = jobs_files.CurrentCheckout";
                myqry += " WHERE 1";
                myqry += " AND jobs_files.Deleted = 0 AND jobs_files.Status = 'FINAL' ";
                myqry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%'  )";
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchKey + "%')";
                }
                //myqry += " ORDER BY jobs_files.FinalFix Desc, jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";


                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {
                    myqry += " ORDER BY jobs_files.FinalFix Desc, jobs_files.Deadline desc, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

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
                //MySqlCommand cnt = new MySqlCommand("SELECT COUNT(*) FROM jobs_files  LEFT JOIN jobs ON jobs.ID = jobs_files.JobID  LEFT JOIN clients ON clients.ID = jobs.ClientID LEFT JOIN users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN users AS checkout ON checkout.ID = jobs_files.CurrentCheckout  WHERE 1 AND jobs_files.Deleted = 0 AND jobs_files.Status = 'FINAL' AND (jobs_files.ID Like '%%' OR jobs_files.JobID Like '%%' OR jobs_files.Filename Like '%%') ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI');", conn);
                ////string countQry = "Select COUNT(*) from jobs_files a";
                //string countQry = "SELECT jobs_files.ID, jobs_files.JobID, jobs_files.Filename, jobs_files.Status, jobs_files.Pages, jobs_files.Deadline, jobs_files.DeadlineTime, jobs_files.CurrentPage, jobs_files.CurrentCheckout, jobs_files.ReviewFix, clients.Company,assign.FirstName AS AssignedTo,checkout.FirstName AS CheckedOutTo FROM jobs_files LEFT JOIN jobs ON jobs.ID = jobs_files.JobID LEFT JOIN clients ON clients.ID = jobs.ClientID LEFT JOIN users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN users AS checkout ON checkout.ID = jobs_files.CurrentCheckout ";
                //countQry += " WHERE 1 AND jobs_files.Deleted = 0 AND jobs_files.Status = 'FINAL' ";
                //countQry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%')";
                //countQry += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";



                string countQry = " SELECT count(*)";
                countQry += " FROM jobs_files";
                countQry += " LEFT JOIN jobs ON jobs.ID = jobs_files.JobID";
                countQry += " LEFT JOIN clients ON clients.ID = jobs.ClientID";
                countQry += " LEFT JOIN users AS assign ON assign.ID = jobs_files.AssignedTo";
                countQry += " LEFT JOIN users AS checkout ON checkout.ID = jobs_files.CurrentCheckout";
                countQry += " WHERE 1";
                countQry += " AND jobs_files.Deleted = 0 AND jobs_files.Status = 'FINAL' ";
                countQry += " AND (jobs_files.ID Like '%" + SearchStr + "%' OR jobs_files.JobID Like '%" + SearchStr + "%' OR jobs_files.Filename Like '%" + SearchStr + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchStr + "%'  )";

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs_files.ID Like '%" + SearchKey + "%' OR jobs_files.JobID Like '%" + SearchKey + "%' OR jobs_files.Filename Like '%" + SearchKey + "%' OR CONCAT(YEAR(ClientCreated),'-',COALESCE(clients.Country,''),'-',Code,'-',jobs.ID) like '%" + SearchKey + "%')";
                }

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());



                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                }
                else
                {
                    return new List<JobsFiles>();
                }



            }
        }

        public List<JobsFiles> GetJobFileListIDByJobID(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            List<int> JobID = new List<int>();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT DISTINCT(jobs_files.ID) AS ID " +
                            "FROM jobs_files INNER JOIN jobs_files_versions ON jobs_files.ID = jobs_files_versions.FileID " +
                            "WHERE jobs_files.JobID = " + id + " AND jobs_files.Deleted = 0 AND jobs_files_versions.Deleted = 0 ORDER BY Deadline";
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
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                else
                    return null;
            }
        }

        public List<JobsFiles> GetJobfilesFileTree(int[] id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            List<int> JobID = new List<int>();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,Filename, Deadline, STATUS, DeliveryCount, Pages FROM jobs_files WHERE ID IN (" + String.Join(",", id) + ") AND Deleted=0";
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
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                else
                    return null;
            }
        }


        public List<JobsFiles> GetJobfilesFileTreePaging(int[] id, int inStartIndex, int inEndIndex, out int TotalRecord)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            List<int> JobID = new List<int>();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,Filename, Deadline, STATUS, DeliveryCount, Pages FROM jobs_files WHERE ID IN (" + String.Join(",", id) + ") AND Deleted=0";

                if (inEndIndex != -1)
                {
                    qry = qry + " LIMIT @end OFFSET @start";
                }
                MySqlCommand cmd = new MySqlCommand(qry, conn);

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
                //string countQry = "Select COUNT(*) from jobs_files a";
                string countQry = "SELECT COUNT(*) FROM jobs_files WHERE ID IN (" + String.Join(",", id) + ") AND Deleted=0";


                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                }
                else
                {
                    TotalRecord = 0;
                    return null;
                }

            }
        }

        public string UpdateByQuote()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET Description = @Description, Quantity = @Quantity, PricePer = @PricePer, Price = @Price, FolderFlagged = 2 WHERE JobID = @JobID and ID =@ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string InsertByQuote()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "INSERT INTO jobs_files(JobID, Description, Quantity, PricePer, Price, Filename, FolderFlagged)" +
                    " VALUES (@JobID, @Description, @Quantity, @PricePer, @Price, @Filename,2)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string FileDeleteQuote(string AllFileDeleteQuote = "")
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "";
                if (AllFileDeleteQuote == "")
                {
                    query = "UPDATE jobs_files SET Deleted = 1 WHERE ID = @ID";
                }
                else
                {
                    query = "UPDATE jobs_files SET Deleted = 1 WHERE  JobID = @JobID";
                }


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string JobPanelUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET PricePer = @PricePer, Price = @Price, Quantity = @Quantity, Tax = @Tax, Description = @Description, OldPricePer = @OldPricePer, OldPrice = @OldPrice, OldQuantity = @OldQuantity, OldTax = @OldTax, OldDescription = @OldDescription,description_id=@description_id  WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string JobPanelOldPriceUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update jobs_files Set OldPricePer=@OldPricePer, OldPrice=@OldPrice, OldQuantity=@OldQuantity, OldTax=@OldTax, OldDescription=@OldDescription Where ID=@ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string DefaultPriceUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET PricePer = @PricePer, Price = @Price,Quantity=@Quantity WHERE JobID = @JobID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string DefaultDocUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET Quantity = 1 WHERE JobID = @JobID";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



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
                ParameterName = "@Quantity",
                DbType = DbType.Decimal,
                Value = Quantity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Description",
                DbType = DbType.String,
                Value = Description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PricePer",
                DbType = DbType.String,
                Value = PricePer,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Price",
                DbType = DbType.Decimal,
                Value = Price,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Filename",
                DbType = DbType.String,
                Value = Filename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Tax",
                DbType = DbType.String,
                Value = Tax,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldPricePer",
                DbType = DbType.Int32,
                Value = OldPricePer,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldPrice",
                DbType = DbType.Int32,
                Value = OldPrice,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldQuantity",
                DbType = DbType.Int32,
                Value = OldQuantity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldDescription",
                DbType = DbType.Int32,
                Value = OldDescription,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldTax",
                DbType = DbType.Int32,
                Value = OldTax,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Deadline",
                DbType = DbType.String,
                Value = Deadline,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description_id",
                DbType = DbType.Int32,
                Value = description_id,
            });
            

        }
        public string UpdateCurrentCheckout()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET CurrentCheckout = @CurrentCheckout WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindIdJobFiles(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindIdJobFiles(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CurrentCheckout",
                DbType = DbType.Int32,
                Value = CurrentCheckout,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });
        }
        public string UpdateStatus()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET Status ='" + Status + "'  WHERE ID = " + ID;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateFilePosition(string currentStatus, string OldStatus, long OldPriority, int ID, string OldDeadline)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            string msg = "";
            JobsFiles objJobsFiles = new JobsFiles();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT Priority, Deadline FROM jobs_files WHERE Status = @Status AND STR_TO_DATE(Deadline, '%Y-%m-%d') <= STR_TO_DATE(@Deadline, '%Y-%m-%d') AND PriorityManual IS NULL " +
                                    "ORDER BY -STR_TO_DATE(Deadline, '%Y-%m-%d'), Priority DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = currentStatus,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Deadline",
                    DbType = DbType.String,
                    Value = OldDeadline,
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
                {
                    objJobsFiles = ConvertDataTable<JobsFiles>(dt.Rows[0].Table).FirstOrDefault();
                    objJobsFiles.Priority = objJobsFiles.Priority + 1;
                }


                if (objJobsFiles.Priority.ToString() != "")
                {
                    msg = UpdateJobFilesPriorityByStatus(currentStatus, objJobsFiles.Priority, "New");
                    if (msg != "exit")
                    {
                        msg = UpdateJobFilesPriorityStatusByID(currentStatus, objJobsFiles.Priority, ID);
                    }

                }
                else if (objJobsFiles.Priority.ToString() == "")
                {
                    int MaxPriority = GetMaxPriorityByStatus(currentStatus);
                    msg = UpdateJobFilesPriorityStatusByID(currentStatus, MaxPriority, ID);
                }

                msg = UpdateJobFilesPriorityByStatus(OldStatus, OldPriority, "Old");
            }
            return msg;
        }

        public string UpdateJobFilesPriorityByStatus(string newStatus, long positionPriority, string flag)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = string.Empty;
                    if (flag == "New")
                    {
                        query = "UPDATE jobs_files SET Priority = Priority+1 WHERE Status = @Status AND Priority >= @LastPriority";
                    }
                    else if (flag == "Old")
                    {
                        query = "UPDATE jobs_files SET Priority = Priority-1 WHERE Status = @Status AND Priority > @LastPriority";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@Status",
                        DbType = DbType.String,
                        Value = newStatus,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@LastPriority",
                        DbType = DbType.Int32,
                        Value = positionPriority,
                    });

                    cmd.ExecuteNonQuery();
                    int LastID = ID;

                    ExecutionString = LastID.ToString();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                ExecutionString = "exit";
            }

            return ExecutionString;
        }

        public string UpdateJobFilesPriorityStatusByID(string newStatus, long positionPriority, long ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string PriorityManual = null;
                    string query = "UPDATE jobs_files SET Status = @Status, Priority=@Priority, PriorityManual=@PriorityManual WHERE ID = @ID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@Status",
                        DbType = DbType.String,
                        Value = newStatus,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@Priority",
                        DbType = DbType.Int32,
                        Value = positionPriority,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@PriorityManual",
                        DbType = DbType.String,
                        Value = PriorityManual,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@ID",
                        DbType = DbType.Int32,
                        Value = ID,
                    });

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return ExecutionString;
        }

        public int GetMaxPriorityByStatus(string newStatus)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            int maxPriority = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT MAX(Priority) as maxPriority FROM jobs_files WHERE Status = @Status";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = Status,
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
                {
                    maxPriority = abledoc.Utility.CommonHelper.GetDBInt(dt.Rows[0]["maxPriority"]);
                    maxPriority = maxPriority + 1;
                }
            }
            return maxPriority;
        }

        public string UpdateJobFilesByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                LastTagger = AssignedTo;
                string query = "UPDATE jobs_files SET " +
                                "Status = @Status, AssemblyPage =@AssemblyPage, CurrentPage=@CurrentPage, Pages=@Pages, Deadline=@Deadline, DeadlineTime=@DeadlineTime, AssignedTo=@AssignedTo, Language=@Language, Filename=@Filename, AltTxt=@AltTxt ";
                if (Status == "TAGGING")
                {
                    if (AssignedTo != "")
                    {
                        query += ", LastTagger= @LastTagger ";

                    }
                }
                query += ", LastUpdated = NOW() WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindIdJobFilesUpdate(cmd);
                BindParamJobFilesUpdate(cmd);
                if (Status == "TAGGING")
                {
                    if (AssignedTo != "")
                    {
                        cmd.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@LastTagger",
                            DbType = DbType.String,
                            Value = LastTagger,
                        });

                    }
                }

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobFilesBySendToProduction()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            var currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                
                string query = "UPDATE jobs_files SET " +
                    "SendToProduction = CASE WHEN SendToProduction IS NULL THEN '" + currentTimestamp + "' ELSE SendToProduction END, " +
                    "Status = 'TAGGING', AssignedTo = @AssignedTo, LastTagger = @LastTagger" +
                    " WHERE ID = @ID";
                

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@LastTagger",
                    DbType = DbType.String,
                    Value = LastTagger,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AssignedTo",
                    DbType = DbType.String,
                    Value = AssignedTo,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobFilesBySourceFile(string currentJobStatus, string currentAssignedTo, JobsFiles item)
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                var currentFileStatus = item.Status;
                var currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (currentJobStatus == "OPEN")
                {

                    // If current file status is not tagging and is pending then update it to tagging
                    if (currentFileStatus == "PENDING")
                    {
                        string query = "UPDATE jobs_files SET SendToProduction = CASE WHEN SendToProduction IS NULL THEN '" + currentTimestamp + "' ELSE SendToProduction END, Status='TAGGING' WHERE ID = " + item.ID;
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        cmd.ExecuteNonQuery();
                    }

                    // If the file is not assigned to anyone and current assigned to value on job is not null
                    if (item.AssignedTo == "" || item.AssignedTo == null)
                    {
                        if (currentAssignedTo != "" && currentAssignedTo != null)
                        {
                            // Update this file Assigned to value
                            string query = "UPDATE jobs_files SET AssignedTo='" + currentAssignedTo + "' WHERE ID = " + item.ID + "";
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }


                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobFilesBySourceFileJobID(string currentJobStatus, string currentAssignedTo, int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                var currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (currentJobStatus == "OPEN")
                {

                    // If current file status is not tagging and is pending then update it to tagging

                    string query = "UPDATE jobs_files jf inner join jobs j on jf.jobid = j.id SET SendToProduction = CASE WHEN SendToProduction IS NULL THEN '" + currentTimestamp + "' ELSE SendToProduction END, jf.Status = 'TAGGING' WHERE jobid = " + JobID + " and j.status = 'OPEN' and jf.status = 'PENDING'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();

                    // If the file is not assigned to anyone and current assigned to value on job is not null.

                    string query2 = "UPDATE jobs_files jf inner join jobs j on jf.jobid = j.id SET jf.AssignedTo = '" + currentAssignedTo + "' WHERE jobid = " + JobID + " and j.status = 'OPEN' AND IFNULL(jf.AssignedTo,0)= 0";
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);

                    cmd2.ExecuteNonQuery();
                }


                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindIdJobFilesUpdate(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });
        }
        private void BindParamJobFilesUpdate(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Status",
                DbType = DbType.String,
                Value = Status,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AssemblyPage",
                DbType = DbType.Int32,
                Value = AssemblyPage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CurrentPage",
                DbType = DbType.Int32,
                Value = CurrentPage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Pages",
                DbType = DbType.Int32,
                Value = Pages,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Deadline",
                DbType = DbType.String,
                Value = Deadline,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DeadlineTime",
                DbType = DbType.String,
                Value = DeadlineTime,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AssignedTo",
                DbType = DbType.String,
                Value = AssignedTo,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Language",
                DbType = DbType.String,
                Value = Language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Filename",
                DbType = DbType.String,
                Value = Filename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AltTxt",
                DbType = DbType.Int32,
                Value = AltTxt,
            });

        }

        public List<JobsFiles> GetJobFileListIDByID(int id, string flag)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            List<int> JobID = new List<int>();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT JobID, Status, Deadline, DeliveryCount from jobs_files WHERE JobID=(SELECT JobID from jobs_files WHERE ID=" + id + ")";

                if (flag != "Delete")
                {
                    qry += " AND Deleted=0";
                }

                if (flag != "OrderBy")
                {
                    qry += " ORDER BY Deadline";
                }
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
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                else
                    return null;
            }
        }

        public Dictionary<string, int> PhasesCount(int userId)
        {
            Dictionary<string, int> PhasesCountList = new Dictionary<string, int>();
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(databasename))
            {
                conn.Open();

                int count1 = 0;
                string Where_Status = string.Empty;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);

                Where_Status = " AND f.Status = 'TAGGING'";

                Where_Status = Where_Status + " AND f.AssignedTo = " + userId + " ";

                string countQry1 = "SELECT COUNT(*) " +
                    " FROM jobs_files as f JOIN jobs as j ON f.JobID = j.ID JOIN clients as c ON j.ClientID = c.ID Where f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN' " + Where_Status;

               
                MySqlCommand cou1 = new MySqlCommand(countQry1, conn);
                count1 = Convert.ToInt32(cou1.ExecuteScalar());
                PhasesCountList.Add("Phase1", count1);

                int count2 = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry2 = "SELECT COUNT(*) as TotalCount FROM DBNAME.jobs_files LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout ";
                countQry2 += " WHERE 1 AND jobs_files.Deleted = 0 AND jobs_files.Status = 'REVIEW' ";
                //countQry2 += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                countQry2 = countQry2.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = countQry2.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = countQry2.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry2 = "select SUM(TotalCount) from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a ";


                MySqlCommand cou2 = new MySqlCommand(countQry2, conn);
                count2 = Convert.ToInt32(cou2.ExecuteScalar());
                PhasesCountList.Add("Phase2", count2);

                int count5 = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry5 = "SELECT COUNT(*) as TotalCount";
                countQry5 += "  FROM DBNAME.jobs_files";
                countQry5 += "  LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID";
                countQry5 += " LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID";
                countQry5 += " LEFT JOIN COMMONDBNAME.users AS assign ON assign.ID = jobs_files.AssignedTo";
                countQry5 += "  LEFT JOIN COMMONDBNAME.users AS checkout ON checkout.ID = jobs_files.CurrentCheckout";
                countQry5 += "  WHERE 1";
                countQry5 += " AND jobs_files.Deleted = 0 AND jobs_files.Status = 'QC' ";
                // countQry5 += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                countQry5 = countQry5.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth1 = countQry5.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu1 = countQry5.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry5 = "select SUM(TotalCount) from ( " + myqry_oth1 + " UNION ALL " + myqry_eu1 + " ) as a ";


                MySqlCommand cou5 = new MySqlCommand(countQry5, conn);
                count5 = Convert.ToInt32(cou5.ExecuteScalar());
                PhasesCountList.Add("Phase3", count5);

                int count3 = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                string countQry3 = "SELECT COUNT(*) as TotalCount";
                countQry3 += " FROM DBNAME.jobs_files";
                countQry3 += " LEFT JOIN DBNAME.jobs ON jobs.ID = jobs_files.JobID";
                countQry3 += " WHERE 1";
                countQry3 += " AND jobs.Status = 'OPEN'";
                countQry3 += " AND jobs_files.Status <> 'CLOSED'";
                countQry3 += " AND jobs_files.Status <> 'PENDING'";
                countQry3 += " AND(jobs_files.AltTxt IS NULL OR jobs_files.AltTxt = 0 OR jobs_files.AltTxt = 2 OR jobs_files.AltTxt = 6)";
                countQry3 += " AND jobs_files.FolderFlagged <> 2";
                countQry3 += " AND jobs_files.ALTCheckout = 0";
                countQry3 += " AND jobs_files.Deleted = 0";
                //countQry3 += " ORDER BY jobs_files.Deadline, jobs_files.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                countQry3 = countQry3.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth2 = countQry3.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu2 = countQry3.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry3 = "select SUM(TotalCount) from ( " + myqry_oth2 + " UNION ALL " + myqry_eu2 + " ) as a ";


                MySqlCommand cou3 = new MySqlCommand(countQry3, conn);
                count3 = Convert.ToInt32(cou3.ExecuteScalar());
                PhasesCountList.Add("alttext", count3);

            }

            return PhasesCountList;
        }

        public string JobFileDeadlineUpdateByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET Deadline = @Deadline WHERE JobID = @JobID and (Deadline is null OR Deadline = '')";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);


                cmd.ExecuteNonQuery();

                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public string Update()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "UPDATE jobs_files SET CurrentVersionFileID=@CurrentVersionFileID WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CurrentVersionFileID",
                    DbType = DbType.Int32,
                    Value = CurrentVersionFileID,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public int InsertFilesByJob()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            int ExecutionString = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                DeadlineTime = DateTime.Now.ToString("HH:mm");
                conn.Open();
                string query = "INSERT INTO jobs_files(JobID, Filename,DeadlineTime) Values(@JobID, @Filename,@DeadlineTime)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Filename",
                    DbType = DbType.String,
                    Value = Filename,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@DeadlineTime",
                    DbType = DbType.String,
                    Value = DeadlineTime,
                });
                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID;





                conn.Close();
            }
            return ExecutionString;
        }

        public int InsertFilesByJobCopy(string created)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            int ExecutionString = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "Insert Into jobs_files(JobID, Created, Pages, Filename, Deleted) Values(@JobID,'" + created + "',@Pages,@Filename,@Deleted)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Filename",
                    DbType = DbType.String,
                    Value = Filename,
                });
                //cmd.Parameters.Add(new MySqlParameter
                //{
                //    ParameterName = "@Created",
                //    DbType = DbType.DateTime,
                //    Value = Created,
                //});
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Pages",
                    DbType = DbType.Int64,
                    Value = Pages,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Deleted",
                    DbType = DbType.Int64,
                    Value = Deleted,
                });

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID;





                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdatePages()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "UPDATE jobs_files SET Pages=@Pages WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Pages",
                    DbType = DbType.Int32,
                    Value = Pages,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public decimal GetJobFilesPagesTotalByJobID(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            decimal total = 0;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT SUM(Pages) AS PagesTotal FROM jobs_files WHERE JobID = @JobID AND Deleted=0";
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
                {
                    JobsFiles u = ConvertDataTable<JobsFiles>(dt.Rows[0].Table).FirstOrDefault();
                    total = u.PagesTotal;
                }


            }
            return total;

        }

        public string UpdateALTCheckout()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files SET ALTCheckout = @ALTCheckout, ALTCheckoutUser = @ALTCheckoutUser WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ALTCheckout",
                    DbType = DbType.Int32,
                    Value = ALTCheckout,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ALTCheckoutUser",
                    DbType = DbType.Int32,
                    Value = ALTCheckoutUser,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdatePanelSave()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files SET Deadline = @Deadline, DeadlineTime = @DeadlineTime, Pages = @Pages, PricePer = @PricePer, Price = @Price, Quantity = @Quantity WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Deadline",
                    DbType = DbType.String,
                    Value = Deadline,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@DeadlineTime",
                    DbType = DbType.String,
                    Value = DeadlineTime,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Pages",
                    DbType = DbType.Int64,
                    Value = Pages,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@PricePer",
                    DbType = DbType.String,
                    Value = PricePer,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Price",
                    DbType = DbType.Decimal,
                    Value = Price,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Quantity",
                    DbType = DbType.Decimal,
                    Value = Quantity,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateAltText()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files SET AltTxt = @AltTxt WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AltTxt",
                    DbType = DbType.Int32,
                    Value = AltTxt,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateDeadlineDateByJobID(string olddate = "")
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs_files SET Deadline = @Deadline WHERE JobID = @JobID";
                if(olddate != "")
                {
                    query = query + " and Deadline='" + olddate + "'";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);


                cmd.ExecuteNonQuery();

                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateAltTextCheckOut()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {

                conn.Open();
                string query = "UPDATE jobs_files SET AltTxt=@AltTxt, ALTCheckout=0, ALTCheckoutUser=0 WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AltTxt",
                    DbType = DbType.Int32,
                    Value = AltTxt,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public JobsFiles GetJobIdQuotedAsValue(int jobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT COUNT(ID) as fileC, SUM(Pages) as totalPages, Sum(Quantity) as totalQuantity, PricePer from jobs_files where Status<>'REFERENCE' AND Deleted=0 AND JobID=@jobID";


                MySqlCommand cmd = new MySqlCommand(qry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@jobID",
                    DbType = DbType.Int32,
                    Value = jobID,
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
                    //return Utility.ConvertDataTable<JobsFiles>(dt).FirstOrDefault();
                    return Utility.ConvertDataTable<JobsFiles>(dt).FirstOrDefault();
                else
                    return null;
            }
        }

        public JobsFiles GetMaximumDeadlineDateByJob(int jobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT MAX(Deadline) as MaximizedDeadline from jobs_files WHERE JobID=@jobID";


                MySqlCommand cmd = new MySqlCommand(qry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@jobID",
                    DbType = DbType.Int32,
                    Value = jobID,
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
                    //return Utility.ConvertDataTable<JobsFiles>(dt).FirstOrDefault();
                    return Utility.ConvertDataTable<JobsFiles>(dt).FirstOrDefault();
                else
                    return null;
            }
        }

        public string UpdateJobsFilesTouches()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET LastState='', LastStateTimer='', Touches = CASE WHEN Touches IS NULL THEN @Touches ELSE CONCAT(Touches, @Touches) END, CurrentCheckout = NULL, AltTxt = @AltTxt WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Touches",
                    DbType = DbType.String,
                    Value = Touches
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AltTxt",
                    DbType = DbType.Int64,
                    Value = AltTxt
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesLastStateLastStateTimerByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET LastState = @Phase, LastStateTimer=@timer  WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Phase",
                    DbType = DbType.String,
                    Value = LastState
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@timer",
                    DbType = DbType.String,
                    Value = LastStateTimer
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesFormFlagByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET FormsFlag = CASE WHEN FormsFlag IS NULL OR FormsFlag = 0 THEN 1 ELSE 0 END WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsLinkingFlagFlagByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET LinkingFlag = CASE WHEN LinkingFlag IS NULL OR LinkingFlag = 0 THEN 1 ELSE 0 END WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsReviewFlagByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET ReviewFlag = CASE WHEN ReviewFlag IS NULL OR ReviewFlag = 0 THEN 1 ELSE 0 END WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateJobsReviewFixByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET ReviewFix = @ReviewFix WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ReviewFix",
                    DbType = DbType.Int32,
                    Value = ReviewFix
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateJobsFinalFixByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET FinalFix = @FinalFix WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FinalFix",
                    DbType = DbType.Int32,
                    Value = FinalFix
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateJobsLastTaggerByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET Status = 'TAGGING', AssignedTo = @AssignedTo, LastTagger = @LastTagger,  Fix = @Fix WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AssignedTo",
                    DbType = DbType.String,
                    Value = AssignedTo
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@LastTagger",
                    DbType = DbType.String,
                    Value = LastTagger
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Fix",
                    DbType = DbType.Int64,
                    Value = Fix
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateJobsAssignedToByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET Status = @Status,  AssignedTo = @AssignedTo,  Fix = @Fix WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AssignedTo",
                    DbType = DbType.String,
                    Value = AssignedTo
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = Status
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Fix",
                    DbType = DbType.Int64,
                    Value = Fix
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public bool CheckReadyForDelivery()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT COUNT(ID) AS NotReady FROM jobs_files WHERE JobID=@ID AND Status != 'TOBEDELIVERED' AND Deleted=0";


                MySqlCommand cmd = new MySqlCommand(qry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
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
                    //return Utility.ConvertDataTable<JobsFiles>(dt).FirstOrDefault();
                    if (abledoc.Utility.CommonHelper.GetDBInt(dt.Rows[0]["NotReady"].ToString()) > 0)
                        return true;
                    else
                        return false;
                else
                    return false;
            }
        }
        public List<JobsFiles> GetJobFileListIDByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT JobID, Status, Deadline, DeliveryCount from jobs_files WHERE JobID=@JobID ORDER BY Deadline ";

                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int64,
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
                    return Utility.ConvertDataTable<JobsFiles>(dt);
                else
                    return null;
            }
        }

        public string UpdateJobsFilesTouchesReview()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET LastState='', LastStateTimer='', Touches = CASE WHEN Touches IS NULL THEN @Touches ELSE CONCAT(Touches, @Touches) END, CurrentCheckout = NULL WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Touches",
                    DbType = DbType.String,
                    Value = Touches
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesDeliveryCount()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET DeliveryCount = DeliveryCount+1 WHERE ID = (SELECT FileID FROM jobs_files_versions WHERE ID=@ID LIMIT 1)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateCurrentPage()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET CurrentPage = @CurrentPage WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CurrentPage",
                    DbType = DbType.Int32,
                    Value = CurrentPage
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateAssemblyPage()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET AssemblyPage = @CurrentPage WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CurrentPage",
                    DbType = DbType.Int32,
                    Value = CurrentPage
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobsFilesCheckOut()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET CurrentPage = @CurrentPage, AltTxt = @AltTxt, LastTagger = @LastTagger, LastState='', LastStateTimer='', Touches = CASE WHEN Touches IS NULL THEN @Touches ELSE CONCAT(Touches, @Touches) END, CurrentCheckout = NULL WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Touches",
                    DbType = DbType.String,
                    Value = Touches
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CurrentPage",
                    DbType = DbType.Int64,
                    Value = CurrentPage
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AltTxt",
                    DbType = DbType.Int64,
                    Value = AltTxt
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@LastTagger",
                    DbType = DbType.String,
                    Value = LastTagger
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        //public string uploadExcel(DataTable dt, string jobID)
        //{
        //    using (MySqlConnection con = context.GetConnection(Constants.ABLEDOCS_DB))
        //    {
        //        MySqlConnector.MySqlBulkCopy sqlBulkCopy = new MySqlConnector.MySqlBulkCopy(con);

        //        //Set the database table name.
        //        sqlBulkCopy.DestinationTableName = "jobs_files";

        //        //[OPTIONAL]: Map the Excel columns with that of the database table
        //        sqlBulkCopy.ColumnMappings.Add("JobId", jobID);
        //        sqlBulkCopy.ColumnMappings.Add("Description", "Description");
        //        sqlBulkCopy.ColumnMappings.Add("FileName", "Description");
        //        sqlBulkCopy.ColumnMappings.Add("Quantity", "Quantity");
        //        sqlBulkCopy.ColumnMappings.Add("PricePer", "Unit");
        //        sqlBulkCopy.ColumnMappings.Add("Price", "Unit Price");
        //        con.Open();
        //        sqlBulkCopy.WriteToServer(dt);
        //        con.Close();

        //    }
        //}
    }
}
