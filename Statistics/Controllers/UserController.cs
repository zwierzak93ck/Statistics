using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StatisticsWebAPI.Data;
using StatisticsWebAPI.Data.Models;
using StatisticsWebAPI.Helpers;
using StatisticsWebAPI.Services;

namespace StatisticsWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public const string UserControllerUserId = "UserControllerUserId";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppSettings _appSettings;
        private UserService _userService;
        private TokenService _tokenService;
        private DataBaseContext _dbContext;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _dbContext = new DataBaseContext();
            _userService = new UserService();
            _tokenService = new TokenService();
        }
        // GET api/values
        [HttpPost, Route("users")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] User userForm)
        {
            User user = _userService.Register(_dbContext, userForm, userForm.Password);
            return Ok(user);
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public IActionResult LogIn([FromBody] User userForm)
        {
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            User user = _userService.Authenticate(_dbContext, userForm.UserName, userForm.Password);
            string token = _tokenService.GenerateToken(userForm, key);

            return Ok(new {
                User = user,
                Token = token
            });
        }

        [HttpGet, Route("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok("test2");
        }
    }
}
