using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using System.Collections.Generic;

namespace MRSAPI.Repository
{
    public class MenuRepository : IMenuRepository
    {
        public List<MenuInfoModel> GetMenuList()
        {
            List<MenuInfoModel> menuInfos = new List<MenuInfoModel>()
             = new List<Menu>
            {
                new Menu { Id = 1, Name = "Data 1" },
                new Menu { Id = 2, Name = "Data 2" },
                new Menu{ Id = 3, Name = "Data 3" }
                // Add more data as needed
            }.ToList();

            return list;
        
        }
    }
}
