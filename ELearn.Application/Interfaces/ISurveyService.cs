using ELearn.Application.DTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;

namespace ELearn.Application.Interfaces
{
    public interface ISurveyService
    {
        public Task<Response<CreateSurveyDTO>> CreateNewAsync(CreateSurveyDTO Model);
        public Task<Response<CreateSurveyDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<CreateSurveyDTO>>> GetSurveysByCreator();
        public Task<Response<ICollection<CreateSurveyDTO>>> GetFromUserGroups();
        public Task<Response<ICollection<CreateSurveyDTO>>> GetAllAsync();
        public Task<Response<ICollection<CreateSurveyDTO>>> GetFromGroups(int GroupId);
        public Task<Response<CreateSurveyDTO>> DeleteAsync(int Id);
    }
}
