using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class DiscriptionMaster
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string Language { get; set; }
        public string country_code { get; set; }
        public string unit { get; set; }

        #endregion

        public List<DiscriptionMaster> GetContentListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey,string LanguageFilter, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT dm.id,dm.ProductName,dm.ProductPrice,cm.typename as Language FROM discription_master dm";
                myqry += " LEFT JOIN common_master cm ON cm.typecode=dm.Language";

                myqry += " Where 1=1";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( ProductName LIKE '%" + SearchKey + "%'" +
                        " OR ProductPrice LIKE '%" + SearchKey + "%'" +
                        " OR cm.typename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (LanguageFilter != null)
                {
                    myqry = myqry + " AND Language = '" + LanguageFilter + "'";
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

                var countQry = "SELECT COUNT(*) FROM discription_master Where 1=1";
                if (LanguageFilter != null)
                {
                    countQry = countQry + " AND Language = '" + LanguageFilter + "'";
                }


                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<DiscriptionMaster>(dt);
                }
                else
                {
                    return new List<DiscriptionMaster>();
                }
            }
        }

        public List<DiscriptionMaster> GetDiscriptionMasterList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM discription_master";
                

                DataTable dt = Utility.GetDataTables(qry, conn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Utility.ConvertDataTable<DiscriptionMaster>(dt);
                }
                else
                {
                    return null;
                }
               
            }
        }

        public DiscriptionMaster GetContentById(int id)
        {
            DiscriptionMaster u = new DiscriptionMaster();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM discription_master WHERE id= " + id;
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
                    u = ConvertDataTable<DiscriptionMaster>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public DiscriptionMaster GetContentByName(string name ="nodata")
        {
            DiscriptionMaster u = new DiscriptionMaster();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM discription_master WHERE ProductName like '" + name +"'";
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
                    u = ConvertDataTable<DiscriptionMaster>(dt.Rows[0].Table).FirstOrDefault();

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

                    string query = "INSERT INTO discription_master(ProductName,ProductPrice,Language,country_code,unit) " +
                        "VALUES(@ProductName,@ProductPrice,@Language,@country_code,@unit)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());


                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE discription_master SET ProductName=@ProductName,ProductPrice=@ProductPrice,Language=@Language,country_code=@country_code,unit=@unit" +
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



                string query = "Delete from discription_master WHERE id = " + id;


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
                ParameterName = "@ProductName",
                DbType = DbType.String,
                Value = ProductName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ProductPrice",
                DbType = DbType.Double,
                Value = ProductPrice,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Language",
                DbType = DbType.String,
                Value = Language,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@country_code",
                DbType = DbType.String,
                Value = country_code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@unit",
                DbType = DbType.String,
                Value = unit,
            });



        }

        public List<DiscriptionMaster> GetDescriptionMasterList(string country = "", string Language = "")
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM discription_master WHERE Language='" + Language+"'";
                if (country != "")
                {
                    //qry += " and FIND_IN_SET('"+country+"', country_code)";
                    qry += " AND FIND_IN_SET('" + country + "', country_code) ";
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
                    return Utility.ConvertDataTable<DiscriptionMaster>(dt);
                else
                    return new List<DiscriptionMaster>();
            }
        }

    }
}
