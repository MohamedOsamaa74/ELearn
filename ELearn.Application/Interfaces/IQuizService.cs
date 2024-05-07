using ELearn.Application.DTOs.AnnouncementDTOs;
using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.DTOs.QuizDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IQuizService
    {
        public Task<Response<CreateQuizDTO>> CreateNewAsync(CreateQuizDTO Model, int groupID);
        public Task<Response<EditQuizDTO>> UpdateQuizAsync(EditQuizDTO Model, int quizID);
        public Task<Response<ViewQuizDTO>> GetQuizByIdAsync(int quizId);
        public Task<Response<ICollection<ViewQuizDTO>>> GetAllQuizzesAsync();

        public Task<Response<CreateQuizDTO>> DeleteAsync(int Id);
        public  Task<Response<QuizResultDTO>> ReceiveStudentQuizResponsesAsync(QuizResultDTO userAnswerDto);
        public Task<Response<UserAnswerQuizDTO>> GetUserAnswerAsync(int quizId, string UserId);

        public Task<Response<List<QuizResultDTO>>> GetAllQuizResponsesAsync(int quizId);



    }
}
