using ELearn.Application.DTOs;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _dbContext;

        public MaterialController(IUnitOfWork unitOfWork, AppDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;

        }
        //done
        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            return Ok(_dbContext.Materials.ToList());
        }

        #endregion

        #region Upload

        [HttpPost("UploadMaterial")]
        [Authorize(Roles ="Admin , Staff")]
        public async Task<IActionResult> UploaldMaterial(AddMaterialDTO materialDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var material = new Material
                {
                    Title = materialDTO.Title,
                    Link = materialDTO.Link,
                    Week = materialDTO.Week,
                    File = materialDTO.File,
                    FilePath= await WriteFile(materialDTO.File),
                    GroupId = 2, // Replace with current group ID
                    UserId = "0d67a794-84ea-4c9f-b963-05f00abb985c", // Replace with logic to get current user ID

                };
                _dbContext.Materials.Add(material);
                await _dbContext.SaveChangesAsync();
                return Ok(material.FilePath);

            }
        }
        
        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        } 
        #endregion

        #region DownloadFile
        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", filename);
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        } 
        #endregion
    }
}
