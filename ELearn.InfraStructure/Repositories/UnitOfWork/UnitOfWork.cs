using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.Base;
using ELearn.Domain.Interfaces.UnitOfWork;
using ELearn.InfraStructure.Repositories.Base;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
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

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new BaseRepo<ApplicationUser>(context);
            Announcments = new BaseRepo<Announcement>(context);
           // Assignments = new BaseRepo<Assignment>(context);
            Comments = new BaseRepo<Comment>(context);
            Departments = new BaseRepo<Department>(context);
            Groups = new BaseRepo<Group>(context);
            GroupAnnouncments = new BaseRepo<GroupAnnouncment>(context);
            GroupSurveys = new BaseRepo<GroupSurvey>(context);
            GroupVotings = new BaseRepo<GroupVoting>(context);
            Materials = new BaseRepo<Material>(context);
            Messages = new BaseRepo<Message>(context);
            Options = new BaseRepo<Option>(context);
            Posts = new BaseRepo<Post>(context);
            Questions = new BaseRepo<Question>(context);
            Quizziz = new BaseRepo<Quiz>(context);
            Reacts = new BaseRepo<React>(context);
            Surveys = new BaseRepo<Survey>(context);
            UserAssignments = new BaseRepo<UserAssignment>(context);
            UserSurveys = new BaseRepo<UserSurvey>(context);
            UserSurveys = new BaseRepo<UserSurvey>(context);
            UserVotings = new BaseRepo<UserVoting>(context);
            Votings = new BaseRepo<Voting>(context);
        }
       

        int IUnitOfWork.complete() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();

        public UnitOfWork(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

            Assignments = new BaseRepo<Assignment>(context, webHostEnvironment);
        }
      


    }
   
}
