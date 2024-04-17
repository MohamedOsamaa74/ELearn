using ELearn.Application.DTOs.UserDTOs;
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
        public Task<string> GetCurrentUserIDAsync();
        public Task<ApplicationUser> GetByUserName(string UserName);
        public Task<Response<AddUserDTO>> CreateNewUserAsync(AddUserDTO user);
        public Task<Response<ICollection<AddUserDTO>>> AddMultipleUsersAsync(IFormFile file);
        public Task<Response<ICollection<AddUserDTO>>> GetAllAsync();
        public Task<Response<AddUserDTO>> EditUserAsync(string id, EditUserDTO NewData);
        public Task<Response<AddUserDTO>> DeleteUserAsync(string Id);
        public Task<Response<ICollection<AddUserDTO>>> DeleteManyAsync(List<string> Ids);
    }
}
