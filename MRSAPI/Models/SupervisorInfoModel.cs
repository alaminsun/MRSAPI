using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class SupervisorInfoModel
    {
        public string TerritoryCode { get; set; }
        [Required]
        public string MarketCode { get; set; }
        [Required]
        public string IsSupervisor { get; set; }
    }
}
