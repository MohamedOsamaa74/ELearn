using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region props
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IBaseRepository<ApplicationUser> Users { get; private set; }
        public IBaseRepository<Announcement> Announcments { get; private set; }
        public IBaseRepository<Assignment> Assignments { get; private set; }
        public IBaseRepository<Comment> Comments { get; private set; }
        public IBaseRepository<Department> Departments { get; private set; }
        public IBaseRepository<FileEntity> Files { get; private set; }
        public IBaseRepository<Group> Groups { get; private set; }
        public IBaseRepository<GroupAnnouncment> GroupAnnouncments { get; private set; }
        public IBaseRepository<GroupSurvey> GroupSurveys { get; private set; }
        public IBaseRepository<GroupVoting> GroupVotings { get; private set; }
        public IBaseRepository<Material> Materials { get; private set; }
        public IBaseRepository<Message> Messages { get; private set; }
        public IBaseRepository<Option> Options { get; private set; }
        public IBaseRepository<Post> Posts { get; private set; }
        public IBaseRepository<Question> Questions { get; private set; }
        public IBaseRepository<Quiz> Quizziz { get; private set; }
        public IBaseRepository<React> Reacts { get; private set; }
        public IBaseRepository<Survey> Surveys { get; private set; }
        public IBaseRepository<UserAssignment> UserAssignments { get; private set; }
        public IBaseRepository<UserGroup> UserGroups { get; private set; }
        public IBaseRepository<UserAnswerQuestion> UserQuestions { get; private set; }
        public IBaseRepository<UserSurvey> UserSurveys { get; private set; }
        public IBaseRepository<UserVoting> UserVotings { get; private set; }
        public IBaseRepository<Voting> Votings { get; private set; }

        #endregion

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Users = new BaseRepository<ApplicationUser>(context, userManager);
            Announcments = new BaseRepository<Announcement>(context, userManager);
            Assignments = new BaseRepository<Assignment>(context, userManager);
            Comments = new BaseRepository<Comment>(context, userManager);
            Departments = new BaseRepository<Department>(context, userManager);
            Files = new BaseRepository<FileEntity>(context, userManager);
            Groups = new BaseRepository<Group>(context, userManager);
            GroupAnnouncments = new BaseRepository<GroupAnnouncment>(context, userManager);
            GroupSurveys = new BaseRepository<GroupSurvey>(context, userManager);
            GroupVotings = new BaseRepository<GroupVoting>(context, userManager);
            Materials = new BaseRepository<Material>(context, userManager);
            Messages = new BaseRepository<Message>(context, userManager);
            Options = new BaseRepository<Option>(context, userManager);
            Posts = new BaseRepository<Post>(context, userManager);
            Questions = new BaseRepository<Question>(context, userManager);
            Quizziz = new BaseRepository<Quiz>(context, userManager);
            Reacts = new BaseRepository<React>(context, userManager);
            Surveys = new BaseRepository<Survey>(context, userManager);
            UserAssignments = new BaseRepository<UserAssignment>(context, userManager);
            UserSurveys = new BaseRepository<UserSurvey>(context, userManager);
            UserSurveys = new BaseRepository<UserSurvey>(context, userManager);
            UserVotings = new BaseRepository<UserVoting>(context, userManager);
            Votings = new BaseRepository<Voting>(context, userManager);
        }

        int IUnitOfWork.complete() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}
