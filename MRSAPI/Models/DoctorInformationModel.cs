namespace MRSAPI.Models
{
    public class DoctorInformationModel
    {
        public int DoctorId { get; set; }
        public string RegistrationNo { get; set; }
        public string PotentialCategory { get; set; }
        public string Honorium { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public int DegreeCategory { get; set; }
        public string DegreeName { get; set; }
        public string Designation { get; set; }
        public int DesignationCategory { get; set; }
        public string DesignationName { get; set; }
        public int SpeciFirstCode { get; set; }
        public string SpeciFirstName { get; set; }
        public string SpeciSecondName { get; set; }
        //public int SpeciSecondCode { get; set; }
        public string SpeciSecondCode { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int PatientNo { get; set; }
        public int PrescriptionValue { get; set; }
        public int PrescriptionShare { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        //public List<DoctorMarketDetailsModel> DoctorMarketDetailsModels { get; set; }
        //public List<DoctorRelativeInfo> Relatives { get; set; }
        public List<DoctorInformationModel> DoctorInfoModel { get; set; }
        ///////////doctor practice market ///////////////
        public long DoctorMstSl { get; set; }

        public string MarketCode { get; set; }

        public string MarketName { get; set; }
    }

  
}
