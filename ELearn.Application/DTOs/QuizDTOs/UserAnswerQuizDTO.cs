using ELearn.Application.DTOs.QuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuizDTOs
{
    public class UserAnswerQuizDTO
    {
        public int QuizId { get; set; }
        public ICollection<QuestionQuizDTO> Answers { get; set; } = [];
    }
}
