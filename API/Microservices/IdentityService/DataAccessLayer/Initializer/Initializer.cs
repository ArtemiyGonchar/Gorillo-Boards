using DataAccessLayer.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Initializer
{
    public class Initializer(IdentityDbContext ctx)
    {
        public static void InitializeDb(IdentityDbContext ctx) //initializing db if not exists
        {
            ctx.Database.EnsureCreated();
        }
    }
}
