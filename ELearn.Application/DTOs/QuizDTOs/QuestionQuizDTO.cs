using ELearn.Application.DTOs.QuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuizDTOs
{
    public class QuestionQuizDTO
    {
        public int QuestionId { get; set; }
        public required string Option { get; set; }
        
    }
}
