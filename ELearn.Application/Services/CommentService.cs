using AutoMapper;
using ELearn.Application.DTOs.CommentDTOs;
using ELearn.Application.DTOs.MessageDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;

        }
        #region CreateComment
        public async Task<Response<ViewCommentDTO>> CreateCommentAsync(int postId, CreateCommentDTO Model)
        {
            try
            {
                var comment = _mapper.Map<Comment>(Model);
                var user = _userService.GetCurrentUserAsync();
                comment.UserId = user.Result.Id;
                comment.PostId = postId;
                var userName = user.Result.FirstName + " " + user.Result.LastName;
                var viewComment = _mapper.Map<ViewCommentDTO>(comment);// mapper 
                viewComment.CreatorName = userName;

                if (comment.UserId == null)
                {
                    return ResponseHandler.BadRequest<ViewCommentDTO>("User not found");
                }
                if (Model == null)
                {
                    return ResponseHandler.BadRequest<ViewCommentDTO>();
                }
                // Validate the comment
                var validate = new CommentValidation().Validate(comment);
                if (!validate.IsValid)
                {
                    // Get the errors 
                    var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                    return ResponseHandler.BadRequest<ViewCommentDTO>(null, errors);
                }

                await _unitOfWork.Comments.AddAsync(comment);
                return ResponseHandler.Success<ViewCommentDTO>(viewComment);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewCommentDTO>(ex.Message);
            }
        }
        #endregion

        #region UpdateComment
        public async Task<Response<ViewCommentDTO>> UpdateCommentAsync(int commentId, UpdateCommentDTO Model)
        {
            try
            {
                var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
                if (comment == null)
                {
                    return ResponseHandler.NotFound<ViewCommentDTO>("Comment not found");
                }
                var user = _userService.GetCurrentUserAsync();
                if (comment.UserId != user.Result.Id)
                {
                    return ResponseHandler.Unauthorized<ViewCommentDTO>();
                }
                comment.Text = Model.Text;
                await _unitOfWork.Comments.UpdateAsync(comment);
                return ResponseHandler.Updated<ViewCommentDTO>(_mapper.Map<ViewCommentDTO>(comment));
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewCommentDTO>(ex.Message);
            }
        }
        #endregion

        #region DeleteComment
        public async Task<Response<ViewCommentDTO>> DeleteCommentAsync(int commentId)
        {
            try
            {
                var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
                if (comment == null)
                {
                    return ResponseHandler.NotFound<ViewCommentDTO>("Comment not found");
                }
                var user = _userService.GetCurrentUserAsync();
                if (comment.UserId != user.Result.Id)
                {
                    return ResponseHandler.Unauthorized<ViewCommentDTO>();
                }
                await _unitOfWork.Comments.DeleteAsync(comment);
                return ResponseHandler.Deleted<ViewCommentDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewCommentDTO>(ex.Message);
            }
        }
        #endregion

        #region GetCommentById
        public async Task<Response<ViewCommentDTO>> GetCommentByIdAsync(int commentId)
        {
            try
            {
                var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
                if (comment == null)
                {
                    return ResponseHandler.NotFound<ViewCommentDTO>("Comment not found");
                }
                var user = await _userService.GetByIdAsync(comment.UserId);
                var commentDTO = _mapper.Map<ViewCommentDTO>(comment);
                commentDTO.CreatorName = user.FirstName + " " + user.LastName;
                return ResponseHandler.Success(commentDTO);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewCommentDTO>(ex.Message);
            }
        }
        #endregion

        #region GetCommentsByPostId
        public async Task<Response<ICollection<ViewCommentDTO>>> GetCommentsByPostIdAsync(int postId)
        {
            try
            {
                var comments = await _unitOfWork.Comments.GetWhereAsync(c => c.PostId == postId);
                if (comments == null)
                {
                    return ResponseHandler.NotFound<ICollection<ViewCommentDTO>>("Comments not found");
                }
                ICollection<ViewCommentDTO> viewComments = new List<ViewCommentDTO>();
                foreach (var comment in comments)
                {
                    var user = await _userService.GetByIdAsync(comment.UserId);
                    var commentDTO = _mapper.Map<ViewCommentDTO>(comment);
                    commentDTO.CreatorName = user.FirstName + " " + user.LastName;
                    viewComments.Add(commentDTO);
                }
                return ResponseHandler.ManySuccess(viewComments);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewCommentDTO>>(ex.Message);
            }
        }
        #endregion

        #region GetCommentsByUserId
        public async Task<Response<ICollection<ViewCommentDTO>>> GetCommentsByUserIdAsync(string userId)
        {
            try
            {
                var comments = await _unitOfWork.Comments.GetWhereAsync(c => c.UserId == userId);
                if (comments == null)
                {
                    return ResponseHandler.NotFound<ICollection<ViewCommentDTO>>("Comments not found");
                }
                ICollection<ViewCommentDTO> viewComments = new List<ViewCommentDTO>();
                foreach (var comment in comments)
                {
                    var user = await _userService.GetByIdAsync(comment.UserId);
                    var commentDTO = _mapper.Map<ViewCommentDTO>(comment);
                    commentDTO.CreatorName = user.FirstName + " " + user.LastName;
                    viewComments.Add(commentDTO);
                }
                return ResponseHandler.ManySuccess(viewComments);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewCommentDTO>>(ex.Message);
            }
        }
        #endregion

        #region DeleteAllComments
        public async Task<Response<ViewCommentDTO>> DeleteAllCommentsAsync(int postId)
        {
            try
            {
                var comments = await _unitOfWork.Comments.GetAllAsync(c => c.PostId == postId);

                if (comments == null)
                {
                    return ResponseHandler.NotFound<ViewCommentDTO>("Comments not found");
                }
                await _unitOfWork.Comments.DeleteRangeAsync((ICollection<Comment>)comments);
                return ResponseHandler.Deleted<ViewCommentDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewCommentDTO>(ex.Message);
            }
        }
        #endregion
    }
}