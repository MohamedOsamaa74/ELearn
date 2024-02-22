using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Interfaces
{
    public interface IAnnouncementRepo : IBaseRepo<Announcement>
    {
        public Task<Announcement> CreateNew(string Creator, string Text);
        public Task<IEnumerable<GroupAnnouncment>> SendToGroups(ICollection<int> Groups, int announcementId);
        public Task<ICollection<Announcement>> GetAnnouncements(List<int> Ids);
        //Send Announcement To Multiple Users (Not Deecided)
    }
}
