using ELearn.Application.DTOs.UserDTOs;
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
using Microsoft.IdentityModel.Tokens;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IUserService _userService;
        public ApplicationUserController(IUserService userService)
        {
            _userService = userService;
        }

        #region Add Single User
        [HttpPost("AddSignleUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSingleUser([FromBody] AddUserDTO Model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var response =  await _userService.CreateNewUserAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Add Multiple Users From CSV
        [HttpPost("AddMultipleUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AddMultipleUsers(IFormFile file)
        {
            var responses = await _userService.AddMultipleUsersAsync(file);
            return this.CreateResponse(responses);
        }
        #endregion

        #region Get All
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AdminGetAll()
        {
            var responses = await _userService.GetAllAsync();
            return this.CreateResponse(responses);
        }
        #endregion

        #region Delete One
        [HttpDelete("DeleteUser{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteUser(string Id)
        {
            var response = await _userService.DeleteUserAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Many
        [HttpDelete("DeleteManyUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteMany(List<string>Ids)
        {
            var response = await _userService.DeleteManyAsync(Ids);
            return this.CreateResponse(response);
        }
        #endregion

        #region Edit
        [HttpPut("EditUser {Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string Id, EditUserDTO Model)
        {
            var response = await _userService.EditUserAsync(Id, Model);
            return this.CreateResponse(response);
        }
        #endregion
    }
}