using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.DTO.Ticket.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IChartsService
    {
        Task<List<TicketDTO>> GetTicketsByDate(SprintDTO sprintDTO);
        Task<List<TicketDTO>> GetTicketsByDateAndBoard(SprintBoardDTO sprintBoardDTO);
    }
}
