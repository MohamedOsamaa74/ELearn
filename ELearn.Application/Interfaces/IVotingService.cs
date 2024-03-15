using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IVotingService
    {
        public Task<Response<VotingDTO>> CreateNewAsync(VotingDTO Model);
        public Task<Response<ICollection<VotingDTO>>> GetVotesByDate(DateTime date);
        public Task<Response<ICollection<VotingDTO>>> GetVotesByCreator(string UserId);
        public Task<Response<VotingDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<VotingDTO>>> GetAllAsync();
        public Task<Response<VotingDTO>> DeleteAsync(int Id);
        public Task<Response<OptionDTO>> DeleteOptionAsync(int Id);
        public Task<Response<VotingDTO>> DeleteManyAsync(ICollection<int> Id);
        public Task<Response<VotingDTO>> UpdateAsync(int Id, VotingDTO Model);
        public Task<Response<OptionDTO>> EditOption(int Id, OptionDTO Model);
        public Task<Response<ICollection<VotingDTO>>> GetFromGroups(int GroupId);
        public Task<Response<UserVotingDTO>> RecieveStudentResponse(int VotingId, int OptionId);
        public Task<Response<ICollection<UserVotingDTO>>> GetVotingResponses(int VotingId);
    }
}