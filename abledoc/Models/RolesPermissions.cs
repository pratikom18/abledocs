using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class RolesPermissions
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int rolespermissionid { get; set; }
        public int roleid { get; set; }
        public int menuid { get; set; }
        public string MenuName { get; set; }
        public bool isview { get; set; }
        public bool isadd { get; set; }
        public bool isupdate { get; set; }
        public bool isdelete { get; set; }
        public int MenuMasterID { get; set; }
        public bool AllChk { get; set; }

        public List<RolesPermissions> RolesPermissionsList { get; set; }

        #endregion

        public List<RolesPermissions> GetRolePermissionList(int roleid)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "select m.MenuMasterId,m.MenuName,UR.rolespermissionid,UR.roleid,UR.menuid,UR.isadd,UR.isupdate,UR.isdelete,UR.isview from menumaster as m";
                qry += " LEFT JOIN rolespermissions UR ON m.MenuMasterId = UR.menuid AND UR.roleid = " + roleid + " where m.IsActive=TRUE";
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
                    return Utility.ConvertDataTable<RolesPermissions>(dt);
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
                        string query = "INSERT INTO rolespermissions(roleid,menuid,isview,isadd,isupdate,isdelete) VALUES(@roleid,@menuid,@view,@add,@update,@delete)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);

                        cmd.ExecuteNonQuery();
                        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                        ExecutionString = LastID.ToString();

                    }
                    else
                    {
                        string query = "UPDATE rolespermissions SET isview=@view,isadd=@add,isupdate=@update,isdelete=@delete WHERE roleid=@roleid and menuid=@menuid";

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
                string qry = "SELECT rolespermissionid FROM rolespermissions WHERE roleid= '" + roleid + "' and menuid='" + menuid + "'";
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
                Value = roleid,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@menuid",
                DbType = DbType.Int64,
                Value = menuid,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@view",
                DbType = DbType.Boolean,
                Value = isview,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@add",
                DbType = DbType.Boolean,
                Value = isadd,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@update",
                DbType = DbType.Boolean,
                Value = isupdate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@delete",
                DbType = DbType.Boolean,
                Value = isdelete,
            });

        }
    }
}
