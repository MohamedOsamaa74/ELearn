using ELearn.Application.DTOs.QuestionDTOs;

namespace ELearn.Application.DTOs
{
    public class CreateSurveyDTO
    {
        public required string title { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required ICollection<int> GroupIds { get; set; } = new HashSet<int>();
        public required ICollection<QuestionDTO> Questions { get; set; } = new HashSet<QuestionDTO>();
    }
}
