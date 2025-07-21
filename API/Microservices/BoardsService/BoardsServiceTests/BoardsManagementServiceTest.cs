using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Services.Classes;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Interfaces;
using GorilloBoards.Contracts.IntegrationEvents;
using GorilloBoards.Contracts.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardsServiceTests
{
    public class BoardsManagementServiceTest
    {
        private readonly Mock<IBoardRoleRepository> _boardRoleRepositoryMock;
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly BoardsManagmentService _boardsManagmentService;
        public BoardsManagementServiceTest()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _boardRoleRepositoryMock = new Mock<IBoardRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _boardsManagmentService = new BoardsManagmentService(_boardRepositoryMock.Object, _mapperMock.Object, _boardRoleRepositoryMock.Object, _eventPublisherMock.Object);
        }

        [Fact]
        public async Task CreateBoardAsync_ValidInput_ReturnsGuid()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var boardCreate = new BoardCreateDTO
            {
                Title = "Test",
                Description = "Test"
            };

            var board = new Board
            {
                Description = "Test",
                Title = "Test",
                AllowedRoles = new List<BoardRole>(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
            };

            var boardCreatedEvent = new BoardCreatedEvent
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
            };

            _mapperMock.Setup(m => m.Map<Board>(boardCreate)).Returns(board);
            _boardRepositoryMock.Setup(b => b.CreateAsync(board)).ReturnsAsync(expectedId);
            _eventPublisherMock.Setup(e => e.Publish(boardCreatedEvent));

            var result = await _boardsManagmentService.CreateBoardAsync(boardCreate);

            Assert.Equal(result, expectedId);
        }

        [Fact]
        public async Task DeleteBoardAsync_ValidInput_ReturnTrue()
        {
            var title = "Test";

            var board = new Board
            {
                Description = "Test",
                Title = "Test",
                AllowedRoles = new List<BoardRole>(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
            };

            var boardDeletedEvent = new BoardDeletedEvent
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Title = "Test",
            };

            _boardRepositoryMock.Setup(b => b.GetBoardByTitle(title)).ReturnsAsync(board);
            _boardRepositoryMock.Setup(b => b.DeleteAsync(board.Id)).ReturnsAsync(true);
            _eventPublisherMock.Setup(e => e.Publish(boardDeletedEvent));

            var result = await _boardsManagmentService.DeleteBoardAsync(title);

            Assert.True(result);
        }

        [Fact]
        public async Task CreateBoardRole_ValidInput_ReturnGuid()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156B");

            var roleCreateDTO = new BoardCreateAllowedRoleDTO
            {
                AllowedRole = UserRoleBL.Admin,
                Title = "Test"
            };

            var board = new Board
            {
                Description = "Test",
                Title = "Test",
                AllowedRoles = new List<BoardRole>(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
            };

            var boardRole = new BoardRole
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Board = board,
                BoardId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Role = UserRoleGlobal.Admin
            };

            _boardRepositoryMock.Setup(b => b.GetBoardByTitle(roleCreateDTO.Title)).ReturnsAsync(board);
            _mapperMock.Setup(m => m.Map<BoardRole>(roleCreateDTO)).Returns(boardRole);
            _boardRoleRepositoryMock.Setup(b => b.CreateAsync(boardRole)).ReturnsAsync(expectedId);

            var result = await _boardsManagmentService.CreateBoardRole(roleCreateDTO);
            Assert.Equal(result, expectedId);
        }
    }
}
