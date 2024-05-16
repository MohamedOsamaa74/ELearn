using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.InfraStructure.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ELearn.InfraStructure
{
    public static class Infrastructure
    {
        public static void InfrastructureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Validation Services
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            #endregion

            #region DbContext
            var db = Configuration.GetConnectionString("Default Connection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(db));
            #endregion
            
            #region BaseRepo
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion
        }
    }
}
