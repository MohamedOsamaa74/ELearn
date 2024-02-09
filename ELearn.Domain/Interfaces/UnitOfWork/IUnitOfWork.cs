using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        #region props
        IBaseRepo<ApplicationUser> Users { get; }
        IBaseRepo<Announcement> Announcments { get; }
        IBaseRepo<Assignment> Assignments { get; }
        IBaseRepo<Comment> Comments { get; }
        IBaseRepo<Department> Departments { get; }
        IBaseRepo<Group> Groups { get; }
        IBaseRepo<GroupAnnouncment> GroupAnnouncments { get; }
        IBaseRepo<GroupSurvey> GroupSurveys { get; }
        IBaseRepo<GroupVoting> GroupVotings { get; }
        IBaseRepo<Material> Materials { get; }
        IBaseRepo<Message> Messages { get; }
        IBaseRepo<Option> Options { get; }
        IBaseRepo<Post> Posts { get; }
        IBaseRepo<Question> Questions { get; }
        IBaseRepo<Quiz> Quizziz { get; }
        IBaseRepo<React> Reacts { get; }
        IBaseRepo<Survey> Surveys { get; }
        IBaseRepo<UserAssignment> UserAssignments { get; }
        IBaseRepo<UserGroup> UserGroups { get; }
        IBaseRepo<UserQuestion> UserQuestions { get; }
        IBaseRepo<UserSurvey> UserSurveys { get; }
        IBaseRepo<UserVoting> UserVotings { get; }
        IBaseRepo<Voting> Votings { get; }
        #endregion
        int complete();
    }
}
