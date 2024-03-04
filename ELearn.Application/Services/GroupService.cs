using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public GroupService(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
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

        public async Task<Response<GroupDTO>> DeleteManyAsync(ICollection<int> Ids)
        {
            try
            {
                if (Ids.Count == 0) return ResponseHandler.NotFound<GroupDTO>();
                var groups = new List<Group>();
                foreach(var id in Ids)
                {
                    var group = await _unitOfWork.Groups.GetByIdAsync(id);
                    if (group != null)
                        groups.Add(group);
                }
                if(groups.IsNullOrEmpty())
                    return ResponseHandler.NotFound<GroupDTO>();

                await _unitOfWork.Groups.DeleteRangeAsync(groups);
                return ResponseHandler.Deleted<GroupDTO>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<GroupDTO>($"An Error Occurred,{Ex}");
            }
        }

        public async Task<Response<ICollection<GroupDTO>>> GetAllAsync()
        {
            try
            {
                var groups = await _unitOfWork.Groups.GetAllAsync();
                if (groups.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<GroupDTO>>("There Are No Groups Yet");
                var groupsDto = new List<GroupDTO>();
                foreach(var group in groups)
                {
                    var dto = _mapper.Map<GroupDTO>(group);
                    groupsDto.Add(dto);
                }
                return ResponseHandler.ManySuccess(groupsDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<GroupDTO>>($"An Error Occurred,{Ex}");
            }
        }

        public async Task<Response<GroupDTO>> GetByIdAsync(int Id)
        {
            try
            {
                var group = await _unitOfWork.Groups.GetByIdAsync(Id);
                if (group == null)
                    return ResponseHandler.NotFound<GroupDTO>("There Is No Such Group");
                return ResponseHandler.Success(_mapper.Map<GroupDTO>(group));
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<GroupDTO>($"An Error Occurred, {Ex}");
            }
        }

        public async Task<Response<ICollection<GroupDTO>>> GetByNameAsync(string Name)
        {
            try
            {
                var groups = await _context.Groups.Where(g => g.GroupName == Name).ToListAsync();
                if (groups.IsNullOrEmpty())
                    return ResponseHandler.NotFound<ICollection<GroupDTO>>("There Is No Groups With The Given Name");
                var groupsDto = new List<GroupDTO>();
                foreach(var group in groups)
                {
                    groupsDto.Add(_mapper.Map<GroupDTO>(group));
                }
                return ResponseHandler.ManySuccess(groupsDto);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<GroupDTO>>($"An Error Occurred, {Ex}");
            }
        }

        public async Task<Response<ICollection<GroupDTO>>> GetUserGroupsAsync(string UserName = null)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (UserName != null)
                    user = await _userService.GetByUserName(UserName);
                var groups = await _unitOfWork.UserGroups.GetWhereSelectAsync(u => u.UserId == user.Id, g => g.GroupId);
                var groupsDto = new List<GroupDTO>();
                foreach(var item in groups)
                {
                    var group = await _unitOfWork.Groups.GetByIdAsync(item);
                    groupsDto.Add(_mapper.Map<GroupDTO>(group));
                }
                return ResponseHandler.ManySuccess(groupsDto);
            }
            catch(Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<GroupDTO>>($"An Error Occurred, {ex}");
            }
        }

        public async Task<Response<GroupDTO>> UpdateAsync(GroupDTO Model, int Id)
        {
            try
            {
                var group = await _unitOfWork.Groups.GetByIdAsync(Id);
                if (group is null)
                    return ResponseHandler.NotFound<GroupDTO>();
                var entity = _mapper.Map<Group>(Model);
                entity.Id = Id;
                await _unitOfWork.Groups.UpdateAsync(entity);
                return ResponseHandler.Updated(Model);
            }
            catch(Exception ex)
            {
                return ResponseHandler.BadRequest<GroupDTO>($"An Error Occurred, {ex}");
            }
        }
    }
}