﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Repositories;
using Ordering.Domain.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Base;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region INMEMORY?
            /*
             * EntityFrameworku UseInMemoryDatabase olacak şekilde konfigure ettik. 
             * InMemoryDatabase = Uygulama ayağa kalkarken inmemoryde bir db olacak şekilde kendini konfigure ediyor. 
             */
            #endregion
            services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"),
                                                ServiceLifetime.Singleton,
                                                ServiceLifetime.Singleton);

            //services.AddDbContext<OrderContext>(options =>
            //        options.UseSqlServer(
            //            configuration.GetConnectionString("OrderConnection"),
            //            b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)), ServiceLifetime.Singleton);

            //Add Repositories
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //Best practice olarak kendi içerisinde tip alan interfaceleri lifetime a eklerken typeof ile ekliyoruz.
            services.AddTransient<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
