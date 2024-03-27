using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        #region Create
        [HttpPost("CreateNew")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> Create([FromBody] GroupDTO Group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _groupService.CreateAsync(Group);
            return this.CreateResponse(response);
        }
        #endregion


        #region GetById
        [HttpGet("GetById{Id:int}")]
        public async Task<IActionResult>GetById(int Id)
        {
            var response = await _groupService.GetByIdAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetByName
        [HttpGet("GetByName{Name}")]
        public async Task<IActionResult> GetByName(string Name)
        {
            var response = await _groupService.GetByNameAsync(Name);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetAll
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminGetAll()
        {
            var response = await _groupService.GetAllAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region GetUserGroups
        [HttpGet("GetUserGroups")]
        [Authorize(Roles ="Staff, Student")]
        public async Task<IActionResult> GetUserGroups([FromRoute]string UserId = null)
        {
            var response = await _groupService.GetUserGroupsAsync(UserId);
            return this.CreateResponse(response);

        }
        #endregion

        #region Delete
        [HttpDelete("Delete/{GroupId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGroup(int GroupId)
        {
            var response = await _groupService.DeleteAsync(GroupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Many
        [HttpDelete("DeleteManyGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteMany(ICollection<int> Ids)
        {
            var response = await _groupService.DeleteManyAsync(Ids);
            return this.CreateResponse(response);
        }
        #endregion

        #region Update
        [HttpPut("EditGroup/{Id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditGroup([FromBody]GroupDTO Model, int Id)
        {
            var response = await _groupService.UpdateAsync(Model, Id);
            return this.CreateResponse(response);
        }
        #endregion

    }
}
