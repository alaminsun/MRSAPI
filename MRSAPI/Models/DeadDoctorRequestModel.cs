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
        [Required]
        public string Remarkes { get; set; }
        public string Status { get; set; }
        public List<DoctorInfoModel> deadDoctorInfoModels { get; set; }
        public List<SupervisorInfoModel> doctorSupervisorInfoModels { get; set; }

    }

    public class DoctorInformation
    {
        [Required]
        //public int DoctorMasterId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public string Specialization { get; set; }

    }


}
