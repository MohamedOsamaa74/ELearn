﻿using ELearn.Application.Helpers.Account;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using ELearn.InfraStructure.Repositories.UnitOfWork;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using ELearn.Application.DTOs.AuthDTOs;


namespace ELearn.Application.Services
{
    public class AccountService : IAccountService
    {
        #region Fields & Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MailSettings _mailSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWT _jwt;
        private readonly IUserService _userService;

        private static readonly Dictionary<string, (string OTP, DateTime Expiry)> _otpCache
                = new Dictionary<string, (string OTP, DateTime Expiry)>();
        public AccountService(IOptions<MailSettings> mailSettings, IOptions<JWT> jwt, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _jwt = jwt.Value;
            _mailSettings = mailSettings.Value;
        }
        #endregion

        #region Login
        public async Task<Response<AuthDTO>> LoginAsync(LogInUserDTO Model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(Model.UserName);
                if (user == null || !await _userManager.CheckPasswordAsync(user, Model.Password))
                    return ResponseHandler.BadRequest<AuthDTO>("Invalid Credintials");

                var token = await CreateTokenAsync(user);
                AuthDTO auth = new AuthDTO()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    IsAuthenticated = true,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user) as List<string>,
                };
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var ActiveRefreshToken = user.RefreshTokens.First(a => a.IsActive);
                    auth.RefreshToken = ActiveRefreshToken.Token;
                    auth.RefreshTokenExpiration = ActiveRefreshToken.ExpiresOn;
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(user);
                    auth.RefreshToken = refreshToken.Token;
                    auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
                }

                if (!string.IsNullOrEmpty(auth.RefreshToken))
                    SetRefreshTokenInCookie(auth.RefreshToken, auth.RefreshTokenExpiration);

                return ResponseHandler.Success(auth, $"Welcome Back,{user.FirstName}");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<AuthDTO>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Logout
        public async Task<Response<string>> LogoutAsync()
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();

                if (user == null)
                    return ResponseHandler.BadRequest<string>("User not found");

                await _signInManager.SignOutAsync();

                return ResponseHandler.Success("Logged Out Successfully");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Refresh Token
        public async Task<Response<AuthDTO>> RefreshTokenAsync(string Token)
        {
            try
            {
                var RefreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
                if (string.IsNullOrEmpty(RefreshToken))
                    return ResponseHandler.BadRequest<AuthDTO>("InValidToken");
                var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == Token));
                if (user == null)
                    return ResponseHandler.BadRequest<AuthDTO>("Invalid Token");
                var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
                if (!refreshToken.IsActive)
                {
                    return ResponseHandler.BadRequest<AuthDTO>("InActive Token");
                }
                refreshToken.RevokedOn = DateTime.UtcNow;
                var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
                var jwtToken = await CreateTokenAsync(user);
                AuthDTO auth = new AuthDTO
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiration = newRefreshToken.ExpiresOn,
                    IsAuthenticated = true,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user) as List<string>,
                };
                SetRefreshTokenInCookie(auth.RefreshToken, auth.RefreshTokenExpiration);
                return ResponseHandler.Success(auth);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<AuthDTO>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Revok Token
        public async Task<Response<string>> RevokeTokenAsync(string Token)
        {
            try
            {
                Token = Token ?? _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
                if (string.IsNullOrEmpty(Token))
                    return ResponseHandler.BadRequest<string>("InActiveToken");

                var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == Token));
                if (user == null)
                    return ResponseHandler.BadRequest<string>("Invalid Token");

                var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
                if (!refreshToken.IsActive)
                    return ResponseHandler.BadRequest<string>("InActive Token");
                
                refreshToken.RevokedOn = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
                return ResponseHandler.Success("Token Revoked");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Change Password
        public async Task<Response<string>> ChangePasswordAsync(ChangePasswordDTO Model)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return ResponseHandler.BadRequest<string>("User not found");

                if(Model.NewPassword != Model.ConfirmPassword)
                    return ResponseHandler.BadRequest<string>("Passwords do not match");

                var result = await _userManager.ChangePasswordAsync(user, Model.CurrentPassword, Model.NewPassword);
                if (!result.Succeeded)
                    return ResponseHandler.BadRequest<string>(result.Errors.FirstOrDefault().Description);
                
                return ResponseHandler.Success("Password Changed Successfully");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>( $"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Add Email
        public async Task<Response<string>> AddEmailAsync(string Email)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (await _userManager.FindByEmailAsync(Email) != null)
                    return ResponseHandler.BadRequest<string>("Email already exists");

                user.Email = Email;
                await _userManager.UpdateAsync(user);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?Token={token}";
                var emailModel = new EmailDTO
                {
                    Email = Email,
                    Subject = "Confirm Email",
                    Body = $"<h1>Confirm Your Email</h1><br>" +
                    $"<p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a></p>"
                };
                await SendEmailAsync(emailModel);
                return ResponseHandler.Success("An Email was Sent Containing the link to confirm your email");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Confirm Email
        public async Task<Response<string>> ConfirmEmailAsync(string Token)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                    return ResponseHandler.BadRequest<string>("User not found");

                var result = await _userManager.ConfirmEmailAsync(user, Token);
                if (!result.Succeeded)
                    return ResponseHandler.BadRequest<string>(result.Errors.FirstOrDefault().Description);
                return ResponseHandler.Success("Yor Email Has been Confirmed");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Forget Password
        public async Task<Response<string>> ForgotPasswordAsync(string Email)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                    return ResponseHandler.BadRequest<string>("User not found");
                var otp = GenerateRandomOTP();
                _otpCache[Email] = (otp, DateTime.UtcNow.AddMinutes(5));
                var emailModel = new EmailDTO
                {
                    Email = Email,
                    Subject = "Password Reset",
                    Body = $"<h1>Reset Your Password</h1><br>" +
                    $"<p>Your OTP is {otp}</p>"
                };
                var response = await SendEmailAsync(emailModel);
                if (response.StatusCode is HttpStatusCode.BadRequest)
                    return ResponseHandler.BadRequest<string>(response.Message);
                
                return ResponseHandler.Success("An Email Was Sent To Your Email Containing The OTP To Reseting Your Password");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Reset Password
        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordDTO Model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Model.Email);
                if (user == null)
                    return ResponseHandler.BadRequest<string>("User not found");

                if (!VerifyOTP(Model.Email, Model.OTP))
                    return ResponseHandler.BadRequest<string>("Invalid OTP");
                
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                if(token == null)
                    return ResponseHandler.BadRequest<string>("Token not generated");

                if(Model.NewPassword != Model.ConfirmPassword)
                    return ResponseHandler.BadRequest<string>("Passwords do not match");

                var result = await _userManager.ResetPasswordAsync(user, token, Model.NewPassword);
                if (!result.Succeeded)
                    return ResponseHandler.BadRequest<string>(result.Errors.FirstOrDefault().Description);
                
                return ResponseHandler.Success("Your Password Reset Was Successfull, Please LogIn Using the new Password");
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<string>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Send Email
        public async Task<Response<EmailDTO>> SendEmailAsync(EmailDTO Model)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                message.To.Add(new MailboxAddress("", Model.Email));
                message.Subject = Model.Subject;
                var builder = new BodyBuilder
                {
                    HtmlBody = Model.Body
                };
                if (Model.Attachements != null)
                {
                    byte[] fileBytes;
                    foreach (var file in Model.Attachements)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await file.CopyToAsync(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                return ResponseHandler.Success(Model);
            }
            catch (Exception ex)
            {
                return ResponseHandler.BadRequest<EmailDTO>($"An error occurred, {ex.Message}");
            }
        }
        #endregion

        #region Private Methods

        #region Create Token
        private async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user)
        {
            #region claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            #endregion

            #region get roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            #endregion

            #region sign-in credintials
            SecurityKey securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            #endregion

            #region create token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: signingCred
            );
            #endregion

            return token;
        }
        #endregion

        #region Generate Refresh Token
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
            };
        }
        #endregion

        #region Set Refresh Token in Cookie
        private void SetRefreshTokenInCookie(string Token, DateTime expires)
        {
            var CoockieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", Token, CoockieOptions);
        }
        #endregion

        #region Generate OTP
        private string GenerateRandomOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        #endregion

        #region Verify OTP
        private bool VerifyOTP(string email, string otp)
        {
            if (_otpCache.ContainsKey(email))
            {
                var (cachedOTP, expiry) = _otpCache[email];
                if (DateTime.UtcNow <= expiry && otp == cachedOTP)
                {
                    _otpCache.Remove(email);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #endregion
    }
}
