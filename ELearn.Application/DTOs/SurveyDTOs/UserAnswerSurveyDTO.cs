using ELearn.Application.DTOs.QuestionDTOs;

namespace ELearn.Application.DTOs.SurveyDTOs
{
    public class UserAnswerSurveyDTO
    {
        public int SurveyId { get; set; }
        public ICollection<QuestionAnswerDTO> Answers { get; set; } = [];
    }
}
