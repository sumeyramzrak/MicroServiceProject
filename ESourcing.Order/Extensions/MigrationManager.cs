using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Data;

namespace ESourcing.Order.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            //CreateScope oluşturmamızın nedeni ilgili host üzerindeki servis katmanına ulaşıp istediğimiz servisi dependecy injection ile çağırabilmeyi sağlamak.
            {
                try
                {
                    var orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                    if (orderContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        orderContext.Database.Migrate();
                    }
                    OrderContextSeed.SeedAsync(orderContext).Wait(); //Async çalıştığı için wait dedik.
                }
                catch (Exception ex)
                {
                    throw;
                }
                //Poly frameworku ile buradaki işlemi retry mekanizması ile de yapmasını sağlayabiliriz.
            }

            return host;
        }
    }
}
