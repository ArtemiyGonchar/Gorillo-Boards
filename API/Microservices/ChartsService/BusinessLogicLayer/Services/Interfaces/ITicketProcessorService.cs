using BusinessLogicLayer.DTO.Ticket.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ITicketProcessorService
    {
        Task<Guid> CreateTicket(TicketCreateDTO ticketCreateDTO);
    }
}
