using ELearn.Application.DTOs.QuestionDTOs;
using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuizDTOs
{
    public class CreateQuizDTO
    {
        public required string title { get; set; }
        public required int Grade { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual required ICollection<QuestionDTO> Questions { get; set; } = new HashSet<QuestionDTO>();

    }
}
