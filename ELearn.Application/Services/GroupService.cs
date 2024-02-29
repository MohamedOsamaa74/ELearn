using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GroupDTO>> CreateAsync(GroupDTO Model)
        {
            
            var user = await _userService.GetCurrentUserAsync();
            if(user is null)
                return ResponseHandler.BadRequest<GroupDTO>("User Dows Not Exist");
            try
            {
                Group group = new Group()
                {
                    GroupName = Model.Name,
                    Description = Model.Description,
                    CreatorId = user.Id,
                    DepartmentId = Model.DepartmentId
                };
                await _unitOfWork.Groups.AddAsync(group);
                return ResponseHandler.Created(Model);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<GroupDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }

        }

        public async Task<Response<GroupDTO>> DeleteAsync(int Id)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(Id);
            if (group is null)
                return ResponseHandler.NotFound<GroupDTO>("The Group Doesn't Exist");
            try
            {
                await _unitOfWork.Groups.DeleteAsync(group);
                return ResponseHandler.Deleted<GroupDTO>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<GroupDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }
    }
}