﻿using Azure;
using ELearn.Application.DTOs.VotingDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
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
        #region Fields & Constructor
        private readonly IVotingService _votingService; 
        public VotingController(IVotingService votingService)
        {
            _votingService = votingService;
        }
        #endregion

        #region Create New
        [HttpPost("CreateVoting")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult>Create([FromBody] AddVotingDTO Model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _votingService.CreateNewAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Recieve Student Response
        [HttpPost("SubmitResponse/{VoteId:int}")]
        [Authorize]
        public async Task<IActionResult> SubmitResponse(int VoteId,[FromBody] string Option)
        {
            var response = await _votingService.SubmitResponseAsync(VoteId, Option);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Voting Responses
        [HttpGet("GetVotingResponses/{VoteId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetVotingResponses(int VoteId)
        {
            var response = await _votingService.GetVotingResponses(VoteId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get By Id
        [HttpGet("GetVoting/{Id:int}")]
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

        #region GetFromGroup
        [HttpGet("GetFromGroup/{GroupId:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult>GetFromGroups(int GroupId)
        {
            var response = await _votingService.GetFromGroup(GroupId);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetUserGroupsVotes
        [HttpGet("GetUserGroupsVotes")]
        [Authorize(Roles = "Staff, Student,Admin")]
        public async Task<IActionResult> GetUserGroupsVotes()
        {
            var response = await _votingService.GetUserGroupsVotes();
            return this.CreateResponse(response);
        }
        #endregion

        #region GetActive
        [HttpGet("GetActiveVotes")]
        public async Task<IActionResult> GetActiveVotes()
        {
            var response = await _votingService.GetVotesByDate(DateTime.UtcNow);
            return this.CreateResponse(response);
        }
        #endregion
        
        #region GetByDate
        [HttpGet("GetVotesByDate/{date}")]
        public async Task<IActionResult> GetVotesByDate(DateTime date)
        {
            var response = await _votingService.GetVotesByDate(date);
            return this.CreateResponse(response);
        }
        #endregion

        #region Update
        [HttpPut("UpdateVoting/{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult>Update(int Id, [FromBody] AddVotingDTO Model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _votingService.UpdateAsync(Id, Model);
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

        #region DeleteMany
        [HttpDelete("DeleteMany")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMany([FromBody] ICollection<int> Id)
        {
            var response = await _votingService.DeleteManyAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

    }
}