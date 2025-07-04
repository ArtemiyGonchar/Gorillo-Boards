﻿using AutoMapper;
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

        public async Task<Guid> TicketWorkEnd(TicketEndWorkDTO ticketEndWorkDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketEndWorkDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists: {ticketEndWorkDTO.TicketId}");
            }

            if (ticket.IsClosed)
            {
                throw new Exception($"This ticket is closed: {ticket.Id}");
            }

            if (ticket.UserAssigned != ticketEndWorkDTO.UserId)
            {
                throw new Exception($"Only assigned user can end work on tickets: {ticket.Id}");
            }

            var timeLog = await _timeLogRepository.GetByUserAndTicket(ticketEndWorkDTO.UserId, ticketEndWorkDTO.TicketId);

            if (timeLog == null)
            {
                throw new Exception($"No work found on this ticket: {ticketEndWorkDTO.TicketId}");
            }

            timeLog.EndedAt = DateTime.UtcNow;
            var timeLogId = await _timeLogRepository.UpdateAsync(timeLog);

            return timeLogId;
        }

        public async Task<Guid> TicketWorkStart(TicketStartWorkDTO ticketStartWorkDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketStartWorkDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists: {ticketStartWorkDTO.TicketId}");
            }

            if (ticket.IsClosed)
            {
                throw new Exception($"This ticket is closed: {ticket.Id}");
            }

            if (ticket.UserAssigned == null)
            {
                throw new Exception($"Assign yourself to ticket first: {ticket.Id}");
            }

            if (ticket.UserAssigned != ticketStartWorkDTO.UserId)
            {
                throw new Exception($"Only assigned user can start work on tickets: {ticket.Id}");
            }

            var timeLog = await _timeLogRepository.GetByUserAndTicket(ticketStartWorkDTO.UserId, ticketStartWorkDTO.TicketId);

            if (timeLog != null && timeLog.EndedAt == null)
            {
                throw new Exception($"Work already stared on this ticket: {ticketStartWorkDTO.TicketId}");
            }

            var timeLogMapped = _mapper.Map<TicketTimeLog>(ticketStartWorkDTO);
            timeLogMapped.StartedAt = DateTime.UtcNow;
            var timeLogId = await _timeLogRepository.CreateAsync(timeLogMapped);

            return timeLogId;
        }
    }
}
