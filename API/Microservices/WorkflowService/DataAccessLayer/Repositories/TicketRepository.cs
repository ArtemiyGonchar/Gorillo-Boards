﻿using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        public async Task<bool> DeleteClosedTickets()
        {
            var toDelete = await _ctx.Set<Ticket>().Where(t => t.TicketClosed != null).ToListAsync();
            if (toDelete == null)
            {
                return false;
            }

            _ctx.Set<Ticket>().RemoveRange(toDelete);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ticket>?> GetClosedTickets()
        {
            return await _ctx.Set<Ticket>().Where(t => t.IsClosed).ToListAsync();
        }

        public async Task<int> GetMaxOrderCount(Guid stateId)
        {
            var order = await _ctx.Set<Ticket>()
                .Where(t => t.StateId == stateId)
                .Select(t => (int?)t.Order)
                .MaxAsync() ?? 0;
            return order;
        }

        public async Task<List<Ticket>> GetTicketByState(Guid stateId)
        {
            return await _ctx.Set<Ticket>()
                .Where(t => t.StateId == stateId
                ).Include(t => t.TicketLabel)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsByLabel(Guid labelId)
        {
            return await _ctx.Set<Ticket>()
                .Where(t => t.TicketLabelId == labelId)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsByStateId(Guid stateId)
        {
            var tickets = await _ctx.Set<Ticket>()
                .Where(t => t.StateId == stateId)
                .OrderBy(s => s.Order)
                .ToListAsync();
            return tickets;
        }

        public async Task<bool> UpdateManyTickets(IEnumerable<Ticket> tickets)
        {
            try
            {
                foreach (var ticket in tickets)
                {
                    _ctx.Set<Ticket>().Update(ticket);
                }
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
