using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class QuestionQuiz
    {
        [Key]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public virtual Question Question { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}