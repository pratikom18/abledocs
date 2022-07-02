using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class InvoiceInstance
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int rowID { get; set; }
        public Int64 InvoiceID { get; set; }
        public Int64 JobID { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public Int64 Approved { get; set; }
        public Int64 ApprovedUID { get; set; }
        public DateTime ApprovedTimestamp { get; set; }
        public Int64 VarianceApproved { get; set; }
        public Int64 VarianceApprovedUID { get; set; }
        public DateTime VarianceApprovedTimestamp { get; set; }
        public string Status { get; set; }
        public Int64 LockedMode { get; set; }
        public Int64 InvoiceIDQB { get; set; }
        public string BillingContactID { get; set; }
        public string ContactID { get; set; }
        public int InvoiceSentBack { get; set; }

        public string clientSince { get; set; }

        public DateTime clientDate { get; set; }

        public DateTime LastUpdated { get; set; }
        public string clientSinceExplode { get; set; }
        public string clientSinceYear { get; set; }
        public string clientSinceMonth { get; set; }
        public string EngagementNum { get; set; }
        public string contact { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime EndDate { get; set; }
        public Decimal QuoteAmount { get; set; }
        public string Tax { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PretaxAmount { get; set; }
        public decimal justTax { get; set; }
        public Int64 CreditMemoID { get; set; }
        public decimal CreditMemoAmount { get; set; }
        public string countryname { get; set; }
        public string currency { get; set; }
        public string databasename { get; set; }
        public decimal total { get; set; }
        public Int64 Years { get; set; }
        public string Notes { get; set; }
        public string Datepaid { get; set; }
        public string fullname { get; set; }
        public string Product { get; set; }
        public Int64 documents { get; set; }
        public decimal totalPages { get; set; }
        public decimal netwithhst { get; set; }
        public decimal hstamount { get; set; }
        public decimal foregincurrencyvalue { get; set; }

        #endregion


        public List<InvoiceInstance> GetInvoiceInstanceAlreadyPopulated(Int64 ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT invoice_instance.ID as rowID FROM invoice_instance INNER JOIN invoice_tmp ON invoice_instance.InvoiceID = invoice_tmp.InvoiceID WHERE invoice_tmp.InvoiceID=" + ID;
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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                else
                    return null;
            }
        }

        public InvoiceInstance GetInvoiceInstanceByJobID(int jobid)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM invoice_instance WHERE InvoiceID = (SELECT InvoiceID FROM invoice_tmp WHERE JobID=" + jobid + " LIMIT 1)";
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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                else
                    return null;
            }
        }
        public InvoiceInstance GetInvoiceInstanceByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,CompanyName,Address1,Address2,ClientFirstName,ClientLastName,Email,Telephone,City,Province,Country,PostalCode,InvoiceIDQB from invoice_instance WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                BindId(cmd);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<InvoiceInstance>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public InvoiceInstance GetInvoiceInstanceByInvoiceID(int InvoiceID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * from invoice_instance WHERE InvoiceID=" + InvoiceID;
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
                    return ConvertDataTable<InvoiceInstance>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string InvoiceInstanceInsert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Insert into invoice_instance(InvoiceID, JobID, CompanyName, Address1, Address2, ClientFirstName, ClientLastName, Email, Telephone, City, Province, Country, PostalCode, BillingContactID, ContactID) " +
                      "Values(@InvoiceID, @JobID, @CompanyName, @Address1, @Address2, @ClientFirstName, @ClientLastName, @Email, @Telephone, @City, @Province, @Country, @PostalCode, @BillingContactID, @ContactID)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string InvoiceInstanceUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_instance SET CompanyName = @CompanyName, Address1 = @Address1, Address2 = @Address2, ClientFirstName = @ClientFirstName, ClientLastName = @ClientLastName, Email = @Email, Telephone = @Telephone, City = @City, Province = @Province, Country = @Country, PostalCode = @PostalCode, BillingContactID = @BillingContactID, ContactID = @ContactID WHERE InvoiceID = @InvoiceID AND JobID = @JobID";


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

        public string JobInvoiceContactUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_instance SET CompanyName = @CompanyName, Address1 = @Address1, Address2 = @Address2,ClientFirstName = @ClientFirstName," +
                    " ClientLastName = @ClientLastName,Email = @Email, Telephone = @Telephone, " +
                    " City = @City, Province = @Province, Country = @Country, PostalCode = @PostalCode WHERE ID = @ID";


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
                ParameterName = "@InvoiceID",
                DbType = DbType.Int32,
                Value = InvoiceID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CompanyName",
                DbType = DbType.String,
                Value = CompanyName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Address1",
                DbType = DbType.String,
                Value = Address1,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Address2",
                DbType = DbType.String,
                Value = Address2,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientFirstName",
                DbType = DbType.String,
                Value = ClientFirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientLastName",
                DbType = DbType.String,
                Value = ClientLastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Email",
                DbType = DbType.String,
                Value = Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Telephone",
                DbType = DbType.String,
                Value = Telephone,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@City",
                DbType = DbType.String,
                Value = City,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Province",
                DbType = DbType.String,
                Value = Province,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Country",
                DbType = DbType.String,
                Value = Country,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PostalCode",
                DbType = DbType.String,
                Value = PostalCode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@BillingContactID",
                DbType = DbType.String,
                Value = BillingContactID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ContactID",
                DbType = DbType.String,
                Value = ContactID,
            });
        }

        public List<InvoiceInstance> GetGlobalSales()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT a.Country,SUM(InvoiceAmount) as InvoiceAmount,r.country as countryname  FROM DBNAME.invoice_instance a " +
                           " INNER JOIN (SELECT * FROM (SELECT *, LastUpdated AS LastUpdatedInv FROM DBNAME.invoice_tracking WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID) b ON a.JobID=b.JobID " +
                           " LEFT JOIN (SELECT * FROM (SELECT * FROM DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID) q ON a.JobID=q.JobID " +
                           " INNER JOIN (SELECT * FROM (SELECT * FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID=l.JobID " +
                           " LEFT JOIN (SELECT  ID, JobQuotedAs FROM DBNAME.jobs) j ON j.ID=a.JobID " +
                           " left join (select  code,country from COMMONDBNAME.countries )r on a.Country=r.code " +
                           " WHERE a.Approved=0 AND a.Status='INVOICEPENDING' AND a.InvoiceSentBack=0 " +
                           " Group by a.Country ";

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select Country,SUM(InvoiceAmount) as InvoiceAmount,countryname from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by Country";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }

            }


        }
        public List<Clients> GetInvoiceInstanceList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string AlphaSearch, out int TotalRecord, string StartDate, string EndDate)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = " SELECT CONCAT(CASE WHEN MONTH(cl3.ClientCreated) >= 11 and MONTH(cl3.ClientCreated) <= 12 THEN YEAR(cl3.ClientCreated) + 1 ELSE YEAR(cl3.ClientCreated) END, '-', cl3.Country, '-', cl3.Code, '-', a.jobID) as EngagementNum, b.LastUpdated, a.JobID ";
                myqry += " FROM invoice_instance a INNER JOIN(SELECT * from(Select *, LastUpdated AS LastUpdatedInv from invoice_tracking WHERE OperationType = 'InvoiceGenerated' ";
                myqry += " ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID = b.JobID INNER JOIN(SELECT * from (Select* from quote_tracking";
                myqry += " WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID = q.JobID";
                myqry += " INNER JOIN(SELECT * from (Select * from invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID = l.JobID";
                myqry += " INNER JOIN(SELECT * from (Select * from jobs) as cl) cl1 ON a.JobID = cl1.ID INNER JOIN(SELECT * from (Select * from clients) as cl2) cl3";
                myqry += " ON cl1.ClientID = cl3.ID WHERE a.Approved = 1 AND b.LastUpdated >= '" + StartDate + "' AND b.LastUpdated <= '" + EndDate + "' + INTERVAL 1 DAY";

                if (SearchKey != "")
                {
                    //myqry = myqry + " AND (EngagementNum LIKE '%" + SearchKey + "%'" +
                    myqry = myqry + " AND CONCAT(CASE WHEN MONTH(cl3.ClientCreated) >= 11 and MONTH(cl3.ClientCreated) <= 12 THEN YEAR(cl3.ClientCreated) + 1 ELSE YEAR(cl3.ClientCreated) END, '-', cl3.Country, '-', cl3.Code, '-', a.jobID) LIKE '%" + SearchKey + "%'" +
                            " OR ( a.JobID LIKE '%" + SearchKey + "%'" +
                    //" OR contact LIKE '%" + SearchKey + "%'" +
                            " OR  b.LastUpdated LIKE '%" + SearchKey + "%'" +
                            ")";
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

                var countQry = "SELECT COUNT(*) FROM invoice_instance";



                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    //return ConvertDataTable<InvoiceInstance>(dt);
                    return ConvertDataTable<Clients>(dt);

                }
                else
                {
                    return new List<Clients>();
                }
            }

        }

        public List<InvoiceInstance> GetInvoiceInstanceStatusListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, int TodayTask)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                if (Status == "INVOICEPENDING")
                {
                    Where_Status = "invoice_instance.Status = 'INVOICEPENDING'";
                }

                string myqry = "";

                myqry = "SELECT invoice_instance.ID,invoice_instance.JOBID,CONCAT(YEAR(clients.ClientCreated), '-', clients.Country, '-', clients.Code, '-', jobs.ID) as EngagementNum,'DBNAME' AS databasename FROM DBNAME.invoice_instance inner join DBNAME.jobs on jobs.ID = invoice_instance.JobID left join DBNAME.clients on clients.Id = jobs.ClientID where " + Where_Status;

                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    myqry = myqry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (invoice_instance.ID LIKE '%" + SearchKey + "%'" +
                   "OR CONCAT(YEAR(clients.ClientCreated), '-', clients.Country, '-', clients.Code, '-', jobs.ID) like '%" + SearchKey + "%')";

                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {

                    myqry3 = myqry3 + " ORDER BY EngagementNum";

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

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();

                int count = 0;

                var countQry = "";

                countQry = "SELECT invoice_instance.ID,invoice_instance.JOBID,CONCAT(YEAR(clients.ClientCreated), '-', clients.Country, '-', clients.Code, '-', jobs.ID) as EngagementNum FROM DBNAME.invoice_instance inner join DBNAME.jobs on jobs.ID = invoice_instance.JobID left join DBNAME.clients on clients.Id = jobs.ClientID where " + Where_Status;

                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    countQry = countQry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    countQry = countQry + " AND (invoice_instance.ID LIKE '%" + SearchKey + "%'" +
                   "OR CONCAT(YEAR(clients.ClientCreated), '-', clients.Country, '-', clients.Code, '-', jobs.ID) like '%" + SearchKey + "%')";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }
            }
        }

        public List<InvoiceInstance> GetToBeInvoicedListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string Status, out int TotalRecord, int TodayTask)
        {
            string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (Status == "INVOICEPENDING")
                {
                    Where_Status = "invoice_instance.Status = 'INVOICEPENDING'";
                }

                string myqry = "";

                myqry = "Select jobs.ID, jobs.Created, jobs.Status, jobs.QuoteFlag, jobs.Deadline, jobs.DeadlineTime," +
                    " jobs.JobType, jobs.Notes, jobs.QuoteOnly, jobs.JobQuotedAs,jobs.VarianceComment,clients.Company, " +
                    "clients.OldCode, clients.Code, clients.ClientSince, clients.Country, clients.ClientCreated, " +
                    "invoice_instance.InvoiceSentBack From jobs  LEFT JOIN clients ON clients.ID = jobs.ClientID  " +
                    "LEFT JOIN invoice_instance ON jobs.ID = invoice_instance.JobID Where jobs.ID NOT IN(Select it.JobID from " +
                    "invoice_tracking it LEFT JOIN invoice_instance ii ON it.JobID = ii.JobID Where it.OperationType = 'InvoiceGenerated' " +
                    "AND ii.InvoiceSentBack <> 1) AND jobs.Status <> 'CLOSED' AND jobs.Status <> 'INVOICEPENDING' AND jobs.Status <> 'CANCELLED'" +
                    " Order By FIELD(jobs.Status, 'DELIVERED', 'TOBEDELIVERED', 'OPEN', 'PENDING')";

                if (TodayTask == 1)
                {
                    var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    myqry = myqry + " AND j.Deadline = '" + TodayDate + "'";
                }
                if (SearchKey != "")
                {
                    //myqry = myqry + " AND (invoice_instance.ID LIKE '%" + SearchKey + "%'" +

                    //   "OR CONCAT(YEAR(ClientCreated), '-', Country, '-', Code, '-', jobs.ID) like '%" + SearchKey + "%'" +

                    //    ")";

                    //myqry += " AND (CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jobs.ID) like '%" + SearchKey + "%'";

                    //myqry = "SELECT * from invoice_instance";
                    //myqry += " (Select CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jobs.ID) from clients ";
                    //myqry += " RIGHT JOIN jobs ON jobs.ClientID = clients.ID Where jobs.ID = JOBID) EngagementNum";
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

                //var countQry = "SELECT COUNT(*) FROM invoice_instance  WHERE " + Where_Status;
                var countQry = "SELECT COUNT(jobs_files.ID) as fileTotal, SUM(jobs_files.Pages) as sumPage from jobs_files where JobID=ID";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }
            }
        }

        public List<InvoiceInstance> GetApprovedInvoicesListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord, string searchstr, string startdate, string enddate)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "";


                if (startdate != null)
                {

                    if (enddate == null)
                    {
                        enddate = startdate;
                    }

                    myqry = " SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax , (Select CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jb.ID) from DBNAME.clients RIGHT JOIN DBNAME.jobs jb ON jb.ClientID = clients.ID  Where jb.ID = a.JobID) EngagementNum FROM DBNAME.invoice_instance a INNER JOIN " +
                           " (SELECT * from (Select *, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID=b.JobID LEFT JOIN " +
                           " (SELECT * from (Select * from DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID=q.JobID INNER JOIN " +
                           " (SELECT * from (Select * from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID=l.JobID INNER JOIN " +
                           " (SELECT * from (Select * from DBNAME.jobs) as cl) cl1 ON a.JobID=cl1.ID INNER JOIN " +
                           " (SELECT * from (Select * from DBNAME.clients) as cl2) cl3 ON cl1.ClientID=cl3.ID " +
                           " WHERE a.Approved=1 AND  b.LastUpdated >= '" + startdate + "' AND b.LastUpdated <= '" + enddate + "' + INTERVAL 1 DAY ";


                }
                else
                {
                    myqry = " SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax  , (Select CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jb.ID) from DBNAME.clients RIGHT JOIN DBNAME.jobs jb ON jb.ClientID = clients.ID  Where jb.ID = a.JobID) EngagementNum FROM DBNAME.invoice_instance a INNER JOIN (SELECT * from " +
                            " (Select *, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking WHERE " +
                            " OperationType = 'InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b " +
                            " ON a.JobID = b.JobID LEFT JOIN(SELECT* from (Select* from DBNAME.quote_tracking WHERE " +
                            " OperationType = 'QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q " +
                            " ON a.JobID = q.JobID INNER JOIN(SELECT* from (Select* from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l" +
                            " ON a.JobID = l.JobID INNER JOIN(SELECT* from (Select* from DBNAME.jobs) as cl) cl1 ON a.JobID = cl1.ID " +
                            " INNER JOIN (SELECT* from (Select* from DBNAME.clients) as cl2) cl3 ON cl1.ClientID = cl3.ID " +
                            " WHERE a.Approved = 1 ";

                }

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (a.invoiceID LIKE '%" + SearchKey + "%'  OR (CONCAT(YEAR(ClientCreated),'-',cl3.Country,'-',Code,'-',a.JobID) Like '%" + SearchKey + "%' ))";

                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 = myqry3 + " ORDER BY InvoiceID ASC";
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

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();

                int count = 0;

                string countQry = "";
                if (startdate != null)
                {

                    if (enddate == null)
                    {
                        enddate = startdate;
                    }

                    countQry = " SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax  , (Select CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jb.ID) from DBNAME.clients RIGHT JOIN DBNAME.jobs jb ON jb.ClientID = clients.ID  Where jb.ID = a.JobID) EngagementNum FROM DBNAME.invoice_instance a INNER JOIN " +
                           " (SELECT * from (Select *, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID=b.JobID LEFT JOIN " +
                           " (SELECT * from (Select * from DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID=q.JobID INNER JOIN " +
                           " (SELECT * from (Select * from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID=l.JobID INNER JOIN " +
                           " (SELECT * from (Select * from DBNAME.jobs) as cl) cl1 ON a.JobID=cl1.ID INNER JOIN " +
                           " (SELECT * from (Select * from DBNAME.clients) as cl2) cl3 ON cl1.ClientID=cl3.ID " +
                           " WHERE a.Approved=1 AND  b.LastUpdated >= '" + startdate + "' AND b.LastUpdated <= '" + enddate + "' + INTERVAL 1 DAY ";


                }
                else
                {
                    countQry = " SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax   , (Select CONCAT(YEAR(ClientCreated),'-',Country,'-',Code,'-',jb.ID) from DBNAME.clients RIGHT JOIN DBNAME.jobs jb ON jb.ClientID = clients.ID  Where jb.ID = a.JobID) EngagementNum FROM DBNAME.invoice_instance a INNER JOIN (SELECT * from " +
                            " (Select *, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking WHERE " +
                            " OperationType = 'InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b " +
                            " ON a.JobID = b.JobID LEFT JOIN(SELECT* from (Select* from DBNAME.quote_tracking WHERE " +
                            " OperationType = 'QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q " +
                            " ON a.JobID = q.JobID INNER JOIN(SELECT* from (Select* from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l" +
                            " ON a.JobID = l.JobID INNER JOIN(SELECT* from (Select* from DBNAME.jobs) as cl) cl1 ON a.JobID = cl1.ID " +
                            " INNER JOIN (SELECT* from (Select* from DBNAME.clients) as cl2) cl3 ON cl1.ClientID = cl3.ID " +
                            " WHERE a.Approved = 1 ";

                }

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (a.invoiceID LIKE '%" + SearchKey + "%'  OR (CONCAT(YEAR(ClientCreated),'-',cl3.Country,'-',Code,'-',a.JobID) Like '%" + SearchKey + "%' ))";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }
            }
        }

        public List<InvoiceInstance> GetPendingInvoicedListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord, string state, string param1)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "";
                string startDate = string.Empty;
                string endDate = string.Empty;

                if (!string.IsNullOrEmpty(param1))
                {
                    string dateRange = param1;
                    string[] dateRangeExplode = dateRange.Split("-");
                    startDate = dateRangeExplode[0];
                    endDate = dateRangeExplode[1];

                    myqry = "SELECT SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax,con.currency as currency,'DBNAME' as databasename FROM DBNAME.invoice_instance a INNER JOIN" +
                            " (SELECT * from (Select *, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID=b.JobID LEFT JOIN " +
                            " (SELECT * from (Select * from DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID=q.JobID INNER JOIN " +
                            " (SELECT * from (Select * from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID=l.JobID LEFT JOIN " +
                            " (SELECT ID, JobQuotedAs from DBNAME.jobs) j ON j.ID=a.JobID " +
                            " left join (select * from  COMMONDBNAME.countries ) con on con.code=a.Country " +
                            " WHERE a.Approved=0 AND a.Status='INVOICEPENDING' AND a.InvoiceSentBack=0 ";

                }
                else
                {
                    myqry = " SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax, con.currency as currency, " +
                              " ( " +
                              " Select CONCAT(YEAR(f.ClientCreated),'-',COALESCE(f.Country,''),'-',f.Code,'-',jb.ID)   from DBNAME.clients f  RIGHT JOIN DBNAME.jobs jb ON jb.ClientID = f.ID " +
                              " Where jb.ID = a.JobID " +
                              "  ) EngagementNum ,'DBNAME' as databasename" +
                             " FROM DBNAME.invoice_instance a " +
                             " INNER JOIN  (SELECT * from (Select *,InvoiceAmount as InvoiceAmount1, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking " +
                             "  WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID=b.JobID  " +
                             " LEFT JOIN  (SELECT * from (Select * from DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID=q.JobID  " +
                             " left join DBNAME.clients cl on cl.ID=a.JobID " +
                             " INNER JOIN  (SELECT * from (Select * from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID=l.JobID LEFT JOIN  (SELECT ID, JobQuotedAs from DBNAME.jobs) j ON j.ID=a.JobID  " +
                             " left join (select * from  COMMONDBNAME.countries ) con on con.code=a.Country  " +
                             " WHERE a.Approved=0 AND a.Status='INVOICEPENDING' AND a.InvoiceSentBack=0  ";

                }

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (a.invoiceID LIKE '%" + SearchKey + "%' OR CONCAT(YEAR(cl.ClientCreated), '-', COALESCE(cl.Country,''), '-', cl.Code, '-', a.jobID) like '%" + SearchKey + "%')";
                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 = myqry3 + " ORDER BY InvoiceID ASC";
                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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


                var countQry = "";

                if (!string.IsNullOrEmpty(param1))
                {
                    string dateRange = param1;
                    string[] dateRangeExplode = dateRange.Split("-");
                    startDate = dateRangeExplode[0];
                    endDate = dateRangeExplode[1];

                    countQry = "SELECT SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax,con.currency as currency FROM DBNAME.invoice_instance a INNER JOIN" +
                            " (SELECT * from (Select *, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID=b.JobID LEFT JOIN " +
                            " (SELECT * from (Select * from DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID=q.JobID INNER JOIN " +
                            " (SELECT * from (Select * from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID=l.JobID LEFT JOIN " +
                            " (SELECT ID, JobQuotedAs from DBNAME.jobs) j ON j.ID=a.JobID " +
                            " left join (select * from  COMMONDBNAME.countries ) con on con.code=a.Country " +
                            " WHERE a.Approved=0 AND a.Status='INVOICEPENDING' AND a.InvoiceSentBack=0 ";

                }
                else
                {
                    countQry = " SELECT a.*,b.InvoiceAmount,b.LastUpdated,q.QuoteAmount,l.Tax, con.currency as currency, " +
                              " ( " +
                              " Select CONCAT(YEAR(f.ClientCreated),'-',COALESCE(f.Country,''),'-',f.Code,'-',jb.ID)   from DBNAME.clients f  RIGHT JOIN DBNAME.jobs jb ON jb.ClientID = f.ID " +
                              " Where jb.ID = a.JobID " +
                              "  ) EngagementNum " +
                             " FROM DBNAME.invoice_instance a " +
                             " INNER JOIN  (SELECT * from (Select *,InvoiceAmount as InvoiceAmount1, LastUpdated AS LastUpdatedInv from DBNAME.invoice_tracking " +
                             "  WHERE OperationType='InvoiceGenerated' ORDER BY LastUpdated DESC) as c GROUP BY c.JobID) b ON a.JobID=b.JobID  " +
                             " LEFT JOIN  (SELECT * from (Select * from DBNAME.quote_tracking WHERE OperationType='QuoteGenerated' ORDER BY LastUpdated DESC) as qt GROUP BY qt.JobID) q ON a.JobID=q.JobID  " +
                             " left join DBNAME.clients cl on cl.ID=a.JobID " +
                             " INNER JOIN  (SELECT * from (Select * from DBNAME.invoice_tmp) as it GROUP BY it.JobID) l ON a.JobID=l.JobID LEFT JOIN  (SELECT ID, JobQuotedAs from DBNAME.jobs) j ON j.ID=a.JobID  " +
                             " left join (select * from  COMMONDBNAME.countries ) con on con.code=a.Country  " +
                             " WHERE a.Approved=0 AND a.Status='INVOICEPENDING' AND a.InvoiceSentBack=0  ";

                }

                if (SearchKey != "")
                {
                    countQry = countQry + " AND (a.invoiceID LIKE '%" + SearchKey + "%' OR CONCAT(YEAR(cl.ClientCreated), '-', COALESCE(cl.Country,''), '-', cl.Code, '-', a.jobID) like '%" + SearchKey + "%')";
                }


                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }
            }
        }

        public List<InvoiceInstance> GetPendingCreditNotesListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord, string State, string param1)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "";


                if (!string.IsNullOrEmpty(param1))
                {
                    string startdate = "";
                    string enddate = "";
                    myqry = " SELECT a.*,b.CreditMemoAmount,b.LastUpdated,b.CreditMemoID FROM DBNAME.credit_memo_instance a" +
                           " INNER JOIN DBNAME.credit_memo_tracking b ON a.ID=b.CreditMemoID " +
                           " WHERE a.Approved=0 AND a.Status='PENDING' and AND b.LastUpdated >= '" + startdate + "' AND b.LastUpdated <= '" + enddate + "' + INTERVAL 1 DAY ";

                }
                else
                {

                    myqry = " SELECT a.*,b.CreditMemoAmount,b.LastUpdated,b.CreditMemoID FROM DBNAME.credit_memo_instance a" +
                            " INNER JOIN DBNAME.credit_memo_tracking b ON a.ID=b.CreditMemoID " +
                            " WHERE a.Approved=0 AND a.Status='PENDING'";

                }



                if (SearchKey != "")
                {
                    myqry = myqry + " AND (a.invoiceID LIKE '%" + SearchKey + "%')";

                }

                string myqry3 = "";

                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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

                if (!string.IsNullOrEmpty(param1))
                {
                    string startdate = "";
                    string enddate = "";
                    countQry = " SELECT a.*,b.CreditMemoAmount,b.LastUpdated,b.CreditMemoID FROM DBNAME.credit_memo_instance a" +
                           " INNER JOIN DBNAME.credit_memo_tracking b ON a.ID=b.CreditMemoID " +
                           " WHERE a.Approved=0 AND a.Status='PENDING' and AND b.LastUpdated >= '" + startdate + "' AND b.LastUpdated <= '" + enddate + "' + INTERVAL 1 DAY ";

                }
                else
                {

                    countQry = " SELECT a.*,b.CreditMemoAmount,b.LastUpdated,b.CreditMemoID FROM DBNAME.credit_memo_instance a" +
                            " INNER JOIN DBNAME.credit_memo_tracking b ON a.ID=b.CreditMemoID " +
                            " WHERE a.Approved=0 AND a.Status='PENDING'";

                }




                if (SearchKey != "")
                {
                    countQry = countQry + " AND (a.invoiceID LIKE '%" + SearchKey + "%')";

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
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }
            }
        }

        public string UpdateStatusByInvoiceID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_instance SET Status = @Status, InvoiceSentBack = @InvoiceSentBack WHERE InvoiceID = @InvoiceID";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Status",
                    DbType = DbType.String,
                    Value = Status,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@InvoiceSentBack",
                    DbType = DbType.Int32,
                    Value = InvoiceSentBack,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@InvoiceID",
                    DbType = DbType.Int32,
                    Value = InvoiceID,
                });


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public InvoiceInstance GetInvoiceInstanceCompanyName()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select CompanyName FROM invoice_instance a INNER JOIN credit_memo_instance b ON a.InvoiceID=b.InvoiceID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                BindId(cmd);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<InvoiceInstance>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public InvoiceInstance GetInvoiceInstanceByInvoiceIDJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * from invoice_instance Where InvoiceID=@InvoiceID And JobID=@JobID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@InvoiceID",
                    DbType = DbType.Int32,
                    Value = InvoiceID,
                });
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
                    return ConvertDataTable<InvoiceInstance>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string InvoiceInstanceUpdateByInvoiceID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_instance SET InvoiceIDQB = @InvoiceIDQB, InvoiceSentBack = @InvoiceSentBack, Approved = @Approved, ApprovedUID = @ApprovedUID, ApprovedTimestamp = @ApprovedTimestamp, VarianceApproved = @VarianceApproved, VarianceApprovedUID = @VarianceApprovedUID, VarianceApprovedTimestamp = @VarianceApprovedTimestamp, Status = @Status WHERE InvoiceID = @InvoiceID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParamsUpdate(cmd);

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParamsUpdate(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@InvoiceID",
                DbType = DbType.Int32,
                Value = InvoiceID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@InvoiceIDQB",
                DbType = DbType.Int32,
                Value = InvoiceIDQB,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@InvoiceSentBack",
                DbType = DbType.Int32,
                Value = InvoiceSentBack,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Approved",
                DbType = DbType.Int64,
                Value = Approved,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ApprovedUID",
                DbType = DbType.Int64,
                Value = ApprovedUID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ApprovedTimestamp",
                DbType = DbType.DateTime,
                Value = ApprovedTimestamp,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VarianceApproved",
                DbType = DbType.Int64,
                Value = VarianceApproved,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VarianceApprovedUID",
                DbType = DbType.Int64,
                Value = VarianceApprovedUID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VarianceApprovedTimestamp",
                DbType = DbType.DateTime,
                Value = VarianceApprovedTimestamp,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Status",
                DbType = DbType.String,
                Value = Status,
            });

        }

        public List<InvoiceInstance> GetGlobalInvoice()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT SUM(b.InvoiceAmount) AS InvoiceAmount ,YEAR(b.LastUpdated) AS Years FROM DBNAME.invoice_instance a  " +
                        "INNER JOIN(SELECT* FROM (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                        "FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                       // "LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                        //"LEFT JOIN DBNAME.clients cl ON cl.ID = a.JobID " +
                        "INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                        //"LEFT JOIN(SELECT ID, JobQuotedAs FROM DBNAME.jobs) j ON j.ID = a.JobID " +
                        "WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING' AND a.InvoiceSentBack = 0  AND YEAR(b.LastUpdated) BETWEEN '" + (Convert.ToInt32(DateTime.Today.Year) - 1).ToString() + "' AND '" + DateTime.Today.Year + "' " +
                        "GROUP BY YEAR(b.LastUpdated)";

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(InvoiceAmount) as InvoiceAmount,Years from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by Years";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }

            }


        }

        public InvoiceInstance GetPreviousDailyMaxJobs()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT COUNT(ID) AS total, DATE(created) created FROM DBNAME.jobs where YEAR(created) =" + (Convert.ToInt32(DateTime.Today.Year) - 1).ToString() + " GROUP BY DATE(created)";

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(total) as total,created from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by DATE(created) ORDER BY total DESC LIMIT 1 ;";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }


        }

        public InvoiceInstance GetDailyMaxJobs()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT COUNT(ID) AS total, DATE(created) created FROM DBNAME.jobs where YEAR(created) =" + DateTime.Today.Year + " GROUP BY DATE(created)";

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(total) as total,created from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by DATE(created) ORDER BY total DESC LIMIT 1 ;";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }


        }

        public InvoiceInstance GetWeeklyMaxJobs()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = "SELECT created,WEEK,SUM(total) AS total FROM (SELECT DATE(created) AS created, WEEK(created) AS WEEK, COUNT(id) AS total FROM DBNAME.jobs where YEAR(created) =" + DateTime.Today.Year + " GROUP BY DATE(created),WEEK(created) ORDER BY total DESC) AS a GROUP BY YEAR(created),WEEK(created)";

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "SELECT created,WEEK,SUM(total) AS total FROM ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) AS a GROUP BY YEAR(created),WEEK(created) ORDER BY total DESC LIMIT 1";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }


        }

        public InvoiceInstance GetDailyMaxFile()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT COUNT(ID) AS total, DATE(created) created FROM DBNAME.jobs_files WHERE created IS NOT NULL GROUP BY DATE(created)";//and YEAR(created) =" + DateTime.Today.Year + "

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "SELECT SUM(total) AS total,DATE(created) as created from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) AS a GROUP BY DATE(created) ORDER BY total DESC LIMIT 1";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }


        }

        public List<InvoiceInstance> GetGlobalInvoiceQUARTER()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT SUM(b.InvoiceAmount) AS InvoiceAmount ,QUARTER(b.LastUpdated) AS Years FROM DBNAME.invoice_instance a  " +
                        "INNER JOIN(SELECT* FROM (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                        "FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                        //"LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                        //"LEFT JOIN DBNAME.clients cl ON cl.ID = a.JobID " +
                        "INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                        //"LEFT JOIN(SELECT ID, JobQuotedAs FROM DBNAME.jobs) j ON j.ID = a.JobID " +
                        "WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING' AND a.InvoiceSentBack = 0  AND YEAR(b.LastUpdated) = '" + DateTime.Today.Year + "' " +
                        "GROUP BY QUARTER(b.LastUpdated)";

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(InvoiceAmount) as InvoiceAmount,Years from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by Years";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }

            }


        }

        public List<InvoiceInstance> GetGlobalInvoiceQUARTERPre()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT SUM(b.InvoiceAmount) AS InvoiceAmount ,QUARTER(b.LastUpdated) AS Years FROM DBNAME.invoice_instance a  " +
                        "INNER JOIN(SELECT* FROM (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                        "FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                        //"LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                       // "LEFT JOIN DBNAME.clients cl ON cl.ID = a.JobID " +
                        "INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                        //"LEFT JOIN(SELECT ID, JobQuotedAs FROM DBNAME.jobs) j ON j.ID = a.JobID " +
                        "WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING' AND a.InvoiceSentBack = 0  AND YEAR(b.LastUpdated) = '" + (DateTime.Today.Year - 1).ToString() + "' " +
                        "GROUP BY QUARTER(b.LastUpdated)";

                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(InvoiceAmount) as InvoiceAmount,Years from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by Years";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }

            }


        }

        public InvoiceInstance GetGlobalInvoiceBilling()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT SUM(b.InvoiceAmount) AS InvoiceAmount ,WEEK(b.LastUpdated) AS Years,DATE(b.LastUpdated) AS created FROM DBNAME.invoice_instance a  " +
                        "INNER JOIN(SELECT* FROM (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                        "FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                        //"LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                        //"LEFT JOIN DBNAME.clients cl ON cl.ID = a.JobID " +
                        "INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                        //"LEFT JOIN(SELECT ID, JobQuotedAs FROM DBNAME.jobs) j ON j.ID = a.JobID " +
                        "WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING' AND a.InvoiceSentBack = 0 " +
                        "GROUP BY YEAR(b.LastUpdated),WEEK(b.LastUpdated)";


                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(InvoiceAmount) as InvoiceAmount,Years,created from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a GROUP BY Years,created ORDER BY invoiceamount DESC LIMIT 1";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }


        }

        public List<InvoiceInstance> GetGlobalWeeklySummary()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT a.invoiceid,DATE(b.LastUpdated) AS created,'' AS Notes,'' AS Datepaid," +
                        "CONCAT(COALESCE(YEAR(cl.ClientCreated), ''), '-', COALESCE(cl.Country, ''), '-', COALESCE(cl.Code, ''), '-', COALESCE(a.JobID, ''))EngagementNum," +
                        " CONCAT(cc.FirstName, ' ', cc.LastName) AS fullname," +
                        "'ADService' AS Product," +
                        "COUNT(jf.id) AS documents," +
                        "SUM(jf.Pages) AS totalPages," +
                        "SUM(b.InvoiceAmount) AS InvoiceAmount," +
                        "0 AS netwithhst," +
                        "0 AS hstamount," +
                        "c.currency_code AS currency," +
                        "0 AS foregincurrencyvalue" +
                        " FROM DBNAME.invoice_instance a " +
                        " INNER JOIN(SELECT* FROM " +
                        " (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                        " FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                        " LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                        " LEFT JOIN DBNAME.jobs js ON js.ID = a.JobID  " +
                        " LEFT JOIN DBNAME.clients cl ON cl.ID = js.clientid  " +
                        " INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                        "  LEFT JOIN DBNAME.clients_contacts cc ON cc.id = a.BillingContactID " +
                        "  LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.jobs_files WHERE STATUS<>'REFERENCE' AND Deleted = 0 ) AS it GROUP BY it.JobID) jf ON a.JobID = jf.JobID " +
                        "  LEFT JOIN COMMONDBNAME.countries c ON c.code = a.Country " +
                        " WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING'" +
                        " AND a.InvoiceSentBack = 0 " +
                        " AND YEARWEEK(b.LastUpdated) = YEARWEEK(NOW()) " +
                        " GROUP BY a.invoiceid,DATE(b.LastUpdated) ";

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select invoiceid,created,Notes, Datepaid,EngagementNum,fullname,Product, documents,totalPages,InvoiceAmount,netwithhst,hstamount,currency,foregincurrencyvalue from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by invoiceid,created,EngagementNum";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }

            }


        }

        public List<InvoiceInstance> GetGlobalTotalOffice()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT c.name AS fullname, " +
                            "SUM(b.InvoiceAmount) AS InvoiceAmount " +
                            "FROM DBNAME.invoice_instance a " +
                            "INNER JOIN(SELECT* FROM " +
                            "(SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                            "FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                            "LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                            " LEFT JOIN DBNAME.jobs js ON js.ID = a.JobID  " +
                            " LEFT JOIN DBNAME.clients cl ON cl.ID = js.clientid  " +
                            "INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                            "LEFT JOIN manage_company c ON c.code = cl.officecode " +
                            "WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING' " +
                            "AND a.InvoiceSentBack = 0 " +
                            "GROUP BY c.code";


                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select fullname,SUM(InvoiceAmount) as InvoiceAmount from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by fullname";

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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }

            }


        }

        public InvoiceInstance GetJobWaterMarks()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string myqry = string.Empty;
                myqry = " SELECT jobs AS total, DATE(LastUpdated) created FROM jobhighwatermark";//and YEAR(created) =" + DateTime.Today.Year + "


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
                    return Utility.ConvertDataTable<InvoiceInstance>(dt).FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }


        }



      

        public List<InvoiceInstance> GetGlobalWeeklySummary(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "";


                myqry = " SELECT a.invoiceid,DATE(b.LastUpdated) AS created,'' AS Notes,'' AS Datepaid," +
                       "CONCAT(COALESCE(YEAR(cl.ClientCreated), ''), '-', COALESCE(cl.Country, ''), '-', COALESCE(cl.Code, ''), '-', COALESCE(a.JobID, ''))EngagementNum," +
                       " CONCAT(cc.FirstName, ' ', cc.LastName) AS fullname," +
                       "'ADService' AS Product," +
                       "COUNT(jf.id) AS documents," +
                       "SUM(jf.Pages) AS totalPages," +
                       "SUM(b.InvoiceAmount) AS InvoiceAmount," +
                       "ROUND(SUM(b.InvoiceAmount) + ((SUM(b.InvoiceAmount) * 13) / 100), 2) AS netwithhst,"+
                        "ROUND((SUM(b.InvoiceAmount) * 13) / 100, 2) AS hstamount,"+
                       " c.currency_code AS currency  " +
                       //" foregincurrencyvalue" +
                       " FROM DBNAME.invoice_instance a " +
                       " INNER JOIN(SELECT* FROM " +
                       " (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                       " FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                       " LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                       " LEFT JOIN DBNAME.jobs js ON js.ID = a.JobID  " +
                       " LEFT JOIN DBNAME.clients cl ON cl.ID = js.clientid  " +
                       " INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                       "  LEFT JOIN DBNAME.clients_contacts cc ON cc.id = a.BillingContactID " +
                       "  LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.jobs_files WHERE STATUS<>'REFERENCE' AND Deleted = 0 ) AS it GROUP BY it.JobID) jf ON a.JobID = jf.JobID " +
                       "  LEFT JOIN COMMONDBNAME.countries c ON c.code = a.Country " +
                       " WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING'" +
                       " AND a.InvoiceSentBack = 0 " +
                       " AND YEARWEEK(b.LastUpdated) = YEARWEEK(NOW()) ";


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (a.invoiceID LIKE '%" + SearchKey + "%'  OR (CONCAT(YEAR(ClientCreated),'-',cl3.Country,'-',Code,'-',a.JobID) Like '%" + SearchKey + "%' ))";

                }

                myqry = myqry + " GROUP BY a.invoiceid,DATE(b.LastUpdated) ";

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {
                    myqry3 = myqry3 + " ORDER BY InvoiceID ASC";
                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                //                myqry = "select invoiceid,created,Notes, Datepaid,EngagementNum,fullname,Product, documents,totalPages,InvoiceAmount,netwithhst,hstamount,currency,foregincurrencyvalue from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by invoiceid,created,EngagementNum " + myqry3;
                myqry = "select invoiceid,created,Notes, Datepaid,EngagementNum,fullname,Product, documents,totalPages,InvoiceAmount,netwithhst,hstamount,currency from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a Group by invoiceid,created,EngagementNum " + myqry3;

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

                countQry = " SELECT a.invoiceid,DATE(b.LastUpdated) AS created,'' AS Notes,'' AS Datepaid," +
                       "CONCAT(COALESCE(YEAR(cl.ClientCreated), ''), '-', COALESCE(cl.Country, ''), '-', COALESCE(cl.Code, ''), '-', COALESCE(a.JobID, ''))EngagementNum," +
                       " CONCAT(cc.FirstName, ' ', cc.LastName) AS fullname," +
                       "'ADService' AS Product," +
                       "COUNT(jf.id) AS documents," +
                       "SUM(jf.Pages) AS totalPages," +
                       "SUM(b.InvoiceAmount) AS InvoiceAmount," +
                       "0 AS netwithhst," +
                       "0 AS hstamount," +
                       "c.currency_code AS currency  " +
                       //" foregincurrencyvalue" +
                       " FROM DBNAME.invoice_instance a " +
                       " INNER JOIN(SELECT* FROM " +
                       " (SELECT*, InvoiceAmount AS InvoiceAmount1, LastUpdated AS LastUpdatedInv " +
                       " FROM DBNAME.invoice_tracking WHERE OperationType= 'InvoiceGenerated' ORDER BY LastUpdated DESC) AS c GROUP BY c.JobID ) b ON a.JobID = b.JobID " +
                       " LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.quote_tracking WHERE OperationType= 'QuoteGenerated' ORDER BY LastUpdated DESC) AS qt GROUP BY qt.JobID  ) q ON a.JobID = q.JobID " +
                       " LEFT JOIN DBNAME.jobs js ON js.ID = a.JobID  " +
                       " LEFT JOIN DBNAME.clients cl ON cl.ID = js.clientid  " +
                       " INNER JOIN(SELECT* FROM (SELECT* FROM DBNAME.invoice_tmp) AS it GROUP BY it.JobID) l ON a.JobID = l.JobID " +
                       "  LEFT JOIN DBNAME.clients_contacts cc ON cc.id = a.BillingContactID " +
                       "  LEFT JOIN(SELECT* FROM (SELECT* FROM DBNAME.jobs_files WHERE STATUS<>'REFERENCE' AND Deleted = 0 ) AS it GROUP BY it.JobID) jf ON a.JobID = jf.JobID " +
                       "  LEFT JOIN COMMONDBNAME.countries c ON c.code = a.Country " +
                       " WHERE a.Approved = 0 AND a.Status = 'INVOICEPENDING'" +
                       " AND a.InvoiceSentBack = 0 " +
                       " AND YEARWEEK(b.LastUpdated) = YEARWEEK(NOW()) ";


                if (SearchKey != "")
                {
                    countQry = countQry + " AND (a.invoiceID LIKE '%" + SearchKey + "%'  OR (CONCAT(YEAR(ClientCreated),'-',cl3.Country,'-',Code,'-',a.JobID) Like '%" + SearchKey + "%' ))";

                }

                countQry = countQry + " GROUP BY a.invoiceid,DATE(b.LastUpdated) ";


                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select COUNT(*) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a Group by invoiceid,created,EngagementNum";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<InvoiceInstance>(dt);
                }
                else
                {
                    return new List<InvoiceInstance>();
                }
            }
        }


    }
}
