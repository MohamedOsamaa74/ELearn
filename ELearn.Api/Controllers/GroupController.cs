﻿using ELearn.Application.DTOs;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupService _groupService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public GroupController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext context, IGroupService groupService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
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

        #region Delete
        [HttpDelete("Delete/{GroupId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGroup(int GroupId)
        {
            var response = await _groupService.DeleteAsync(GroupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetAll
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminGetAll()
        {
            /*await _unitOfWork.Groups.GetAllAsync()*/
            return Ok(await _unitOfWork.Groups.GetAllAsync(p => new { p.GroupName, p.Description, p.DepartmentId }));
        }
        #endregion

        #region GetUserGroups
        //Refactor
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
                return NoContent();
            }
            else return Ok(UserGroups);
        }
        #endregion

        //Update
    }
}
