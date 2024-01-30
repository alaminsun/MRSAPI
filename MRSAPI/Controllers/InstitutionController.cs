using Microsoft.AspNetCore.Mvc;
using MRSAPI.Repository.IRepository;
using MRSAPI.Models;
using MRSAPI.Models.DTO;

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

        [HttpGet("[action]")]
        public IActionResult InstitutionTypeList()
        {
            var data = _institutionRepo.GetInstitutionTypeList();
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PostInstitutionInformation([FromBody] InstitutionInformationDTO institutionInformationDTO)
        {
            try
            {
                if (institutionInformationDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string Code = String.Empty;
                var obj = new InstitutionInfoModel
                {
                    EmployeeId = institutionInformationDTO.EmployeeId,
                    InstituteCode = Code,
                    InstituteName = institutionInformationDTO.InstitutionInfo.InstituteName,
                    InstituteTypeCode = institutionInformationDTO.InstitutionInfo.InstituteTypeCode,
                    Address = institutionInformationDTO.InstitutionInfo.Address,
                    InstitutePhone = institutionInformationDTO.InstitutionInfo.InstitutePhone,
                    UpazilaCode = institutionInformationDTO.InstitutionInfo.UpazilaCode,
                    DistrictCode = institutionInformationDTO.InstitutionInfo.DistrictCode,
                    NoOfBeds = institutionInformationDTO.InstitutionInfo.NoOfBeds,
                    AvgNoAdmitPatient = institutionInformationDTO.InstitutionInfo.AvgNoAdmitPatient,
                    AvgNoOutDoorPatient = institutionInformationDTO.InstitutionInfo.AvgNoOutDoorPatient,
                    Remarks = institutionInformationDTO.InstitutionInfo.InstituteName,

                    // Map other properties manually if needed
                };
                if (!await _institutionRepo.CreateInstitute(obj))
                {
                    ModelState.AddModelError("", $"Something went wrong when save the record");
                    return StatusCode(500, ModelState);
                }
                return Ok(new { Message = "Data saved successfully.", obj.InstituteCode, obj });
                //return CreatedAtAction(nameof(GetEntity), new { id = model.InstitutionInfo.InstituteCode }, model);
            }
            catch (Exception ex)
            {
                // Log the exception (you may want to log it to a file or another storage)           
                Console.WriteLine(ex.Message);

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "An error occurred while saving the data." });
            }

        }
    }
}
