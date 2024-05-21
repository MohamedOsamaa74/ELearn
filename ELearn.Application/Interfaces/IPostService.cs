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
        public Task<Response<ViewPostDTO>> UpdatePostAsync(int PostId, CreatePostDTO Model);
        public Task<Response<ViewPostDTO>> DeletePostAsync(int PostId);
        public Task<Response<ViewPostDTO>> GetPostByIdAsync(int PostId);
        public Task<Response<List<ViewPostDTO>>> GetAllPostsAsync();
        public Task<Response<List<ViewPostDTO>>> GetPostsByUserIdAsync();
    }
}
