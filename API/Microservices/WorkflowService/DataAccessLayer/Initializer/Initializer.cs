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

        private readonly WorkflowDbContext _dbContext;

        public Initializer(WorkflowDbContext dbContext) { _dbContext = dbContext; }

        public async Task InitializeDb(WorkflowDbContext ctx) //initializing db if not exists
        {
            await ctx.Database.MigrateAsync();
        }
    }
}
