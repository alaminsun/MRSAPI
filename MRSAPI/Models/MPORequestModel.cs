namespace MRSAPI.Models
{
    public class MPORequestModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string MarketCode { get; set; }
        public string OperationType { get; set; }
        public string FromMarket { get; set; }
        public string ToMarket { get; set; }
        public string Status { get; set; }
        public string Remarkes { get; set; }
        public int DoctorId { get; set; }
        //public List<LinkDoctoroModel> LinkDoctoros { get; set; }

    }
    //public class LinkDoctoroModel
    //{
    //    //public int DoctorMasterId { get; set; }
    //    public int DoctorId { get; set; }

    //}
}
