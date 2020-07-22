using BitTrusterWebApi.Helper;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrusterWebApi.Model
{
    public class DbContext
    {
        public static string ConnectionString { get; set; }
        private string GetConnectionString()
        {
            return ConnectionString; //AppSettings.ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
