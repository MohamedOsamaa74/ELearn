using ELearn.Application.DTOs.OptionDTOs;
using ELearn.Application.DTOs.VotingDTOs;
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
        public Task<Response<AddVotingDTO>> CreateNewAsync(AddVotingDTO Model);
        public Task<Response<ICollection<string>>> AddOptionsAsync(int VotingId, ICollection<string> Model);
        public Task<Response<ICollection<AddVotingDTO>>> GetVotesByDate(DateTime date);
        public Task<Response<ICollection<AddVotingDTO>>> GetVotesByCreator(string UserId); 
        public Task<Response<AddVotingDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<AddVotingDTO>>> GetAllAsync();
        public Task<Response<AddVotingDTO>> DeleteAsync(int Id);
        public Task<Response<OptionDTO>> DeleteOptionAsync(int Id);
        public Task<Response<AddVotingDTO>> DeleteManyAsync(ICollection<int> Id);
        public Task<Response<AddVotingDTO>> UpdateAsync(int Id, AddVotingDTO Model);
        public Task<Response<OptionDTO>> EditOption(int Id, OptionDTO Model);
        public Task<Response<ICollection<AddVotingDTO>>> GetFromGroups(int GroupId);
        public Task<Response<UserVotingDTO>> RecieveStudentResponse(int VotingId, int OptionId);
        public Task<Response<ICollection<UserVotingDTO>>> GetVotingResponses(int VotingId);
    }
}