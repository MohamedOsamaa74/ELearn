using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Image { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Address { get; set; }
        public required string Nationality { get; set; }
        public required string NId { get; set; }
        public string? Grade { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }

        #region Foreign Key
        public int DepartmentId { get; set; }
        #endregion

        #region Navigation Property
        public React? React { get; set; }
        public virtual Department Department { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
        public ICollection<FileEntity> Files { get;} = new List<FileEntity>();
        public ICollection<Message>? SentMessages { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
        public ICollection<Voting>? Votings { get; set; }
        public ICollection<UserAnswerVoting>? UserVoting { get; set; }
        public ICollection<Survey>? Surveys { get; set; }
        public ICollection<UserAnswerSurvey>? UserSurvey { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Announcement>? Announcements { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<UserAnswerQuestion>? UserQuestion { get; set; }
        public ICollection<Question>? Question { get; set; }

        //user create many groups
        public virtual ICollection<Group>? CreatedGroups { get; set; } = new HashSet<Group>();

        //users in groups
        public virtual ICollection<Group>? MyGroups { get; set; } = new HashSet<Group>();
        public virtual ICollection<UserGroup>? UserGroups { get; set; } = new HashSet<UserGroup>();

        //many to many task
        public ICollection<UserAnswerAssignment>? UserAssignment { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        #endregion

    }
}
