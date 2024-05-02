using ELearn.Application.DTOs.QuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuizDTOs
{
    public class ViewQuizDTO
    {
        public required string title { get; set; }
        public required int Grade { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual required ICollection<ViewQuizDTO> Questions { get; set; } = new HashSet<ViewQuizDTO>();
    }
}
