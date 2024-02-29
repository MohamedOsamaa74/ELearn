using ELearn.Application.DTOs;
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
    }
}