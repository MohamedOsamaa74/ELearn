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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public AnnouncementService(AppDbContext context, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IUserService userService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Response<AnnouncementDTO>> GetByIdAsync(int id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(id);
            if (announcement == null)
                return ResponseHandler.NotFound<AnnouncementDTO>("Announcement Not Found");
            try
            {
                AnnouncementDTO announcementDTO = new AnnouncementDTO()
                {
                    text = announcement.Text,
                    Groups = await _unitOfWork.GroupAnnouncments.GetWhereSelectAsync(a => a.AnnouncementId == id, g => g.GroupId)
                };
                return ResponseHandler.Success(announcementDTO);
            }
            catch(Exception ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {ex}");
            }
        }

        public async Task<Response<AnnouncementDTO>> CreateNewAsync(AnnouncementDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                Announcement announcement = new Announcement()
                {
                    UserId = user.Id,
                    Text = Model.text
                };
                await _unitOfWork.Announcments.AddAsync(announcement);
                await SendToGroups((ICollection<int>)Model.Groups, announcement.Id);
                return ResponseHandler.Created(Model);
            }
            catch(Exception Ex)
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
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }

        public async Task<Response<AnnouncementDTO>> UpdateAsync(AnnouncementDTO Model, int Id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(Id);
            if(announcement is null)
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
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<AnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }
        
        public async Task<ICollection<Announcement>> GetAnnouncements(IEnumerable<int> Ids)
        {
            List<Announcement> announcements = new List<Announcement>();
            foreach (var Id in Ids)
            {
                var entity = await _context.Announcements.FirstOrDefaultAsync(a => a.Id == Id);
                if (entity != null)
                    announcements.Add(entity);
            }
            return announcements;
        }

        public async Task<ICollection<Announcement>> GetFromGroups(string UserId)
        {
            var announcements = await _context.Announcements
                .Include(a => a.GroupAnnouncements)
                .Where(a => a.GroupAnnouncements.Any(ga =>
                        ga.GroupId == ga.Group.Id &&
                        ga.Group.UsersInGroup.Any(ug => ug.Id == UserId)))
                .Select(a => a.Text)
                .ToListAsync();
            return (ICollection<Announcement>)announcements;
        }

        private async Task SendToGroups(ICollection<int> Groups, int announcementId)
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

    }
}
