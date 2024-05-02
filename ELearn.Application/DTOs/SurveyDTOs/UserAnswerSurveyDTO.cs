using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearn.Application.DTOs.QuestionDTOs;

namespace ELearn.Application.DTOs.SurveyDTOs
{
    public class UserAnswerSurveyDTO
    {
        public int SurveyId { get; set; }
        public ICollection<QuestionAnswerDTO> Answers { get; set; } = [];
    }
}
