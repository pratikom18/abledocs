using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ErrorReport
    {
        #region "Properties"
        DataContext context = new DataContext();
        public Int32 ID { get; set; }
        public uint JobID { get; set; }
        public uint FileID { get; set; }
        public string ErrorReportName { get; set; }
        public string PhysicalPath { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<ErrorReport> GetErrorReportList(int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM error_report WHERE FileID = "+ fileID + " ORDER BY ID Desc";
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
                    return Utility.ConvertDataTable<ErrorReport>(dt);
                else
                    return null;
            }
        }

        public ErrorReport GetErrorReportListByID(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM error_report WHERE ID = "+ID+" LIMIT 1";
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
                    return Utility.ConvertDataTable<ErrorReport>(dt).FirstOrDefault();
                else
                    return null;
            }
        }
        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "INSERT INTO error_report (JobID, FileID, ErrorReportName) VALUES (@JobID, @FileID, @ErrorReportName)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParams(cmd);
              
                cmd.ExecuteNonQuery();
                int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                ExecutionString = LastID.ToString();



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdatePhysicalPath()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "Update error_report SET PhysicalPath=@PhysicalPath WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@PhysicalPath",
                    DbType = DbType.String,
                    Value = PhysicalPath,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



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
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ErrorReportName",
                DbType = DbType.String,
                Value = ErrorReportName,
            });
            
        }
    }
}
