using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic.FileIO;
using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Helpers;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MRSAPI.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DBHelper _dbHelper;
        private readonly MRSDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly IDGenerated _iDGenerated;
        private readonly IHttpContextAccessor _httpContextAccessor;
        long mxSl = 0;
        public DoctorRepository(DBHelper dbHelper, MRSDbContext db, IWebHostEnvironment environment, IDGenerated iDGenerated, IHttpContextAccessor httpContextAccessor)
        {
            _dbHelper = dbHelper;
            _db = db;
            _environment = environment;
            _iDGenerated = iDGenerated;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<DoctorInformationModel> GetDoctorList(string doctorName, string? registrationNo, string? mobileNo, string designation, string specialization)
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();
            string query = "SELECT DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,DOCTOR_NAME,DEGREE,NVL(DEGREE_CODE,0)DEGREE_CODE," +
                          "DESIGNATION,NVL(DESIGNATION_CODE,0)DESIGNATION_CODE,NVL(SPECIA_1ST_CODE,0)SPECIA_1ST_CODE,NVL(SPECIA_2ND_CODE,0)SPECIA_2ND_CODE,GENDER,RELIGION," +
                          "TO_CHAR(DATE_OF_BIRTH,'dd/MM/yyyy')DATE_OF_BIRTH, DOC_PERS_PHONE,DOCTOR_EMAIL," +
                          "NVL(PATIENT_PER_DAY,0)PATIENT_PER_DAY,NVL(AVG_PRESC_VALUE,0)AVG_PRESC_VALUE,NVL(PRESC_SHARE,0)PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4," +
                          "REMARKS,FSPECIALIZATION,SPECIALIZATION SSPECIALIZATION FROM " +
                          //"(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR, DOCTOR_SPECIALIZATION WHERE DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                          "(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR Left Join DOCTOR_SPECIALIZATION ON  DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                          "Left Join DOCTOR_SPECIALIZATION ON DOCTORS.SPECIA_2ND_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE Where 1=1 ";
            if (doctorName != "" && designation != "" && specialization != "")
            {
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DESIGNATION) LIKE '%" + designation.ToUpper() + "%' AND UPPER(FSPECIALIZATION) LIKE '%" + specialization.ToUpper() + "%'";
            }
            else if (doctorName != "")
            {
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%'";
            }
            else if (designation != "")
            {
                query += " AND UPPER(DESIGNATION) LIKE '%" + designation.ToUpper() + "%'";
            }
            else if (specialization != "")
            {
                query += " AND UPPER(FSPECIALIZATION) LIKE '%" + specialization.ToUpper() + "%'";
            }
            else if (specialization != "")
            {
                query += " AND UPPER(FSPECIALIZATION) LIKE '%" + specialization.ToUpper() + "%'";
            }
            else if (specialization != "")
            {
                query += " AND UPPER(FSPECIALIZATION) LIKE '%" + specialization.ToUpper() + "%'";
            }
            else if (registrationNo != "")
            {
                query += " AND UPPER(REGISTRATION_NO) LIKE '%" + registrationNo.ToUpper() + "%'";
            }
            else if (mobileNo != "")
            {
                query += " AND UPPER(DOC_PERS_PHONE) LIKE '%" + mobileNo.ToUpper() + "%'";
            }
            else
            {
                query = query;
            }
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        model.Honorium = reader["HONORARIUM"].ToString();
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        model.SpeciFirstCode = Convert.ToInt32(reader["SPECIA_1ST_CODE"]);
                        model.SpeciFirstName = reader["FSPECIALIZATION"].ToString();
                        //model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        model.Gender = reader["GENDER"].ToString();
                        model.Religion = reader["RELIGION"].ToString();
                        model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        model.Email = reader["DOCTOR_EMAIL"].ToString();
                        model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        model.Address3 = reader["ADDRESS3"].ToString();
                        model.Address4 = reader["ADDRESS4"].ToString();
                        model.Remarks = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        public List<DoctorModel> GetDoctorsByMarketCode(string marketCode)
        {
            List<DoctorModel> listData = new List<DoctorModel>();
            //string query = "Select DM.DOCTOR_ID, D.DOCTOR_NAME,dd.* From DOC_MKT_MAS dm left join DOC_MKT_DTL dd on DD.DOC_MKT_MAS_SLNO = DM.DOC_MKT_MAS_SLNO " +
            //                "left join DOCTOR d on D.DOCTOR_ID = DM.DOCTOR_ID Where 1=1";
            string query = "SELECT DM.DOCTOR_ID, D.DOCTOR_NAME, DD.PRAC_MKT_CODE, M.MARKET_NAME, DD.DOC_MKT_MAS_SLNO, DD.DOC_MKT_DTL_SLNO, D.DESIGNATION_CODE, D.DESIGNATION, " +
                " D.SPECIA_1ST_CODE Speciality_Code, DS.SPECIALIZATION Speciality_Name, DD.INSTI_CODE, I.INSTI_NAME, DD.UPAZILA_CODE, DD.SBU_UNIT,  DD.PERSONAL_PHONE," +
                " DD.MDP_LOC_CODE, DD.MDP_LOC_NAME,DD.EDP_LOC_CODE, DD.EDP_LOC_NAME,DD.CHAMB_PHONE, " +
                " (DD.CHAMB_ADDRESS1 || ' ' || DD.CHAMB_ADDRESS2 || ' ' || DD.CHAMB_ADDRESS3 || ' ' || DD.CHAMB_ADDRESS2) Address, " +
                " DD.UPAZILA_CODE,DU.UPAZILA_NAME, DU.DISTC_CODE,DU.DISTC_NAME,D.POTENTIAL_CATEGORY,D.PATIENT_PER_DAY,M.SBU_CODE,DD.SBU_UNIT, D.PRESC_SHARE, D.REMARKS" +
                "  FROM DOC_MKT_MAS dm LEFT JOIN DOC_MKT_DTL dd ON DD.DOC_MKT_MAS_SLNO = DM.DOC_MKT_MAS_SLNO " +
                "  LEFT JOIN DOCTOR d ON D.DOCTOR_ID = DM.DOCTOR_ID" +
                "  LEFT JOIN MARKET m ON M.MARKET_CODE = DD.PRAC_MKT_CODE" +
                "  LEFT JOIN INSTITUTION i ON I.INSTI_CODE = DD.INSTI_CODE " +
                "  LEFT JOIN DOCTOR_SPECIALIZATION ds  ON DS.SPECIALIZATION_CODE = D.SPECIA_1ST_CODE" +
                " LEFT JOIN DISTRICT_UPAZILA du ON DD.UPAZILA_CODE = DU.UPAZILA_CODE WHERE 1=1";

            if (marketCode != "")
            {
                //query += " AND UPPER(dd.PRAC_MKT_CODE) LIKE '%" + marketCode.ToUpper() + "%'";
                query += " AND UPPER(dd.PRAC_MKT_CODE) = '" + marketCode.ToUpper() + "'";
            }
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorModel model = new DoctorModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.DoctorMstSl = Convert.ToInt32(reader["DOC_MKT_MAS_SLNO"]);
                        model.DoctorDetailSl = Convert.ToInt64(reader["DOC_MKT_DTL_SLNO"].ToString());
                        if (reader["DESIGNATION_CODE"] != DBNull.Value)
                        {
                            model.DesignationCode = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        }
                        else
                        {
                            model.DesignationCode = 0;
                        }
                        model.DesignationName = reader["DESIGNATION"].ToString();
                        model.SpecialtyCode = reader["Speciality_Code"].ToString();
                        model.SpecialtyName = reader["Speciality_Name"].ToString();
                        model.InstitutionCode = reader["INSTI_CODE"].ToString();
                        model.InstitutionName = reader["INSTI_NAME"].ToString();
                        model.Address = reader["Address"].ToString();
                        model.MarketCode = reader["PRAC_MKT_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        model.MorningLocCode = reader["MDP_LOC_CODE"].ToString();
                        model.MorningLocName = reader["MDP_LOC_NAME"].ToString();
                        model.EveningLocCode = reader["EDP_LOC_CODE"].ToString();
                        model.EveningLocName = reader["EDP_LOC_NAME"].ToString();
                        model.DistrictCode = reader["DISTC_CODE"].ToString();
                        model.DistrictName = reader["DISTC_NAME"].ToString();
                        model.UpazilaCode = reader["UPAZILA_CODE"].ToString();
                        model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        model.PatientPerDay = reader["PATIENT_PER_DAY"].ToString();
                        model.SBUCode = reader["SBU_CODE"].ToString();
                        model.SBUUnit = reader["SBU_UNIT"].ToString();
                        model.PersonalPhoneNumber = reader["PERSONAL_PHONE"].ToString();
                        model.ChamberPhoneNumber = reader["CHAMB_PHONE"].ToString();
                        model.doctorAttachments = GetAttachmentByDoctor(model.DoctorId);
                        model.Remarks = reader["REMARKS"].ToString();

                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DoctorAttachmentModel> GetAttachmentByDoctor(int doctorId)
        {
            List<DoctorAttachmentModel> listData = new List<DoctorAttachmentModel>();
            string query = "Select ID,DOCTOR_ID,FILE_TYPE,FILE_PATH,FILE_NAME From DOCTOR_FILES WHERE DOCTOR_ID = " + doctorId + "";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorAttachmentModel model = new DoctorAttachmentModel();
                        model.Id = Convert.ToInt32(reader["ID"]);
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.FileType = reader["FILE_TYPE"].ToString();
                        model.FilePath = reader["FILE_PATH"].ToString();
                        model.FileName = reader["FILE_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DesignationModel> GetDesignationList()
        {
            List<DesignationModel> listData = new List<DesignationModel>();
            string query = "Select DESIGNATION_CODE,DESIGNATION From DOCTOR_DESIGNATION ";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DesignationModel model = new DesignationModel();
                        model.DESIGNATION_CODE = reader["DESIGNATION_CODE"].ToString();
                        model.DESIGNATION_NAME = reader["DESIGNATION"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<SpecializationModel> GetSpecializationList()
        {
            List<SpecializationModel> listData = new List<SpecializationModel>();
            string query = "Select SPECIALIZATION_CODE,SPECIALIZATION From DOCTOR_SPECIALIZATION ";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SpecializationModel model = new SpecializationModel();
                        model.SPECIALIZATION_CODE = reader["SPECIALIZATION_CODE"].ToString();
                        model.SPECIALIZATION_NAME = reader["SPECIALIZATION"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<LocationModel> GetLocation()
        {
            List<LocationModel> listData = new List<LocationModel>();
            string query = "Select ZONE_CODE,ZONE_NAME,DIVISION_CODE,DIVISION_NAME,REGION_CODE,REGION_NAME,TERRITORY_CODE,TERRITORY_NAME,MARKET_CODE,MARKET_NAME,SBU_CODE,SBU_UNIT From LOCATION_VUE ";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LocationModel model = new LocationModel();
                        model.ZONE_CODE = reader["ZONE_CODE"].ToString();
                        model.ZONE_NAME = reader["ZONE_NAME"].ToString();
                        model.DIVISION_CODE = reader["DIVISION_CODE"].ToString();
                        model.DIVISION_NAME = reader["DIVISION_NAME"].ToString();
                        model.REGION_CODE = reader["REGION_CODE"].ToString();
                        model.REGION_NAME = reader["REGION_NAME"].ToString();
                        model.TERRITORY_CODE = reader["TERRITORY_CODE"].ToString();
                        model.TERRITORY_NAME = reader["TERRITORY_NAME"].ToString();
                        model.MARKET_CODE = reader["MARKET_CODE"].ToString();
                        model.MARKET_NAME = reader["MARKET_NAME"].ToString();
                        model.SBU_CODE = reader["MARKET_CODE"].ToString();
                        model.SBU_UNIT = reader["SBU_UNIT"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<PotentialCategoryModel> GetPotentialCategoryList()
        {
            List<PotentialCategoryModel> listData = new List<PotentialCategoryModel>();
            string query = "Select distinct POTENTIAL_CATEGORY From DOCTOR Where POTENTIAL_CATEGORY is not null";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PotentialCategoryModel model = new PotentialCategoryModel();
                        model.PotentialCatName = reader["POTENTIAL_CATEGORY"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DistrictModel> GetDistrictList()
        {
            List<DistrictModel> listData = new List<DistrictModel>();
            string query = "Select DISTC_CODE,DISTC_NAME From DISTRICT";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DistrictModel model = new DistrictModel();
                        model.DistrictCode = reader["DISTC_CODE"].ToString();
                        model.DistrictName = reader["DISTC_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<MarketInfoModel> GetMarketListWithSBU(string? marketName)
        {
            List<MarketInfoModel> listData = new List<MarketInfoModel>();
            string query = "Select MARKET_CODE,MARKET_NAME,SBU_CODE,SBU_UNIT,(MARKET_CODE || '|' || SBU_CODE) Market_SBU_Code,(MARKET_NAME || '|' || SBU_UNIT) Market_SBU_Name  From MARKET Where 1=1";
            if (marketName != "" && marketName != null)
            {
                query += " AND UPPER(MARKET_NAME) LIKE '%" + marketName.ToUpper() + "%'";
            }

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MarketInfoModel model = new MarketInfoModel();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        model.SBUCode = reader["SBU_CODE"].ToString();
                        model.SBUUnit = reader["SBU_UNIT"].ToString();
                        model.MarketSBUCode = reader["Market_SBU_Code"].ToString();
                        model.MarketSBUName = reader["Market_SBU_Name"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<UpazilaModel> GetUpazilaList()
        {
            List<UpazilaModel> listData = new List<UpazilaModel>();
            string query = "Select DUM.DISTC_UPAZILA_MAS_SLNO,DUM.DISTC_CODE, D.DISTC_NAME, DUD.UPAZILA_CODE, " +
                " U.UPAZILA_NAME from DISTC_UPAZILA_MAS DUM, DISTC_UPAZILA_DTL DUD,UPAZILA U,DISTRICT D" +
                " Where DUM.DISTC_UPAZILA_MAS_SLNO = DUD.DISTC_UPAZILA_MAS_SLNO" +
                " AND DUM.DISTC_CODE = D.DISTC_CODE AND DUD.UPAZILA_CODE = U.UPAZILA_CODE";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpazilaModel model = new UpazilaModel();
                        model.DistrictCode = reader["DISTC_CODE"].ToString();
                        model.DistrictName = reader["DISTC_NAME"].ToString();
                        model.UpazilaCode = reader["UPAZILA_CODE"].ToString();
                        model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }


        public async Task<List<string>> SavePostImageAsync(FileUploadDTO fileUpload)
        {
            List<string> filePath = new List<string>();
            string uniqueFileName = string.Empty;
            if (fileUpload.Files != null)
            {
                foreach (var file in fileUpload.Files)
                {
                    //string uploadFolder = Path.Combine(_environment.WebRootPath, "Content/Laptop/");
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                    //var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string FilePath = Path.Combine(uploadFolder, uniqueFileName);
                    //string deleteFromFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                    using (var fileStream = new FileStream(FilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    filePath.Add(FilePath);
                }

            }

            return filePath.ToList();

        }

        public string GetFilePatheWithName(string filePath)
        {
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select FILE_PATH from DOCTOR_FILES Where FILE_PATH = '" + filePath + "'";
                //string filePath = string.Empty;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        filePath = reader["FILE_PATH"].ToString();
                    }
                    return filePath;
                }
            }
        }

        public async Task<bool> CreatePostAsync(FileUploadModel fileUpload)
        {
            bool isTrue = false;
            try
            {
                string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                if (fileUpload.FilePathList != null)
                {
                    //foreach (string FilePath in fileUpload.FilePathList)
                        foreach (var (FilePath, AttachmentType) in fileUpload.FilePathList.Zip(fileUpload.AttachmentTypes, (FilePath, AttachmentType) => (FilePath, AttachmentType)))
                        {
                            mxSl = _iDGenerated.getMAXSL("ID", "DOCTOR_FILES");
                            string saveQuery = "INSERT INTO DOCTOR_FILES (ID,DOCTOR_ID,FILE_TYPE,FILE_PATH,CREATION_DATE) VALUES(" + mxSl + "," + fileUpload.DoctorId + ",'" + AttachmentType + "','" + FilePath + "',(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')))";
                            
                            if (_dbHelper.CmdExecute(saveQuery) > 0)
                            {
                                fileUpload.Id = mxSl;
                                //InsertFilType(fileUpload);
                                isTrue = true;
                                
                            }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return isTrue;

        }

        private bool InsertFilType(FileUploadModel fileUpload)
        {
            bool isTrue = false;
            foreach (var item in fileUpload.AttachmentTypes)
            {
                string updateQuery = "Update DOCTOR_FILES Set FILE_TYPE = '"+ item + "' Where Id = "+fileUpload.Id+"";
                //string updateQuery = " INSERT INTO DOCTOR_FILES(FILE_TYPE) SELECT FILE_TYPE FROM DOCTOR_FILES WHERE Id = "+fileUpload.Id+" ";

                if (_dbHelper.CmdExecute(updateQuery) > 0)
                {
                    isTrue = true;
                }
            }
            return isTrue;
        }

        private long GetUploadIdByPath(List<string> filePathList)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveDoctorInfo(DoctorInformationAPIModel model)
        {
            bool isTrue = false;
            try
            {

                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                mxSl = _iDGenerated.getMAXSL("DOCTOR_ID", "DOCTOR Where DOCTOR_ID not in (900000)");

                string qry = "INSERT INTO DOCTOR (DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,DOCTOR_NAME,DEGREE,DEGREE_CODE,DESIGNATION_CODE,SPECIA_1ST_CODE,GENDER,RELIGION,DATE_OF_BIRTH,DOC_PERS_PHONE, " +
                                 "DOCTOR_EMAIL,PATIENT_PER_DAY,AVG_PRESC_VALUE,ADDRESS1,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                    "VALUES(" + mxSl + ", '" + model.RegistrationNo + "', '" + model.PotentialCategory + "', '" + model.DoctorName + "','" + model.DegreeTitle + "', " +
                    "'" + model.DegreeCode + "','" + model.DesignationCode + "','" + model.SpecializationCode + "'," +
                    "'" + model.Gender + "','" + model.Religion + "',(TO_DATE('" + model.DateOfBirth + "','dd-MM-yyyy')),'" + model.personalContactNumber + "','" + model.Email + "','" + model.PatientNoPerDay + "','" + model.ValuePerPrescription + "'," +
                    "'" + model.Address + "','" + model.Remarks + "','" + model.EmployeeId + "'," +
                    "(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')),'" + ip + "')";

                if (_dbHelper.CmdExecute(qry) > 0)
                {
                    isTrue = true;
                    model.DoctorId = mxSl;
                }

                if (model.DoctorMarketDetailsModels != null)
                {
                    long DoctorMstSl = _iDGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                    string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + DoctorMstSl + "," + mxSl + ")";
                   
                    if (_dbHelper.CmdExecute(query) > 0)
                    {
                        isTrue = true;
                    }

                    foreach (var detailModel in model.DoctorMarketDetailsModels)
                    {
                        long DoctorDetailSl = _iDGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                        string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT, " +
                                " UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,DISTC_CODE) Values(" + DoctorDetailSl + "," + DoctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBUUnit + "', " +
                                "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocName + "','" + detailModel.EveningLocName + "','" + detailModel.InstituteCode + "'," +
                                "(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')),'" + detailModel.DistrictCode + "')";
                        if (_dbHelper.CmdExecute(query1) > 0)
                        {
                            isTrue = true;
                        }
                    }
                }
                if (model.DoctorInSBUs != null)
                {
                    foreach (DoctorInSBU detail in model.DoctorInSBUs)
                    {
                        long DoctorSBUId = _iDGenerated.getMAXSL("DOCTOR_SBU_ID", "DOC_MARKET_SBU");
                        string query2 = "Insert into DOC_MARKET_SBU(DOCTOR_SBU_ID,DOCTOR_ID,MARKET_CODE,SBU_UNIT) Values(" + DoctorSBUId + "," + mxSl + ",'" + detail.MarketCode + "','" + detail.SBUUnit + "')";
                        if (_dbHelper.CmdExecute(query2) > 0)
                        {
                            isTrue = true;
                        }
                    }
                }

                //model.DoctorId = mxSl;
            }

            catch (Exception ex)
            {

                if (ex.Message.Contains("ORA-00001"))
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                if (ex.Message.Contains("ORA-12899"))
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

            }
            return isTrue;

        }

        public async Task<bool> LinkDoctorWithMarket(DeadDoctorRequestModel model)
        {
            bool isTrue = false;
            try
            {
                mxSl = _iDGenerated.getMAXSL("ID", "OPERATIONS_MASTER");
                string CreationDate = DateTime.Now.ToString("dd/MM/yyyy");
                string OperationType = "Dead Doctor";
                string qry = "INSERT INTO OPERATIONS_MASTER (ID,EMPLOYEE_ID,MARKET_CODE,OPERATION_TYPE,REMARK,CREATED_DATETIME,STATUS )" +
                    "VALUES(" + mxSl + ", '" + model.EmployeeId + "', '" + model.MarketCode + "', '" + OperationType + "','" + model.Remarkes + "', " +
                    "(TO_DATE('" + CreationDate + "','dd-MM-yyyy')),'" + model.Status.ToUpper() + "')";


                if (_dbHelper.CmdExecute(qry) > 0)
                {
                    isTrue = true;
                    model.Id = mxSl;
                }


                if (model.deadDoctorInfoModels != null)
                {

                    foreach (var detailModel in model.deadDoctorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_DOCTORS");
                        string query1 = "Insert Into OPERATION_DOCTORS(ID,OPERATION_MASTER_ID,DOCTOR_ID) Values(" + Sl + "," + mxSl + ",'" + detailModel.DoctorId + "')";

                        if (_dbHelper.CmdExecute(query1) > 0)
                        {
                            isTrue = true;
                        }

                    }
                }
                if (model.doctorSupervisorInfoModels != null)
                {

                    foreach (var detailModel in model.doctorSupervisorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_SUPERVISORS");
                        string query2 = "Insert Into OPERATION_SUPERVISORS(ID,OPERATION_MASTER_ID,EMPLOYEE_ID,TERITORY_CODE,MARKET_CODE,IS_SUPERVISOR,REMARKS,CREATED_DATETIME)" +
                            " Values(" + Sl + "," + mxSl + ",'" + detailModel.EmployeeId + "','" + detailModel.TerritoryCode + "','" + detailModel.MarketCode + "','" + detailModel.IsSupervisor + "','" + detailModel.Remarkes + "',(TO_DATE('" + CreationDate + "','dd-MM-yyyy')))";

                        if (_dbHelper.CmdExecute(query2) > 0)
                        {
                            isTrue = true;
                        }

                    }
                }
                //model.Id = mxSl;
            }
            catch (Exception)
            {

                throw;
            }
            return isTrue;
        }


        public async Task<bool> DoctorShiftMarket(DoctorShiftRequestModel model)
        {
            bool isTrue = false;
            try
            {
               
                string CreationDate = DateTime.Now.ToString("dd/MM/yyyy");
                string OperationType = "Shift Doctor";
                mxSl = _iDGenerated.getMAXSL("ID", "OPERATIONS_MASTER");

                string qry = "INSERT INTO OPERATIONS_MASTER (ID,EMPLOYEE_ID,MARKET_CODE,OPERATION_TYPE,FROM_MARKET,TO_MARKET,REMARK,CREATED_DATETIME,STATUS )" +
                    "VALUES(" + mxSl + ", '" + model.EmployeeId + "', '" + model.MarketCode + "', '" + OperationType + "','" + model.FromMarket + "','" + model.ToMarket + "','" + model.Remarkes + "', " +
                    "(TO_DATE('" + CreationDate + "','dd-MM-yyyy')),'" + model.Status.ToUpper() + "')";


                if (_dbHelper.CmdExecute(qry) > 0)
                {
                    isTrue = true;
                    model.Id = mxSl;
                }

                if (model.doctorInfoModels != null)
                {

                    foreach (var detailModel in model.doctorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_DOCTORS");
                        string query1 = "Insert Into OPERATION_DOCTORS(ID,OPERATION_MASTER_ID,DOCTOR_ID) Values(" + Sl + "," + mxSl + ",'" + detailModel.DoctorId + "')";

                        if (_dbHelper.CmdExecute(query1) > 0)
                        {
                            isTrue = true;
                        }
                    }
                }
                if (model.supervisorInfoModels != null)
                {

                    foreach (var detailModel in model.supervisorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_SUPERVISORS");
                        string query2 = "Insert Into OPERATION_SUPERVISORS(ID,OPERATION_MASTER_ID,EMPLOYEE_ID,TERITORY_CODE,MARKET_CODE,IS_SUPERVISOR,REMARKS,CREATED_DATETIME)" +
                            " Values(" + Sl + "," + mxSl + ",'" + detailModel.EmployeeId + "','" + detailModel.TerritoryCode + "','" + detailModel.MarketCode + "','" + detailModel.IsSupervisor + "','" + detailModel.Remarkes + "',(TO_DATE('" + CreationDate + "','dd-MM-yyyy')))";

                        if (_dbHelper.CmdExecute(query2) > 0)
                        {
                            isTrue = true;
                        }
                    }
                }
                //model.Id = mxSl;
            }
            catch (Exception)
            {

                throw;
            }
            return isTrue;
        }
        public async Task<bool> DoctorLinkWithMarket(DoctorLinkRequestModel model)
        {
            bool isTrue = false;
            try
            {

                string CreationDate = DateTime.Now.ToString("dd/MM/yyyy");
                string OperationType = "Link Doctor";
                mxSl = _iDGenerated.getMAXSL("ID", "OPERATIONS_MASTER");

                string qry = "INSERT INTO OPERATIONS_MASTER (ID,EMPLOYEE_ID,MARKET_CODE,OPERATION_TYPE,REMARK,CREATED_DATETIME,STATUS )" +
                    "VALUES(" + mxSl + ", '" + model.EmployeeId + "', '" + model.MarketCode + "', '" + OperationType + "','" + model.Remarkes + "', " +
                    "(TO_DATE('" + CreationDate + "','dd-MM-yyyy')),'" + model.Status.ToUpper() + "')";

                if (_dbHelper.CmdExecute(qry) > 0)
                {
                    isTrue = true;
                    model.Id = mxSl;
                }

                if (model.doctorInfoModels != null)
                {

                    foreach (var detailModel in model.doctorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_DOCTORS");
                        string query1 = "Insert Into OPERATION_DOCTORS(ID,OPERATION_MASTER_ID,DOCTOR_ID) Values(" + Sl + "," + mxSl + ",'" + detailModel.DoctorId + "')";

                        if (_dbHelper.CmdExecute(query1) > 0)
                        {
                            isTrue = true;
                        }
                    }
                }
                if (model.supervisorInfoModels != null)
                {

                    foreach (var detailModel in model.supervisorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_SUPERVISORS");
                        string query1 = "Insert Into OPERATION_SUPERVISORS(ID,OPERATION_MASTER_ID,EMPLOYEE_ID,TERITORY_CODE,MARKET_CODE,IS_SUPERVISOR,REMARKS,CREATED_DATETIME)" +
                            " Values(" + Sl + "," + mxSl + ",'" + detailModel.EmployeeId + "','" + detailModel.TerritoryCode + "','" + detailModel.MarketCode + "','" + detailModel.IsSupervisor + "','" + detailModel.Remarkes + "',(TO_DATE('" + CreationDate + "','dd-MM-yyyy')))";

                        if (_dbHelper.CmdExecute(query1) > 0)
                        {
                            isTrue = true;
                        }

                    }
                }
                //model.Id = mxSl;
            }
            catch (Exception)
            {

                throw;
            }
            return isTrue;
        }


        public bool MarketExist(int id)
        {

            string query = "SELECT COUNT(*) FROM OPERATIONS_MASTER WHERE ID = " + id + "";
            DataTable dt = _dbHelper.GetDataTable(query);
            int rowCount = dt.Rows.Count;
            return rowCount > 0;

        }

        public async Task<bool> UpdateTMInfo(TMRponsesOnRequest obj)
        {
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            bool isTrue = false;
            try
            {
                //mxSl = _iDGenerated.getMAXSL("INSTI_CODE", "INSTITUTION Where INSTI_CODE not in (99999)");
                string updateQuery = "Update OPERATIONS_MASTER Set APPROVED_BY = '" + obj.EmployeeId + "' Where ID = " + obj.Id + "";

                if (_dbHelper.CmdExecute(updateQuery) > 0)
                {
                    isTrue = true;
                    //model.InstituteCode = mxSl.ToString();
                }

                if (obj.TMResponses != null)
                {
                    foreach (var detailModel in obj.TMResponses)
                    {
                        string updateQuery1 = "Update OPERATION_SUPERVISORS Set APRROVAL_STATUS = '" + detailModel.ApprovalStatus + "', UPDATED_DATETIME = (TO_DATE('" + CurrentDate + "','dd-MM-yyyy')) Where ID = " + detailModel.Id + " AND OPERATION_MASTER_ID = " + obj.Id + "";
                        if (_dbHelper.CmdExecute(updateQuery1) > 0)
                        {
                            isTrue = true;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return isTrue;
        }



        //public async Task<FileUploadModel> UpdatePutAsync(int id,FileUploadModel existingItem)
        //{
        //    var put = new FileUploadModel
        //    {
        //        //DoctorId = fileUpload.DoctorId,
        //        FileName = existingItem.FileName,
        //        FileType = existingItem.FileType,
        //        FilePath = existingItem.FilePath,
        //    };

        //    string saveQuery = "UPDATE DOCTOR_FILES SET FILE_NAME='" + put.FileName + "', FILE_TYPE='" + put.FileType + "',FILE_PATH='" + put.FilePath + "' WHERE ID= " + id + ""; 
        //    _dbHelper.CmdExecute(saveQuery);
        //    return put;
        //}



        public DoctorShiftRequestModel GetMarketById(int id)
        {
            DoctorShiftRequestModel model = new DoctorShiftRequestModel();
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select * from OPERATIONS_MASTER Where ID = " + id + "";
                //long doctor_Id = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        model.Id = Convert.ToInt32(reader["ID"]);
                        model.EmployeeId = reader["EMPLOYEE_ID"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        //model.OperationType = reader["OPERATION_TYPE"].ToString();
                        model.FromMarket = reader["FROM_MARKET"].ToString();
                        model.ToMarket = reader["TO_MARKET"].ToString();
                        model.Status = reader["STATUS"].ToString();
                        model.Remarkes = reader["REMARK"].ToString();
                        //model.ApprovedBy = reader["APPROVED_BY"].ToString();
                        model.doctorInfoModels = GetDoctorListById(id);
                        model.supervisorInfoModels = GetSupervisorInfoById(id);
                    }

                }
            }
            return model;

        }

        private List<SupervisorInfoModel> GetSupervisorInfoById(int id)
        {
            List<SupervisorInfoModel> listData = new List<SupervisorInfoModel>();
            string query = "Select OPERATION_MASTER_ID,EMPLOYEE_ID,TERITORY_CODE,MARKET_CODE,IS_SUPERVISOR,REMARKS,APRROVAL_STATUS,CREATED_DATETIME,UPDATED_DATETIME From OPERATION_SUPERVISORS Where OPERATION_MASTER_ID = " + id + "";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SupervisorInfoModel model = new SupervisorInfoModel();
                        model.EmployeeId = reader["EMPLOYEE_ID"].ToString();
                        model.TerritoryCode = reader["TERITORY_CODE"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.IsSupervisor = reader["IS_SUPERVISOR"].ToString();
                        model.Remarkes = reader["REMARKS"].ToString();
                        //model.ApprovalStatus = reader["APRROVAL_STATUS"].ToString();
                        //model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        private List<DoctorInfoModel> GetDoctorListById(int id)
        {
            List<DoctorInfoModel> listData = new List<DoctorInfoModel>();
            string query = "Select OPERATION_MASTER_ID,DOCTOR_ID From OPERATION_DOCTORS Where OPERATION_MASTER_ID = " + id + "";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInfoModel model = new DoctorInfoModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        //model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }


        public List<MPORequestModel> GetMPORequestByTMId(string TmId)
        {
            List<MPORequestModel> listData = new List<MPORequestModel>();
            //string query = "Select EMPLOYEE_ID,MARKET_CODE,OPERATION_TYPE,FROM_MARKET,TO_MARKET,STATUS,REMARK from OPERATIONS_MASTER Where Status = 'PENDING'";
            string query = "Select OM.ID,OM.EMPLOYEE_ID,OM.MARKET_CODE,OM.OPERATION_TYPE,OD.DOCTOR_ID, OM.FROM_MARKET,OM.TO_MARKET,OM.STATUS,OM.REMARK from OPERATIONS_MASTER OM\r\nLeft join OPERATION_DOCTORS OD on OM.ID=OD.OPERATION_MASTER_ID \r\nLeft join OPERATION_SUPERVISORS OS on OM.ID = OS.OPERATION_MASTER_ID Where OS.EMPLOYEE_ID = '" + TmId + "' AND OM.Status = 'PENDING'";
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MPORequestModel model = new MPORequestModel();
                        model.Id = Convert.ToInt32(reader["ID"]);
                        model.EmployeeId = reader["EMPLOYEE_ID"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.OperationType = reader["OPERATION_TYPE"].ToString();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.FromMarket = reader["FROM_MARKET"].ToString();
                        model.ToMarket = reader["TO_MARKET"].ToString();
                        model.Status = reader["STATUS"].ToString();
                        model.Remarkes = reader["REMARK"].ToString();

                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public long GetDoctorById(int doctorId)
        {
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select * from doctor Where DOCTOR_ID = " + doctorId + "";
                long doctor_Id = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctor_Id = Convert.ToInt64(reader["DOCTOR_ID"]);
                    }
                    return doctor_Id;
                }
            }

        }

        public int GetFileAttachmentId(int doctorId, string attachmentType)
        {
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select * from DOCTOR_FILES Where 1 = 1";

                if (attachmentType != null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + " And FILE_TYPE = '" + attachmentType + "'";
                }
                else if (attachmentType == null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + "";
                }
                else if (attachmentType != null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + " And FILE_TYPE = '" + attachmentType + "'";
                }
                else if (attachmentType == null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + "";
                }
                int Id = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Id = Convert.ToInt32(reader["ID"]);
                    }
                    return Id;
                }
            }
        }

        public bool DeleteMarketWithDocotor(DoctorShiftRequestModel obj)
        {
            bool isTrue = false;
            if (obj.supervisorInfoModels != null)
            {
                foreach (var detailModel in obj.supervisorInfoModels)
                {
                    string query1 = "Delete from OPERATION_SUPERVISORS Where OPERATION_MASTER_ID = " + obj.Id + "";
                    if (_dbHelper.CmdExecute(query1) > 0)
                    {
                        isTrue = true;
                    }
                }
            }
            if (obj.doctorInfoModels != null)
            {
                foreach (var detailModel in obj.doctorInfoModels)
                {
                    string query2 = "Delete from OPERATION_DOCTORS Where OPERATION_MASTER_ID = " + obj.Id + "";
                    if (_dbHelper.CmdExecute(query2) > 0)
                    {
                        isTrue = true;
                    }

                }
            }
            string query3 = "Delete from OPERATIONS_MASTER Where ID = " + obj.Id + "";
            if (_dbHelper.CmdExecute(query3) > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }
    }
}
