using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StatisticsWebAPI.Data;
using StatisticsWebAPI.Data.Models;
using StatisticsWebAPI.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StatisticsWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private DataBaseContext _dbContext;

        public AccountController(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _dbContext = new DataBaseContext();
        }

        [HttpGet, Route("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfimrEmailAsync(string userId, string token)
        {
            //var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var decodedToken = token.Replace(" ", "+");
            if (userId == null || decodedToken == null)
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
        
        [HttpPost, Route("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] User userForm)
        {
            User user = await _userManager.FindByEmailAsync(userForm.Email);
            if (user != null)
            {
                string resetPasswordToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                //string encodedResetPasswordToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetPasswordToken));

                await _emailSender.SendEmailAsync(user.Email, "Reset your password",
                    $"Hi " + user.UserName + "Please change your password by <a href='http://localhost:4200/newPassword?userId=" + user.Id + "&token=" + resetPasswordToken + "'>clicking here</a>.");

                return Ok();
            }
            return BadRequest();
        }

        [HttpGet, Route("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string token, string newPassword)
        {
            User user = await _userManager.FindByIdAsync(userId);
            var decodedToken = token.Replace(" ", "+");
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
                if(result.Succeeded)
                {
                    //user.Password = newPassword;
                    //await _dbContext.SaveChangesAsync();
                    // var r = await _userManager.UpdateAsync(user);
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }
        
    }
}
