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
        List<DoctorInformationModel> GetPotentialCategoryList();
    }
}
