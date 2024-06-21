using ELearn.Application.DTOs.QuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuizDTOs
{
    public class QuizResultDTO
    {
        public int QuizId { get; set; }
        public string StudentName { get; set; }
        public double ?TotalScore { get; set; }
        public List <QuestionAnswerDTO> QuestionAnswers { get; set; }
    }
}
