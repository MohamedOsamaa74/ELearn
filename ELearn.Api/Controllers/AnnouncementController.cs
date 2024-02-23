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
using Microsoft.EntityFrameworkCore;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAnnouncementService announcementService;
        private readonly AppDbContext _context;
        public AnnouncementController(IUnitOfWork unitOfWork, AppDbContext context, IAnnouncementService AnnouncementService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            announcementService = AnnouncementService;
        }

        #region Get By Id
        [HttpGet("{Id:int}")]
        [Authorize]
        public async Task<IActionResult>GetById(int Id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(Id);
            if(announcement == null)
                return NotFound();
            try
            {
                return Ok(announcement.Text);
            }
            catch(Exception ex)
            {
                return BadRequest($"An Error Occured While Proccessing The Request, {ex.Message}");
            }

        }
        #endregion

        #region Get All
        [HttpGet("Get-All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _unitOfWork.Announcments.GetAllAsync(a => a.Text));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }
        #endregion

        #region Get Announcements from user groups
        [HttpGet("Get-All-From-Groups")]
            [Authorize]
            public async Task<IActionResult> GetAllFromGroups()
            {
                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("GetAll");
                }
                var currentUser = await _unitOfWork.Announcments.GetCurrentUserAsync(User);

                var announcements =await announcementService.GetFromGroups(currentUser.Id);

                if (announcements == null)
                {
                    return NotFound("No announcements found for the current user.");
                }

                return Ok(announcements);
            }
    #endregion

        #region Get By Creator
        [HttpGet("GetStaffAnnouncement{StaffId}")]
        [Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult>GetStaffAnnouncement(string StaffId)
        {
            if(await _unitOfWork.Users.GetByIdAsync(StaffId) == null)
            {
                return BadRequest("No Such User Exist");
            }
            var user = await _unitOfWork.Announcments.GetCurrentUserAsync(User);
            if (User.IsInRole("Staff") && StaffId != user.Id)
                return Unauthorized();

            return Ok(await _unitOfWork.Announcments.GetWhereSelectAsync
                      (a => a.UserId == StaffId, a => new { a.Text }));
        }
        #endregion
        
        #region Create
        [HttpPost("CreateNew")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> Create([FromBody] AnnouncementDTO Model)
        {
            try
            {
                var CurrentUser = await _unitOfWork.Announcments.GetCurrentUserAsync(User);
                var NewAnnouncement = await announcementService.CreateNew(CurrentUser.Id, Model.text);
                await _unitOfWork.Announcments.AddAsync(NewAnnouncement);

                var GroupAnnouncements = await announcementService.SendToGroups(Model.Groups, NewAnnouncement.Id);
                await _unitOfWork.GroupAnnouncments.AddRangeAsync(GroupAnnouncements);

                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }
        #endregion

        #region Delete One
        [HttpDelete("Delete/{AnnouncementId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAnnouncement(int AnnouncementId)
        {
            var announce = await _unitOfWork.Announcments.GetByIdAsync(AnnouncementId);
            if (announce == null)
            {
                return NotFound();
            }
            else
            {
                await _unitOfWork.Announcments.DeleteAsync(announce);
                return Ok("Announcement Deleted Successfully");
            }
        }
        #endregion
        
        #region Delete Many Need Refactor

        [HttpDelete("DeleteMany")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMany([FromBody] List<int>Ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var announcements = await announcementService.GetAnnouncements(Ids);
            await _unitOfWork.Announcments.DeleteRangeAsync(announcements);
            return Ok("The Selected Announcements Was Deleted Successfully");
        }
        #endregion
        
        #region Edit
        [HttpPut("EditAnnouncement/{AnnouncementId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] AnnouncementDTO Model, int AnnouncementId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var announcement = await _unitOfWork.Announcments.GetByIdAsync(AnnouncementId);
                announcement.Text = Model.text;
                foreach (var groupId in Model.Groups)
                {
                    var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
                    if (!await _unitOfWork.GroupAnnouncments.FindIfExistAsync(ga => ga.GroupId == groupId && ga.AnnouncementId == AnnouncementId))
                    {
                        await _unitOfWork.GroupAnnouncments.AddAsync(new GroupAnnouncment() { AnnouncementId = AnnouncementId, GroupId = groupId });
                    }
                }
                await _unitOfWork.Announcments.UpdateAsync(announcement);
                return Ok("Updated Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }
        #endregion
    }
}