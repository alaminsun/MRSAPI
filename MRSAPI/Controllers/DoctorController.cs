using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRSAPI.Repository.IRepository;

namespace MRSAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepo;
        public DoctorController(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }

        //[HttpGet("{doctorName},{designation}", Name = "GetDoctors")]
        [HttpGet("[action]/{doctorName},{designation},{specialization}")]
        public IActionResult GetDoctors(string doctorName,string? registrationNo, string? mobileNo, string designation, string specialization)
        {
            var data = _doctorRepo.GetDoctorList(doctorName, registrationNo, mobileNo, designation, specialization);
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }
        [HttpGet("[action]/{marketCode}")]
        public IActionResult GetDoctorsByMarket(string marketCode)
        {
            var data = _doctorRepo.GetDoctorsByMarketCode(marketCode);
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpGet("[action]")]
        public IActionResult GetDesignationList()
        { 
            var data = _doctorRepo.GetDesignationList();
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpGet("[action]")]
        public IActionResult GetSpecializationList()
        {
            var data = _doctorRepo.GetSpecializationList();
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpGet("[action]")]
        public IActionResult GetLocation()
        {
            var data = _doctorRepo.GetLocation();
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpGet("[action]")]
        public IActionResult GetPotentialCategoryList()
        {
            var data = _doctorRepo.GetPotentialCategoryList();
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }
    }
}
