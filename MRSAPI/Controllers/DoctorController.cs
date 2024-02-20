
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using System.Reflection;
using System;
using Oracle.ManagedDataAccess.Client;
using MRSAPI.Models.DTO;

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
        public IActionResult GetDoctors(string doctorName, string? registrationNo, string? mobileNo, string designation, string specialization)
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
        public IActionResult GetDistrictInfo()
        {
            var data = _doctorRepo.GetDistrictList();
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpGet("[action]")]
        public IActionResult GetUpazila()
        {
            var data = _doctorRepo.GetUpazilaList();
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

        //[HttpGet("[action]")]
        [HttpGet("[action]")]
        public IActionResult MarketInfo(string? marketName)
        {
            var data = _doctorRepo.GetMarketListWithSBU(marketName);
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        /// <summary>
        /// fileType 1=PDF, 2=EXCEL, 3=DOCX, 4=IMAGE, 5=TEXT 
        /// </summary>
        /// <returns></returns>
        //[HttpPost("[action]")]
        //[HttpPost("{DoctorId:int}", Name = "DoctorFileAttchment")]
        [HttpPost("[action]/{DoctorId:int}")]
        public async Task<IActionResult> PostDoctorFileAttchment([FromForm] FileUploadDTO fileUpload)
        {
            //List<string> FilePathList = null;
            //var data = _doctorRepo.GetDoctorById(fileUpload.DoctorId);
            //if (data == 0)
            //{
            //    return NotFound();
            //}
            //if (fileUpload == null || fileUpload.Files == null || fileUpload.Files.Count == 0)
            //{
            //    return BadRequest("No files uploaded.");
            //}

            //if (fileUpload.Files != null)
            //{
            //    FilePathList = await _doctorRepo.SavePostImageAsync(fileUpload);
            //}

            //var postResponse = await _doctorRepo.CreatePostAsync(fileUpload, FilePathList);
            //int Id = _doctorRepo.GetFileAttachmentId(postResponse.DoctorId, postResponse.AttachmentType);

            //return Ok(new { Message = "Data saved successfully.", Id, FilePathList, postResponse.AttachmentType });
            try
            {
                if (fileUpload == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<string> FilePathList = null;
                var data = _doctorRepo.GetDoctorById(fileUpload.DoctorId);
                if (data == 0)
                {
                    return NotFound();
                }
                if (fileUpload == null || fileUpload.Files == null || fileUpload.Files.Count == 0)
                {
                    return BadRequest("No files uploaded.");
                }
                long Code = 0;
                var obj = new FileUploadModel
                {
                    DoctorId = fileUpload.DoctorId,
                    Id = Code,
                    Files = fileUpload.Files,
                    FilePathList = FilePathList,
                    AttachmentTypes = fileUpload.AttachmentTypes,
                    // Map other properties manually if needed
                };

                if (fileUpload.Files != null)
                {
                    obj.FilePathList = await _doctorRepo.SavePostImageAsync(fileUpload);
                }
                if (!await _doctorRepo.CreatePostAsync(obj))
                {
                    ModelState.AddModelError("", $"Something went wrong when save the record");
                    return StatusCode(500, ModelState);
                }
                return Ok(new { Message = "Data saved successfully.", obj.Id, obj });
            }
            catch (Exception ex)
            {
                // Log the exception (you may want to log it to a file or another storage)
                Console.WriteLine(ex.Message);

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "An error occurred while saving the data." });
            }
        }


        //[HttpPut("{id}")]
        //[HttpPut("{Id:int}", Name = "PutDoctorFileAttchment")]
        //[HttpPut("[action]/{id:int}")]
        //public async Task<IActionResult> PutDoctorFileAttchment(int id, [FromForm] FileUploadModel updatedItem)
        //{
        //    //var existingItem = updatedItem.FirstOrDefault(i => i.Id == id);

        //    var existingItem = _doctorRepo.GetDoctorwithFileById(id);
        //    if (existingItem == null)
        //    {
        //        return NotFound();
        //    }
        //    //var data = _context.Laptops.Where(e => e.Id == model.Id).SingleOrDefault();
        //    string uniqueFileName = string.Empty;
        //    if (updatedItem.File != null)
        //    {
        //        if (existingItem.FilePath != null)
        //        {
        //            //string filePath = Path.Combine(_environment.WebRootPath, "Content/Laptop", data.Path);
        //            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", existingItem.FilePath);
        //            if (System.IO.File.Exists(filePath))
        //            {
        //                System.IO.File.Delete(filePath);
        //            }
        //        }
        //        //uniqueFileName = UploadImage(model);
        //        uniqueFileName = await _doctorRepo.SavePostImageAsync(updatedItem);
        //    }
        //    existingItem.FileName = updatedItem.FileName;
        //    existingItem.FileType = updatedItem.FileType;
        //    //existingItem.FilePath = updatedItem.FilePath;
        //    if (updatedItem.File != null)
        //    {
        //        existingItem.FilePath = uniqueFileName;
        //    }


        //    var postResponse = await _doctorRepo.UpdatePutAsync(id, existingItem);
        //    // Create the ApiResponse
        //    var response = new ApiResponse<FileUploadModel>
        //    {
        //        Message = "Data updated successfully.",
        //        Data = postResponse
        //    };
        //    return Ok(response);
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> PostDoctorInformation([FromBody] DoctorInformationAPIDTO doctorInformationAPIDTO)
        {
            try
            {
                if (doctorInformationAPIDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                int doctorId = 0;
                var obj = new DoctorInformationAPIModel
                {
                    EmployeeId = doctorInformationAPIDTO.EmployeeId,
                    DoctorId = doctorId,
                    DoctorName = doctorInformationAPIDTO.DoctorMasterModels.DoctorName,
                    RegistrationNo = doctorInformationAPIDTO.DoctorMasterModels.RegistrationNo,
                    Gender = doctorInformationAPIDTO.DoctorMasterModels.Gender,
                    Religion = doctorInformationAPIDTO.DoctorMasterModels.Religion,
                    DateOfBirth = doctorInformationAPIDTO.DoctorMasterModels.DateOfBirth,
                    personalContactNumber = doctorInformationAPIDTO.DoctorMasterModels.personalContactNumber,
                    chamberContactNumber = doctorInformationAPIDTO.DoctorMasterModels.chamberContactNumber,
                    Email = doctorInformationAPIDTO.DoctorMasterModels.Email,
                    SpecializationCode = doctorInformationAPIDTO.DoctorMasterModels.SpecializationCode,
                    PotentialCategory = doctorInformationAPIDTO.DoctorMasterModels.PotentialCategory,
                    PatientNoPerDay = doctorInformationAPIDTO.DoctorMasterModels.PatientNoPerDay,
                    ValuePerPrescription = doctorInformationAPIDTO.DoctorMasterModels.ValuePerPrescription,
                    Address = doctorInformationAPIDTO.DoctorMasterModels.Address,
                    DesignationCode = doctorInformationAPIDTO.DoctorMasterModels.DesignationCode,
                    DegreeTitle = doctorInformationAPIDTO.DoctorMasterModels.DegreeTitle,
                    DegreeCode = doctorInformationAPIDTO.DoctorMasterModels.DegreeCode,
                    Remarks = doctorInformationAPIDTO.DoctorMasterModels.Remarks,
                    DoctorMarketDetailsModels = doctorInformationAPIDTO.DoctorMasterModels.DoctorMarketDetailsModels,
                    DoctorInSBUs = doctorInformationAPIDTO.DoctorMasterModels.DoctorInSBUs

                    // Map other properties manually if needed
                };
                if (!await _doctorRepo.SaveDoctorInfo(obj))
                {
                    ModelState.AddModelError("", $"Something went wrong when save the record");
                    return StatusCode(500, ModelState);
                }
                return Ok(new { Message = "Data saved successfully.", obj.DoctorId, obj });
            }
            catch (Exception ex)
            {
                // Log the exception (you may want to log it to a file or another storage)
                Console.WriteLine(ex.Message);

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "An error occurred while saving the data." });
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeadDoctorRequest([FromBody] DeadDoctorRequestDTO deadDoctorRequestDTO)
        {
            try
            {
                if (deadDoctorRequestDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                int Code = 0;
                var obj = new DeadDoctorRequestModel
                {
                    EmployeeId = deadDoctorRequestDTO.EmployeeId,
                    Id = Code,
                    MarketCode = deadDoctorRequestDTO.MarketCode,
                    Remarkes = deadDoctorRequestDTO.Remarkes,
                    Status = deadDoctorRequestDTO.Status,
                    deadDoctorInfoModels = deadDoctorRequestDTO.doctorInfoModels,
                    doctorSupervisorInfoModels = deadDoctorRequestDTO.supervisorInfoModels

                    // Map other properties manually if needed
                };
                if (!await _doctorRepo.DeadDoctorWithMarket(obj))
                {
                    ModelState.AddModelError("", $"Something went wrong when save the record");
                    return StatusCode(500, ModelState);
                }
                return Ok(new { Message = "Data saved successfully.", obj.Id, obj });
            }
            catch (Exception ex)
            {
                // Log the exception (you may want to log it to a file or another storage)
                Console.WriteLine(ex.Message);

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "An error occurred while saving the data." });
            }
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> DoctorShiftRequest([FromBody] DoctorShiftRequestDTO doctorShiftRequestDTO)
        //{
        //    try
        //    {
        //        if (doctorShiftRequestDTO == null)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        int Code = 0;
        //        var obj = new DoctorShiftRequestModel
        //        {
        //            EmployeeId = doctorShiftRequestDTO.EmployeeId,
        //            Id = Code,
        //            MarketCode = doctorShiftRequestDTO.MarketCode,
        //            FromMarket = doctorShiftRequestDTO.FromMarket,
        //            ToMarket = doctorShiftRequestDTO.ToMarket,
        //            Remarkes = doctorShiftRequestDTO.Remarkes,
        //            Status = doctorShiftRequestDTO.Status,
        //            doctorInfoModels = doctorShiftRequestDTO.doctorInfoModels,
        //            supervisorInfoModels = doctorShiftRequestDTO.supervisorInfoModels

        //            // Map other properties manually if needed
        //        };
        //        if (!await _doctorRepo.DoctorShiftMarket(obj))
        //        {
        //            ModelState.AddModelError("", $"Something went wrong when save the record");
        //            return StatusCode(500, ModelState);
        //        }
        //        return Ok(new { Message = "Data saved successfully.", obj.Id, obj });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (you may want to log it to a file or another storage)
        //        Console.WriteLine(ex.Message);

        //        // Return a 500 Internal Server Error response
        //        return StatusCode(500, new { Message = "An error occurred while saving the data." });
        //    }
        //}

        //[HttpPost("[action]")]
        //public async Task<IActionResult> DoctorLinkRequest([FromBody] DoctorLinkRequestDTO doctorLinkRequestDTO)
        //{
        //    try
        //    {
        //        if (doctorLinkRequestDTO == null)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        int Code = 0;
        //        var obj = new DoctorLinkRequestModel
        //        {
        //            EmployeeId = doctorLinkRequestDTO.EmployeeId,
        //            Id = Code,
        //            MarketCode = doctorLinkRequestDTO.MarketCode,
        //            //FromMarket = doctorShiftRequestDTO.FromMarket,
        //            //ToMarket = doctorShiftRequestDTO.ToMarket,
        //            Remarkes = doctorLinkRequestDTO.Remarkes,
        //            Status = doctorLinkRequestDTO.Status,
        //            doctorInfoModels = doctorLinkRequestDTO.doctorInfoModels,
        //            supervisorInfoModels = doctorLinkRequestDTO.supervisorInfoModels

        //            // Map other properties manually if needed
        //        };
        //        if (!await _doctorRepo.DoctorLinkWithMarket(obj))
        //        {
        //            ModelState.AddModelError("", $"Something went wrong when save the record");
        //            return StatusCode(500, ModelState);
        //        }
        //        return Ok(new { Message = "Data saved successfully.", obj.Id, obj });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (you may want to log it to a file or another storage)
        //        Console.WriteLine(ex.Message);

        //        // Return a 500 Internal Server Error response
        //        return StatusCode(500, new { Message = "An error occurred while saving the data." });
        //    }
        //}

        //[HttpPut("[action]")]
        //public async Task<IActionResult> TMReponseOnRequest([FromBody] TMRponsesOnRequest model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        } 
        //        if (!await _doctorRepo.TMResponse(model))
        //        {
        //            ModelState.AddModelError("", $"Something went wrong when save the record");
        //            return StatusCode(500, ModelState);
        //        }
        //        //return Ok(new { Message = "Data saved successfully.", postResponse.Id });
        //        return Ok(new { Message = "Data saved successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (you may want to log it to a file or another storage)
        //        Console.WriteLine(ex.Message);

        //        // Return a 500 Internal Server Error response
        //        return StatusCode(500, new { Message = "An error occurred while saving the data." });
        //    }
        //}


        //[HttpDelete("[action]")]
        //public IActionResult DeleteDoctorMarket(int Id)
        //{
        //    if (!_doctorRepo.MarketExist(Id))
        //    {
        //        return NotFound();
        //    }
        //    var Obj = _doctorRepo.GetMarketById(Id);

        //    if (!_doctorRepo.DeleteMarketWithDocotor(Obj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when deleting the record {Obj.MarketCode}");
        //        return StatusCode(500, ModelState);
        //    }

        //    //return NoContent();
        //    return Ok(new { Message = "Data deleted successfully.", Id });
        //}

        //[HttpGet("[action]")]
        [HttpGet("[action]/{territoryCode}")]
        public IActionResult GetMPORequest(string territoryCode)
        {
            var data = _doctorRepo.GetMPORequestByTMId(territoryCode);
            if (data.Count() == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("[action]/{marketCode}")]
        public IActionResult TerritoryByMarket(string marketCode)
        {
            var data = _doctorRepo.GetTerritoryByMarket(marketCode);
            if (data.Count() == 0)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpPut("[action]")]
        public async Task<IActionResult> TMReponseOnRequest([FromBody] TMRponsesOnRequestDTO tMRponsesOnRequest)
        {
            try
            {
                if (tMRponsesOnRequest == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string Code = String.Empty;
                var obj = new TMRponsesOnRequest
                {
                    EmployeeId = tMRponsesOnRequest.EmployeeId,
                    Id = tMRponsesOnRequest.Id,
                    TMResponses = tMRponsesOnRequest.TMResponsesDTOs


                    // Map other properties manually if needed
                };
                if (!await _doctorRepo.UpdateTMInfo(obj))
                {
                    ModelState.AddModelError("", $"Something went wrong when save the record");
                    return StatusCode(500, ModelState);
                }
                return Ok(new { Message = "Data saved successfully.", obj });
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
