using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class UserPermissions
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int UserPermissionsID { get; set; }
        public int UserID { get; set; }
        public int SettingID { get; set; }
        public bool IsView { get; set; }
        public int ID { get; set; }
        public string PageName { get; set; }
        public string SectionName { get; set; }
        public List<UserPermissions> UserPermissionsList { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<UserPermissions> GetUserPermissionList(int userid,string roleid)
        {
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT m.ID,m.PageName,m.SectionName,UR.UserID,UR.UserPermissionsID,UR.SettingID,UR.IsView "+
                            "FROM setting AS m " +
                            "INNER JOIN setting_permissions SP ON SP.SettingID = m.ID " +
                            "LEFT JOIN User_Permissions UR ON m.ID = UR.SettingID AND UR.userid = "+ userid+
                            " WHERE m.Active = TRUE  AND SP.RoleID IN("+ roleid + ") AND SP.IsView = TRUE";
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
                    return Utility.ConvertDataTable<UserPermissions>(dt);
                else
                    return null;
            }
        }

        public string Upsert()
        {
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();
                    bool isExist = IsExist();
                    if (!isExist)
                    {
                        string query = "INSERT INTO User_Permissions(UserID,SettingID,IsView) VALUES(@userid,@settingid,@view)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);

                        cmd.ExecuteNonQuery();
                        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                        ExecutionString = LastID.ToString();

                    }
                    else
                    {
                        string query = "UPDATE User_Permissions SET IsView=@view WHERE UserID=@userid and SettingID=@settingid";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);

                        cmd.ExecuteNonQuery();

                        ExecutionString = "Update Succefully";
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        public bool IsExist()
        {
            bool isexist = false;
            //Password = abledoc.Utility.CommonHelper.EncryptString(Password);
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT UserPermissionsID FROM User_Permissions WHERE UserID= '" + UserID + "' and SettingID='" + SettingID + "'";
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
                {
                    isexist = true;
                }
            }
            return isexist;
        }

        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@userid",
                DbType = DbType.Int64,
                Value = UserID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@settingid",
                DbType = DbType.Int64,
                Value = SettingID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@view",
                DbType = DbType.Boolean,
                Value = IsView,
            });


        }

        public List<Setting> GetSettingList(int userid,string pagename)
        {
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT m.ID,m.PageName,m.SectionName " +
                            "FROM setting AS m " +
                            "INNER JOIN User_Permissions UR ON UR.SettingID = m.ID " +
                            " WHERE m.Active = TRUE  AND UR.userid = " + userid + " AND UR.IsView = TRUE AND m.PageName = '"+ pagename + "'";
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
                    return Utility.ConvertDataTable<Setting>(dt);
                else
                    return null;
            }
        }
    }
}
