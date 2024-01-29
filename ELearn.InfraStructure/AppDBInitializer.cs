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

namespace ELearn.InfraStructure
{
    public  class AppDbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder builder)
        {
            using(var ServiceScope = builder.ApplicationServices.CreateScope())
            {
                #region roles
                var RoleManager = ServiceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if(!await RoleManager.RoleExistsAsync(UserRoles.Admin))
                    await RoleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await RoleManager.RoleExistsAsync(UserRoles.Staff))
                    await RoleManager.CreateAsync(new IdentityRole(UserRoles.Staff));
                if (!await RoleManager.RoleExistsAsync(UserRoles.Student))
                    await RoleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                #endregion

                #region Users
                var UserManager = ServiceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                #region Admin
                string AdminNID = "0000000000";
                var AdminUser = await UserManager.FindByNameAsync(AdminNID);
                if (AdminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FirstName = "محمد",
                        LastName = "أسامة",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "قنا - الشؤون",
                        Nationality = "مصري",
                        Religion = "مسلم",
                    };
                    await UserManager.CreateAsync(newAdminUser, "Staff@123");
                    await UserManager.AddToRoleAsync(newAdminUser, UserRoles.Staff);
                }
                #endregion

                #region Staff
                string StaffNID = "0000000000";
                var StaffUser = await UserManager.FindByNameAsync(StaffNID);
                if (StaffUser == null)
                {
                    var newStaffUser = new ApplicationUser()
                    {
                        FirstName = "محمد",
                        LastName = "أسامة",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "قنا - الشؤون",
                        Nationality = "مصري",
                        Religion = "مسلم",
                    };
                    await UserManager.CreateAsync(newStaffUser, "Staff@123");
                    await UserManager.AddToRoleAsync(newStaffUser, UserRoles.Staff);
                }
                #endregion

                #region Student
                string StudentNID = "30105068801177";
                var StudentUser = await UserManager.FindByNameAsync(StudentNID);
                if (StudentUser == null)
                {
                    var newStudentUser = new ApplicationUser()
                    {
                        FirstName = "محمد",
                        LastName = "أسامة",
                        BirthDate = DateTime.Parse("6/5/2001"),
                        Address = "قنا - الشؤون",
                        Nationality = "مصري",
                        Religion = "مسلم",
                        Grade = "الفرقة الرابعة",
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
