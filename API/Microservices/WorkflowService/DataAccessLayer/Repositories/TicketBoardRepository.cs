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
    public class TicketBoardRepository : Repository<TicketBoard>, ITicketBoardRepository
    {
        private readonly WorkflowDbContext _ctx;
        public TicketBoardRepository(WorkflowDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<TicketBoard?> GetBoardByTitle(string title)
        {
            return await _ctx.Set<TicketBoard>()
                .SingleOrDefaultAsync(b => b.Title == title);
        }
    }
}
