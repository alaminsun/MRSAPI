
using MRSAPI.Models;
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
        Task<List<string>> SavePostImageAsync(FileUploadModel fileUpload);
        Task<FileUploadModel> CreatePostAsync(FileUploadModel fileUpload, List<string> FilePathList);
        //Task<FileUploadModel> UpdatePutAsync(int id ,FileUploadModel existingItem);
        List<DistrictModel> GetDistrictList();
        List<UpazilaModel> GetUpazilaList();
        List<MarketInfoModel> GetMarketListWithSBU(string marketName);
        Task<DoctorInformationAPIModel> SaveDoctorInfo(DoctorInformationAPIModel model);
        //Task<bool> SaveDoctorInfo(DoctorInformationAPIModel model);
        int GetFileAttachmentId(int doctorId,string attachmentType);
        Task<bool> LinkDoctorWithMarket(DeadDoctorRequestModel model);
        Task<bool> DoctorShiftMarket(DoctorShiftRequestModel model);
        bool MarketExist(int id);
        DoctorShiftRequestModel GetMarketById(int id);
        bool DeleteMarketWithDocotor(DoctorShiftRequestModel obj);
        Task<bool> DoctorLinkWithMarket(DoctorLinkRequestModel model);
        List<MPORequestModel> GetMPORequestByTMId(string TmId);
        //Task<bool> TMResponse(TMRponsesOnRequest model);
        Task<bool> UpdateTMInfo(TMRponsesOnRequest obj);
        //Task CreatePostAsync(FileUploadModel fileUpload, List<string>? filePath);
    }
}
