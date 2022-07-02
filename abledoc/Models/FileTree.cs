using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class FileTree
    {
        DataContext context = new DataContext();
        public int DT_RowId { get; set; }
        public int level { get; set; }
        public string key { get; set; }
        public string parent { get; set; }
        public int ID { get; set; }
        public int FileID { get; set; }
        public string State { get; set; }
        public string Filename { get; set; }
        public string Physical_Path { get; set; }
        public Int64 QCType { get; set; }
        public string Deadline { get; set; }
        public string databasename { get; set; }

        public List<FileTree> GetJobListForFirstLevel(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int[] id, out int TotalRecord)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string myqry = "";

                myqry = "SELECT ID,ID AS FileID,'' AS State,Filename,'' AS Physical_Path,0 AS QCType,Deadline " +
                        "FROM jobs_files " +
                        "WHERE ID IN(" + String.Join(",", id) + ") AND Deleted = 0";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (ID LIKE '%" + SearchKey + "%'" +
                        " OR Filename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {
                    myqry = myqry + " order by Deadline desc";
                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                var countQry = "SELECT COUNT(ID) " +
                            "FROM jobs_files " +
                            "WHERE ID IN(" + String.Join(",", id) + ") AND Deleted = 0";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<FileTree>(dt);
                }
                else
                {
                    return new List<FileTree>();
                }
            }
        }

        public List<FileTree> GetJobListForSecondLevel(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int ID, out int TotalRecord,bool ReferenceNotAllow = false)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string myqry = "";
                string Status = "";
                if(ReferenceNotAllow == true)
                {
                    Status = " AND State <> 'REFERENCE'";
                }

                myqry = "SELECT * FROM(SELECT ID,FileID,'' as State,Filename,Physical_Path,0 as QCType " +
                        "FROM jobs_files_versions " +
                        "WHERE FileID IN(" + ID + ") " + Status + " AND Deleted = 0 AND State NOT IN('REFERENCE','SOURCE','TAGGING','REVIEW','FINAL','QC') " +
                        "UNION ALL " +
                        "SELECT ID, FileID AS FileID, State, Filename,'' AS Physical_Path,0 as QCType " +
                        "FROM jobs_files_versions " +
                        "WHERE FileID IN(" + ID + ") " + Status + " AND Deleted = 0 AND State IN('REFERENCE', 'SOURCE', 'TAGGING', 'REVIEW', 'FINAL', 'QC') " +
                        "GROUP BY State) AS Temp";

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (State LIKE '%" + SearchKey + "%'" +
                        " OR Filename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else {
                    myqry = myqry + " ORDER BY FIELD(State, 'REFERENCE', 'SOURCE', 'TAGGING', 'REVIEW', 'FINAL', 'QC'),ID";
                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                var countQry = "SELECT COUNT(*) FROM(SELECT ID,FileID,'' as State,Filename,Physical_Path,0 as QCType " +
                                "FROM jobs_files_versions " +
                                "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State NOT IN('REFERENCE','SOURCE','TAGGING','REVIEW','FINAL','QC') " +
                                "UNION ALL " +
                                "SELECT ID, FileID AS FileID, State, Filename,'' AS Physical_Path,0 as QCType " +
                                "FROM jobs_files_versions " +
                                "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('REFERENCE', 'SOURCE', 'TAGGING', 'REVIEW', 'FINAL', 'QC') " +
                                "GROUP BY State) AS Temp";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<FileTree>(dt);
                }
                else
                {
                    return new List<FileTree>();
                }
            }
        }

        public List<FileTree> GetJobListForThiredLevel(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int ID, string State, int QcType, out int TotalRecord)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string myqry = "";
                if (State == "QC" && QcType == 0)
                {
                    myqry = "SELECT ID,FileID,State,Filename,Physical_Path, QCType " +
                        "FROM jobs_files_versions " +
                        "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "') "+
                        "GROUP BY QCType,FileID";
                }
                else if (State != "QC" && QcType == 0) {
                    myqry = "SELECT ID,FileID,'' as State,Filename,Physical_Path,0 as QCType " +
                            "FROM jobs_files_versions " +
                            "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "')";
                }else if (State == "QC" && QcType != 0)
                {
                    myqry = "SELECT ID, FileID,'' as State,Filename, Physical_Path,0 as QCType "+
                            "FROM jobs_files_versions "+
                            "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "') AND QCType = " + QcType + "";
                }


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (State LIKE '%" + SearchKey + "%'" +
                        " OR Filename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                var countQry = "";
                if (State == "QC" && QcType == 0)
                {
                    countQry = "SELECT COUNT(*) " +
                        "FROM jobs_files_versions " +
                        "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "') " +
                        "GROUP BY QCType,FileID";
                }
                else if (State != "QC" && QcType == 0)
                {
                    countQry = "SELECT COUNT(*) " +
                            "FROM jobs_files_versions " +
                            "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "')";
                }
                else if (State == "QC" && QcType != 0)
                {
                    countQry = "SELECT COUNT(*) " +
                            "FROM jobs_files_versions " +
                            "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "') AND QCType = " + QCType + "";
                }

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<FileTree>(dt);
                }
                else
                {
                    return new List<FileTree>();
                }
            }
        }

        public List<FileTree> GetJobListForDownloadFileTreeLevel(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int ID, string State, out int TotalRecord)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string myqry = "";
                
                    myqry = "SELECT ID,FileID,'' as State,Filename,Physical_Path,0 as QCType " +
                            "FROM jobs_files_versions " +
                            "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "')";
                

                if (SearchKey != "")
                {
                    myqry = myqry + " AND (State LIKE '%" + SearchKey + "%'" +
                        " OR Filename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                var countQry = "";
                
                    countQry = "SELECT COUNT(*) " +
                            "FROM jobs_files_versions " +
                            "WHERE FileID IN(" + ID + ") AND Deleted = 0 AND State IN('" + State + "')";

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<FileTree>(dt);
                }
                else
                {
                    return new List<FileTree>();
                }
            }
        }


        public List<FileTree> GetJobListForComplianceReportFirstLevel(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int ID, out int TotalRecord)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string myqry = "";
                
                  myqry = "SELECT * FROM jobs_files_versions WHERE JobID = "+ ID +" AND Deleted = 0 AND state = 'QC' GROUP BY qctype";
                


                if (SearchKey != "")
                {
                    myqry = myqry + " AND (State LIKE '%" + SearchKey + "%'" +
                        " OR Filename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                var countQry = "";
                
                    countQry = "SELECT COUNT(*) FROM jobs_files_versions WHERE JobID = " + ID + " AND Deleted = 0 AND state = 'QC' GROUP BY qctype";
                

                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<FileTree>(dt);
                }
                else
                {
                    return new List<FileTree>();
                }
            }
        }

        public List<FileTree> GetJobListForComplianceReportSecondLevel(int inStartIndex, int inEndIndex, string OrderBy, string SearchKey, int ID, string State, int QcType, out int TotalRecord)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string myqry = "";

                myqry = "SELECT * FROM jobs_files_versions WHERE JobID = "+ ID +" AND Deleted = 0 AND state = '"+ State +"' AND qctype="+ QcType + "";



                if (SearchKey != "")
                {
                    myqry = myqry + " AND (State LIKE '%" + SearchKey + "%'" +
                        " OR Filename LIKE '%" + SearchKey + "%'" +
                        ")";
                }
                if (OrderBy != "")
                {
                    myqry = myqry + " order by " + OrderBy;
                }
                else
                {
                    myqry = myqry + " ORDER BY FileID Asc";
                }

                if (inEndIndex != -1)
                {
                    myqry = myqry + " LIMIT @end OFFSET @start";
                }

                MySqlCommand cmd = new MySqlCommand(myqry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@start",
                    DbType = DbType.Int32,
                    Value = inStartIndex,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@end",
                    DbType = DbType.Int32,
                    Value = inEndIndex,
                });
                //cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC;";
                // DataTable dt =cmd.ExecuteReader();

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                int count = 0;
                //MySqlCommand cou = new MySqlCommand("SELECT COUNT(*) FROM jobs Where " + Status, conn);
                var countQry = "";

                countQry = "SELECT COUNT(*) FROM jobs_files_versions WHERE JobID = " + ID + " AND Deleted = 0 AND state = '" + State + "' AND qctype=" + QcType;


                MySqlCommand cou = new MySqlCommand(countQry, conn);

                count = Convert.ToInt32(cou.ExecuteScalar());


                TotalRecord = count;
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecord = count;//Convert.ToInt32(dt.Rows[0]["TotalRecord"]);
                    return Utility.ConvertDataTable<FileTree>(dt);
                }
                else
                {
                    return new List<FileTree>();
                }
            }
        }

    }
}
