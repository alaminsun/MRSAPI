namespace MRSAPI.Models.DTO
{
    public class DoctorMarketDetailsDTO
    {
        public long DoctorDetailSl { get; set; }
        public long DoctorMstSl { get; set; }
        public string MarketCode { get; set; }
        public long DoctorId { get; set; }
        public string? InstituteCode { get; set; }
        public string? InstituteName { get; set; }
        public string? UpazilaCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? MorningLocName { get; set; }
        public string? EveningLocName { get; set; }
        public string SBUUnit { get; set; }
    }
}
