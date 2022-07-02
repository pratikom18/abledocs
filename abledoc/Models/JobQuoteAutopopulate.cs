using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobQuoteAutopopulate
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public string Type { get; set; }
        public string Information { get; set; }
        public string Language { get; set; }
        public string typename { get; set; }
        public Int64 Deleted { get; set; }
        public Int64 tax { get; set; }
        public Int64 display_order { get; set; }
        public string country_code { get; set; }
        public string province { get; set; }
        #endregion


        public List<JobQuoteAutopopulate> GetContentListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string TypeSearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT jqa.ID,jqa.Information,jqa.display_order,cm.typename,cm2.typename as Type FROM job_quote_autopopulate jqa";
                myqry += " LEFT JOIN common_master cm ON cm.typecode=jqa.Language";
                myqry += " LEFT JOIN common_master cm2 ON cm2.typecode=jqa.Type";
                myqry += " WHERE Deleted=0";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( jqa.Type LIKE '%" + SearchKey + "%'" +
                        " OR jqa.Information LIKE '%" + SearchKey + "%'" +
                        " OR cm.typename LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                if (TypeSearch != null)
                {
                    myqry = myqry + " AND jqa.Type = '" + TypeSearch + "'";
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

                var countQry = "SELECT COUNT(*) FROM job_quote_autopopulate WHERE Deleted=0";
                if (TypeSearch != null)
                {
                    countQry = countQry + " AND Type = '" + TypeSearch + "'";
                }

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<JobQuoteAutopopulate>(dt);
                }
                else
                {
                    return new List<JobQuoteAutopopulate>();
                }
            }
        }

        public JobQuoteAutopopulate GetContentById(int id)
        {
            JobQuoteAutopopulate u = new JobQuoteAutopopulate();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM job_quote_autopopulate WHERE id= " + id;
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
                    u = ConvertDataTable<JobQuoteAutopopulate>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public JobQuoteAutopopulate GetSubtitleByCountry(string clientcountry, string lang, string type)
        {
            JobQuoteAutopopulate u = new JobQuoteAutopopulate();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM job_quote_autopopulate WHERE TYPE = '"+ type +"' AND LANGUAGE = '"+ lang +"' AND country_code LIKE '%"+ clientcountry + "%'";
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
                    u = ConvertDataTable<JobQuoteAutopopulate>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public List<JobQuoteAutopopulate> GetJobPopulateList(string country = "", string provinceid = null)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM job_quote_autopopulate WHERE Deleted=0 AND Type='Tax'";
                if(country != "" || provinceid != null)
                {
                    //qry += " and FIND_IN_SET('"+country+"', country_code)";
                    qry += " AND  ( province IS NULL AND FIND_IN_SET('" + country + "', country_code)) OR (FIND_IN_SET(" + provinceid + ", province) AND FIND_IN_SET('" + country + "', country_code))";
                }
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
                    return Utility.ConvertDataTable<JobQuoteAutopopulate>(dt);
                else
                    return null;
            }
        }

        public List<JobQuoteAutopopulate> GetJobPopulateByLanguage()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM job_quote_autopopulate WHERE Language=@Language";
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
                    return Utility.ConvertDataTable<JobQuoteAutopopulate>(dt);
                else
                    return Utility.ConvertDataTable<JobQuoteAutopopulate>(dt);
            }
        }

        public string InsertUpdate()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (ID == 0)
                {

                    string query = "INSERT INTO job_quote_autopopulate(Type,country_code,Language,Information,display_order,tax,province) " +
                        "VALUES(@Type,@country_code,@Language,@Information,@display_order,@tax,@province)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    string query2 = "CALL quotecontent_displayorder("+ LastID + ","+ display_order + ")";
                    MySqlCommand str = new MySqlCommand(query2, conn);
                    str.ExecuteNonQuery();

                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE job_quote_autopopulate SET Type=@Type,country_code=@country_code," +
                        "Language=@Language, Information=@Information,tax=@tax,province=@province WHERE ID=@ID";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);
                    cmd.ExecuteNonQuery();

                    string query2 = "CALL quotecontent_displayorder(" + ID + "," + display_order + ")";
                    MySqlCommand str = new MySqlCommand(query2, conn);

                    str.ExecuteNonQuery();

                    


                    ExecutionString = "Update successfully";
                }

                string query3 = "CALL rank_quotecontent_displayorder('" + Language + "','" + Type + "')";
                MySqlCommand str3 = new MySqlCommand(query3, conn);

                str3.ExecuteNonQuery();

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
                


                    string query = "UPDATE job_quote_autopopulate SET Deleted = 1 WHERE ID="+id;


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    
                    cmd.ExecuteNonQuery();

                JobQuoteAutopopulate modelCont = new JobQuoteAutopopulate();
                modelCont = modelCont.GetContentById(id);

                string query3 = "CALL rank_quotecontent_displayorder('" + modelCont.Language + "','" + modelCont.Type + "')";
                MySqlCommand str3 = new MySqlCommand(query3, conn);

                str3.ExecuteNonQuery();


                ExecutionString = "Delete content successfully";
                
                conn.Close();
            }
            return ExecutionString;
        }

        public JobQuoteAutopopulate GetMaxDisplayOrder(string lang,string type)
        {
            JobQuoteAutopopulate u = new JobQuoteAutopopulate();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT IFNULL(MAX(display_order),0) + 1 as display_order FROM job_quote_autopopulate where Deleted=0 and Language = '" + lang + "' and Type = '" + type + "' ";
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
                    u = ConvertDataTable<JobQuoteAutopopulate>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
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
                ParameterName = "@Language",
                DbType = DbType.String,
                Value = Language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Type",
                DbType = DbType.String,
                Value = Type,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Information",
                DbType = DbType.String,
                Value = Information,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@country_code",
                DbType = DbType.String,
                Value = country_code,
            });
           
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@display_order",
                DbType = DbType.Int64,
                Value = display_order,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@tax",
                DbType = DbType.Int64,
                Value = tax,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@province",
                DbType = DbType.String,
                Value = province,
            });
            


        }
    }
}
