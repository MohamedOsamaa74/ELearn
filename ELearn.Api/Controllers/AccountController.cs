using ELearn.Application.DTOs;
using ELearn.Domain.Entities;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        [HttpPost("LogIn")]
        public async Task<IActionResult>LogIn([FromBody] LogInUserDTO UserDTO)
        {
            var result = await _userManager.FindByNameAsync(UserDTO.UserName);
            if (result == null)
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Error", Message = "User Not Found" });
            if (!await _userManager.CheckPasswordAsync(result, UserDTO.Password))
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Error", Message = $"InCorrect Password" });
            #region claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, result.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            #endregion

            #region get roles
            var roles = await _userManager.GetRolesAsync(result);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            #endregion

            #region sign-in credintials
            SecurityKey securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            #endregion

            #region create token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCred
            );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = DateTime.Now.AddMinutes(30),
            });
            #endregion
        }
    }
}
