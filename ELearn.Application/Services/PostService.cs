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
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public PostService (AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper , IFileService fileService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Response<ViewPostDTO>> CreatePostAsync(CreatePostDTO Model)
        {

            try
            {
                var post = _mapper.Map<Post>(Model);
                var user = _userService.GetCurrentUserAsync();
                post.UserId =user.Result.Id;
                var userName = user.Result.FirstName + user.Result.FirstName;
                var viewpost = _mapper.Map<ViewPostDTO>(post);// mapper 
                viewpost.CreatorName = userName;

                if (post.UserId == null)
                {
                    return ResponseHandler.BadRequest<ViewPostDTO>("User not found");
                }
                if (Model == null)
                {
                    return ResponseHandler.BadRequest<ViewPostDTO>();
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
                    viewpost.urls = ViewUrls;
                    
                }

                await _unitOfWork.Posts.AddAsync(post);
                return ResponseHandler.Created(viewpost);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewPostDTO>(ex.Message);
            }

            
            

        }
    }
}
