using AutoMapper;
using ELearn.Application.DTOs;
using ELearn.Application.DTOs.AnnouncementDTOs;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearn.Application.Services
{
    public class AnnouncementService : IAnnouncementService
    {

        #region Fields
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public AnnouncementService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IFileService fileService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService;
        }
        #endregion

        #region GetByID
        public async Task<Response<ViewAnnouncementDTO>> GetByIdAsync(int id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(id);
            if (announcement == null)
                return ResponseHandler.NotFound<ViewAnnouncementDTO>("Announcement Not Found");
            try
            {
                var groups = await GetAnnouncementGroupsAsync(id);
                var viewAnnouncementDTO = _mapper.Map<ViewAnnouncementDTO>(announcement);
                viewAnnouncementDTO.Groups = (ICollection<int>)groups;
                viewAnnouncementDTO.FilesUrls = await GetAnnouncementFiles(id);
                return ResponseHandler.Success(viewAnnouncementDTO);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewAnnouncementDTO>($"An Error Occurred While Proccessing The Request, {ex}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Response<ICollection<ViewAnnouncementDTO>>> GetAllAnnouncementsAsync(string sort_by,string search_term)
        {
            try
            {
                var announcements = await _unitOfWork.Announcments.GetAllAsync();

                if (announcements == null || !announcements.Any())
                {
                    return ResponseHandler.NotFound<ICollection<ViewAnnouncementDTO>>("No announcements found");
                }
                var viewAnnouncementDTOs = new List<ViewAnnouncementDTO>();
                foreach (var item in announcements)
                {
                    var dto = _mapper.Map<ViewAnnouncementDTO>(item);
                    dto.Groups = (ICollection<int>)await GetAnnouncementGroupsAsync(item.Id);
                    dto.FilesUrls = await GetAnnouncementFiles(item.Id);
                    viewAnnouncementDTOs.Add(dto);
                }

                if (!string.IsNullOrEmpty(sort_by))
                {
                    switch (sort_by.ToLower())
                    {
                        case "text":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.Text).ToList();
                            break;
                        case "text desc":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderByDescending(a => a.Text).ToList();
                            break;
                        case "date":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.CreationDate).ToList();
                            break;
                        case "date desc":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderByDescending(a => a.CreationDate).ToList();
                            break;
                        default:
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.Text).ToList();
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(search_term))
                {
                    viewAnnouncementDTOs = viewAnnouncementDTOs.Where(a => a.Text.Contains(search_term, StringComparison.OrdinalIgnoreCase)).ToList();
                }


                return ResponseHandler.ManySuccess(viewAnnouncementDTOs);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAnnouncementDTO>>($"An error occurred: {ex.Message}");
            }
        }
        #endregion

        #region CreateNew
        public async Task<Response<ViewAnnouncementDTO>> CreateNewAsync(UploadAnnouncementDTO Model)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIDAsync();
                var announcement = _mapper.Map<Announcement>(Model);
                announcement.UserId = userId;
                var validate = new AnnouncementValidation().Validate(announcement);
                if (!validate.IsValid)
                    return ResponseHandler.BadRequest<ViewAnnouncementDTO>(null, validate.Errors.Select(e => e.ErrorMessage).ToList());
                await _unitOfWork.Announcments.AddAsync(announcement);

                var ViewAnnouncement = _mapper.Map<ViewAnnouncementDTO>(announcement);
                ViewAnnouncement.UserId = userId;
                ViewAnnouncement.Groups = (ICollection<int>)Model.Groups;

                List<string> ViewUrls = [];
                if (Model.Files != null && Model.Files.Count != 0)
                {
                    foreach (var file in Model.Files)
                    {
                        var uploadFileDto = new UploadFileDTO()
                        { File = file, FolderName = "Announcements", ParentId = announcement.Id};
                        var newFile = await _fileService.UploadFileAsync(uploadFileDto);
                        ViewUrls.Add(newFile.Data.ViewUrl);
                    }
                    ViewAnnouncement.FilesUrls = ViewUrls;
                }
                await SendToGroupsAsync((ICollection<int>)Model.Groups, announcement.Id);
                return ResponseHandler.Created(ViewAnnouncement);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ViewAnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }
        #endregion

        #region Delete
        public async Task<Response<UploadAnnouncementDTO>> DeleteAsync(int Id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(Id);
            if (announcement is null)
                return ResponseHandler.NotFound<UploadAnnouncementDTO>();
            try
            {
                await _unitOfWork.Announcments.DeleteAsync(announcement);
                return ResponseHandler.Deleted<UploadAnnouncementDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UploadAnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }
        #endregion

        #region Update
        public async Task<Response<UploadAnnouncementDTO>> UpdateAsync(UploadAnnouncementDTO Model, int Id)
        {
            var announcement = await _unitOfWork.Announcments.GetByIdAsync(Id);
            if (announcement is null)
                return ResponseHandler.NotFound<UploadAnnouncementDTO>();
            try
            {
                announcement.Text = Model.Text;
                foreach (var groupId in Model.Groups)
                {
                    var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
                    if (!await _unitOfWork.GroupAnnouncments.FindIfExistAsync(ga => ga.GroupId == groupId && ga.AnnouncementId == Id))
                    {
                        await _unitOfWork.GroupAnnouncments.AddAsync(new GroupAnnouncment() { AnnouncementId = Id, GroupId = groupId });
                    }
                }
                await _unitOfWork.Announcments.UpdateAsync(announcement);
                return ResponseHandler.Updated(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<UploadAnnouncementDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }
        #endregion

        #region GetFromGroups
        public async Task<Response<ICollection<ViewAnnouncementDTO>>> GetFromUserGroupsAsync(string sort_by, string search_term)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
            
                var announcements = await _context.Announcements
                    .Include(a => a.GroupAnnouncements)
                    .Where(a => a.GroupAnnouncements.Any(ga => 
                            ga.GroupId == ga.Group.Id && 
                            ga.Group.UsersInGroup.Any(ug => ug.Id == user.Id)))
                    .ToListAsync();
                if (announcements is null || !announcements.Any())
                    return ResponseHandler.NotFound<ICollection<ViewAnnouncementDTO>>();

                var viewAnnouncementDTOs = new List<ViewAnnouncementDTO>();
                foreach (var item in announcements)
                {
                    var dto = _mapper.Map<ViewAnnouncementDTO>(item);
                    dto.Groups = (ICollection<int>)await GetAnnouncementGroupsAsync(item.Id);
                    dto.FilesUrls = await GetAnnouncementFiles(item.Id);
                    viewAnnouncementDTOs.Add(dto);
                }

                if (!string.IsNullOrEmpty(sort_by))
                {
                    switch (sort_by.ToLower())
                    {
                        case "text":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.Text).ToList();
                            break;
                        case "text desc":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderByDescending(a => a.Text).ToList();
                            break;
                        case "date":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.CreationDate).ToList();
                            break;
                        case "date desc":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderByDescending(a => a.CreationDate).ToList();
                            break;
                        default:
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.Text).ToList();
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(search_term))
                {
                    viewAnnouncementDTOs = viewAnnouncementDTOs.Where(a => a.Text.Contains(search_term, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                return ResponseHandler.ManySuccess(viewAnnouncementDTOs);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAnnouncementDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetByCreator
        public async Task<Response<ICollection<ViewAnnouncementDTO>>> GetByCreatorAsync(string sort_by, string search_term)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();

                var announcements = await _unitOfWork.Announcments
                                   .GetWhereAsync(a => a.UserId == user.Id);

                if (announcements is null || !announcements.Any())
                    return ResponseHandler.NotFound<ICollection<ViewAnnouncementDTO>>("You Haven't sent Any Announcements");
                var viewAnnouncementDTOs = new List<ViewAnnouncementDTO>();
                foreach (var item in announcements)
                {
                    var dto = _mapper.Map<ViewAnnouncementDTO>(item);
                    dto.Groups = (ICollection<int>)await GetAnnouncementGroupsAsync(item.Id);
                    dto.FilesUrls = await GetAnnouncementFiles(item.Id);
                    viewAnnouncementDTOs.Add(dto);
                }
                if (!string.IsNullOrEmpty(sort_by))
                {
                    switch (sort_by.ToLower())
                    {
                        case "text":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.Text).ToList();
                            break;
                        case "text desc":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderByDescending(a => a.Text).ToList();
                            break;
                        case "date":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.CreationDate).ToList();
                            break;
                        case "date desc":
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderByDescending(a => a.CreationDate).ToList();
                            break;
                        default:
                            viewAnnouncementDTOs = viewAnnouncementDTOs.OrderBy(a => a.Text).ToList();
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(search_term))
                {
                    viewAnnouncementDTOs = viewAnnouncementDTOs.Where(a => a.Text.Contains(search_term, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                return ResponseHandler.ManySuccess(viewAnnouncementDTOs);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<ViewAnnouncementDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region DeleteMany
        public async Task<Response<ICollection<UploadAnnouncementDTO>>> DeleteManyAsync(List<int>Ids)
        {
            try
            {
                var announcements = await GetAnnouncementsAsync(Ids);
                if (announcements is null || !announcements.Any())
                    return ResponseHandler.NotFound<ICollection<UploadAnnouncementDTO>>();

                await _unitOfWork.Announcments.DeleteRangeAsync(announcements);
                return ResponseHandler.Deleted<ICollection<UploadAnnouncementDTO>>();
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<UploadAnnouncementDTO>>($"An Error Occured, {Ex}");
            }
        }
        #endregion

        #region PrivateMethods
        private async Task<IEnumerable<int>> GetAnnouncementGroupsAsync(int announcementId)
        => await _unitOfWork.GroupAnnouncments
            .GetWhereSelectAsync(g => g.AnnouncementId == announcementId, g => g.GroupId);

        private async Task SendToGroupsAsync(ICollection<int> Groups, int announcementId)
        {
            foreach (var groupId in Groups)
            {
                GroupAnnouncment NewGroupAnnouncement = new GroupAnnouncment()
                {
                    GroupId = groupId,
                    AnnouncementId = announcementId
                };
                await _unitOfWork.GroupAnnouncments.AddAsync(NewGroupAnnouncement);
            }
        }

        private async Task<ICollection<Announcement>> GetAnnouncementsAsync(IEnumerable<int> Ids)
        {
            List<Announcement> announcements = new List<Announcement>();
            foreach (var Id in Ids)
            {
                var entity = await _unitOfWork.Announcments.GetByIdAsync(Id);
                if (entity != null)
                    announcements.Add(entity);
            }
            return announcements;
        }

        private async Task<ICollection<string>> GetAnnouncementFiles(int announcementId)
        {
            var files = await _unitOfWork.Files
                .GetWhereAsync(f => f.AnnouncementId == announcementId);

            if (files is null || !files.Any())
                return null;
            
            return files.Select(f => f.ViewUrl).ToList();
        }
        #endregion

    }
}
