using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class CreditMemoTmp
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int FileID { get; set; }
        public int JobID { get; set; }
        public string PricePer { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Tax { get; set; }
        public string Description { get; set; }
        public int Deleted { get; set; }
        public int InvoiceIDQB { get; set; }
        public int CreditMemoIDQB { get; set; }
        public int Locked { get; set; }
        public int CreditMemoID { get; set; }
        public string databasename { get; set; }

        public List<CreditMemoTmp> GetCreditMemoTmpLockedByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM credit_memo_tmp WHERE JobID=@JobID AND Deleted=0 AND Locked=0";
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
                    return Utility.ConvertDataTable<CreditMemoTmp>(dt);
                else
                    return null;
            }
        }

        public List<CreditMemoTmp> GetCreditMemoTmpByJobIDCreditMemoID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM credit_memo_tmp WHERE JobID=@JobID AND Deleted=0 AND CreditMemoID=@CreditMemoID";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CreditMemoID",
                    DbType = DbType.Int32,
                    Value = CreditMemoID,
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
                    return Utility.ConvertDataTable<CreditMemoTmp>(dt);
                else
                    return null;
            }
        }

        public string UpdateByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE credit_memo_tmp SET Locked = @Locked, CreditMemoID = @CreditMemoID WHERE JobID = @JobID AND Locked = @OldLockedStatus";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Locked",
                    DbType = DbType.Int32,
                    Value = 1,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CreditMemoID",
                    DbType = DbType.Int32,
                    Value = CreditMemoID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@OldLockedStatus",
                    DbType = DbType.Int32,
                    Value = 0,
                });

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
    }
}
