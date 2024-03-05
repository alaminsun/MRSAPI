
using MRSAPI.Models;
using MRSAPI.Models.DTO;
using System.Numerics;
using System.Reflection;

namespace MRSAPI.Repository.IRepository
{
    public interface IDoctorRepository
    {
        List<DoctorInformationModel> GetDoctorList(string doctorName, string? registrationNo, string? mobileNo, string designation, string specialization);
        List<DoctorModel> GetDoctorsByMarketCode(string marketCode);
        List<DesignationModel> GetDesignationList();
        List<SpecializationModel> GetSpecializationList();
        List<LocationModel> GetLocation();
        List<PotentialCategoryModel> GetPotentialCategoryList();
        long GetDoctorById(int doctorId);
        string GetFilePatheWithName(string fileName);
        //FileUploadModel GetDoctorwithFileById(int id);
        //Task SavePostImageAsync(FileUploadModel fileUpload);
        Task<List<string>> SavePostImageAsync(FileUploadDTO fileUpload);
        Task<bool> CreatePostAsync(FileUploadModel fileUpload);
        //Task<FileUploadModel> UpdatePutAsync(int id ,FileUploadModel existingItem);
        List<DistrictModel> GetDistrictList();
        List<UpazilaModel> GetUpazilaList();
        List<MarketInfoModel> GetMarketListWithSBU(string marketName);
        Task<bool> SaveDoctorInfo(DoctorInformationAPIModel model);
        //Task<bool> SaveDoctorInfo(DoctorInformationAPIModel model);
        int GetFileAttachmentId(int doctorId,string attachmentType);
        //Task<bool> LinkDoctorWithMarket(DeadDoctorRequestModel model);
        Task<bool> DoctorShiftMarket(DoctorShiftRequestModel model);
        bool MarketExist(int id);
        DoctorShiftRequestModel GetMarketById(int id);
        bool DeleteMarketWithDocotor(DoctorDeleteRequestModel obj);
        Task<bool> DeadDoctorWithMarket(DeadDoctorRequestModel model);
        List<MPORequestModel> GetMPODeadRequestByTMId(string territoryCode);
        //List<MPORequestModel> GetMPOShiftRequestByTMId(string territoryCode);
        //List<MPORequestModel> GetMPODeleteRequestByTMId(string territoryCode);
        //Task<bool> TMResponse(TMRponsesOnRequest model);
        Task<bool> UpdateTMInfo(TMRponsesOnRequest obj);
        List<TerritoryModel> GetTerritoryByMarket(string marketCode);
        //Task CreatePostAsync(FileUploadModel fileUpload, List<string>? filePath);
        Task<bool> DoctorDeleteMarket(DoctorDeleteRequestModel model);
    }
}
