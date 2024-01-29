using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models.DTO
{
    public class TMRponsesOnRequestDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public List<TMResponses> TMResponsesDTOs { get; set; }
    }
}
