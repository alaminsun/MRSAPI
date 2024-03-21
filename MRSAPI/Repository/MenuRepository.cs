using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Helpers;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace MRSAPI.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DBHelper _dbHelper;
        private readonly MRSDbContext _db;
        private readonly IDGenerated _iDGenerated;
        long mxSl = 0;
        public MenuRepository(DBHelper dbHelper, MRSDbContext db, IDGenerated iDGenerated)
        {
            _dbHelper = dbHelper;
            _db = db;
            _iDGenerated = iDGenerated;
        }
        public List<Menu> GetMenuList()
        {
            List<Menu> listData = new List<Menu>();
            //string query = "Select ID,FROM_MARKET From OPERATIONS_MASTER Where ID = " + mstId + "";
            string query = "Select * From MENULIST";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Menu model = new Menu();
                        model.Id = Convert.ToInt32(reader["ID"]);
                        model.Name = reader["NAME"].ToString();
                        model.Type = reader["TYPE"].ToString();
                        model.Order = Convert.ToInt32(reader["ORDER"]);
                        model.IsActive = reader["ISACTIVE"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
    }
}
