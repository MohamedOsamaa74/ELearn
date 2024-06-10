using ELearn.Application.DTOs.GroupDTOs;
using ELearn.Application.DTOs.UserDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IGroupService
    {
        public Task<Response<GroupDTO>> CreateAsync(GroupDTO Model);
        public Task<Response<GroupDTO>> DeleteAsync(int Id);
        public Task<Response<GroupDTO>> DeleteManyAsync(ICollection<int> Ids);
        public Task<Response<GroupDTO>> UpdateAsync(GroupDTO Model, int Id);
        public Task<Response<ICollection<GroupDTO>>> GetAllAsync();
        public Task<Response<GroupDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<GroupDTO>>> GetByNameAsync(string Name);
        public Task<Response<ICollection<GroupDTO>>> GetUserGroupsAsync(string UserName = null);
        public Task<Response<ICollection<ParticipantDTO>>> GetGroupParticipantsAsync(int GroupId);
    }
}