using ESourcing.Core.Entities;
using ESourcing.Core.Repositories;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repository.Base;

namespace ESourcing.Infrastructure.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly WebAppDbContext _dbContext;

        public UserRepository(WebAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
