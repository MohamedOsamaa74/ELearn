﻿using ELearn.Application.DTOs.AssignmentDTOs;
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
        public Task<Response<AssignmentDTO>>DeleteAssignmentAsync(int Id);
        public Task<Response<AssignmentDTO>> UpdateAssignmentAsync(int AssignmentId, AssignmentDTO Model);
        public Task<Response<ICollection<AssignmentDTO>>> GetAllAssignmentsAsync();
        public Task<Response<AssignmentDTO>> GetAssignmentByIdAsync(int AssignmentId);
        //public Task<IEnumerable<AssignmentDTO>> GetAssignmentsByStaffId(string staffId);
        public Task<Response<ICollection<AssignmentDTO>>> GetAssignmentsByCreator();
        public Task<Response<ICollection<AssignmentDTO>>> DeleteManyAsync(List<int> Ids);
    }
}
