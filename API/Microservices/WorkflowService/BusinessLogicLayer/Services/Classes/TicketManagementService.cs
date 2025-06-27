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

        public async Task<bool> ChangeOrderTicket(Guid ticketId, int targetOrder)
        {
            var ticket = await _ticketRepository.GetAsync(ticketId);

            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }

            var ticketsByBoard = await _ticketRepository.GetTicketsByStateId(ticket.StateId);

            if (ticket.Order == targetOrder)
            {
                return false;
            }

            if (ticket.Order < targetOrder)
            {
                foreach (var ticketInList in ticketsByBoard)
                {
                    if (ticketInList.Order > ticket.Order && ticketInList.Order <= targetOrder)
                    {
                        ticketInList.Order--;
                    }
                }
            }
            else
            {
                foreach (var ticketInList in ticketsByBoard)
                {
                    if (ticketInList.Order >= targetOrder && ticketInList.Order < ticket.Order)
                    {
                        ticketInList.Order++;
                    }
                }
            }

            ticket.Order = targetOrder;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.UpdateManyTickets(ticketsByBoard);
            return true;
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

        public async Task<bool> DeleteTicket(Guid ticketId)
        {
            var ticket = await _ticketRepository.GetAsync(ticketId);
            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }
            var allTickets = await _ticketRepository.GetTicketsByStateId(ticket.StateId);

            var orderToDelete = ticket.Order;

            foreach (var ticketInList in allTickets)
            {
                if (ticketInList.Order > orderToDelete)
                {
                    ticketInList.Order--;
                }
            }
            var isDeleted = await _ticketRepository.DeleteAsync(ticketId);
            await _ticketRepository.UpdateManyTickets(allTickets);
            return isDeleted;
        }
    }
}
