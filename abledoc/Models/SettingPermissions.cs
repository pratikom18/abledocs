using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class SettingPermissions
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int SettingPermissionsID { get; set; }
        public int RoleID { get; set; }
        public int SettingID { get; set; }
        public bool IsView { get; set; }
        public int ID { get; set; }
        public string PageName { get; set; }
        public string SectionName { get; set; }

        public List<SettingPermissions> SettingPermissionsList { get; set; }
        #endregion

        public List<SettingPermissions> GetSettingPermissionList(int roleid)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT m.ID,m.PageName,m.SectionName,UR.SettingPermissionsID,UR.RoleID,UR.SettingID,UR.IsView " +
                            "FROM setting AS m " +
                            "LEFT JOIN setting_permissions UR ON m.ID = UR.SettingID AND UR.roleid =" + roleid +
                            " WHERE m.Active = TRUE";
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
                    return Utility.ConvertDataTable<SettingPermissions>(dt);
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
                        string query = "INSERT INTO setting_permissions(RoleID,SettingID,IsView) VALUES(@roleid,@settingid,@view)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);

                        cmd.ExecuteNonQuery();
                        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                        ExecutionString = LastID.ToString();

                    }
                    else
                    {
                        string query = "UPDATE setting_permissions SET IsView=@view WHERE RoleID=@roleid and SettingID=@settingid";

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
                string qry = "SELECT SettingPermissionsID FROM setting_permissions WHERE RoleID= '" + RoleID + "' and SettingID='" + SettingID + "'";
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
                ParameterName = "@roleid",
                DbType = DbType.Int64,
                Value = RoleID,
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
    }
}
