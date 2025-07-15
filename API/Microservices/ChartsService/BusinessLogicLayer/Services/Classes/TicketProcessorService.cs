using AutoMapper;
using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class TicketProcessorService : ITicketProcessorService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TicketProcessorService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateTicket(TicketCreateDTO ticketCreateDTO)
        {
            var ticket = _mapper.Map<Ticket>(ticketCreateDTO);
            var ticketId = await _ticketRepository.CreateAsync(ticket);
            return ticketId;
        }

        public async Task<bool> DeleteTicket(TicketDeleteDTO ticketDeleteDTO)
        {
            var isDeleted = await _ticketRepository.DeleteAsync(ticketDeleteDTO.Id);
            return isDeleted;
        }
    }
}
