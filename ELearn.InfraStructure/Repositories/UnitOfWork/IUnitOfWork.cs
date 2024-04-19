using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        #region props
        IBaseRepository<ApplicationUser> Users { get; }
        IBaseRepository<Announcement> Announcments { get; }
        IBaseRepository<Assignment> Assignments { get; }
        IBaseRepository<Comment> Comments { get; }
        IBaseRepository<Department> Departments { get; }
        IBaseRepository<FileEntity> Files { get; }
        IBaseRepository<Group> Groups { get; }
        IBaseRepository<GroupAnnouncment> GroupAnnouncments { get; }
        IBaseRepository<GroupSurvey> GroupSurveys { get; }
        IBaseRepository<GroupVoting> GroupVotings { get; }
        IBaseRepository<Material> Materials { get; }
        IBaseRepository<Message> Messages { get; }
        //IBaseRepository<Option> Options { get; }
        IBaseRepository<Post> Posts { get; }
        IBaseRepository<Question> Questions { get; }
        IBaseRepository<Quiz> Quizziz { get; }
        IBaseRepository<React> Reacts { get; }
        IBaseRepository<Survey> Surveys { get; }
        IBaseRepository<UserAnswerAssignment> UserAnswerAssignments { get; }
        IBaseRepository<UserGroup> UserGroups { get; }
        IBaseRepository<UserAnswerQuiz> UserAnswerQuizziz { get; }
        IBaseRepository<UserAnswerQuestion> UserAnswerQuestions { get; }
        IBaseRepository<UserAnswerSurvey> UserAnswerSurveys { get; }
        IBaseRepository<UserAnswerVoting> UserAnswerVotings { get; }
        IBaseRepository<Voting> Votings { get; }
        #endregion
        int complete();
    }
}
