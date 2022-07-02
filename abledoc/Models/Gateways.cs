using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Gateways
    {
        #region "Properties"
        DataContext context = new DataContext();
        [Required]
        public string subdomain { get; set; }
        public string ClientID { get; set; }
        [Required]
        public string PortalName { get; set; }
        public string ClientName { get; set; }
        public string logo_alt { get; set; }
        public string Languages { get; set; }
        public string DefaultLang { get; set; }
        public string fields { get; set; }
        public string fields_custom { get; set; }
        public string text_Welcome { get; set; }
        public string text_Thankyou { get; set; }
        public string text_Email { get; set; }
        public string email_FromName { get; set; }
        public string email_FromAddress { get; set; }
        public string Hide_Logo { get; set; }
        public string Hide_Contact_Info { get; set; }
        public string Hide_Delivery_Time { get; set; }
        public string apiKey { get; set; }
        public string Azure_AD_Client { get; set; }
        public string Azure_AD_Secret { get; set; }
        public string Code { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
        public string Portal_Password { get;set; }
        public string SAML_SSO_Provider { get; set; }
        #endregion

        public List<Gateways> GatewaysListByClientId(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT subdomain, PortalName,ClientID FROM gateways WHERE ClientID = @ClientID ORDER BY subdomain ASC";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ClientID",
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
                    return Utility.ConvertDataTable<Gateways>(dt);
                else
                    return null;
            }
        }

        public Gateways GetewayById(string subdomain)
        {
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM gateways WHERE subdomain= '"+ subdomain+"'";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

               // BindId(cmd);
                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return ConvertDataTable<Gateways>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string Insert()
        {
            string ExecutionString = "";

            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                try
                {

                    string query = "INSERT INTO gateways(subdomain,Languages,ClientID,ClientName,logo_alt,PortalName,email_FromName,email_FromAddress,fields,fields_custom,text_Welcome,text_Thankyou,text_Email,Azure_AD_Client,Azure_AD_Secret," +
                                    "Hide_Logo,Hide_Contact_Info,Hide_Delivery_Time,Portal_Password,SAML_SSO_Provider) " +
                                    "VALUES(@subdomain,@Languages,@ClientID,@ClientName,@logo_alt,@PortalName,@email_FromName,@email_FromAddress,@fields,@fields_custom,@text_Welcome,@text_Thankyou,@text_Email,@Azure_AD_Client,@Azure_AD_Secret," +
                                    "@Hide_Logo,@Hide_Contact_Info,@Hide_Delivery_Time,@Portal_Password,@SAML_SSO_Provider)";
                  
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);

                    cmd.ExecuteNonQuery();

                    ExecutionString = subdomain;
                }
                catch (Exception ex)
                {
                }




                conn.Close();
            }
            return ExecutionString;
        }

        public string InsertGateway()
        {
            string ExecutionString = "";

            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                var dictionary = new Dictionary<string, string>();
                dictionary.Add(Languages, ClientName);

                ClientName = JsonConvert.SerializeObject(dictionary);

                var dictionary2 = new Dictionary<string, string>();
                dictionary2.Add(Languages, "");
                logo_alt = JsonConvert.SerializeObject(dictionary2);

                var dictionary3 = new Dictionary<string, string>();
                dictionary3.Add(Languages, "");
                text_Welcome = JsonConvert.SerializeObject(dictionary3);

                var dictionary4 = new Dictionary<string, string>();
                dictionary4.Add(Languages, "");
                text_Thankyou = JsonConvert.SerializeObject(dictionary4);

                var dictionary5 = new Dictionary<string, string>();
                dictionary5.Add(Languages, "");
                text_Email = JsonConvert.SerializeObject(dictionary5);


                fields = "{'FirstName':'Required','LastName':'Required','Phone':'Required','Email':'Required','Extension':'Show','AdditionalContact':'Hide','BillingContact':'Hide','Quote':'Show','Notes':'Hide'}";

                DefaultLang = Languages;
                string[] langArray = { Languages };
                Languages = JsonConvert.SerializeObject(langArray);
                try
                {

                    string query = "INSERT INTO gateways (subdomain, Languages, DefaultLang, ClientID, ClientName, apiKey,logo_alt,fields,text_Welcome,text_Thankyou,text_Email) VALUES(@subdomain,@Languages,@DefaultLang, @ClientID,@ClientName,@apiKey,@logo_alt,@fields,@text_Welcome,@text_Thankyou,@text_Email)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);

                    cmd.ExecuteNonQuery();

                    ExecutionString = subdomain;
                }
                catch (Exception ex)
                { 
                }
                

               
                
                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateGateway()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                try
                {
                    string query = "UPDATE gateways SET Languages=@Languages," +
                        "ClientID=@ClientID,ClientName=@ClientName,logo_alt=@logo_alt,PortalName=@PortalName," +
                        "email_FromName=@email_FromName,email_FromAddress=@email_FromAddress,fields=@fields," +
                        "fields_custom=@fields_custom,text_Welcome=@text_Welcome,text_Thankyou=@text_Thankyou,text_Email=@text_Email," +
                        "Azure_AD_Client=@Azure_AD_Client,Azure_AD_Secret=@Azure_AD_Secret," +
                        "Hide_Logo=@Hide_Logo,Hide_Contact_Info=@Hide_Contact_Info,Hide_Delivery_Time=@Hide_Delivery_Time,Portal_Password=@Portal_Password,SAML_SSO_Provider=@SAML_SSO_Provider " +
                        " WHERE subdomain=@subdomain";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);
                    int a = cmd.ExecuteNonQuery();

                    ExecutionString = subdomain;
                }
                catch (Exception ex)
                {
                    ExecutionString = ex.Message;
                }

                conn.Close();
            }
            return ExecutionString;
        }

        public string DeleteGateway()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_gateway(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "DELETE FROM gateways WHERE 1 AND subdomain = @subdomain AND ClientID = @ClientID ";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                BindParams(cmd);
                cmd.ExecuteNonQuery();


                ExecutionString = "Delete Gateway successfully";

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@subdomain",
                DbType = DbType.String,
                Value = subdomain,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Languages",
                DbType = DbType.String,
                Value = Languages,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DefaultLang",
                DbType = DbType.String,
                Value = DefaultLang,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientID",
                DbType = DbType.String,
                Value = ClientID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientName",
                DbType = DbType.String,
                Value = ClientName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@apiKey",
                DbType = DbType.String,
                Value = apiKey,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@logo_alt",
                DbType = DbType.String,
                Value = logo_alt,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PortalName",
                DbType = DbType.String,
                Value = PortalName,
            });
            
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fields",
                DbType = DbType.String,
                Value = fields,
            });
            cmd.Parameters.Add(new MySqlParameter {
                ParameterName = "@email_FromName",
                DbType = DbType.String,
                Value = email_FromName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email_FromAddress",
                DbType = DbType.String,
                Value = email_FromAddress,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fields_custom",
                DbType = DbType.String,
                Value = fields_custom,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@text_Welcome",
                DbType = DbType.String,
                Value = text_Welcome,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@text_Thankyou",
                DbType = DbType.String,
                Value = text_Thankyou,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@text_Email",
                DbType = DbType.String,
                Value = text_Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Azure_AD_Client",
                DbType = DbType.String,
                Value = Azure_AD_Client,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Azure_AD_Secret",
                DbType = DbType.String,
                Value = Azure_AD_Secret,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Hide_Logo",
                DbType = DbType.String,
                Value = Hide_Logo,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Hide_Contact_Info",
                DbType = DbType.String,
                Value = Hide_Contact_Info,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Hide_Delivery_Time",
                DbType = DbType.String,
                Value = Hide_Delivery_Time,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@databasename",
                DbType = DbType.String,
                Value = databasename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Portal_Password",
                DbType = DbType.String,
                Value = Portal_Password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SAML_SSO_Provider",
                DbType = DbType.String,
                Value = SAML_SSO_Provider,
            });

        }


        //public List<Gateways> GetGatewayList()
        //{
        //    using (MySqlConnection conn = context.GetConnection(Constants.GATEWAY_DB))
        //    {
        //        conn.Open();
        //        string qry = string.Empty;
        //        qry = "SELECT subdomain, ClientID, ClientName, PortalName FROM gateways";

        //        MySqlCommand cmd = new MySqlCommand(qry, conn);


        //        MySqlDataReader dr = cmd.ExecuteReader();
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        ds.Tables.Add(dt);
        //        ds.EnforceConstraints = false;
        //        dt.Load(dr);
        //        dr.Close();
        //        conn.Close();
        //        if (dt != null && dt.Rows.Count > 0)
        //            return Utility.ConvertDataTable<Gateways>(dt);
        //        else
        //            return null;
        //    }
        //}




        public List<Gateways> GetGatewayList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string AlphaSearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.GATEWAY_DB))
            {
                conn.Open();


                string myqry = "SELECT subdomain, ClientID, PortalName,'DBNAME' as databasename" +
                                " FROM DBNAME.gateways" +
                                " where 1 ";


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (ClientID LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR ClientName LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR PortalName LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR subdomain LIKE '%" + SearchKey.Trim() + "%'" +
                        ")";
                }

                //if (AlphaSearch != "")
                //{
                //    myqry = myqry + " AND Company LIKE '" + AlphaSearch + "%'";
                //}
                string myqry3 = "";
                if (OrderBy != "")
                {
                    myqry3 = myqry3 + " order by " + OrderBy;
                }
                else
                {

                    myqry3 = myqry3 + " ORDER BY subdomain ASC";

                }
                if (inEndIndex != -1)
                {
                    myqry3 = myqry3 + " LIMIT @end OFFSET @start";
                }

                string myqry_oth = myqry.Replace("DBNAME", Constants.GATEWAY_DB);
                string myqry_eu = myqry.Replace("DBNAME", Constants.GATEWAY_EU_DB);

                myqry = " Select * from (" + myqry_oth + " UNION ALL " + myqry_eu + ") as a" + myqry3;


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

                var countQry = "SELECT COUNT(*) as total FROM gateways where 1 ";
                if (SearchKey != "")
                {
                    myqry = myqry + " AND (ClientID LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR ClientName LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR PortalName LIKE '%" + SearchKey.Trim() + "%'" +
                        " OR subdomain LIKE '%" + SearchKey.Trim() + "%'" +
                        ")";
                }
                
                string countQry_oth = countQry.Replace("DBNAME", Constants.GATEWAY_DB);
                string countQry_eu = countQry.Replace("DBNAME", Constants.GATEWAY_EU_DB);

                myqry = " Select SUM(total) from (" + countQry_oth + " UNION ALL " + countQry_eu + ") as a" ;

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Gateways>(dt);
                }
                else
                {
                    return Utility.ConvertDataTable<Gateways>(dt);
                }
            }
        }


    }


    public class Languages
    {
        public string Name { get; set; }
    }
}
