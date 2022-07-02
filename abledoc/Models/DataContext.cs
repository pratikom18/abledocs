using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class DataContext
    {
        //public string ConnectionString { get; set; }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public DataContext()
        {
           
        }
        public MySqlConnection GetConnection(string Databasename)
        {
            var configuation = GetConfiguration();
            //this.ConnectionString = configuation.GetConnectionString(Databasename);
            return new MySqlConnection(configuation.GetConnectionString(Databasename));
        }
    }
}
