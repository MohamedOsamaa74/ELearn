﻿using CsvHelper.Configuration;
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
using ELearn.Domain.Const;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.ObjectModel;
using AutoMapper;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using ELearn.Application.DTOs.UserDTOs;
using System.Security.Cryptography;
using ELearn.InfraStructure.Validations;

namespace ELearn.Application.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        #endregion

        #region GetUserRole
        public async Task<string> GetUserRoleAsync()
        {
            var user = await GetCurrentUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
        #endregion

        #region GetCurrentUser
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }
        #endregion

        #region GetCurrentUserID
        public async Task<string> GetCurrentUserIDAsync()
        {
            ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
            return _userManager.GetUserId(currentUser);
        }
        #endregion

        #region GetByUserName
        public async Task<ApplicationUser> GetByUserName(string UserName)
        {
            return await _userManager.FindByNameAsync(UserName);
        }
        #endregion

        #region GetByEmail
        public async Task<ApplicationUser> GetByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }
        #endregion

        #region GetById
        public async Task<ApplicationUser> GetByIdAsync(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }
        #endregion

        #region GetUsersWithRole
        public async Task<Response<ICollection<AddUserDTO>>> GetUsersWithRoleAsync(string Role)
        {
            var users = await _userManager.GetUsersInRoleAsync(Role);
            if (users.IsNullOrEmpty())
                return ResponseHandler.NotFound<ICollection<AddUserDTO>>("There Are No Users");
            try
            {
                var usersDTO = new List<AddUserDTO>();
                foreach (var item in users)
                {
                    var dto = _mapper.Map<AddUserDTO>(item);
                    dto.Role = (await _userManager.GetRolesAsync(item)).FirstOrDefault();
                    usersDTO.Add(dto);
                }
                return ResponseHandler.ManySuccess(usersDTO);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddUserDTO>>($"An Error Occurred,{Ex}");
            }
        }
        #endregion

        #region CreateNewUser
        public async Task<Response<AddUserDTO>> CreateNewUserAsync(AddUserDTO Model)
        {
            if (Model == null)
                return ResponseHandler.BadRequest<AddUserDTO>("Invalid User");
            try
            {
                var NewUser = _mapper.Map<ApplicationUser>(Model);
                NewUser.SecurityStamp = Guid.NewGuid().ToString();
                var (publicKey, privateKey) = GenerateKeyPair();
                NewUser.PublicKey = publicKey;
                NewUser.PrivateKey = privateKey;
                var valid = new ApplicationUserValidation().Validate(NewUser);
                if (!valid.IsValid)
                    return ResponseHandler.BadRequest<AddUserDTO>(null,valid.Errors.Select(x => x.ErrorMessage).ToList());
                var result = await _userManager.CreateAsync(NewUser, NewUser.NId);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                        errors += error.Description;
                    return ResponseHandler.BadRequest<AddUserDTO>(errors);
                }
                if (Model.Role == "Instructor")
                    await _userManager.AddToRoleAsync(NewUser, UserRoles.Staff);
                else if(Model.Role == "Student")
                    await _userManager.AddToRoleAsync(NewUser, UserRoles.Student);
                return ResponseHandler.Created(Model);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AddUserDTO>(Ex.ToString());
            }

        }
        #endregion

        #region AddMultipleUsers
        public async Task<Response<ICollection<AddUserDTO>>> AddMultipleUsersAsync(IFormFile file)
        {
            if (file == null)
            {
                return ResponseHandler.BadRequest<ICollection<AddUserDTO>>("Invalid, Please Upload the File");
            }
            try
            {
                var NewUsers = await UploadCSV(file);
                if (NewUsers.IsNullOrEmpty())
                    return ResponseHandler.BadRequest<ICollection<AddUserDTO>>("The Uploaded File Doesn't Contain Any Data");
                var users = new List<AddUserDTO>();
                foreach (var user in NewUsers)
                {
                    var NewUser = _mapper.Map<AddUserDTO>(user);
                    if (IsValidUser(user))
                    {
                        await CreateNewUserAsync(NewUser);
                        users.Add(NewUser);
                    }

                }
                return ResponseHandler.ManyCreated(users);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddUserDTO>>($"An Error Occured While Proccessing The Request, {Ex}");
            }

        }
        #endregion

        #region GetAll
        public async Task<Response<ICollection<AddUserDTO>>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            if (users.IsNullOrEmpty())
                return ResponseHandler.NotFound<ICollection<AddUserDTO>>("There Are No Users");
            try
            {
                var usersDTO = new List<AddUserDTO>();
                foreach (var item in users)
                {
                    var dto = _mapper.Map<AddUserDTO>(item);
                    dto.Role = (await _userManager.GetRolesAsync(item)).FirstOrDefault();
                    usersDTO.Add(dto);
                }
                return ResponseHandler.ManySuccess(usersDTO);
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddUserDTO>>($"An Error Occurred,{Ex}");
            }
        }
        #endregion

        #region DeleteUser
        public async Task<Response<AddUserDTO>> DeleteUserAsync(string Id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(Id);
            if (user == null)
                return ResponseHandler.NotFound<AddUserDTO>();
            try
            {
                await _unitOfWork.Users.DeleteAsync(user);
                return ResponseHandler.Deleted<AddUserDTO>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AddUserDTO>($"An Error Occurred, {Ex}");
            }

        }
        #endregion

        #region EditUser
        public async Task<Response<AddUserDTO>> EditUserAsync(string id, EditUserDTO NewData)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user is null)
                return ResponseHandler.NotFound<AddUserDTO>("There is no such user");
            try
            {
                if (NewData.FirstName != null) user.FirstName = NewData.FirstName;
                if (NewData.LastName != null) user.LastName = NewData.LastName;
                if (NewData.Nationality != null) user.Nationality = NewData.Nationality;
                if (NewData.PhoneNumber != null) user.PhoneNumber = NewData.PhoneNumber;
                if (NewData.Address != null) user.Address = NewData.Address;
                if (NewData.BirthDate != null) user.BirthDate = (DateTime)NewData.BirthDate;
                if (NewData.DepartmentId != 0) user.DepartmentId = NewData.DepartmentId;
                var valid = new ApplicationUserValidation().Validate(user);
                if (!valid.IsValid)
                    return ResponseHandler.BadRequest<AddUserDTO>(null, valid.Errors.Select(x => x.ErrorMessage).ToList());
                await _unitOfWork.Users.UpdateAsync(user);
                return ResponseHandler.Updated(_mapper.Map<AddUserDTO>(user));
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<AddUserDTO>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region DeleteMany
        public async Task<Response<ICollection<AddUserDTO>>> DeleteManyAsync(List<string> Ids)
        {
            var users = await GetSelectedUsersAsync(Ids);
            if (users.IsNullOrEmpty())
                return ResponseHandler.NotFound<ICollection<AddUserDTO>>();
            try
            {
                await _unitOfWork.Users.DeleteRangeAsync(users);
                return ResponseHandler.Deleted<ICollection<AddUserDTO>>();
            }
            catch (Exception Ex)
            {
                return ResponseHandler.BadRequest<ICollection<AddUserDTO>>($"An Error Occurred, {Ex}");
            }
        }
        #endregion

        #region GetUserProfilePicture
        public async Task<string> GetUserProfilePictureAsync(string UserId = null)
        {
            if (UserId == null)
                UserId = await GetCurrentUserIDAsync();
            var ProfilePictureUrl = await _unitOfWork.Files.GetWhereAsync(x => x.UserId == UserId && x.FolderName == "ProfilePictures");
            if (ProfilePictureUrl.IsNullOrEmpty())
                return null;
            return ProfilePictureUrl.FirstOrDefault().ViewUrl;
        }
        #endregion

        #region Private Methods
        private async Task<IEnumerable<ApplicationUser>> UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Enumerable.Empty<ApplicationUser>();
            }

            using (var stream = file.OpenReadStream())
            {
                if (stream == null)
                {
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

        private async Task<ICollection<ApplicationUser>> GetSelectedUsersAsync(List<string> Ids)
        {
            var users = new List<ApplicationUser>();
            foreach(var id in Ids)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user != null)
                    users.Add(user);
            }
            return users;
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

        #region CreateKeys
        private static (string publicKey, string privateKey) GenerateKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                return (
                    publicKey: Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                    privateKey: Convert.ToBase64String(rsa.ExportRSAPrivateKey())
                );
            }
        }
        #endregion
        #endregion
    }
}