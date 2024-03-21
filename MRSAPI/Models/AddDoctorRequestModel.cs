using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class AddDoctorRequestModel
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
        public List<DoctorInfoModel> doctorInfoModels { get; set; }
        public List<SupervisorInfoModel> supervisorInfoModels { get; set; }

    }
}
