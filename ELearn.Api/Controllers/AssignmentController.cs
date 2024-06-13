using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Application.Services;
using ELearn.Application.DTOs.AssignmentDTOs;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService AssignmentService)
        {
            _assignmentService = AssignmentService;
        }


        #region Delete Assignment
        [HttpDelete("Delete/{AssignmentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAssignment(int AssignmentId)
        {
            var response = await _assignmentService.DeleteAssignmentAsync(AssignmentId);
            return this.CreateResponse(response);
        }
        #endregion

        #region update Assignment
        [HttpPut("UpdateAssignment/{AssignmentId:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateAssignment(int AssignmentId, [FromBody] AssignmentDTO Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _assignmentService.UpdateAssignmentAsync(AssignmentId, Model);
            return this.CreateResponse(response);
        }


        #endregion

        #region GetAll Assignments
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] string sort_by = null, [FromQuery] string search_term = null)
        {
            var response = await _assignmentService.GetAllAssignmentsAsync(sort_by, search_term);

            return this.CreateResponse(response);
        }
        #endregion

        #region Get Assignment By ID
        [HttpGet("{assignmentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAssignmentById(int assignmentId)
        {
            var response = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get assignment By GroupID
        [HttpGet("GetByGroupId/{GroupId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByGroupId(int GroupId)
        {
            var response = await _assignmentService.GetFromGroupAsync(GroupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete All Assignments
        [HttpDelete("DeleteMany")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMany([FromBody] List<int> Ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _assignmentService.DeleteManyAsync(Ids);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetAssignmentsOfCurrentUser
        [HttpGet("GetAssignmentsByCreator")]
        public async Task<IActionResult> GetAssignmentsByCreator([FromQuery] string sort_by = null, [FromQuery] string search_term = null)
        {
            var response = await _assignmentService.GetAssignmentsByCreator(sort_by, search_term);
            return this.CreateResponse(response);
        }
        #endregion
    }
}