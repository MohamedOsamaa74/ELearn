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
        #region Fields
        private readonly IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService AssignmentService)
        {
            _assignmentService = AssignmentService;
        }
        #endregion

        #region Create Assignment
        [HttpPost("Create")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateAssignment([FromForm] UploadAssignmentDTO Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _assignmentService.CreateAssignmentAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Submit Assignment Response
        [HttpPost("SubmitAssignmentResponse")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitAssignmentResponseAsync([FromForm]SubmitAssignmentResponseDTO submitAssignmentResponse)
        {
            var response = await _assignmentService.SubmitAssignmentResponseAsync(submitAssignmentResponse.AssignmentId, submitAssignmentResponse.file);
            return this.CreateResponse(response);
        }
        #endregion

        #region Give Grade To Student
        [HttpPost("GiveGradeToStudent/{ResponseId:int}")]
        [Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult>GiveGradeToStudentResponse(int ResponseId, int Mark)
        {
            var response = await _assignmentService.GiveGradeToStudentResponseAsync(ResponseId, Mark);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Assignment Responses
        [HttpGet("GetAssignmentResponses/{AssignmentId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAssignmentResponsesAsync(int AssignmentId, [FromQuery] string filter_by = null, [FromQuery] string sort_by = null)
        {
            var response = await _assignmentService.GetAssignmentResponsesAsync(AssignmentId, filter_by, sort_by);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Assignment
        [HttpDelete("Delete/{AssignmentId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteAssignment(int AssignmentId)
        {
            var response = await _assignmentService.DeleteAssignmentAsync(AssignmentId);
            return this.CreateResponse(response);
        }
        #endregion

        #region update Assignment
        [HttpPut("UpdateAssignment/{AssignmentId:int}")]
        [Authorize(Roles = "Admin, Staff")]

        public async Task<IActionResult> UpdateAssignment(int AssignmentId, [FromBody] UploadAssignmentDTO Model)
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
        [HttpGet("GetById/{assignmentId:int}")]
        [Authorize]
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
            var response = await _assignmentService.GetAssignmentsByCreatorAsync(sort_by, search_term);
            return this.CreateResponse(response);
        }
        #endregion
    }
}