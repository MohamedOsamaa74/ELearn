namespace ELearn.Application.DTOs.QuestionDTOs
{
    public class QuestionAnswerDTO
    {
        public int QuestionId { get; set; }
        public required string Option { get; set; }
        public double? Score { get; set; }
    }
}