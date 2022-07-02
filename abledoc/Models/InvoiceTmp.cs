using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class InvoiceTmp
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public Int64 InvoiceID { get; set; }
        public Int64 JobID { get; set; }
        public Int64 FileID { get; set; }
        public string PricePer { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string Tax { get; set; }
        public string Description { get; set; }
        public Int64 Deleted { get; set; }
        public Int64 InvoiceIDQB { get; set; }
        public Int64 OverrideValues { get; set; }
        public int maxInvoiceID { get; set; }
        public Int64 OldPricePer { get; set; }
        public Int64 OldPrice { get; set; }
        public Int64 OldQuantity { get; set; }
        public Int64 OldTax { get; set; }
        public Int64 OldDescription { get; set; }
        public int CreditMemoID { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<InvoiceTmp> GetInvoiceTmpByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM invoice_tmp WHERE JobID=@JobID AND Deleted=0";
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
                    return Utility.ConvertDataTable<InvoiceTmp>(dt);
                else
                    return null;
            }
        }

        public InvoiceTmp GetMaxInvoiceID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT MAX(InvoiceID) as maxInvoiceID FROM invoice_tmp";
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
                    return ConvertDataTable<InvoiceTmp>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string InvoiceTmpInsert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Insert into invoice_tmp(InvoiceID, FileID, JobID, PricePer, Price, Quantity, Tax, Description) Values(@InvoiceID, @FileID, @JobID, @PricePer, @Price, @Quantity, @Tax, @Description)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString()); ;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string InvoiceTmpUpdate()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_tmp SET PricePer = CASE WHEN PricePer = '0' OR PricePer IS NULL OR @OldPricePer = 1 THEN @PricePer ELSE PricePer END, " +
                 " Price = CASE WHEN Price = 0 OR Price IS NULL OR @OldPrice = 1 THEN @Price ELSE Price END, " +
                 " Quantity = CASE WHEN Quantity = 0 OR Quantity IS NULL OR @OldQuantity = 1 THEN @Quantity ELSE Quantity END, " +
                 " Tax = CASE WHEN Tax = '0' OR Tax IS NULL OR @OldTax = 1 THEN @Tax ELSE Tax END, " +
                 " Description = CASE WHEN Description IS NULL OR @OldDescription = 1 THEN @Description ELSE Description END " +
                 " WHERE InvoiceID = @InvoiceID And FileID = @FileID AND JobID = @JobID";


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

        public string JobPanelUpdateInvoice()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_tmp SET PricePer = @PricePer, Price = @Price, Quantity = @Quantity, Tax = @Tax, Description = @Description, OverrideValues = 1 WHERE ID = @ID";


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

        public string FileDeleteInvoice()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_tmp SET Deleted = 1 WHERE ID = @ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);


                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
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
                ParameterName = "@InvoiceID",
                DbType = DbType.Int32,
                Value = InvoiceID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PricePer",
                DbType = DbType.String,
                Value = PricePer,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Price",
                DbType = DbType.Double,
                Value = Price,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Quantity",
                DbType = DbType.Double,
                Value = Quantity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Tax",
                DbType = DbType.String,
                Value = Tax,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Description",
                DbType = DbType.String,
                Value = Description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldPricePer",
                DbType = DbType.Int32,
                Value = OldPricePer,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldPrice",
                DbType = DbType.Int32,
                Value = OldPrice,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldQuantity",
                DbType = DbType.Int32,
                Value = OldQuantity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldTax",
                DbType = DbType.Int32,
                Value = OldTax,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OldDescription",
                DbType = DbType.Int32,
                Value = OldDescription,
            });

        }

        public List<InvoiceTmp> GetCreditMemoTmpLockedByJobID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT Quantity,Tax,Description,PricePer,Price FROM credit_memo_tmp WHERE JobID=@JobID AND Deleted=0 AND Locked=0";
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
                    return Utility.ConvertDataTable<InvoiceTmp>(dt);
                else
                    return null;
            }
        }

        public List<InvoiceTmp> GetCreditMemoTmpByJobIDCreditMemoID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT Quantity,Tax,Description,PricePer,Price FROM credit_memo_tmp WHERE JobID=@JobID AND Deleted=0 AND CreditMemoID=@CreditMemoID";
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
                    return Utility.ConvertDataTable<InvoiceTmp>(dt);
                else
                    return null;
            }
        }

        public string UpdateInvoiceIDQBByInvoiceID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE invoice_tmp SET InvoiceIDQB = @InvoiceIDQB WHERE InvoiceID = @InvoiceID";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@InvoiceIDQB",
                    DbType = DbType.Int64,
                    Value = InvoiceIDQB,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@InvoiceID",
                    DbType = DbType.Int32,
                    Value = InvoiceID,
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
