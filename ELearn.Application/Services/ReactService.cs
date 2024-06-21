using ELearn.Application.DTOs.ReactDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using Microsoft.IdentityModel.Tokens;

namespace ELearn.Application.Services
{
    public class ReactService : IReactService
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public ReactService(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region CreateNew
        public async Task<Response<ReactDTO>> CreateNewAsync(ReactDTO reactDTO)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var react = new React()
                {
                    UserID = user.Id
                };
                if(reactDTO.Parent == "Comment")
                {
                    react.CommentId = reactDTO.ParentId;
                }
                else if(reactDTO.Parent == "Post")
                {
                    react.PostID = reactDTO.ParentId;
                }
                var validate = new ReactValidation().Validate(react);
                if (!validate.IsValid)
                {
                    return ResponseHandler.BadRequest<ReactDTO>(null,validate.Errors.Select(x => x.ErrorMessage).ToList());
                }
                await _unitOfWork.Reacts.AddAsync(react);
                reactDTO.FirstName = user.FirstName;
                return ResponseHandler.Success(reactDTO);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ReactDTO>($"An Error Occurred, {ex}");
            }
        }
        #endregion

        #region Delete
        public async Task<Response<string>> DeleteAsync(int Id)
        {
            try
            {
                var react = await _unitOfWork.Reacts.GetByIdAsync(Id);
                if(react is null)
                {
                    return ResponseHandler.NotFound<string>();
                }
                await _unitOfWork.Reacts.DeleteAsync(react);
                return ResponseHandler.Deleted<string>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<string>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region Get By Parent
        public async Task<Response<ICollection<string>>>GetReactorsAsync(ReactDTO reactDTO)
        {
            try
            {
                ICollection<string> usersReacted = [];
                if(reactDTO.Parent == "Post")
                {
                    var Post = await _unitOfWork.Posts.GetByIdAsync(reactDTO.ParentId);
                    if(Post is null)
                        return ResponseHandler.NotFound<ICollection<string>>();
                    var Reacts = await _unitOfWork.Reacts.GetWhereAsync(p => p.PostID == Post.Id);
                    foreach(var react in Reacts)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(react.UserID);
                        usersReacted.Add(user.FirstName+' '+user.LastName);
                    }
                    if(usersReacted.IsNullOrEmpty())
                        return ResponseHandler.NotFound<ICollection<string>>();
                }
                else if(reactDTO.Parent == "Comment")
                {
                    var Comment = await _unitOfWork.Comments.GetByIdAsync(reactDTO.ParentId);
                    if (Comment is null)
                        return ResponseHandler.NotFound<ICollection<string>>();
                    var Reacts = await _unitOfWork.Reacts.GetWhereAsync(c => c.CommentId == Comment.Id);
                    foreach (var react in Reacts)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(react.UserID);
                        usersReacted.Add(user.FirstName + ' ' + user.LastName);
                    }
                    if(usersReacted.IsNullOrEmpty())
                        return ResponseHandler.NotFound<ICollection<string>>();
                }
                return ResponseHandler.Success(usersReacted);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<string>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion
    }
}
