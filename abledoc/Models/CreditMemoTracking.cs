using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class CreditMemoTracking
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public decimal CreditMemoAmount { get; set; }
        public int UID { get; set; }
        public string OperationType { get; set; }
        public DateTime LastUpdated { get; set; }
        public string PhysicalLocation { get; set; }
        public int CreditMemoID { get; set; }
        public string databasename { get; set; }
        #endregion

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "Insert into credit_memo_tracking(JobID, CreditMemoAmount, UID, OperationType, CreditMemoID) Values(@JobID,@CreditMemoAmount,@UID,'CreditMemoGenerated',@CreditMemoID)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
               
                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString()); 

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CreditMemoAmount",
                DbType = DbType.Decimal,
                Value = CreditMemoAmount,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UID",
                DbType = DbType.Int32,
                Value = UID,
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CreditMemoID",
                DbType = DbType.Int32,
                Value = CreditMemoID,
            });
            

        }

        public string UpdatePhysicalLocationByID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE credit_memo_tracking SET PhysicalLocation = @PhysicalLocation WHERE ID = @ID";
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
