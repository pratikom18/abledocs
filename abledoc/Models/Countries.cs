using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Countries
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public string code { get; set; }
        public string country { get; set; }
        public string phone_prefix { get; set; }
        public string phone_length { get; set; }
        public string phone_format { get; set; }
        public string currency { get; set; }
        public string currency_code { get; set; }
        public string smtphost { get; set; }
        public string smtpport { get; set; }
        public string emailaddress { get; set; }
        public string password { get; set; }
        public bool isdefault { get; set; }
        public string currencycode { get; set; }
        public bool EUcountry { get; set; }
        #endregion

        public List<Countries> GetContentListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string CountrySearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT * FROM countries";
                myqry += " Where 1=1";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( code LIKE '%" + SearchKey + "%'" +
                        " OR country LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                if (CountrySearch != null)
                {
                    myqry = myqry + " AND code = '" + CountrySearch + "'";
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

                var countQry = "SELECT COUNT(*) FROM countries Where 1=1";
                if (CountrySearch != null)
                {
                    countQry = countQry + " AND code = '" + CountrySearch + "'";
                }

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<Countries>(dt);
                }
                else
                {
                    return new List<Countries>();
                }
            }
        }

        public List<Countries> GetCountryList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM countries";
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
                    return Utility.ConvertDataTable<Countries>(dt);
                else
                    return null;
            }
        }

        public List<Countries> GetCA_CountryList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM countries where EUcountry = 0";
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
                    return Utility.ConvertDataTable<Countries>(dt);
                else
                    return null;
            }
        }

        public List<Countries> GetEU_CountryList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM countries where EUcountry = 1";
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
                    return Utility.ConvertDataTable<Countries>(dt);
                else
                    return null;
            }
        }

        public List<Countries> GetCountryCurrencyList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT code,CONCAT(country, '(', currency_code,')') As currencycode FROM countries";
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
                    return Utility.ConvertDataTable<Countries>(dt);
                else
                    return null;
            }
        }

        public Countries GetCountryByCode()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM countries WHERE code= @code";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@code",
                    DbType = DbType.String,
                    Value = code,
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
                    return ConvertDataTable<Countries>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public Countries GetCountryByDefaultSMTP()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM countries WHERE isdefault= 1";
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
                    return ConvertDataTable<Countries>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public Countries GetContentById(int id)
        {
            Countries u = new Countries();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM countries WHERE id= " + id;
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
                    u = ConvertDataTable<Countries>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public string InsertUpdate()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (id == 0)
                {

                    string query = "INSERT INTO countries(code,country,currency,currency_code,smtphost,smtpport,emailaddress,password) " +
                        "VALUES(@code,@country,@currency,@currency_code,@smtphost,@smtpport,@emailaddress,@password)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());


                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE countries SET code=@code,country=@country,currency=@currency,currency_code=@currency_code,smtphost=@smtphost,smtpport=@smtpport,emailaddress=@emailaddress,password=@password" +
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



                string query = "Delete from countries WHERE id = " + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();


                ExecutionString = "Delete data successfully";

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
                ParameterName = "@code",
                DbType = DbType.String,
                Value = code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@country",
                DbType = DbType.String,
                Value = country,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phone_format",
                DbType = DbType.String,
                Value = phone_format,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phone_length",
                DbType = DbType.String,
                Value = phone_length,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phone_prefix",
                DbType = DbType.String,
                Value = phone_prefix,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@currency",
                DbType = DbType.String,
                Value = currency,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@currency_code",
                DbType = DbType.String,
                Value = currency_code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@smtphost",
                DbType = DbType.String,
                Value = smtphost,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@smtpport",
                DbType = DbType.String,
                Value = smtpport,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@emailaddress",
                DbType = DbType.String,
                Value = emailaddress,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = password,
            });
        }

        public string isDefault()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string query = "UPDATE countries SET isdefault=0";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                
                cmd.ExecuteNonQuery();


                string query1 = "UPDATE countries SET isdefault = 1 " +
                    " WHERE code='"+code+"'";


                MySqlCommand cmd1 = new MySqlCommand(query1, conn);
                
                cmd1.ExecuteNonQuery();


                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

    }


}

