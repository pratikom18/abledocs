using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class States
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public string countrycode { get; set; }
        public string code { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string databasename { get; set; }
        #endregion


        public List<States> GetContentListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, string CountrySearch, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT s.id,s.state,s.code,c.country as country FROM states s";
                myqry += " LEFT JOIN countries c ON c.code=s.countrycode";
                myqry += " Where 1=1";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( s.state LIKE '%" + SearchKey + "%'" +
                        " OR c.country LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                if (CountrySearch != null)
                {
                    myqry = myqry + " AND countrycode = '" + CountrySearch + "'";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {

                    myqry = myqry + " ORDER BY s.id ASC";

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

                var countQry = "SELECT COUNT(*) FROM states Where 1=1";
                if (CountrySearch != null)
                {
                    countQry = countQry + " AND countrycode = '" + CountrySearch + "'";
                }

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<States>(dt);
                }
                else
                {
                    return new List<States>();
                }
            }
        }
        
        public List<States> GetStateList(string countrycode = "")
        {
        
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM states";
                if(countrycode != "")
                {
                    qry+= " Where countrycode='"+ countrycode + "'";
                }

                DataTable dt = Utility.GetDataTables(qry, conn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Utility.ConvertDataTable<States>(dt);
                } else
                {
                    return null;
                }
                //MySqlCommand cmd = new MySqlCommand(qry, conn);
                //MySqlDataReader dr = cmd.ExecuteReader();
                //DataSet ds = new DataSet();
                //DataTable dt = new DataTable();
                //ds.Tables.Add(dt);
                //ds.EnforceConstraints = false;
                //dt.Load(dr);
                //dr.Close();
                //if (dt != null && dt.Rows.Count > 0)
                //    return Utility.ConvertDataTable<States>(dt);
                //else
                //    return null;
            }
        }

        public List<States> GetStateListByCountry(string[] countrycode = null)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM states";
                if (countrycode != null)
                {
                    string country = string.Join("','", countrycode);
                    qry += " Where countrycode IN('" + country + "')";
                }

                DataTable dt = Utility.GetDataTables(qry, conn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Utility.ConvertDataTable<States>(dt);
                }
                else
                {
                    return null;
                }
                //MySqlCommand cmd = new MySqlCommand(qry, conn);
                //MySqlDataReader dr = cmd.ExecuteReader();
                //DataSet ds = new DataSet();
                //DataTable dt = new DataTable();
                //ds.Tables.Add(dt);
                //ds.EnforceConstraints = false;
                //dt.Load(dr);
                //dr.Close();
                //if (dt != null && dt.Rows.Count > 0)
                //    return Utility.ConvertDataTable<States>(dt);
                //else
                //    return null;
            }
        }


        public States GetContentById(int id)
        {
            States u = new States();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM states WHERE id= " + id;
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
                    u = ConvertDataTable<States>(dt.Rows[0].Table).FirstOrDefault();

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

                    string query = "INSERT INTO states(countrycode,code,state) " +
                        "VALUES(@countrycode,@code,@state)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());


                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE states SET countrycode=@countrycode,code=@code,state=@state" +
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



                string query = "Delete from states WHERE id = " + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();


                ExecutionString = "Delete state successfully";

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
                ParameterName = "@state",
                DbType = DbType.String,
                Value = state,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@code",
                DbType = DbType.String,
                Value = code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@countrycode",
                DbType = DbType.String,
                Value = countrycode,
            });

        }
    }
}
