using AutoMapper;
using BusinessLogicLayer.ApiClients.Clients;
using BusinessLogicLayer.DTO.State;
using BusinessLogicLayer.Services.Classes;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using GorilloBoards.Contracts.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowServiceTests
{
    public class StateManagementServiceTest
    {
        private readonly Mock<IStateRepository> _stateRepository;
        private readonly Mock<ITicketRepository> _ticketRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBoardsServiceClient> _refitClientMock;
        private readonly StateManagementService _service;

        public StateManagementServiceTest()
        {
            _stateRepository = new Mock<IStateRepository>();
            _ticketRepository = new Mock<ITicketRepository>();
            _mapper = new Mock<IMapper>();
            _refitClientMock = new Mock<IBoardsServiceClient>();
            _service = new StateManagementService(_stateRepository.Object, _mapper.Object, _refitClientMock.Object, _ticketRepository.Object);
        }

        [Fact]
        public async Task ChangeOrderState_ValidDTO_Return_True()
        {
            var changeOrder = new StateChangeOrder
            {
                BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                StateId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                OrderTarget = 2
            };

            var statesByBoard = new List<State>
            {
                new State
                {
                    BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    Order = 1,
                    Title = "Test",
                },
                new State
                {
                    BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                    Id = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A"),
                    Order = 2,
                    Title = "Test2",
                }
            };

            var stateUpdated = new State
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Order = 2,
                Title = "Test",
                BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A")
            };

            var state = new State
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Order = 1,
                Title = "Test",
                BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A")
            };

            var updatedStates = new List<State>
            {
                new State
                {
                    BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    Order = 2,
                    Title = "Test",
                },
                new State
                {
                    BoardId = Guid.Parse("B7061755-661B-401E-E470-08DDB0D1156A"),
                    Id = Guid.Parse("C7061755-661B-401E-E470-08DDB0D1156A"),
                    Order = 1,
                    Title = "Test2",
                }
            };

            _stateRepository.Setup(s => s.GetAsync(state.Id)).ReturnsAsync(state);
            _refitClientMock.Setup(c => c.HasAccess(state.BoardId)).ReturnsAsync(true);
            _stateRepository.Setup(s => s.GetStatesByBoardId(state.BoardId)).ReturnsAsync(statesByBoard);
            _stateRepository.Setup(s => s.UpdateAsync(stateUpdated));
            _stateRepository.Setup(s => s.UpdateManyStates(updatedStates));

            var result = await _service.ChangeOrderState(changeOrder);

            Assert.True(result);

        }
    }
}
