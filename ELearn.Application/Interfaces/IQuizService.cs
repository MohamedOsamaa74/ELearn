using ELearn.Application.DTOs.AnnouncementDTOs;
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
        public Task<Response<ViewQuizDTO>> GetAllQuizzesAsync();
    }
}
