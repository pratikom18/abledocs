using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Jobs
    {

        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public int ClientID { get; set; }
        public Int64 UID { get; set; }
        public string Code { get; set; }
        public string Company { get; set; }
        public string Deadline { get; set; }
        public string DeadlineTime { get; set; }
        public string JobType { get; set; }
        public Int64 Files { get; set; }
        public decimal Pages { get; set; }
        public decimal QuotedValue { get; set; }
        public string Currency { get; set; }
        public decimal QuotedHours { get; set; }
        public decimal hours { get; set; }
        public Int64 FinalFiles { get; set; }
        public decimal FinalPages { get; set; }
        public decimal FinalPercent { get; set; }
        public string Status { get; set; }
        public decimal Count { get; set; }
        public string Year { get; set; }
        public string CountryCode { get; set; }
        public string AssignedTo { get; set; }
        public string POText { get; set; }
        public string Security { get; set; }
        public string Acrobat_Security_Settings { get; set; }
        public string Tagging_Instructions { get; set; }
        public string CustomPassword { get; set; }
        public string Logo_altText { get; set; }
        public string Notes { get; set; }
        public string QuoteOnly { get; set; }
        public string DaxsFlag { get; set; }
        public string SourceFileTagging { get; set; }

        //[Required(ErrorMessage = "Contact field is required")]
        public Int64 ContactID { get; set; }
        public string BillingContactID { get; set; }
        public Int64 P1 { get; set; }
        public Int64 P2 { get; set; }
        public Int64 P4 { get; set; }
        public Int64 P5 { get; set; }
        public string AltProvided { get; set; }
        public string MultiLanguage { get; set; }
        public string Sample { get; set; }
        public string FixUpdate { get; set; }
        public string MultiDeadline { get; set; }
        public string ADScan { get; set; }
        public string ADStream { get; set; }
        public string NonProductionRelated { get; set; }
        public Int64 Priority { get; set; }
        public string CopiedToID { get; set; }
        public string SecDeliveryContactID { get; set; }
        public Int64 P1ToP4 { get; set; }
        public Int64 P4ToDelivery { get; set; }
        public string RestrictedUID { get; set; }
        public string Mode { get; set; }
        public Int64 Approved { get; set; }
        public string DefaultUnit { get; set; }
        public Decimal HourlyRate { get; set; }
        public Decimal PageRate { get; set; }
        public Decimal DocumentRate { get; set; }
        public Decimal DefaultPrice { get; set; }
        public string EngagementNum { get; set; }
        public string JobQuotedAs { get; set; }
        public string ContractDate { get; set; }
        public string VarianceComment { get; set; }
        public DateTime Created { get; set; }
        public string ManualDeadline { get; set; }
        public string QuoteFlag { get; set; }
        public string POClient { get; set; }
        public int AltTxt { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalPages { get; set; }
        public string PricePer { get; set; }
        public List<JobsFiles> JobFileTree { get; set; }
        public List<JobsFilesVersions> JobsFilesVersionsTree { get; set; }
        public List<JobsFilesVersions> OtherFilesList { get; set; }
        public List<JobsFilesVersions> ReferenceFilesList { get; set; }
        public List<JobsFiles> FilesList { get; set; }
        public decimal totalQuantity { get; set; }
        public decimal totalPages { get; set; }
        public string pricePer { get; set; }
        public Int64 fileC { get; set; }
        public int Age { get; set; }
        public DateTime CreatedJobTimestamp { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public string Difference { get; set; }
        public int Days { get; set; }
        public string PurchaseOrder { get; set; }
        public EmailTemplate DeliveryEmailTemplate { get; set; }
        public Int64 VarianceApproved { get; set; }
        public string JobFromADapi { get; set; }
        public string databasename { get; set; }
        public string loginusercountry { get; set; }
        public string flag { get; set; }
        public string PORequired { get; set; }
        #endregion

        public List<Jobs> GetJobListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, int TodayTask)
        {
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "OPEN")
                {

                    Where_Status = "Status = 'OPEN'";
                }
                else if (Status == "PENDING")
                {

                    Where_Status = "Status = 'PENDING'";
                }
                else if (Status == "QUOTE")
                {

                    Where_Status = "Status <> 'CLOSED' AND Status <> 'CANCELLED' AND QuoteFlag = '0'";
                }
                else if (Status == "TOBEDELIVERED")
                {

                    Where_Status = "Status = 'TOBEDELIVERED'";
                }
                else if (Status == "DELIVERED")
                {

                    Where_Status = "Status = 'DELIVERED'";
                }
                else if (!string.IsNullOrEmpty(Status))
                {

                    Where_Status = "Status = '"+ Status + "'";
                }

                string myqry = "";
                if (Status == "OPEN")
                {
                    //string myqry = "SELECT j.ID as JobID, c.ID as ClientID, c.Code, j.Deadline, j.DeadlineTime, j.JobType, j.JobQuotedAs, c.Currency, j.PageRate,vjs.Files,vjs.Pages,vjs.QuotedValue,vqh.QuotedHours,vt.hours,vpb.FinalPages,vpb.FinalFiles,(vpb.FinalPages * 100 / vjs.Pages) as FinalPercent FROM jobs AS j JOIN clients AS c ON j.ClientID = c.ID LEFT JOIN vwjobssummary vjs ON j.ID=vjs.JobID LEFT JOIN vwquotedhours vqh ON j.ID=vqh.JobID LEFT JOIN vwtimer vt ON j.ID=vt.JobID LEFT JOIN vwprogressbar vpb ON j.ID=vpb.JobID Where " + Status;
                    myqry = "SELECT j.Notes,j.ID as JobID, c.ID as ClientID, c.Code, j.Deadline, j.DeadlineTime, j.JobType, j.JobQuotedAs, c.Currency, j.PageRate,vjs.Files,vjs.Pages,vjs.QuotedValue,vqh.QuotedHours,vt.hours,vpb.FinalPages,vpb.FinalFiles,(vpb.FinalPages * 100 / vjs.Pages) as FinalPercent,DATE_FORMAT(c.ClientSince, '%Y') as Year,co.code as CountryCode," +
                    "(SELECT COUNT(*) AS p1 FROM DBNAME.jobs_files AS fp1 JOIN DBNAME.jobs AS jp1 ON jp1.ID = fp1.JobID WHERE jp1.ID = j.ID AND fp1.Status = 'TAGGING' AND fp1.FolderFlagged <> 2 AND fp1.Deleted = 0 AND jp1.Status = 'OPEN') AS P1," +
                    "(SELECT COUNT(*) AS p2 FROM DBNAME.jobs_files AS fp2 JOIN DBNAME.jobs AS jp2 ON jp2.ID = fp2.JobID WHERE jp2.ID = j.ID AND fp2.Status = 'REVIEW' AND fp2.FolderFlagged <> 2 AND fp2.Deleted = 0 AND jp2.Status = 'OPEN') AS P2," +
                    "(SELECT COUNT(*) AS p4 FROM DBNAME.jobs_files AS fp4 JOIN DBNAME.jobs AS jp4 ON jp4.ID = fp4.JobID WHERE jp4.ID = j.ID AND fp4.Status = 'FINAL' AND fp4.FolderFlagged <> 2 AND fp4.Deleted = 0 AND jp4.Status = 'OPEN') AS P4," +
                    "(SELECT COUNT(*) AS p5 FROM DBNAME.jobs_files AS fp5 JOIN DBNAME.jobs AS jp5 ON jp5.ID = fp5.JobID WHERE jp5.ID = j.ID AND fp5.Status = 'QC' AND fp5.FolderFlagged <> 2 AND fp5.Deleted = 0 AND jp5.Status = 'OPEN') AS P5" +
                    ", jt.display_order,j.Priority,'DBNAME' AS databasename FROM DBNAME.jobs AS j " +
                    "JOIN DBNAME.clients AS c ON j.ClientID = c.ID " +
                    "left join COMMONDBNAME.countries co ON co.code=c.country " +
                    "LEFT JOIN DBNAME.vwjobssummary vjs ON j.ID=vjs.JobID " +
                    "LEFT JOIN DBNAME.vwquotedhours vqh ON j.ID=vqh.JobID " +
                    "LEFT JOIN DBNAME.vwtimer vt ON j.ID=vt.JobID " +
                    "LEFT JOIN DBNAME.vwprogressbar vpb ON j.ID=vpb.JobID " +
                    "LEFT JOIN COMMONDBNAME.jobs_type jt ON jt.jobtype=j.JobType " +
                    "Where " + Where_Status;

                }
                else
                {
                    myqry = "SELECT j.Notes,j.ID as JobID, c.ID as ClientID, c.Code, j.Deadline, j.DeadlineTime, j.JobType, j.JobQuotedAs, c.Currency, j.PageRate,vjs.Files,vjs.Pages,vjs.QuotedValue,vqh.QuotedHours,vt.hours,0.00 as FinalPages,0 as FinalFiles,0.00 as FinalPercent,DATE_FORMAT(c.ClientSince, '%Y') as Year,co.code as CountryCode," +
                    "0 AS P1,0 AS P2,0 AS P4,0 AS P5,jt.display_order,j.Priority,'DBNAME' AS databasename " +
                    " FROM DBNAME.jobs AS j " +
                    "JOIN DBNAME.clients AS c ON j.ClientID = c.ID " +
                    "left join COMMONDBNAME.countries co ON co.code=c.country " +
                    "LEFT JOIN DBNAME.vwjobssummary vjs ON j.ID=vjs.JobID " +
                    "LEFT JOIN DBNAME.vwquotedhours vqh ON j.ID=vqh.JobID " +
                    "LEFT JOIN DBNAME.vwtimer vt ON j.ID=vt.JobID " +
                    "LEFT JOIN COMMONDBNAME.jobs_type jt ON jt.jobtype=j.JobType " +
                    "Where " + Where_Status;
                }


                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    myqry = myqry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (c.ID LIKE '%" + SearchKey + "%'" +
                        " OR c.Code LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(j.Deadline,' ',j.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR j.ID LIKE '%" + SearchKey + "%'" +
                        " OR j.JobType LIKE '%" + SearchKey + "%'" +
                        " OR c.Currency LIKE '%" + SearchKey + "%'" +
                        " OR vjs.Files LIKE '%" + SearchKey + "%'" +
                        " OR vjs.Pages LIKE '%" + SearchKey + "%'" +
                        " OR vjs.QuotedValue LIKE '%" + SearchKey + "%'" +
                        " OR vqh.QuotedHours LIKE '%" + SearchKey + "%'" +
                        " OR vt.hours LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    //myqry = myqry + " order by j.Deadline ASC,j.DeadlineTime ASC,case when j.JobType = 'ASAP' THEN 1 when j.JobType = 'RUSH' THEN 2 when j.JobType = 'FIXED' THEN 3 when j.JobType = 'FLEX' THEN 4 when j.JobType = 'MULTI' THEN 5 END ASC";
                    if (Status == "OPEN")
                    {
                        //myqry = myqry + " order by j.Deadline ASC,j.DeadlineTime ASC,case when j.JobType = 'ASAP' THEN 1 when j.JobType = 'RUSH' THEN 2 when j.JobType = 'FIXED' THEN 3 when j.JobType = 'FLEX' THEN 4 when j.JobType = 'MULTI' THEN 5 END ASC";
                        myqry3 = myqry3 + " order by display_order asc,Deadline ASC,DeadlineTime ASC";
                    }
                    else
                    {
                        //myqry = myqry + " order by j.Deadline DESC,j.DeadlineTime ASC,case when j.JobType = 'ASAP' THEN 1 when j.JobType = 'RUSH' THEN 2 when j.JobType = 'FIXED' THEN 3 when j.JobType = 'FLEX' THEN 4 when j.JobType = 'MULTI' THEN 5 END ASC";
                        myqry3 = myqry3 + " order by display_order asc,Deadline DESC,DeadlineTime ASC,Priority DESC";

                    }
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
                var countQry = "SELECT COUNT(*) as total FROM DBNAME.jobs Where " + Where_Status;
                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    countQry = countQry + " AND Deadline = '" + TodayDate + "'";
                }

                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select SUM(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Jobs>(dt);
                }
                else
                {
                    return new List<Jobs>();
                }
            }
        }

        public string GetDataTableFiltered(string SearchKey, string Status, int TodayTask)
        {
            string Where_Status = null;
            string totalcount = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "OPEN")
                {

                    Where_Status = "Status = 'OPEN'";
                }
                else if (Status == "PENDING")
                {

                    Where_Status = "Status = 'PENDING'";
                }
                else if (Status == "QUOTE")
                {

                    Where_Status = "Status <> 'CLOSED' AND Status <> 'CANCELLED' AND QuoteFlag = '0'";
                }
                else if (Status == "TOBEDELIVERED")
                {

                    Where_Status = "Status = 'TOBEDELIVERED'";
                }
                else if (Status == "DELIVERED")
                {

                    Where_Status = "Status = 'DELIVERED'";
                }
                else if (!string.IsNullOrEmpty(Status))
                {

                    Where_Status = "Status = '" + Status + "'";
                }

                string myqry = "";
                if (Status == "OPEN")
                {
                    //string myqry = "SELECT j.ID as JobID, c.ID as ClientID, c.Code, j.Deadline, j.DeadlineTime, j.JobType, j.JobQuotedAs, c.Currency, j.PageRate,vjs.Files,vjs.Pages,vjs.QuotedValue,vqh.QuotedHours,vt.hours,vpb.FinalPages,vpb.FinalFiles,(vpb.FinalPages * 100 / vjs.Pages) as FinalPercent FROM jobs AS j JOIN clients AS c ON j.ClientID = c.ID LEFT JOIN vwjobssummary vjs ON j.ID=vjs.JobID LEFT JOIN vwquotedhours vqh ON j.ID=vqh.JobID LEFT JOIN vwtimer vt ON j.ID=vt.JobID LEFT JOIN vwprogressbar vpb ON j.ID=vpb.JobID Where " + Status;
                    myqry = "SELECT COUNT(*) TotalCount FROM DBNAME.jobs AS j " +
                    "JOIN DBNAME.clients AS c ON j.ClientID = c.ID " +
                    "left join COMMONDBNAME.countries co ON co.code=c.country " +
                    "LEFT JOIN DBNAME.vwjobssummary vjs ON j.ID=vjs.JobID " +
                    "LEFT JOIN DBNAME.vwquotedhours vqh ON j.ID=vqh.JobID " +
                    "LEFT JOIN DBNAME.vwtimer vt ON j.ID=vt.JobID " +
                    "LEFT JOIN DBNAME.vwprogressbar vpb ON j.ID=vpb.JobID " +
                    "LEFT JOIN COMMONDBNAME.jobs_type jt ON jt.jobtype=j.JobType " +
                    "Where " + Where_Status;

                }
                else
                {
                    myqry = "SELECT COUNT(*) TotalCount FROM DBNAME.jobs AS j " +
                    "JOIN DBNAME.clients AS c ON j.ClientID = c.ID " +
                    "left join COMMONDBNAME.countries co ON co.code=c.country " +
                    "LEFT JOIN DBNAME.vwjobssummary vjs ON j.ID=vjs.JobID " +
                    "LEFT JOIN DBNAME.vwquotedhours vqh ON j.ID=vqh.JobID " +
                    "LEFT JOIN DBNAME.vwtimer vt ON j.ID=vt.JobID " +
                    "LEFT JOIN COMMONDBNAME.jobs_type jt ON jt.jobtype=j.JobType " +
                    "Where " + Where_Status;
                }


                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    myqry = myqry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (c.ID LIKE '%" + SearchKey + "%'" +
                        " OR c.Code LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(j.Deadline,' ',j.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR j.ID LIKE '%" + SearchKey + "%'" +
                        " OR j.JobType LIKE '%" + SearchKey + "%'" +
                        " OR c.Currency LIKE '%" + SearchKey + "%'" +
                        " OR vjs.Files LIKE '%" + SearchKey + "%'" +
                        " OR vjs.Pages LIKE '%" + SearchKey + "%'" +
                        " OR vjs.QuotedValue LIKE '%" + SearchKey + "%'" +
                        " OR vqh.QuotedHours LIKE '%" + SearchKey + "%'" +
                        " OR vt.hours LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(TotalCount) as TotalCount from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " ;
                MySqlCommand cmd = new MySqlCommand(myqry, conn);

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
                    Production p = ConvertDataTable<Production>(dt.Rows[0].Table).FirstOrDefault();
                    totalcount = p.TotalCount.ToString();
                }
            }
            return totalcount;

        }

        public Jobs GetJobsById(long Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT j.*,c.PO as POClient,c.code,DATE_FORMAT(c.ClientSince, '%Y') as Year,co.code as CountryCode FROM jobs j Left JOIN clients AS c ON j.ClientID = c.ID left join COMMONDBNAME.countries co ON co.code=c.country WHERE j.ID= " + Id;

                qry = qry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                
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
                    return ConvertDataTable<Jobs>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public List<Jobs> GetClientsList()
        {
            //SQLHelper sh = new SQLHelper();
            //DataTable dt = sh.FillDataTable("admin_ParentMenuList", CommandType.StoredProcedure);
            //var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string query = "SELECT Code, Company from DBNAME.clients";

               
                string myqry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                query = "select * from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a ORDER BY Code ASC";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt);
                else
                    return null;
            }

        }

        public List<Jobs> GetClientsCountryList()
        {
            //SQLHelper sh = new SQLHelper();
            //DataTable dt = sh.FillDataTable("admin_ParentMenuList", CommandType.StoredProcedure);
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Code, Company from clients ORDER BY Code ASC", conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt);
                else
                    return null;
            }

        }

        public List<Jobs> GetStatusList()
        {
            //SQLHelper sh = new SQLHelper();
            //DataTable dt = sh.FillDataTable("admin_ParentMenuList", CommandType.StoredProcedure);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                //var query = "SELECT STATUS,COUNT(*) AS COUNT FROM jobs where Status != 'CANCELLED' AND Status != 'CLOSED' GROUP BY STATUS" +
                //  " UNION ALL SELECT 'QUOTE' AS STATUS, COUNT(*) AS COUNT FROM jobs WHERE STATUS <> 'CLOSED' AND STATUS <> 'CANCELLED' AND QuoteFlag = '0' GROUP BY STATUS";
                var query = "select * from ( " +
                            "SELECT jobs.STATUS,COUNT(*) AS COUNT, js.statusid "+
                            "FROM DBNAME.jobs " +
                            "INNER JOIN COMMONDBNAME.jobs_status js ON js.status = jobs.status " +
                            "WHERE jobs.Status != 'CANCELLED' AND jobs.Status != 'CLOSED' " +
                            "GROUP BY STATUS "+
                            "UNION ALL "+
                            "SELECT 'QUOTE' AS STATUS, COUNT(*) AS COUNT, js.statusid "+
                            "FROM DBNAME.jobs " +
                            "INNER JOIN COMMONDBNAME.jobs_status js ON js.status = jobs.status " +
                            "WHERE jobs.STATUS <> 'CLOSED' AND jobs.STATUS <> 'CANCELLED' AND jobs.QuoteFlag = '0' "+
                            "GROUP BY STATUS "+
                            ") as temp";
                query = query.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB); ;

                query = "select STATUS,FLOOR(SUM(COUNT)) as COUNT,statusid from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a GROUP BY STATUS ORDER BY statusid ASC";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt);
                else
                    return null;
            }

        }

        public string CreateJob()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "INSERT INTO jobs (ClientID, Security, CustomPassword, Acrobat_Security_Settings, Tagging_Instructions, Logo_altText, FileStorage_Location, Limited_Users, HourlyRate, PageRate, RestrictedUID, Mode)";
                query += " SELECT ID, Security, CustomPassword, Acrobat_Security_Settings, Tagging_Instructions, Logo_altText, FileStorage_Location, Limited_Users, HourlyRate, PageRate, RestrictedUID, Mode FROM clients WHERE ID = @ClientID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);

                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJob()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update Jobs set ClientID = @ClientID, Status = @Status, Security = @Security," +
                    "CustomPassword = @CustomPassword, Acrobat_Security_Settings=@Acrobat_Security_Settings," +
                    " Tagging_Instructions=@Tagging_Instructions, Logo_altText=@Logo_altText," +
                    " ContactID=@ContactID, Deadline=@Deadline, DeadlineTime=@DeadlineTime," +
                    " AssignedTo=@AssignedTo, JobType=@JobType, SourceFileTagging=@SourceFileTagging," +
                    " DaxsFlag=@DaxsFlag, POText=@POText, BillingContactID=@BillingContactID," +
                    " SecDeliveryContactID=@SecDeliveryContactID, P1ToP4=@P1ToP4," +
                    " P4ToDelivery=@P4ToDelivery, QuoteOnly=@QuoteOnly, JobFromADapi=@JobFromADapi," +
                    " AltProvided=@AltProvided,MultiLanguage=@MultiLanguage,Sample=@Sample," +
                    " FixUpdate=@FixUpdate,MultiDeadline=@MultiDeadline,ADScan=@ADScan," +
                    " ADStream=@ADStream,NonProductionRelated=@NonProductionRelated,Priority=@Priority,Notes=@Notes,PurchaseOrder=@PurchaseOrder" +
                    " WHERE ID=@ID";


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

        public string UpdateClient()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update Jobs set ClientID = @ClientID WHERE ID=@ID";


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

        public string UpdateCopyJob()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs SET Deadline = @Deadline, UID = @UID WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Deadline",
                    DbType = DbType.String,
                    Value = Deadline,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UID",
                    DbType = DbType.Int32,
                    Value = UID,
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobDeadlineDate(int ManualDeadline = 0)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs SET Deadline = @Deadline";
                if (ManualDeadline == 1)
                {
                    query = query + ", ManualDeadline = 1";
                }
                query = query + " WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Deadline",
                    DbType = DbType.String,
                    Value = Deadline,
                });
                

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string JobCancel()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update jobs SET Status='CANCELLED' WHERE ID=@ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string JobCancelByCopyJob(int newJobID,int oldJobID)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Update jobs SET Status='CANCELLED', CopiedToID=" + newJobID + " WHERE ID="+ oldJobID;


                MySqlCommand cmd = new MySqlCommand(query, conn);
                


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public List<Jobs> GetInvoiceTracking(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM invoice_tracking a INNER JOIN invoice_tmp b ON a.JobID=b.JobID WHERE a.JobID = '" + ID + "' AND a.OperationType = 'InvoiceGenerated' AND b.InvoiceIDQB<>0", conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt);
                else
                    return null;
            }

        }

        public List<Jobs> GetInvoiceInstance(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select Approved from invoice_instance Where JobID= '" + ID + "'", conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt);
                else
                    return null;
            }

        }

        public string UpdateDefaultPrice(string unitTypePrice)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs SET DefaultUnit = @DefaultUnit, " + unitTypePrice + " = @DefaultPrice WHERE ID = @ID";


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

        public string QuoteDone()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE jobs SET QuoteFlag = 1 WHERE ID = @ID";


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

        public Jobs GetJobsByJobFilesID(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM jobs WHERE ID = (SELECT JobID from jobs_files WHERE ID=" + ID + " LIMIT 1)", conn);


                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt).FirstOrDefault();
                else
                    return null;
            }
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientID",
                DbType = DbType.Int32,
                Value = ClientID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            //cmd.Parameters.Add(new MySqlParameter
            //{
            //    ParameterName = "@ClientID",
            //    DbType = DbType.Int32,
            //    Value = ClientID,
            //});

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Status",
                DbType = DbType.String,
                Value = Status,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Security",
                DbType = DbType.String,
                Value = Security,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CustomPassword",
                DbType = DbType.String,
                Value = CustomPassword,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Acrobat_Security_Settings",
                DbType = DbType.String,
                Value = Acrobat_Security_Settings,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Tagging_Instructions",
                DbType = DbType.String,
                Value = Tagging_Instructions,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Logo_altText",
                DbType = DbType.String,
                Value = Logo_altText,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ContactID",
                DbType = DbType.Int64,
                Value = ContactID,
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
                ParameterName = "@JobType",
                DbType = DbType.String,
                Value = JobType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SourceFileTagging",
                DbType = DbType.String,
                Value = SourceFileTagging,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DaxsFlag",
                DbType = DbType.String,
                Value = DaxsFlag,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@POText",
                DbType = DbType.String,
                Value = POText,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@BillingContactID",
                DbType = DbType.String,
                Value = BillingContactID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SecDeliveryContactID",
                DbType = DbType.String,
                Value = SecDeliveryContactID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@P1ToP4",
                DbType = DbType.Int64,
                Value = P1ToP4,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@P4ToDelivery",
                DbType = DbType.Int64,
                Value = P4ToDelivery,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@QuoteOnly",
                DbType = DbType.String,
                Value = QuoteOnly,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobFromADapi",
                DbType = DbType.String,
                Value = JobFromADapi,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AltProvided",
                DbType = DbType.String,
                Value = AltProvided,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MultiLanguage",
                DbType = DbType.String,
                Value = MultiLanguage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Sample",
                DbType = DbType.String,
                Value = Sample,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FixUpdate",
                DbType = DbType.String,
                Value = FixUpdate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MultiDeadline",
                DbType = DbType.String,
                Value = MultiDeadline,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ADScan",
                DbType = DbType.String,
                Value = ADScan,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ADStream",
                DbType = DbType.String,
                Value = ADStream,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NonProductionRelated",
                DbType = DbType.String,
                Value = NonProductionRelated,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Priority",
                DbType = DbType.Int64,
                Value = Priority,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Notes",
                DbType = DbType.String,
                Value = Notes,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@HourlyRate",
                DbType = DbType.Decimal,
                Value = HourlyRate,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PageRate",
                DbType = DbType.Decimal,
                Value = PageRate,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DocumentRate",
                DbType = DbType.Decimal,
                Value = DocumentRate,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DefaultPrice",
                DbType = DbType.Decimal,
                Value = DefaultPrice,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DefaultUnit",
                DbType = DbType.String,
                Value = DefaultUnit,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ContractDate",
                DbType = DbType.String,
                Value = ContractDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VarianceComment",
                DbType = DbType.String,
                Value = VarianceComment,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PurchaseOrder",
                DbType = DbType.String,
                Value = PurchaseOrder,
            });
            
        }

        public string UpdateJobsStatusByID(int JobID)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET Status = 'TOBEDELIVERED' WHERE ID = " + JobID;

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

        public string JobInvoiceContactUpdate()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET ContractDate = @ContractDate WHERE ID = @ID";

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
        public string UpdateTagging(int ID, string TaggingInstructions)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET Tagging_Instructions = '" + TaggingInstructions + "' WHERE ID = " + ID;

                MySqlCommand cmd = new MySqlCommand(query, conn);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string SaveVarianceComment()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET VarianceComment = @VarianceComment WHERE ID = @ID";

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
        public string UpdateDeadlineBySourcefile()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET Deadline = CASE" +
                            " WHEN JobType<>'MULTI' THEN @Deadline" +
                            " ELSE Deadline" +
                            " END WHERE ID = @ID AND ManualDeadline = '0'";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);

                cmd.ExecuteNonQuery();


                ExecutionString = "Update successfully";



                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateQuoteFlag()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "UPDATE jobs SET QuoteFlag=@QuoteFlag WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@QuoteFlag",
                    DbType = DbType.String,
                    Value = QuoteFlag,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public Jobs GetJobsByFileID(int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT j.ClientID as ClientID, jf.AltTxt as AltTxt, j.ID as JobID FROM jobs j INNER JOIN jobs_files jf ON j.ID=jf.JobID WHERE jf.ID=" + fileID + " LIMIT 1", conn);


                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt).FirstOrDefault();
                else
                    return null;

            }

        }

        public Jobs GetJobsByJobID(int jobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,(select CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jobs.ID) from clients where clients.ID = jobs.ClientID) as EngagementNum FROM jobs WHERE ID = " + jobID + " LIMIT 1", conn);


                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<Jobs>(dt).FirstOrDefault();
                else
                    return null;

            }

        }

        public List<Jobs> GetJobStatusListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, int TodayTask)
        {
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "PENDINGJOBS")
                {

                    Where_Status = "jobs.Status = 'PENDING'";
                }
                else if (Status == "QUOTEJOBS")
                {

                    Where_Status = "jobs.Status = 'QUOTE'";
                }
                else if (Status == "OPENJOBS")
                {

                    Where_Status = "jobs.Status = 'OPEN'";
                }
                else if (Status == "CLOSEDJOBS")
                {

                    Where_Status = "jobs.Status = 'CLOSED'";
                }
                else if (Status == "TOBEDELIVEREDJOBS")
                {

                    Where_Status = "jobs.Status = 'TOBEDELIVERED'";
                }
                else if (Status == "CANCELLEDJOBS")
                {

                    Where_Status = "jobs.Status = 'CANCELLED'";
                }
                else if (Status == "DELIVEREDJOBS")
                {

                    Where_Status = "jobs.Status = 'DELIVERED'";
                }

                string myqry = "";
                //if (Status == "CLOSEDJOBS")
                //{
                myqry = "SELECT jobs.ID, jobs.Created, jobs.Status, jobs.QuoteFlag, jobs.Deadline, jobs.DeadlineTime, " +
                "jobs.JobType, jobs.Notes, jobs.QuoteOnly, jobs.JobFromADapi, jobs.JobQuotedAs, jobs.VarianceComment, clients.Company,CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jobs.ID) EngagementNum, " +
                "clients.OldCode, clients.Code, clients.ClientSince, clients.Country, clients.ClientCreated,'DBNAME' AS databasename FROM DBNAME.jobs " +
                "LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID WHERE " + Where_Status;

               if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    myqry = myqry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs.ID LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(jobs.Deadline,' ',jobs.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR jobs.JobType LIKE '%" + SearchKey + "%'" +
                        "OR CONCAT(YEAR(ClientCreated), '-', Country, '-', Code, '-', jobs.ID) like '%" + SearchKey + "%'" +
                        ")";
                }

                string myqry3 = "";

                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {

                    myqry3 = myqry3 + " ORDER BY Deadline, DeadlineTime, FIELD(JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }


                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "SELECT * FROM ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " + myqry3;

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

                string countQry = "";
                countQry = "SELECT jobs.ID, jobs.Created, jobs.Status, jobs.QuoteFlag, jobs.Deadline, jobs.DeadlineTime, " +
              "jobs.JobType, jobs.Notes, jobs.QuoteOnly, jobs.JobFromADapi, jobs.JobQuotedAs, jobs.VarianceComment, clients.Company,CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jobs.ID) EngagementNum, " +
              "clients.OldCode, clients.Code, clients.ClientSince, clients.Country, clients.ClientCreated FROM DBNAME.jobs " +
              "LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID WHERE " + Where_Status;

                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    countQry = countQry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs.ID LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(jobs.Deadline,' ',jobs.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR jobs.JobType LIKE '%" + SearchKey + "%'" +
                        "OR CONCAT(YEAR(ClientCreated), '-', Country, '-', Code, '-', jobs.ID) like '%" + SearchKey + "%'" +
                        ")";
                }

            
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select COUNT(*) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Jobs>(dt);
                }
                else
                {
                    return new List<Jobs>();
                }
            }
        }

        public List<Jobs> GetCancelJobStatusListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, int TodayTask)
        {
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "PENDINGJOBS")
                {

                    Where_Status = "jobs.Status = 'PENDING'";
                }
                else if (Status == "QUOTEJOBS")
                {

                    Where_Status = "jobs.Status = 'QUOTE'";
                }
                else if (Status == "OPENJOBS")
                {

                    Where_Status = "jobs.Status = 'OPEN'";
                }
                else if (Status == "CLOSEDJOBS")
                {

                    Where_Status = "jobs.Status = 'CLOSED'";
                }
                else if (Status == "TOBEDELIVEREDJOBS")
                {

                    Where_Status = "jobs.Status = 'TOBEDELIVERED'";
                }
                else if (Status == "CANCELLEDJOBS")
                {

                    Where_Status = "jobs.Status = 'CANCELLED'";
                }
                else if (Status == "DELIVEREDJOBS")
                {

                    Where_Status = "jobs.Status = 'DELIVERED'";
                }

                string myqry = "";
                //if (Status == "CLOSEDJOBS")
                //{
                myqry = "SELECT jobs.ID, jobs.Created, jobs.Status, jobs.QuoteFlag, jobs.Deadline, jobs.DeadlineTime, " +
                "jobs.JobType, jobs.Notes, jobs.QuoteOnly, jobs.JobFromADapi,jobs.JobQuotedAs, jobs.VarianceComment, clients.Company,CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jobs.ID) EngagementNum, " +
                "clients.OldCode, clients.Code, clients.ClientSince, clients.Country, clients.ClientCreated FROM jobs " +
                "LEFT JOIN clients ON clients.ID = jobs.ClientID WHERE " + Where_Status;

                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    myqry = myqry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs.ID LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(jobs.Deadline,' ',jobs.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR jobs.JobType LIKE '%" + SearchKey + "%'" +
                        "OR CONCAT(YEAR(ClientCreated), '-', Country, '-', Code, '-', jobs.ID) like '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {

                    myqry = myqry + " ORDER BY jobs.Deadline, jobs.DeadlineTime, FIELD(jobs.JobType, 'ASAP', 'RUSH', 'FIXED', 'FLEX', 'MULTI')";

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

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();

                int count = 0;

                //var countQry = "SELECT COUNT(*) FROM jobs Where " + Where_Status;
                var countQry = "SELECT COUNT(*) FROM jobs WHERE " + Where_Status;
                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    countQry = countQry + " AND Deadline = '" + TodayDate + "'";
                }
                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Jobs>(dt);
                }
                else
                {
                    return new List<Jobs>();
                }
            }
        }

        public List<Jobs> GetToBeInvoicedListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, int TodayTask)
        {
            //string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
              
                string myqry = "";


                myqry = "Select distinct jobs.ID, jobs.Created, jobs.Status, jobs.QuoteFlag, jobs.Deadline, jobs.DeadlineTime," +
                   " jobs.JobType, jobs.Notes, jobs.QuoteOnly,jobs.JobFromADapi, jobs.JobQuotedAs,jobs.VarianceComment,clients.Company, " +
                   "CONCAT(YEAR(clients.ClientCreated), '-',COALESCE(clients.Country,'') , '-', clients.Code, '-', jobs.ID)EngagementNum, " +
                   "clients.OldCode, clients.Code, clients.ClientSince, clients.Country, clients.ClientCreated, " +
                   "invoice_instance.InvoiceSentBack,'DBNAME' as databasename " +
                   "From DBNAME.jobs  " +
                   "LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID  " +
                   "LEFT JOIN DBNAME.invoice_instance ON jobs.ID = invoice_instance.JobID " +
                   "Where jobs.ID NOT IN(Select it.JobID from " +
                   "DBNAME.invoice_tracking it " +
                   "LEFT JOIN DBNAME.invoice_instance ii ON it.JobID = ii.JobID " +
                   "Where it.OperationType = 'InvoiceGenerated' " +
                   "AND ii.InvoiceSentBack <> 1) AND jobs.Status <> 'CLOSED' AND jobs.Status <> 'INVOICEPENDING' AND jobs.Status <> 'CANCELLED'";


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (jobs.ID LIKE '%" + SearchKey + "%'" +
                   " OR CONCAT(YEAR(clients.ClientCreated), '-', COALESCE(clients.Country,''), '-', clients.Code, '-', jobs.ID) like '%" + SearchKey + "%' OR jobs.Status like '%" + SearchKey + "%' OR jobs.JobType like '%" + SearchKey + "%' OR jobs.Status like '%" + SearchKey + "%')";
                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 += "Order By FIELD(Status, 'DELIVERED', 'TOBEDELIVERED', 'OPEN', 'PENDING')";

                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = myqry_oth + " UNION ALL " + myqry_eu + " " + myqry3;

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

                //var countQry = "SELECT COUNT(*) FROM invoice_instance  WHERE " + Where_Status;
                var countQry = "Select distinct COUNT(*) as total " +
                    "From DBNAME.jobs  " +
                    "LEFT JOIN DBNAME.clients ON clients.ID = jobs.ClientID  " +
                    "LEFT JOIN DBNAME.invoice_instance ON jobs.ID = invoice_instance.JobID " +
                    "Where jobs.ID NOT IN(Select it.JobID from " +
                    "DBNAME.invoice_tracking it " +
                    "LEFT JOIN DBNAME.invoice_instance ii ON it.JobID = ii.JobID " +
                    "Where it.OperationType = 'InvoiceGenerated' " +
                    "AND ii.InvoiceSentBack <> 1) AND jobs.Status <> 'CLOSED' AND jobs.Status <> 'INVOICEPENDING' AND jobs.Status <> 'CANCELLED'";


                if (SearchKey != "")
                {
                    countQry = countQry + " AND (jobs.ID LIKE '%" + SearchKey + "%'" +
                   "OR CONCAT(YEAR(clients.ClientCreated), '-', COALESCE(clients.Country,''), '-', clients.Code, '-', jobs.ID) like '%" + SearchKey + "%' OR jobs.Status like '%" + SearchKey + "%' OR jobs.JobType like '%" + SearchKey + "%' OR jobs.Status like '%" + SearchKey + "%')";
                }


  
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select Sum(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Jobs>(dt);
                }
                else
                {
                    return new List<Jobs>();
                }
            }
        }

        public string UpdateJobForDelivery()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET Status = 'TOBEDELIVERED' WHERE ID = @ID";

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

        public string UpdateJobPOText()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string query = "UPDATE jobs SET POText = @POText WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@POText",
                    DbType = DbType.String,
                    Value = POText
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobStatus()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET Status = @Status WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = Status
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }
        public string UpdateJobStatusWithContractDate()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs SET Status = @Status,ContractDate = @ContractDate WHERE ID = @ID";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = Status
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ContractDate",
                    DbType = DbType.String,
                    Value = ContractDate
                });
                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public List<Jobs> CheckJobStatus()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT j.Status, ii.Approved, ii.VarianceApproved FROM jobs j LEFT JOIN invoice_instance ii ON j.ID=ii.JobID WHERE j.ID=@ID";
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
                    return Utility.ConvertDataTable<Jobs>(dt);
                else
                    return null;
            }
        }

    }
}
