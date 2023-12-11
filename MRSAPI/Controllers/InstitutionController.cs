using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRSAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.Headers;
using System.Reflection.PortableExecutable;

namespace MRSAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Institution")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly IInstitutionRepository _institutionRepo;
        public InstitutionController(IInstitutionRepository institutionRepo)
        {
            _institutionRepo = institutionRepo;
        }
        [HttpGet("[action]/{name}")]
        public IActionResult GetInstitutionList(string name)
        {
            var data = _institutionRepo.GetInstitutionList(name);
            if (data.Count() == 0)
            {
                return NotFound();
            }
            return Ok(data);

        }
    }
}
