using ELearn.Application.DTOs;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.SurveyDTOs;
using ELearn.Application.Helpers.Response;

namespace ELearn.Application.Interfaces
{
    public interface ISurveyService
    {
        public Task<Response<ViewSurveyDTO>> CreateNewAsync(CreateSurveyDTO Model);
        public Task<Response<ViewSurveyDTO>> GetByIdAsync(int Id);
        public Task<Response<ICollection<ViewSurveyDTO>>> GetSurveysByCreator();
        public Task<Response<ICollection<ViewSurveyDTO>>> GetFromUserGroups();
        public Task<Response<ICollection<ViewSurveyDTO>>> GetAllAsync();
        public Task<Response<ICollection<ViewSurveyDTO>>> GetFromGroup(int GroupId);
        public Task<Response<CreateSurveyDTO>> DeleteAsync(int Id);
        public Task<Response<CreateSurveyDTO>> DeleteManyAsync(int[] Ids);
        //public Task<Response<CreateSurveyDTO>> UpdateAsync(int Id, CreateSurveyDTO Model);
        public Task<Response<UserAnswerSurveyDTO>> SubmitResponseAsync(UserAnswerSurveyDTO userAnswerDTO);
        public Task<Response<UserAnswerSurveyDTO>> GetUserAnswerAsync(int SurveyId, string UserId);
    }
}