using ELearn.Application.DTOs;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnnouncementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Announcments.GetAllAsync());
        }
        [HttpPost("CreateNew")]
        [Authorize(Roles = UserRoles.Admin + UserRoles.Staff)]
        public IActionResult CreateAnnouncement(AnnouncementDTO announcement)
        {
            return Ok();
        }
    }
}
