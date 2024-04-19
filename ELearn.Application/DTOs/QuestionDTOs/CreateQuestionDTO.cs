namespace ELearn.Application.DTOs.QuestionDTOs
{
    public class CreateQuestionDTO
    {
        public required string Text { get; set; }
        public ICollection<string> Options { get; set; } = [];
        public string? CorrectOption { get; set; }
    }
}
