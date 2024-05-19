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


    }
}
