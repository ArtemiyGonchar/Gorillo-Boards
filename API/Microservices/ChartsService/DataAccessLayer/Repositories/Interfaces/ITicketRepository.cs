using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<List<Ticket>> GetAllByPeriod(DateTime start, DateTime end);
        Task<List<Ticket>> GetAllByPeriodAndBoard(DateTime start, DateTime end, Guid boardId);
    }
}
