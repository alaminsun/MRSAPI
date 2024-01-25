using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Helpers;
using MRSAPI.Models;
using MRSAPI.Models.DTO;
using MRSAPI.Repository.IRepository;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace MRSAPI.Repository
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly DBHelper _dbHelper;
        private readonly MRSDbContext _db;
        private readonly IDGenerated _iDGenerated;
        private readonly IHttpContextAccessor _httpContextAccessor;
        long mxSl = 0;

        public InstitutionRepository(DBHelper dbHelper, MRSDbContext db, IDGenerated iDGenerated, IHttpContextAccessor httpContextAccessor)
        {
            _dbHelper = dbHelper;
            _db = db;
            _iDGenerated = iDGenerated;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<bool> CreateInstitute(InstitutionInfoModel model)
        {
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            bool isTrue = false;
            try
            {
                mxSl = _iDGenerated.getMAXSL("INSTI_CODE", "INSTITUTION Where INSTI_CODE not in (99999)");
                string saveQuery = "INSERT INTO INSTITUTION (INSTI_CODE,INSTI_NAME,INSTI_TYPE_CODE,ADDRESS1,INST_PHONE,DISTC_CODE,UPAZILA_CODE,NO_OF_BEDS,AVG_NO_ADMT_PATI,AVG_NO_OD_PATI,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES(" + mxSl + ",'" + model.InstituteName + "','" + model.InstituteTypeCode + "','" + model.Address + "','" + model.InstitutePhone + "','" + model.DistrictCode + "','" + model.UpazilaCode + "'," + model.NoOfBeds + "," + model.AvgNoAdmitPatient + "," + model.AvgNoOutDoorPatient + ",'" + model.Remarks + "', '" + model.EmployeeId + "',(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')),'" + ip + "') ";

                if (_dbHelper.CmdExecute(saveQuery) > 0)
                {
                    isTrue = true;
                    model.InstituteCode=mxSl.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return isTrue;
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

        

        public List<InstitutionTypeModel> GetInstitutionTypeList()
        {
            List<InstitutionTypeModel> listData = new List<InstitutionTypeModel>();
            string query = " select INSTI_TYPE_CODE, INSTI_TYPE_NAME from INSTITUTION_TYPE ";
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        InstitutionTypeModel model = new InstitutionTypeModel();
                        model.InstituteTypeCode = reader["INSTI_TYPE_CODE"].ToString();
                        model.InstituteTypeName = reader["INSTI_TYPE_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public InstitutionInfoModel GetInstitutionById(int id)
        {
            InstitutionInfoModel model = new InstitutionInfoModel();
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select * from INSTITUTION Where INSTI_CODE = " + id + "";
                //long doctor_Id = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        model.InstituteCode = reader["INSTI_CODE"].ToString();
                        model.InstituteName = reader["INSTI_NAME"].ToString();
                        model.InstituteCode = reader["INSTI_TYPE_CODE"].ToString();
                        //INSTI_TYPE_NAME = reader["INSTI_TYPE_NAME"].ToString();
                        model.Address = reader["ADDRESS1"].ToString();
                        //ADDRESS2 = reader["ADDRESS2"].ToString();
                        //ADDRESS3 = reader["ADDRESS3"].ToString();
                        //ADDRESS4 = reader["ADDRESS4"].ToString();
                        model.InstitutePhone = reader["INST_PHONE"].ToString();
                        model.UpazilaCode = reader["UPAZILA_CODE"].ToString();
                        //UPAZILA_NAME = reader["UPAZILA_NAME"].ToString();
                        model.DistrictCode = reader["DISTC_CODE"].ToString();
                        //DISTC_NAME = reader["DISTC_NAME"].ToString();
                        model.NoOfBeds = Convert.ToInt32(reader["NO_OF_BEDS"].ToString());
                        model.AvgNoAdmitPatient = Convert.ToInt32(reader["AVG_NO_ADMT_PATI"].ToString());
                        model.AvgNoOutDoorPatient = Convert.ToInt32(reader["AVG_NO_OD_PATI"].ToString());
                        model.Remarks = reader["REMARKS"].ToString();
                    }

                }
            }
            return model;
        }
    }
}
