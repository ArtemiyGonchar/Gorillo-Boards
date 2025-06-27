using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entites;
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
        private readonly WorkflowDbContext _ctx;
        public TicketRepository(WorkflowDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<Guid> CreateTicketWithOder(Ticket ticket)
        {
            var order = await _ctx.Set<Ticket>()
                .Where(t => t.StateId == ticket.StateId)
                .Select(t => (int?)t.Order)
                .MaxAsync() ?? 0;

            ticket.Order = order + 1;
            await _ctx.Set<Ticket>().AddAsync(ticket);
            await _ctx.SaveChangesAsync();
            return ticket.Id;
        }
    }
}
