using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class SurveyDTO
    {
        public required string Text { get; set; }
        public DateTime CreateDate => DateTime.UtcNow.ToLocalTime();
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required ICollection<int> groups { get; set; }
        public required ICollection<string> Options { get; set; }
    }
}
