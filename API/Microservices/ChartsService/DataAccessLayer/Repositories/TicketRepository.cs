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
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly ChartsDbContext _ctx;

        public TicketRepository(ChartsDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
