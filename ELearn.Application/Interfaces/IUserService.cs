using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IUserService
    {
        public Task<ApplicationUser> GetCurrentUserAsync();
        public Task<Response<UserDTO>> CreateNewUserAsync(UserDTO user);
        public Task<IEnumerable<Response<UserDTO>>> AddMultipleUsersAsync(IFormFile file);
        public Task<IEnumerable<ApplicationUser>> UploadCSV(IFormFile file);
        public Task<IEnumerable<UserDTO>> GetAllAsync();
    }
}
