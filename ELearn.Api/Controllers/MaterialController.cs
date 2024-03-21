using ELearn.Application.DTOs;
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
        private readonly IMaterialService _materialService;
       
        public MaterialController(IMaterialService MaterialService)
        {
            _materialService = MaterialService;
           
        }
       
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
        
        [HttpGet("GetAllFromGroup")]
        public async Task<IActionResult> GetAllFromGroup(int id)
        {
            var response = await _materialService.GetAllMaterialsFromGroupAsync(id);
            return this.CreateResponse(response);
        }
        #endregion

        //#region Upload

        //[HttpPost("UploadMaterial")]
        //[Authorize(Roles = "Admin , Staff")]
        //public async Task<IActionResult> UploaldMaterial(AddMaterialDTO materialDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {

        //        var material = new Material
        //        {
        //            Title = materialDTO.Title,
        //            Week = materialDTO.Week,
        //            File = materialDTO.File,
        //            FilePath = await WriteFile(materialDTO.File),
        //            GroupId =1,
        //            UserId =

        //        };
        //        await _unitOfWork.Materials.AddAsync(material);
        //        return Ok(material.FilePath);

        //    }
        //}

        //private async Task<string> WriteFile(IFormFile file)
        //{
        //    string filename = "";
        //    try
        //    {
        //        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
        //        filename = DateTime.Now.Ticks.ToString() + extension;

        //        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");

        //        if (!Directory.Exists(filepath))
        //        {
        //            Directory.CreateDirectory(filepath);
        //        }

        //        var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", filename);
        //        using (var stream = new FileStream(exactpath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return filename;
        //}
        //#endregion

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
            
           
                var response = await _materialService.DeleteMaterialAsync(MaterialId);
                return this.CreateResponse(response);
            
        }
        #endregion



        #region Get Mateial By ID
        [HttpGet("GetMaterialById/{MaterialId:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetMaterialById(int materialId)
        {
            var response = await _materialService.GetMaterialByIdAsync(materialId);
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

        // view material in browser لسة مش معمول هنشوف تيم الفرونت 
    }
}
