using ELearn.Application.DTOs;
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
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserController(IUnitOfWork unitOfWork, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost("AddSignleUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSingleUser([FromBody] UserDTO Model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var NewUser = new ApplicationUser()
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    BirthDate = Model.BirthDate,
                    Address = Model.Address,
                    Nationality = Model.Nationality,
                    NId = Model.NId,
                    UserName = Model.UserName,
                    PhoneNumber = Model.PhoneNumber,
                    DepartmentId = Model.DepartmentId,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var result = await _userManager.CreateAsync(NewUser, NewUser.NId);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                        errors += error.Description;
                    return BadRequest(errors);
                }
                await _userManager.AddToRoleAsync(NewUser, UserRoles.Student);
                return Created();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
            
        }

        [HttpPost("AddMultipleUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AddMultipleUsers(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Invalid, Please Upload the File");
            }
            try
            {
                var NewUsers = await _userService.UploadCSV(file);
                if(NewUsers.IsNullOrEmpty())
                {
                    return BadRequest("No Users");
                }
                foreach(var user in NewUsers)
                {
                    var NewUser = new ApplicationUser()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                        Address = user.Address,
                        Nationality = user.Nationality,
                        NId = user.NId,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        DepartmentId = user.DepartmentId,
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };
                    await _userManager.CreateAsync(NewUser, NewUser.NId);
                    await _userManager.AddToRoleAsync(NewUser, UserRoles.Student);
                }
                return Created();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AdminGetAll()
        {
            return Ok(await _unitOfWork.Users.GetAllAsync(u => 
            new {u.FirstName, u.LastName, u.BirthDate, u.Address, u.DepartmentId, u.NId, u.PhoneNumber}
            ));
        }
    }
}
