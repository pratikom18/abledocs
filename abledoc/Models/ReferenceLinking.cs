using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ReferenceLinking
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int CFileID { get; set; }
        public int CJobID { get; set; }
        public int RFileID { get; set; }
        public string databasename { get; set; }
        #endregion

        public string Insert(int JobID,int FileID, string RFileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                for (int i = 0; i < RFileID.Split(",").Length; i++)
                {
                    int refID = abledoc.Utility.CommonHelper.GetDBInt(RFileID.Split(",").GetValue(i));

                    string query = "INSERT INTO reference_linking (CFileID, CJobID, RFileID) VALUES (@CFileID,@CJobID,@RFileID)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@CFileID",
                        DbType = DbType.Int32,
                        Value = FileID,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@CJobID",
                        DbType = DbType.Int32,
                        Value = JobID,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@RFileID",
                        DbType = DbType.Int32,
                        Value = refID,
                    });
                    cmd.ExecuteNonQuery();
                }
               

                conn.Close();
            }
            return ExecutionString;
        }

        public string InsertBySendToProduction()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                    string query = "INSERT INTO reference_linking (CFileID, CJobID, RFileID) VALUES (@CFileID,@CJobID,@RFileID)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@CFileID",
                        DbType = DbType.Int32,
                        Value = CFileID,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@CJobID",
                        DbType = DbType.Int32,
                        Value = CJobID,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@RFileID",
                        DbType = DbType.Int32,
                        Value = RFileID,
                    });
                    cmd.ExecuteNonQuery();
                


                conn.Close();
            }
            return ExecutionString;
        }

        public List<ReferenceLinking> GetReferenceLinkingByJob()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                var qry = "SELECT * FROM reference_linking WHERE CJobID=@CJobID";


                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@CJobID",
                    DbType = DbType.Int32,
                    Value = CJobID,
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
                    return ConvertDataTable<ReferenceLinking>(dt);
                else
                    return null;
            }
        }
    }
}
