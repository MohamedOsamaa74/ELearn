using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class QuestionSurvey
    {
        public int QuestionsId { get; set; }
        public int SurveyId { get; set; }
        public virtual required Survey Survey { get; set; }
        public virtual required Question Questions { get; set; }
    }
}