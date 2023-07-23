using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories.Base;

namespace Ordering.Infrastructure.Repositories
{
    /// <summary>
    /// IOrderRepository tipinde bir istekte bulunulduğunda OrderRepository olarak generate edilecek.
    /// Dependency injection constructorındaki nesneleri generate edecek.
    /// OrderRepository oluştuğunda biz onu arka taraftaki generic repository classımıza vericez ve additional repositoryden gelen fonksiyonları da kullanabiliyor olacağız.
    /// </summary>
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext) //Repository i base aldığı için parametre olarak vermemiz gerekti.
        {

        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                      .Where(o => o.SellerUserName == userName)
                      .ToListAsync();

            return orderList;
        }
    }
}
