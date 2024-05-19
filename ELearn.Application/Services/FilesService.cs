using AutoMapper;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace ELearn.Application.Services
{
    public class FilesService : IFileService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public FilesService(IUnitOfWork unitOfWork, IUserService userService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _mapper = mapper;
        }
        #endregion

        #region UploadFile
        public async Task<Response<FileDTO>> UploadFileAsync(UploadFileDTO fileDTO)
        {
            try
            {
                var folderName = fileDTO.FolderName;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileDTO.File.FileName);

                var folderPath = Path.Combine(_env.WebRootPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileDTO.File.CopyToAsync(stream);
                }
                var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                          $"{_httpContextAccessor.HttpContext.Request.Host}" +
                          $"/api/files";
                var viewUrl = $"{url}/ViewFile/{folderName}/{fileName}";
                var downloadUrl = $"{url}/DownloadFile/{folderName}/{fileName}";
                var file = new FileEntity()
                {
                    Title = fileDTO.File.FileName,
                    FileName = fileName,
                    FolderName = folderName,
                    FilePath = filePath,
                    ViewUrl = viewUrl,
                    DownloadUrl = downloadUrl,
                    Type = fileDTO.File.ContentType,
                    UserId = await _userService.GetCurrentUserIDAsync(),
                    AnnouncementId = fileDTO.FolderName == "Announcements" ? fileDTO.ParentId : null,
                    MaterialId = fileDTO.FolderName == "Materials" ? fileDTO.ParentId : null,
                    PostId = fileDTO.FolderName == "Posts" ? fileDTO.ParentId : null
                };
                await _unitOfWork.Files.AddAsync(file);
                var dto = _mapper.Map<FileDTO>(file);
                return ResponseHandler.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<FileDTO>($"An Error Occurred While Uploading The File, {ex.Message}");
            }
        }
        #endregion

        #region DownloadFile
        public async Task<byte[]> DownloadFileAsync(DownloadFileDTO downloadFileDTO)
        {
            var filePath = Path.Combine(_env.WebRootPath, downloadFileDTO.FolderName, downloadFileDTO.FileName);
            if (filePath is null)
            {
                return null;
            }
            return await File.ReadAllBytesAsync(filePath);
        }
        #endregion

        #region GetFileType
        public async Task<string> GetFileType(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        #endregion

        #region DeleteFile
        public async Task<Response<FileDTO>> DeleteAsync(int Id)
        {
            try
            {
                var file = await _unitOfWork.Files.GetByIdAsync(Id);
                if (file == null)
                {
                    return ResponseHandler.NotFound<FileDTO>("The File Does Not Exist");
                }
                var filePath = GetFilePath(file.FolderName, file.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    await _unitOfWork.Files.DeleteAsync(file);
                }
                return ResponseHandler.Deleted<FileDTO>();
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<FileDTO>($"An Error Occurred While Deleting The File, {ex.Message}");
            }
        }
        #endregion

        #region GetFilePath
        private string GetFilePath(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            return null;
        }
        #endregion
        
        #region GetById
        public async Task<Response<FileDTO>> GetByIdAsync(int Id)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(Id);
            if (file == null)
            {
                return ResponseHandler.NotFound<FileDTO>("The File Does Not Exist");
            }
            return ResponseHandler.Success(new FileDTO()
            {
                Title = file.Title,
                FileName = file.FileName,
                FolderName = file.FolderName,
                FilePath = file.FilePath,
                ViewUrl = file.ViewUrl,
                Type = file.Type,
                UserId = file.UserId,
                DownloadUrl = file.DownloadUrl,
                Creation = file.CreationDate
            });
        }
        #endregion

        #region GetByFileName
        public async Task<Response<FileDTO>>GetByFileNameAsync(string fileName)
        {
            try
            {
                var file = await _unitOfWork.Files.GetWhereSingleAsync(f => f.FileName == fileName);
                if (file is null)
                    return ResponseHandler.NotFound<FileDTO>("File Does Not Exist");
                var fileDTO = _mapper.Map<FileDTO>(file);
                return ResponseHandler.Success(fileDTO);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<FileDTO>("An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetAllFiles
        public async Task<Response<ICollection<FileDTO>>> GetAllFilesAsync()
        {
            var files = await _unitOfWork.Files.GetAllAsync();
            if (files == null)
            {
                return ResponseHandler.NotFound<ICollection<FileDTO>>("There Are No Files");
            }
            var filesDTO = _mapper.Map<ICollection<FileDTO>>(files);
            return ResponseHandler.ManySuccess(filesDTO);
        }
        #endregion
    }
}