using Microsoft.IdentityModel.Tokens;
using StatisticsWebAPI.Data;
using StatisticsWebAPI.Data.Models;
using StatisticsWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Services
{
    public class TokenService
    {
        public string GenerateToken(DataBaseContext dbContext, User userForm, byte[] key)
        {
            User user = dbContext.Users.SingleOrDefault(u => u.UserName == userForm.UserName);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = this.GenerateaClaimsIdentity(user),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsIdentity GenerateaClaimsIdentity(User user)
        {
            return new ClaimsIdentity(new Claim[] {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            });
        }
    }
}
