using ELearn.Application.DTOs.AnnouncementDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
        public async Task<IActionResult> GetAll([FromQuery] string sort_by = null, [FromQuery] string search_term = null)
        {
            var response = await _announcementService.GetAllAnnouncementsAsync(sort_by, search_term);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Announcements from user groups
        [HttpGet("Get-All-From-Groups")]
        [Authorize]
        public async Task<IActionResult> GetAllFromGroups([FromQuery] string sort_by = null, [FromQuery] string search_term = null)
        {
            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("GetAll");
            }
            var responses = await _announcementService.GetFromUserGroupsAsync(sort_by, search_term);

            return this.CreateResponse(responses);
        }
        #endregion

        #region Get By Creator
        [HttpGet("GetUserAnnouncement")]
        [Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult>GetUserAnnouncement([FromQuery] string sort_by = null, [FromQuery] string search_term = null)
        {
            var responses = await _announcementService.GetByCreatorAsync(sort_by, search_term);
            return this.CreateResponse(responses);
        }
        #endregion
        
        #region Create New
        [HttpPost("CreateNew")]
        [Authorize(Roles = "Admin ,Staff")]
        public async Task<IActionResult> Create([FromForm] UploadAnnouncementDTO Model)
        {
            var response = await _announcementService.CreateNewAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Edit
        [HttpPut("EditAnnouncement/{AnnouncementId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] UploadAnnouncementDTO Model, int AnnouncementId)
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
        [Authorize(Roles = "Admin, Staff")]
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