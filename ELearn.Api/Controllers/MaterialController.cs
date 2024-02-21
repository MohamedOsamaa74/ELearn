using ELearn.Application.DTOs;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.UnitOfWork;
using ELearn.InfraStructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;


namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public MaterialController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = appDbContext;


        }
        //done
        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Materials.GetAllAsync(m => new { m.Title, m.Week, m.FilePath }));
        }

        #endregion

        [HttpGet("GetAllFromGroup")]
        public async Task<IActionResult> GetAllFromGroup(int id)
        {
            return Ok(await _context.Materials.Where(x => x.GroupId == id).ToListAsync());
        }

        #region Upload

        [HttpPost("UploadMaterial")]
        [Authorize(Roles = "Admin , Staff")]
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
                    Week = materialDTO.Week,
                    File = materialDTO.File,
                    FilePath = await WriteFile(materialDTO.File),
                    GroupId = 2, // Replace with current group ID
                    UserId = _userManager.GetUserId(User),

                };
                await _unitOfWork.Materials.AddAsync(material);
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


        #region Delete Mateial
        [HttpDelete("Delete/{MaterialId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMaterial(int MaterialId)
        {
            var Material = await _unitOfWork.Materials.GetByIdAsync(MaterialId);
            if (Material == null)
            {
                return BadRequest("Material not found.");
            }
            else
            {
                await _unitOfWork.Materials.DeleteAsync(Material);
                return Ok("Material Deleted Successfully");
            }
        }
        #endregion


        #region Get Mateial By ID
        [HttpGet("GetMaterialById/{MaterialId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMaterialById(int MaterialId)
        {
            try
            {
                var Material = await _unitOfWork.Materials.GetByIdAsync(MaterialId);
                if (Material == null)
                {
                    return NotFound($"Material with ID {MaterialId} not found");
                }
                return Ok(Material);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request");
            }

        }

        #endregion


        #region update Material
        [HttpPut("UpdateMaterial/{MaterialId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMaterial(int MaterialId, [FromBody] UpdatMaterialeDto updateDto)
        {
            try
            {
                var materialToUpdate = await _unitOfWork.Materials.GetByIdAsync(MaterialId);
                if (materialToUpdate == null)
                {
                    return NotFound($"Material with ID {MaterialId} not found");
                }

                // Update properties from the DTO
                materialToUpdate.Title = updateDto.Title;
                materialToUpdate.Link = updateDto.Link;
                materialToUpdate.Week = updateDto.Week;



                _unitOfWork.Materials.UpdateAsync(materialToUpdate);


                return Ok(materialToUpdate);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request");
            }




        }
        #endregion        

        // view material in browser لسة مش معمول هنشوف تيم الفرونت 
    }
}
