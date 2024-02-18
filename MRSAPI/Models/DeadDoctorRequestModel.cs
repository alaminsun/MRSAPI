using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class DeadDoctorRequestModel
    {

        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string MarketCode { get; set; }
        //public string OperationType { get; set; }
        [Required]
        public string Remarkes { get; set; }
        //public string CreationDate { get; set; }
        //public string? UpdatedDate { get; set; }
        public string Status { get; set; }
        //public string ApprovedBy { get; set; }
        public List<DoctorInfoModel> deadDoctorInfoModels { get; set; }
        public List<SupervisorInfoModel> doctorSupervisorInfoModels { get; set; }

    }

    public class DoctorInfoModel
    {
        [Required]
        //public int DoctorMasterId { get; set; }
        public int DoctorId { get; set; }

    }
    public class SupervisorInfoModel
    {
        //public int DoctorMasterId { get; set; }
        [Required]
        //public string EmployeeId { get; set; }
        public string TerritoryCode { get; set; }
        [Required]
        public string MarketCode { get; set; }
        [Required]
        public string IsSupervisor { get; set; }
        //public string Remarkes { get; set; }
        //public string ApprovalStatus { get; set; }
        //public string CreatedDate { get; set; }

    }


}
