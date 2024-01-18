using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class DeadDoctorLocationModel
    {

        public int Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string MarketCode { get; set; }
        public string OperationType { get; set; }
        public string Remarkes { get; set; }
        public string CreationDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Status { get; set; } = "Pending";
        public List<DeadDoctorInfoModel> deadDoctorInfoModels { get; set; }
        public string ApprovedBy { get; internal set; }
    }

    public class DeadDoctorInfoModel
    {
        public int DeadDoctorMasterId { get; set; }
        public int DoctorId { get; set; }


    }


}
