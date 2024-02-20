using ELearn.Application.DTOs;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.UnitOfWork;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public AnnouncementController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
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

        #region Get Announcements for student
        // لسه
        [HttpGet("Get-All-From-Groups")]
            [Authorize(Roles = "Staff, Student")]
            public async Task<IActionResult> GetAllFromGroups()
            {
                var currentUserId = _userManager.GetUserId(User);

                var announcements = await _context.Announcements
                    .Include(a => a.GroupAnnouncements)
                    .Where(a => a.GroupAnnouncements.Any(ga =>
                        ga.GroupId == ga.Group.Id &&
                        ga.Group.UsersInGroup.Any(ug => ug.Id == currentUserId)))
                    .Select(a => a.Text)
                    .ToListAsync();

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
            if (User.IsInRole("Staff") && StaffId != _userManager.GetUserId(User))
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
                var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                Announcement NewAnnouncement = new Announcement()
                {
                    UserId = CurrentUser.Id,
                    Text = Model.text
                };
                await _unitOfWork.Announcments.AddAsync(NewAnnouncement);
                foreach (var groupId in Model.Groups)
                {
                    var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
                    GroupAnnouncment NewGroupAnnouncement = new GroupAnnouncment()
                    {
                        GroupId = groupId,
                        AnnouncementId = NewAnnouncement.Id
                    };
                    await _unitOfWork.GroupAnnouncments.AddAsync(NewGroupAnnouncement);
                }
                return Ok("Announcement Sent Succesfully");
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
                return BadRequest("There Is No Such Announcement");
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
            List<Announcement> announcements = new List<Announcement>();
            foreach(var Id in Ids)
            {
                var entity = await _unitOfWork.Announcments.GetByIdAsync(Id);
                announcements.Add(entity);
            }
            await _unitOfWork.Announcments.DeleteRangeAsync(announcements);
            return NoContent();
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
                    if(!await _unitOfWork.GroupAnnouncments.FindIfExistAsync(ga => ga.GroupId == groupId && ga.AnnouncementId == AnnouncementId))
                    {
                         await _unitOfWork.GroupAnnouncments.AddAsync(new GroupAnnouncment() { AnnouncementId = AnnouncementId, GroupId = groupId});
                    }
                }
                await _unitOfWork.Announcments.UpdateAsync(announcement);
                return Ok("Updated Successfully");

            }
            catch(Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }
        #endregion
    }
}
