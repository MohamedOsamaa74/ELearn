using AutoMapper;
using ELearn.Application.DTOs.AssignmentDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;

namespace ELearn.Application.Services
{
    public class AssignmentService : IAssignmentService
    {
        #region Fields
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AssignmentService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));


        }
        #endregion

        #region Delete Assignment 
        public async Task<Response<AssignmentDTO>> DeleteAssignmentAsync(int Id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(Id);
            if (assignment is null)
                return ResponseHandler.NotFound<AssignmentDTO>();
            try
            {
                await _unitOfWork.Assignments.DeleteAsync(assignment);
                return ResponseHandler.Deleted<AssignmentDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AssignmentDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }


        #endregion

        #region Update Assignment
        public async Task<Response<AssignmentDTO>> UpdateAssignmentAsync(int AssignmentId, AssignmentDTO Model)

        {
            var AssignmentToUpdate = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
            if (AssignmentToUpdate == null)
                return ResponseHandler.NotFound<AssignmentDTO>();

            try
            {

                _mapper.Map(Model, AssignmentToUpdate);


                await _unitOfWork.Assignments.UpdateAsync(AssignmentToUpdate);
                await _context.SaveChangesAsync();

                var updatedDto = _mapper.Map<AssignmentDTO>(AssignmentToUpdate);


                return ResponseHandler.Updated(updatedDto);
            }
            catch (Exception ex)
            {
                // Handle exception
                return ResponseHandler.BadRequest<AssignmentDTO>($"An error occurred while updating material: {ex.Message}");
            }
        }

        #endregion

        #region GetAll
        public async Task<Response<ICollection<AssignmentDTO>>> GetAllAssignmentsAsync(string sort_by, string search_term)
        {
            try
            {
                var assignments = await _unitOfWork.Assignments.GetAllAsync();
                if (assignments == null)
                {
                    return ResponseHandler.NotFound<ICollection<AssignmentDTO>>("There are no assignments yet");
                }
                var assignmentDtos = _mapper.Map<ICollection<AssignmentDTO>>(assignments);
                if (!string.IsNullOrEmpty(sort_by))
                {
                    switch (sort_by.ToLower())
                    {
                        case "title":
                            assignmentDtos = assignmentDtos.OrderBy(a => a.Title).ToList();
                            break;
                        case "title desc":
                            assignmentDtos = assignmentDtos.OrderByDescending(a => a.Title).ToList();
                            break;
                        case "date":
                            assignmentDtos = assignmentDtos.OrderBy(a => a.Date).ToList();
                            break;
                        case "date desc":
                            assignmentDtos = assignmentDtos.OrderByDescending(a => a.Date).ToList();
                            break;
                        case "active first":
                            var currentDate = DateTime.UtcNow.ToLocalTime();
                            assignmentDtos = assignmentDtos.OrderByDescending(a => a.Start <= currentDate && a.End >= currentDate).ThenBy(a => a.Title).ToList();
                            break;
                        case "not active":
                            var currentDate2 = DateTime.UtcNow.ToLocalTime();
                            assignmentDtos = assignmentDtos.OrderBy(a => a.Start <= currentDate2 && a.End >= currentDate2).ThenBy(a => a.Title).ToList();
                            break;
                        default:
                            assignmentDtos = assignmentDtos.OrderBy(a => a.Title).ToList();
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(search_term))
                {
                    assignmentDtos = assignmentDtos.Where(a => a.Title.Contains(search_term, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                return ResponseHandler.Success(assignmentDtos);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<AssignmentDTO>>($"An error occurred while retrieving materials: {ex.Message}");
            }
        }
        #endregion

        #region Get Assignment By ID
        public async Task<Response<AssignmentDTO>> GetAssignmentByIdAsync(int AssignmentId)
        {
            try
            {
                var assignment = await _unitOfWork.Assignments.GetByIdAsync(AssignmentId);
                if (assignment == null)
                {
                    return ResponseHandler.NotFound<AssignmentDTO>("There is no such Assignment");
                }

                var assignmentDto = _mapper.Map<AssignmentDTO>(assignment);
                return ResponseHandler.Success(assignmentDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<AssignmentDTO>($"An error occurred while retrieving Assignment: {ex.Message}");
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
                return ResponseHandler.Success(viewAssignmentDTOs);

            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAssignmentDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetAssignmentsByCreator
        public async Task<Response<ICollection<AssignmentDTO>>> GetAssignmentsByCreator(string sort_by, string search_term)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var Assignments = await _unitOfWork.Assignments.GetWhereSelectAsync(v => v.UserId == user.Id, v => v.Id);

                if (Assignments is null)
                    return ResponseHandler.NotFound<ICollection<AssignmentDTO>>("There are No Assignments yet");
                ICollection<AssignmentDTO> AssignmentsDto = new List<AssignmentDTO>();
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
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.Title).ToList();
                            break;
                        case "title desc":
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.Title).ToList();
                            break;
                        case "date":
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.Date).ToList();
                            break;
                        case "date desc":
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.Date).ToList();
                            break;
                        case "active first":
                            var currentDate = DateTime.UtcNow.ToLocalTime();
                            AssignmentsDto = AssignmentsDto.OrderByDescending(a => a.Start <= currentDate && a.End >= currentDate).ThenBy(a => a.Title).ToList();
                            break;
                        case "not active":
                            var currentDate2 = DateTime.UtcNow.ToLocalTime();
                            AssignmentsDto = AssignmentsDto.OrderBy(a => a.Start <= currentDate2 && a.End >= currentDate2).ThenBy(a => a.Title).ToList();
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
                return ResponseHandler.BadRequest<ICollection<AssignmentDTO>>($"An Error Occurred, {Ex}");
            }


        }

        #endregion

        #region Delete Delete All Assignments
        public async Task<Response<ICollection<AssignmentDTO>>> DeleteManyAsync(List<int> Ids)
        {
            try
            {
                var assignments = await GetAssignmentByIdAsync(Ids); 
                if (assignments is null || !assignments.Any())
                    return ResponseHandler.NotFound<ICollection<AssignmentDTO>>();

                await _unitOfWork.Assignments.DeleteRangeAsync(assignments);
                return ResponseHandler.Deleted<ICollection<AssignmentDTO>>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AssignmentDTO>>($"An Error Occured, {Ex}");
            }
        }
        #endregion

        #region Methods
        private async Task<ICollection<Assignment>> GetAssignmentByIdAsync(IEnumerable<int> Ids)
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
    }
}
