using ELearn.Application.DTOs.MaterialDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Application.Services;
using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;


namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        #region Fields
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService MaterialService)
        {
            _materialService = MaterialService;
        }
        #endregion

        #region Add Material
        [HttpPost("{GroupId:int}/AddMaterial")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> AddMaterial(int GroupId, [FromForm] AddMaterialDTO Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _materialService.AddMaterialAsync(GroupId, Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Get Mateial By ID
        [HttpGet("GetMaterialById/{materialId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMaterialById(int materialId)
        {
            var response = await _materialService.GetMaterialByIdAsync(materialId);
            return this.CreateResponse(response);
        }
        #endregion

        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _materialService.GetAllMaterialsAsync();

            return this.CreateResponse(response);
        }
        #endregion

        #region GetAllFromGroup
        
        [HttpGet("GetAllFromGroup/{groupId:int}")]
        public async Task<IActionResult> GetAllFromGroup(int groupId)
        {
            var response = await _materialService.GetAllMaterialsFromGroupAsync(groupId);
            return this.CreateResponse(response);
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
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteMaterial(int MaterialId)
        {
            var response = await _materialService.DeleteMaterialAsync(MaterialId);
            return this.CreateResponse(response);
        }
        #endregion

        #region update Material
        [HttpPut("UpdateMaterial/{MaterialId:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateMaterial(int MaterialId,[FromBody] UpdateMaterialDTO Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _materialService.UpdateMaterialAsync(MaterialId, Model);
            return this.CreateResponse(response);
        }

      
        #endregion        
    }
}
