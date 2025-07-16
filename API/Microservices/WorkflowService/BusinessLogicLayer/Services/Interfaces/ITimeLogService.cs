using BusinessLogicLayer.DTO.TimeLog.Request;
using BusinessLogicLayer.DTO.TimeLog.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ITimeLogService
    {
        Task<Guid> TicketWorkStart(TicketStartWorkDTO ticketStartWorkDTO);
        Task<Guid> TicketWorkEnd(TicketEndWorkDTO ticketEndWorkDTO);
        Task<bool> TicketInProgress(TicketInProgressDTO ticketInProgressDTO);
        Task<List<TimeLogListDTO>> GetTimeLogsByTicket(GetTimeLogsByTicketDTO getTimeLogsByTicketDTO);
        Task AddTimeLogToTicket(TimeLogToTicketDTO timeLogToTicketDTO);
    }
}
