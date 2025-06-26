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
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly WorkflowDbContext _ctx;
        public StateRepository(WorkflowDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<Guid> CreateStateWithOder(State state)
        {
            var order = await _ctx.Set<State>()
                .Where(s => s.Board.Id == state.BoardId)
                .Select(s => (int?)s.Order)
                .MaxAsync() ?? 0;

            state.Order = order + 1;
            await _ctx.Set<State>().AddAsync(state);
            await _ctx.SaveChangesAsync();
            return state.Id;
        }

        public async Task<List<State>> GetStatesByBoardId(Guid boardId)
        {
            var states = await _ctx.Set<State>()
                .Where(s => s.BoardId == boardId)
                .OrderBy(s => s.Order)
                .ToListAsync();
            return states;
        }

        public async Task<bool> UpdateManyStates(IEnumerable<State> states)
        {
            try
            {
                foreach (var state in states)
                {
                    _ctx.Set<State>().Update(state);
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
