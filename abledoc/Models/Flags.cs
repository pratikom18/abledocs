using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class Flags
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        #endregion

        public Flags GetFlagByName(string name)
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();
                string qry = "SELECT * FROM flags WHERE name = @name";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@name",
                    DbType = DbType.String,
                    Value = name,
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
                    return ConvertDataTable<Flags>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }
    }
}
