using DataAccessLayer.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Initializer
{
    public class Initializer
    {
        private readonly IdentityDbContext _dbContext;


        public Initializer(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeDb(IdentityDbContext ctx) //initializing db if not exists
        {
            await ctx.Database.MigrateAsync();
        }
    }
}
