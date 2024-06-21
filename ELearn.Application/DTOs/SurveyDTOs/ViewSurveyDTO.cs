using ELearn.Application.DTOs.QuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.SurveyDTOs
{
    public class ViewSurveyDTO
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public required string CreatorName { get; set; }
        public ICollection<QuestionDTO> Questions { get; set; } = [];
    }
}
