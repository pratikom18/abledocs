using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Users
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int ID { get; set; }
        public string ERPID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string Tagger { get; set; }
        public string Reviewer { get; set; }
        public string Finalizer { get; set; }
        public string QC { get; set; }
        public string FilesDashboard { get; set; }
        public string JobsDashboard { get; set; }
        public string Quote { get; set; }
        public string AltText { get; set; }
        public string FilesAdmin { get; set; }
        public string InvoiceDeactivate { get; set; }
        public string ClosedDeactivate { get; set; }
        public string Admin { get; set; }
        public string Supervisor { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Title { get; set; }
        public string ProductionStaff { get; set; }
        public Int64 CurrentSupervisor { get; set; }
        public string PermissionGranter { get; set; }
        public Int64 NotificationCount { get; set; }
        public string Hourly { get; set; }
        public string Salary { get; set; }
        public string DashboardTimesheet { get; set; }
        public string DashboardErrorFrequency { get; set; }
        public string DashboardFileProgress { get; set; }
        public string DashboardOpenJob { get; set; }
        public string DashboardProductionCount { get; set; }
        public string ClientsManagement { get; set; }
        public string ClientsContactsManagement { get; set; }
        public string QuotesList { get; set; }
        public string ToBeInvoiced { get; set; }
        public string ApprovedInvoiceScreen { get; set; }
        public string Search { get; set; }
        public string UserRoleId { get; set; }
        public string FullName { get; set; }
        public string RealName { get; set; }
        public string Cookie_String { get; set; }
        public string Country { get; set; }
        public string databasename { get; set; }
        #endregion

        public List<Users> GetUsersList()
        {

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT ID, FirstName, LastName, username,email, CONCAT(FirstName, ' ', LastName) as FullName, Country FROM users ORDER BY FirstName, LastName ASC";

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
                    return Utility.ConvertDataTable<Users>(dt);
                else
                    return null;
            }
        }

        public List<Users> GetActiveUsersList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT ID, FirstName, LastName, username,email, CONCAT(FirstName, ' ', LastName) as FullName,Country FROM users WHERE Active=1 ORDER BY FirstName, LastName ASC";
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
                    return Utility.ConvertDataTable<Users>(dt);
                else
                    return null;
            }
        }

        public string GetUsernameById(int Id)
        {
            string username = "";
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT u.ID,u.FirstName, u.LastName " +
                             "FROM users u " +
                             "inner join userroles AS ur  ON FIND_IN_SET(ur.RoleID, u.userroleid) " +
                             "where Active= 1 AND (ur.RoleName = 'FilesDashboard' OR ur.RoleName = 'ProductionStaff') " +
                             "And ID= " + Id;
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
                    Users u = ConvertDataTable<Users>(dt.Rows[0].Table).FirstOrDefault();
                    username = u.FirstName + " " + u.LastName[0] + ".";
                }

            }
            return username;
        }

        public List<Users> GetAssignedUsersList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                //string qry = "SELECT ID, FirstName, LastName,CONCAT(FirstName,' ',LastName) as FullName FROM users WHERE Active=1 AND (FilesDashboard='1' OR ProductionStaff='1') ORDER BY FirstName ASC, LastName ASC";
                //string qry = "SELECT ID, FirstName, LastName FROM users WHERE Active=1 AND (FilesDashboard='1' OR ProductionStaff='1') ORDER BY FirstName ASC, LastName ASC";
                //string qry = "SELECT ID, FirstName, LastName,CONCAT(FirstName,' ',LastName) as FullName FROM users WHERE Active=1 AND (FilesDashboard='1' OR ProductionStaff='1') ORDER BY FirstName ASC, LastName ASC";
                string qry = "SELECT distinct ID, FirstName, LastName, CONCAT(FirstName, ' ', LastName) as FullName " +
                             "FROM users u " +
                             "inner join userroles AS ur  ON FIND_IN_SET(ur.RoleID, u.userroleid) " +
                             "where Active= 1 AND (ur.RoleName = 'FilesDashboard' OR ur.RoleName = 'ProductionStaff') " +
                             "ORDER BY FirstName ASC, LastName ASC";

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
                    return Utility.ConvertDataTable<Users>(dt);
                else
                    return null;
            }
        }

        public List<Users> GetSupervisorList()
        {
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                // string qry = "SELECT ID, CONCAT(FirstName,' ', LastName) as FullName FROM users Where Supervisor = 1  ORDER BY FirstName, LastName ASC";
                string qry = "SELECT distinct ID, FirstName, LastName, CONCAT(FirstName, ' ', LastName) as FullName " +
                             "FROM users u " +
                             "inner join abledocs.userroles AS ur  ON FIND_IN_SET(ur.RoleID, u.userroleid) " +
                             "where Active= 1 AND (ur.RoleName = 'Supervisor') " +
                             "ORDER BY FirstName ASC, LastName ASC";
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
                    return Utility.ConvertDataTable<Users>(dt);
                else
                    return new List<Users>();
            }
        }

        public Users GetUserById(int Id)
        {
            
            Users u = new Users();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT ID,FirstName, LastName,UserName,Email,Password,Title,CurrentSupervisor,UserRoleId, Country FROM users WHERE ID= " + Id;
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
                    u = ConvertDataTable<Users>(dt.Rows[0].Table).FirstOrDefault();
                    //if(u.Password != "")
                    //{
                    //    u.Password = abledoc.Utility.CommonHelper.DecryptString(u.Password);
                    //}

                }
            }
            return u;
        }

        public Users GetActiveUserById(int Id)
        {
            
            Users u = new Users();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT ID,FirstName, LastName,UserName,Email,Password,Title,CurrentSupervisor,UserRoleId,Country FROM users WHERE ID= " + Id + " AND Active=1";
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
                    u = ConvertDataTable<Users>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public Users GetDataByEmailPassword()
        {
            Users u = new Users();

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT ID,FirstName, LastName,UserName,Email,Password,Title,CurrentSupervisor,UserRoleId,Country FROM users WHERE UserName= '" + Username + "' and Password='" + Password + "'";
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
                    u = ConvertDataTable<Users>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }
        public string Upsert()
        {
            string ExecutionString = "";
            if (ID == 0)
            {
                if (IsExist())
                {
                    ExecutionString = "-999";
                }
                else
                {
                    Insert();
                }
            }
            else {
                if (IsExistUpdate())
                {
                    ExecutionString = "-999";
                }
                else
                {
                    Update();
                }
                
            }
           
            return ExecutionString;
        }

        public string Insert()
        {
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();
                    //Password = abledoc.Utility.CommonHelper.EncryptString(Password);
                    
                        string query = "INSERT INTO users(FirstName,LastName,Username,Email,Title,Password,CurrentSupervisor,UserRoleId,Country) VALUES(@firstName,@lastName,@username,@email,@title,@password,@currentsupervisor,@userroleid,@Country)";

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

        public string Update()
        {
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();
                    //Password = abledoc.Utility.CommonHelper.EncryptString(Password);
                    
                        string query = "UPDATE users SET FirstName=@firstName,LastName=@lastName,Username=@username,Email=@email,Title=@title,CurrentSupervisor=@currentsupervisor,UserRoleId=@userroleid,Country = @Country WHERE ID=@id";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        BindId(cmd);

                        cmd.ExecuteNonQuery();

                        ExecutionString = "Update Succefully";
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        public string UpdatePassword()
        {
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();
                    //Password = abledoc.Utility.CommonHelper.EncryptString(Password);

                    string query = "UPDATE users SET Password=@Password WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);

                    cmd.ExecuteNonQuery();

                    ExecutionString = "Update Succefully";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        public string UpdateProfile()
        {
            string ExecutionString = "";
            if (IsExistUpdate())
            {
                ExecutionString = "-999";
            }
            else
            {
                updateprofile1();
            }

            return ExecutionString;
        }

        public string updateprofile1() {
      
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();

                    string query = "UPDATE users SET FirstName=@firstName,LastName=@lastName,Username=@username,Title=@title WHERE ID=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindParams(cmd);
                    BindId(cmd);

                    cmd.ExecuteNonQuery();

                    ExecutionString = "Update Succefully";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }


            return ExecutionString;
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = ID,
            });
        }
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastname",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = Username,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = Title,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = Password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@currentsupervisor",
                DbType = DbType.Int64,
                Value = CurrentSupervisor,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@userroleid",
                DbType = DbType.String,
                Value = UserRoleId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Country",
                DbType = DbType.String,
                Value = Country
            });
        }

        public int GetUserIDByUserName()
        {
            int isexist = 0;
     
            using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))//Constants.Mantis_DB
            {
                conn.Open();
                string qry = "SELECT id FROM user WHERE username LIKE @userName LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userName",
                    DbType = DbType.String,
                    Value = Username,
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
                {
                    isexist = abledoc.Utility.CommonHelper.GetDBInt(dt.Rows[0]["id"]);
                }
            }
            return isexist;
        }

        public string InsertUpdateUserBugReport()
        {
            string ExecutionString = "";
            
            using (MySqlConnection conn = context.GetConnection(Constants.Mantis_DB))//Constants.Mantis_DB
            {
                conn.Open();
                int isExist = GetUserIDByUserName();
                if (isExist == 0)
                {
                    string query = "INSERT INTO user (username, realname, email, password, cookie_string)" +
                    " VALUES(@username, @realname, @email, @password, @cookie_string)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindUserBugReportParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();
                }
                else
                {
                    ExecutionString = ID.ToString();
                }
                conn.Close();
            }
            return ExecutionString;
        }

        private void BindUserBugReportParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = Username,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@realname",
                DbType = DbType.String,
                Value = RealName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = Password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cookie_string",
                DbType = DbType.String,
                Value = Cookie_String,
            });


        }

        public List<Users> GetOwnerList(int ID)
        {
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT ID, FirstName, LastName, username,email, CONCAT(FirstName, ' ', LastName) as FullName, Country FROM users where id=" + ID + " ORDER BY FirstName, LastName ASC";
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
                    return Utility.ConvertDataTable<Users>(dt);
                else
                    return null;
            }
        }

        public bool IsExist()
        {
            bool isexist = false;
            //Password = abledoc.Utility.CommonHelper.EncryptString(Password);

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT id FROM users WHERE Username='" + Username + "'";
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


        public bool IsExistUpdate()
        {
            bool isexist = false;
            //Password = abledoc.Utility.CommonHelper.EncryptString(Password);

            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT id FROM users WHERE id = "+ID+" or Username ='" + Username + "'";
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                MySqlDataReader dr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.EnforceConstraints = false;
                dt.Load(dr);
                dr.Close();
                conn.Close();

                if (dt != null && dt.Rows.Count > 1)
                {
                    isexist = true;
                }
            }
            return isexist;
        }

    }
}
