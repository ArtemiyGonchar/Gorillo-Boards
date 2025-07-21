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
        private readonly BoardsDbContext _context;

        public Initializer(BoardsDbContext context) { _context = context; }

        public async Task InitializeDb(BoardsDbContext ctx)
        {
            await ctx.Database.MigrateAsync();
        }
    }
}
