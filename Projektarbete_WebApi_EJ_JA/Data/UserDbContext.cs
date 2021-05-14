using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projektarbete_WebApi_EJ_JA.Models;
using Microsoft.EntityFrameworkCore;

namespace Projektarbete_WebApi_EJ_JA.Data
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<Models.V1.GeoMessage> GeoMessages { get; set; }
        public DbSet<Models.V2.GeoMessage> GeoMessagesV2 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<User>();

            User user = new User()
            {
                FirstName = "Emil",
                LastName = "Johannesson",
                UserName = "Emil",
                NormalizedUserName ="EMIL",
                PasswordHash = hasher.HashPassword(null, "Test")
            };
            /*
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            passwordHasher.HashPassword(user, "Test");
            */
            builder.Entity<User>().HasData(user);
        }
        
    }
}
