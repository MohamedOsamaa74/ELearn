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
using Microsoft.EntityFrameworkCore;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService AnnouncementService)
        {
            _announcementService = AnnouncementService;
        }

        #region Get By Id
        [HttpGet("{Id:int}")]
        [Authorize]
        public async Task<IActionResult>GetById(int Id)
        {
            var response = await _announcementService.GetByIdAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get All
        [HttpGet("Get-All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _announcementService.GetAllAnnouncementsAsync();
            return this.CreateResponse(response);
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

            var responses = await _announcementService.GetFromGroupsAsync();

            return this.CreateResponse(responses);
        }
        #endregion

        #region Get By Creator
        [HttpGet("GetUserAnnouncement")]
        [Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult>GetUserAnnouncement()
        {
            var responses = await _announcementService.GetByCreatorAsync();
            return this.CreateResponse(responses);
        }
        #endregion
        
        #region Create New
        [HttpPost("CreateNew")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> Create([FromBody] AnnouncementDTO Model)
        {
            var response = await _announcementService.CreateNewAsync(Model);
            return this.CreateResponse(response);
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
            var response = await _announcementService.UpdateAsync(Model, AnnouncementId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete One
        [HttpDelete("Delete/{AnnouncementId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAnnouncement(int AnnouncementId)
        {
            var response = await _announcementService.DeleteAsync(AnnouncementId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Many

        [HttpDelete("DeleteMany")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMany([FromBody] List<int>Ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _announcementService.DeleteManyAsync(Ids);
            return this.CreateResponse(response);
        }
        #endregion
        
    }
}