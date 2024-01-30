using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models.DTO
{
    public class DoctorInformationAPIDTO
    {
        [Required]
        public string EmployeeId { get; set; }

        public DoctorMaster DoctorMasterModels { get; set; }

    }
}
