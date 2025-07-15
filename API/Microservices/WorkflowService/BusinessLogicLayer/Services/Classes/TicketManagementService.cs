using AutoMapper;
using BusinessLogicLayer.DTO.Ticket;
using BusinessLogicLayer.DTO.TimeLog;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using GorilloBoards.Contracts.IntegrationEvents;
using GorilloBoards.Contracts.Interfaces;
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
        private readonly ITimeLogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        public TicketManagementService(ITicketRepository ticketRepository, IMapper mapper, IStateRepository stateRepository, ITimeLogRepository timeLogRepository, IEventPublisher eventPublisher)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _stateRepository = stateRepository;
            _logRepository = timeLogRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<TicketUserAssignedDTO> AssignUserToTicket(TicketAssigneUserDTO ticketAssigneUserDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketAssigneUserDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists");
            }

            if (ticket.IsClosed)
            {
                throw new Exception($"This ticket is closed");
            }

            var timeLog = await _logRepository.GetInProgressLogByTicket(ticketAssigneUserDTO.TicketId);
            if (timeLog != null)
            {
                throw new Exception($"This ticket in progress by another user");
            }

            ticket.UserAssigned = ticketAssigneUserDTO.UserId;
            var ticketId = await _ticketRepository.UpdateAsync(ticket);

            var userAssigned = new TicketUserAssignedDTO { Id = ticketId, UserId = ticketAssigneUserDTO.UserId };
            return userAssigned;
        }

        public async Task<Guid> ChangeDescriptionTicket(TicketChangeDescription ticketChangeDescription)
        {
            var ticket = await _ticketRepository.GetAsync(ticketChangeDescription.Id);
            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }

            ticket.Description = ticketChangeDescription.Description;
            var ticketId = await _ticketRepository.UpdateAsync(ticket);
            return ticketId;
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

        public async Task<Guid> ChangeTicketState(TicketChangeStateDTO ticketChangeStateDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketChangeStateDTO.Id);

            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }

            if (ticket.StateId == ticketChangeStateDTO.StateId)
            {
                throw new Exception("Trying to ticket's change state to same state");
            }

            var state = await _stateRepository.GetAsync(ticketChangeStateDTO.StateId);

            if (state == null)
            {
                throw new Exception("Such state not exists");
            }
            //changing orders starting from right
            var allTickets = await _ticketRepository.GetTicketsByStateId(ticket.StateId);

            var orderToDelete = ticket.Order;

            foreach (var ticketInList in allTickets)
            {
                if (ticketInList.Order > orderToDelete)
                {
                    ticketInList.Order--;
                }
            }

            ticket.StateId = ticketChangeStateDTO.StateId;
            //new order for another state
            ticket.Order = (await _ticketRepository.GetMaxOrderCount(ticketChangeStateDTO.StateId)) + 1;

            var ticketId = await _ticketRepository.UpdateAsync(ticket);
            return ticketId;
        }

        public async Task<Guid> CloseTicket(TicketCloseDTO ticketCloseDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketCloseDTO.TicketId);
            if (ticket == null)
            {
                throw new Exception($"Such ticket not exists");
            }

            if (ticket.UserRequestor != ticketCloseDTO.UserRequestor)
            {
                throw new Exception($"Only requestor can close this ticket");
            }

            var timeLogs = await _logRepository.GetAllInProgressLogByTicket(ticketCloseDTO.TicketId);
            if (timeLogs.Any(l => l.EndedAt == null))
            {
                throw new Exception($"Work already started on this ticket");
            }

            ticket.IsClosed = true;
            ticket.TicketClosed = DateTime.UtcNow;
            var ticketId = await _ticketRepository.UpdateAsync(ticket);

            var ticketClosedEvent = new TicketClosedEvent
            {
                Id = ticketId,
                TicketClosed = (DateTime)ticket.TicketClosed
            };

            await _eventPublisher.Publish(ticketClosedEvent);

            return ticketId;
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

            var ticketCreatedEvent = new TicketCreatedEvent
            {
                Id = ticketId,
                CreatedAt = DateTime.UtcNow,
            };

            await _eventPublisher.Publish(ticketCreatedEvent);

            return ticketId;
        }

        public async Task<bool> DeleteTicket(DeleteTicketDTO deleteTicketDTO)
        {
            var ticket = await _ticketRepository.GetAsync(deleteTicketDTO.Id);
            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }
            //changing orders starting from right
            var allTickets = await _ticketRepository.GetTicketsByStateId(ticket.StateId);

            var orderToDelete = ticket.Order;

            foreach (var ticketInList in allTickets)
            {
                if (ticketInList.Order > orderToDelete)
                {
                    ticketInList.Order--;
                }
            }
            var isDeleted = await _ticketRepository.DeleteAsync(deleteTicketDTO.Id);

            var ticketDeletedEvent = new TicketDeletedEvent
            {
                Id = ticket.Id
            };
            await _eventPublisher.Publish(ticketDeletedEvent);

            await _ticketRepository.UpdateManyTickets(allTickets);
            return isDeleted;
        }

        public async Task<List<TicketListDTO>> GetTicketsByState(TicketGetByState ticketGetByState)
        {
            var state = await _stateRepository.GetAsync(ticketGetByState.StateId);
            if (state == null)
            {
                throw new Exception("Such state not esxists");
            }

            var tickets = await _ticketRepository.GetTicketByState(ticketGetByState.StateId);

            return _mapper.Map<List<TicketListDTO>>(tickets);
        }

        public async Task<Guid> RenameTicket(TicketRenameDTO ticketRenameDTO)
        {
            var ticket = await _ticketRepository.GetAsync(ticketRenameDTO.Id);
            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }

            ticket.Title = ticketRenameDTO.Title;

            var ticketId = await _ticketRepository.UpdateAsync(ticket);
            return ticketId;
        }
    }
}
