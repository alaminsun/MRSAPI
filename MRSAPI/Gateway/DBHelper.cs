using MRSAPI.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace MRSAPI.Gateway
{
    public class DBHelper
    {
        private readonly MRSDbContext _db;

        public DBHelper(MRSDbContext db)
        {
            _db = db;
        }

        public DataTable GetDataTable(string Qry)
        {
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, _db.GetConnectionString());
            DataTable dt = new DataTable();
            oracleDataAdapter.Fill(dt);
            return dt;
        }

        public int CmdExecute(string Qry)
        {
            int noOfRows = 0;
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(Qry, con);
                con.Open();
                noOfRows = cmd.ExecuteNonQuery();
            }
            return noOfRows;

            //OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            //oracleConnection.Open();
            //OracleCommand oracleCommand = new OracleCommand(Qry, oracleConnection);
            //int noOfRows = oracleCommand.ExecuteNonQuery();
            //oracleConnection.Close();
            //return noOfRows;
        }


    public T ExecuteScalar<T>(string query, object parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(_db.GetConnectionString()))
        {
            connection.Open();

            using (OracleCommand command = new OracleCommand(query, connection))
            {
                // Add parameters if any
                if (parameters != null)
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        OracleParameter oracleParameter = new OracleParameter(":" + prop.Name, prop.GetValue(parameters));
                        command.Parameters.Add(oracleParameter);
                    }
                }

                // Execute the command and return the scalar value
                object result = command.ExecuteScalar();

                // Convert the result to the desired type
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }


}
}