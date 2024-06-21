namespace ELearn.Application.DTOs.QuestionDTOs
{
    public class QuestionDTO
    {
        public required string Text { get; set; }
        //file
        public required string Option1 { get; set; }
        public required string Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string? CorrectOption { get; set; }
        public double? Grade { get; set; }
    }
}
