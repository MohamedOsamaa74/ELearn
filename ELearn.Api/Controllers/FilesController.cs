using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        #region Fields
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
        #endregion

        #region Get By Id
        [HttpGet("{Id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            var response = await _fileService.GetByIdAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get All
        [HttpGet("Get-All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _fileService.GetAllFilesAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] UploadFileDTO uploadFileDto)
        {
            var response = await _fileService.UploadFileAsync(uploadFileDto);
            return this.CreateResponse(response);
        }
        #endregion

        #region Delete
        [HttpDelete("{Id:int}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _fileService.DeleteAsync(Id);
            return this.CreateResponse(response);
        }
        #endregion

        #region ViewFile
        [HttpGet("ViewFile")]
        [Authorize]
        public async Task<IActionResult> ViewFile([FromQuery] DownloadFileDTO downloadFileDTO)
        {
            var type = await _fileService.GetFileType(downloadFileDTO.FileName);
            var response = await _fileService.DownloadFileAsync(downloadFileDTO);
            return File(response, type);
        }
        #endregion

        #region DownloadFile
        [HttpGet("DownloadFile")]
        [Authorize]
        public async Task<IActionResult> DownloadFile([FromQuery] DownloadFileDTO downloadFileDTO)
        {
            var type = await _fileService.GetFileType(downloadFileDTO.FileName);
            var response = await _fileService.DownloadFileAsync(downloadFileDTO);
            return File(response, type, downloadFileDTO.FileName);
        }
        #endregion
    }
}