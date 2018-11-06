using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using StatisticsWebAPI.Data;
using StatisticsWebAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Services
{
    public class UserService
    {
        public User Register(DataBaseContext dbContext, User user, string password)
        {
            string passwordHash;
            byte[] passwordSalt;
            GeneratePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return user;
        }

        public User Authenticate(DataBaseContext dbContext, string userName, string password)
        {

            User user = dbContext.Users.SingleOrDefault(u => u.UserName == userName);

            return (user == null) ? null :
                !CheckPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null :
                    user;
        }

        private static void GeneratePasswordHash(string password, out string passwordHash, out byte[] passwordSalt)
        {

            passwordSalt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(passwordSalt);
            }

            passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: passwordSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
            );
        }

        private static bool CheckPasswordHash(string password, string storedPasswordHash, byte[] storedPasswordSalt)
        {
            byte[] passwordSalt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(passwordSalt);
            }

            string computedPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: storedPasswordSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
            );

            return (storedPasswordHash != computedPasswordHash) ? false : true;
        }
    }
}
