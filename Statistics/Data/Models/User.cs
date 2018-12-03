using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Data.Models
{
    public class User : IdentityUser
    {
        public string Password { get; set; }
        //public byte[] PasswordSalt { get; set; }
    }
}
