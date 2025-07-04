using BusinessLogicLayer.DTO.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ITicketManagementService
    {
        Task<Guid> CreateTicket(TicketCreateDTO ticketCreateDTO);
        Task<bool> ChangeOrderTicket(Guid ticketId, int targetOrder);
        Task<bool> DeleteTicket(Guid ticketId);
        Task<Guid> RenameTicket(TicketRenameDTO ticketRenameDTO);
        Task<Guid> ChangeDescriptionTicket(TicketChangeDescription ticketChangeDescription);
        Task<Guid> ChangeTicketState(TicketChangeStateDTO ticketChangeStateDTO);
        Task<Guid> CloseTicket(TicketCloseDTO ticketCloseDTO);
        Task<Guid> AssignUserToTicket(TicketAssigneUserDTO ticketAssigneUserDTO);

        Task<List<TicketListDTO>> GetTicketsByState(TicketGetByState ticketGetByState);
    }
}
