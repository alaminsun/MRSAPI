using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRSAPI.Data;
using MRSAPI.Gateway;
using MRSAPI.Repository.IRepository;
using System.Security.Cryptography.Xml;

namespace MRSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ISampleRepository _sampleRepo;
        public SampleController(ISampleRepository sampleRepo)
        {
            _sampleRepo = sampleRepo;
        }

        [HttpGet]
        public IActionResult GetGeneric()
        {
            var data = _sampleRepo.GetAllGenericInfo();
            //var allGeneric = Json(data);
            //allGeneric.MaxJsonLength = int.MaxValue;
            //return allGeneric;
            return Ok(data);

        }
        
    }
}
