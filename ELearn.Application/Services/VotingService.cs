using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.DTOs.OptionDTOs;
using ELearn.Application.DTOs.VotingDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using Microsoft.IdentityModel.Tokens;
using System.Data;

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

        #region Create New
        public async Task<Response<ViewVotingDTO>> CreateNewAsync(AddVotingDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var vote = _mapper.Map<Voting>(Model);
                vote.CreatorId = user.Id;
                if (Model.Options.IsNullOrEmpty() || Model.Options.Count() < 2 || Model.Options.Count() > 5)
                    return ResponseHandler.BadRequest<ViewVotingDTO>("You have to insert at least two options and at most five options");


                var addOptions = AddOptions(vote, Model.Options);
                if (addOptions != "Success")
                    return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {addOptions}");

                var validation = new VotingValidation().Validate(vote);
                if (!validation.IsValid)
                    return ResponseHandler.BadRequest<ViewVotingDTO>(null, validation.Errors.Select(x => x.ErrorMessage).ToList());


                if (vote.Title.IsNullOrEmpty())
                    return ResponseHandler.BadRequest<ViewVotingDTO>("Title Is Required");

                if (vote.Description.IsNullOrEmpty())
                    return ResponseHandler.BadRequest<ViewVotingDTO>("Description Is Required");
                await _unitOfWork.Votings.AddAsync(vote);
                var sendVote = await SendToGroupsAsync(vote.Id, Model.groups);
                if (sendVote != "Success")
                    return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {sendVote}");

                var viewVote = await GetByIdAsync(vote.Id);
                return ResponseHandler.Created(viewVote.Data);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetById
        public async Task<Response<ViewVotingDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var vote = await _unitOfWork.Votings.GetByIdAsync(Id);
                if (vote is null)
                    return ResponseHandler.NotFound<ViewVotingDTO>("There is no such Voting");

                var user = await _unitOfWork.Users.GetByIdAsync(vote.CreatorId);
                var currentUser = await _userService.GetCurrentUserAsync();
                var viewVote = _mapper.Map<ViewVotingDTO>(vote);
                viewVote.Groups = await _unitOfWork.GroupVotings
                    .GetWhereSelectAsync(v => v.Id == Id, v => v.GroupId);
                viewVote.CreatorName = user.FirstName + " " + user.LastName;
                viewVote.OptionPercentages = await GetVotingOptionsPercentageAsync(Id);
                var userId = currentUser.Id;
                var userVote = await _unitOfWork.UserAnswerVotings.GetWhereSingleAsync(v => v.VotingId == Id && v.UserId == currentUser.Id);
                viewVote.HasVoted = userVote is null ? false : true;
                viewVote.UserVote = userVote?.Option;
                return ResponseHandler.Success(viewVote);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Delete
        public async Task<Response<ViewVotingDTO>> DeleteAsync(int Id)
        {
            var user = await _userService.GetCurrentUserAsync();
            var vote = await _unitOfWork.Votings.GetByIdAsync(Id);
            var role = await _userService.GetUserRoleAsync();
            if (vote is null)
                return ResponseHandler.NotFound<ViewVotingDTO>("There is no such Voting");
            if (vote.CreatorId != user.Id && role != "Admin")
                return ResponseHandler.Unauthorized<ViewVotingDTO>("You are not authorized to do this action");
            try
            {
                await _unitOfWork.Votings.DeleteAsync(vote);
                return ResponseHandler.Deleted<ViewVotingDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetFromGroup
        public async Task<Response<ICollection<ViewVotingDTO>>> GetFromGroup(int GroupId)
        {
            try
            {
                if (await _unitOfWork.Groups.GetByIdAsync(GroupId) == null)
                    return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>("Invalid Group Id");

                var votes = await _unitOfWork.GroupVotings.GetWhereSelectAsync(v => v.GroupId == GroupId, v => v.VotingId);
                if (votes is null)
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There are no Votings yet");

                ICollection<ViewVotingDTO> votesDto = new List<ViewVotingDTO>();
                foreach (var vote in votes)
                {
                    var votedto = await GetByIdAsync(vote);
                    votesDto.Add(votedto.Data);
                }
                ICollection<ViewVotingDTO> activeVotesDto = new List<ViewVotingDTO>();
                activeVotesDto = GetActiveVotes(votesDto).Data;
                if (activeVotesDto.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There are no Active Votings");

                return ResponseHandler.Success(activeVotesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetUserGroupsVotes
        public async Task<Response<ICollection<ViewVotingDTO>>> GetUserGroupsVotes()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There is no such User");
                var groups = await _unitOfWork.UserGroups.GetWhereSelectAsync(g => g.UserId == user.Id, g => g.GroupId);
                if (groups.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There are no Votings yet");
                ICollection<ViewVotingDTO> votesDto = [];
                foreach (var group in groups)
                {
                    var votes = await GetFromGroup(group);
                    if (votes.Data != null)
                    {
                        foreach (var vote in votes.Data)
                        {
                            if (!votesDto.Any(v => v.Id == vote.Id))
                                votesDto.Add(vote);
                        }
                    }
                }
                return ResponseHandler.Success(votesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetByDate
        public async Task<Response<ICollection<ViewVotingDTO>>> GetVotesByDate(DateTime date)
        {
            try
            {
                if (date == null)
                    return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>("Invalid date");
                var votes = await _unitOfWork.Votings.GetWhereSelectAsync(v => v.CreationDate <= date && v.End >= date, v => v.Id);
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There are no Votings ");
                ICollection<ViewVotingDTO> votesDto = new List<ViewVotingDTO>();
                foreach (var vote in votes)
                {
                    var votedto = await GetByIdAsync(vote);
                    votesDto.Add(votedto.Data);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetVotingResponses
        public async Task<Response<ICollection<UserVotingDTO>>> GetVotingResponses(int VotingId)
        {
            try
            {
                var vote = await _unitOfWork.Votings.GetByIdAsync(VotingId);
                if (vote is null)
                    return ResponseHandler.NotFound<ICollection<UserVotingDTO>>("There is no such Voting");

                var userVotes = await _unitOfWork.UserAnswerVotings.GetWhereAsync(v => v.VotingId == VotingId);

                if (userVotes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<UserVotingDTO>>("There are no Responses yet");

                ICollection<UserVotingDTO> userVotesDto = [];
                foreach (var userVote in userVotes)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(userVote.UserId);
                    var userVoteDto = new UserVotingDTO()
                    {
                        FullName = user.FirstName + ' ' + user.LastName,
                        Voting = vote.Description,
                        Option = userVote.Option,
                        Created = userVote.CreationDate
                    };
                    userVotesDto.Add(userVoteDto);
                }
                return ResponseHandler.Success(userVotesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<UserVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region RecieveStudentResponse
        public async Task<Response<UserVotingDTO>> SubmitResponseAsync(int VotingId, string Option)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if(user is null)
                    return ResponseHandler.Forbidden<UserVotingDTO>();
                var vote = await _unitOfWork.Votings.GetByIdAsync(VotingId);
                if (vote is null)
                    return ResponseHandler.NotFound<UserVotingDTO>("There is no such Voting");
                if (!CheckOption(vote, Option))
                    return ResponseHandler.NotFound<UserVotingDTO>("There is no such Option");
                var userVote = new UserAnswerVoting()
                {
                    UserId = user.Id,
                    VotingId = VotingId,
                    Option = Option
                };
                await _unitOfWork.UserAnswerVotings.AddAsync(userVote);
                var userVoteDto = new UserVotingDTO()
                {
                    FullName = user.FirstName + ' ' + user.LastName,
                    Voting = vote.Description,
                    Option = Option,
                    Created = DateTime.UtcNow
                };
                return ResponseHandler.Created(userVoteDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UserVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Update
        public async Task<Response<ViewVotingDTO>> UpdateAsync(int Id, AddVotingDTO Model)
        {
            try
            {
                var originalVote = await _unitOfWork.Votings.GetByIdAsync(Id);
                var user = await _userService.GetCurrentUserAsync();
                var role = await _userService.GetUserRoleAsync();
                if (user == null)
                    return ResponseHandler.NotFound<ViewVotingDTO>("There is no such User");
                if (originalVote.CreatorId != user.Id && role != "Admin")
                    return ResponseHandler.Unauthorized<ViewVotingDTO>("You are not authorized to do this action");

                if (originalVote is null)
                    return ResponseHandler.NotFound<ViewVotingDTO>("There is no such Voting");
                originalVote.Description = Model.Title;
                originalVote.End = Model.End;
                var editOptions = AddOptions(originalVote, Model.Options);
                if (editOptions != "Success")
                    return ResponseHandler.BadRequest<ViewVotingDTO>(editOptions);
                var sendToGroups = await SendToGroupsAsync(Id, Model.groups);
                if (sendToGroups != "Success")
                    return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occured,{sendToGroups}");

                var validation = new VotingValidation().Validate(originalVote);
                if (!validation.IsValid)
                    return ResponseHandler.BadRequest<ViewVotingDTO>(null, validation.Errors.Select(x => x.ErrorMessage).ToList());

                await _unitOfWork.Votings.UpdateAsync(originalVote);
                var viewVote = _mapper.Map<ViewVotingDTO>(originalVote);
                viewVote.OptionPercentages = await GetVotingOptionsPercentageAsync(Id);
                return ResponseHandler.Success(viewVote);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region DeleteMany
        public async Task<Response<ViewVotingDTO>> DeleteManyAsync(ICollection<int> Id)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var role = await _userService.GetUserRoleAsync();

                foreach (var id in Id)
                {
                    var vote = await _unitOfWork.Votings.GetByIdAsync(id);
                    if (vote is null)
                        return ResponseHandler.NotFound<ViewVotingDTO>("There is no such Voting");
                    if (vote.CreatorId != user.Id && role != "Admin")
                        return ResponseHandler.Unauthorized<ViewVotingDTO>("You are not authorized to do this action");
                    if (vote is null)
                        return ResponseHandler.NotFound<ViewVotingDTO>("There is no such Voting");
                    await _unitOfWork.Votings.DeleteAsync(vote);
                }
                return ResponseHandler.Deleted<ViewVotingDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewVotingDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Response<ICollection<ViewVotingDTO>>> GetAllAsync()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var role = await _userService.GetUserRoleAsync();
                if (role != "Admin")
                    return ResponseHandler.Unauthorized<ICollection<ViewVotingDTO>>("Unauthorized");
                var votes = await _unitOfWork.Votings.GetAllAsync();
                if (votes.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There are no Votings yet");

                ICollection<ViewVotingDTO> votesDto = new List<ViewVotingDTO>();
                foreach (var vote in votes)
                {
                    var votedto = _mapper.Map<ViewVotingDTO>(vote);
                    votedto.Groups = await _unitOfWork.GroupVotings
                        .GetWhereSelectAsync(v => v.VotingId == vote.Id, v => v.GroupId);
                    votesDto.Add(votedto);
                }
                return ResponseHandler.Success(votesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        public async Task<Response<OptionDTO>> DeleteOptionAsync(int VotingId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<OptionDTO>> EditOption(int Id, OptionDTO Model)
        {
            throw new NotImplementedException();
        }


        #region Private Methods

        #region SendToGroups
        private async Task<string> SendToGroupsAsync(int votingId, ICollection<int> GroupIds)
        {
            try
            {
                foreach (var groupId in GroupIds)
                {
                    if (await _unitOfWork.Groups.GetByIdAsync(groupId) == null)
                        return "Invalid Group Id";

                    var groupVoting = await _unitOfWork.GroupVotings
                        .GetWhereAsync(g => g.GroupId == groupId && g.VotingId == votingId);

                    if (!groupVoting.IsNullOrEmpty())
                        continue;
                    GroupVoting NewGroupVotings = new GroupVoting()
                    {
                        GroupId = groupId,
                        VotingId = votingId
                    };
                    await _unitOfWork.GroupVotings.AddAsync(NewGroupVotings);
                }
                return "Success";
            }
            catch (Exception Ex)
            {
                return $"{Ex}";
            }
        }
        #endregion

        #region AddOptions
        private static string AddOptions(Voting vote, ICollection<string> Options)
        {
            try
            {
                vote.Option1 = Options.ElementAt(0);
                vote.Option2 = Options.ElementAt(1);
                if (Options.Count() > 2)
                    vote.Option3 = Options.ElementAt(2);
                if (Options.Count() > 3)
                    vote.Option4 = Options.ElementAt(3);
                if (Options.Count() > 4)
                    vote.Option5 = Options.ElementAt(4);
                return "Success";
            }
            catch (Exception Ex)
            {
                return $"An Error Occurred, {Ex}";
            }
        }
        #endregion

        #region CheckOption
        private bool CheckOption(Voting vote, string Option)
        {
            if (Option is null)
                return false;
                if (Option == vote.Option1 || Option == vote.Option2) return true;
                if (vote.Option3 != null && Option == vote.Option3) return true;
                if (vote.Option4 != null && Option == vote.Option4) return true;
                if (vote.Option5 != null && Option == vote.Option5) return true;

            /*if(vote.Option3 != null && Option!=vote.Option3)
                return false;
            if (vote.Option4 != null && Option != vote.Option4)
                return false;
            if (vote.Option5 != null && Option != vote.Option5)
                return false;*/

            return false;
        }
        #endregion

        #region GetActiveVotes
        public Response<ICollection<ViewVotingDTO>> GetActiveVotes(ICollection<ViewVotingDTO> votes)
        {
            try
            {
                ICollection<ViewVotingDTO> votesDto = new List<ViewVotingDTO>();
                foreach (var vote in votes)
                {
                    if (vote.Start <= DateTime.UtcNow.ToLocalTime() && vote.End >= DateTime.UtcNow.ToLocalTime())
                        votesDto.Add(vote);
                }
                if (votesDto.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<ViewVotingDTO>>("There are no Active Votings");
                return ResponseHandler.Success(votesDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewVotingDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetVotingOptionsPercentage
        private async Task<ICollection<OptionPercentageDTO>> GetVotingOptionsPercentageAsync(int VoteId)
        {
            var vote = await _unitOfWork.Votings.GetByIdAsync(VoteId);
            if (vote is null)
                return null;

            ICollection<OptionPercentageDTO> options = [];
            
            options.Add(new OptionPercentageDTO()
            {
                Text = vote.Option1,
                Percentage = await _unitOfWork.UserAnswerVotings
                    .CountAsync(v => v.VotingId == VoteId && v.Option == vote.Option1)
            });

            options.Add(new OptionPercentageDTO()
            {
                Text = vote.Option2,
                Percentage = await _unitOfWork.UserAnswerVotings
                    .CountAsync(v => v.VotingId == VoteId && v.Option == vote.Option2)
            });

            if(vote.Option3 != null)
            {
                options.Add(new OptionPercentageDTO()
                {
                    Text = vote.Option3,
                    Percentage = await _unitOfWork.UserAnswerVotings
                    .CountAsync(v => v.VotingId == VoteId && v.Option == vote.Option3)
                });
            }

            if (vote.Option4 != null)
            {
                options.Add(new OptionPercentageDTO()
                {
                    Text = vote.Option4,
                    Percentage = await _unitOfWork.UserAnswerVotings
                    .CountAsync(v => v.VotingId == VoteId && v.Option == vote.Option4)
                });
            }

            if (vote.Option5 != null)
            {
                options.Add(new OptionPercentageDTO()
                {
                    Text = vote.Option5,
                    Percentage = await _unitOfWork.UserAnswerVotings
                    .CountAsync(v => v.VotingId == VoteId && v.Option == vote.Option5)
                });
            }
            return options;
        }
        #endregion

        #endregion

    }
}