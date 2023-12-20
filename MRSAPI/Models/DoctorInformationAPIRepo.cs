namespace MRSAPI.Models
{
    public class DoctorInformationAPIRepoModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string RegistrationNo { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string DateOfBirth { get; set; }
        public string personalContactNumber { get; set; }
        public string chamberContactNumber { get; set; }
        public string Email { get; set; }
        public string SpecializationCode { get; set; }
        public string PotentialCategory { get; set; }
        public int PatientNoPerDay { get; set; }
        public int ValuePerPrescription { get; set; }
        public string Address { get; set; }
        public string DesignationCode { get; set; }
        public string DegreeTitle { get; set; }
        public string DegreeCode { get; set; }
        public string Remarks { get; set; }
        public List<DoctorMarketDetailsRepoModel> DoctorMarketDetailsModels { get; set; }
       //public List<DoctorRelativeInfo> Relatives { get; set; }
        public List<DoctorInSBURepo> DoctorInSBUs { get; set; }

        public List<DoctorInformationAPIModel> DoctorInfoModel { get; set; }
        public long DoctorMstSl { get; set; }

        public string MarketCode { get; set; }

        public string MarketName { get; set; }

        public string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
    }

    public class DoctorMarketDetailsRepoModel
    {
        public long DoctorDetailSl { get; set; }
        public long DoctorMstSl { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int InstituteCode { get; set; }
        public string InstituteName { get; set; }
        public string UpazilaCode { get; set; }
        public string DistrictCode { get; set; }
        public string MorningLocName { get; set; }
        public string EveningLocName { get; set; }
        public string SBU_UNIT { get; set; }
    }

    //public class DoctorRelativeInfo
    //{
    //    public int RelativeId { get; set; }
    //    public int DocId { get; set; }
    //    public string Relation { get; set; }
    //    public string DoctorName { get; set; }
    //}

    public class DoctorInSBURepo
    {
        public int DoctorSBUId { get; set; }
        public int DoctorId { get; set; }
        public string MarketCode { get; set; }
        public string SBUCode { get; set; }
    }

}
