using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Jobstype
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int jobtypeid { get; set; }
        public string jobtype { get; set; }
        public string jobtypename { get; set; }
        public int display_order { get; set; }
        #endregion

        public List<Jobstype> GetJobTypeList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM jobs_type ORDER BY jobtypeid ASC";
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
                    return Utility.ConvertDataTable<Jobstype>(dt);
                else
                    return null;
            }
        }
    }
}
