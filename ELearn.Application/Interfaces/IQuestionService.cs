using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IQuestionService
    {
        public Task<Response<CreateQuestionDTO>> CreateQuestion(CreateQuestionDTO Model);
        public Task<Response<CreateQuestionDTO>> UpdateQuestion(CreateQuestionDTO Model, int questionID);   
    }
}
