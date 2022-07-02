using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class JobsFilesReviews
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int VersionID { get; set; }
        public int ReviewerID { get; set; }
        public int FileID { get; set; }
        public int JobID { get; set; }
        public string Comments { get; set; }
        public Int64 TagOrderE { get; set; }
        public Int64 ContentOrderE { get; set; }
        public Int64 TabbingOrderE { get; set; }
        public Int64 IncorrectTagsE { get; set; }
        public Int64 TableStructureE { get; set; }
        public Int64 SideBySideE { get; set; }
        public Int64 AssemblyE { get; set; }
        public Int64 FiguresTextE { get; set; }
        public Int64 FiguresMissedE { get; set; }
        public Int64 HeadingsE { get; set; }
        public Int64 ReferencesMissedE { get; set; }
        public Int64 ReferencesTypeE { get; set; }
        public Int64 SpaceIssuesE { get; set; }
        public Int64 FormFieldPropertiesE { get; set; }
        public Int64 FormFieldTooltipsE { get; set; }
        public Int64 FormFieldTypeE { get; set; }
        public Int64 ArtifactE { get; set; }
        public Int64 ListStructureE { get; set; }
        public Int64 LinksE { get; set; }
        public Int64 LanguageAttributesE { get; set; }
        public Int64 CSRE { get; set; }
        public Int64 DocumentNamingE { get; set; }
        public Int64 TextInFiguresE { get; set; }
        public Int64 MissedTextE { get; set; }
        public Int64 MissedArtifactingE { get; set; }
        public Int64 LinksNotLiveE { get; set; }
        public Int64 ToolTipE { get; set; }
        public Int64 FormObjectOrderE { get; set; }
        public Int64 MissedSpacesE { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Deleted { get; set; }
        public int TaggedBy { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<JobsFilesReviews> GetRLC(int jobID, int fileID)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT j.*,CONCAT(u.FirstName, ' ' ,u.LastName) AS FullName FROM jobs_files_reviews j " +
                                "Left JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID " +
                                "WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                "ORDER BY ID DESC";
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
                    return Utility.ConvertDataTable<JobsFilesReviews>(dt);
                else
                    return null;
            }
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

                    string query = "INSERT INTO jobs_files_reviews (VersionID,ReviewerID,FileID,JobID,LastUpdated) VALUES (@VersionID,@ReviewerID,@FileID,@JobID,@LastUpdated)";

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
                ParameterName = "@VersionID",
                DbType = DbType.Int32,
                Value = VersionID
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ReviewerID",
                DbType = DbType.Int32,
                Value = ReviewerID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FileID",
                DbType = DbType.Int32,
                Value = FileID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastUpdated",
                DbType = DbType.DateTime,
                Value = LastUpdated,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@JobID",
                DbType = DbType.Int32,
                Value = JobID,
            });

        }

        public string Update()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {

                conn.Open();
                string query = "UPDATE jobs_files_reviews SET Comments = @Comments," +
                  "TagOrderE = @TagOrder," +
                  "ContentOrderE = @ContentOrder," +
                  "TabbingOrderE = @TabbingOrder," +
                  "IncorrectTagsE = @IncorrectTags," +
                  "AssemblyE = @Assembly," +
                  "FiguresTextE = @FiguresText," +
                  "FiguresMissedE = @FiguresMissed," +
                  "HeadingsE = @Headings," +
                  "ReferencesMissedE = @ReferencesMissed," +
                  "ReferencesTypeE = @ReferencesType," +
                  "SpaceIssuesE = @SpaceIssues," +
                  "FormFieldPropertiesE = @FormFieldProperties," +
                  "FormFieldTooltipsE = @FormFieldTooltips," +
                  "FormFieldTypeE = @FormFieldType," +
                  "ArtifactE = @Artifact," +
                  "TableStructureE = @TableStructure," +
                  "ListStructureE = @ListStructure," +
                  "LinksE = @Links," +
                  "LanguageAttributesE = @LanguageAttributes," +
                  "SideBySideE = @SideBySide," +
                  "CSRE = @CSR," +
                  "DocumentNamingE = @DocumentNaming," +
                  "TaggedBy = @TaggedBy" +
                  " WHERE VersionID = @VersionID AND ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindParamsUpdate(cmd);
                cmd.ExecuteNonQuery();

                ExecutionString = "Update Successfully";



                conn.Close();
            }
            return ExecutionString;
        }

        private void BindParamsUpdate(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Comments",
                DbType = DbType.String,
                Value = Comments
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TagOrder",
                DbType = DbType.Int32,
                Value = TagOrderE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ContentOrder",
                DbType = DbType.Int32,
                Value = ContentOrderE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TabbingOrder",
                DbType = DbType.Int32,
                Value = TabbingOrderE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IncorrectTags",
                DbType = DbType.Int32,
                Value = IncorrectTagsE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Assembly",
                DbType = DbType.Int32,
                Value = AssemblyE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FiguresText",
                DbType = DbType.Int32,
                Value = FiguresTextE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FiguresMissed",
                DbType = DbType.Int32,
                Value = FiguresMissedE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Headings",
                DbType = DbType.Int32,
                Value = HeadingsE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ReferencesMissed",
                DbType = DbType.Int32,
                Value = ReferencesMissedE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ReferencesType",
                DbType = DbType.Int32,
                Value = ReferencesTypeE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SpaceIssues",
                DbType = DbType.Int32,
                Value = SpaceIssuesE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FormFieldProperties",
                DbType = DbType.Int32,
                Value = FormFieldPropertiesE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FormFieldTooltips",
                DbType = DbType.Int32,
                Value = FormFieldTooltipsE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FormFieldType",
                DbType = DbType.Int32,
                Value = FormFieldTypeE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Artifact",
                DbType = DbType.Int32,
                Value = ArtifactE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TableStructure",
                DbType = DbType.Int32,
                Value = TableStructureE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ListStructure",
                DbType = DbType.Int32,
                Value = ListStructureE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Links",
                DbType = DbType.Int32,
                Value = LinksE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LanguageAttributes",
                DbType = DbType.Int32,
                Value = LanguageAttributesE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SideBySide",
                DbType = DbType.Int32,
                Value = SideBySideE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CSR",
                DbType = DbType.Int32,
                Value = CSRE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DocumentNaming",
                DbType = DbType.Int32,
                Value = DocumentNamingE,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TaggedBy",
                DbType = DbType.Int32,
                Value = TaggedBy,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@VersionID",
                DbType = DbType.Int32,
                Value = VersionID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });

        }

        public List<JobsFilesReviews> GetCommentsPhase4or5(int jobID, int fileID, string pagename, string Status)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = string.Empty;
                if (pagename == "qc")
                {
                    qry = "SELECT * FROM (SELECT j.Comments,j.LastUpdated,'Phase 3' AS STATUS,CONCAT(u.FirstName, ' ' ,u.LastName) AS FullName " +
                            "FROM jobs_files_qc j LEFT JOIN COMMONDBNAME.users u ON u.ID = j.QualityControllerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                            "UNION ALL " +
                            "SELECT j.Comments,j.LastUpdated,'Phase 2' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName " +
                                "FROM jobs_files_reviews j " +
                                "LEFT JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                            ") AS temp " +
                            "ORDER BY LastUpdated DESC";
                }
                else if (pagename == "finalfile")
                {
                    if (Status == "QC")
                    {
                        qry = "SELECT * FROM (SELECT j.Comments,j.LastUpdated,'Phase 3' AS STATUS,CONCAT(u.FirstName, ' ' ,u.LastName) AS FullName " +
                                "FROM jobs_files_qc j LEFT JOIN COMMONDBNAME.users u ON u.ID = j.QualityControllerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                "UNION ALL " +
                                "SELECT j.Comments,j.LastUpdated,'Phase 2' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName " +
                                "FROM jobs_files_reviews j " +
                                "LEFT JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                ") AS temp " +
                                "ORDER BY LastUpdated DESC";
                    }
                    else if (Status == "FINAL")
                    {
                        qry = "SELECT * FROM ( " +
                                "SELECT j.Comments,j.LastUpdated,'Phase 3' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName,0 AS TagOrderE,0 AS ContentOrderE,0 AS TabbingOrderE,0 AS IncorrectTagsE,0 AS AssemblyE,0 AS FiguresTextE,0 AS FiguresMissedE,0 AS HeadingsE,0 AS ReferencesMissedE,0 AS ReferencesTypeE,0 AS SpaceIssuesE,0 AS FormFieldPropertiesE,0 AS FormFieldTooltipsE,0 AS FormFieldTypeE,0 AS ArtifactE,0 AS TableStructureE,0 AS ListStructureE,0 AS LinksE,0 AS LanguageAttributesE,0 AS SideBySideE,0 AS CSRE,0 AS DocumentNamingE " +
                                "FROM jobs_files_qc j LEFT JOIN COMMONDBNAME.users u ON u.ID = j.QualityControllerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                "UNION ALL " +
                                "SELECT j.Comments,j.LastUpdated,'Phase 2' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName, j.TagOrderE,j.ContentOrderE,j.TabbingOrderE,j.IncorrectTagsE,j.AssemblyE,j.FiguresTextE,j.FiguresMissedE,j.HeadingsE,j.ReferencesMissedE,j.ReferencesTypeE,j.SpaceIssuesE,j.FormFieldPropertiesE,j.FormFieldTooltipsE,j.FormFieldTypeE,j.ArtifactE,j.TableStructureE,j.ListStructureE,j.LinksE,j.LanguageAttributesE,j.SideBySideE,j.CSRE,j.DocumentNamingE " +
                                "FROM jobs_files_reviews j " +
                                "LEFT JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                ") AS temp " +
                                "ORDER BY LastUpdated DESC";
                    }
                    else
                    {
                        qry = "SELECT j.Comments,j.LastUpdated,'Phase 3' AS STATUS,CONCAT(u.FirstName, ' ' ,u.LastName) AS FullName " +
                                    "FROM jobs_files_qc j LEFT JOIN COMMONDBNAME.users u ON u.ID = j.QualityControllerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " ORDER BY LastUpdated DESC";
                    }

                }
                else if (pagename == "tag")
                {
                    qry = "SELECT * FROM ( " +
                                   "SELECT j.Comments,j.LastUpdated,'Phase 2' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName, j.TagOrderE,j.ContentOrderE,j.TabbingOrderE,j.IncorrectTagsE,j.AssemblyE,j.FiguresTextE,j.FiguresMissedE,j.HeadingsE,j.ReferencesMissedE,j.ReferencesTypeE,j.SpaceIssuesE,j.FormFieldPropertiesE,j.FormFieldTooltipsE,j.FormFieldTypeE,j.ArtifactE,j.TableStructureE,j.ListStructureE,j.LinksE,j.LanguageAttributesE,j.SideBySideE,j.CSRE,j.DocumentNamingE " +
                                   "FROM jobs_files_reviews j " +
                                   "LEFT JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                   ") AS temp " +
                                   "ORDER BY LastUpdated DESC";
                }
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
                    return Utility.ConvertDataTable<JobsFilesReviews>(dt);
                else
                    return null;
            }
        }

        public List<JobsFilesReviews> GetCommentsFiles(int jobID, int fileID, string Status)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = string.Empty;
                if (Status == "TAGGING" || Status == "REVIEW")
                {
                    qry = "SELECT * FROM ( " +
                                  "SELECT j.Comments,j.LastUpdated,'Phase 2' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName, j.TagOrderE,j.ContentOrderE,j.TabbingOrderE,j.IncorrectTagsE,j.AssemblyE,j.FiguresTextE,j.FiguresMissedE,j.HeadingsE,j.ReferencesMissedE,j.ReferencesTypeE,j.SpaceIssuesE,j.FormFieldPropertiesE,j.FormFieldTooltipsE,j.FormFieldTypeE,j.ArtifactE,j.TableStructureE,j.ListStructureE,j.LinksE,j.LanguageAttributesE,j.SideBySideE,j.CSRE,j.DocumentNamingE " +
                                  "FROM jobs_files_reviews j " +
                                  "LEFT JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                  ") AS temp " +
                                  "ORDER BY LastUpdated DESC";
                }
                else if (Status == "FINAL" || Status == "QC")
                {
                    qry = "SELECT * FROM ( " +
                                    "SELECT j.Comments,j.LastUpdated,'Phase 3' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName,0 AS TagOrderE,0 AS ContentOrderE,0 AS TabbingOrderE,0 AS IncorrectTagsE,0 AS AssemblyE,0 AS FiguresTextE,0 AS FiguresMissedE,0 AS HeadingsE,0 AS ReferencesMissedE,0 AS ReferencesTypeE,0 AS SpaceIssuesE,0 AS FormFieldPropertiesE,0 AS FormFieldTooltipsE,0 AS FormFieldTypeE,0 AS ArtifactE,0 AS TableStructureE,0 AS ListStructureE,0 AS LinksE,0 AS LanguageAttributesE,0 AS SideBySideE,0 AS CSRE,0 AS DocumentNamingE " +
                                    "FROM jobs_files_qc j LEFT JOIN COMMONDBNAME.users u ON u.ID = j.QualityControllerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                    "UNION ALL " +
                                    "SELECT j.Comments,j.LastUpdated,'Phase 2' AS STATUS, CONCAT(u.FirstName, ' ', u.LastName) AS FullName, j.TagOrderE,j.ContentOrderE,j.TabbingOrderE,j.IncorrectTagsE,j.AssemblyE,j.FiguresTextE,j.FiguresMissedE,j.HeadingsE,j.ReferencesMissedE,j.ReferencesTypeE,j.SpaceIssuesE,j.FormFieldPropertiesE,j.FormFieldTooltipsE,j.FormFieldTypeE,j.ArtifactE,j.TableStructureE,j.ListStructureE,j.LinksE,j.LanguageAttributesE,j.SideBySideE,j.CSRE,j.DocumentNamingE " +
                                    "FROM jobs_files_reviews j " +
                                    "LEFT JOIN COMMONDBNAME.users u ON u.ID = j.ReviewerID WHERE j.JobID = " + jobID + " AND j.FileID = " + fileID + " " +
                                    ") AS temp " +
                                    "ORDER BY LastUpdated DESC";
                }

                if (!string.IsNullOrEmpty(qry))
                {
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
                        return Utility.ConvertDataTable<JobsFilesReviews>(dt);
                    else
                        return null;

                }
                else
                {
                    return null;
                }
               
            }
        }
    }
}
