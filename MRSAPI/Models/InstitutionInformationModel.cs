using System.ComponentModel.DataAnnotations;

namespace MRSAPI.Models
{


    //public class InstitutionInformationModel
    //{
        
    //    public InstitutionInfoModel InstitutionInfo { get; set; }
    //}

    public class InstitutionInfoModel
    {
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string InstituteCode { get; set; }
        [Required]
        public string InstituteName { get; set; }
        public string InstituteTypeCode { get; set; }
        //public string InstituteTypeName { get; set; }
        public string Address { get; set; }
        public string InstitutePhone { get; set; }
        public string UpazilaCode { get; set; }
        //public string UPAZILA_NAME { get; set; }
        public string DistrictCode { get; set; }
        //public string DistrictName { get; set; }
        public int NoOfBeds { get; set; }
        public int AvgNoAdmitPatient { get; set; }
        public int AvgNoOutDoorPatient { get; set; }
        public string Remarks { get; set; }

    }


}
