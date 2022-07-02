using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class AssignedMenu
    {
        #region "Properties"
        DataContext context = new DataContext();

        public int menumasterid { get; set; }
        public string menuname { get; set; }
        public string pageurl { get; set; }
        public double ParentID { get; set; }
        public int DisplayOrder { get; set; }
        public string IconName { get; set; }
        public bool isview { get; set; }
        public bool isadd { get; set; }
        public bool isupdate { get; set; }
        public bool isdelete { get; set; }
        public bool IsHeaderMenu { get; set; }
        public string databasename { get; set; }

        #endregion

        public List<AssignedMenu> GetAssignedMenuList(int userid)
        {
            
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = string.Empty;
                qry = "select distinct m.menumasterid,m.menuname ,m.pageurl,',' + m.ParentID + ',' as ParentID,m.DisplayOrder,m.IconName ,ur.isview,ur.isadd,ur.isupdate,ur.isdelete,m.IsHeaderMenu	";
	            qry += " from MenuMaster m";
                qry += " inner join rolespermissions as ur on ur.MenuID = m.menumasterid and ur.isview = 1";
                qry += " inner join  users as u on FIND_IN_SET(ur.RoleID,u.UserRoleId)";
                qry += " where u.id = "+ userid + " and m.isactive=1 order by m.DisplayOrder asc,ur.isview desc,ur.isadd desc,ur.isupdate desc ,ur.isdelete desc";
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
                    return Utility.ConvertDataTable<AssignedMenu>(dt);
                else
                    return null;
            }
        }

        public List<AssignedMenu> GetAssignedMenuList1(int userid)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = string.Empty;
                qry = "select distinct m.menumasterid,m.menuname ,m.pageurl,',' + m.ParentID + ',' as ParentID,m.DisplayOrder,m.IconName ,m.IsHeaderMenu ";
                qry += " from MenuMaster m";
                qry += " inner join rolespermissions as ur on ur.MenuID = m.menumasterid and ur.isview = 1";
                qry += " inner join  users as u on FIND_IN_SET(ur.RoleID,u.UserRoleId)";
                qry += " where u.id = " + userid + " and m.isactive=1 order by m.DisplayOrder asc,ur.isview desc,ur.isadd desc,ur.isupdate desc ,ur.isdelete desc";
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
                    return Utility.ConvertDataTable<AssignedMenu>(dt);
                else
                    return null;
            }
        }
    }
}
