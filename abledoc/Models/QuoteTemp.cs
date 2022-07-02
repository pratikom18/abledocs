using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class QuoteTemp
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public string TypeTmp { get; set; }
        public string Information { get; set; }
        public int Deleted { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<QuoteTemp> GetQuoteTmpListByJobID(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM quote_tmp WHERE JobID=@JobID ORDER BY FIELD(TypeTmp, 'sub_title','offering','notes')";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
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
                    return Utility.ConvertDataTable<QuoteTemp>(dt);
                else
                    return null;
            }
        }
        public List<QuoteTemp> GetQuoteTmpDefaultInsert(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT Information FROM quote_tmp WHERE JobID=@JobID AND TypeTmp='DefaultInsert'";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
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
                    return Utility.ConvertDataTable<QuoteTemp>(dt);
                else
                    return null;
            }
        }

        public string QuoteTmpInsert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Insert into quote_tmp(JobID, TypeTmp, Information) Values(@JobID,@TypeTmp,@Information)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }


        public string SaveQuoteText()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE quote_tmp SET Information = @Information WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string QuoteTmpDelete()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE quote_tmp SET Deleted = 1 WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
        public string DeleteAll()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE quote_tmp SET Deleted = 1 WHERE JobID = @JobID AND TypeTmp = @TypeTmp";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateQuoteTmpClientName()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE quote_tmp SET Information = @Information WHERE TypeTmp = 'client_name' AND JobID = @JobID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public QuoteTemp GetInformation(string Information)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            QuoteTemp u = new QuoteTemp();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM quote_tmp WHERE Information = '" + Information + "'";
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
                    u = ConvertDataTable<QuoteTemp>(dt.Rows[0].Table).FirstOrDefault();

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
                ParameterName = "@Information",
                DbType = DbType.String,
                Value = Information,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TypeTmp",
                DbType = DbType.String,
                Value = TypeTmp,
            });
            
        }
    }
}
