using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.Base;
using ELearn.Domain.Interfaces.UnitOfWork;
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
        public IBaseRepo<ApplicationUser> Users { get; private set; }
        public IBaseRepo<Announcement> Announcments { get; private set; }
        public IBaseRepo<Assignment> Assignments { get; private set; }
        public IBaseRepo<Comment> Comments { get; private set; }
        public IBaseRepo<Department> Departments { get; private set; }
        public IBaseRepo<Group> Groups { get; private set; }
        public IBaseRepo<GroupAnnouncment> GroupAnnouncments { get; private set; }
        public IBaseRepo<GroupSurvey> GroupSurveys { get; private set; }
        public IBaseRepo<GroupVoting> GroupVotings { get; private set; }
        public IBaseRepo<Material> Materials { get; private set; }
        public IBaseRepo<Message> Messages { get; private set; }
        public IBaseRepo<Option> Options { get; private set; }
        public IBaseRepo<Post> Posts { get; private set; }
        public IBaseRepo<Question> Questions { get; private set; }
        public IBaseRepo<Quiz> Quizziz { get; private set; }
        public IBaseRepo<React> Reacts { get; private set; }
        public IBaseRepo<Survey> Surveys { get; private set; }
        public IBaseRepo<UserAssignment> UserAssignments { get; private set; }
        public IBaseRepo<UserGroup> UserGroups { get; private set; }
        public IBaseRepo<UserQuestion> UserQuestions { get; private set; }
        public IBaseRepo<UserSurvey> UserSurveys { get; private set; }
        public IBaseRepo<UserVoting> UserVotings { get; private set; }
        public IBaseRepo<Voting> Votings { get; private set; }

        #endregion

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Users = new BaseRepo<ApplicationUser>(context, userManager);
            Announcments = new BaseRepo<Announcement>(context, userManager);
            Assignments = new BaseRepo<Assignment>(context, userManager);
            Comments = new BaseRepo<Comment>(context, userManager);
            Departments = new BaseRepo<Department>(context, userManager);
            Groups = new BaseRepo<Group>(context, userManager);
            GroupAnnouncments = new BaseRepo<GroupAnnouncment>(context, userManager);
            GroupSurveys = new BaseRepo<GroupSurvey>(context, userManager);
            GroupVotings = new BaseRepo<GroupVoting>(context, userManager);
            Materials = new BaseRepo<Material>(context, userManager);
            Messages = new BaseRepo<Message>(context, userManager);
            Options = new BaseRepo<Option>(context, userManager);
            Posts = new BaseRepo<Post>(context, userManager);
            Questions = new BaseRepo<Question>(context, userManager);
            Quizziz = new BaseRepo<Quiz>(context, userManager);
            Reacts = new BaseRepo<React>(context, userManager);
            Surveys = new BaseRepo<Survey>(context, userManager);
            UserAssignments = new BaseRepo<UserAssignment>(context, userManager);
            UserSurveys = new BaseRepo<UserSurvey>(context, userManager);
            UserSurveys = new BaseRepo<UserSurvey>(context, userManager);
            UserVotings = new BaseRepo<UserVoting>(context, userManager);
            Votings = new BaseRepo<Voting>(context, userManager);
        }

        int IUnitOfWork.complete() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}
