using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static abledoc.Models.Utility;

namespace abledoc.Models
{
    public class ADSales
    {
        #region "Properties"
        DataContext context = new DataContext();
        public int adsalesid { get; set; }
        public int clientid { get; set; }
        public int contactid { get; set; }
        public string title { get; set; }
        public decimal value { get; set; }
        public string currency { get; set; }
        public string pipeline { get; set; }
        public string stage { get; set; }
        public string expecteddate { get; set; }
        public int ownerid { get; set; }
        public string visibleto { get; set; }
        public DateTime createddate { get; set; }
        public int userid { get; set; }
        public string Company { get; set; }
        public string currencysymbol { get; set; }
        public string currencycode { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
        public List<CommonMaster> Pipelinelist { get; set; }
        public List<ADSales> adsaleslist { get; set; }
        public List<ADSalesPhone> adsalesphonelist { get; set; }
        public List<ADSalesEmail> adsalesemaillist { get; set; }
        #endregion
        public List<ADSales> GetADSalesList()
        {
            using (MySqlConnection conn = context.GetConnection(Constants.ABLEDOCS_DB))
            {
                conn.Open();

                string qry = "select a.*,c.Company,co.currency as currencysymbol,co.currency_code as currencycode FROM DBNAME.adsales as a " +
                             "left join DBNAME.clients as c on c.id = a.clientid " +
                             "left join COMMONDBNAME.countries as co on co.code = a.currency ";
                             

                qry = qry.Replace("COMMONDBNAME", Constants.COMMON_DB);
                string myqry_oth = qry.Replace("DBNAME", Constants.ABLEDOCS_DB);
                string myqry_eu = qry.Replace("DBNAME", Constants.ABLEDOCS_EU_DB);

                string myqry3 = "ORDER BY adsalesid ASC";

                qry = myqry_oth + " UNION ALL " + myqry_eu + " " + myqry3;
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
                    return Utility.ConvertDataTable<ADSales>(dt);
                else
                    return null;
            }
        }

        public ADSales GetADSalesbyId()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
            {
                conn.Open();

                string qry = "select * FROM adsales where adsalesid = " + adsalesid;
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
                    return Utility.ConvertDataTable<ADSales>(dt).FirstOrDefault();
                else
                    return null;
            }
        }

        public string Upsert()
        {
            var DatabaseConnection = Models.Utility.getDatabase(databasename);
            string ExecutionString = "";
            try
            {
                using (MySqlConnection conn = context.GetConnection(DatabaseConnection))
                {
                    conn.Open();
                    //Password = abledoc.Utility.CommonHelper.EncryptString(Password);
                    if (adsalesid == 0)
                    {
                        string query = "INSERT INTO adsales(clientid,contactid,title,value,currency,pipeline,stage,expecteddate,ownerid,visibleto,createddate,userid) VALUES(@clientid,@contactid,@title,@value,@currency,@pipeline,@stage,@expecteddate,@ownerid,@visibleto,NOW(),@userid)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);

                        cmd.ExecuteNonQuery();
                        int LastID = Convert.ToInt32(cmd.LastInsertedId.ToString());

                        ExecutionString = LastID.ToString();

                    }
                    else
                    {
                        string query = "UPDATE adsales SET clientid=@clientid,contactid=@contactid,title=@title,value=@value,currency=@currency,pipeline=@pipeline,stage=@stage,expecteddate=@expecteddate,ownerid=@ownerid,visibleto=@visibleto,userid=@userid WHERE adsalesid=@adsalesid";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        BindParams(cmd);
                        BindId(cmd);

                        cmd.ExecuteNonQuery();

                        ExecutionString = adsalesid.ToString();
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
                ParameterName = "@adsalesid",
                DbType = DbType.Int32,
                Value = adsalesid,
            });
        }
        private void BindParams(MySqlCommand cmd)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clientid",
                DbType = DbType.Int32,
                Value = clientid,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@contactid",
                DbType = DbType.Int32,
                Value = contactid,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = title,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@value",
                DbType = DbType.Decimal,
                Value = value,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@currency",
                DbType = DbType.String,
                Value = currency,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pipeline",
                DbType = DbType.String,
                Value = pipeline,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@stage",
                DbType = DbType.String,
                Value = stage,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@expecteddate",
                DbType = DbType.String,
                Value = expecteddate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ownerid",
                DbType = DbType.Int32,
                Value = ownerid
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@visibleto",
                DbType = DbType.String,
                Value = visibleto,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@userid",
                DbType = DbType.Int32,
                Value = userid
            });
        }
    }
}
