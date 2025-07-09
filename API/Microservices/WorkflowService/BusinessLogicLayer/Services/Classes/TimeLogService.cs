using AutoMapper;
using BusinessLogicLayer.DTO.TimeLog;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class TimeLogService : ITimeLogService
    {
        private readonly IMapper _mapper;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly ITicketRepository _ticketRepository;

        public TimeLogService(IMapper mapper, ITimeLogRepository timeLogRepository, ITicketRepository ticketRepository)
        {
            _mapper = mapper;
            _timeLogRepository = timeLogRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> TicketInProgress(TicketInProgressDTO ticketInProgressDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketInProgressDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists");
            }

            var timeLog = await _timeLogRepository.GetInProgressLogByTicket(ticketInProgressDTO.TicketId);

            if (timeLog == null)
            {
                return false;
            }

            return true;
        }

        public async Task<Guid> TicketWorkEnd(TicketEndWorkDTO ticketEndWorkDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketEndWorkDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists");
            }

            if (ticket.IsClosed)
            {
                throw new Exception($"This ticket is closed");
            }

            if (ticket.UserAssigned != ticketEndWorkDTO.UserId)
            {
                throw new Exception($"Only assigned user can end work on tickets");
            }

            var timeLogs = await _timeLogRepository.GetAllByUserAndTicket(ticketEndWorkDTO.UserId, ticketEndWorkDTO.TicketId);

            var activeLog = timeLogs.FirstOrDefault(l => l.EndedAt == null);

            if (activeLog == null)
            {
                throw new Exception($"No active work found on this ticket");
            }

            activeLog.EndedAt = DateTime.UtcNow;
            var timeLogId = await _timeLogRepository.UpdateAsync(activeLog);

            return timeLogId;
        }

        public async Task<Guid> TicketWorkStart(TicketStartWorkDTO ticketStartWorkDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketStartWorkDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists");
            }

            if (ticket.IsClosed)
            {
                throw new Exception($"This ticket is closed");
            }

            if (ticket.UserAssigned == null)
            {
                throw new Exception($"Assign yourself to ticket first");
            }

            if (ticket.UserAssigned != ticketStartWorkDTO.UserId)
            {
                throw new Exception($"Only assigned user can start work on tickets");
            }

            var timeLogs = await _timeLogRepository.GetAllByUserAndTicket(ticketStartWorkDTO.UserId, ticketStartWorkDTO.TicketId);

            if (timeLogs.Any(l => l.EndedAt == null))
            {
                throw new Exception($"Work already started on this ticket");
            }


            var timeLogMapped = _mapper.Map<TicketTimeLog>(ticketStartWorkDTO);
            timeLogMapped.StartedAt = DateTime.UtcNow;
            var timeLogId = await _timeLogRepository.CreateAsync(timeLogMapped);

            return timeLogId;
        }
    }
}
