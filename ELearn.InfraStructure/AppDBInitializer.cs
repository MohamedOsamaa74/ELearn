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
using System.Text.RegularExpressions;
using Group = ELearn.Domain.Entities.Group;

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
                string StaffNID = "123456789";
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

                #region Announcements
                if (!context.Announcements.Any())
                {
                    context.Announcements.AddRange(new List<Announcement>()
                    {
                        new Announcement()
                        {
                            Text="First announcement text",
                            UserId="786ff688-6ef9-4e49-b7df-2ea5418ea2c5"

                        },
                        new Announcement()
                        {
                            Text = "Second announcement text",
                            UserId = "429a3de1-af0e-4b6c-803f-ace25939cc72"
                        },
                        new Announcement()
                        {
                            Text = "Third announcement text",
                            UserId = "786ff688-6ef9-4e49-b7df-2ea5418ea2c5"
                        }
                    });

                }
                context.SaveChanges();
                #endregion

                #region Groups
                if (!context.Groups.Any())
                {
                    context.Groups.AddRange(new List<Group>()
                    {
                        new Group()
                        {
                            GroupName = "Group 1",
                            Description = "Description for Group 1",
                            CreatorId = "786ff688-6ef9-4e49-b7df-2ea5418ea2c5",
                            DepartmentId = 1
                        },
                        new Group()
                        {
                            GroupName = "Group 2",
                            Description = "Description for Group 2",
                            CreatorId = "429a3de1-af0e-4b6c-803f-ace25939cc72",
                            DepartmentId = 1
                            
                        },
                        new Group()
                        {
                            GroupName = "Group 3",
                            Description = "Description for Group 3",
                            CreatorId = "786ff688-6ef9-4e49-b7df-2ea5418ea2c5",
                            DepartmentId = 2
                        }
                    });

                }
                context.SaveChanges();
                #endregion
            }
        }
    }
}
