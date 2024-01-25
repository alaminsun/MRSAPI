using MRSAPI.Models;
using MRSAPI.Models.DTO;

namespace MRSAPI.Repository.IRepository
{
    public interface IInstitutionRepository
    {
        Task<bool> CreateInstitute(InstitutionInfoModel model);
        InstitutionInfoModel GetInstitutionById(int id);
        List<InstitutionModel> GetInstitutionList(string name);
        List<InstitutionTypeModel> GetInstitutionTypeList();
    }
}
