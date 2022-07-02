using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ADLegacyCustom
    {
        #region "Properties"
        DataContext context = new DataContext();
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string logo_alt { get; set; }
        public string Languages { get; set; }
        public string DefaultLang { get; set; }
        public string text_Welcome { get; set; }
        public string text_Thankyou { get; set; }
        public string text_Email { get; set; }
        public string StampText { get; set; }
        public string email_FromName { get; set; }
        public string email_FromAddress { get; set; }
        public string filename_append { get; set; }
        public string Hide_Logo { get; set; }
        public string Hide_Contact_Info { get; set; }
        public string icon { get; set; }
        public string Notification_Email { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
        #endregion

        public ADLegacyCustom GetADlegacyCustomById(int Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adlegacy_custom WHERE ClientID = " + Id;
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
                    return ConvertDataTable<ADLegacyCustom>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return new ADLegacyCustom();
            }
        }

        public string InsertLegacyCustom()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);

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

                var dictionary6 = new Dictionary<string, string>();
                dictionary6.Add(Languages, "");
                StampText = JsonConvert.SerializeObject(dictionary6);

                DefaultLang = Languages;
                string[] langArray = { Languages };
                Languages = JsonConvert.SerializeObject(langArray);
                try
                {

                    string query = "INSERT INTO adlegacy_custom (Languages, DefaultLang, ClientID, ClientName,logo_alt,text_Welcome,text_Thankyou,text_Email,StampText) VALUES ('" + Languages+"', '"+DefaultLang+"', '"+ClientID+"', '"+ClientName+ "','"+logo_alt+"','"+text_Welcome+"','"+text_Thankyou+"','"+text_Email+"','"+ StampText+"')";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    //BindParams(cmd);
                    //BindId(cmd);

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

        public string UpdateLegacyCustom()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                try
                {
                    string query = "UPDATE adlegacy_custom SET ClientName = @ClientName, " +
                        "Notification_Email = @Notification_Email, DefaultLang = @DefaultLang," +
                        " logo_alt = @logo_alt, Languages = @Languages, StampText = @StampText," +
                        " filename_append = @filename_append, text_Welcome = @text_Welcome," +
                        " text_Thankyou = @text_Thankyou,text_Email = @text_Email," +
                        "email_FromName = @email_FromName,email_FromAddress = @email_FromAddress," +
                        "Hide_Logo = @Hide_Logo,Hide_Contact_Info = @Hide_Contact_Info" +
                        " WHERE  ClientID = @ClientID";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);
                    cmd.ExecuteNonQuery();

                    ExecutionString = "Update successfully.";
                }
                catch (Exception ex)
                {
                    ExecutionString = ex.Message;
                }

                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateIconLegacyCustom()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_legacy(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                try
                {
                    string query = "UPDATE adlegacy_custom SET icon = @icon WHERE  ClientID = @ClientID";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@ClientID",
                        DbType = DbType.String,
                        Value = ClientID,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@icon",
                        DbType = DbType.String,
                        Value = icon,
                    });
                    cmd.ExecuteNonQuery();

                    ExecutionString = "Update successfully.";
                }
                catch (Exception ex)
                {
                    ExecutionString = ex.Message;
                }

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientID",
                DbType = DbType.String,
                Value = ClientID,
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
                ParameterName = "@ClientName",
                DbType = DbType.String,
                Value = ClientName,
            });
            

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@logo_alt",
                DbType = DbType.String,
                Value = logo_alt,
            });
            

            
            cmd.Parameters.Add(new MySqlParameter
            {
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
                ParameterName = "@StampText",
                DbType = DbType.String,
                Value = StampText,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@filename_append",
                DbType = DbType.String,
                Value = filename_append,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Notification_Email",
                DbType = DbType.String,
                Value = Notification_Email,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@icon",
                DbType = DbType.String,
                Value = icon,
            });

        }
    }
}
