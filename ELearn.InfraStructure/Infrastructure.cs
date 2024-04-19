using ELearn.Domain.Entities;
using ELearn.InfraStructure.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure
{
    public static class Infrastructure
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            #region Validation Services
            services.AddTransient<IValidator<Announcement>, AnnouncementValidation>();
            services.AddTransient<IValidator<ApplicationUser>, ApplicationUserValidation>();
            services.AddTransient<IValidator<Assignment>, AssignmentValidation>();
            services.AddTransient<IValidator<Comment>, CommentValidation>();
            services.AddTransient<IValidator<Department>, DepartmentValidation>();
            services.AddTransient<IValidator<GroupAnnouncment>, GroupAnnouncmentValidation>();
            services.AddTransient<IValidator<Group>, GroupValidation>();
            services.AddTransient<IValidator<React>, ReactValidation>();
            services.AddTransient<IValidator<Survey>, SurveyValidation>();
            services.AddTransient<IValidator<UserAnswerAssignment>, UserAssignmentValidation>();
            services.AddTransient<IValidator<UserGroup>, UserGroupValidation>();
            services.AddTransient<IValidator<UserAnswerQuestion>, UserQuestionValidation>();
            services.AddTransient<IValidator<UserAnswerSurvey>, UserSurveyValidation>();
            services.AddTransient<IValidator<UserAnswerVoting>, UserVotingValidation>();
            services.AddTransient<IValidator<Voting>, VotingValidation>();
            services.AddTransient<IValidator<GroupSurvey>, GroupSurveyValidation>();
            services.AddTransient<IValidator<GroupVoting>, GroupVotingValidation>();
            services.AddTransient<IValidator<Question>, QuestionValidation>();
            services.AddTransient<IValidator<Material>, MaterialValidation>();
            services.AddTransient<IValidator<Message>, MessageValidation>();
            //services.AddTransient<IValidator<Option>, OptionValidation>();
            services.AddTransient<IValidator<Post>, PostValidation>();
            services.AddTransient<IValidator<Quiz>, QuizValidation>();
            #endregion
        }
    }
}
