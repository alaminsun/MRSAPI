using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class DoctorShiftRequestModel
    {
        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string MarketCode { get; set; }
        //public string OperationType { get; set; }
        public string FromMarket { get; set; }
        public string ToMarket { get; set; }
        public string Remarkes { get; set; }
        //public string CreationDate { get; set; }
        //public string? UpdatedDate { get; set; }
        public string Status { get; set; }
        //public string ApprovedBy { get; set; }
        public List<DoctorInfoModel> doctorInfoModels { get; set; }
        public List<SupervisorInfoModel> supervisorInfoModels { get; set; }
    }
    public class DoctorDeleteRequestModel
    {
        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string MarketCode { get; set; }
        //public string OperationType { get; set; }
        //public string FromMarket { get; set; }
        //public string ToMarket { get; set; }
        public string Remarkes { get; set; }
        //public string CreationDate { get; set; }
        //public string? UpdatedDate { get; set; }
        public string Status { get; set; }
        //public string ApprovedBy { get; set; }
        public List<DoctorInfoModel> doctorInfoModels { get; set; }
        public List<SupervisorInfoModel> supervisorInfoModels { get; set; }
    }
}
