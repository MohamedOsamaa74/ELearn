using ELearn.Application.DTOs.AssignmentDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IAssignmentService
    {
        public Task<Response<ViewAssignmentDTO>> CreateAssignmentAsync(UploadAssignmentDTO Model);
        public Task<Response<UploadAssignmentDTO>>DeleteAssignmentAsync(int Id);
        public Task<Response<UploadAssignmentDTO>> UpdateAssignmentAsync(int AssignmentId, UploadAssignmentDTO Model);
        public Task<Response<ViewAssignmentDTO>> GetAssignmentByIdAsync(int AssignmentId);
        public Task<Response<ICollection<ViewAssignmentDTO>>> GetFromGroupAsync(int GroupId);
        public Task<Response<ICollection<UploadAssignmentDTO>>> GetAllAssignmentsAsync(string sort_by, string search_term);
        //public Task<IEnumerable<AssignmentDTO>> GetAssignmentsByStaffId(string staffId);
        public Task<Response<ICollection<ViewAssignmentDTO>>> GetAssignmentsByCreator(string sort_by, string search_term);
        public Task<Response<ICollection<UploadAssignmentDTO>>> DeleteManyAsync(List<int> Ids);
    }
}
