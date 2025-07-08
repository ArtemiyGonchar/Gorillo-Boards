using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ITimeLogRepository : IRepository<TicketTimeLog>
    {
        Task<TicketTimeLog?> GetByUserAndTicket(Guid userId, Guid ticketId);
        Task<TicketTimeLog?> GetByTicket(Guid ticketId);
        Task<TicketTimeLog?> GetInProgressLogByTicket(Guid ticketId);
        Task<List<TicketTimeLog>> GetAllByUserAndTicket(Guid userId, Guid ticketId);
    }
}
