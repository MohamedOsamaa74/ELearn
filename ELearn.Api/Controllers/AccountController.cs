using ELearn.Application.DTOs.AuthDTOs;
using ELearn.Application.Helpers.Response;
using ELearn.Application.Interfaces;
using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ELearn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        #region Fields & Constructor
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion

        #region LogIn
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInUserDTO Model)
        {
            var response = await _accountService.LoginAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region LogOut
        [HttpPost("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var response = await _accountService.LogoutAsync();
            return this.CreateResponse(response);
        }
        #endregion

        #region Refresh Token
        [HttpPost("Refresh-Token")]

        public async Task<IActionResult> RefreshToken([FromBody] string Token)
        {
            var response = await _accountService.RefreshTokenAsync(Token);
            return this.CreateResponse(response);
        }
        #endregion

        #region Revoke Token
        [HttpPost("Revoke-Token")]
        public async Task<IActionResult> RevokeToken([FromBody] string Token)
        {
            var response = await _accountService.RevokeTokenAsync(Token);
            return this.CreateResponse(response);
        }
        #endregion

        #region Change Password
        [HttpPut("Change-Password")]
        public async Task<IActionResult>ChangePassword([FromBody] ChangePasswordDTO Model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _accountService.ChangePasswordAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Send Email
        [HttpPost("Send-Email")]
        public async Task<IActionResult> SendEmail([FromForm] EmailDTO Model)
        {
            var response = await _accountService.SendEmailAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Add Email
        [HttpPost("Add-Email")]
        [Authorize]
        public async Task<IActionResult> AddEmail([FromBody] string Email)
        {
            var response = await _accountService.AddEmailAsync(Email);
            return this.CreateResponse(response);
        }
        #endregion

        #region Confirm Email
        [HttpPost("Confirm-Email/{Token}")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail(string Token)
        {
            var response = await _accountService.ConfirmEmailAsync(Token);
            return this.CreateResponse(response);
        }
        #endregion
        
        #region Forgot Password
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string Email)
        {
            var response = await _accountService.ForgotPasswordAsync(Email);
            return this.CreateResponse(response);
        }
        #endregion

        #region Verify OTP
        [HttpPost("Verify-OTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPDTO Model)
        {
            var response = await _accountService.VerifyOTPAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion

        #region Reset Password
        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO Model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _accountService.ResetPasswordAsync(Model);
            return this.CreateResponse(response);
        }
        #endregion
    }
}