using DataAccessLayer.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Initializer
{
    public class Initializer
    {
        public static void InitializeDb(BoardsDbContext ctx)
        {
            ctx.Database.EnsureCreated();
        }
    }
}
