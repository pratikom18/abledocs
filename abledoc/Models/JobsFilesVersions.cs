using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesVersions
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int JobID { get; set; }
        public int FileID { get; set; }
        public string State { get; set; }
        public string Filename { get; set; }
        public string ERPFilename { get; set; }
        public string OriginalFilename { get; set; }
        public int Copied { get; set; }
        public int AssignedTo { get; set; }
        public string AssignedToUser { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Physical_Path { get; set; }
        public string Old_Path { get; set; }
        public DateTime LastUpdated { get; set; }
        public int FolderFlagged { get; set; }
        public int FilenameCopyPhase { get; set; }
        public int MetaDataID { get; set; }
        public int Deleted { get; set; }
        public int QCType { get; set; }
        public string FullName { get; set; }
        public  string databasename { get; set; }

        public List<JobsFilesVersions> GetUserListByJobID(long id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT DISTINCT AssignedTo,CONCAT(u.firstname,' ',u.lastname) AS FullName FROM jobs_files_versions j " +
                            "INNER JOIN COMMONDBNAME.users u ON j.AssignedTo = u.ID WHERE JobID = " + id;
                qry = qry.Replace("COMMONDBNAME", Constants.COMMON_DB);
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetFileVersionByFileID(long id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, State, Filename, LastUpdated,Physical_Path FROM jobs_files_versions WHERE FileID = " + id + " AND State <> 'REFERENCE' ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetFileVersionByID(string id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, State, Filename, LastUpdated,Physical_Path FROM jobs_files_versions WHERE ID in (" + id + ") AND State <> 'REFERENCE' ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetFileVersionStateByJobID(long id, string state)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,FileID, Filename FROM jobs_files_versions WHERE JobID = " + id + " AND State='" + state + "' AND Deleted=0 ORDER BY ID ASC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
            }
        }

        public List<JobsFilesVersions> GetFileVersionByJobID(int JobID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_files_versions WHERE 1 AND jobs_files_versions.JobID = " + JobID;
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                // BindId(cmd);
                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
            }
        }

        public List<JobsFilesVersions> GetFileVersionByIDS(string id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_files_versions WHERE ID in (" + id + ") ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetFileVersionByFileIDS(string id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_files_versions WHERE FileID in (" + id + ") AND State <> 'REFERENCE' ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetJobfilesFileTree(int[] id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            List<int> JobID = new List<int>();
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,FileID, State, Filename, LastUpdated, Old_Path, Physical_Path, QCType FROM jobs_files_versions WHERE FileID IN (" + String.Join(",", id) + ") AND Deleted=0 ORDER BY FIELD(State, 'REFERENCE','SOURCE','TAGGING','REVIEW','FINAL','QC'), FIELD(QCType, 0,1,2,3,4), ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetReferenceFile(int id)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT a.ID, a.State, a.Filename, a.LastUpdated FROM jobs_files_versions a, reference_linking b WHERE a.ID = b.RFileID AND b.CFileID = " + id + " ORDER BY a.ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetLinkPopupFile(int jobID, int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT a.ID, a.State, a.Filename, a.LastUpdated FROM jobs_files_versions a WHERE a.JobID IN (SELECT ID FROM jobs WHERE ClientID IN (SELECT ClientID FROM jobs WHERE ID = " + jobID + ")) AND a.State='CLOSED' AND a.ID NOT IN (SELECT b.RFileID FROM reference_linking b WHERE b.CFileID=" + fileID + ") ORDER BY a.ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public string DeleteFileVersion(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);

            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files_versions SET Deleted = 1 WHERE ID = " + ID;

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            return ExecutionString;
        }

        public string DeleteFileVersionSource(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string query = "UPDATE jobs_files_versions SET Deleted = 1 WHERE FileID =  " + ID + " AND State = 'SOURCE'";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            return ExecutionString;
        }

        public string Insert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = "INSERT INTO jobs_files_versions (JobID, FileID, Filename, LastUpdated, State, Type, QCType, Size) VALUES (@JobID,@FileID,@Filename,@LastUpdated,@State,@Type,@QCType,@Size)";

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

        public string InsertByCopyJob()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();

                    string query = "Insert Into jobs_files_versions(JobID, FileID, State, Filename, Type, Size, Physical_Path, MetaDataID, Deleted) Values(@JobID, @FileID, @State, @Filename, @Type, @Size, @Physical_Path, @MetaDataID, @Deleted)";

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

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Filename",
                DbType = DbType.String,
                Value = Filename,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastUpdated",
                DbType = DbType.DateTime,
                Value = LastUpdated,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@State",
                DbType = DbType.String,
                Value = State,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Type",
                DbType = DbType.String,
                Value = Type,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@QCType",
                DbType = DbType.String,
                Value = QCType,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Size",
                DbType = DbType.String,
                Value = Size,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Physical_Path",
                DbType = DbType.String,
                Value = Physical_Path,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MetaDataID",
                DbType = DbType.Int32,
                Value = MetaDataID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Deleted",
                DbType = DbType.Int32,
                Value = Deleted,
            });

        }

        public string UpdatePhysicalPath()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "UPDATE jobs_files_versions SET Physical_Path=@Physical_Path WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Physical_Path",
                    DbType = DbType.String,
                    Value = Physical_Path,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateJobFileVersionByCopyJob()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "Update jobs_files_versions SET Filename=@Filename, Physical_Path=@Physical_Path WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Physical_Path",
                    DbType = DbType.String,
                    Value = Physical_Path,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Filename",
                    DbType = DbType.String,
                    Value = Filename,
                });
                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public string UpdateMetaDataID()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string query = "UPDATE jobs_files_versions SET MetaDataID=@MetaDataID WHERE ID=@ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@ID",
                    DbType = DbType.Int32,
                    Value = ID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@MetaDataID",
                    DbType = DbType.Int32,
                    Value = MetaDataID,
                });

                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        public JobsFilesVersions GetFileVersionByID(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID,FileID,JobID, Filename, Physical_Path FROM jobs_files_versions WHERE ID=@ID";
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
                    return ConvertDataTable<JobsFilesVersions>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public JobsFilesVersions GetFileVersionStateByFileID(int FileID, string State)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, Filename, Physical_Path FROM jobs_files_versions WHERE State=@State AND FileID=@FileID ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FileID",
                    DbType = DbType.Int32,
                    Value = FileID,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@State",
                    DbType = DbType.String,
                    Value = State,
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
                    return ConvertDataTable<JobsFilesVersions>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public List<JobsFilesVersions> GetJobsFilesVersionsReferenceListByFileID(int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT ID, Physical_Path, Filename FROM jobs_files_versions WHERE FileID=" + fileID + " AND State='REFERENCE'";
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
                    return Utility.ConvertDataTable<JobsFilesVersions>(dt);
                else
                    return null;
            }
        }

        public JobsFilesVersions GetJobsFilesVersionsByFileID(int ID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_files_versions WHERE FileID=@ID ORDER BY ID DESC LIMIT 1";
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
                    return ConvertDataTable<JobsFilesVersions>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }

        }

        public JobsFilesVersions GetJobsFilesVersionsLastData()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_files_versions ORDER BY ID DESC LIMIT 1";
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
                    return ConvertDataTable<JobsFilesVersions>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }

        }

    }
}
