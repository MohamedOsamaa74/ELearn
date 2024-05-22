using ELearn.Application.Helpers.Account;
using ELearn.Application.Helpers.AutoMapper;
using ELearn.Application.Interfaces;
using ELearn.Application.Services;
using ELearn.Data;
using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application
{
    public static class Application
    {
        public static void ApplicationServices(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Services
            services.Configure<MailSettings>
                (Configuration.GetSection("MailSettings"));
            services.Configure<JWT>(Configuration.GetSection("JWT"));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IVotingService, VotingService>();
            services.AddTransient<ISurveyService, SurveyService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IAssignmentService, AssignmentService>();
            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IFileService, FilesService>();
            services.AddTransient<IUserTwoFactorTokenProvider<ApplicationUser>, EmailTokenProvider<ApplicationUser>>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<JWT>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<IReactService, ReactService>();
            #endregion

            #region authentication&autherization
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Tokens.ProviderMap["Default"] = new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<ApplicationUser>));
                options.Tokens.EmailConfirmationTokenProvider = "Default";
            }).AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion
        }
    }
}