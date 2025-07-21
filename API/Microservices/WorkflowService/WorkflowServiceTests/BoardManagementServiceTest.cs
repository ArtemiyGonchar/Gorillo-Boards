using AutoMapper;
using BusinessLogicLayer.DTO.Board;
using BusinessLogicLayer.Services.Classes;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using Moq;
using System.Reflection.Emit;

namespace WorkflowServiceTests
{
    public class BoardManagementServiceTest
    {
        private readonly Mock<ITicketBoardRepository> _boardRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly BoardManagementService _service;

        public BoardManagementServiceTest()
        {
            _boardRepository = new Mock<ITicketBoardRepository>();
            _mapper = new Mock<IMapper>();
            _service = new BoardManagementService(_boardRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task BoardCreate_ValidEvent_ReturnGuid()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var boardCreatedEvent = new BoardCreatedDTO
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test"
            };

            var board = new TicketBoard
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                States = new List<State>(),
                Labels = new List<TicketLabel>(),
                Title = "Test"
            };

            _mapper.Setup(m => m.Map<TicketBoard>(boardCreatedEvent)).Returns(board);
            _boardRepository.Setup(b => b.CreateAsync(board)).ReturnsAsync(expectedId);

            var result = await _service.BoardCreate(boardCreatedEvent);

            Assert.Equal(result, expectedId);
            Assert.Equal(board.Title, boardCreatedEvent.Title);
        }

        [Fact]
        public async Task BoardDelete_ValidEvent_ReturnTrue()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var titleToDelete = "Test";

            var board = new TicketBoard
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                States = new List<State>(),
                Labels = new List<TicketLabel>(),
                Title = "Test"
            };

            _boardRepository.Setup(b => b.GetBoardByTitle(titleToDelete)).ReturnsAsync(board);
            _boardRepository.Setup(b => b.DeleteAsync(board.Id)).ReturnsAsync(true);

            var result = await _service.BoardDelete(titleToDelete);

            Assert.True(result);
        }
    }
}