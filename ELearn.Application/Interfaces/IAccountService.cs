using ELearn.Application.DTOs.AuthDTOs;
using ELearn.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IAccountService
    {
        /*public Task<Response<RegisterDTO>> RegisterAsync(RegisterDTO Model);
        */
        public Task<Response<AuthDTO>> LoginAsync(LogInUserDTO Model);
        public Task<Response<string>> LogoutAsync();
        public Task<Response<AuthDTO>> RefreshTokenAsync();
        public Task<Response<string>> RevokeTokenAsync(string Token);
        public Task<Response<string>> ChangePasswordAsync(ChangePasswordDTO Model);
        public Task<Response<EmailDTO>> SendEmailAsync(EmailDTO Model);
        public Task<Response<string>> ForgotPasswordAsync(string Email);
        public Task<Response<string>> VerifyOTPAsync(VerifyOTPDTO Model);
        public Task<Response<string>> ResetPasswordAsync(ResetPasswordDTO Model);
        public Task<Response<string>> AddEmailAsync(string Email);
        public Task<Response<string>> ConfirmEmailAsync(string Token);
        //public Task<Response<string>> DeleteEmailAsync(string Email);
        //public Task<Response<string>> DeleteAccountAsync();
    }
}