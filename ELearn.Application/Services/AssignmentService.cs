using AutoMapper;
using ELearn.Application.DTOs.AssignmentDTOs;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.DTOs.MessageDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using MailKit;
using Microsoft.AspNetCore.Http;

namespace ELearn.Application.Services
{
    public class AssignmentService : IAssignmentService
    {
        #region Fields
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IGroupService _groupService;
        public AssignmentService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IFileService fileService, IGroupService groupService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        #endregion

        #region Create Assignment
        public async Task<Response<ViewAssignmentDTO>> CreateAssignmentAsync(UploadAssignmentDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user is null)
                    return ResponseHandler.NotFound<ViewAssignmentDTO>("User does Not Exist");

                if(await _unitOfWork.Groups.GetByIdAsync(Model.GroupId) == null)
                    return ResponseHandler.NotFound<ViewAssignmentDTO>("Group does Not Exist");

                if (Model.End <= DateTime.UtcNow.ToLocalTime())
                    return ResponseHandler.BadRequest<ViewAssignmentDTO>($"End Date {Model.End} Must be in the Future");

                if(!await _groupService.UserInGroupAsync(Model.GroupId, user.UserName))
                    return ResponseHandler.Unauthorized<ViewAssignmentDTO>("You are not a member of this group");
                var assignment = _mapper.Map<Assignment>(Model);
                assignment.UserId = user.Id;

                // Validate the Assignment
                var validate = new AssignmentValidation().Validate(assignment);
                if (!validate.IsValid)
                {
                    // Get the errors 
                    var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                    return ResponseHandler.BadRequest<ViewAssignmentDTO>(null, errors);
                }

                await _unitOfWork.Assignments.AddAsync(assignment);
                ICollection<string> ViewURLs = [];
                if (Model.Attachements != null && Model.Attachements.Count != 0)
                {
                    foreach (var file in Model.Attachements)
                    {
                        var uploadFileDTO = new UploadFileDTO
                        {
                            ParentId = assignment.Id,
                            File = file,
                            FolderName = "Assignments"
                        };
                        var uploadResult = await _fileService.UploadFileAsync(uploadFileDTO);
                        if (uploadResult.Succeeded)
                        {
                            ViewURLs.Add(uploadResult.Data.ViewUrl);
                        }
                    }
                }
                var assignmentDto = _mapper.Map<ViewAssignmentDTO>(assignment);
                assignmentDto.CreatorName = user.FirstName + ' ' + user.LastName;
                assignmentDto.FilesURLs = ViewURLs;
                return ResponseHandler.Created(assignmentDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewAssignmentDTO>($"An error occurred while creating Assignment: {ex.Message}");
            }
        }
        #endregion

        #region Delete Assignment 
        public async Task<Response<UploadAssignmentDTO>> DeleteAssignmentAsync(int Id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(Id);
            if (assignment is null)
                return ResponseHandler.NotFound<UploadAssignmentDTO>();

            var user = await _userService.GetCurrentUserAsync();
            if(assignment.UserId != user.Id)
                return ResponseHandler.Unauthorized<UploadAssignmentDTO>();
            try
            {
                await _unitOfWork.Assignments.DeleteAsync(assignment);
                return ResponseHandler.Deleted<UploadAssignmentDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UploadAssignmentDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }


        #endregion

        #region Update Assignment
        public async Task<Response<UploadAssignmentDTO>> UpdateAssignmentAsync(int AssignmentId, UploadAssignmentDTO Model)

        {
            var AssignmentToUpdate = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
            if (AssignmentToUpdate == null)
                return ResponseHandler.NotFound<UploadAssignmentDTO>();
            var user = await _userService.GetCurrentUserAsync();
            var role = await _userService.GetUserRoleAsync();
            if (AssignmentToUpdate.UserId != user.Id && role != "Admin")
                return ResponseHandler.Unauthorized<UploadAssignmentDTO>();
            try
            {
                _mapper.Map(Model, AssignmentToUpdate);
                await _unitOfWork.Assignments.UpdateAsync(AssignmentToUpdate);
                var updatedDto = _mapper.Map<UploadAssignmentDTO>(AssignmentToUpdate);
                return ResponseHandler.Updated(updatedDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<UploadAssignmentDTO>($"An error occurred while updating material: {ex.Message}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Response<ICollection<UploadAssignmentDTO>>> GetAllAssignmentsAsync(string sort_by, string search_term)
        {
            try
            {
                var assignments = await _unitOfWork.Assignments.GetAllAsync();
                if (assignments == null)
                {
                    return ResponseHandler.NotFound<ICollection<UploadAssignmentDTO>>("There are no assignments yet");
                }
                if (!string.IsNullOrEmpty(sort_by))
                {
                    switch (sort_by.ToLower())
                    {
                        case "title":
                            assignments = assignments.OrderBy(a => a.Title).ToList();
                            break;
                        case "title desc":
                            assignments = assignments.OrderByDescending(a => a.Title).ToList();
                            break;
                        case "start date":
                            assignments = assignments.OrderBy(a => a.CreationDate).ToList();
                            break;
                        case "date desc":
                            assignments = assignments.OrderByDescending(a => a.CreationDate).ToList();
                            break;
                        case "end date":
                            assignments = assignments.OrderBy(a => a.End).ToList();
                            break;
                        case "end desc":
                            assignments = assignments.OrderByDescending(a => a.End).ToList();
                            break;
                        case "active first":
                            var currentDate = DateTime.UtcNow.ToLocalTime();
                            assignments = assignments.OrderByDescending(a => a.CreationDate <= currentDate && a.End >= currentDate).ThenBy(a => a.Title).ToList();
                            break;
                        case "not active":
                            var currentDate2 = DateTime.UtcNow.ToLocalTime();
                            assignments = assignments.OrderBy(a => a.CreationDate <= currentDate2 && a.End >= currentDate2).ThenBy(a => a.Title).ToList();
                            break;
                        default:
                            assignments = assignments.OrderBy(a => a.Title).ToList();
                            break;
                    }
                }
                var assignmentDtos = _mapper.Map<ICollection<UploadAssignmentDTO>>(assignments);
                if (!string.IsNullOrEmpty(search_term))
                {
                    assignmentDtos = assignmentDtos.Where(a => a.Title.Contains(search_term, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                return ResponseHandler.Success(assignmentDtos);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<UploadAssignmentDTO>>($"An error occurred while retrieving materials: {ex.Message}");
            }
        }
        #endregion

        #region Get Assignment By ID
        public async Task<Response<ViewAssignmentDTO>> GetAssignmentByIdAsync(int AssignmentId)
        {
            try
            {
                var assignment = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
                if (assignment == null)
                {
                    return ResponseHandler.NotFound<ViewAssignmentDTO>("There is no such Assignment");
                }

                var assignmentDto = _mapper.Map<ViewAssignmentDTO>(assignment);
                var user = await _unitOfWork.Users.GetByIdAsync(assignment.UserId);
                assignmentDto.CreatorName = user.FirstName + ' ' + user.LastName;
                assignmentDto.FilesURLs = await GetAssignmentFilesAsync(AssignmentId);
                return ResponseHandler.Success(assignmentDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewAssignmentDTO>($"An error occurred while retrieving Assignment: {ex.Message}");
            }
        }
        #endregion

        #region Submit Assignment Response
        public async Task<Response<ViewAssignmentResponseDTO>> SubmitAssignmentResponseAsync(int AssignmentId, IFormFile file)
        {
            try
            {
                var assignment = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
                if (assignment is null)
                    return ResponseHandler.NotFound<ViewAssignmentResponseDTO>("Assignment does Not Exist");

                var user = await _userService.GetCurrentUserAsync();
                if (user is null)
                    return ResponseHandler.NotFound<ViewAssignmentResponseDTO>("User does Not Exist");

                var userAnswerAssignment = new UserAnswerAssignment
                {
                    UserId = user.Id,
                    AssignmentId = AssignmentId
                };
                await _unitOfWork.UserAnswerAssignments.AddAsync(userAnswerAssignment);
                var uploadFileDTO = new UploadFileDTO
                {
                    ParentId = userAnswerAssignment.Id,
                    File = file,
                    FolderName = "AssignmentsResponses"
                };
                var uploadResult = await _fileService.UploadFileAsync(uploadFileDTO);
                if (!uploadResult.Succeeded)
                {
                    await _unitOfWork.UserAnswerAssignments.DeleteAsync(userAnswerAssignment);
                    return ResponseHandler.BadRequest<ViewAssignmentResponseDTO>($"An error occurred while uploading the file: {uploadResult.Message}");
                }
                var responseDto = _mapper.Map<ViewAssignmentResponseDTO>(userAnswerAssignment);
                responseDto.FullName = user.FirstName + ' ' + user.LastName;
                responseDto.UserName = user.UserName;
                responseDto.UploadDate = DateOnly.FromDateTime(userAnswerAssignment.CreationDate.Date);
                responseDto.UploadTime = TimeOnly.FromTimeSpan(userAnswerAssignment.CreationDate.TimeOfDay);
                responseDto.FileURL = uploadResult.Data.ViewUrl;
                return ResponseHandler.Created(responseDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewAssignmentResponseDTO>($"An error occurred while submitting the response: {ex.Message}");
            }
        }
        #endregion

        #region Give Grade To Student Response
        public async Task<Response<int>> GiveGradeToStudentResponseAsync(int userAnswerAssignmentId, int Mark)
        {
            try
            {
                var response = await _unitOfWork.UserAnswerAssignments.GetByIdAsync(userAnswerAssignmentId);
                if(response is null)
                {
                    return ResponseHandler.NotFound<int>("The Response Does Not Exist");
                }
                var assignment = await _unitOfWork.Assignments.GetByIdAsync(response.AssignmentId);
                if (Mark < 0 || Mark > assignment.Grade)
                    return ResponseHandler.BadRequest<int>($"Mark is Invalid, it has to be between 0 and {assignment.Grade}");
                
                response.Grade = Mark;
                await _unitOfWork.UserAnswerAssignments.UpdateAsync(response);
                return ResponseHandler.Success(Mark);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<int>($"An error Occurred, {Ex}");
            }
        }
        #endregion

        #region Get Assignment Responses
        public async Task<Response<ICollection<ViewAssignmentResponseDTO>>> GetAssignmentResponsesAsync(int AssignmentId, string filter_by = null, string sort_by = null)
        {
            try
            {
                var assignment = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
                if (assignment == null)
                {
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentResponseDTO>>("There is no such Assignment");
                }
                var responses = await _unitOfWork.UserAnswerAssignments.GetWhereAsync(a => a.AssignmentId == AssignmentId);
                if (responses == null || !responses.Any())
                {
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentResponseDTO>>("There are no responses for this Assignment");
                }

                ICollection<ViewAssignmentResponseDTO> responseDtos = [];
                foreach (var response in responses)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(response.UserId);
                    var responseDto = _mapper.Map<ViewAssignmentResponseDTO>(response);
                    responseDto.UserName = user.UserName;
                    responseDto.FullName = user.FirstName + ' ' + user.LastName;
                    responseDto.UploadDate = DateOnly.FromDateTime(response.CreationDate.Date);
                    responseDto.UploadTime = TimeOnly.FromTimeSpan(response.CreationDate.TimeOfDay);
                    responseDto.FileURL = await GetAssignmentResponseFileUrlAsync(response.Id);
                    responseDtos.Add(responseDto);
                }
                if (!string.IsNullOrEmpty(sort_by))
                {
                    responseDtos = sort_by.ToLower() switch
                    {
                        "name" => responseDtos.OrderBy(a => a.FullName).ToList(),
                        "name desc" => responseDtos.OrderByDescending(a => a.FullName).ToList(),
                        "date" => responseDtos.OrderBy(a => a.UploadDate).ToList(),
                        "date desc" => responseDtos.OrderByDescending(a => a.UploadDate).ThenByDescending(a => a.UploadTime).ToList(),
                        _ => responseDtos.OrderBy(a => a.FullName).ToList(),
                    };
                }
                if (!string.IsNullOrEmpty(filter_by))
                {
                    responseDtos = filter_by.ToLower() switch
                    {
                        "graded" => responseDtos.Where(a => a.Mark != null).ToList(),
                        "not graded" => responseDtos.Where(a => a.Mark == null).ToList(),
                        _ => [.. responseDtos],
                    };
                }
                if(responseDtos.Count == 0) 
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentResponseDTO>>($"there is no Responses");

                return ResponseHandler.Success(responseDtos);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAssignmentResponseDTO>>($"an Error Occurred, {ex}");
            }
        }
        #endregion

        #region Get All From Group
        public async Task<Response<ICollection<ViewAssignmentDTO>>> GetFromGroupAsync(int GroupId)
        {
            try
            {
                if(await _unitOfWork.Groups.GetByIdAsync(GroupId) == null)
                {
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentDTO>>("No Group Found with this Id");
                }
                var assignments = await _unitOfWork.Assignments.GetWhereAsync(g => g.GroupId == GroupId);
                if (assignments == null || !assignments.Any())
                {
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentDTO>>("No Assignments Found for this Group");
                }

                ICollection<ViewAssignmentDTO> viewAssignmentDTOs = [];
                foreach(var assignemnt in assignments)
                {
                    var dto = _mapper.Map<ViewAssignmentDTO>(assignemnt);
                    var user = await _unitOfWork.Users.GetByIdAsync(assignemnt.UserId);
                    dto.CreatorName = user.FirstName + ' ' + user.LastName;
                    viewAssignmentDTOs.Add(dto);
                }
                viewAssignmentDTOs = GetActiveAssignments(viewAssignmentDTOs);
                if (viewAssignmentDTOs is null)
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentDTO>>("No Active Assignments Found for this Group");

                return ResponseHandler.Success(viewAssignmentDTOs);

            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAssignmentDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetAssignmentsByCreator
        public async Task<Response<ICollection<ViewAssignmentDTO>>> GetAssignmentsByCreatorAsync(string sort_by, string search_term)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var Assignments = await _unitOfWork.Assignments.GetWhereSelectAsync(v => v.UserId == user.Id, v => v.Id);

                if (Assignments is null)
                    return ResponseHandler.NotFound<ICollection<ViewAssignmentDTO>>("There are No Assignments yet");
                ICollection<ViewAssignmentDTO> AssignmentsDto = [];
                foreach (var Assignment in Assignments)
                {
                    var Assignmentto = await GetAssignmentByIdAsync(Assignment);
                    AssignmentsDto.Add(Assignmentto.Data);
                }
                if (!string.IsNullOrEmpty(sort_by))
                {
                    switch (sort_by.ToLower())
                    {
                        case "title":
                            AssignmentsDto = [.. AssignmentsDto.OrderBy(a => a.Title)];
                            break;
                        case "title desc":
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.Title).ToList();
                            break;
                        case "start date":
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.CreationDate).ToList();
                            break;
                        case "date desc":
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.CreationDate).ToList();
                            break;
                        case "end date":
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.End).ToList();
                            break;
                        case "end desc":
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.End).ToList();
                            break;
                        case "active first":
                            var currentDate = DateTime.UtcNow.ToLocalTime();
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.CreationDate <= currentDate && a.End >= currentDate).ThenBy(a => a.Title).ToList();
                            break;
                        case "not active":
                            var currentDate2 = DateTime.UtcNow.ToLocalTime();
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.CreationDate <= currentDate2 && a.End >= currentDate2).ThenBy(a => a.Title).ToList();
                            break;
                        default:
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.Title).ToList();
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(search_term))
                {
                    AssignmentsDto = AssignmentsDto.Where(a => a.Title.Contains(search_term, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                return ResponseHandler.Success(AssignmentsDto);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAssignmentDTO>>($"An Error Occurred, {Ex}");
            }


        }

        #endregion

        #region Delete Delete All Assignments
        public async Task<Response<ICollection<UploadAssignmentDTO>>> DeleteManyAsync(List<int> Ids)
        {
            try
            {
                var assignments = await GetAssignmentsByIdsAsync(Ids); 
                if (assignments is null || !assignments.Any())
                    return ResponseHandler.NotFound<ICollection<UploadAssignmentDTO>>();

                await _unitOfWork.Assignments.DeleteRangeAsync(assignments);
                return ResponseHandler.Deleted<ICollection<UploadAssignmentDTO>>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<UploadAssignmentDTO>>($"An Error Occured, {Ex}");
            }
        }
        #endregion

        #region Methods

        #region GetAssignmentsByIdsAsync
        private async Task<ICollection<Assignment>> GetAssignmentsByIdsAsync(IEnumerable<int> Ids)
        {
            List<Assignment> assignments = new List<Assignment>();
            foreach (var Id in Ids)
            {
                var entity = await _unitOfWork.Assignments.GetByIdAsync(Id);
                if (entity != null)
                    assignments.Add(entity);
            }
            return assignments;
        }
        #endregion

        #region Get Assignment Files
        private async Task<ICollection<string>?> GetAssignmentFilesAsync(int AssignmentId)
        {
            var files = await _unitOfWork.Files.GetWhereAsync(a => a.AssignmentId == AssignmentId);
            if (files is null || !files.Any())
                return null;
            List<string> filesUrls = [];
            foreach (var file in files)
            {
                filesUrls.Add(file.ViewUrl);
            }
            return filesUrls;
        }
        #endregion

        #region Get User Response File URL
        private async Task<string?> GetAssignmentResponseFileUrlAsync(int UserAnswerAssignmentId)
        {
            var files = await _unitOfWork.Files.GetWhereAsync(a => a.UserAssignementId == UserAnswerAssignmentId);
            var file = files.FirstOrDefault();
            if (file is null)
                return null;
            return file.ViewUrl;
        }
        #endregion

        #region Get Active Assignments
        private ICollection<ViewAssignmentDTO>? GetActiveAssignments(ICollection<ViewAssignmentDTO> viewAssignmentDTOs)
        {
            var currentDate = DateTime.UtcNow.ToLocalTime();
            var activeAssignment = viewAssignmentDTOs.Where(a => a.CreationDate <= currentDate && a.End >= currentDate).ToList();
            if (activeAssignment is null || activeAssignment.Count == 0)
                return null;

            return activeAssignment;
        }
        #endregion

        #endregion
    }
}