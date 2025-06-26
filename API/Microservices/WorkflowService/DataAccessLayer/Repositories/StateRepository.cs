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
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(WorkflowDbContext ctx) : base(ctx) { }

    }
}
