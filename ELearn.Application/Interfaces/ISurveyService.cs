using ELearn.Application.DTOs;
using ELearn.Application.DTOs.QuestionDTOs;
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
        public Task<Response<ICollection<CreateSurveyDTO>>> GetFromGroup(int GroupId);
        public Task<Response<CreateSurveyDTO>> DeleteAsync(int Id);
        public Task<Response<CreateSurveyDTO>> DeleteManyAsync(int[] Ids);
        //public Task<Response<CreateSurveyDTO>> UpdateAsync(int Id, CreateSurveyDTO Model);
        public Task<Response<UserAnswerSurveyDTO>> RecieveStudentResponseAsync(UserAnswerSurveyDTO userAnswerDTO);
        public Task<Response<UserAnswerSurveyDTO>> GetUserAnswerAsync(int SurveyId, string UserId);
    }
}