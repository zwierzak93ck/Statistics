using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Statistics;
using StatisticsWebAPI.Data.Models;
using StatisticsWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var x = DataBaseSettings.ConnectionString;
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(DataBaseSettings.ConnectionString
                //"server=127.0.0.1; database=StatisticsDb; user=d.zwierzchowski; password=Damian123; SslMode=none"
                );
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>()
            //    .Property(p => p.PasswordSalt)
             //   .HasColumnType("varbinary(16)");
        }
    }
}
