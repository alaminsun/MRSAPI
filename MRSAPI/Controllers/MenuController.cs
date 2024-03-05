using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;

namespace MRSAPI.Controllers
{
    [Route("api/v{version:apiVersion}/Menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        //private readonly IMenuRepository _menuRepo;
        //public MenuController(IMenuRepository menuRepo)
        //{
        //    _menuRepo = menuRepo;
        //}

        private readonly List<Menu> _staticData;

        public MenuController()
        {
            // Initialize your static data
            _staticData = new List<Menu>
            {
                new Menu { Id = 1, Name = "Home", Type="Navigation", IsActive=true  },
                new Menu { Id = 2, Name = "Products", Type="drawer", IsActive=false }
                // Add more data as needed
            };
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<MenuInfoModel>> GetMenuList()
        {

            return Ok(_staticData);
        }
    }
     
}
