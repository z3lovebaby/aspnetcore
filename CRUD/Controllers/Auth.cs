using CRUD.Models;
using CRUD.Repositories;
using CRUD.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly IJWTManagerRepository jWTManager;
        private readonly IUserServiceRepository userServiceRepository;
        private readonly UserManager<AppUsers> userManager;
        public Auth(IJWTManagerRepository jWTManager, IUserServiceRepository userServiceRepository, UserManager<AppUsers> userManager)
        {
            this.jWTManager = jWTManager;
            this.userServiceRepository = userServiceRepository;
            this.userManager = userManager;
        }
        [HttpGet("Check")]
        [Authorize]
        [HttpGet]
        public IActionResult Test()
        {
            // Your synchronous code here
            return Ok();
        }

        [HttpPost("SignUp")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            // Create IdentityUser object
            
            var user = new AppUsers
            {
                UserName = model.Email?.Trim(),
                Email = model.Email?.Trim()
            };

            // Create User
            var identityResult = await userManager.CreateAsync(user, model.PassWord);
            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignIn usersdata)
        {
            var validUser = await userServiceRepository.IsValidUserAsync(usersdata);

            if (!validUser)
            {
                return Unauthorized("Incorrect username or password!");
            }
            var identityUser = await userManager.FindByEmailAsync(usersdata.Email);
            var roles = await userManager.GetRolesAsync(identityUser);
            var token = jWTManager.GenerateToken(usersdata.Email, roles.ToList());

            if (token == null)
            {
                return Unauthorized("Invalid Attempt!");
            }

            // saving refresh token to the db
            UserRefreshTokens obj = new UserRefreshTokens
            {
                RefreshToken = token.RefreshToken,
                UserName = usersdata.Email,
                ExpirationTime = DateTime.Now.AddMinutes(5),
            };

            userServiceRepository.AddUserRefreshTokens(obj);
            userServiceRepository.SaveCommit();
            var response = new LoginResponseDto
            {
                Token = token.AccessToken,
                ReToken = token.RefreshToken,              
                Email = usersdata.Email,
                Roles = roles.ToList()
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(Tokens token)
        {
            var principal = jWTManager.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;
            //retrieve the saved refresh token from database
            var savedRefreshToken = userServiceRepository.GetSavedRefreshTokens(username, token.RefreshToken);
            if (savedRefreshToken.RefreshToken != token.RefreshToken)
            {
                return Unauthorized("Invalid attempt!");
            }
            var identityUser = await userManager.FindByEmailAsync(username);
            var roles = await userManager.GetRolesAsync(identityUser);
            var newJwtToken = jWTManager.GenerateRefreshToken(username, roles.ToList());
            if (newJwtToken == null)
            {
                return Unauthorized("Invalid attempt!");
            }

            // saving refresh token to the db
/*            UserRefreshTokens obj = new UserRefreshTokens
            {
                RefreshToken = newJwtToken.RefreshToken,
                UserName = username
            };

            userServiceRepository.DeleteUserRefreshTokens(username, token.RefreshToken);
            userServiceRepository.AddUserRefreshTokens(obj);
            userServiceRepository.SaveCommit();*/

            var response = new LoginResponseDto
            {
                Token = newJwtToken.AccessToken,
                ReToken = token.RefreshToken,
                Email = username,
                Roles = roles.ToList()
            };
            return Ok(response);
        }
    }
}
