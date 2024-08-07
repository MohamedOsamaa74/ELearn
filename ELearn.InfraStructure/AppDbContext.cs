﻿using Microsoft.EntityFrameworkCore;
using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ELearn.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            builder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAnnouncment> GroupAnnouncments { get; set; }
        public DbSet<GroupSurvey> GroupSurveys { get; set; }
        public DbSet<GroupVoting> GroupVotings { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Message> Messages { get; set; }
        //public DbSet<Option> Options { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<React> Reacts { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserAnswerQuiz> UserAnswerQuizziz { get; set; }
        public DbSet<UserAnswerQuestion> UserAnswerQuestions { get; set; }
        public DbSet<UserAnswerSurvey> UserAnswerSurveys { get; set; }
        public DbSet<UserAnswerVoting> UserAnswerVotings { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<UserAnswerAssignment> UserAnswerAssignments { get; set; }
        public DbSet<FileEntity> Files { get; set; }
    }
}