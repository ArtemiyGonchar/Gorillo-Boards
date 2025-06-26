using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IStateRepository : IRepository<State>
    {
        Task<Guid> CreateStateWithOder(State state);
        Task<List<State>> GetStatesByBoardId(Guid boardId);
        Task<bool> UpdateManyStates(IEnumerable<State> states);
    }
}
