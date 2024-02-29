using CsvHelper.Configuration;
using CsvHelper;
using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.CSV_Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearn.Application.Interfaces;
using ELearn.Application.Helpers.Response;
using ELearn.Application.DTOs;
using ELearn.Domain.Const;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ELearn.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(AppDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IHttpContextAccessor Get_httpContextAccessor()
        {
            return _httpContextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }
        public async Task<Response<UserDTO>> CreateNewUserAsync(UserDTO Model)
        {
            if (Model == null)
                return ResponseHandler.BadRequest<UserDTO>("Invalid User");
            try
            {
                var NewUser = new ApplicationUser()
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    BirthDate = Model.BirthDate,
                    Address = Model.Address,
                    Nationality = Model.Nationality,
                    NId = Model.NId,
                    UserName = Model.UserName,
                    PhoneNumber = Model.PhoneNumber,
                    DepartmentId = Model.DepartmentId,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var result = await _userManager.CreateAsync(NewUser, NewUser.NId);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                        errors += error.Description;
                    return ResponseHandler.BadRequest<UserDTO>(errors);
                }
                await _userManager.AddToRoleAsync(NewUser, UserRoles.Student);
                var UserData = new UserDTO()
                {
                    FirstName = NewUser.FirstName,
                    LastName = NewUser.LastName,
                    BirthDate = NewUser.BirthDate,
                    Address = NewUser.Address,
                    Nationality = NewUser.Nationality,
                    NId = NewUser.NId,
                    UserName = NewUser.UserName,
                    PhoneNumber = NewUser.PhoneNumber,
                };
                return ResponseHandler.Created(UserData);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<UserDTO>(Ex.ToString());
            }

        }
        public async Task<IEnumerable<Response<UserDTO>>>AddMultipleUsersAsync(IFormFile file)
        {
            if (file == null)
            {
                return (IEnumerable<Response<UserDTO>>)ResponseHandler.BadRequest<UserDTO>("Invalid, Please Upload the File");
            }
            try
            {
                var Responses = new List<Response<UserDTO>>();
                var NewUsers = await UploadCSV(file);
                if (NewUsers.IsNullOrEmpty())
                    return (IEnumerable<Response<UserDTO>>)ResponseHandler.BadRequest<UserDTO>("The Uploaded File Doesn't Contain Any Data");
                foreach (var user in NewUsers)
                {
                    var NewUser = new UserDTO()
                    {
                        Address = user.Address,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        NId = user.NId,
                        UserName = user.UserName,
                        Nationality = user.Nationality,
                        PhoneNumber = user.PhoneNumber,
                        BirthDate = user.BirthDate,
                    };
                    if(!IsValidUser(user))
                        return (IEnumerable<Response<UserDTO>>)ResponseHandler.BadRequest<UserDTO>();
                    var response = await CreateNewUserAsync(NewUser);
                    Responses.Add(response);
                }

                return (IEnumerable<Response<UserDTO>>)ResponseHandler.Created(Responses);
            }
            catch(Exception Ex)
            {
                return ResponseHandler.BadRequest<UserDTO>($"An Error Occured While Proccessing The Request, {Ex}") as IEnumerable<Response<UserDTO>>;
            }

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
            if (string.IsNullOrEmpty(user.FirstName)) return false;
            if (string.IsNullOrEmpty(user.LastName)) return false;
            if (string.IsNullOrEmpty(user.Address)) return false;
            if (string.IsNullOrEmpty(user.Nationality)) return false;
            if (string.IsNullOrEmpty(user.UserName)) return false;
            if (string.IsNullOrEmpty(user.NId)) return false;
            if (string.IsNullOrEmpty(user.PhoneNumber)) return false;
            return true;
        }

        public Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

    }
}
