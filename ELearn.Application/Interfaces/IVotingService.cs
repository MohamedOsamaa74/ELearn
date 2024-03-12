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
        public Task<Response<VotingDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<VotingDTO>>> GetAllAsync();
        public Task<Response<VotingDTO>> DeleteAsync(int Id);
    }
}
