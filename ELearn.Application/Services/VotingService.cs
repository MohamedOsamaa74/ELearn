using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class VotingService : IVotingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public VotingService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        #region Create
        public async Task<Response<VotingDTO>> CreateNewAsync(VotingDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var vote = _mapper.Map<Voting>(Model);
                vote.CreatorId = user.Id;
                await _unitOfWork.Votings.AddAsync(vote);
                if (Model.Options.IsNullOrEmpty() || Model.Options.Count()<2)
                    return ResponseHandler.BadRequest<VotingDTO>("You have to insert at least two options");
                foreach(var text in Model.Options)
                {
                    Option option = new Option() { Text = text, VotingId = vote.Id};
                    await _unitOfWork.Options.AddAsync(option);
                }
                await SendToGroupsAsync(Model.groups, vote.Id);
                return ResponseHandler.Success(Model);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<VotingDTO>($"An Error Occurred, {Ex}");
            }
        }

        #endregion

        #region GetById
        public async Task<Response<VotingDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var vote = await _unitOfWork.Votings.GetByIdAsync(Id);
                if(vote is null)
                    return ResponseHandler.NotFound<VotingDTO>("There is no such Voting");
                var voteDto = _mapper.Map<VotingDTO>(vote);
                voteDto.groups = await _unitOfWork.GroupVotings
                    .GetWhereSelectAsync(v => v.Id == Id, v => v.GroupId);
                voteDto.Options = await _unitOfWork.Options.GetWhereSelectAsync(opt => opt.VotingId == Id, opt => opt.Text);
                return ResponseHandler.Success(voteDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<VotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Delete One
        public async Task<Response<VotingDTO>> DeleteAsync(int Id)
        {
            var vote = await _unitOfWork.Votings.GetByIdAsync(Id);
            if (vote is null)
                return ResponseHandler.NotFound<VotingDTO>("There is no such Voting");
            try
            {
                var options = await _unitOfWork.Options.GetWhereAsync(opt => opt.VotingId == Id);
                foreach(var opt in options)
                {
                    await _unitOfWork.Options.DeleteAsync(opt);
                }
                await _unitOfWork.Votings.DeleteAsync(vote);
                return ResponseHandler.Deleted<VotingDTO>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<VotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        public async Task<Response<ICollection<VotingDTO>>> GetAllAsync()
        {
            try
            {
                var votes = await _unitOfWork.Votings.GetAllAsync();
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<VotingDTO>>("There are no Votings yet");
                
                ICollection<VotingDTO> votesDto = new List<VotingDTO>();
                foreach(var vote in votes)
                {
                    var votedto = _mapper.Map<VotingDTO>(vote);
                    votedto.Options = await GetVotingOptios(vote.Id);
                    votedto.groups = await GetVotingGroups(vote.Id);
                    votesDto.Add(votedto);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<VotingDTO>>($"An Error Occurred, {Ex}");
            }
        }

        #region Private Methods
        private async Task SendToGroupsAsync(ICollection<int> Groups, int VoteId)
        {
            foreach (var groupId in Groups)
            {
                GroupVoting NewGroupVotings = new GroupVoting()
                {
                    GroupId = groupId,
                    VotingId = VoteId
                };
                await _unitOfWork.GroupVotings.AddAsync(NewGroupVotings);
            }
        }
        
        private async Task<ICollection<string>>GetVotingOptios(int VotingId)
        {
            return await _unitOfWork.Options.GetWhereSelectAsync(opt => opt.VotingId == VotingId, opt => opt.Text);
        }
        
        private async Task<ICollection<int>> GetVotingGroups(int VotingId)
        {
            return await _unitOfWork.GroupVotings.GetWhereSelectAsync(g => g.VotingId == VotingId, g => g.GroupId);
        }
        #endregion
    }
}