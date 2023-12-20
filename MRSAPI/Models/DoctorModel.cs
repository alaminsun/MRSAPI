namespace MRSAPI.Models
{
    public class DoctorModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public long DoctorDetailSl { get; set; }
        public long DoctorMstSl { get; set; }
        public int DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string SpecialtyCode { get; set; }
        public string SpecialtyName { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string Address { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string PhoneNumber { get; set; }
        public string MorningLocCode { get; set; }
        public string MorningLocName { get; set; }
        public string EveningLocCode { get; set; }
        public string EveningLocName { get; set; }
        public string? DistrictName { get; set; }
        public string? DistrictCode { get; set; }
        public string? UpazilaCode { get; set; }
        public string? UpazilaName { get; set; }
        public string? PotentialCategory { get; set; }
        public string? PatientPerDay { get; set; }
        public string? SBUCode { get; set; }
        public string? SBUUnit { get; set; }
        public string? PersonalPhoneNumber { get; set; }
        public string? ChamberPhoneNumber { get; set; }
        public List<DoctorAttachmentModel> doctorAttachments { get; set; }
        public string? Remarks { get; set; }
    }
}
