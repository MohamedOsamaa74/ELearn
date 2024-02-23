using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
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

        public AnnouncementService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Announcement> CreateNew(string Creator, string Text)
        {
            try
            {
                Announcement announcement = new Announcement()
                {
                    UserId = Creator,
                    Text = Text
                };
                return announcement;
            }
            catch
            {
                return null;
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

        public async Task<IEnumerable<GroupAnnouncment>> SendToGroups(ICollection<int> Groups, int announcementId)
        {
            List<GroupAnnouncment> GroupAnnouncments = new List<GroupAnnouncment>();
            foreach (var groupId in Groups)
            {
                GroupAnnouncment NewGroupAnnouncement = new GroupAnnouncment()
                {
                    GroupId = groupId,
                    AnnouncementId = announcementId
                };
                GroupAnnouncments.Add(NewGroupAnnouncement);
            }
            return GroupAnnouncments;
        }
    }
}
