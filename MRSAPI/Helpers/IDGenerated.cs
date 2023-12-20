

using MRSAPI.Data;
using MRSAPI.Gateway;
using Oracle.ManagedDataAccess.Client;

namespace MRSAPI.Helpers
{
    public class IDGenerated
    {

        private readonly MRSDbContext _db;
        //private readonly DBHelper _dBHelper;

        public IDGenerated(MRSDbContext db, DBHelper dBHelper)
        {
            _db = db;
            //_dBHelper = dBHelper;
        }
        //DBConnection dbConnection = new DBConnection();
        //DBHelper dbHelper = new DBHelper();
        public Int64 getMAXSL(string columnName, string tableName)
        {
            Int64 MAXID = 0;
            string QueryString = "select NVL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
            OracleConnection oracleConnection = new OracleConnection(_db.GetConnectionString());
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
            OracleDataReader rdr = oracleCommand.ExecuteReader();
            if (rdr.Read())
            {
                MAXID = Convert.ToInt64(rdr["id"].ToString());
            }
            rdr.Close();
            oracleConnection.Close();
            return MAXID;
        }
        public string getMAXID(string columnName, string tableName, string fm9)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            OracleConnection oracleConnection = new OracleConnection(_db.GetConnectionString());
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
            OracleDataReader rdr = oracleCommand.ExecuteReader();
            if (rdr.Read())
            {
                MAXID = rdr[0].ToString();
            }
            rdr.Close();
            oracleConnection.Close();
            return MAXID;
        }
        public string getRequisitionMAXID(string columnName, string tableName, string fm9)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            OracleConnection oracleConnection = new OracleConnection(_db.GetConnectionString());
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
            OracleDataReader rdr = oracleCommand.ExecuteReader();
            if (rdr.Read())
            {
                MAXID = rdr[0].ToString();
            }
            rdr.Close();
            oracleConnection.Close();
            return MAXID;
        }
    }
}