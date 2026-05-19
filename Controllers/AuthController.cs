using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Models.DTO;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using NZWalksAuthAPI.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IToken token;

        public AuthController(UserManager<IdentityUser> userManager, IToken token)
        {
            this.userManager = userManager;
            this.token = token;
        }

        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(
                identityUser,
                registerRequestDto.Password
            );

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(
                        identityUser,
                        registerRequestDto.Roles
                    );

                    if (identityResult.Succeeded)
                    {
                        return Ok("User registered!");
                    }
                }
            }

            return BadRequest(identityResult.Errors);
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(
                    user,
                    loginRequestDto.Password
                );

                if (checkPasswordResult)
                { // Create Token
                    var roles = await userManager.GetRolesAsync(user);
                    var response = new LoginResponseDto { jwtToken = token.CreateJWTToken(user, roles.ToList()) };
                    return Ok(response);
                }
            }

            return BadRequest("Username or password incorrect");
        }

    }

}
