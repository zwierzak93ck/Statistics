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
        private readonly SignInManager<User> _signInManager;
        private readonly TokenSettings _tokenSettings;
        private readonly EmailSettings _emailSettings;
        private readonly HttpContext _httpContext;
        private UserService _userService;
        private TokenService _tokenService;
        private DataBaseContext _dbContext;
        private readonly IEmailSender _emailSender;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<TokenSettings> tokenSettings,
            IOptions<EmailSettings> emailSettings,
            IHttpContextAccessor httpContext, 
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenSettings = tokenSettings.Value;
            _emailSettings = emailSettings.Value;
            _httpContext = httpContext.HttpContext;
            _dbContext = new DataBaseContext();
            _userService = new UserService();
            _tokenService = new TokenService();
            _emailSender = emailSender;
        }
        // GET api/values
        [HttpPost, Route("users")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] User userForm)
        {
            string passwordHash;
            byte[] passwordSalt;
            //User user = _userService.Register(_dbContext, userForm, userForm.Password);
            var result = await _userManager.CreateAsync(userForm);
            
                var user = await _userManager.FindByNameAsync(userForm.UserName);
                _userService.GeneratePasswordHash(user.Password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
            
            

            var confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

            var callbackUrl = Url.Action("confirmEmail", "user",
                values: new { userId = user.Id, token = HttpUtility.UrlEncode(confirmationToken) },
                protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='" + callbackUrl + "'>clicking here</a>.", 
                _emailSettings.SmtpClientHost, _emailSettings.SmtpClientPort, _emailSettings.MailAddressEmail, _emailSettings.MailAddressPassword);

            return Ok(user);
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public IActionResult LogIn([FromBody] User userForm)
        {
            byte[] key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            User user = _userService.Authenticate(_dbContext, userForm.UserName, userForm.Password);

            if (!user.EmailConfirmed)
                return BadRequest("You have to confirm email first");

            
            _httpContext.Response.Headers.Add("Token" , _tokenService.GenerateToken(_dbContext, user, key));

            return Ok(new {
                User = user
            });
        }

        [HttpGet, Route("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            var isAuth = _httpContext.User.Identity.IsAuthenticated;

            return new OkObjectResult(isAuth);
        }

        [HttpGet, Route("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfimrEmail(string userId, string token)
        {
            var decodedToken = HttpUtility.UrlDecode(token);
            if(userId == null || token == null)
            {
                return BadRequest("error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                return Ok("ok");
            }
            return BadRequest("error");
        }
    }
}
