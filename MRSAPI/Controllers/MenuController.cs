using Microsoft.AspNetCore.Mvc;
using MRSAPI.Repository.IRepository;

namespace MRSAPI.Controllers
{
    [Route("api/v{version:apiVersion}/Menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepo;
        public MenuController(IMenuRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        [HttpGet("[action]")]
        public IActionResult GetMenuList()
        {
            var data = _menuRepo.GetMenuList();
            if (data.Count() == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
     
}
