using ESourcing.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace ESourcing.Infrastructure.Data
{
    public class WebAppDbContext : IdentityDbContext<AppUser> //Identity kütüphanesini kullandığımız için IdentityDbContext ekledik
    {
        public WebAppDbContext()
        {

        }
        public WebAppDbContext(DbContextOptions<WebAppDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = localhost; Database = WebAppDb; User Id = sa; Password = 1q2w3e4R!; ");
            }
        }
    }
}
