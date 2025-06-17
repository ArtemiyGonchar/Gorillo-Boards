using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IdentityDbContext _ctx;

        public UserRepository(IdentityDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
