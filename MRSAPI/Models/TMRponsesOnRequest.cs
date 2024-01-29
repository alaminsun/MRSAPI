using MRSAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class TMRponsesOnRequest
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        //public string Status { get; set; }
        public List<TMResponses> TMResponses { get; set; }
    }

    public class TMResponses
    {
        public long Id { get; set; }
        public string ApprovalStatus { get; set; }
        //public string UpdatedDate { get; set; }
        //public string Remarks { get; set; }

    }
}
