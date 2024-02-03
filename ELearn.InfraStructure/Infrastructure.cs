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
    public class Infrastructure
    {
        public void ConfigureServices(IServiceCollection services)
        {
            #region Validation Services
            services.AddTransient<IValidator<GroupSurvey>, GroupSurveyValidation>();
            services.AddTransient<IValidator<GroupVoting>, GroupVotingValidation>();
            services.AddTransient<IValidator<Question>, QuestionValidation>();
            services.AddTransient<IValidator<Material>, MaterialValidation>();
            services.AddTransient<IValidator<Message>, MessageValidation>();
            services.AddTransient<IValidator<Option>, OptionValidation>();
            services.AddTransient<IValidator<Post>, PostValidation>();
            services.AddTransient<IValidator<Quiz>, QuizValidation>();


            #endregion
        }
    }
}
