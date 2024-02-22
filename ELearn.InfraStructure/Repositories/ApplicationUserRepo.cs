using ELearn.Data;
using ELearn.Domain.Const;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.InfraStructure.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper;
using ELearn.InfraStructure.CSV_Validation;

namespace ELearn.InfraStructure.Repositories
{
    public class ApplicationUserRepo : BaseRepo<ApplicationUser>, IApplicationUserRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserRepo(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        public async Task<IEnumerable<ApplicationUser>> UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Handle the case where the file is null or empty
                return Enumerable.Empty<ApplicationUser>();
            }

            using (var stream = file.OpenReadStream())
            {
                if (stream == null)
                {
                    // Handle the case where the stream is null
                    return Enumerable.Empty<ApplicationUser>();
                }

                var readerConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                };
                var reader = new CsvReader(new StreamReader(stream), readerConfig);
                reader.Context.RegisterClassMap<ApplicationUserMap>();
                var UserRecords = reader.GetRecords<ApplicationUser>();
                var ValidUsers = new List<ApplicationUser>();
                foreach (var user in UserRecords)
                {
                    if (IsValidUser(user))
                        ValidUsers.Add(user);
                }
                return ValidUsers;
            }
        }

        private bool IsValidUser(ApplicationUser user)
        {
            if(string.IsNullOrEmpty(user.FirstName)) return false;
            if(string.IsNullOrEmpty(user.LastName)) return false;
            if(string.IsNullOrEmpty(user.Address)) return false;
            if(string.IsNullOrEmpty(user.Nationality)) return false;
            if(string.IsNullOrEmpty(user.UserName)) return false;
            if(string.IsNullOrEmpty(user.NId)) return false;
            if(string.IsNullOrEmpty(user.PhoneNumber)) return false;
            return true;
        }
    }
}
