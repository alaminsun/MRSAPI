﻿
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using System.Reflection;
using System;
using Oracle.ManagedDataAccess.Client;

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
        public async Task<IActionResult> PostDoctorFileAttchment([FromForm] FileUploadModel fileUpload)
        {
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

            // Process each uploaded file
            //foreach (var file in fileUpload.Files)
            //{
            //    //// Perform file processing logic here (e.g., save to disk, store in database)
            //    //// Example: Save to disk
            //    //var filePath = Path.Combine("YourUploadDirectory", file.FileName);
            //    //using (var stream = new FileStream(filePath, FileMode.Create))
            //    //{
            //    //    file.CopyTo(stream);
            //    //}
            //    FilePath = await _doctorRepo.SavePostImageAsync(fileUpload, FilePath);
            //}
            if (fileUpload.Files != null)
            {
                FilePathList = await _doctorRepo.SavePostImageAsync(fileUpload);
            }

            var postResponse = await _doctorRepo.CreatePostAsync(fileUpload, FilePathList);
            int Id = _doctorRepo.GetFileAttachmentId(postResponse.DoctorId, postResponse.AttachmentType);

            return Ok(new { Message = "Data saved successfully.", Id, FilePathList, postResponse.AttachmentType });
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
        public async Task<IActionResult> PostDoctorInformation([FromBody] DoctorInformationAPIModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var postResponse = await _doctorRepo.SaveDoctorInfo(model);
                // Create the ApiResponse
                if (postResponse.DoctorMasterModels.DoctorId == 0)
                {
                    return BadRequest(new { message = "Error while saving" });
                }
                var response = new ApiResponse<DoctorInformationAPIModel>
                {
                    Message = "Data saved successfully.",
                    DoctorId = postResponse.DoctorMasterModels.DoctorId,
                    Data = postResponse
                };
                return Ok(response);
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
        public async Task<IActionResult> DeadDoctorLinkedRequest([FromBody] DeadDoctorRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var postResponse = await _doctorRepo.LinkDoctorWithMarket(model);
                // Create the ApiResponse
                if (postResponse.Id == 0)
                {
                    return BadRequest(new { message = "Error while saving" });
                }
                //return Ok(response);
                return Ok(new { Message = "Data saved successfully.", postResponse.Id });
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
        public async Task<IActionResult> DoctorShiftRequest([FromBody] DoctorShiftRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var postResponse = await _doctorRepo.DoctorShiftMarket(model);
                // Create the ApiResponse
                if (postResponse.Id == 0)
                {
                    return BadRequest(new { message = "Error while saving" });
                }
                //return Ok(response);
                return Ok(new { Message = "Data saved successfully.", postResponse.Id });
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
        public async Task<IActionResult> DoctorLinkRequest([FromBody] DoctorLinkRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //var postResponse = await _doctorRepo.DoctorShiftMarket(model);
                //// Create the ApiResponse
                //if (postResponse.Id == 0)
                //{
                //    return BadRequest(new { message = "Error while saving" });
                //}
                if (!await _doctorRepo.DoctorLinkWithMarket(model))
                {
                    ModelState.AddModelError("", $"Something went wrong when save the record");
                    return StatusCode(500, ModelState);
                }
                //return Ok(new { Message = "Data saved successfully.", postResponse.Id });
                return Ok(new { Message = "Data saved successfully."});
            }
            catch (Exception ex)
            {
                // Log the exception (you may want to log it to a file or another storage)
                Console.WriteLine(ex.Message);

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "An error occurred while saving the data." });
            }
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteDoctorMarket(int Id)
        {
            if (!_doctorRepo.MarketExist(Id))
            {
                return NotFound();
            }
            var Obj = _doctorRepo.GetMarketById(Id);

            if (!_doctorRepo.DeleteMarketWithDocotor(Obj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {Obj.MarketCode}");
                return StatusCode(500, ModelState);
            }

            //return NoContent();
            return Ok(new { Message = "Data deleted successfully.", Id });
        }
    }
}
