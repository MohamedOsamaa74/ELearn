using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AnnouncementService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<Response<AnnouncementDTO>> GetByIdAsync(int id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(id);
            if (announcement == null)
                return ResponseHandler.NotFound<AnnouncementDTO>("Announcement Not Found");
            try
            {
                var groups = await GetAnnouncementGroupsAsync(id);
                var announcementDTO = _mapper.Map<AnnouncementDTO>(announcement);
                announcementDTO.Groups = groups;
                return ResponseHandler.Success(announcementDTO);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {ex}");
            }
        }
       
        public async Task<Response<ICollection<AnnouncementDTO>>> GetAllAnnouncementsAsync()
        {
            try
            {
                var announcements = await _unitOfWork.Announcments.GetAllAsync();

                if (announcements == null || !announcements.Any())
                {
                    return ResponseHandler.NotFound<ICollection<AnnouncementDTO>>("No announcements found");
                }
                var announcementDTOs = new List<AnnouncementDTO>();
                foreach (var item in announcements)
                {
                    var dto = _mapper.Map<AnnouncementDTO>(item);
                    dto.Groups = await GetAnnouncementGroupsAsync(item.Id);
                    announcementDTOs.Add(dto);
                }
                return ResponseHandler.ManySuccess(announcementDTOs);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<AnnouncementDTO>>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Response<AnnouncementDTO>> CreateNewAsync(AnnouncementDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var announcement = _mapper.Map<Announcement>(Model);
                announcement.UserId = user.Id;
                await _unitOfWork.Announcments.AddAsync(announcement);
                await SendToGroupsAsync((ICollection<int>)Model.Groups, announcement.Id);
                return ResponseHandler.Created(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }

        public async Task<Response<AnnouncementDTO>> DeleteAsync(int Id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(Id);
            if (announcement is null)
                return ResponseHandler.NotFound<AnnouncementDTO>();
            try
            {
                await _unitOfWork.Announcments.DeleteAsync(announcement);
                return ResponseHandler.Deleted<AnnouncementDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }

        public async Task<Response<AnnouncementDTO>> UpdateAsync(AnnouncementDTO Model, int Id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(Id);
            if (announcement is null)
                return ResponseHandler.NotFound<AnnouncementDTO>();
            try
            {
                announcement.Text = Model.text;
                foreach (var groupId in Model.Groups)
                {
                    var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
                    if (!await _unitOfWork.GroupAnnouncments.FindIfExistAsync(ga => ga.GroupId == groupId && ga.AnnouncementId == Id))
                    {
                        await _unitOfWork.GroupAnnouncments.AddAsync(new GroupAnnouncment() { AnnouncementId = Id, GroupId = groupId });
                    }
                }
                await _unitOfWork.Announcments.UpdateAsync(announcement);
                return ResponseHandler.Updated(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }

        public async Task<Response<ICollection<Announcement>>> GetFromGroupsAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            
            var announcements = await _context.Announcements
                .Include(a => a.GroupAnnouncements)
                .Where(a => a.GroupAnnouncements.Any(ga => 
                        ga.GroupId == ga.Group.Id && 
                        ga.Group.UsersInGroup.Any(ug => ug.Id == user.Id)))
                .Select(a => a.Text)
                .ToListAsync();
            if (announcements is null || !announcements.Any())
                return ResponseHandler.NotFound<ICollection<Announcement>>();

            return ResponseHandler.ManySuccess((ICollection<Announcement>)announcements);
        }

        public async Task<Response<ICollection<AnnouncementDTO>>> GetByCreatorAsync()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();

                var announcements = await _context.Announcements.
                    Where(a => a.UserId == user.Id)
                    .ToListAsync();

                if (announcements is null || !announcements.Any())
                    return ResponseHandler.NotFound<ICollection<AnnouncementDTO>>("You Haven't sent Any Announcements");
                var announcementDTOs = new List<AnnouncementDTO>();
                foreach (var item in announcements)
                {
                    var dto = _mapper.Map<AnnouncementDTO>(item);
                    dto.Groups = await GetAnnouncementGroupsAsync(item.Id);
                    announcementDTOs.Add(dto);
                }
                return ResponseHandler.ManySuccess(announcementDTOs);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AnnouncementDTO>>($"An Error Occurred, {Ex}");
            }
        }

        public async Task<Response<ICollection<AnnouncementDTO>>> DeleteManyAsync(List<int>Ids)
        {
            try
            {
                var announcements = await GetAnnouncementsAsync(Ids);
                if (announcements is null || !announcements.Any())
                    return ResponseHandler.NotFound<ICollection<AnnouncementDTO>>();

                await _unitOfWork.Announcments.DeleteRangeAsync(announcements);
                return ResponseHandler.Deleted<ICollection<AnnouncementDTO>>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AnnouncementDTO>>($"An Error Occured, {Ex}");
            }
        }
        
        private async Task<IEnumerable<int>> GetAnnouncementGroupsAsync(int announcementId)
        => await _unitOfWork.GroupAnnouncments
            .GetWhereSelectAsync(g => g.AnnouncementId == announcementId, g => g.GroupId);

        private async Task SendToGroupsAsync(ICollection<int> Groups, int announcementId)
        {
            foreach (var groupId in Groups)
            {
                GroupAnnouncment NewGroupAnnouncement = new GroupAnnouncment()
                {
                    GroupId = groupId,
                    AnnouncementId = announcementId
                };
                await _unitOfWork.GroupAnnouncments.AddAsync(NewGroupAnnouncement);
            }
        }

        private async Task<ICollection<Announcement>> GetAnnouncementsAsync(IEnumerable<int> Ids)
        {
            List<Announcement> announcements = new List<Announcement>();
            foreach (var Id in Ids)
            {
                var entity = await _unitOfWork.Announcments.GetByIdAsync(Id);
                if (entity != null)
                    announcements.Add(entity);
            }
            return announcements;
        }

    }
}
