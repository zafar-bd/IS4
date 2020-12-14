using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IS4WithIdenity.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace IS4WithIdenity.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<IdentityUser<string>>().ToTable("Users");
            //builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            //builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            //builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            //builder.Entity<IdentityUserLogin<Guid>>()
            //    .ToTable("UserLogins")
            //    .HasKey(l => new
            //    {
            //        l.LoginProvider,
            //        l.ProviderKey
            //    });
            //builder.Entity<IdentityUserRole<Guid>>()
            //    .ToTable("UserRoles")
            //     .HasKey(l => new
            //     {
            //         l.UserId,
            //         l.RoleId
            //     });
            //builder.Entity<IdentityUserToken<Guid>>()
            //    .ToTable("UserTokens")
            //     .HasKey(l => new
            //     {
            //         l.UserId,
            //         l.LoginProvider,
            //         l.Name
            //     });
        }
    }
}
