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
using Microsoft.IdentityModel.Tokens;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public ApplicationUserController(IUnitOfWork unitOfWork, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userManager = userManager;
        }
        #region Add Single User
        [HttpPost("AddSignleUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSingleUser([FromBody] UserDTO Model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response =  await _userService.CreateNewUserAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Add Multiple Users From CSV (Need Refactor)
        [HttpPost("AddMultipleUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AddMultipleUsers(IFormFile file)
        {
            var responses = await _userService.AddMultipleUsersAsync(file);
            return this.CreateResponse((Response<IEnumerable<Response<UserDTO>>>)responses);
        }
        #endregion

        #region Get All (Need Refactor)
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AdminGetAll()
        {
            return Ok(await _unitOfWork.Users.GetAllAsync(u => 
            new {u.FirstName, u.LastName, u.BirthDate, u.Address, u.DepartmentId, u.NId, u.PhoneNumber}
            ));
        }
        #endregion
    }
}
