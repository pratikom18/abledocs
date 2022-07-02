using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ManageCompany
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        #endregion

        public List<ManageCompany> GetContentListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT * FROM manage_company";
                
                myqry += " Where 1=1";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( code LIKE '%" + SearchKey + "%'" +
                        " OR name LIKE '%" + SearchKey + "%'" +
                        ")";
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

                var countQry = "SELECT COUNT(*) FROM manage_company Where 1=1";
                

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<ManageCompany>(dt);
                }
                else
                {
                    return new List<ManageCompany>();
                }
            }
        }

        public List<ManageCompany> GetManageCompanyList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM manage_company";
                

                DataTable dt = Utility.GetDataTables(qry, conn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Utility.ConvertDataTable<ManageCompany>(dt);
                }
                else
                {
                    return null;
                }
               
            }
        }

        public ManageCompany GetContentById(int id)
        {
            ManageCompany u = new ManageCompany();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM manage_company WHERE id= " + id;
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
                    u = ConvertDataTable<ManageCompany>(dt.Rows[0].Table).FirstOrDefault();

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

                    string query = "INSERT INTO manage_company(code,name) " +
                        "VALUES(@code,@name)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());


                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE manage_company SET code=@code,name=@name" +
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



                string query = "Delete from manage_company WHERE id = " + id;


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
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });



        }
    }
}
