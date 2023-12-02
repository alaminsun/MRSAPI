using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MRS.DAL.Gateway
{
    public class DBConnection
    {
        string connectionString = "";
        public DBConnection()
        {
            StringRead();
        }

        public string StringRead()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Conn"].ToString();
            return connectionString;
        }
    }
}