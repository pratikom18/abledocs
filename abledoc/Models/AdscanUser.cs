using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class AdscanUser
    {
        DataContext context = new DataContext();
        public int ID { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public DateTime Created_At { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone_Num { get; set; }
        public string Company_Address { get; set; }
        public string Postal_Code { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public int ADO_Contact_ID { get; set; }
        public string Lang { get; set; }
        public string Role { get; set; }
        public string databasename { get; set; }

        public AdscanUser GetAdscanUserById(int Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adscan_user WHERE Id = "+ Id +" LIMIT 1";
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
                    return ConvertDataTable<AdscanUser>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }
        public AdscanUser GetAdscanUserByContactId(int Id)
        {
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();
                string qry = "SELECT * FROM adscan_user WHERE ADO_Contact_ID = " + Id + " LIMIT 1";
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
                    return ConvertDataTable<AdscanUser>(dt.Rows[0].Table).FirstOrDefault();
                else
                    return null;
            }
        }

        public string Insert()
        {
            string ExecutionString = "";
            try
            {
                var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();
                    string query = "INSERT INTO adscan_user (ADO_Contact_ID, Role, Lang, Email, Pwd, CompanyID, CompanyName, Created_At, First_Name, Last_Name, Company_Address, City, Province, Postal_Code, Country, Phone_Num) " +
                       "VALUES (@ADO_Contact_ID, @Role, @Lang, @Email, @Pwd, @CompanyID, @CompanyName, @Created_At, @First_Name, @Last_Name,@Company_Address, @City, @Province, @Postal_Code, @Country, @Phone_Num)";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    BindBugParams(cmd);

                    cmd.ExecuteNonQuery();
                    int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    ExecutionString = LastID.ToString();

                    conn.Close();
                }
                return ExecutionString;
            }
            catch (Exception ex) { return ExecutionString; }
            
        }

        public string UpdatePwd()
        {
            string ExecutionString = "";
            var DatabaseConnection = Models.Utility.getDatabase_adscan(databasename);

            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();


                string query = "UPDATE adscan_user SET Pwd = @Pwd WHERE ADO_Contact_ID=@ID";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                BindId(cmd);
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Pwd",
                    DbType = DbType.String,
                    Value = Pwd,
                });
               

                cmd.ExecuteNonQuery();
                int LastID = ID;

                ExecutionString = LastID.ToString();

                conn.Close();
            }
            return ExecutionString;
        }
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ADO_Contact_ID,
            });
        }
        private void BindBugParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ADO_Contact_ID",
                DbType = DbType.Int32,
                Value = ADO_Contact_ID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Role",
                DbType = DbType.String,
                Value = Role,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Lang",
                DbType = DbType.String,
                Value = Lang,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Email",
                DbType = DbType.String,
                Value = Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Pwd",
                DbType = DbType.String,
                Value = Pwd,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CompanyID",
                DbType = DbType.Int32,
                Value = CompanyID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CompanyName",
                DbType = DbType.String,
                Value = CompanyName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Created_At",
                DbType = DbType.DateTime,
                Value = DateTime.Today//abledoc.Utility.CommonHelper.ConvertToTimestamp(DateTime.Today),
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@First_Name",
                DbType = DbType.String,
                Value = First_Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Last_Name",
                DbType = DbType.String,
                Value = Last_Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Company_Address",
                DbType = DbType.String,
                Value = Company_Address,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@City",
                DbType = DbType.String,
                Value = City,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Province",
                DbType = DbType.String,
                Value = Province,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Postal_Code",
                DbType = DbType.String,
                Value = Postal_Code,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Country",
                DbType = DbType.String,
                Value = Country,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Phone_Num",
                DbType = DbType.String,
                Value = Phone_Num,
            });

        }
    }
}
