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

        [HttpGet("Get-All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _unitOfWork.Announcments.GetAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" an Error occurred while processing the request {ex.Message}");
            }
        }

        // لسه
        [HttpGet("Get-All-From-Groups")]
        [Authorize(Roles = "Staff, Student")]
        public async Task<IActionResult> GetAllFromGroups()
        {
            var currentUserId = _userManager.GetUserId(User); // Get current user's ID

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

        [HttpPost("CreateNew")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> Create([FromBody] AnnouncementDTO announcement)
        {
            try
            {
                var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                Announcement NewAnnouncement = new Announcement()
                {
                    UserId = CurrentUser.Id,
                    Text = announcement.text
                };
                await _unitOfWork.Announcments.AddAsync(NewAnnouncement);
                foreach (var groupId in announcement.Groups)
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
    }
}
