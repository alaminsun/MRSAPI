
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
        Task<string> SavePostImageAsync(FileUploadModel fileUpload, string FilePath);
        Task<FileUploadModel> CreatePostAsync(FileUploadModel fileUpload,string FilePath);
        //Task<FileUploadModel> UpdatePutAsync(int id ,FileUploadModel existingItem);
        List<DistrictModel> GetDistrictList();
        List<UpazilaModel> GetUpazilaList();
        List<MarketInfoModel> GetMarketListWithSBU(string marketName);
        Task<DoctorInformationAPIModel> SaveDoctorInfo(DoctorInformationAPIModel model);
        //Task<bool> SaveDoctorInfo(DoctorInformationAPIModel model);
        int GetFileAttachmentId(int doctorId,string attachmentType,string filePath);
        Task<DeadDoctorModel> LinkDoctorWithMarket(DeadDoctorModel model);
        Task<DoctorShiftModel> DoctorShiftMarket(DoctorShiftModel model);
        bool MarketExist(int id);
        DoctorShiftModel GetMarketById(int id);
        bool DeleteMarketWithDocotor(DoctorShiftModel obj);
    }
}
