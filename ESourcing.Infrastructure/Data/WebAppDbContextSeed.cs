using ESourcing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESourcing.Infrastructure.Data
{
    public class WebAppDbContextSeed
    {
        public static async Task SeedAsync(WebAppDbContext webAppContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                webAppContext.Database.Migrate();
                if (!webAppContext.AppUsers.Any())
                {
                    webAppContext.AppUsers.AddRange(GetPreconfiguredOrders());
                    await webAppContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<WebAppDbContextSeed>();
                    log.LogError(ex.Message);
                    Thread.Sleep(2000);
                    await SeedAsync(webAppContext, loggerFactory, retryForAvailability);
                }
            }
        }
        private static IEnumerable<AppUser> GetPreconfiguredOrders()
        {
            return new List<AppUser>()
            {
                new AppUser
                {
                    FirstName ="User1",
                    LastName = "User LastName1",
                    IsSeller = true,
                    IsBuyer = false
                }
            };
        }

    }
}
