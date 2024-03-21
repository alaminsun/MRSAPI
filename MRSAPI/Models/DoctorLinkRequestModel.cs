using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class DoctorLinkRequestModel
    {
        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string MarketCode { get; set; }
        public string Remarkes { get; set; }
        public string Status { get; set; }
        public List<DoctorInfoModel> doctorInfoModels { get; set; }
        public List<SupervisorInfoModel> supervisorInfoModels { get; set; }
    }
}
