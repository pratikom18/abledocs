using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class InvoiceTracking
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public Int64 JobID { get; set; }
        public decimal InvoiceAmount { get; set; }
        public Int64 UID { get; set; }
        public string OperationType { get; set; }
        public DateTime LastUpdated { get; set; }
        public string PhysicalLocation { get; set; }
        public string databasename { get; set; }
        #endregion

        public InvoiceTracking GetInvoiceTrackingByJobID(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM invoice_tracking WHERE JobID="+ ID + " AND OperationType='InvoiceGenerated' ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
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
                    return ConvertDataTable<InvoiceTracking>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string InsertInvoiceTracking()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Insert into invoice_tracking(JobID, InvoiceAmount, UID, OperationType) Values(@JobID,@InvoiceAmount,@UID,@OperationType)";


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
                ParameterName = "@InvoiceAmount",
                DbType = DbType.Decimal,
                Value = InvoiceAmount,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UID",
                DbType = DbType.Int64,
                Value = UID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OperationType",
                DbType = DbType.String,
                Value = OperationType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastUpdated",
                DbType = DbType.DateTime,
                Value = LastUpdated,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PhysicalLocation",
                DbType = DbType.String,
                Value = PhysicalLocation,
            });


        }

        public string UpdatePhysicalLocationByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE invoice_tracking SET PhysicalLocation = @PhysicalLocation WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@PhysicalLocation",
                    DbType = DbType.String,
                    Value = PhysicalLocation,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
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
