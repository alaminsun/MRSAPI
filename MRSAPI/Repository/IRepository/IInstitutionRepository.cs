using MRSAPI.Models;

namespace MRSAPI.Repository.IRepository
{
    public interface IInstitutionRepository
    {
        List<InstitutionModel> GetInstitutionList(string name);
    }
}
