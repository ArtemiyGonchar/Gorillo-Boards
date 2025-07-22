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
        private readonly ChartsDbContext _context;

        public Initializer(ChartsDbContext context) { _context = context; }

        public async Task InitializeDb(ChartsDbContext ctx)
        {
            await ctx.Database.MigrateAsync();
        }
    }
}
