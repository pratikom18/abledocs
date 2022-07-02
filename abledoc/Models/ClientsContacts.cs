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
    public class ClientsContacts
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Client field is required")]
        public Int64 ClientID { get; set; }
        [Required(ErrorMessage = "First Name field is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name field is required")]
        public string LastName { get; set; }

        public string Title { get; set; }
        public string Email { get; set; }
        public string EmailDisplayName { get; set; }
        public string Telephone { get; set; }
        public string Extension { get; set; }
        public string Cell { get; set; }
        public string Language { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public DateTime LastUpdated { get; set; }
        public string NoteOnContact { get; set; }
        public string HomeStreet { get; set; }
        public string HomePostalCode { get; set; }
        public string HomeCity { get; set; }
        public string HomeProvince { get; set; }
        public string HomeCountry { get; set; }
        public string BusinessFax { get; set; }
        public string CompanyMainPhone { get; set; }
        public Int64 IsSecBillingContact { get; set; }
        public Int64 IsSecDeliveryContact { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string phone_prefix { get; set; }
        public string phone_length { get; set; }
        public string phone_format { get; set; }
        public string Year { get; set; }
        public string CountryCode { get; set; }
        public int ADScan_UserID { get; set; }
        public string CountryName { get; set; }
        public string databasename { get; set; }
        public Int64 Deleted { get; set; }
        public string loginusercountry { get; set; }
        public string flag { get; set; }
        #endregion


        public List<ClientsContacts> GetContactListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string AlphaSearch, out int TotalRecord)
        {
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "SELECT DBNAME.clients_contacts.ID, DBNAME.clients_contacts.FirstName, DBNAME.clients_contacts.LastName,DBNAME.clients_contacts.Email,DBNAME.clients_contacts.City,COMMONDBNAME.countries.country as Country,DATE_FORMAT(DBNAME.clients.ClientSince, '%Y') as Year,COMMONDBNAME.countries.code as CountryCode, DBNAME.clients.Company,DBNAME.clients.Code,'DBNAME' AS databasename" +
                        " FROM DBNAME.clients_contacts" +
                        " LEFT JOIN DBNAME.clients ON DBNAME.clients.ID = DBNAME.clients_contacts.ClientID LEFT JOIN COMMONDBNAME.countries ON DBNAME.clients_contacts.Country = COMMONDBNAME.countries.code WHERE DBNAME.clients_contacts.Deleted = 0";


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (DBNAME.clients_contacts.ID LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients_contacts.Email LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(DBNAME.clients_contacts.FirstName,' ',DBNAME.clients_contacts.LastName) LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients_contacts.City LIKE '%" + SearchKey + "%'" +
                        " OR COMMONDBNAME.countries.country LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients.Company LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients.Code LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (AlphaSearch != "" && AlphaSearch != null)
                {
                    myqry = myqry + " AND DBNAME.clients.Company LIKE '" + AlphaSearch + "%'";
                }

                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " ORDER BY " + OrderBy;
                }
                else
                {

                    myqry3 = myqry3 + " ORDER BY clients.Company ASC";
                    
                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }
               
                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "select * from ( "+ myqry_oth + " UNION ALL " + myqry_eu +" ) as a "+myqry3;

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
                
                var countQry = "SELECT COUNT(*) as total FROM DBNAME.clients_contacts LEFT JOIN DBNAME.clients ON DBNAME.clients.ID = DBNAME.clients_contacts.ClientID LEFT JOIN COMMONDBNAME.countries ON DBNAME.clients_contacts.Country = COMMONDBNAME.countries.code WHERE DBNAME.clients_contacts.Deleted = 0";
                if (AlphaSearch != "" && AlphaSearch != null)
                {
                    countQry = countQry + " And DBNAME.clients.Company LIKE '" + AlphaSearch + "%'";
                }

                
                countQry = countQry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string countQry_oth = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = " Select Sum(total) from ("+ countQry_oth + " UNION ALL " + countQry_eu +") as a";
                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());

                conn.Close();
                TotalRecord = count;
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<ClientsContacts>(dt);
                }
                else
                {
                    return new List<ClientsContacts>();
                }
            }
        }

        public string GetDataTableFiltered(string SearchKey, string AlphaSearch)
        {
            string totalcount = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT COUNT(*) as TotalCount" +
                        " FROM DBNAME.clients_contacts" +
                        " LEFT JOIN DBNAME.clients ON clients.ID = DBNAME.clients_contacts.ClientID LEFT JOIN COMMONDBNAME.countries ON DBNAME.clients_contacts.Country = COMMONDBNAME.countries.code WHERE DBNAME.clients_contacts.Deleted = 0";


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (DBNAME.clients_contacts.ID LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients_contacts.Email LIKE '%" + SearchKey + "%'" +
                        " OR CONCAT(DBNAME.clients_contacts.FirstName,' ',DBNAME.clients_contacts.LastName) LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients_contacts.City LIKE '%" + SearchKey + "%'" +
                        " OR COMMONDBNAME.countries.country LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients.Company LIKE '%" + SearchKey + "%'" +
                        " OR DBNAME.clients.Code LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (AlphaSearch != "" && AlphaSearch != null)
                {
                    myqry = myqry + " AND DBNAME.clients.Company LIKE '" + AlphaSearch + "%'";
                }

                myqry = myqry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = myqry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                myqry = "SELECT SUM(TotalCount) as TotalCount FROM (" + myqry_oth + " UNION ALL " + myqry_eu + " ) as a";

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
                string qry = "SELECT LEFT(company,1) as Alphabet,COUNT(company) as TotalCount FROM DBNAME.clients_contacts LEFT JOIN DBNAME.clients ON clients.ID = clients_contacts.ClientID  where clients_contacts.Deleted=0 GROUP BY LEFT(company,1) ";

                string myqry_oth = qry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = qry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                qry = "SELECT Alphabet,SUM(TotalCount) as TotalCount FROM (" + myqry_oth + " UNION ALL " + myqry_eu + " ) as a GROUP BY Alphabet ORDER BY Alphabet asc";

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

        public List<ClientsContacts> GetContactList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT clients_contacts.ID total" +
                        " FROM DBNAME.clients_contacts "+
                        " LEFT JOIN DBNAME.clients ON clients.ID = clients_contacts.ClientID LEFT JOIN COMMONDBNAME.countries ON clients_contacts.Country = countries.code WHERE clients_contacts.Deleted = 0"; ;

                qry = qry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = qry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = qry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                qry = "select * from ( " + myqry_oth + " UNION ALL " + myqry_eu + " ) as a " ;

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
                    return Utility.ConvertDataTable<ClientsContacts>(dt);
                else
                    return null;
            }
        }

        public List<ClientsContacts> GetClientContactList(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,CONCAT(FirstName, ' ', LastName) As FullName, IsSecBillingContact, IsSecDeliveryContact, Email, FirstName, LastName,Country FROM clients_contacts WHERE ClientID = '" + ID+"' AND Deleted=0 ORDER BY LastName, FirstName ASC";
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
                    return Utility.ConvertDataTable<ClientsContacts>(dt);
                else
                    return null;
            }
        }

        public List<ClientsContacts> GetJobContactList(int ID,string status = "")
        {
		var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                 if(status == "BillingContact")
                {
                    status = " AND IsSecBillingContact=1";
                }
                string qry = "SELECT ID,CONCAT(IFNULL(FirstName,''), ' ', IFNULL(LastName,'')) As FullName, IsSecBillingContact, IsSecDeliveryContact, Email, FirstName, LastName,Country FROM clients_contacts WHERE ClientID = '" + ID + "' "+ status + " AND Deleted=0 ORDER BY FirstName,LastName ASC";
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
                    return Utility.ConvertDataTable<ClientsContacts>(dt);
                else
                    return null;
            }
        }

        public ClientsContacts GetContactById(int Id)
        {
            
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT cc.*,co.phone_prefix,co.phone_length,co.phone_format,co.country as CountryName FROM clients_contacts cc left join COMMONDBNAME.countries co on co.code=cc.country WHERE cc.ID= " + Id;

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
                    return ConvertDataTable<ClientsContacts>(dt.Rows[0].Table).FirstOrDefault();
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

                    string query = "INSERT INTO clients_contacts(FirstName,LastName,ClientID,Language,Title,Address1,Address2," +
                        "City,Province,PostalCode,Country,Email,Telephone,Extension," +
                        "Cell,NoteOnContact,IsSecBillingContact,IsSecDeliveryContact) " +
                        "VALUES(@FirstName,@LastName,@ClientID,@Language,@Title,@Address1," +
                        "@Address2,@City,@Province,@PostalCode,@Country,@Email,@Telephone," +
                        "@Extension,@Cell,@NoteOnContact,@IsSecBillingContact,@IsSecDeliveryContact)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE clients_contacts SET ClientID=@ClientID,FirstName=@FirstName," +
                        "LastName=@LastName," +
                        "Title=@Title,Address1=@Address1,Address2=@Address2,City=@City," +
                        "Province=@Province,PostalCode=@PostalCode," +
                        "Country=@Country,Email=@Email,Telephone=@Telephone,Extension=@Extension," +
                        "Cell=@Cell,NoteOnContact=@NoteOnContact,IsSecBillingContact=@IsSecBillingContact," +
                        "IsSecDeliveryContact=@IsSecDeliveryContact,Language=@Language,LastUpdated = NOW() WHERE ID=@ID";


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
        public string UpdateADScanUserID()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE clients_contacts SET ADScan_UserID = @UserID WHERE ID="+ID;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UserID",
                    DbType = DbType.String,
                    Value = ADScan_UserID,
                });


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }

        public string DeleteContact()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                

                string query = "UPDATE clients_contacts SET Deleted = @Deleted WHERE ID=" + ID;


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Deleted",
                    DbType = DbType.Int64,
                    Value = Deleted,
                });


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
                ParameterName = "@ClientID",
                DbType = DbType.String,
                Value = ClientID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FirstName",
                DbType = DbType.String,
                Value = FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastName",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Title",
                DbType = DbType.String,
                Value = Title,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Cell",
                DbType = DbType.String,
                Value = Cell,
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
                ParameterName = "@Province",
                DbType = DbType.String,
                Value = Province,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@City",
                DbType = DbType.String,
                Value = City,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Country",
                DbType = DbType.String,
                Value = Country,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Email",
                DbType = DbType.String,
                Value = Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PostalCode",
                DbType = DbType.String,
                Value = PostalCode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Telephone",
                DbType = DbType.String,
                Value = Telephone,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IsSecBillingContact",
                DbType = DbType.String,
                Value = IsSecBillingContact,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IsSecDeliveryContact",
                DbType = DbType.String,
                Value = IsSecDeliveryContact,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NoteOnContact",
                DbType = DbType.String,
                Value = NoteOnContact,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Extension",
                DbType = DbType.String,
                Value = Extension,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Language",
                DbType = DbType.String,
                Value = Language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@databasename",
                DbType = DbType.String,
                Value = databasename,
            });
            
        }

        public ClientsContacts GetContactByContactID(int ContactID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM clients_contacts WHERE ID= " + ContactID;
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
                    return ConvertDataTable<ClientsContacts>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }
    }
}
