using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class CommonMaster
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int commonid { get; set; }
        public string type { get; set; }
        public string typename { get; set; }
        public string typecode { get; set; }
        public Int64 country_required { get; set; }
        public Int64 display_order { get; set; }
        #endregion

        public List<CommonMaster> GetUnitListForTable(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "SELECT * FROM common_master where type='Unit'";
                
                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( typename LIKE '%" + SearchKey + "%'" +
                        " OR display_order LIKE '%" + SearchKey + "%'" +
                        ")";
                }

                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {

                    myqry = myqry + " ORDER BY display_order ASC";

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

                var countQry = "SELECT COUNT(*) FROM common_master where type='Unit'";
               

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<CommonMaster>(dt);
                }
                else
                {
                    return new List<CommonMaster>();
                }
            }
        }

        public List<CommonMaster> GetCommonMasterList(string type)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                
                string qry = "SELECT * FROM common_master where type = '"+ type + "' order by display_order asc";
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
                    return Utility.ConvertDataTable<CommonMaster>(dt);
                else
                    return null;
            }
        }

        public CommonMaster GetCommonMasterById(int id)
        {
            CommonMaster u = new CommonMaster();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM common_master WHERE commonid= " + id;
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
                    u = ConvertDataTable<CommonMaster>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public List<CommonMaster> GetCommonMasterByMultiId(string id = "")
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string qry = "SELECT * FROM common_master WHERE 1=1";
                if(id != "")
                {
                    qry = qry + " and commonid in (" + id + ")";
                }
                else
                {
                    qry = qry + " and commonid = 0";
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
                    return Utility.ConvertDataTable<CommonMaster>(dt);
                else
                    return Utility.ConvertDataTable<CommonMaster>(dt);
            }
        }


        public CommonMaster GetCommonByTypeCode(string type)
        {
            CommonMaster u = new CommonMaster();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM common_master where typecode = '" + type + "' order by display_order asc";
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
                    u = ConvertDataTable<CommonMaster>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public string UpdateAllDisplayOrderByType()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                int dorder = 0;

                var dispQry = "SELECT display_order FROM common_master where commonid=@commonid";


                MySqlCommand disp = new MySqlCommand(dispQry, conn);
                BindId(disp);
                dorder = Convert.ToInt32(disp.ExecuteScalar());
                string query = "";
                if (dorder > display_order){

                    query = "update common_master set display_order = display_order+1 where display_order >= @display_order AND display_order< "+dorder+" and type=@type";
                }
                else
                {
                    query = "UPDATE common_master SET display_order = display_order-1 WHERE display_order<= @display_order AND display_order> "+dorder+" AND type=@type";
                }


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                cmd.ExecuteNonQuery();

                query = "UPDATE common_master SET display_order=@display_order WHERE commonid=@commonid";
                MySqlCommand cmd2 = new MySqlCommand(query, conn);
                BindParams(cmd2);
                BindId(cmd2);
                cmd2.ExecuteNonQuery();

                string query3 = "CALL rank_commonmaster_displayorder('" + type + "')";
                MySqlCommand str3 = new MySqlCommand(query3, conn);

                str3.ExecuteNonQuery();


                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string InsertUpdateUnit()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (commonid == 0)
                {

                    string query = "INSERT INTO common_master(type,typename,typecode,display_order) " +
                        "VALUES(@type,@typename,@typecode,@display_order)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());


                    ExecutionString = LastID.ToString();

                }
                else
                {


                    string query = "UPDATE common_master SET typename=@typename,typecode=@typecode,display_order=@display_order" +
                        " WHERE commonid=@commonid";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);
                    cmd.ExecuteNonQuery();


                    ExecutionString = commonid.ToString();
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



                string query = "Delete from common_master WHERE commonid = " + id;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();


                ExecutionString = "Delete state successfully";

                conn.Close();
            }
            return ExecutionString;
        }

        public CommonMaster GetMaxDisplayOrderByType(string type)
        {
            CommonMaster u = new CommonMaster();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT max(display_order) as display_order FROM common_master WHERE type= '" + type +"'";
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
                    u = ConvertDataTable<CommonMaster>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@commonid",
                DbType = DbType.Int32,
                Value = commonid,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@typename",
                DbType = DbType.String,
                Value = typename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@typecode",
                DbType = DbType.String,
                Value = typecode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@type",
                DbType = DbType.String,
                Value = type,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@display_order",
                DbType = DbType.Int32,
                Value = display_order,
            });

            



        }

        public List<CommonMaster> GetPipelineList(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, out int TotalRecord)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();


                string myqry = "select * from (SELECT * FROM common_master where type = 'Pipeline' " +
                                "Union ALL " +
                                "SELECT* FROM common_master where type in (SELECT typecode FROM common_master where type = 'Pipeline') ) as temp ";

                myqry += " Where 1=1";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND ( type LIKE '%" + SearchKey + "%'" +
                        " OR typecode LIKE '%" + SearchKey + "%'" +
                        " OR typename LIKE '%" + SearchKey + "%'" +
                        ")";
                }


                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {

                    myqry = myqry + " ORDER BY display_order ASC";

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

               // var countQry = "SELECT COUNT(*) FROM manage_company Where 1=1";
                string countQry = "select COUNT(*) from (SELECT * FROM common_master where type = 'Pipeline' " +
                                "Union ALL " +
                                "SELECT* FROM common_master where type in (SELECT typecode FROM common_master where type = 'Pipeline') ) as temp ";

                countQry += " Where 1=1";

                if (SearchKey != "")
                {
                    countQry = countQry + " AND ( type LIKE '%" + SearchKey + "%'" +
                        " OR typecode LIKE '%" + SearchKey + "%'" +
                        " OR typename LIKE '%" + SearchKey + "%'" +
                        ")";
                }


                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<CommonMaster>(dt);
                }
                else
                {
                    return new List<CommonMaster>();
                }
            }
        }

    }
}
