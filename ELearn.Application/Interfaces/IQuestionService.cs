using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.Helpers.Response;

namespace ELearn.Application.Interfaces
{
    public interface IQuestionService
    {
        public Task<Response<QuestionDTO>> CreateNewAsync(QuestionDTO Model, string Parent, int ParentId);
        public Task<Response<QuestionDTO>> UpdateAsync(int Id, QuestionDTO Model);
        public Task<Response<QuestionDTO>> DeleteAsync(int Id);
        public Task<Response<QuestionDTO>> GetByIdAsync(int Id);
        public Task<Response<QuestionAnswerDTO>>RecieveStudentAnswerAsync(QuestionAnswerDTO Model);
        public Task<Response<ICollection<QuestionAnswerDTO>>> GetStudentAnswersAsync(string Parent, int ParentId, string UserId);
        public Task<Response<ICollection<QuestionDTO>>> GetQuestionsByQuizIdAsync(int quizId);
    }
}
