using AutoMapper;
using BusinessLogicLayer.DTO;
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
    public class TicketManagementService : ITicketManagementService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketManagementService(ITicketRepository ticketRepository, IMapper mapper, IStateRepository stateRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _stateRepository = stateRepository;
        }

        public async Task<Guid> CreateTicket(TicketCreateDTO ticketCreateDTO)
        {
            var stateExists = await _stateRepository.GetAsync(ticketCreateDTO.StateId);

            if (stateExists == null)
            {
                throw new Exception("Such state not exists");
            }

            var ticket = _mapper.Map<Ticket>(ticketCreateDTO);

            var ticketId = await _ticketRepository.CreateTicketWithOder(ticket);
            return ticketId;
        }
    }
}
