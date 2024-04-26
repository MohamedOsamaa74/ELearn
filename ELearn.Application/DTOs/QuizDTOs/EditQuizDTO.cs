using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuizDTOs
{
    public class EditQuizDTO
    {
        public required string title { get; set; }
        public required int Grade { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
    }
}
