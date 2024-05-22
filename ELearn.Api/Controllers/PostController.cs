using ELearn.Application.DTOs.PostDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        #region Create Post
        [HttpPost("CreateNewPost")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDTO model)
        {
            var response = await _postService.CreatePostAsync(model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Update Post
        [HttpPut("UpdatePost/{PostId}")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> UpdatePost(int PostId, [FromForm] CreatePostDTO model)
        {
            var response = await _postService.UpdatePostAsync(PostId, model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete Post
        [HttpDelete("DeletePost/{PostId}")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> DeletePost(int PostId)
        {
            var response = await _postService.DeletePostAsync(PostId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Post By Id
        [HttpGet("GetPostById/{PostId}")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> GetPostById(int PostId)
        {
            var response = await _postService.GetPostByIdAsync(PostId);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get All Posts
        [HttpGet("GetAllPosts")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> GetAllPosts()
        {
            var response = await _postService.GetAllPostsAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Posts By User Id
        [HttpGet("GetPostsByUserId")]
        [Authorize(Roles = "Admin ,Student")]
        public async Task<IActionResult> GetPostsByUserId()
        {
            var response = await _postService.GetPostsByUserIdAsync();
            return this.CreateResponse(response);
        }
        #endregion



    }
}
