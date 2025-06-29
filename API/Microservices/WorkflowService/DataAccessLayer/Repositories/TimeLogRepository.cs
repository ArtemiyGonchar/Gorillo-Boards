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
    public class TimeLogRepository : Repository<TicketTimeLog>, ITimeLogRepository
    {
        private readonly WorkflowDbContext _ctx;

        public TimeLogRepository(WorkflowDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<TicketTimeLog?> GetByTicket(Guid ticketId)
        {
            return await _ctx.Set<TicketTimeLog>()
                .FirstOrDefaultAsync(l => l.TicketId == ticketId);
        }

        public async Task<TicketTimeLog?> GetByUserAndTicket(Guid userId, Guid ticketId)
        {
            return await _ctx.Set<TicketTimeLog>()
                .FirstOrDefaultAsync(l => l.TicketId == ticketId && l.UserId == userId);
        }

        public async Task<TicketTimeLog?> GetInProgressLogByTicket(Guid ticketId)
        {
            return await _ctx.Set<TicketTimeLog>()
                .FirstOrDefaultAsync(l => l.TicketId == ticketId && l.EndedAt == null);
        }
    }
}
