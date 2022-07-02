using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class MetaDataFiles
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int VersionID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Subject { get; set; }
        public string Keywords { get; set; }
        public string FileCreationDate { get; set; }
        public string FileModificationDate { get; set; }
        public string databasename { get; set; }
        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = "INSERT INTO meta_data_files (VersionID, Title, Author, Pages, Subject, Keywords, FileCreationDate, FileModificationDate) VALUES(@VersionID, @Title, @Author, @Pages, @Subject, @Keywords, @FileCreationDate, @FileModificationDate)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();


                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        public string UpdateMetaDataPages()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = "UPDATE meta_data_files SET Pages = @Pages WHERE VersionID = @VersionID";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    
                    ExecutionString = "Update Successfully";


                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VersionID",
                DbType = DbType.Int32,
                Value = VersionID
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Title",
                DbType = DbType.String,
                Value = Title,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Author",
                DbType = DbType.String,
                Value = Author,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Pages",
                DbType = DbType.Int32,
                Value = Pages,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Subject",
                DbType = DbType.String,
                Value = Subject,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Keywords",
                DbType = DbType.String,
                Value = Keywords,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileCreationDate",
                DbType = DbType.String,
                Value = FileCreationDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileModificationDate",
                DbType = DbType.String,
                Value = FileModificationDate,
            });

        }
    }
}
