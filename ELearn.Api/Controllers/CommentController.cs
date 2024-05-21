using ELearn.Application.DTOs.CommentDTOs;
using ELearn.Application.DTOs.PostDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Application.Services;
using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        #region Create Comment
        [HttpPost("CreateComment")]
      
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> CreateComment(int postId,[FromBody] CreateCommentDTO model)
        {
            var response = await _commentService.CreateCommentAsync(postId,model);


            return this.CreateResponse(response);
        }

        #endregion

        #region Update Comment
        [HttpPut("UpdateComment")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] UpdateCommentDTO model)
        {
            var response = await _commentService.UpdateCommentAsync(commentId, model);

            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Comment
        [HttpDelete("DeleteComment")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var response = await _commentService.DeleteCommentAsync(commentId);

            return this.CreateResponse(response);
        }
        #endregion

        #region Get Comment By Id
        [HttpGet("GetCommentById")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            var response = await _commentService.GetCommentByIdAsync(commentId);

            return this.CreateResponse(response);
        }
        #endregion

        #region Get Comments By Post Id
        [HttpGet("GetCommentsByPostId")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var response = await _commentService.GetCommentsByPostIdAsync(postId);

            return this.CreateResponse(response);
        }
        #endregion

        #region Get Comments By User Id
        [HttpGet("GetCommentsByUserId")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> GetCommentsByUserId(string userId)
        {
            var response = await _commentService.GetCommentsByUserIdAsync(userId);

            return this.CreateResponse(response);
        }
        #endregion

        #region Delete All Comments
        [HttpDelete("DeleteAllComments/{PostId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteAllComments(int PostId)
        {
            var response = await _commentService.DeleteAllCommentsAsync(PostId);

            return this.CreateResponse(response);
        }
        #endregion



    }
}
