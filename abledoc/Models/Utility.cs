using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public static class Utility
    {
        public static class Constants
        {
            public const string ABLEDOCS_DB = "abledocs_live";
            public const string ADSCAN_DB = "adscan";
            public const string GATEWAY_DB = "gateway";
            public const string Mantis_DB = "mantis";
            public const string LEGACY_DB = "legacy";
            public const string ABLEDOCS_EU_DB = "abledocs_eu_live";
            public const string ADSCAN_EU_DB = "adscan_eu";
            public const string GATEWAY_EU_DB = "gateway_eu";
            public const string Mantis_EU_DB = "mantis_eu";
            public const string LEGACY_EU_DB = "legacy_eu";
            public const string COMMON_DB = "abledocs_live";
            public const string VERSION = "0.3";
        }

        public static bool isElementExist(dynamic ar, string key)
        {
            bool retval = false;
            foreach (string item in ar)
            {
                if (item.Equals(key))
                {
                    retval = true;
                }
            }
            return retval;
        }

        
        public static class JobStatus
        {
            
           public const string OPEN = "OPEN";
           public const string PENDING = "PENDING";
           public const string QUOTE = "QUOTE";
           public const string TOBEDELIVERED = "TOBEDELIVERED";
           public const string DELIVERED = "DELIVERED";

           public const string DEFAULTSTATUS = "PENDING";
            

        }
        public static class File_Status
        {

            public const string PENDING = "Pending";
            public const string TAGGING = "Phase 1";
            public const string REVIEW = "Phase 2";
            public const string FINAL = "Phase 4";
            public const string QC = "Phase 3";
            public const string COMPLETE = "Final";
            public const string DELIVERY = "Delivery";
            public const string TOBEDELIVERED = "To Be Delivered";

        }

        public static class File_Search_Status
        {

            public const string PENDING = "PENDING";
            public const string Phase1 = "TAGGING";
            public const string Phase2 = "REVIEW";
            public const string Phase3 = "OPEN";
            public const string Phase4 = "FINAL";
            public const string Phase5 = "QC";
            public const string Final = "COMPLETE";
            public const string Delivery = "DELIVERY";
            public const string ToBeDelivered = "TOBEDELIVERED";

        }



        public static string getSearchStatus(string status)
        {
            string FileStatus = "";
            if (status == "phase 1")
            {
                FileStatus = File_Search_Status.Phase1;
            }
            else if (status == "phase 2")
            {
                FileStatus = File_Search_Status.Phase2;
            }
            else if (status == "phase 3")
            {
                FileStatus = File_Search_Status.Phase3;
            }
            else if (status == "phase 4")
            {
                FileStatus = File_Search_Status.Phase4;
            }
            else if (status == "phase 5")
            {
                FileStatus = File_Search_Status.Phase5;
            }
            else if (status == "to be delivered")
            {
                FileStatus = File_Search_Status.ToBeDelivered;
            }
            else if (status == "pending")
            {
                FileStatus = File_Search_Status.PENDING;
            }
            //else if (status == "TOBEDELIVERED")
            //{
            //    FileStatus = File_Status.TOBEDELIVERED;
            //}
            return FileStatus;
        }


        public static string getStatus(string status)
        {
            string FileStatus = "";
            if (status == "PENDING")
            {
                FileStatus = File_Status.PENDING;
            }
            else if (status == "TAGGING")
            {
                FileStatus = File_Status.TAGGING;
            }
            else if (status == "REVIEW")
            {
                FileStatus = File_Status.REVIEW;
            }
            else if (status == "FINAL")
            {
                FileStatus = File_Status.FINAL;
            }
            else if (status == "QC")
            {
                FileStatus = File_Status.QC;
            }
            else if (status == "COMPLETE")
            {
                FileStatus = File_Status.COMPLETE;
            }
            else if (status == "DELIVERY")
            {
                FileStatus = File_Status.DELIVERY;
            }
            else if (status == "TOBEDELIVERED")
            {
                FileStatus = File_Status.TOBEDELIVERED;
            }
            return FileStatus;
        }

        public static string getDatabase(string databasename = "")
        {
            var DatabaseConnection = Constants.ABLEDOCS_DB;
            if (databasename == Constants.ABLEDOCS_EU_DB)
            {
                DatabaseConnection = Constants.ABLEDOCS_EU_DB;
            }
            else if (databasename == Constants.ABLEDOCS_DB)
            {

                DatabaseConnection = Constants.ABLEDOCS_DB;
            }
            /*else if (databasename == "abledocs_ca")
            {

                DatabaseConnection = Constants.ABLEDOCS_CA_DB;
            }*/
            return DatabaseConnection;
        }

        public static string getDatabase_adscan(string databasename = "")
        {
            var DatabaseConnection = Constants.ADSCAN_DB;
            if (databasename == Constants.ADSCAN_EU_DB)
            {
                DatabaseConnection = Constants.ADSCAN_EU_DB;
            }
            else if (databasename == Constants.ADSCAN_DB)
            {

                DatabaseConnection = Constants.ADSCAN_DB;
            }
            
            return DatabaseConnection;
        }
        public static string getDatabase_gateway(string databasename = "")
        {
            var DatabaseConnection = Constants.GATEWAY_DB;
            if (databasename == Constants.GATEWAY_EU_DB)
            {
                DatabaseConnection = Constants.GATEWAY_EU_DB;
            }
            else if (databasename == Constants.GATEWAY_DB)
            {

                DatabaseConnection = Constants.GATEWAY_DB;
            }
            
            return DatabaseConnection;
        }
        public static string getDatabase_mantis(string databasename = "")
        {
            var DatabaseConnection = Constants.Mantis_DB;
            if (databasename == "mantis_eu")
            {
                DatabaseConnection = Constants.Mantis_EU_DB;
            }
            else if (databasename == "mantis")
            {

                DatabaseConnection = Constants.Mantis_DB;
            }
            
            return DatabaseConnection;
        }
        public static string getDatabase_legacy(string databasename = "")
        {
            var DatabaseConnection = Constants.LEGACY_DB;
            if (databasename == Constants.LEGACY_EU_DB)
            {
                DatabaseConnection = Constants.LEGACY_EU_DB;
            }
            else if (databasename == Constants.LEGACY_DB)
            {

                DatabaseConnection = Constants.LEGACY_DB;
            }
            
            return DatabaseConnection;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLowerInvariant() == column.ColumnName.ToLowerInvariant() && dr[column.ColumnName] != DBNull.Value)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }



        public static DataTable GetDataTables(string qry, MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            ds.EnforceConstraints = false;
            dt.Load(dr);
            dr.Close();
            if (dt != null && dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
    }
    
}
