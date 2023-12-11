using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MRSAPI.Repository
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly DBHelper _dbHelper;
        private readonly MRSDbContext _db;
        public InstitutionRepository(DBHelper dbHelper, MRSDbContext db)
        {
            _dbHelper = dbHelper;
            _db = db;
        }
        public List<InstitutionModel> GetInstitutionList(string name)
        {
            List<InstitutionModel> listData = new List<InstitutionModel>();
            string query = " select  INS.INSTI_CODE,INS.INSTI_NAME,INS.INSTI_TYPE_CODE,F.INSTI_TYPE_NAME,INS.ADDRESS1,INS.ADDRESS2  , " +
                         " INS.ADDRESS3,INS.ADDRESS4,INS.INST_PHONE, INS.UPAZILA_CODE,  d.UPAZILA_NAME,  D.DISTC_CODE,D.DISTC_NAME, NVL(INS.NO_OF_BEDS,0) NO_OF_BEDS,  " +
                         " nvl(INS.AVG_NO_ADMT_PATI,0)AVG_NO_ADMT_PATI, nvl(INS.AVG_NO_OD_PATI,0)AVG_NO_OD_PATI ,INS.REMARKS  from INSTITUTION INS  " +
                         " Left Join VW_DISTRICT_UPAZILA d on INS.UPAZILA_CODE=d.UPAZILA_CODE  Left Join INSTITUTION_TYPE f on INS.INSTI_TYPE_CODE=F.INSTI_TYPE_CODE Where 1=1";

            if (name != "")
            {
                query += " AND UPPER(INS.INSTI_NAME) LIKE '%" + name.ToUpper() + "%'";
            }
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        InstitutionModel model = new InstitutionModel();
                        model.INSTI_CODE = reader["INSTI_CODE"].ToString();
                        model.INSTI_NAME = reader["INSTI_NAME"].ToString();
                        model.INSTI_TYPE_CODE = reader["INSTI_TYPE_CODE"].ToString();
                        model.INSTI_TYPE_NAME = reader["INSTI_TYPE_NAME"].ToString();
                        model.ADDRESS1 = reader["ADDRESS1"].ToString();
                        model.ADDRESS2 = reader["ADDRESS2"].ToString();
                        model.ADDRESS3 = reader["ADDRESS3"].ToString();
                        model.ADDRESS4 = reader["ADDRESS4"].ToString();
                        model.INST_PHONE = reader["INST_PHONE"].ToString();
                        model.UPAZILA_CODE = reader["UPAZILA_CODE"].ToString();
                        model.UPAZILA_NAME = reader["UPAZILA_NAME"].ToString();
                        model.DISTC_CODE = reader["DISTC_CODE"].ToString();
                        model.DISTC_NAME = reader["DISTC_NAME"].ToString();
                        model.NO_OF_BEDS = Convert.ToInt32(reader["NO_OF_BEDS"].ToString());
                        model.AVG_NO_ADMT_PATI = Convert.ToInt32(reader["AVG_NO_ADMT_PATI"].ToString());
                        model.AVG_NO_OD_PATI = Convert.ToInt32(reader["AVG_NO_OD_PATI"].ToString());
                        model.REMARKS = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        //_dataTable = _dbHelper.GetDataTable(Qry);

        //    List<InstitutionModel> itemList = new List<InstitutionModel>();
        //    itemList = (from DataRow row in _dataTable.Rows
        //                select new InstitutionModel
        //                {


        //                }).ToList();
        //    return itemList;
        //}
    }
}
