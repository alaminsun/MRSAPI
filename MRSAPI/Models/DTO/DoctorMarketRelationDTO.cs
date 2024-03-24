using MRSAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models.DTO
{
    public class DoctorMarketRelationDTO
    {
        [Required]
        public string EmployeeId { get; set; }
        //public long DoctorMstSl; 
        public long DoctorId;
        [Required]
        public string DoctorName { get; set; }
        public string? RegistrationNo { get; set; }
        public string? Gender { get; set; }
        public string? Religion { get; set; }
        [CustomDateFormat("dd-MM-yyyy")]
        public string DateOfBirth { get; set; }
        public string? personalContactNumber { get; set; }
        //public string? chamberContactNumber { get; set; }
        public string? Email { get; set; }
        public string? SpecializationCode { get; set; }
        public string PotentialCategory { get; set; }
        public int? PatientNoPerDay { get; set; }
        public int? ValuePerPrescription { get; set; }
        public string? Address { get; set; }
        public string? DesignationCode { get; set; }
        public string? DegreeTitle { get; set; }
        public string? DegreeCode { get; set; }
        public string? Remarks { get; set; }
        public List<DoctorMarketDetailsDTO>? DoctorMarketDetailsModels { get; set; }
        //public List<DoctorRelativeInfo> Relatives { get; set; }
        public List<DoctorInSBUDTO> DoctorInSBUs { get; set; }
    }
}
