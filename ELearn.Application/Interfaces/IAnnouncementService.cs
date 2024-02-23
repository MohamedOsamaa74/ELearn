using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IAnnouncementService
    {
        public Task<Announcement> CreateNew(string Creator, string Text);
        public Task<IEnumerable<GroupAnnouncment>> SendToGroups(ICollection<int> Groups, int announcementId);
        public Task<ICollection<Announcement>> GetAnnouncements(IEnumerable<int> Ids);
        public Task<ICollection<Announcement>> GetFromGroups(string UserId);
        //Send Announcement To Multiple Users (Not Deecided)
    }
}
