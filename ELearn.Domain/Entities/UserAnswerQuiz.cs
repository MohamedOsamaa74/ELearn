using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class UserAnswerQuiz
    {
        public int Id { get; set; }
        public static DateTime AnswerDate => DateTime.UtcNow.ToLocalTime();
        public required int Grade { get; set; }

        #region ForeignKeys
        public required string UserId { get; set; }
        public required int QuizId { get; set; }
        #endregion

        #region Relations
        public virtual ApplicationUser User { get; set; }
        public virtual Quiz Quiz { get; set; }
        #endregion
    }
}
