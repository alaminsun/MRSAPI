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
        public string Remarkes { get; set; }
        //public string CreationDate { get; set; }
        //public string? UpdatedDate { get; set; }
        public string Status { get; set; }
        //public string ApprovedBy { get; set; }
        public List<DeadDoctorInfoModel> deadDoctorInfoModels { get; set; }
        public List<DoctorSupervisorInfoModel> doctorSupervisorInfoModels { get; set; }

    }

    public class DeadDoctorInfoModel
    {
        //public int DoctorMasterId { get; set; }
        public int DoctorId { get; set; }

    }
    public class DoctorSupervisorInfoModel
    {
        //public int DoctorMasterId { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public string TerritoryCode { get; set; }
        public string MarketCode { get; set; }
        public string IsSupervisor { get; set; }
        public string Remarkes { get; set; }
        //public string ApprovalStatus { get; set; }
        //public string CreatedDate { get; set; }

    }


}
