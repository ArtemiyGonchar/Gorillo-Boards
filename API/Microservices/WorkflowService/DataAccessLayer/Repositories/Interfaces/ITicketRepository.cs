using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<bool> UpdateManyTickets(IEnumerable<Ticket> ticket);
        Task<List<Ticket>> GetTicketsByStateId(Guid stateId);
        Task<Guid> CreateTicketWithOder(Ticket tickets);
        Task<int> GetMaxOrderCount(Guid stateId);
        Task<List<Ticket>> GetTicketsByLabel(Guid labelId);
    }
}
