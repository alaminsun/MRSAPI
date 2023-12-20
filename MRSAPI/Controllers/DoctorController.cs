
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using System.Reflection;
using System;

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
        public IActionResult GetDistrictInfo()
        {
            var data = _doctorRepo.GetUpazilaList();
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
        [HttpGet("[action]/{marketName}")]
        public IActionResult MarketInfo(string marketName)
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
        public async Task<IActionResult> PostDoctorFileAttchment([FromForm] FileUploadModel fileUpload)
        {

            var data = _doctorRepo.GetDoctorById(fileUpload.DoctorId);
            if (data == 0)
            {
                return NotFound();
            }
            if (fileUpload.File != null)
            {
                fileUpload.FilePath  = await _doctorRepo.SavePostImageAsync(fileUpload);
            }

            var postResponse = await _doctorRepo.CreatePostAsync(fileUpload);
           // Create the ApiResponse
            var response = new ApiResponse<FileUploadModel>
            {
                Message = "Data saved successfully.",
                Data = postResponse
            };
            return Ok(response);

        }

        /// <summary>
        /// fileType 1=PDF, 2=EXCEL, 3=DOCX, 4=IMAGE, 5=TEXT 
        /// </summary>
        /// <returns></returns>
        //[HttpPut("{id}")]
        //[HttpPut("{Id:int}", Name = "PutDoctorFileAttchment")]
        [HttpPut("[action]/{id:int}")]
        public async Task<IActionResult> PutDoctorFileAttchment(int id, [FromForm] FileUploadModel updatedItem)
        {
            //var existingItem = updatedItem.FirstOrDefault(i => i.Id == id);

            var existingItem = _doctorRepo.GetDoctorwithFileById(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            //var data = _context.Laptops.Where(e => e.Id == model.Id).SingleOrDefault();
            string uniqueFileName = string.Empty;
            if (updatedItem.File != null)
            {
                if (existingItem.FilePath != null)
                {
                    //string filePath = Path.Combine(_environment.WebRootPath, "Content/Laptop", data.Path);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", existingItem.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                //uniqueFileName = UploadImage(model);
                uniqueFileName = await _doctorRepo.SavePostImageAsync(updatedItem);
            }
            existingItem.FileName = updatedItem.FileName;
            existingItem.FileType = updatedItem.FileType;
            //existingItem.FilePath = updatedItem.FilePath;
            if (updatedItem.File != null)
            {
                existingItem.FilePath = uniqueFileName;
            }
    

            var postResponse = await _doctorRepo.UpdatePutAsync(id,existingItem);
            // Create the ApiResponse
            var response = new ApiResponse<FileUploadModel>
            {
                Message = "Data updated successfully.",
                Data = postResponse
            };
            return Ok(response);
        }


    }
}
