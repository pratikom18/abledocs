using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class MenuMaster
    {
        #region "Properties"
        DataContext context = new DataContext();

        public int MenuMasterId { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public string PageUrl { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string IconName { get; set; }
        public bool IsHeaderMenu { get; set; }
		public int Level { get; set; }
		public string ParentChild { get; set; }
		public string ParentName { get; set; }
		#endregion

		public List<MenuMaster> GetMenuMasterList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = string.Empty;
				qry = "SELECT MenuMasterId,MenuName,ParentId,PageUrl,IsActive,DisplayOrder,IsHeaderMenu,IconName FROM menumaster";
				//qry = "WITH UniqueParentMenu AS(";
				//qry += "SELECT  T1.MenuMasterId,";
				//qry += "isnull(stuff((select  distinct(', ' + menu.menuname)";
				//qry += "from MenuMaster menu";
				//qry += "inner";
				//qry += "join MenuMaster menum on menu.MenuMasterId in (select string from dbo.ufn_CSVToTable(menu.ParentID)) where menu.MenuMasterId in (select string from dbo.ufn_CSVToTable(T1.ParentID))";
				//qry += "for xml path('')";
				//qry += "),1,2,('')),'') ParentName";
				//qry += "FROM MenuMaster AS T1";
				//qry += "),";
				//qry += "CTE AS";
				//qry += "(";
				//qry += "SELECT MenuMasterId,";
				//qry += "menuname ,";
				//qry += "0 AS Level,";
				//qry += "CAST(menuname AS VARCHAR(MAX)) AS ParentChild,";
				//qry += "ParentID,";
				//qry += "isActive,";
				//qry += "pageurl,";
				//qry += "DisplayOrder,";
				//qry += "isnull(stuff((select  distinct(', ' + menu.menuname)";
				//qry += "from MenuMaster menu";
				//qry += "inner";
				//qry += "join MenuMaster menum on menu.MenuMasterId in (select string from dbo.ufn_CSVToTable(menum.ParentID)) where menu.MenuMasterId in (select string from dbo.ufn_CSVToTable(MM.ParentID))";
				//qry += "for xml path('')";
				//qry += "),1,2,('')),'') ParentName,";
				//qry += "MM.IsHeaderMenu,";
				//qry += "MM.IconName";
				//qry += "FROM MenuMaster MM";
				//qry += "WHERE parentID = '0'";
				//qry += "UNION ALL";
				//qry += "SELECT T1.MenuMasterId,";
				//qry += "T1.menuname,";
				//qry += "CTE.Level + 1 AS Level,";
				//qry += "CAST(CTE.ParentChild + ' >> ' + T1.menuname AS VARCHAR(MAX)) AS ParentChild,";
				//qry += "T1.ParentID,";
				//qry += "T1.isActive,";
				//qry += "T1.pageurl,";
				//qry += "T1.DisplayOrder,";
				//qry += "UPM.ParentName,";
				//qry += "T1.IsHeaderMenu,";
				//qry += "T1.IconName";
				//qry += "FROM UniqueParentMenu AS UPM";
				//qry += "INNER JOIN MenuMaster T1 on T1.MenuMasterId = UPM.MenuMasterId";
				//qry += "INNER JOIN CTE ON  CTE.MenuMasterId in (select String from dbo.ufn_CSVToTable(T1.ParentID))";
				//qry += ")";
				//qry += "SELECT*";
				//qry += "FROM CTE order by ParentChild";

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
                    return Utility.ConvertDataTable<MenuMaster>(dt);
                else
                    return null;
            }
        }

        public List<MenuMaster> GetParentMenuList()
        {
            List<MenuMaster> objlist = new List<MenuMaster>();
            try
            {
                
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();
                    string qry = string.Empty;
                    qry = "SELECT MenuMasterId,MenuName,ParentId,PageUrl,DisplayOrder,IconName FROM menumaster WHERE ParentId=0 " +
                           "UNION ALL " +
                            "SELECT MenuMasterId,MenuName,ParentId,PageUrl,DisplayOrder,IconName FROM menumaster WHERE ParentId IN (SELECT MenuMasterId FROM menumaster WHERE ParentId=0) AND PageUrl='#'";


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
                        return Utility.ConvertDataTable<MenuMaster>(dt);
                    else
                        return objlist;
                }
            }
            catch (Exception ex) {
                return objlist;
            }
            
        }

        public MenuMaster GetMenuMasterById(int menuMasterId)
        {
            MenuMaster u = new MenuMaster();
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "Select MenuMasterId,MenuName,ParentId,PageUrl,IsActive,DisplayOrder,IsHeaderMenu,IconName FROM menumaster WHERE MenuMasterId= " + menuMasterId;
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
                    u = ConvertDataTable<MenuMaster>(dt.Rows[0].Table).FirstOrDefault();

                }
            }
            return u;
        }

        public string Upsert()
        {
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
                {
                    conn.Open();
                    if (MenuMasterId == 0)
                    {
                        string query = "INSERT INTO MenuMaster(MenuName,ParentId,PageUrl,IsActive,DisplayOrder,IsHeaderMenu,IconName) VALUES(@menuname,@parentid,@pageurl,@isactive,@displayorder,@isheadermenu,@IconName)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);

                        cmd.ExecuteNonQuery();
                        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                        ExecutionString = LastID.ToString();

                    }
                    else
                    {
                        string query = "UPDATE MenuMaster SET MenuName=@menuname,ParentId=@parentid,PageUrl=@pageurl,IsActive=@isactive,DisplayOrder=@displayorder,IsHeaderMenu=@isheadermenu,IconName=@IconName WHERE MenuMasterId=@menumasterid";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        BindId(cmd);

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

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@menumasterid",
                DbType = DbType.Int32,
                Value =MenuMasterId,
            });
        }
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@menuname",
                DbType = DbType.String,
                Value = MenuName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@parentid",
                DbType = DbType.Int32,
                Value = ParentId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pageurl",
                DbType = DbType.String,
                Value = PageUrl,
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
                DbType = DbType.Int32,
                Value = DisplayOrder,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@isheadermenu",
                DbType = DbType.Boolean,
                Value = IsHeaderMenu,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IconName",
                DbType = DbType.String,
                Value = IconName,
            });
        }
    }
}
