using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StatisticsWebAPI.Data;
using StatisticsWebAPI.Data.Models;
using StatisticsWebAPI.Helpers;
using StatisticsWebAPI.Interfaces;
using StatisticsWebAPI.Services;

namespace StatisticsWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public const string UserControllerUserId = "UserControllerUserId";

        private readonly UserManager<User> _userManager;
        private readonly HttpContext _httpContext;
        private TokenService _tokenService;
        private DataBaseContext _dbContext;
        private readonly IEmailSender _emailSender;

        public UserController(
            UserManager<User> userManager,
            IHttpContextAccessor httpContext, 
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _httpContext = httpContext.HttpContext;
            _dbContext = new DataBaseContext();
            _tokenService = new TokenService();
            _emailSender = emailSender;
        }
        // GET api/values
        [HttpPost, Route("users")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] User userForm)
        {
            IdentityResult result = await _userManager.CreateAsync(userForm, userForm.Password);
            if(result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(userForm.UserName);
                string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                //string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

                /*
                string callbackUrl = Url.Action("confirmEmail", "account",
                    values: new { userId = user.Id, token = HttpUtility.UrlEncode(confirmationToken) },
                    protocol: HttpContext.Request.Scheme);
                
                string callbackUrl = Url.RouteUrl("localhost:4200", values: new { userId = user.Id, token = HttpUtility.UrlEncode(confirmationToken) },
                    protocol: HttpContext.Request.Scheme);
                    */
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Hi " + user.UserName + "Please confirm your account by <a href='http://localhost:4200/emailConfirm?userId=" + user.Id + "&token=" + confirmationToken + "'>clicking here</a>.");

                return Ok(user);
            }
            return BadRequest("a");
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] User userForm)
        {
            User user = await _userManager.FindByNameAsync(userForm.UserName);
            Boolean checkPassword = await _userManager.CheckPasswordAsync(user, userForm.Password);
            PasswordVerificationResult checkHash = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userForm.Password);
            if ((user != null) && checkPassword && (checkHash == PasswordVerificationResult.Success))
            {
                    if (user.EmailConfirmed)
                    {
                        _httpContext.Response.Headers.Add("Token", _tokenService.GenerateToken(_dbContext, user, Encoding.ASCII.GetBytes(TokenSettings.Secret)));

                        return Ok(new
                        {
                            User = user
                        });
                    }
                    return BadRequest("Confirm email First");
                }
                return BadRequest("Wrong user name or password");
        }

        [HttpGet, Route("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            var isAuth = _httpContext.User.Identity.IsAuthenticated;

            return new OkObjectResult(isAuth);
        }
    }
}
