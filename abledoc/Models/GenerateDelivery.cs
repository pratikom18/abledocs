using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class GenerateDelivery
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public Int64 JobID { get; set; }
        public string AllJFVID { get; set; }
        public string DownloadKey { get; set; }
        public Int64 UID { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime LinkExpiry { get; set; }
        public string DeliveryMessage { get; set; }
        public string databasename { get; set; }
        #endregion


        public GenerateDelivery GetGenerateDeliveryByJobID(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "Select * From generate_delivery Where JobID=@JobID Order By ID DESC Limit 1";
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
                    return ConvertDataTable<GenerateDelivery>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string InsertGenerateDelivery()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "Insert into generate_delivery(JobID, LinkExpiry, AllJFVID, DownloadKey, UID, DeliveryMessage) Values(@JobID, @LinkExpiry, @AllJFVID, @DownloadKey, @UID, @DeliveryMessage)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@JobID",
                    DbType = DbType.Int32,
                    Value = JobID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@LinkExpiry",
                    DbType = DbType.DateTime,
                    Value = LinkExpiry
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@AllJFVID",
                    DbType = DbType.String,
                    Value = AllJFVID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@DownloadKey",
                    DbType = DbType.String,
                    Value = DownloadKey
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@UID",
                    DbType = DbType.Int32,
                    Value = UID
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@DeliveryMessage",
                    DbType = DbType.String,
                    Value = DeliveryMessage
                });
                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }
    }
}
