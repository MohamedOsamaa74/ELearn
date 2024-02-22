using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.Base;
using ELearn.InfraStructure.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories
{
    public class AnnouncementRepository : BaseRepo<Announcement>, IAnnouncementRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnnouncementRepository(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
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

        public async Task<ICollection<Announcement>> GetAnnouncements(List<int> Ids)
        {
            List<Announcement> announcements = new List<Announcement>();
            foreach (var Id in Ids)
            {
                var entity = await _context.FindAsync<Announcement>(Id);
                announcements.Add(entity);
            }
            return announcements;
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