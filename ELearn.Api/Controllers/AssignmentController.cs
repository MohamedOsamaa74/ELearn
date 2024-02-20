using ELearn.Application.DTOs;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.UnitOfWork;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}
