using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{
    public class MPORequestModel
    {
        public int MstId { get; set; }
        //public string EmployeeId { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string OperationType { get; set; }
        public string FromMarket { get; set; }
        public string ToMarket { get; set; }
        public string ToMarketName { get; set; }
        public string Status { get; set; }
        public string Remarkes { get; set; }
        public List<DoctorInfoModel> DoctorList { get; set; }
        public List<LinkTMModel> TMList { get; set; }

    }
    public class LinkTMModel
    {
        public int Id { get; set; }
        public string TerritoryCode { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string IsSupervisor { get; set; }
        public string Remarks { get; set; }
        public string ApprovalStatus { get; set; }


    }
}
