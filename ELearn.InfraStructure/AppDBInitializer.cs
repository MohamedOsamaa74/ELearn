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
                string AdminUserName = "0000000000";
                var AdminUser = await UserManager.FindByNameAsync(AdminUserName);
                if (AdminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        UserName = AdminUserName,
                        DepartmentId = 1,
                        FirstName = "Admin",
                        LastName = "Test",
                        Address = "test address",
                        Nationality = "test",
                        NId = "test",
                    };
                    await UserManager.CreateAsync(newAdminUser, "Admin@123");
                    await UserManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                #endregion

                #region Staff
                string StaffUserName = "123456789";
                var StaffUser = await UserManager.FindByNameAsync(StaffUserName);
                if (StaffUser == null)
                {
                    var newStaffUser = new ApplicationUser()
                    {
                        UserName = StaffUserName,
                        DepartmentId = 2,
                        FirstName = "Staff",
                        LastName = "Test",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "Test Address",
                        Nationality = "test",
                        NId = "test",
                    };
                    await UserManager.CreateAsync(newStaffUser, "Staff@123");
                    await UserManager.AddToRoleAsync(newStaffUser, UserRoles.Staff);
                }
                #endregion

                #region Student
                string StudentUserName = "12345678901234";
                var StudentUser = await UserManager.FindByNameAsync(StudentUserName);
                if (StudentUser == null)
                {
                    var newStudentUser = new ApplicationUser()
                    {
                        UserName = StudentUserName,
                        DepartmentId = 3,
                        FirstName = "student",
                        LastName = "Test",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "test address",
                        Nationality = "test",
                        NId = "test",
                    };
                    await UserManager.CreateAsync(newStudentUser, "Student@123");
                    await UserManager.AddToRoleAsync(newStudentUser, UserRoles.Student);
                }
                #endregion

                #endregion

                //#region Announcements
                //if (!context.Announcements.Any())
                //{
                //    context.Announcements.AddRange(new List<Announcement>()
                //    {
                //        new Announcement()
                //        {
                //            Text="First announcement text",
                //            UserId="925eda7d-288d-419c-ac7f-bcdc64b45187"

                //        },
                //        new Announcement()
                //        {
                //            Text = "Second announcement text",
                //            UserId = "2eb94dab-3a56-4694-8691-6a880a40cc25"
                //        },
                //        new Announcement()
                //        {
                //            Text = "Third announcement text",
                //            UserId = "2eb94dab-3a56-4694-8691-6a880a40cc25"
                //        }
                //    });

                //}
                //context.SaveChanges();
                //#endregion

                //#region Groups
                //if (!context.Groups.Any())
                //{
                //    context.Groups.AddRange(new List<Group>()
                //    {
                //        new Group()
                //        {
                //            GroupName = "Group 1",
                //            Description = "Description for Group 1",
                //            CreatorId = "925eda7d-288d-419c-ac7f-bcdc64b45187",
                //            DepartmentId = 1
                //        },
                //        new Group()
                //        {
                //            GroupName = "Group 2",
                //            Description = "Description for Group 2",
                //            CreatorId = "925eda7d-288d-419c-ac7f-bcdc64b45187",
                //            DepartmentId = 1

                //        },
                //        new Group()
                //        {
                //            GroupName = "Group 3",
                //            Description = "Description for Group 3",
                //            CreatorId = "925eda7d-288d-419c-ac7f-bcdc64b45187",
                //            DepartmentId = 2
                //        }
                //    });

                //}
                //context.SaveChanges();
                //#endregion

                //#region Assignments
                //if (!context.Assignments.Any())
                //{
                //    context.Assignments.AddRange(new List<Assignment>()
                //    {
                //        new Assignment()
                //        {
                //            Title = "Assignment 1",
                //            Date = DateTime.Now.AddDays(-7), // Example date
                //            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2).AddMinutes(30) },
                //            UserId = "2eb94dab-3a56-4694-8691-6a880a40cc25",
                //            GroupId = 1,
                //        },
                //        new Assignment()
                //        {
                //            Title = "Assignment 2",
                //            Date = DateTime.Now.AddDays(-5), // Example date
                //            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2).AddMinutes(30) },
                //            UserId = "2eb94dab-3a56-4694-8691-6a880a40cc25",
                //            GroupId = 3
                //        },
                //        new Assignment()
                //        {
                //            Title = "Assignment 3",
                //            Date = DateTime.Now.AddDays(-3), // Example date
                //            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2).AddMinutes(30) },
                //            UserId = "2eb94dab-3a56-4694-8691-6a880a40cc25",
                //            GroupId = 3
                //        }
                //    });

                //}
                //context.SaveChanges();
                //#endregion

                /*
                #region Surveys
                if (!context.Surveys.Any())
                {
                    context.Surveys.AddRange(new List<Survey>()
                    {
                        new Survey
                        {
                            Text = "Survey 1 Text",
                            Date = DateTime.Now.AddDays(-7), 
                            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) }, 
                            ApplicationUserId = "2eb94dab-3a56-4694-8691-6a880a40cc25",
                            Options = new List<Option>
                            {
                                new Option { Id = 1, Text = "Option 1 for Survey 1" },
                                new Option { Id = 2, Text = "Option 2 for Survey 1" },
                                new Option { Id = 3, Text = "Option 3 for Survey 1" },
                                new Option { Id = 4, Text = "Option 4 for Survey 1" }
                            }
                        },
                        new Survey
                        {
                            Text = "Survey 2 Text",
                            Date = DateTime.Now.AddDays(-5),
                            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2) },
                            ApplicationUserId = "786ff688-6ef9-4e49-b7df-2ea5418ea2c5",
                            Options = new List<Option>
                            {
                                new Option { Id = 3, Text = "Option 1 for Survey 2" },
                                new Option { Id = 4, Text = "Option 2 for Survey 2" }
                            }
                        }
                    });

                }
                context.SaveChanges();
                #endregion

                #region Votings
                if (!context.Votings.Any())
                {
                    context.Votings.AddRange(new List<Voting>()
                    {
                        new Voting
                        {
                            Id = 1,
                            Text = "Voting 1 Text",
                            CreateDate = DateTime.Now.AddDays(-7),
                            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) }, 
                            ApplicationUserId = "2eb94dab-3a56-4694-8691-6a880a40cc25",
                            Options = new List<Option>
                            {
                                new Option { Id = 1, Text = "Option 1 for Voting 1" },
                                new Option { Id = 2, Text = "Option 2 for Voting 1" }
                                
                            }
                            
                        },
                        new Voting
                        {
                            Id = 2,
                            Text = "Voting 2 Text",
                            CreateDate = DateTime.Now.AddDays(-5), 
                            Duration = new Duration { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2) }, 
                            ApplicationUserId = "2eb94dab-3a56-4694-8691-6a880a40cc25",
                            Options = new List<Option>
                            {
                                new Option { Id = 3, Text = "Option 1 for Voting 2" },
                                new Option { Id = 4, Text = "Option 2 for Voting 2" }
                                
                            }
                            
                        }
                    });

                }
                context.SaveChanges();
                #endregion*/
            }
        }
    }
}
