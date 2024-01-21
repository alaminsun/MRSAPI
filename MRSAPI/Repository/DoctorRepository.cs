using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Helpers;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using Oracle.ManagedDataAccess.Client;

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
                        model.DESIGNATION_CODE = Convert.ToInt32(reader["DESIGNATION_CODE"]);
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
                        model.SPECIALIZATION_CODE = Convert.ToInt32(reader["SPECIALIZATION_CODE"]);
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
            string query = "Select UPAZILA_CODE,UPAZILA_NAME From UPAZILA";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpazilaModel model = new UpazilaModel();
                        model.UpazilaCode = reader["UPAZILA_CODE"].ToString();
                        model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }


        public async Task<string> SavePostImageAsync(FileUploadModel fileUpload, string FilePath)
        {
            string uniqueFileName = string.Empty;
            if (fileUpload.File != null)
            {
                //string uploadFolder = Path.Combine(_environment.WebRootPath, "Content/Laptop/");
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                //var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + fileUpload.File.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                //string deleteFromFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileUpload.File.CopyToAsync(fileStream);
                }
                FilePath = filePath;
            }
            return FilePath;


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

        public async Task<FileUploadModel> CreatePostAsync(FileUploadModel fileUpload, string FilePath)
        {
            var post = new FileUploadModel
            {
                DoctorId = fileUpload.DoctorId,
                AttachmentType = fileUpload.AttachmentType
            };

            string saveQuery = "INSERT INTO DOCTOR_FILES (ID,DOCTOR_ID,FILE_TYPE,FILE_PATH) VALUES(incremet_id.NEXTVAL," + post.DoctorId + ",'" + post.AttachmentType + "','" + FilePath + "')";
            _dbHelper.CmdExecute(saveQuery);

            return post;

        }

        public async Task<DoctorInformationAPIModel> SaveDoctorInfo(DoctorInformationAPIModel model)
        {
            try
            {
                //if (IsDoctorIDExitsByDoctorID(model.DoctorId))
                //{
                //    _vmMsg.Type = Enums.MessageType.Error;
                //    _vmMsg.Msg = "Data Already Exist.";
                //}
                //else
                //{
                //string employeeId = "1000";
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                ////if (model.DoctorInfoModel != null)
                ////    {
                //        //foreach (DoctorInformationAPIModel docModel in model.DoctorMasterModels)
                //        //{
                //            mxSl = _iDGenerated.getMAXSL("DOCTOR_ID", "DOCTOR Where DOCTOR_ID not in (900000)");
                //        //string setOndate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");


                //        string qry1 = "INSERT INTO DOCTOR (DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,DOCTOR_NAME,DEGREE,DEGREE_CODE,DESIGNATION_CODE,SPECIA_1ST_CODE,GENDER,RELIGION,DATE_OF_BIRTH,DOC_PERS_PHONE, " +
                //                         "DOCTOR_EMAIL,PATIENT_PER_DAY,AVG_PRESC_VALUE,ADDRESS1,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //            "VALUES(" + mxSl + ", '" + model.DoctorMasterModels.RegistrationNo + "', '" + model.DoctorMasterModels.PotentialCategory + "', '" + model.DoctorMasterModels.DoctorName + "','" + model.DoctorMasterModels.DegreeTitle + "', " +
                //            "'" + model.DoctorMasterModels.DegreeCode + "','" + model.DoctorMasterModels.DesignationCode + "','" + model.DoctorMasterModels.SpecializationCode + "'," +
                //            "'" + model.DoctorMasterModels.Gender + "','" + model.DoctorMasterModels.Religion + "',(TO_DATE('" + model.DoctorMasterModels.DateOfBirth + "','dd-MM-yyyy')),'" + model.DoctorMasterModels.personalContactNumber + "','" + model.DoctorMasterModels.Email + "','" + model.DoctorMasterModels.PatientNoPerDay + "','" + model.DoctorMasterModels.ValuePerPrescription + "'," +
                //            "'" + model.DoctorMasterModels.Address + "','" + model.DoctorMasterModels.Remarks + "','" + model.EmployeeId + "'," +
                //            "(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')),'" + ip + "')";

                //            _dbHelper.CmdExecute(qry1);

                //            if (model.DoctorMasterModels.DoctorMarketDetailsModels != null)
                //            {
                //                long DoctorMstSl = _iDGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                //                string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + DoctorMstSl + "," + mxSl + ")";
                //                _dbHelper.CmdExecute(query);

                //                foreach (var detailModel in model.DoctorMasterModels.DoctorMarketDetailsModels)
                //                {
                //                    long DoctorDetailSl = _iDGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                //                    string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT, " +
                //                            " UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,DISTC_CODE) Values(" + DoctorDetailSl + "," + DoctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBUUnit + "', " +
                //                            "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocName + "','" + detailModel.EveningLocName + "'," + detailModel.InstituteCode + ",(TO_DATE('" + CurrentDate + "','dd/MM/yyyy'))," +
                //                            "'" + detailModel.DistrictCode + "')";
                //                    _dbHelper.CmdExecute(query1);
                //                }
                //            }
                //}

                //}
                mxSl = _iDGenerated.getMAXSL("DOCTOR_ID", "DOCTOR Where DOCTOR_ID not in (900000)");

                string qry = "INSERT INTO DOCTOR (DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,DOCTOR_NAME,DEGREE,DEGREE_CODE,DESIGNATION_CODE,SPECIA_1ST_CODE,GENDER,RELIGION,DATE_OF_BIRTH,DOC_PERS_PHONE, " +
                                 "DOCTOR_EMAIL,PATIENT_PER_DAY,AVG_PRESC_VALUE,ADDRESS1,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                    "VALUES(" + mxSl + ", '" + model.DoctorMasterModels.RegistrationNo + "', '" + model.DoctorMasterModels.PotentialCategory + "', '" + model.DoctorMasterModels.DoctorName + "','" + model.DoctorMasterModels.DegreeTitle + "', " +
                    "'" + model.DoctorMasterModels.DegreeCode + "','" + model.DoctorMasterModels.DesignationCode + "','" + model.DoctorMasterModels.SpecializationCode + "'," +
                    "'" + model.DoctorMasterModels.Gender + "','" + model.DoctorMasterModels.Religion + "',(TO_DATE('" + model.DoctorMasterModels.DateOfBirth + "','dd-MM-yyyy')),'" + model.DoctorMasterModels.personalContactNumber + "','" + model.DoctorMasterModels.Email + "','" + model.DoctorMasterModels.PatientNoPerDay + "','" + model.DoctorMasterModels.ValuePerPrescription + "'," +
                    "'" + model.DoctorMasterModels.Address + "','" + model.DoctorMasterModels.Remarks + "','" + model.EmployeeId + "'," +
                    "(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')),'" + ip + "')";

                _dbHelper.CmdExecute(qry);

                if (model.DoctorMasterModels.DoctorMarketDetailsModels != null)
                {
                    long DoctorMstSl = _iDGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                    string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + DoctorMstSl + "," + mxSl + ")";
                    _dbHelper.CmdExecute(query);

                    foreach (var detailModel in model.DoctorMasterModels.DoctorMarketDetailsModels)
                    {
                        long DoctorDetailSl = _iDGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                        //string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                        //        "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,MDP_LOC_NAME,EDP_LOC_NAME) Values(" + detailModel.DoctorDetailSl + "," + model.DoctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBU_GROUP + "', " +
                        //        "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                        //        "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + detailModel.MorningLocTextName + "','" + detailModel.EveningTextLocName + "')";
                        string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT, " +
                                " UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,DISTC_CODE) Values(" + DoctorDetailSl + "," + DoctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBUUnit + "', " +
                                "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocName + "','" + detailModel.EveningLocName + "','" + detailModel.InstituteCode + "'," +
                                "(TO_DATE('" + CurrentDate + "','dd/MM/yyyy')),'" + detailModel.DistrictCode + "')";
                        _dbHelper.CmdExecute(query1);
                    }
                }
                if (model.DoctorMasterModels.DoctorInSBUs != null)
                {
                    foreach (DoctorInSBU detail in model.DoctorMasterModels.DoctorInSBUs)
                    {
                        long DoctorSBUId = _iDGenerated.getMAXSL("DOCTOR_SBU_ID", "DOC_MARKET_SBU");
                        string query = "Insert into DOC_MARKET_SBU(DOCTOR_SBU_ID,DOCTOR_ID,MARKET_CODE,SBU_UNIT) Values(" + DoctorSBUId + "," + mxSl + ",'" + detail.MarketCode + "','" + detail.SBUUnit + "')";
                        _dbHelper.CmdExecute(query);
                    }
                }

                model.DoctorMasterModels.DoctorId = mxSl;
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
            return model;

        }

        public async Task<DeadDoctorModel> LinkDoctorWithMarket(DeadDoctorModel model)
        {
            try
            {
                mxSl = _iDGenerated.getMAXSL("ID", "OPERATIONS_MASTER");

                string qry = "INSERT INTO OPERATIONS_MASTER (ID,EMPLOYEE_ID,MARKET_CODE,OPERATION_TYPE,REMARK,CREATED_DATETIME,STATUS,APPROVED_BY )" +
                    "VALUES(" + mxSl + ", '" + model.EmployeeId + "', '" + model.MarketCode + "', '" + model.OperationType + "','" + model.Remarkes + "', " +
                    "(TO_DATE('" + model.CreationDate + "','dd-MM-yyyy')),'" + model.Status + "','" + model.ApprovedBy + "')";


                _dbHelper.CmdExecute(qry);

                if (model.deadDoctorInfoModels != null)
                {

                    foreach (var detailModel in model.deadDoctorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_DOCTORS");
                        string query1 = "Insert Into OPERATION_DOCTORS(ID,OPERATION_MASTER_ID,DOCTOR_ID) Values(" + Sl + "," + mxSl + ",'" + detailModel.DoctorId + "')";

                        _dbHelper.CmdExecute(query1);
                    }
                }
                if (model.doctorSupervisorInfoModels != null)
                {

                    foreach (var detailModel in model.doctorSupervisorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_SUPERVISORS");
                        string query1 = "Insert Into OPERATION_SUPERVISORS(ID,OPERATION_MASTER_ID,EMPLOYEE_ID,TERITORY_CODE,MARKET_CODE,IS_SUPERVISOR,REMARKS,APRROVAL_STATUS,CREATED_DATETIME)" +
                            " Values(" + Sl + "," + mxSl + ",'" + detailModel.EmployeeId + "','" + detailModel.TerritoryCode +"','" + detailModel.MarketCode + "','" + detailModel.IsSupervisor + "','" + detailModel.Remarkes + "','" + detailModel.ApprovalStatus + "',(TO_DATE('" + model.CreationDate + "','dd-MM-yyyy')))";

                        _dbHelper.CmdExecute(query1);
                    }
                }
                model.Id = mxSl;
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }


        public async Task<DoctorShiftModel> DoctorShiftMarket(DoctorShiftModel model)
        {
            try
            {
                mxSl = _iDGenerated.getMAXSL("ID", "OPERATIONS_MASTER");

                string qry = "INSERT INTO OPERATIONS_MASTER (ID,EMPLOYEE_ID,MARKET_CODE,OPERATION_TYPE,FROM_MARKET,TO_MARKET,REMARK,CREATED_DATETIME,STATUS,APPROVED_BY )" +
                    "VALUES(" + mxSl + ", '" + model.EmployeeId + "', '" + model.MarketCode + "', '" + model.OperationType + "','" + model.FromMarket + "','" + model.ToMarket + "','" + model.Remarkes + "', " +
                    "(TO_DATE('" + model.CreationDate + "','dd-MM-yyyy')),'" + model.Status + "','" + model.ApprovedBy + "')";


                _dbHelper.CmdExecute(qry);

                if (model.deadDoctorInfoModels != null)
                {

                    foreach (var detailModel in model.deadDoctorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_DOCTORS");
                        string query1 = "Insert Into OPERATION_DOCTORS(ID,OPERATION_MASTER_ID,DOCTOR_ID) Values(" + Sl + "," + mxSl + ",'" + detailModel.DoctorId + "')";

                        _dbHelper.CmdExecute(query1);
                    }
                }
                if (model.doctorSupervisorInfoModels != null)
                {

                    foreach (var detailModel in model.doctorSupervisorInfoModels)
                    {
                        long Sl = _iDGenerated.getMAXSL("ID", "OPERATION_SUPERVISORS");
                        string query1 = "Insert Into OPERATION_SUPERVISORS(ID,OPERATION_MASTER_ID,EMPLOYEE_ID,TERITORY_CODE,MARKET_CODE,IS_SUPERVISOR,REMARKS,APRROVAL_STATUS,CREATED_DATETIME)" +
                            " Values(" + Sl + "," + mxSl + ",'" + detailModel.EmployeeId + "','" + detailModel.TerritoryCode + "','" + detailModel.MarketCode + "','" + detailModel.IsSupervisor + "','" + detailModel.Remarkes + "','" + detailModel.ApprovalStatus + "',(TO_DATE('" + model.CreationDate + "','dd-MM-yyyy')))";

                        _dbHelper.CmdExecute(query1);
                    }
                }
                model.Id = mxSl;
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }


        public bool MarketExist(int id)
        {

            string query = "SELECT COUNT(*) FROM Doctor WHERE DOCTOR_ID = @DoctorID";

            // Assume dbHelper is an instance of your database helper class with a method ExecuteScalar
            int rowCount = _dbHelper.ExecuteScalar<int>(query, new { DoctorID = id });

            return rowCount > 0;
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



        //public FileUploadModel GetDoctorwithFileById(int id)
        //{
        //    FileUploadModel model = new FileUploadModel();
        //    using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
        //    {
        //        string query = "Select * from DOCTOR_FILES Where ID = " + id + "";
        //        //long doctor_Id = 0;
        //        OracleCommand cmd = new OracleCommand(query, con);
        //        con.Open();
        //        using (OracleDataReader reader = cmd.ExecuteReader())
        //        {

        //            while (reader.Read())
        //            {

        //                model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
        //                model.FileName = reader["FILE_NAME"].ToString();
        //                //model.FileType = (FileType)Convert.ToInt32(reader["FILE_TYPE"]);
        //                model.FilePath = reader["FILE_PATH"].ToString();
        //            }

        //        }
        //    }
        //    return model;

        //}

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
                        doctor_Id = Convert.ToInt32(reader["DOCTOR_ID"]);
                    }
                    return doctor_Id;
                }
            }

        }

        public int GetFileAttachmentId(int doctorId, string attachmentType, string filePath)
        {
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select * from DOCTOR_FILES Where 1 = 1";

                if (attachmentType != null && filePath == null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + " And FILE_TYPE = '" + attachmentType + "'";
                }
                else if (attachmentType == null && filePath != null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + " And FILE_PATH = '" + filePath + "'";
                }
                else if (attachmentType != null && filePath != null)
                {
                    query += " AND DOCTOR_ID = " + doctorId + " And FILE_PATH = '" + filePath + "' And FILE_TYPE = '" + attachmentType + "'";
                }
                else if (attachmentType == null && filePath == null)
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

        public DoctorShiftModel GetMarketById(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMarketWithDocotor(DoctorShiftModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
