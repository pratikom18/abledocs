using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobQuoteAutopopulateCountries
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public Int64 fkjobquoteautoid { get; set; }
        public string fkcountrycode { get; set; }
        public Int64 fkprovinceid { get; set; }
        #endregion

        public List<JobQuoteAutopopulateCountries> GetQuoteCountrybyID(int fkjobquoteautoid = 0)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM job_quote_autopopulate_countries WHERE fkjobquoteautoid="+ fkjobquoteautoid;
                
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
                    return Utility.ConvertDataTable<JobQuoteAutopopulateCountries>(dt);
                else
                    return null;
            }
        }

        public string Insert()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                int count = 0;
                int TotalRecord = 0;
                var countQry = "SELECT COUNT(*) FROM job_quote_autopopulate_countries where fkjobquoteautoid=@fkjobquoteautoid and fkcountrycode=@fkcountrycode and fkprovinceid=@fkprovinceid";
                MySqlCommand cou = new MySqlCommand(countQry, conn);
                BindParams(cou);
                count = Convert.ToInt32(cou.ExecuteScalar());
                TotalRecord = count;
                if (TotalRecord == 0)
                {
                    string query = "INSERT INTO job_quote_autopopulate_countries(fkjobquoteautoid,fkcountrycode,fkprovinceid) VALUES(@fkjobquoteautoid,@fkcountrycode,@fkprovinceid)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    cmd.ExecuteNonQuery();
                    ExecutionString = "Update Succefully";
                }
                conn.Close();
            }
            return ExecutionString;
        }

        public string Delete(int fkjobquoteautoid, string fkcountrycode ="",string fkprovinceid ="")
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string query = "DELETE FROM job_quote_autopopulate_countries WHERE fkjobquoteautoid = " + fkjobquoteautoid;
                if (fkcountrycode.ToString() != "")
                {
                    query += " AND fkcountrycode NOT IN ('" + fkcountrycode + "')";
                }
                if (fkprovinceid.ToString() != "")
                {
                    query += " AND fkprovinceid NOT IN (" + fkprovinceid + ")";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);


                cmd.ExecuteNonQuery();

                ExecutionString = "Delete Succefully";

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
                ParameterName = "@fkjobquoteautoid",
                DbType = DbType.Int32,
                Value = fkjobquoteautoid,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fkcountrycode",
                DbType = DbType.String,
                Value = fkcountrycode,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fkprovinceid",
                DbType = DbType.Int32,
                Value = fkprovinceid,
            });



        }
    }
}
