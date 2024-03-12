using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly IVotingService _votingService; 
        public VotingController(IVotingService votingService)
        {
            _votingService = votingService;
        }

        #region Create New
        [HttpPost("CreateVoting")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult>Create([FromBody] VotingDTO Model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _votingService.CreateNewAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get By Id
        [HttpGet("GetVoting/{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult>GetById(int Id)
        {
            var response = await _votingService.GetByIdAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetAll
        [HttpGet("GetAllVotings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>GetAll()
        {
            var response = await _votingService.GetAllAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete One
        [HttpDelete("DeleteVoting/{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult>Delete(int Id)
        {
            var response = await _votingService.DeleteAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

    }
}