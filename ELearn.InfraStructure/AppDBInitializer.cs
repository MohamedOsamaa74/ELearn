using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Data;

namespace ELearn.InfraStructure
{
    public  class AppDbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder builder)
        {
            using(var ServiceScope = builder.ApplicationServices.CreateScope())
            {

                var context = ServiceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                #region Departments
                if(!context.Departments.Any())
                {
                     context.Departments.AddRange(new List<Department>()
                    {
                        new Department()
                        {
                            title = "Dept1",
                        },
                        new Department()
                        {
                            title = "Dept2",
                        },
                        new Department()
                        {
                            title = "Dept3",
                        }
                    });
                    
                }
                context.SaveChanges();
                #endregion

                #region roles
                var RoleManager = ServiceScope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();
                if(!await RoleManager.RoleExistsAsync(UserRoles.Admin))
                    await RoleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await RoleManager.RoleExistsAsync(UserRoles.Staff))
                    await RoleManager.CreateAsync(new IdentityRole(UserRoles.Staff));
                if (!await RoleManager.RoleExistsAsync(UserRoles.Student))
                    await RoleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                #endregion

                #region Users
                var UserManager = ServiceScope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

                #region Admin
                string AdminNID = "0000000000";
                var AdminUser = await UserManager.FindByNameAsync(AdminNID);
                if (AdminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        UserName = AdminNID,
                        DepartmentId = 1,
                        FirstName = "Admin",
                        LastName = "Test",
                        Address = "test address",
                        Nationality = "test",
                        Religion = "test",
                    };
                    await UserManager.CreateAsync(newAdminUser, "Admin@123");
                    await UserManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                #endregion

                #region Staff
                string StaffNID = "0000000000";
                var StaffUser = await UserManager.FindByNameAsync(StaffNID);
                if (StaffUser == null)
                {
                    var newStaffUser = new ApplicationUser()
                    {
                        UserName = StaffNID,
                        DepartmentId = 2,
                        FirstName = "Staff",
                        LastName = "Test",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "Test Address",
                        Nationality = "test",
                        Religion = "test",
                    };
                    await UserManager.CreateAsync(newStaffUser, "Staff@123");
                    await UserManager.AddToRoleAsync(newStaffUser, UserRoles.Staff);
                }
                #endregion

                #region Student
                string StudentNID = "12345678901234";
                var StudentUser = await UserManager.FindByNameAsync(StudentNID);
                if (StudentUser == null)
                {
                    var newStudentUser = new ApplicationUser()
                    {
                        UserName = StudentNID,
                        DepartmentId = 3,
                        FirstName = "student",
                        LastName = "Test",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "test address",
                        Nationality = "test",
                        Religion = "test",
                    };
                    await UserManager.CreateAsync(newStudentUser, "Student@123");
                    await UserManager.AddToRoleAsync(newStudentUser, UserRoles.Student);
                }
                #endregion

                #endregion
            }
        }
    }
}
