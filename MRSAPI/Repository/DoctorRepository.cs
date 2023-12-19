
using Microsoft.Extensions.Hosting;
using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Helpers;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace MRSAPI.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DBHelper _dbHelper;
        private readonly MRSDbContext _db;
        private readonly IWebHostEnvironment environment;
        public DoctorRepository(DBHelper dbHelper, MRSDbContext db, IWebHostEnvironment environment)
        {
            _dbHelper = dbHelper;
            _db = db;
            this.environment = environment;
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
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DESIGNATION) LIKE '%" + designation.ToUpper() + "%' AND UPPER(FSPECIALIZATION) LIKE '%" + specialization.ToUpper() +"%'";
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
                " (DD.CHAMB_ADDRESS1 || ' ' || DD.CHAMB_ADDRESS2 || ' ' || DD.CHAMB_ADDRESS3 || ' ' || DD.CHAMB_ADDRESS2) Address "+
                "  FROM DOC_MKT_MAS dm LEFT JOIN DOC_MKT_DTL dd ON DD.DOC_MKT_MAS_SLNO = DM.DOC_MKT_MAS_SLNO " +
                "  LEFT JOIN DOCTOR d ON D.DOCTOR_ID = DM.DOCTOR_ID" +
                "  LEFT JOIN MARKET m ON M.MARKET_CODE = DD.PRAC_MKT_CODE" +
                "  LEFT JOIN INSTITUTION i ON I.INSTI_CODE = DD.INSTI_CODE " +
                "  LEFT JOIN DOCTOR_SPECIALIZATION ds  ON DS.SPECIALIZATION_CODE = D.SPECIA_1ST_CODE WHERE 1=1";



 //           SELECT DM.DOCTOR_ID,
 //      D.DOCTOR_NAME,
 //      DD.PRAC_MKT_CODE,
 //      M.MARKET_NAME,
 //      DD.DOC_MKT_MAS_SLNO,
 //      DD.DOC_MKT_DTL_SLNO,
 //      D.DESIGNATION_CODE,
 //      D.DESIGNATION,
 //      D.SPECIA_1ST_CODE Speciality_Code,
 //      DS.SPECIALIZATION Speciality_Name,
 //      DD.INSTI_CODE,
 //      I.INSTI_NAME,
 //      DD.PERSONAL_PHONE,
 //      DD.MDP_LOC_CODE,
 //      DD.MDP_LOC_NAME,
 //      DD.EDP_LOC_CODE,
 //      DD.EDP_LOC_NAME,
 //      DD.CHAMB_PHONE,
 //      (DD.CHAMB_ADDRESS1
 //       || ' '
 //       || DD.CHAMB_ADDRESS2
 //       || ' '
 //       || DD.CHAMB_ADDRESS3
 //       || ' '
 //       || DD.CHAMB_ADDRESS2) Address,
 //      DD.UPAZILA_CODE,
 //      DU.UPAZILA_NAME,
 //      DU.DISTC_CODE,
 //      DU.DISTC_NAME,
 //      D.POTENTIAL_CATEGORY,
 //      D.PATIENT_PER_DAY,
 //      DD.SBU_UNIT,
 //      D.PRESC_SHARE,
 //      D.REMARKS

 // FROM DOC_MKT_MAS dm
 //      LEFT JOIN DOC_MKT_DTL dd ON DD.DOC_MKT_MAS_SLNO = DM.DOC_MKT_MAS_SLNO
 //      LEFT JOIN DOCTOR d ON D.DOCTOR_ID = DM.DOCTOR_ID
 //      LEFT JOIN MARKET m ON M.MARKET_CODE = DD.PRAC_MKT_CODE
 //      LEFT JOIN INSTITUTION i ON I.INSTI_CODE = DD.INSTI_CODE
 //      LEFT JOIN DOCTOR_SPECIALIZATION ds ON DS.SPECIALIZATION_CODE = D.SPECIA_1ST_CODE
 //      LEFT JOIN DISTRICT_UPAZILA du ON DD.UPAZILA_CODE = DU.UPAZILA_CODE


 //WHERE 1 = 1

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
                        model.PhoneNumber = reader["CHAMB_PHONE"].ToString();
                        model.MorningLocCode = reader["MDP_LOC_CODE"].ToString();
                        model.MorningLocName = reader["MDP_LOC_NAME"].ToString();
                        model.EveningLocCode = reader["EDP_LOC_CODE"].ToString();
                        model.EveningLocName = reader["EDP_LOC_NAME"].ToString();

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

        public List<DoctorInformationModel> GetPotentialCategoryList()
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();
            string query = "Select distinct POTENTIAL_CATEGORY From DOCTOR Where POTENTIAL_CATEGORY is not null";

            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }





        public async Task<string> SavePostImageAsync(FileUploadModel fileUpload)
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
                fileUpload.FilePath = filePath;
            }
            return uniqueFileName;

            //string FileName = string.Empty;
            //if (fileUpload.File != null)
            //{
            //    //string uploadFolder = Path.Combine(_environment.WebRootPath, "Content/Laptop/");
            //    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
            //    //var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
            //    if (!Directory.Exists(uploadFolder))
            //    {
            //        Directory.CreateDirectory(uploadFolder);
            //    }
            //    //FileName = Guid.NewGuid().ToString() + "_" + fileUpload.File.FileName;
            //    FileName = fileUpload.File.FileName;
            //    string filePath = Path.Combine(uploadFolder, FileName);
            //    var dbFilePathe = GetFilePatheWithName(filePath);
            //    //string dbFilePathe = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, data.FilePath);
            //    if (dbFilePathe == filePath)
            //    {
            //        if (System.IO.File.Exists(dbFilePathe))
            //        {
            //            System.IO.File.Delete(dbFilePathe);
            //        }
            //    }
            //    //string deleteFromFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await fileUpload.File.CopyToAsync(fileStream);
            //    }
            //    fileUpload.FilePath = filePath;
            //}
            //return FileName;

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

        public async Task<FileUploadModel> CreatePostAsync(FileUploadModel fileUpload)
        {
            var post = new FileUploadModel
            {
                DoctorId = fileUpload.DoctorId,
                FileName = fileUpload.FileName,
                FileType = fileUpload.FileType,
                FilePath = fileUpload.FilePath
            };

            string saveQuery = "INSERT INTO DOCTOR_FILES (ID,DOCTOR_ID,FILE_TYPE,FILE_PATH,FILE_NAME) VALUES(incremet_id.NEXTVAL," + post.DoctorId + ",'" + post.FileType + "','" + post.FilePath + "','" + post.FileName + "')";
            _dbHelper.CmdExecute(saveQuery);

            return post;

        }


        public async Task<FileUploadModel> UpdatePutAsync(int id,FileUploadModel existingItem)
        {
            var put = new FileUploadModel
            {
                //DoctorId = fileUpload.DoctorId,
                FileName = existingItem.FileName,
                FileType = existingItem.FileType,
                FilePath = existingItem.FilePath,
            };

            string saveQuery = "UPDATE DOCTOR_FILES SET FILE_NAME='" + put.FileName + "', FILE_TYPE='" + put.FileType + "',FILE_PATH='" + put.FilePath + "' WHERE ID= " + id + ""; 
            _dbHelper.CmdExecute(saveQuery);


            return put;
        }



        public FileUploadModel GetDoctorwithFileById(int id)
        {
            FileUploadModel model = new FileUploadModel();
            using (OracleConnection con = new OracleConnection(_db.GetConnectionString()))
            {
                string query = "Select * from DOCTOR_FILES Where ID = " + id + "";
                //long doctor_Id = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.FileName = reader["FILE_NAME"].ToString();
                        //model.FileType = (FileType)Convert.ToInt32(reader["FILE_TYPE"]);
                        model.FilePath = reader["FILE_PATH"].ToString();
                    }
                   
                }
            }
            return model;

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
                        doctor_Id = Convert.ToInt32(reader["DOCTOR_ID"]);
                        //model.FileName = reader["FILE_NAME"].ToString();
                        ////model.FileType = (FileType)Convert.ToInt32(reader["FILE_TYPE"]);
                        //model.FilePath = reader["FILE_PATH"].ToString();
                    }
                    return doctor_Id;
                }
            }

        }

    }
}
