using AutoMapper;
using ELearn.Application.DTOs.FileDTOs;
using ELearn.Application.DTOs.MaterialDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Services
{
    public class MaterialService : IMaterialService
    {
        #region Fields
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public MaterialService(AppDbContext context, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IFileService fileService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        #endregion

        #region Add Material
        public async Task<Response<ViewMaterialDTO>> AddMaterialAsync(int GroupId, AddMaterialDTO Model)
        {
            try
            {
                var material = _mapper.Map<Material>(Model);
                material.GroupId = GroupId;
                material.UserId = await _userService.GetCurrentUserIDAsync();
                await _unitOfWork.Materials.AddAsync(material);
                var fileDto = new UploadFileDTO
                {
                    File = Model.File,
                    FolderName = "Materials",
                    ParentId = material.Id
                };
                var file = await _fileService.UploadFileAsync(fileDto);
                if (!file.Succeeded)
                {
                    return ResponseHandler.BadRequest<ViewMaterialDTO>($"An error occurred while adding material: {file.Message}");
                }
                var viewMaterial = _mapper.Map<ViewMaterialDTO>(material);
                viewMaterial.ViewUrl = file.Data.ViewUrl;
                viewMaterial.DownloadUrl = file.Data.DownloadUrl;
                viewMaterial.Title = file.Data.Title;
                return ResponseHandler.Success(viewMaterial);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ViewMaterialDTO>($"An error occurred while adding material: {ex.Message}");
            }
        }
        #endregion
           
        #region Delete Material
        public async Task<Response<AddMaterialDTO>> DeleteMaterialAsync(int Id)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(Id);
            if (material is null)
                return ResponseHandler.NotFound<AddMaterialDTO>();
            try
            {
                await _unitOfWork.Materials.DeleteAsync(material);
                return ResponseHandler.Deleted<AddMaterialDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AddMaterialDTO>($"An Error Occurred While Proccessing The Request, {Ex}");
            }
        }

       
        #endregion

        #region Update Material
        public async Task<Response<UpdateMaterialDTO>> UpdateMaterialAsync(int MaterialId, UpdateMaterialDTO Model)
           
        {
            var materialToUpdate = await _unitOfWork.Materials.GetByIdAsync(MaterialId);
            if (materialToUpdate == null)
                return ResponseHandler.NotFound<UpdateMaterialDTO>();

            try
            {
                
                _mapper.Map(Model, materialToUpdate);

                
                await _unitOfWork.Materials.UpdateAsync(materialToUpdate);
                await _context.SaveChangesAsync();

                var updatedDto = _mapper.Map<UpdateMaterialDTO>(materialToUpdate);

                
                return ResponseHandler.Updated(updatedDto);
            }
            catch (Exception ex)
            {
                // Handle exception
                return ResponseHandler.BadRequest<UpdateMaterialDTO>($"An error occurred while updating material: {ex.Message}");
            }
        }

        #endregion

        #region Get Material By ID
        public async Task<Response<AddMaterialDTO>> GetMaterialByIdAsync(int materialId)
        {
            try
            {
                var material = await _unitOfWork.Materials.GetByIdAsync(materialId);
                if (material == null)
                {
                    return ResponseHandler.NotFound<AddMaterialDTO>();
                }

                var materialDto = _mapper.Map<AddMaterialDTO>(material);
                return ResponseHandler.Success(materialDto);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<AddMaterialDTO>($"An error occurred while retrieving material: {ex.Message}");
            }
        }
        #endregion

        #region GetAll
        public async Task<Response<IEnumerable<UpdateMaterialDTO>>> GetAllMaterialsAsync()
        {
            try
            {
                var materials = await _unitOfWork.Materials.GetAllAsync();
                var materialDtos = _mapper.Map<IEnumerable<UpdateMaterialDTO>>(materials);
                return ResponseHandler.Success(materialDtos);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<IEnumerable<UpdateMaterialDTO>>($"An error occurred while retrieving materials: {ex.Message}");
            }
        }
        #endregion

        #region Get All From Group
        public async Task<Response<ICollection<UpdateMaterialDTO>>> GetAllMaterialsFromGroupAsync(int groupId)
        {
            try
            {
                var materials = await _context.Materials
                    .Where(x => x.GroupId == groupId)
                    .ToListAsync();

                if (materials == null || !materials.Any())
                {
                    return ResponseHandler.NotFound<ICollection<UpdateMaterialDTO>>($"No materials found for group with ID {groupId}");
                }

                var materialDtos = _mapper.Map<ICollection<UpdateMaterialDTO>>(materials);
                return ResponseHandler.Success(materialDtos);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<ICollection<UpdateMaterialDTO>>($"An error occurred while retrieving materials: {ex.Message}");
            }
        }

        #endregion
    }
}