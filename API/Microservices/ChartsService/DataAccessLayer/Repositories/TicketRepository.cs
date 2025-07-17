using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Ticket>> GetAllByPeriod(DateTime start, DateTime end)
        {
            return await _ctx.Set<Ticket>()
                .Where(t => t.CreatedAt >= start && t.CreatedAt <= end)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetAllByPeriodAndBoard(DateTime start, DateTime end, Guid boardId)
        {
            return await _ctx.Set<Ticket>()
                .Where(t => t.CreatedAt >= start && t.CreatedAt <= end && t.BoardId == boardId)
                .ToListAsync();
        }
    }
}
