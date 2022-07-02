using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class CreditMemoInstance
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int JobID { get; set; }
        public int Approved { get; set; }
        public int ApprovedUID { get; set; }
        public DateTime ApprovedTimestamp { get; set; }
        public string Status { get; set; }
        public int Locked { get; set; }
        public int CreditMemoIDQB { get; set; }
        public string BillingContactID { get; set; }
        public string ContactID { get; set; }
        #endregion
        public string Tax { get; set; }
        public decimal CreditMemoAmount { get; set; }
        public decimal PretaxAmount { get; set; }
        public string Company { get; set; }
        public DateTime LastUpdated { get; set; }
        public int CreditMemoID { get; set; }
        public string databasename { get; set; }
        public CreditMemoInstance GetCreditMemoInstanceByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * from credit_memo_instance WHERE JobID=@JobID ORDER BY ID DESC Limit 1";
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
                    return ConvertDataTable<CreditMemoInstance>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public CreditMemoInstance GetCreditMemoInstanceByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * from credit_memo_instance WHERE ID=@ID ORDER BY ID DESC Limit 1";
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
                    return ConvertDataTable<CreditMemoInstance>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public List<CreditMemoInstance> GetApprovedCreditNotesListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord, string searchstr,string State, string param1, string startdate, string enddate)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "";
                var countQry = "";
                
                if (startdate != null)
                {
                    if (enddate == null)
                    {
                        enddate = startdate;
                    }

                            myqry = " SELECT a.*,d.CreditMemoAmount,b.tax,b.CreditMemoID,d.LastUpdated,cl3.Company FROM DBNAME.credit_memo_instance a INNER JOIN " +
                                    " (SELECT InvoiceID, Tax, CreditMemoID FROM DBNAME.credit_memo_tmp c Group By CreditMemoID) b ON a.ID=b.CreditMemoID INNER JOIN " +
                                    " (SELECT *, LastUpdated AS LastUpdatedCreditMemo FROM DBNAME.credit_memo_tracking ) d ON a.ID=d.CreditMemoID INNER JOIN " +
                                    " (SELECT * from (Select * from DBNAME.jobs) as cl) cl1 ON a.JobID=cl1.ID INNER JOIN " +
                                    " (SELECT * from (Select * from DBNAME.clients) as cl2) cl3 ON cl1.ClientID=cl3.ID " +
                                    " WHERE a.Approved=1 AND  d.LastUpdated >= '" + startdate + "' AND d.LastUpdated <= '" + enddate + "' + INTERVAL 1 DAY ";
                }
                         
                else
                {
                            myqry = " SELECT a.*,d.CreditMemoAmount,b.tax,b.CreditMemoID,d.LastUpdated,cl3.Company FROM DBNAME.credit_memo_instance a INNER JOIN " +
                                    " (SELECT InvoiceID, Tax, CreditMemoID FROM DBNAME.credit_memo_tmp c Group By CreditMemoID) b ON a.ID=b.CreditMemoID INNER JOIN " +
                                    " (SELECT *, LastUpdated AS LastUpdatedCreditMemo FROM DBNAME.credit_memo_tracking ) d ON a.ID=d.CreditMemoID INNER JOIN " +
                                    " (SELECT * from (Select * from DBNAME.jobs) as cl) cl1 ON a.JobID=cl1.ID INNER JOIN " +
                                    " (SELECT * from (Select * from DBNAME.clients) as cl2) cl3 ON cl1.ClientID=cl3.ID " +
                                    " WHERE a.Approved = 1 ";

                }

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (a.invoiceID LIKE '%" + SearchKey + "%'  OR (CONCAT(YEAR(ClientCreated),'-',cl3.Country,'-',Code,'-',a.ID) Like '%" + SearchKey + "%' ))";

                }

                string myqry3 = "";
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select * from (" + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " + myqry3;

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

                if (startdate != null)
                {
                    if (enddate == null)
                    {
                        enddate = startdate;
                    }

                    countQry = " SELECT a.*,d.CreditMemoAmount,b.tax,b.CreditMemoID,d.LastUpdated,cl3.Company FROM DBNAME.credit_memo_instance a INNER JOIN " +
                            " (SELECT InvoiceID, Tax, CreditMemoID FROM DBNAME.credit_memo_tmp c Group By CreditMemoID) b ON a.ID=b.CreditMemoID INNER JOIN " +
                            " (SELECT *, LastUpdated AS LastUpdatedCreditMemo FROM DBNAME.credit_memo_tracking ) d ON a.ID=d.CreditMemoID INNER JOIN " +
                            " (SELECT * from (Select * from DBNAME.jobs) as cl) cl1 ON a.JobID=cl1.ID INNER JOIN " +
                            " (SELECT * from (Select * from DBNAME.clients) as cl2) cl3 ON cl1.ClientID=cl3.ID " +
                            " WHERE a.Approved=1 AND  d.LastUpdated >= '" + startdate + "' AND d.LastUpdated <= '" + enddate + "' + INTERVAL 1 DAY ";
                }

                else
                {
                    countQry = " SELECT a.*,d.CreditMemoAmount,b.tax,b.CreditMemoID,d.LastUpdated,cl3.Company FROM DBNAME.credit_memo_instance a INNER JOIN " +
                            " (SELECT InvoiceID, Tax, CreditMemoID FROM DBNAME.credit_memo_tmp c Group By CreditMemoID) b ON a.ID=b.CreditMemoID INNER JOIN " +
                            " (SELECT *, LastUpdated AS LastUpdatedCreditMemo FROM DBNAME.credit_memo_tracking ) d ON a.ID=d.CreditMemoID INNER JOIN " +
                            " (SELECT * from (Select * from DBNAME.jobs) as cl) cl1 ON a.JobID=cl1.ID INNER JOIN " +
                            " (SELECT * from (Select * from DBNAME.clients) as cl2) cl3 ON cl1.ClientID=cl3.ID " +
                            " WHERE a.Approved = 1 ";

                }

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (a.invoiceID LIKE '%" + SearchKey + "%'  OR (CONCAT(YEAR(ClientCreated),'-',cl3.Country,'-',Code,'-',a.ID) Like '%" + SearchKey + "%' ))";

                }


                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select COUNT(*) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;
                    return Utility.ConvertDataTable<CreditMemoInstance>(dt);
                }
                else
                {
                    return new List<CreditMemoInstance>();
                }
            }
        }


        public string UpdateByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE credit_memo_instance SET Status = @Status WHERE JobID = @JobID AND Status = @OldStatus";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = "PENDING",
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@OldStatus",
                    DbType = DbType.String,
                    Value = "WAITING",
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@OldLockedStatus",
                    DbType = DbType.Int32,
                    Value = 0,
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
    }
}
