using ELearn.Application.DTOs;
using ELearn.Application.Helpers.Response;
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
        public Task<Response<AnnouncementDTO>> GetByIdAsync(int id);
        public Task<Response<AnnouncementDTO>> CreateNewAsync(AnnouncementDTO Model);
        public Task<Response<AnnouncementDTO>> DeleteAsync(int Id);
        public Task<Response<AnnouncementDTO>> UpdateAsync(AnnouncementDTO Model, int Id);
        public Task<ICollection<Announcement>> GetAnnouncements(IEnumerable<int> Ids);
        public Task<ICollection<Announcement>> GetFromGroups(string UserId);

        //Send Announcement To Multiple Users (Not Deecided)
    }
}
