using AutoMapper;
using BusinessLogicLayer.DTO.Ticket;
using BusinessLogicLayer.Services.Classes;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using GorilloBoards.Contracts.IntegrationEvents;
using GorilloBoards.Contracts.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowServiceTests
{
    public class TicketManagementServiceTest
    {
        private readonly Mock<IStateRepository> _stateRepository;
        private readonly Mock<ITicketRepository> _ticketRepository;
        private readonly Mock<ITimeLogRepository> _logRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventPublisher> _eventPublisher;
        private readonly TicketManagementService _ticketManagementService;
        public TicketManagementServiceTest()
        {
            _stateRepository = new Mock<IStateRepository>();
            _ticketRepository = new Mock<ITicketRepository>();
            _logRepository = new Mock<ITimeLogRepository>();
            _mapper = new Mock<IMapper>();
            _eventPublisher = new Mock<IEventPublisher>();
            _ticketManagementService = new TicketManagementService(_ticketRepository.Object,
                _mapper.Object,
                _stateRepository.Object,
                _logRepository.Object,
                _eventPublisher.Object
                );
        }

        [Fact]
        public async Task AssignUserToTicket_ValidDTO_ReturnTicketUserAssignedDTO()
        {
            var ticketId = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A");
            var userAsignee = new TicketAssigneUserDTO
            {
                BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                TicketId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                UserId = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A")
            };

            var ticket = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UserAssigned = null
            };

            var ticketAsigned = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UserAssigned = userAsignee.UserId
            };

            var ticketUserAssigned = new TicketUserAssignedDTO
            {
                Id = ticketId,
                UserId = userAsignee.UserId
            };

            _ticketRepository.Setup(t => t.GetAsync(userAsignee.TicketId)).ReturnsAsync(ticket);
            _ticketRepository.Setup(t => t.UpdateAsync(It.Is<Ticket>(t =>
                t.Id == userAsignee.TicketId && t.UserAssigned == userAsignee.UserId))).ReturnsAsync(userAsignee.TicketId);


            var result = await _ticketManagementService.AssignUserToTicket(userAsignee);

            Assert.NotNull(result);
            Assert.Equal(result.Id, ticketAsigned.Id);
            Assert.Equal(result.UserId, ticketAsigned.UserAssigned);
        }

        [Fact]
        public async Task ChangeDescriptionTicket_ValidInput_ReturnGuid()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var changeDesc = new TicketChangeDescription
            {
                BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                Description = "Test",
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A")
            };

            var ticket = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UserAssigned = null
            };

            var ticketChange = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UserAssigned = null,
                Description = "Test",

            };

            _ticketRepository.Setup(t => t.GetAsync(changeDesc.Id)).ReturnsAsync(ticket);
            _ticketRepository.Setup(t => t.UpdateAsync(It.Is<Ticket>(t =>
                t.Id == changeDesc.Id && t.Description == changeDesc.Description))).ReturnsAsync(expectedId);


            var result = await _ticketManagementService.ChangeDescriptionTicket(changeDesc);

            Assert.Equal(result, changeDesc.Id);
        }

        [Fact]
        public async Task ChangeTicketState_ValidDTO_ReturnGuid()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");

            var changeState = new TicketChangeStateDTO
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                StateId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                BoardId = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A")
            };

            var ticket = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                StateId = Guid.Parse("D7061755-661B-401E-E470-08DDB0D1156A"),
                Order = 3
            };

            var state = new State
            {
                BoardId = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A"),
                Id = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test"
            };

            var updatedTicket = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                StateId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                Order = 1
            };

            _ticketRepository.Setup(t => t.GetAsync(changeState.Id)).ReturnsAsync(ticket);
            _stateRepository.Setup(s => s.GetAsync(changeState.StateId)).ReturnsAsync(state);
            _ticketRepository.Setup(t => t.GetTicketsByStateId(ticket.StateId)).ReturnsAsync(new List<Ticket>());
            _ticketRepository.Setup(t => t.GetMaxOrderCount(changeState.StateId)).ReturnsAsync(0);

            _ticketRepository.Setup(t => t.UpdateAsync(It.Is<Ticket>(t => t.Id == changeState.Id))).ReturnsAsync(expectedId);
            var result = await _ticketManagementService.ChangeTicketState(changeState);

            Assert.Equal(result, changeState.Id);
        }
        [Fact]
        public async Task CloseTicket_ValidDTO_ReturnGuid()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var ticket = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                StateId = Guid.Parse("D7061755-661B-401E-E470-08DDB0D1156A"),
                IsClosed = false,
            };

            var closeTicket = new TicketCloseDTO
            {
                TicketId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                UserRequestor = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A")
            };

            var updatedTicket = new Ticket
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                StateId = Guid.Parse("D7061755-661B-401E-E470-08DDB0D1156A"),
                IsClosed = true,
                TicketClosed = DateTime.UtcNow
            };

            var ticketClosedEvent = new TicketClosedEvent
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                TicketClosed = DateTime.UtcNow
            };

            var timelogs = new List<TicketTimeLog>();

            _ticketRepository.Setup(t => t.GetAsync(closeTicket.TicketId)).ReturnsAsync(ticket);
            _logRepository.Setup(l => l.GetAllInProgressLogByTicket(closeTicket.TicketId)).ReturnsAsync(timelogs);
            _ticketRepository.Setup(t => t.UpdateAsync(It.Is<Ticket>(t => t.Id == closeTicket.TicketId))).ReturnsAsync(expectedId);
            _eventPublisher.Setup(e => e.Publish(ticketClosedEvent));

            var result = await _ticketManagementService.CloseTicket(closeTicket);

            Assert.Equal(result, updatedTicket.Id);
        }
    }
}
