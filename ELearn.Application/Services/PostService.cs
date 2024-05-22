using AutoMapper;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.DTOs.PostDTOs;
using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class PostService : IPostService
    {
        #region Constructor
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public PostService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
            _fileService = fileService;
        }
        #endregion

        #region Create Post
        public async Task<Response<ViewPostDTO>> CreatePostAsync(CreatePostDTO Model)
        {

            try
            {
                var post = _mapper.Map<Post>(Model);
                var user = _userService.GetCurrentUserAsync();
                post.UserId = user.Result.Id;
                var userName = user.Result.FirstName + " " + user.Result.FirstName;
                var viewpost = _mapper.Map<ViewPostDTO>(post);
                viewpost.CreatorName = userName;

                if (post.UserId == null)
                {
                    return ResponseHandler.BadRequest<ViewPostDTO>("User not found");
                }
                if (Model == null)
                {
                    return ResponseHandler.BadRequest<ViewPostDTO>();
                }
                await _unitOfWork.Posts.AddAsync(post);
                List<string> ViewUrls = [];
                if (Model.Files != null && Model.Files.Any())
                {
                    foreach (var file in Model.Files)
                    {
                        var uploadFileDto = new UploadFileDTO()
                        { File = file, FolderName = "Posts", ParentId = post.Id };
                        var newFile = await _fileService.UploadFileAsync(uploadFileDto);
                        ViewUrls.Add(newFile.Data.ViewUrl);
                    }
                    viewpost.urls = ViewUrls;

                }

                return ResponseHandler.Created(viewpost);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewPostDTO>(ex.Message);
            }
        }
        #endregion

        #region Update Post
        public async Task<Response<ViewPostDTO>> UpdatePostAsync(int PostId, CreatePostDTO Model)
        {
            try
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(PostId);
                if (post == null)
                {
                    return ResponseHandler.NotFound<ViewPostDTO>("Post not found");
                }
                var user = _userService.GetCurrentUserAsync();
                if (post.UserId != user.Result.Id)
                {
                    return ResponseHandler.Unauthorized<ViewPostDTO>();
                }
                post.UserId = user.Result.Id;
                post.Text = Model.Text;
                post.CreationDate = post.CreationDate;
                await _unitOfWork.Posts.UpdateAsync(post);
                var viewpost = _mapper.Map<ViewPostDTO>(post);
                viewpost.CreatorName = user.Result.FirstName + " " + user.Result.LastName;

                var files =  await _fileService.GetFilesByPostId(PostId);
                var ids = files.Data;
                if (ids != null && ids.Any())
                {
                    foreach (var id in ids)
                    {
                        await _fileService.DeleteAsync(id);
                    }
                }
                List<string> ViewUrls = [];
                if (Model.Files != null && Model.Files.Any())
                {
                    foreach (var file in Model.Files)
                    {
                        var uploadFileDto = new UploadFileDTO()
                        { File = file, FolderName = "Posts", ParentId = post.Id };
                        var newFile = await _fileService.UploadFileAsync(uploadFileDto);
                        ViewUrls.Add(newFile.Data.ViewUrl);
                    }
                }
                
                viewpost.urls = ViewUrls;
                return ResponseHandler.Updated(viewpost);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewPostDTO>(ex.Message);
            }
        }
        #endregion

        #region Delete Post
        public async Task<Response<ViewPostDTO>> DeletePostAsync(int PostId)
        {
            try
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(PostId);
                if (post == null)
                {
                    return ResponseHandler.NotFound<ViewPostDTO>("Post not found");
                }
                var user = await _userService.GetCurrentUserAsync();
                var userRole = await _userService.GetUserRoleAsync();
                if (userRole=="Student" && post.UserId != user.Id )
                {
                    return ResponseHandler.Unauthorized<ViewPostDTO>();
                }
                var files = await _fileService.GetFilesByPostId(PostId);
                var ids = files.Data;
                if (ids != null && ids.Any())
                {
                    foreach (var id in ids)
                    {
                        await _fileService.DeleteAsync(id);
                    }
                }
                await _unitOfWork.Posts.DeleteAsync(post);
                return ResponseHandler.Deleted<ViewPostDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewPostDTO>(ex.Message);
            }
        }
        #endregion

        #region Get Post By Id
        public async Task<Response<ViewPostDTO>> GetPostByIdAsync(int PostId)
        {
            try
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(PostId);
                if (post == null)
                {
                    return ResponseHandler.NotFound<ViewPostDTO>("Post not found");
                }
                var viewpost = _mapper.Map<ViewPostDTO>(post);
                var userid = post.UserId;
                var user = await _userService.GetByIdAsync(userid);
                viewpost.CreatorName = user.FirstName + " " + user.LastName;
                var filesids = await _fileService.GetFilesByPostId(PostId);
                List<string> urls = [];
                foreach (var id in filesids.Data)
                {
                    var file = await _fileService.GetByIdAsync(id);
                    urls.Add(file.Data.ViewUrl);
                }
                viewpost.urls = urls;
                return ResponseHandler.Success(viewpost);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewPostDTO>(ex.Message);
            }
        }
        #endregion

        #region Get All Posts
        public async Task<Response<List<ViewPostDTO>>> GetAllPostsAsync()
        {
            try
            {
                var posts = await _unitOfWork.Posts.GetAllAsync();
                if (posts == null)
                {
                    return ResponseHandler.NotFound<List<ViewPostDTO>>("There Are No Posts");
                }
                List<ViewPostDTO> viewposts2 = [];
                foreach (var post in posts)
                {
                    var viewpost = _mapper.Map<ViewPostDTO>(post);
                    var userid = post.UserId;
                    var user = await _userService.GetByIdAsync(userid);
                    viewpost.CreatorName = user.FirstName + " " + user.LastName;
                    var filesids = await _fileService.GetFilesByPostId(post.Id);
                    List<string> urls = [];
                    foreach (var id in filesids.Data)
                    {
                        var file = await _fileService.GetByIdAsync(id);
                        urls.Add(file.Data.ViewUrl);
                    }
                    viewpost.urls = urls;
                    viewposts2.Add(viewpost);
                }
                return ResponseHandler.Success(viewposts2);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<List<ViewPostDTO>>(ex.Message);
            }
        }
        #endregion

        #region Get Posts By User Id
        public async Task<Response<List<ViewPostDTO>>> GetPostsByUserIdAsync()
        {
            try
            {
                var userr = await _userService.GetCurrentUserAsync();
                var UserId = userr.Id;
                var posts = await _unitOfWork.Posts.GetWhereAsync(p => p.UserId == UserId);
                if (posts == null)
                {
                    return ResponseHandler.NotFound<List<ViewPostDTO>>("There Are No Posts");
                }
                List<ViewPostDTO> viewposts2 = [];
                foreach (var post in posts)
                {
                    var viewpost = _mapper.Map<ViewPostDTO>(post);
                    var userid = post.UserId;
                    var user = await _userService.GetByIdAsync(userid);
                    viewpost.CreatorName = user.FirstName + " " + user.LastName;
                    var filesids = await _fileService.GetFilesByPostId(post.Id);
                    List<string> urls = [];
                    foreach (var id in filesids.Data)
                    {
                        var file = await _fileService.GetByIdAsync(id);
                        urls.Add(file.Data.ViewUrl);
                    }
                    viewpost.urls = urls;
                    viewposts2.Add(viewpost);
                }
                return ResponseHandler.Success(viewposts2);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<List<ViewPostDTO>>(ex.Message);
            }
        }
        #endregion






        
    }
}
