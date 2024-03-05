using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models.DTO
{
    public class TMRponsesOnRequestDTO
    {
        [Required]
        public long MstId { get; set; }
        [Required]
        //public string EmployeeId { get; set; }
        public string TerritoryCode { get; set; }
        
        public List<TMResponses> TMResponsesDTOs { get; set; }
    }
}
