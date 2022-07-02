using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ClientsMultirate
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int clientsmultirate_id { get; set; }
        public int clientid { get; set; }
        public string typecode { get; set; }
        public decimal rate { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<ClientsMultirate> GetList(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT clientsmultirate_id, clientid, typecode,rate FROM clients_multirate WHERE clientid =" + id + " ORDER BY clientsmultirate_id ASC";
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
                    return Utility.ConvertDataTable<ClientsMultirate>(dt);
                else
                    return null;
            }
        }

        public string Insert()
        {
            string ExecutionString = "";
            try
            {
                var DatabaseConnection = Models.Utility.getDatabase(databasename);

                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    //int count = 0;
                    //int TotalRecord = 0;
                    //var countQry = "SELECT COUNT(*) FROM clients_multirate where clientid=@clientid and typecode=@typecode";
                    //MySqlCommand cou = new MySqlCommand(countQry, conn);
                    //BindParams(cou);
                    //count = Convert.ToInt32(cou.ExecuteScalar());
                    //TotalRecord = count;
                    //if (TotalRecord == 0)
                    //{
                    string query = "INSERT INTO clients_multirate(clientid, typecode,rate) VALUES(@clientid, @typecode,@rate)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();
                    //}
                    //else {
                    //    ExecutionString = "-999";
                    //}


                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        public string Update()
        {
            string ExecutionString = "";
            try
            {
                var DatabaseConnection = Models.Utility.getDatabase(databasename);

                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();


                    string query = "update clients_multirate set typecode = @typecode,rate = @rate where clientsmultirate_id = @clientsmultirate_id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@clientsmultirate_id",
                        DbType = DbType.Int32,
                        Value = clientsmultirate_id,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@typecode",
                        DbType = DbType.String,
                        Value = typecode,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@rate",
                        DbType = DbType.Decimal,
                        Value = rate,
                    });

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        public string Delete()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "DELETE FROM clients_multirate WHERE clientsmultirate_id = " + clientsmultirate_id;
               
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        public string Deleterate(string clientid, string typecode)
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                // accesskey = abledoc.Utility.ComboHelper.RandomString();

                string query = "DELETE FROM clients_multirate WHERE clientid = " + clientid;
                if (typecode.ToString() != "")
                {
                    query += " AND typecode NOT IN (" + typecode + ")";
                }


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                ExecutionString = "Update Succefully";

                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientid",
                DbType = DbType.Int32,
                Value = clientid
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@typecode",
                DbType = DbType.String,
                Value = typecode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rate",
                DbType = DbType.Decimal,
                Value = rate,
            });

        }

        private void BindParams1(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientid",
                DbType = DbType.Int32,
                Value = clientid
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@typecode",
                DbType = DbType.String,
                Value = typecode,
            });


        }
    }
}
