using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Production
    {
        #region "Properties"
        DataContext context = new DataContext();
        public Int64 JobID { get; set; }
        public Int64 ClientID { get; set; }
        public Int64 FileID { get; set; }
        public string Code { get; set; }
        public string Filename { get; set; }
        public string Deadline { get; set; }
        public string DeadlineTime { get; set; }
        public string JobType { get; set; }
        public Int64 CurrentPage { get; set; }
        public Int64 Pages { get; set; }
        public Int64 AssemblyPage { get; set; }
        public Int64 CurrentCheckout { get; set; }
        public string AssignedTo { get; set; }
        public string FormsFlag { get; set; }
        public string LinkingFlag { get; set; }
        public string ReviewFlag { get; set; }
        public Int64 Lastcheckin { get; set; }
        public decimal TotalCount { get; set; }
        public string EngagementNum { get; set; }
        public string databasename { get; set; }
        public string multiassign { get; set; }
        public Int64 CurrentVersionFileID { get; set; }
        #endregion


        public List<Production> GetProdListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, string SearchBy = "", string txtSearch = "")
        {
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "Phase 1")
                {

                    Where_Status = " AND f.Status = 'TAGGING'";
                }
                else if (Status == "Phase 2")
                {

                    Where_Status = " AND f.Status = 'REVIEW'";
                }
                else if (Status == "ALT Text")
                {

                    Where_Status = " AND j.Status = 'OPEN' AND f.Status <> 'CLOSED' AND f.Status <> 'PENDING' AND (f.AltTxt IS NULL OR f.AltTxt = 0 OR f.AltTxt = 2 OR f.AltTxt = 6) AND f.FolderFlagged <> 2 AND f.Deleted = 0";
                }
                //else if (Status == "Phase 4")
                //{

                //    Where_Status = " AND f.Status = 'FINAL'";
                //}
                else if (Status == "Phase 3")
                {

                    Where_Status = " AND f.Status = 'QC'";
                }
                else if (Status == "Deliverables")
                {

                    Where_Status = " AND f.Status = 'TOBEDELIVERED'";
                }

                string myqry = "SELECT f.ID AS FileID, j.ID as JobID, f.Filename, f.Pages, f.CurrentPage, f.AssemblyPage, f.Deadline, f.DeadlineTime, j.JobType, f.CurrentCheckout, f.AssignedTo, c.Code, c.ID AS ClientID, f.FormsFlag, f.LinkingFlag, f.ReviewFlag,(SELECT u.ID FROM DBNAME.jobs_files_checkouts AS jfc JOIN COMMONDBNAME.users AS u ON jfc.UserID = u.ID WHERE FileID = f.ID AND State<> 'ALT' ORDER BY jfc.Checkin DESC LIMIT 1) as Lastcheckin,case when 'DBNAME' = '" + databasename+ "' then '1' else '0' end as multiassign,'DBNAME' AS databasename,f.CurrentVersionFileID   FROM DBNAME.jobs_files as f JOIN DBNAME.jobs as j ON f.JobID = j.ID JOIN DBNAME.clients as c ON j.ClientID = c.ID Where f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN' " + Where_Status;

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (f.ID LIKE '%" + SearchKey + "%'" +
                        " OR c.Code LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(f.Deadline,' ',f.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR j.ID LIKE '%" + SearchKey + "%'" +
                        " OR j.JobType LIKE '%" + SearchKey + "%'" +
                        " OR f.Filename LIKE '%" + SearchKey + "%'" +
                        " OR f.CurrentCheckout LIKE '%" + SearchKey + "%'" +
                        " OR f.AssignedTo LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (txtSearch != null)
                {
                    if (SearchBy == "ClientName")
                    {
                        myqry = myqry + " AND (c.Company Like '%" + txtSearch + "%' OR c.Code LIKE '%" + txtSearch + "%') ";
                    }
                    else
                    {
                        myqry = myqry + " AND j.ID Like '%" + txtSearch + "%' ";
                    }
                }

                string myqry3 = "";

                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 = myqry3 + " order by Deadline ASC, DeadlineTime ASC,case when JobType = 'ASAP' THEN 1 when JobType = 'RUSH' THEN 2 when JobType = 'FIXED' THEN 3 when JobType = 'FLEX' THEN 4 when JobType = 'MULTI' THEN 5 END ASC, JobID ASC, Filename ASC";
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
                var countQry = "SELECT COUNT(*) as TotalCount  FROM DBNAME.jobs_files as f JOIN DBNAME.jobs as j ON f.JobID = j.ID JOIN DBNAME.clients as c ON j.ClientID = c.ID Where f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN' " + Where_Status;
                if (txtSearch != null)
                {
                    if (SearchBy == "ClientName")
                    {
                        countQry = countQry + " AND  c.Company Like '%" + txtSearch + "%' ";
                    }
                    else
                    {
                        countQry = countQry + " AND  j.ID Like '%" + txtSearch + "%' ";
                    }
                }

             
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(TotalCount) as TotalCount from ( " + countQry_oth + " UNION ALL " + countQry_eu + " ) as a ";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Production>(dt);
                }
                else
                {
                    return new List<Production>();
                }
            }
        }

        public List<Production> GetProdListForCurrentUserTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, string userId, out int TotalRecord, string SearchBy = "", string txtSearch = "")
        {
            string Where_Status = null;
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "Phase 1")
                {

                    Where_Status = " AND f.Status = 'TAGGING'";
                }
                else if (Status == "Phase 2")
                {

                    Where_Status = " AND f.Status = 'REVIEW'";
                }
                else if (Status == "ALT Text")
                {

                    Where_Status = " AND j.Status = 'OPEN' AND f.Status <> 'CLOSED' AND f.Status <> 'PENDING' AND (f.AltTxt IS NULL OR f.AltTxt = 0 OR f.AltTxt = 2 OR f.AltTxt = 6) AND f.FolderFlagged <> 2 AND f.Deleted = 0";
                }
                //else if (Status == "Phase 4")
                //{

                //    Where_Status = " AND f.Status = 'FINAL'";
                //}
                else if (Status == "Phase 3")
                {

                    Where_Status = " AND f.Status = 'QC'";
                }
                else if (Status == "Deliverables")
                {

                    Where_Status = " AND f.Status = 'TOBEDELIVERED'";
                }

                Where_Status = Where_Status + " AND f.AssignedTo = " + userId + " ";

                string myqry = "SELECT f.ID AS FileID, j.ID as JobID, f.Filename, f.Pages, f.CurrentPage, f.AssemblyPage, f.Deadline, f.DeadlineTime, j.JobType, f.CurrentCheckout, f.AssignedTo, c.Code, c.ID AS ClientID, f.FormsFlag, f.LinkingFlag, f.ReviewFlag,(SELECT u.ID FROM DBNAME.jobs_files_checkouts AS jfc JOIN users AS u ON jfc.UserID = u.ID WHERE FileID = f.ID AND State<> 'ALT' ORDER BY jfc.Checkin DESC LIMIT 1) as Lastcheckin, " +
                    " CONCAT(YEAR(c.ClientCreated),'-',COALESCE(c.Country,''),'-',c.Code,'-',j.ID) as EngagementNum,'DBNAME' as databasename " +
                    " FROM DBNAME.jobs_files as f JOIN DBNAME.jobs as j ON f.JobID = j.ID JOIN DBNAME.clients as c ON j.ClientID = c.ID Where f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN' " + Where_Status;

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (f.ID LIKE '%" + SearchKey + "%'" +
                        " OR c.Code LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(f.Deadline,' ',f.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR j.ID LIKE '%" + SearchKey + "%'" +
                        " OR j.JobType LIKE '%" + SearchKey + "%'" +
                        " OR f.Filename LIKE '%" + SearchKey + "%'" +
                        " OR f.CurrentCheckout LIKE '%" + SearchKey + "%'" +
                        " OR f.AssignedTo LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (txtSearch != null)
                {
                    if (SearchBy == "ClientName")
                    {
                        myqry = myqry + " AND  c.Company Like '%" + txtSearch + "%' ";
                    }
                    else
                    {
                        myqry = myqry + " AND  j.ID Like '%" + txtSearch + "%' ";
                    }
                }

                string myqry3="";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    //myqry3 = myqry3 + " order by f.Deadline ASC, f.DeadlineTime ASC,case when j.JobType = 'ASAP' THEN 1 when j.JobType = 'RUSH' THEN 2 when j.JobType = 'FIXED' THEN 3 when j.JobType = 'FLEX' THEN 4 when j.JobType = 'MULTI' THEN 5 END ASC, j.ID ASC, f.Filename ASC";
                    myqry3 = myqry3 + " order by Deadline ASC, DeadlineTime ASC,case when JobType = 'ASAP' THEN 1 when JobType = 'RUSH' THEN 2 when JobType = 'FIXED' THEN 3 when JobType = 'FLEX' THEN 4 when JobType = 'MULTI' THEN 5 END ASC, JobID ASC, Filename ASC";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select * from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " + myqry3;

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                //cmd.Parameters.Add(new MySqlParameter
                //{
                //    ParameterName = "@start",
                //    DbType = DbType.Int32,
                //    Value = inStartIndex,
                //});
                //cmd.Parameters.Add(new MySqlParameter
                //{
                //    ParameterName = "@end",
                //    DbType = DbType.Int32,
                //    Value = inEndIndex,
                //});
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
                var countQry = "SELECT COUNT(*) as TotalCount FROM DBNAME.jobs_files as f JOIN DBNAME.jobs as j ON f.JobID = j.ID JOIN DBNAME.clients as c ON j.ClientID = c.ID Where f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN' " + Where_Status;
                if (txtSearch != null)
                {
                    if (SearchBy == "ClientName")
                    {
                        countQry = countQry + " AND  c.Company Like '%" + txtSearch + "%' ";
                    }
                    else
                    {
                        countQry = countQry + " AND  j.ID Like '%" + txtSearch + "%' ";
                    }
                }

              
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(TotalCount) as TotalCount from ( " + countQry_oth + " UNION ALL " + countQry_eu + " ) as a ";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Production>(dt);
                }
                else
                {
                    return new List<Production>();
                }
            }
        }
        public string GetDataTableFiltered(string SearchKey, string Status)
        {
            string Where_Status = null;
            string totalcount = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "Phase 1")
                {

                    Where_Status = " AND f.Status = 'TAGGING'";
                }
                else if (Status == "Phase 2")
                {

                    Where_Status = " AND f.Status = 'REVIEW'";
                }
                else if (Status == "ALT Text")
                {

                    Where_Status = " AND j.Status = 'OPEN' AND f.Status <> 'CLOSED' AND f.Status <> 'PENDING' AND (f.AltTxt IS NULL OR f.AltTxt = 0 OR f.AltTxt = 2 OR f.AltTxt = 6) AND f.FolderFlagged <> 2 AND f.Deleted = 0";
                }
                //else if (Status == "Phase 4")
                //{

                //    Where_Status = " AND f.Status = 'FINAL'";
                //}
                else if (Status == "Phase 3")
                {

                    Where_Status = " AND f.Status = 'QC'";
                }
                else if (Status == "Deliverables")
                {

                    Where_Status = " AND f.Status = 'TOBEDELIVERED'";
                }

                var query = "SELECT COUNT(*) as TotalCount FROM DBNAME.jobs_files as f JOIN DBNAME.jobs as j ON f.JobID = j.ID JOIN DBNAME.clients as c ON j.ClientID = c.ID Where f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN' " + Where_Status;
                if (SearchKey != "")
                {
                    query = query + " AND (f.ID LIKE '%" + SearchKey + "%'" +
                        " OR c.Code LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(f.Deadline,' ',f.DeadlineTime) LIKE '%" + SearchKey + "%'" +
                        " OR j.ID LIKE '%" + SearchKey + "%'" +
                        " OR j.JobType LIKE '%" + SearchKey + "%'" +
                        " OR f.Filename LIKE '%" + SearchKey + "%'" +
                        " OR f.CurrentCheckout LIKE '%" + SearchKey + "%'" +
                        " OR f.AssignedTo LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                query = query.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                query = "select SUM(TotalCount) as TotalCount from ( " + countQry_oth + " UNION ALL " + countQry_eu + " ) as a ";
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
                {
                    Production p = ConvertDataTable<Production>(dt.Rows[0].Table).FirstOrDefault();
                    totalcount = p.TotalCount.ToString();
                }
            }
            return totalcount;

        }

        public string GetStatusList(string Status)
        {
            string Where_Status = null;
            string totalcount = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "Phase 1")
                {

                    Where_Status = " f.Status = 'TAGGING'";
                }
                else if (Status == "Phase 2")
                {

                    Where_Status = " f.Status = 'REVIEW'";
                }
                else if (Status == "ALT Text")
                {

                    Where_Status = " j.Status = 'OPEN' AND f.Status <> 'CLOSED' AND f.Status <> 'PENDING' AND (f.AltTxt IS NULL OR f.AltTxt = 0 OR f.AltTxt = 2 OR f.AltTxt = 6) AND f.FolderFlagged <> 2 AND f.Deleted = 0";
                }
                //else if (Status == "Phase 4")
                //{

                //    Where_Status = " f.Status = 'FINAL'";
                //}
                else if (Status == "Phase 3")
                {

                    Where_Status = " f.Status = 'QC'";
                }
                else if (Status == "Deliverables")
                {

                    Where_Status = " f.Status = 'TOBEDELIVERED'";
                }

                var query = "SELECT COUNT(*) as TotalCount FROM DBNAME.jobs_files as f JOIN DBNAME.jobs as j ON j.ID = f.JobID WHERE " + Where_Status + " AND f.FolderFlagged <> 2 AND f.Deleted = 0 AND j.Status = 'OPEN'";

                query = query.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = query.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = query.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);
                query = "select SUM(TotalCount) as TotalCount from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " ;

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
                {
                    Production p = ConvertDataTable<Production>(dt.Rows[0].Table).FirstOrDefault();
                    totalcount = p.TotalCount.ToString();
                }
            }
            return totalcount;

        }

        public string AssignUpdate(string FileID, string assignedUser)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET AssignedTo = @assignedUser WHERE ID = @FileID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@assignedUser",
                    DbType = DbType.String,
                    Value = assignedUser,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.String,
                    Value = FileID,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string MultiAssignUpdate(string FileID, string assignedUser)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET AssignedTo = @assignedUser WHERE ID IN (" + FileID + ")";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@assignedUser",
                    DbType = DbType.String,
                    Value = assignedUser,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.String,
                    Value = FileID,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string MultiAssignDateUpdate(string FileID)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files SET Deadline = @Deadline,DeadlineTime=@DeadlineTime WHERE ID IN (" + FileID + ")";


                MySqlCommand cmd = new MySqlCommand(query, conn);
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

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }


    }
}
