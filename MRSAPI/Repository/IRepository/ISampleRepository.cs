using MRSAPI.Models;
using System.Diagnostics;

namespace MRSAPI.Repository.IRepository
{
    public interface ISampleRepository
    {
        ICollection<GenericModel> GetAllGenericInfo();
    }
}
