using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class UserRoles
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<UserRoles> GetUserRolesList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT RoleId, RoleName,IsActive,DisplayOrder FROM UserRoles  ORDER BY DisplayOrder ASC";
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
                    return Utility.ConvertDataTable<UserRoles>(dt);
                else
                    return null;
            }
        }

        public UserRoles GetUserRoleById(int roleId)
        {
            UserRoles u = new UserRoles();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT RoleId, RoleName,IsActive,DisplayOrder FROM UserRoles WHERE RoleId= " + roleId;
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
                    u = ConvertDataTable<UserRoles>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public string Upsert()
        {
            string ExecutionString = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                if (RoleId == 0)
                {
                    string query = "INSERT INTO userroles(RoleName,IsActive,DisplayOrder) VALUES(@rolename,@isactive,@displayorder)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();

                }
                else
                {
                    string query = "UPDATE userroles SET RoleName=@rolename,IsActive=@isactive,DisplayOrder=@displayorder WHERE RoleId=@roleid";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);

                    cmd.ExecuteNonQuery();

                    ExecutionString = "Update Succefully";
                }
                conn.Close();
            }
            return ExecutionString;
        }

        public List<UserRoles> GetUserRoleByUserId(int userid)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "select r.RoleId,r.RoleName from userroles as r";
                qry += " inner join users as u on FIND_IN_SET(r.RoleId, u.UserRoleId)";
                qry += " where id = "+ userid;
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
                    return Utility.ConvertDataTable<UserRoles>(dt);
                else
                    return null;
            }
            
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@roleid",
                DbType = DbType.Int32,
                Value = RoleId,
            });
        }
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rolename",
                DbType = DbType.String,
                Value = RoleName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@isactive",
                DbType = DbType.Boolean,
                Value = IsActive,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@displayorder",
                DbType = DbType.Int64,
                Value = DisplayOrder,
            });
        }
    }
}
