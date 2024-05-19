using ELearn.Application.DTOs.AssignmentDTOs;
using ELearn.Application.DTOs.PostDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IPostService
    {
        public Task<Response<ViewPostDTO>> CreatePostAsync(CreatePostDTO Model);

    }
}
