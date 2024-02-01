using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string? Image { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Address { get; set; }
        public required string Nationality { get; set; }
        public required string Religion { get; set; }
        public string? Grade { get; set; }

        #region Foreign Key
        public int DeptId { get; set; }
        #endregion

        #region Navigation Property
        public React? React { get; set; }
        public Department Department { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        
        public virtual ApplicationUser Receiver { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }

        public ICollection<Message>? SentMessages { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
        public ICollection<Voting>? Votings { get; set; }
        public ICollection<UserVoting>? UserVoting { get; set; }
        public ICollection<Survey>? Surveys { get; set; }
        public ICollection<UserSurvey>? UserSurvey { get; set; }
        public ICollection<UserQuestion>? UserQuestion { get; set; }
        public ICollection<Question> Question { get; set; }


        #endregion

    }
}
