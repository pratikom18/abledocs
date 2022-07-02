using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ClientSpecificRequirements
    {
        #region Properties
        DataContext context = new DataContext();
        public int id { get; set; }
        public string client_code { get; set; }
        public string remediation_requirements { get; set; }
        public string common_alt { get; set; }
        public string author { get; set; }
        public bool unsecured { get; set; }
        public bool secured { get; set; }
        public bool pdf { get; set; }
        public bool pac_reports { get; set; }
        public string notes { get; set; }
        public int ClientID { get; set; }
        public string Company { get; set; }
        public string databasename { get; set; }
        public string unsecured1 { get; set; }
        public string secured1 { get; set; }
        public string pdf1 { get; set; }
        public string pac_reports1 { get; set; }
        public string flag { get; set; }
        #endregion

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                try
                {
                    string query = "INSERT INTO client_specific_requirements(client_code,remediation_requirements,common_alt,author,unsecured,secured,pdf,pac_reports,notes) VALUES (@client_code,@remediation_requirements,@common_alt,@author,@unsecured,@secured,@pdf,@pac_reports,@notes)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();
                }
                catch (Exception ex)
                {
                }




                conn.Close();
            }
            return ExecutionString;
        }

        public string InsertUpdate()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                if (id == 0)
                {

                    string query = "INSERT INTO client_specific_requirements(client_code,remediation_requirements,common_alt,author,unsecured,secured,pdf,pac_reports,notes) VALUES (@client_code,@remediation_requirements,@common_alt,@author,@unsecured,@secured,@pdf,@pac_reports,@notes)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());


                    ExecutionString = LastID.ToString();

                }
                else
                {

                    string query = "UPDATE client_specific_requirements SET client_code=@client_code,remediation_requirements=@remediation_requirements,common_alt=@common_alt," +
                                   " author=@author,unsecured=@unsecured,secured=@secured,pdf=@pdf,pac_reports=@pac_reports,notes=@notes WHERE id=@id";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    // BindParams(cmd);
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@id",
                        DbType = DbType.Int32,
                        Value = id,
                    });
                    cmd.ExecuteNonQuery();


                    ExecutionString = "Update Succefully";
                }



                conn.Close();
            }
            return ExecutionString;
        }


        public string Delete(int id)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
      
                string query = "DELETE FROM client_specific_requirements WHERE id=" + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                ExecutionString = "Deleted Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@client_code",
                DbType = DbType.String,
                Value = client_code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@remediation_requirements",
                DbType = DbType.String,
                Value = remediation_requirements,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@common_alt",
                DbType = DbType.String,
                Value = common_alt,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@author",
                DbType = DbType.String,
                Value = author,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@unsecured",
                DbType = DbType.Boolean,
                Value = unsecured,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@secured",
                DbType = DbType.Boolean,
                Value = secured,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pdf",
                DbType = DbType.Boolean,
                Value = pdf,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pac_reports",
                DbType = DbType.Boolean,
                Value = pac_reports,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@notes",
                DbType = DbType.String,
                Value = notes,
            });

        }

        public ClientSpecificRequirements GetClientSpecificRequirements(string code)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM client_specific_requirements WHERE client_code = '" + code+"'";
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
                    return ConvertDataTable<ClientSpecificRequirements>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return new ClientSpecificRequirements();
            }
        }


        public ClientSpecificRequirements GetCSRById(int Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT csr.* from client_specific_requirements csr WHERE csr.id= " + Id;
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
                    return ConvertDataTable<ClientSpecificRequirements>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }


        public List<ClientSpecificRequirements>  GetCSRByClientCode(string code)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT csr.* from client_specific_requirements csr WHERE csr.client_code= '" + code + "'";
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
                   
                    return Utility.ConvertDataTable<ClientSpecificRequirements>(dt);
                else
                    return null;
            }
        }


        public string UpdateNote()
        {
             var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE client_specific_requirements SET notes ='" + notes + "'  WHERE client_code = '" + client_code+"'";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                int LastID = id;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public List<ClientSpecificRequirements> GetCSRList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey,out int TotalRecord)
        {
            //string Where_Status = null;
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string myqry = "";

                myqry = "select csr.id,csr.client_code,csr.remediation_requirements,common_alt,csr.author,case when csr.unsecured = 1 then 'True' else 'False' end as unsecured1, "+
                        "case when csr.secured = 1 then 'True' else 'False' end as secured1, "+
                        "case when csr.pdf = 1 then 'True' else 'False' end as pdf1, "+
                        "case when csr.pac_reports = 1  then 'True' else 'False' end as pac_reports1 ,"+
                        "csr.notes,c.ID as ClientID, c.Company,'DBNAME' as databasename from DBNAME.client_specific_requirements csr inner join DBNAME.clients c on c.Code = csr.client_code ";


                if (SearchKey != "")
                {
                    myqry = myqry + " where (csr.client_code LIKE '%" + SearchKey + "%'" +
                   " OR c.Company like '%" + SearchKey + "%' OR csr.notes like '%" + SearchKey + "%' OR csr.remediation_requirements like '%" + SearchKey + "%' OR csr.common_alt like '%" + SearchKey + "%' OR csr.author like '"+ SearchKey + "')";
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

                //myqry =  myqry_oth +" " + myqry3;

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
                string countQry = "";

                countQry = "select COUNT(*) as total from DBNAME.client_specific_requirements csr inner join DBNAME.clients c on c.Code = csr.client_code ";


                if (SearchKey != "")
                {
                    countQry = countQry + " where (csr.client_code LIKE '%" + SearchKey + "%'" +
                   " OR c.Company like '%" + SearchKey + "%' OR csr.notes like '%" + SearchKey + "%' OR csr.remediation_requirements like '%" + SearchKey + "%' OR csr.common_alt like '%" + SearchKey + "%' OR csr.author like '" + SearchKey + "')";
                }


                string myqry_oth1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu1 = countQry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                countQry = "select SUM(total) from ( " + myqry_oth1 + " UNION ALL " + myqry_eu1 + " ) as a " ;

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<ClientSpecificRequirements>(dt);
                }
                else
                {
                    return new List<ClientSpecificRequirements>();
                }
            }
        }

    }
}
