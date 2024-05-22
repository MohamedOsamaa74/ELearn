using ELearn.Application.DTOs.CommentDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface ICommentService
    {
        public Task<Response<ViewCommentDTO>> CreateCommentAsync(int postId, CreateCommentDTO Model);
        public Task<Response<ViewCommentDTO>> UpdateCommentAsync(int commentId, UpdateCommentDTO Model);
        public Task<Response<ViewCommentDTO>> DeleteCommentAsync(int commentId);
        public Task<Response<ViewCommentDTO>> GetCommentByIdAsync(int commentId);
        public Task<Response<ICollection<ViewCommentDTO>>> GetCommentsByPostIdAsync(int postId);
        public Task<Response<ICollection<ViewCommentDTO>>> GetCommentsByUserIdAsync(string userId);
        public  Task<Response<ViewCommentDTO>> DeleteAllCommentsAsync(int postId);
    }
}
