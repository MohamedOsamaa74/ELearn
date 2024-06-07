using ELearn.Application.DTOs.MaterialDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IMaterialService
    {
        public Task<Response<MaterialDTO>> AddMaterialAsync(int GroupId, AddMaterialDTO Model);
        public Task<Response<AddMaterialDTO>> DeleteMaterialAsync(int Id);
        public Task<Response<UpdateMaterialDTO>> UpdateMaterialAsync(int materialId, UpdateMaterialDTO Model);
        public Task<Response<AddMaterialDTO>> GetMaterialByIdAsync(int materialId);
        public Task<Response<IEnumerable<UpdateMaterialDTO>>> GetAllMaterialsAsync();
        public Task<Response<ICollection<MaterialDTO>>> GetAllMaterialsFromGroupAsync(int groupId);
      
    }
}