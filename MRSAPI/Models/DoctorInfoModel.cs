using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class DoctorInfoModel
    {
        [Required]
        //public int DoctorMasterId { get; set; }
        public int DoctorId { get; set; }
    }
}
