using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class AltTexts
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int FileID { get; set; }
        public int PageNum { get; set; }
        public string AltText { get; set; }
        public int Deleted { get; set; }
        public int SaveAsFrequent { get; set; }
        public int FrequentDelete { get; set; }
        public int FrequentPage { get; set; }
        public string FrequentText { get; set; }
        public int ClientID { get; set; }
        public string databasename { get; set; }
        public List<AltTexts> GetAllAltTextListByFileId(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM alt_text WHERE FileID = "+id;
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
                    return Utility.ConvertDataTable<AltTexts>(dt);
                else
                    return null;
            }
        }

        public List<AltTexts> GetAltTextListByClientID(int clientId)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, FrequentPage, FrequentText FROM alt_text WHERE SaveAsFrequent = 1 AND FrequentDelete = 0 AND ClientID="+ clientId + " ORDER BY ID Desc";
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
                    return Utility.ConvertDataTable<AltTexts>(dt);
                else
                    return null;
            }
            
        }

        public List<AltTexts> GetAltTextListByFileId(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, AltText, PageNum FROM alt_text WHERE Deleted = 0 AND FileID = "+id+" ORDER BY ID Desc";
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
                    return Utility.ConvertDataTable<AltTexts>(dt);
                else
                    return null;
            }
        }
        public string UpdateAltTextByFileId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE alt_text SET PageNum = "+PageNum+", AltText = '"+AltText+"' WHERE ID="+ FileID;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();


                conn.Close();
            }
            return ExecutionString;
        }
        public string DeleteByFileId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE alt_text SET Deleted = 1 WHERE ID=" + FileID;


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                

                conn.Close();
            }
            return ExecutionString;
        }

        public string CreateAltText()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "INSERT INTO alt_text (FileID, PageNum, AltText, SaveAsFrequent, FrequentText, FrequentPage, ClientID) VALUES (@FileID, @PageNum, @AltText, @SaveAsFrequent, @FrequentText, @FrequentPage, @ClientID)";

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
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PageNum",
                DbType = DbType.Int32,
                Value = PageNum,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AltText",
                DbType = DbType.String,
                Value = AltText,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SaveAsFrequent",
                DbType = DbType.Int32,
                Value = SaveAsFrequent,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FrequentText",
                DbType = DbType.String,
                Value = FrequentText,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FrequentPage",
                DbType = DbType.Int64,
                Value = FrequentPage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ClientID",
                DbType = DbType.Int32,
                Value = ClientID,
            });
            
        }

    }
}
