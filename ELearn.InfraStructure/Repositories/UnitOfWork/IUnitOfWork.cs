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
        IBaseRepository<Option> Options { get; }
        IBaseRepository<Post> Posts { get; }
        IBaseRepository<Question> Questions { get; }
        IBaseRepository<Quiz> Quizziz { get; }
        IBaseRepository<React> Reacts { get; }
        IBaseRepository<Survey> Surveys { get; }
        IBaseRepository<UserAssignment> UserAssignments { get; }
        IBaseRepository<UserGroup> UserGroups { get; }
        IBaseRepository<UserAnswerQuestion> UserQuestions { get; }
        IBaseRepository<UserSurvey> UserSurveys { get; }
        IBaseRepository<UserVoting> UserVotings { get; }
        IBaseRepository<Voting> Votings { get; }
        #endregion
        int complete();
    }
}
