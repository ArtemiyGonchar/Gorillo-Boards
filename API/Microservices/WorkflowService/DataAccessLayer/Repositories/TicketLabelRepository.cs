using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class TicketLabelRepository : Repository<TicketLabel>, ITicketLabelRepository
    {
        private readonly WorkflowDbContext _ctx;

        public TicketLabelRepository(WorkflowDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
