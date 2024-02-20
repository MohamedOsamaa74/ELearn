using ELearn.Application.DTOs;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.UnitOfWork;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AssignmentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = appDbContext;



        }
        #region Delete Assignment
        [HttpDelete("Delete/{AssignmentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAssignment(int AssignmentId)
        {
            var Assignment = await _unitOfWork.Materials.GetByIdAsync(AssignmentId);
            if (Assignment == null)
            {
                return BadRequest("Assignment not found.");
            }
            else
            {
                await _unitOfWork.Materials.DeleteAsync(Assignment);
                return Ok("Assignment Deleted Successfully");
            }
        }
        #endregion
        #region update Assignment
        [HttpPut("UpdateAssignment/{AssignmentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAssignment(int AssignmentId, [FromBody] AssignmentDTO updateDto)
        {
            try
            {
                var AssignmentToUpdate = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
                if (AssignmentToUpdate == null)
                {
                    return NotFound($"Assignment with ID {AssignmentId} not found");
                }

                // Update properties from the DTO
                AssignmentToUpdate.Title = updateDto.Title;
                AssignmentToUpdate.Date = updateDto.Date;
                AssignmentToUpdate.Duration = updateDto.Duration;



                _unitOfWork.Assignments.UpdateAsync(AssignmentToUpdate);


                return Ok(AssignmentToUpdate);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request");
            }




        }
        #endregion



        //#region DownloadAssignment
        //[HttpGet]
        //[Route("DownloadFile")]
        //public async Task<IActionResult> DownloadFile(string filename)
        //{
        //    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "UploadAssignment", filename);
        //    if (!System.IO.File.Exists(filepath))
        //    {
        //        return NotFound();
        //    }
        //    var provider = new FileExtensionContentTypeProvider();
        //    if (!provider.TryGetContentType(filepath, out var contenttype))
        //    {
        //        contenttype = "application/octet-stream";
        //    }

        //    var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
        //    return File(bytes, contenttype, Path.GetFileName(filepath));
        //}
        //#endregion


        #region UploadAssignment
        [HttpPost("UploadAssignment")]
        [Authorize(Roles = "Admin , Staff, Student")]
        public async Task<IActionResult> UploadAssignment([FromForm] AssignmentDTO assignmentDTO)
        {
            try
            {
                if (assignmentDTO == null || assignmentDTO.File == null)
                    return BadRequest("Assignment data or file not provided.");

                // Define the folder path where assignments will be stored
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "AssignmentUpload");

                // Call the file upload service to save the assignment file
                var filePath = await _unitOfWork.Assignments.UploadFileAsync(assignmentDTO.File, folderPath);

                // Map DTO to Entity
                var assignment = new Assignment
                {
                    Title = assignmentDTO.Title,
                    Date = assignmentDTO.Date,
                    Duration = assignmentDTO.Duration,
                    UserId = assignmentDTO.UserId,
                    GroupId = assignmentDTO.GroupId,
                    FilePath = filePath // Save the file path in the database

                };

                // Add assignment to database
                await _unitOfWork.Assignments.AddAsync(assignment);
               
                await _context.SaveChangesAsync();

                return Ok("Assignment uploaded successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        #endregion



       
      



    }
}
