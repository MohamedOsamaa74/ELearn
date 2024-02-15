using ELearn.Application.DTOs;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public GroupController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("CreateNew")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> Create([FromBody] GroupDTO Group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                Group NewGroup = new Group()
                {
                    GroupName = Group.Name,
                    Description = Group.Description,
                    CreatorId = CurrentUser.Id,
                    DepartmentId = Group.DepartmentId,
                    //CreationDate = Group.CreateDate
                };
                await _unitOfWork.Groups.AddAsync(NewGroup);
                return Ok("Group Created Successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminGetAll()
        {
            /*await _unitOfWork.Groups.GetAllAsync()*/
            return Ok(_context.Groups.Select(p => new { p.GroupName , p.Description, p.DepartmentId}));
        }

        [HttpGet("GetUserGroups")]
        [Authorize(Roles ="Staff, Student")]
        public async Task<IActionResult> GetUserGroups()
        {
            var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var UserGroups = _context.UserGroups
                .Where(u => u.UserId == CurrentUser.Id)
                .Select(ug => new { ug.GroupId, ug.UserId }).ToList();
            if(UserGroups == null)
            {
                return BadRequest("You are not in Any Groups");
            }
            else return Ok(UserGroups);
        }

        [HttpDelete("Delete/{GroupId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGroup(int GroupId)
        {
            var Group = await _unitOfWork.Groups.GetByIdAsync(GroupId);
            if(Group == null)
            {
                return BadRequest("There Is No Such Group");
            }
            else
            {
                await _unitOfWork.Groups.DeleteAsync(Group);
                return Ok("Group Deleted Successfully");
            }
        }

        //Update
    }
}
