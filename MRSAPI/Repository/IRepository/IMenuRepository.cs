using MRSAPI.Models;

namespace MRSAPI.Repository.IRepository
{
    public interface IMenuRepository
    {
        List<Menu> GetMenuList();
    }
}
