using ELearn.Application.DTOs.AnnouncementDTOs;
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
        public Task<Response<ViewAnnouncementDTO>> GetByIdAsync(int id);
        public Task<Response<ViewAnnouncementDTO>> CreateNewAsync(UploadAnnouncementDTO Model);
        public Task<Response<UploadAnnouncementDTO>> DeleteAsync(int Id);
        public Task<Response<UploadAnnouncementDTO>> UpdateAsync(UploadAnnouncementDTO Model, int Id);
        public Task<Response<ICollection<ViewAnnouncementDTO>>> GetAllAnnouncementsAsync();
        public Task<Response<ICollection<ViewAnnouncementDTO>>> GetFromUserGroupsAsync();
        public Task<Response<ICollection<ViewAnnouncementDTO>>> GetByCreatorAsync();
        public Task<Response<ICollection<UploadAnnouncementDTO>>> DeleteManyAsync(List<int>Ids);
        //public Task<IEnumerable<int>> GetAnnouncementGroupsAsync(int AnnouncementId);
        //Send Announcement To Multiple Users (Not Deecided)
    }
}
