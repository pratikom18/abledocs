using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class BrandImage
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public int ClientID { get; set; }
        public string PhysicalPath { get; set; }
        public string AltTextImage { get; set; }
        public int Deleted { get; set; }
        public string databasename { get; set; }
        public List<BrandImage> GetBrandImageByClientID(int clientId)
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM brand_image WHERE ClientID="+ clientId + " AND Deleted=0";
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
                    return Utility.ConvertDataTable<BrandImage>(dt);
                else
                    return null;
            }
        }
    }
}
