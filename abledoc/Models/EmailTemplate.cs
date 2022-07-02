using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class EmailTemplate
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public string language { get; set; }
        public string subject { get; set; }
        public string email_for { get; set; }
        public string DeliveryEmail { get; set; }
        #endregion

        public List<EmailTemplate> GetContentListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string TypeSearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT et.id,et.DeliveryEmail,cm.typename as language,cm2.typename as email_for FROM email_template et";
                myqry += " LEFT JOIN common_master cm ON cm.typecode=et.language";
                myqry += " LEFT JOIN common_master cm2 ON cm2.typecode=et.email_for";
                myqry += " Where 1=1";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( cm.typename LIKE '%" + SearchKey + "%'" +
                        " OR DeliveryEmail LIKE '%" + SearchKey + "%'" +
                        " OR cm2.typename LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                if (TypeSearch != null)
                {
                    myqry = myqry + " AND language = '" + TypeSearch + "'";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {

                    myqry = myqry + " ORDER BY id ASC";

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

                var countQry = "SELECT COUNT(*) FROM email_template Where 1=1";
                if (TypeSearch != null)
                {
                    countQry = countQry + " AND language = '" + TypeSearch + "'";
                }

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<EmailTemplate>(dt);
                }
                else
                {
                    return new List<EmailTemplate>();
                }
            }
        }

        public EmailTemplate GetContentById(int id)
        {
            EmailTemplate u = new EmailTemplate();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM email_template WHERE id= " + id;
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
                {
                    u = ConvertDataTable<EmailTemplate>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public EmailTemplate GetTemplateByLang(string lang, string email_for)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "Select * from email_template where language='" + lang + "' and email_for='"+ email_for + "'";
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
                    return ConvertDataTable<EmailTemplate>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string InsertUpdate()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (id == 0)
                {
                    
                    string query = "INSERT INTO email_template(language,subject,email_for,DeliveryEmail) " +
                        "VALUES(@language,@subject,@email_for,@DeliveryEmail)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                   
                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE email_template SET language=@language,subject=@subject,email_for=@email_for,DeliveryEmail=@DeliveryEmail" +
                        " WHERE id=@id";


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

        public string DeleteContent(int id)
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();



                string query = "Delete from email_template WHERE id = " + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                
                ExecutionString = "Delete content successfully";

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@language",
                DbType = DbType.String,
                Value = language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email_for",
                DbType = DbType.String,
                Value = email_for,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DeliveryEmail",
                DbType = DbType.String,
                Value = DeliveryEmail,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@subject",
                DbType = DbType.String,
                Value = subject,
            });
            




        }
    }
}
