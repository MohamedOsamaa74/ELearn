using ELearn.Application.DTOs.OptionDTOs;
using ELearn.Application.DTOs.VotingDTOs;
using ELearn.Application.Helpers.Response;

namespace ELearn.Application.Interfaces
{
    public interface IVotingService
    {
        public Task<Response<ViewVotingDTO>> CreateNewAsync(AddVotingDTO Model);
        public Task<Response<ViewVotingDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<ViewVotingDTO>>> GetVotesByDate(DateTime date);
        public Task<Response<ICollection<ViewVotingDTO>>> GetUserGroupsVotes(); 
        public Task<Response<ICollection<ViewVotingDTO>>> GetAllAsync();
        public Task<Response<ICollection<ViewVotingDTO>>> GetFromGroup(int GroupId);
        public Task<Response<ViewVotingDTO>> DeleteAsync(int Id);
        public Task<Response<OptionDTO>> DeleteOptionAsync(int VotingId);
        public Task<Response<ViewVotingDTO>> DeleteManyAsync(ICollection<int> Id);
        public Task<Response<ViewVotingDTO>> UpdateAsync(int Id, AddVotingDTO Model);
        public Task<Response<OptionDTO>> EditOption(int Id, OptionDTO Model);
        public Task<Response<UserVotingDTO>> RecieveStudentResponse(int VotingId, string Option);
        public Task<Response<ICollection<UserVotingDTO>>> GetVotingResponses(int VotingId);
    }
}