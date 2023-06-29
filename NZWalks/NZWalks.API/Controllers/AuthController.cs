using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        //POST:/api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            //if (registerRequestDto == null) 
            //{

            //}
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.userName,
                Email = registerRequestDto.userName

            };

            var identityResult = await userManager.CreateAsync(identityUser
                , registerRequestDto.password);
            if (identityResult.Succeeded)
            {
                if (registerRequestDto.roles != null && registerRequestDto.roles.Any())
                {
                    //Add Roles to this user
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was successfully registered, Please Login");

                    }
                }
            }
            return BadRequest("Something went wrong");
        }

        //POST:/api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.userName);
            if(user != null)
            {
                var checkPasswordResult = await userManager.
                    CheckPasswordAsync(user, loginRequestDto.password);
                
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create Token
                        var jwtToken= tokenRepository.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("UserName/Password is incorrect");
        }
           
    }
}
