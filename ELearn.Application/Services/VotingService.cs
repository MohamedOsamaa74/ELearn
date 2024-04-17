using AutoMapper;
using ELearn.Application.DTOs.OptionDTOs;
using ELearn.Application.DTOs.VotingDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;

namespace ELearn.Application.Services
{
    public class VotingService : IVotingService
    {
        #region Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public VotingService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }
        #endregion

        #region Create
        public async Task<Response<AddVotingDTO>> CreateNewAsync(AddVotingDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var vote = _mapper.Map<Voting>(Model);
                vote.CreatorId = user.Id;
                if (Model.Options.IsNullOrEmpty() || Model.Options.Count()<2)
                    return ResponseHandler.BadRequest<AddVotingDTO>("You have to insert at least two options");
                await _unitOfWork.Votings.AddAsync(vote);
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
                return ResponseHandler.BadRequest<AddVotingDTO>($"An Error Occurred, {Ex}");
            }
        }

        #endregion

        #region GetById
        public async Task<Response<AddVotingDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var vote = await _unitOfWork.Votings.GetByIdAsync(Id);
                if(vote is null)
                    return ResponseHandler.NotFound<AddVotingDTO>("There is no such Voting");
                var voteDto = _mapper.Map<AddVotingDTO>(vote);
                voteDto.groups = await _unitOfWork.GroupVotings
                    .GetWhereSelectAsync(v => v.Id == Id, v => v.GroupId);
                voteDto.Options = await _unitOfWork.Options.GetWhereSelectAsync(opt => opt.VotingId == Id, opt => opt.Text);
                return ResponseHandler.Success(voteDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<AddVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetByDate
        public async Task<Response<ICollection<AddVotingDTO>>> GetVotesByDate(DateTime date)
        {
            if (date == null)
            {
                return ResponseHandler.BadRequest<ICollection<AddVotingDTO>>("Invalid date");
            }
            try
            {
                var votes = await _unitOfWork.Votings.GetWhereSelectAsync(v => v.Start <= date && v.End >= date,v=>v.Id);
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<AddVotingDTO>>("There are no Votings ");
                ICollection<AddVotingDTO>  votesDto = new List<AddVotingDTO>();
                foreach (var vote in votes)
                {
                    var votedto = await GetByIdAsync(vote);
                    votesDto.Add(votedto.Data);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }

        #endregion

        #region GetByCreator
        public async Task<Response<ICollection<AddVotingDTO>>> GetVotesByCreator(string UserId)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if(UserId is null) UserId = user.Id;

                var votes = await _unitOfWork.Votings
                    .GetWhereSelectAsync(v => v.CreatorId == UserId, v => v.Id);

                if (votes is null)
                    return ResponseHandler.NotFound<ICollection<AddVotingDTO>>
                        ("There are No Votings yet");

                ICollection<AddVotingDTO> votesDto = new List<AddVotingDTO>();
                foreach (var vote in votes)
                {
                    var votedto = await GetByIdAsync(vote);
                    votesDto.Add(votedto.Data);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddVotingDTO>>($"An Error Occurred, {Ex}");
            }
            
            
        }

        #endregion

        #region GetFromGroups
        public async Task<Response<ICollection<AddVotingDTO>>> GetFromGroups(int GroupId)
        {
            try
            {
                var votes = await _unitOfWork.GroupVotings.
                    GetWhereSelectAsync(v => v.GroupId == GroupId, v => v.VotingId);
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<AddVotingDTO>>("There are no Votings yet");
                ICollection<AddVotingDTO> votesDto = new List<AddVotingDTO>();
                foreach(var vote in votes)
                {
                    var votedto = await GetByIdAsync(vote);
                    votesDto.Add(votedto.Data);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Response<ICollection<AddVotingDTO>>> GetAllAsync()
        {
            try
            {
                var votes = await _unitOfWork.Votings.GetAllAsync();
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<AddVotingDTO>>("There are no Votings yet");
                
                ICollection<AddVotingDTO> votesDto = new List<AddVotingDTO>();
                foreach(var vote in votes)
                {
                    var votedto = _mapper.Map<AddVotingDTO>(vote);
                    votedto.Options = await GetVotingOptios(vote.Id);
                    votedto.groups = await GetVotingGroups(vote.Id);
                    votesDto.Add(votedto);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Delete One
        public async Task<Response<AddVotingDTO>> DeleteAsync(int Id)
        {
            var vote = await _unitOfWork.Votings.GetByIdAsync(Id);
            if (vote is null)
                return ResponseHandler.NotFound<AddVotingDTO>("There is no such Voting");
            try
            {
                var options = await _unitOfWork.Options.GetWhereAsync(opt => opt.VotingId == Id);
                foreach(var opt in options)
                {
                    await _unitOfWork.Options.DeleteAsync(opt);
                }
                await _unitOfWork.Votings.DeleteAsync(vote);
                return ResponseHandler.Deleted<AddVotingDTO>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<AddVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Delete Many
        public async Task<Response<AddVotingDTO>>DeleteManyAsync(ICollection<int>Id)
        {
            try
            {
                foreach(var id in Id)
                {
                    var vote = await _unitOfWork.Votings.GetByIdAsync(id);
                    if (vote is null)
                        return ResponseHandler.NotFound<AddVotingDTO>("There is no Votings");
                    var options = await _unitOfWork.Options
                        .GetWhereAsync(opt => opt.VotingId == id);
                    foreach (var opt in options)
                    {
                        await _unitOfWork.Options.DeleteAsync(opt);
                    }
                    await _unitOfWork.Votings.DeleteAsync(vote);
                }
                return ResponseHandler.Deleted<AddVotingDTO>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<AddVotingDTO>($"An Error Occured, {Ex}");
            }
        }
        #endregion

        #region Update
        public async Task<Response<AddVotingDTO>> UpdateAsync(int Id, AddVotingDTO Model)
        {
            try
            {
                var oldvote = await _unitOfWork.Votings.GetByIdAsync(Id);
                var newVote = _mapper.Map<Voting>(Model);
                await _unitOfWork.Votings.UpdateAsync(newVote);
                //Edit Options
                //Edit Groups
                return ResponseHandler.Updated(Model);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<AddVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region RecieveStudentResponse
        public async Task<Response<UserVotingDTO>> RecieveStudentResponse(int VotingId, int OptionId)
        {
            try
            {
                var vote = await _unitOfWork.Votings.GetByIdAsync(VotingId);
                var option = await _unitOfWork.Options.GetByIdAsync(OptionId);
                if (vote is null)
                    return ResponseHandler.NotFound<UserVotingDTO>("There is no such Voting");
                if (option is null)
                    return ResponseHandler.NotFound<UserVotingDTO>("There is no such Option");
                var user = await _userService.GetCurrentUserAsync();
                //missing validation
                var uservotes = await _unitOfWork.UserVotings
                    .GetWhereAsync(v => v.VotingId == VotingId && v.userId == user.Id);
                var uservote = uservotes.SingleOrDefault();
                if(uservote is not null)
                {
                    uservote.OptionsId = OptionId;
                    await _unitOfWork.UserVotings.UpdateAsync(uservote);
                    var userVotingDto = new UserVotingDTO()
                    {
                        UserName = user.UserName,
                        Voting = vote.Text,
                        Option = option.Text
                    };
                    return ResponseHandler.Updated(userVotingDto);
                }
                var response = new UserVoting()
                {
                    userId = user.Id,
                    VotingId = VotingId,
                    OptionsId = OptionId
                };
                await _unitOfWork.UserVotings.AddAsync(response);
                var dto = new UserVotingDTO()
                {
                    UserName = user.UserName,
                    Voting = (await _unitOfWork.Votings.GetByIdAsync(VotingId)).Text,
                    Option = (await _unitOfWork.Options.GetByIdAsync(OptionId)).Text
                };
                return ResponseHandler.Created(dto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<UserVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetVotingResponses
        public async Task<Response<ICollection<UserVotingDTO>>> GetVotingResponses(int VotingId)
        {
            try
            {
                var votes = await _unitOfWork.UserVotings.GetWhereAsync(v => v.VotingId == VotingId);
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<UserVotingDTO>>("There are no Responses yet");
                ICollection<UserVotingDTO> votesDto = new List<UserVotingDTO>();
                foreach(var vote in votes)
                {
                    var user = await _userService.GetCurrentUserAsync();
                    var option = await _unitOfWork.Options.GetByIdAsync(vote.OptionsId);
                    var voteDto = new UserVotingDTO()
                    {
                        UserName = user.UserName,
                        Voting = (await _unitOfWork.Votings.GetByIdAsync(VotingId)).Text,
                        Option = option.Text
                    };
                    votesDto.Add(voteDto);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<UserVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region EditOption

        public async Task<Response<OptionDTO>> EditOption(int Id, OptionDTO Model)
        {
            try
            {
                var oldoption = await _unitOfWork.Options.GetByIdAsync(Id);
                var newoption = _mapper.Map<Option>(Model);
                await _unitOfWork.Options.UpdateAsync(newoption);
                return ResponseHandler.Updated(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<OptionDTO>($"An Error Occurred, {Ex}");
            }
        }

        #endregion

        #region DeleteOption

        public async Task<Response<OptionDTO>> DeleteOptionAsync(int Id)
        {
            var opt = await _unitOfWork.Options.GetByIdAsync(Id);
            if (opt is null)
                return ResponseHandler.NotFound<OptionDTO>("There is no Option ");
            try
            {
                await _unitOfWork.Options.DeleteAsync(opt);
                return ResponseHandler.Deleted<OptionDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<OptionDTO>($"An Error Occurred, {Ex}");
            }
        }

        #endregion

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