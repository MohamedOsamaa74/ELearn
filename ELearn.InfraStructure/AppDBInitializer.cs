using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Data;
using Group = ELearn.Domain.Entities.Group;

namespace ELearn.InfraStructure
{
    public class AppDbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder builder)
        {
            using var ServiceScope = builder.ApplicationServices.CreateScope();

            var context = ServiceScope.ServiceProvider.GetService<AppDbContext>();
            context.Database.EnsureCreated();

            #region roles
            var RoleManager = ServiceScope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            if (!await RoleManager.RoleExistsAsync(UserRoles.Admin))
                await RoleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await RoleManager.RoleExistsAsync(UserRoles.Staff))
                await RoleManager.CreateAsync(new IdentityRole(UserRoles.Staff));
            if (!await RoleManager.RoleExistsAsync(UserRoles.Student))
                await RoleManager.CreateAsync(new IdentityRole(UserRoles.Student));
            #endregion

            #region Departments
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(new List<Department>()
                    {
                        new ()
                        {
                            Title = "Dept1",
                        },
                        new ()
                        {
                            Title = "Dept2",
                        },
                        new ()
                        {
                            Title = "Dept3",
                        }
                    });

            }
            context.SaveChanges();
            #endregion

            var departments = context.Departments.ToList();
            var dept1 = departments.FirstOrDefault(d => d.Title == "Dept1");
            var dept2 = departments.FirstOrDefault(d => d.Title == "Dept2");
            var dept3 = departments.FirstOrDefault(d => d.Title == "Dept3");
            if (dept1 == null || dept2 == null || dept3 == null)
            {
                throw new Exception("Departments not found. Ensure they are correctly added to the database.");
            }
            #region Users
            var UserManager = ServiceScope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            #region Admin
            string AdminUserName = "0000000000";
            var AdminUser = await UserManager.FindByNameAsync(AdminUserName);
            if (AdminUser == null)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = AdminUserName,
                    DepartmentId = dept1.Id,
                    FirstName = "Admin",
                    LastName = "Test",
                    Address = "test address",
                    Nationality = "test",
                    NId = "test",
                    Relegion = "Muslim",
                    Faculty = "Compuer Science"
                };
                await UserManager.CreateAsync(newAdmin, "Admin@123");
                await UserManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
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
                    DepartmentId = dept2.Id,
                    FirstName = "Staff",
                    LastName = "Test",
                    BirthDate = DateTime.Parse("6/5/2001"),
                    Address = "Test Address",
                    Nationality = "test",
                    NId = "test",
                    Relegion = "christian",
                    Faculty = "Compuer Science"
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
                    DepartmentId = dept3.Id,
                    FirstName = "student",
                    LastName = "Test",
                    BirthDate = DateTime.Parse("6/5/2001"),
                    Address = "test address",
                    Nationality = "test",
                    NId = "test",
                    Relegion = "Muslim",
                    Faculty = "Compuer Science"
                };
                await UserManager.CreateAsync(newStudentUser, "Student@123");
                await UserManager.AddToRoleAsync(newStudentUser, UserRoles.Student);
            }
            #endregion

            #endregion

            var Admin = await UserManager.FindByNameAsync(AdminUserName);
            var Staff = await UserManager.FindByNameAsync(StaffUserName);
            var Student = await UserManager.FindByNameAsync(StudentUserName);

            #region Groups
            if (!context.Groups.Any())
            {
                context.Groups.AddRange(new List<Group>()
                    {
                        new ()
                        {
                            Name = "Group 1",
                            Description = "Description for Group 1",
                            CreatorId = Admin.Id,
                            DepartmentId = dept1.Id
                        },
                        new ()
                        {
                            Name = "Group 2",
                            Description = "Description for Group 2",
                            CreatorId = Admin.Id,
                            DepartmentId = dept2.Id
                        },
                        new ()
                        {
                            Name = "Group 3",
                            Description = "Description for Group 3",
                            CreatorId = Admin.Id,
                            DepartmentId = dept3.Id
                        },
                        new ()
                        {
                            Name = "Group 4",
                            Description = "Description for Group 4",
                            CreatorId = Admin.Id,
                            DepartmentId = dept3.Id
                        }
                    });
            }
            context.SaveChanges();
            #endregion

            var groups = context.Groups.ToList();
            var group1 = groups.FirstOrDefault(g => g.Name == "Group 1");
            var group2 = groups.FirstOrDefault(g => g.Name == "Group 2");
            var group3 = groups.FirstOrDefault(g => g.Name == "Group 3");
            var group4 = groups.FirstOrDefault(g => g.Name == "Group 4");

            #region UserGroups
            if (!context.UserGroups.Any())
            {
                context.UserGroups.AddRange(new List<UserGroup>()
                    {
                        new ()
                        {
                            UserId = Admin.Id,
                            GroupId = group1.Id
                        },
                        new ()
                        {
                            UserId = Student.Id,
                            GroupId = group2.Id
                        },
                        new ()
                        {
                            UserId = Student.Id,
                            GroupId = group3.Id
                        },
                        new ()
                        {
                            UserId = Student.Id,
                            GroupId = group4.Id
                        },
                        new ()
                        {
                            UserId = Staff.Id,
                            GroupId = group1.Id
                        },
                        new ()
                        {
                            UserId = Staff.Id,
                            GroupId = group2.Id
                        },
                        new ()
                        {
                            UserId = Staff.Id,
                            GroupId = group3.Id
                        },
                    });
                context.SaveChanges();
            }
            #endregion

            #region Announcements
            if (!context.Announcements.Any())
            {
                context.Announcements.AddRange(new List<Announcement>()
                    {
                        new ()
                        {
                            Text = "First announcement text",
                            UserId = Admin.Id,

                        },
                        new ()
                        {
                            Text = "Second announcement text",
                            UserId = Staff.Id
                        },
                        new ()
                        {
                            Text = "Third announcement text",
                            UserId = Staff.Id
                        }
                    });

            }
            context.SaveChanges();
            #endregion

            var announcements = context.Announcements.ToList();
            var announcement1 = announcements.FirstOrDefault(a => a.Text == "First announcement text");
            var announcement2 = announcements.FirstOrDefault(a => a.Text == "Second announcement text");
            var announcement3 = announcements.FirstOrDefault(a => a.Text == "Third announcement text");

            #region AnnouncementGroups
            if (!context.GroupAnnouncments.Any())
            {
                context.GroupAnnouncments.AddRange(new List<GroupAnnouncment>()
                    {
                        new ()
                        {
                            GroupId = group1.Id,
                            AnnouncementId = announcement1.Id
                        },
                        new ()
                        {
                            GroupId = group2.Id,
                            AnnouncementId = announcement2.Id
                        },
                        new ()
                        {
                            GroupId = group3.Id,
                            AnnouncementId = announcement3.Id
                        },
                        new ()
                        {
                            GroupId = group1.Id,
                            AnnouncementId = announcement2.Id
                        },
                        new ()
                        {
                            GroupId = group3.Id,
                            AnnouncementId = announcement1.Id
                        }
                    });
                context.SaveChanges();
            }
            #endregion

            #region Assignments
            if (!context.Assignments.Any())
            {
                context.Assignments.AddRange(new List<Assignment>()
                    {
                        new ()
                        {
                            Title = "Assignment 1",
                            UserId = Admin.Id,
                            GroupId = group1.Id,
                            End = DateTime.Now.AddDays(5)
                        },
                        new ()
                        {
                            Title = "Assignment 2",
                            UserId = Staff.Id,
                            GroupId = group3.Id,
                            End = DateTime.Now.AddDays(3)
                        },
                        new ()
                        {
                            Title = "Assignment 3",
                            UserId = Staff.Id,
                            GroupId = group3.Id,
                            End = DateTime.Now.AddDays(7)
                        },
                        new()
                        {
                            Title = "Assignment 4",
                            UserId = Staff.Id,
                            GroupId = group1.Id,
                            End = DateTime.Now.AddDays(5)
                        },
                        new()
                        {
                            Title = "Assignment 5",
                            UserId = Admin.Id,
                            GroupId = group2.Id,
                            End = DateTime.Now.AddDays(3)
                        }
                    });
            }
            #endregion

            #region Votings
            if (!context.Votings.Any())
            {
                context.Votings.AddRange(new List<Voting>()
                {
                    new()
                    {
                        Title = "Voting 1",
                        Description = "First Voting Description",
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(2),
                        CreatorId = Admin.Id,
                        Option1 = "Option 1 for Voting 1",
                        Option2 = "Option 2 for Voting 1",
                        Option3 = "Option 3 for Voting 1",
                    },
                    new ()
                    {
                        Title = "Voting 2",
                        Description = "Second Voting Description",
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(3),
                        CreatorId = Staff.Id,
                        Option1 = "Option 1 for Voting 2",
                        Option2 = "Option 2 for Voting 2",
                    },
                    new ()
                    {
                        Title = "Voting 3",
                        Description = "Third Voting Description",
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(4),
                        CreatorId = Staff.Id,
                        Option1 = "Option 1 for Voting 3",
                        Option2 = "Option 2 for Voting 3",
                        Option3 = "Option 3 for Voting 3",
                        Option4 = "Option 4 for Voting 3",
                        Option5 = "Option 5 for Voting 3"
                    }
                });
            }
            context.SaveChanges();
            #endregion

            var votings = context.Votings.ToList();
            var voting1 = votings.FirstOrDefault(v => v.Description == "Voting 1");
            var voting2 = votings.FirstOrDefault(v => v.Description == "Voting 2");
            var voting3 = votings.FirstOrDefault(v => v.Description == "Voting 3");

            #region VotingGroups
            if (!context.GroupVotings.Any())
            {
                context.GroupVotings.AddRange(new List<GroupVoting>()
                {
                    new()
                    {
                        VotingId = voting1.Id,
                        GroupId = group1.Id
                    },
                    new()
                    {
                        VotingId = voting2.Id,
                        GroupId = group2.Id
                    },
                    new()
                    {
                        VotingId = voting3.Id,
                        GroupId = group3.Id
                    },
                    new()
                    {
                        VotingId = voting1.Id,
                        GroupId = group2.Id
                    },
                    new()
                    {
                        VotingId = voting2.Id,
                        GroupId = group3.Id
                    },
                    new()
                    {
                        VotingId = voting3.Id,
                        GroupId = group1.Id
                    }
                });
            }
            context.SaveChanges();
            #endregion
        }
    }
}
