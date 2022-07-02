using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Clients
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        [Required]
        public string Code { get; set; }
        public string ClientSince { get; set; }
        [Required]
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Currency { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal PageRate { get; set; }
        public decimal MultiPageRate { get; set; }
        
        public string Security { get; set; }
        public string CustomPassword { get; set; }
        public string NoteOnClient { get; set; }
        public string apiKey { get; set; }
        public string Language { get; set; }
        public string PO { get; set; }
        public string PORequired { get; set; }
        public string Logo_altText { get; set; }
        public string Tagging_Instructions { get; set; }
        public string Acrobat_Security_Settings { get; set; }
        public string Terms { get; set; }
        public string Billing_Company { get; set; }
        public string Billing_Contact { get; set; }
        public string Billing_Telephone { get; set; }
        public string BillingContactID { get; set; }
        public string Billing_Email { get; set; }
        public string SecDeliveryContactID { get; set; }
        public string Alphabet { get; set; }
        public decimal TotalCount { get; set; }
        public string Year { get; set; }
        public string CountryCode { get; set; }
        public int ContactID { get; set; }
        public int ClientID { get; set; }
        public string JobQuotedAs { get; set; }
        public string EngagementNum { get; set; }
        public int JobID { get; set; }
        public DateTime LastUpdated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int QTID { get; set; }
        public string QBID { get; set; }
        public string ContractDate { get; set; }
        public string POText { get; set; }
        public string OfficeCode { get; set; }
        public string CountryName { get; set; }
        public string SalesRepresentative { get; set; }
        public Int32 stateid { get; set; }
        public string databasename { get; set; }
        public string BillingMode { get; set; }
        public decimal total { get; set; }
        public string flag { get; set; }
        
        #endregion

        public List<Clients> GetClientListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string AlphaSearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT clients.ID, clients.Code, Company,Email,City,co.Country,DATE_FORMAT(ClientSince, '%Y') as Year,co.code as CountryCode,'DBNAME' AS databasename FROM DBNAME.clients left join COMMONDBNAME.countries co ON co.code=DBNAME.clients.country WHERE Deleted=0";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (clients.ID LIKE '%" + SearchKey + "%'" +
                        " OR clients.Code LIKE '%" + SearchKey + "%'" +
                        " OR Company LIKE '%" + SearchKey + "%'" +
                        " OR Email LIKE '%" + SearchKey + "%'" +
                        " OR City LIKE '%" + SearchKey + "%'" +
                        " OR co.Country LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                if (AlphaSearch != "")
                {
                    myqry = myqry + " AND Company LIKE '" + AlphaSearch + "%'";
                }

                string myqry3 = "";

                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {

                    myqry3 = myqry3 + " ORDER BY Company ASC";

                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }
                
                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = myqry_oth + " UNION ALL " + myqry_eu +" "+ myqry3;

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

                
                var countQry = "SELECT COUNT(*) as total FROM DBNAME.clients";
                if (AlphaSearch != "")
                {
                    countQry = countQry + " Where Company LIKE '" + AlphaSearch + "%'";
                }
               
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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
                    return Utility.ConvertDataTable<Clients>(dt);
                }
                else
                {
                    return new List<Clients>();
                }
            }
        }

        public string GetDataTableFiltered(string SearchKey, string AlphaSearch)
        {
            string totalcount = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "SELECT COUNT(*) as TotalCount FROM DBNAME.clients left join COMMONDBNAME.countries co ON co.code= DBNAME.clients.country WHERE Deleted=0";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (clients.ID LIKE '%" + SearchKey + "%'" +
                        " OR clients.Code LIKE '%" + SearchKey + "%'" +
                        " OR clients.Company LIKE '%" + SearchKey + "%'" +
                        " OR clients.Email LIKE '%" + SearchKey + "%'" +
                        " OR clients.City LIKE '%" + SearchKey + "%'" +
                        " OR co.Country LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (AlphaSearch != "")
                {
                    myqry = myqry + " AND Company LIKE '" + AlphaSearch + "%'";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select SUM(TotalCount) as TotalCount from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a";

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
                    Clients p = ConvertDataTable<Clients>(dt.Rows[0].Table).FirstOrDefault();
                    totalcount = p.TotalCount.ToString();
                }
            }
            return totalcount;

        }

        public List<Clients> GetAlphabetList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT LEFT(company,1) as Alphabet,COUNT(company) as TotalCount FROM DBNAME.clients WHERE Deleted=0 GROUP BY LEFT(company,1) ";

                string myqry_oth = qry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = qry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                qry = "select Alphabet,SUM(TotalCount) as TotalCount from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a GROUP BY Alphabet ORDER BY Alphabet ASC";

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
                    return Utility.ConvertDataTable<Clients>(dt);
                else
                    return null;
            }
        }

        public Clients GetClientById(int Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT cl.*,c.country as CountryName FROM Clients cl left join COMMONDBNAME.countries c on c.code=cl.country WHERE cl.ID= " + Id;
                
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
                    return ConvertDataTable<Clients>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public Clients GetClientByCode()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID FROM clients WHERE Code = @code LIMIT 1 ";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                BindParams(cmd);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<Clients>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public List<Clients> GetCompanyList(string databasename = "")
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, Company FROM clients ORDER BY Company ASC";
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
                    return Utility.ConvertDataTable<Clients>(dt);
                else
                    return null;
            }
        }

        public string InsertUpdate()
        {
            string ExecutionString = "";
            
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                if (ID == 0)
                {
                    DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;  // using System;

                    var current_time =  now.ToUnixTimeSeconds();
                    apiKey = abledoc.Utility.ComboHelper.MD5Hash(current_time + " " + Company);

                    string query = "INSERT INTO clients(Company,apiKey,Code,ClientSince,Address1,Address2,City," +
                        "Province,PostalCode,Country,Email,Currency,HourlyRate,PageRate,Security," +
                        "CustomPassword,Acrobat_Security_Settings,Tagging_Instructions,Logo_altText," +
                        "PO,PORequired,Terms,Billing_Company,Language,NoteOnClient,BillingContactID," +
                        "SecDeliveryContactID,OfficeCode,MultiPageRate,SalesRepresentative,LastUpdated,BillingMode)" +
                        " VALUES(@company,@apikey,@code,@ClientSince,@Address1," +
                        "@Address2,@City,@province,@postalCode,@country,@email," +
                        "@currency,@hourlyRate,@pageRate,@security,@customPassword," +
                        "@acrobat_Security_Settings,@tagging_Instructions,@logo_altText," +
                        "@po,@porequired,@terms,@billing_company,@language,@noteonclient," +
                        "@BillingContactID,@secDeliveryContactID,@OfficeCode,@MultiPageRate,@SalesRepresentative,NOW(),@BillingMode)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());
                    
                    ExecutionString = LastID.ToString();

                }
                else
                {
                   
                    
                    string query = "UPDATE Clients SET apiKey=@apikey,Code=@code,ClientSince=@clientSince," +
                        "Company=@company,Address1=@address1,Address2=@address2,City=@city,Province=@province,PostalCode=@postalCode," +
                        "Country=@country,Email=@email,Currency=@currency,HourlyRate=@hourlyRate," +
                        "PageRate=@pageRate,Security=@security,CustomPassword=@customPassword,Acrobat_Security_Settings=@acrobat_Security_Settings," +
                        "Tagging_Instructions=@tagging_Instructions,Logo_altText=@logo_altText,PO=@po,PORequired=@porequired,Terms=@terms," +
                        "Billing_Company=@billing_company,Language=@language,NoteOnClient=@noteonclient,BillingContactID=@BillingContactID," +
                        "SecDeliveryContactID=@secDeliveryContactID,OfficeCode=@OfficeCode,MultiPageRate=@MultiPageRate,SalesRepresentative=@SalesRepresentative,LastUpdated=NOW(),BillingMode=@BillingMode WHERE ID=@id";

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

        public Clients GetClientByJobId(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT c.ID as ClientID, c.Company, c.Province, c.Currency, c.HourlyRate,";
                qry += " c.PageRate, c.Address1, c.Address2, c.City, c.Country, c.PostalCode,  c.Billing_Company, c.Billing_Contact, c.Billing_Telephone, c.Billing_Email, c.Language, j.BillingContactID, j.ContactID, j.JobQuotedAs,c.QBID,j.ContractDate,j.POText,s.id as stateid FROM clients c";
                qry += " INNER JOIN jobs j ON j.ClientID = c.ID ";
                qry += " LEFT JOIN COMMONDBNAME.states s ON c.Province = s.state ";
                qry += " WHERE j.ID =" + JobID;

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
                    return ConvertDataTable<Clients>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = ID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@apikey",
                DbType = DbType.String,
                Value = apiKey,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@code",
                DbType = DbType.String,
                Value = Code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientSince",
                DbType = DbType.String,
                Value = ClientSince,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@company",
                DbType = DbType.String,
                Value = Company,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address1",
                DbType = DbType.String,
                Value = Address1,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address2",
                DbType = DbType.String,
                Value = Address2,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@city",
                DbType = DbType.String,
                Value = City,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@country",
                DbType = DbType.String,
                Value = Country,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@currency",
                DbType = DbType.String,
                Value = Currency,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@province",
                DbType = DbType.String,
                Value = Province,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@postalCode",
                DbType = DbType.String,
                Value = PostalCode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hourlyRate",
                DbType = DbType.Decimal,
                Value = HourlyRate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pageRate",
                DbType = DbType.Decimal,
                Value = PageRate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MultiPageRate",
                DbType = DbType.Decimal,
                Value = MultiPageRate,
            });
            
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@security",
                DbType = DbType.String,
                Value = Security,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customPassword",
                DbType = DbType.String,
                Value = CustomPassword,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@acrobat_Security_Settings",
                DbType = DbType.String,
                Value = Acrobat_Security_Settings,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@tagging_Instructions",
                DbType = DbType.String,
                Value = Tagging_Instructions,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@logo_altText",
                DbType = DbType.String,
                Value = Logo_altText,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@po",
                DbType = DbType.String,
                Value = PO,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@porequired",
                DbType = DbType.String,
                Value = PORequired,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@terms",
                DbType = DbType.String,
                Value = Terms,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@billing_company",
                DbType = DbType.String,
                Value = Billing_Company,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@language",
                DbType = DbType.String,
                Value = Language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@noteonclient",
                DbType = DbType.String,
                Value = NoteOnClient,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@BillingContactID",
                DbType = DbType.String,
                Value = BillingContactID,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@secDeliveryContactID",
                DbType = DbType.String,
                Value = SecDeliveryContactID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OfficeCode",
                DbType = DbType.String,
                Value = OfficeCode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SalesRepresentative",
                DbType = DbType.String,
                Value = SalesRepresentative,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@databasename",
                DbType = DbType.String,
                Value = databasename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@BillingMode",
                DbType = DbType.String,
                Value = BillingMode,
            });

        }

        public List<Clients> GetQuotesClientList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord)
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "Select CONCAT(CASE WHEN MONTH(clients.ClientCreated) >= 11 and MONTH(clients.ClientCreated) <= 12 THEN YEAR(clients.ClientCreated) + 1 ELSE ";
                myqry += " YEAR(clients.ClientCreated) END, '-', clients.Country, '-', clients.Code, '-', qt.jobID) as EngagementNum, ";
                myqry += " clients.Code, clients.ClientSince, clients.Country, clients.ClientCreated, qt.JobID, qt.ContactID, qt.LastUpdated,";
                myqry += " qt.QTID, clients_contacts.FirstName, clients_contacts.LastName,'DBNAME' as databasename From DBNAME.clients RIGHT JOIN";
                myqry += " (Select quote_tracking.ID as QTID, quote_tracking.JobID as JobID, quote_tracking.LastUpdated as LastUpdated, jobs.ClientID as ClientID,";
                myqry += " jobs.ContactID as ContactID From DBNAME.quote_tracking RIGHT JOIN DBNAME.jobs ON quote_tracking.JobID = jobs.ID ";
                myqry += " Where quote_tracking.OperationType = 'QuoteGenerated' Order By quote_tracking.ID DESC) qt ON clients.ID = qt.ClientID ";
                myqry += " LEFT JOIN DBNAME.clients_contacts ON qt.ContactID = clients_contacts.ID Group BY qt.JobID";
                if (SearchKey != "")
                {
                    //myqry = myqry + " AND (clients.Country Like '%" + SearchKey + "%' OR  clients.Code Like '%" + SearchKey + "%' OR clients.ClientCreated Like '%" + SearchKey + "%' OR qt.JobID Like '%" + SearchKey + "%'  OR clients_contacts.FirstName Like '%" + SearchKey + "%' OR clients_contacts.LastName Like '%" + SearchKey + "%')";

                    myqry = myqry + " AND (qt.jobID Like '%" + SearchKey + "%'" +
                         "OR CONCAT(YEAR(clients.ClientCreated), '-', clients.Country, '-', clients.Code, '-', qt.jobID) like '%" + SearchKey + "%')";


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
                string countQry = "Select COUNT(1) as total  From DBNAME.clients RIGHT JOIN (Select quote_tracking.ID as QTID, quote_tracking.JobID as JobID, quote_tracking.LastUpdated as LastUpdated, jobs.ClientID as ClientID, jobs.ContactID as ContactID From DBNAME.quote_tracking RIGHT JOIN DBNAME.jobs ON quote_tracking.JobID = jobs.ID Where quote_tracking.OperationType = 'QuoteGenerated' Order By quote_tracking.ID DESC) qt ON clients.ID = qt.ClientID LEFT JOIN DBNAME.clients_contacts ON qt.ContactID = clients_contacts.ID Group BY qt.JobID ";

                string myqry_oth1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select COUNT(total) from ( " + myqry_oth1 + " UNION ALL " + myqry_eu1 + " ) as a " ;

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Clients>(dt);
                }
                else
                {
                    return new List<Clients>();
                }
            }
        }

        public List<Clients> GetClientVisualizationbyCountry()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "select COUNT(*) total,country from DBNAME.clients where Country is not null and Country <>'' group by Country";

                
                string myqry_oth = qry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = qry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                qry = "select country,SUM(total) as total from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a group by Country order by total desc limit 10";

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
                    return Utility.ConvertDataTable<Clients>(dt);
                else
                    return null;
            }
        }

        public List<Clients> GetJobsVisualizationbyCountry()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "select COUNT(j.id) total,c.country from DBNAME.jobs as j join DBNAME.clients as c on j.ClientID  = c.ID where c.Country is not null and c.Country <>'' group by c.Country";

                string myqry_oth = qry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = qry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                qry = "select country,SUM(total) as total from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a group by Country order by total desc limit 10";

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
                    return Utility.ConvertDataTable<Clients>(dt);
                else
                    return null;
            }
        }

    }
}
