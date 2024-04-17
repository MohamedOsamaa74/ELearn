using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.QuestionDTOs
{
    public class CreateQuestionDTO
    {
        public required string Text { get; set; }
        public char? CorrectOption { get; set; }
        public ICollection<string> Options { get; set; } = new HashSet<string>();
    }
}
