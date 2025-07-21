using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Services.Classes;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Interfaces;
using Moq;

namespace BoardsServiceTests
{
    public class BoardAccessServiceTest
    {
        private readonly Mock<IBoardRoleRepository> _boardRoleRepositoryMock;
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BoardAccessService _boardAccessService;

        public BoardAccessServiceTest()
        {
            _boardRoleRepositoryMock = new Mock<IBoardRoleRepository>();
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _mapperMock = new Mock<IMapper>();
            _boardAccessService = new BoardAccessService(_boardRepositoryMock.Object, _mapperMock.Object, _boardRoleRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllBoards_HasBoards_ReturnListBoardResponseDTO()
        {
            var boards = new List<Board>
            {
                new Board
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    AllowedRoles = new List<BoardRole>(),
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Description ="description",
                    Title = "title"
                },

                new Board
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156B"),
                    AllowedRoles = new List<BoardRole>(),
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Description ="description",
                    Title = "title2"
                }
            };


            var boardsMapped = new List<BoardResponseDTO>
            {
                new BoardResponseDTO
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    Description = "description",
                    Title= "title",
                },

                new BoardResponseDTO
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156B"),
                    Description = "description",
                    Title= "title2",
                },
            };

            _boardRepositoryMock.Setup(b => b.GetAllAsync()).ReturnsAsync(boards);
            _mapperMock.Setup(m => m.Map<List<BoardResponseDTO>>(boards)).Returns(boardsMapped);

            var result = await _boardAccessService.GetAllBoards();
            Assert.NotNull(result);
            Assert.Equal(result[0].Title, boards[0].Title);
        }

        [Fact]
        public async Task GetBoardById_ValidId_ReturnsBoardResponseDTO()
        {
            var expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var board = new Board
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                AllowedRoles = new List<BoardRole>(),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Description = "description",
                Title = "title"
            };

            var boardMapped = new BoardResponseDTO
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Description = "description",
                Title = "title",
            };

            _boardRepositoryMock.Setup(b => b.GetAsync(expectedId)).ReturnsAsync(board);
            _mapperMock.Setup(m => m.Map<BoardResponseDTO>(board)).Returns(boardMapped);

            var result = await _boardAccessService.GetBoardById(expectedId);

            Assert.NotNull(result);
            Assert.Equal(result.Id, board.Id);
        }

        [Fact]
        public async Task GetBoards_ValidRole_ReturnsListBoardsByRole()
        {
            string roleInput = "Admin";
            var role = UserRoleBL.Admin;
            var roleMapped = UserRoleGlobal.Admin;

            var boards = new List<Board>
            {
                new Board
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    AllowedRoles = new List<BoardRole>(),
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Description ="description",
                    Title = "title"
                },

                new Board
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156B"),
                    AllowedRoles = new List<BoardRole>(),
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Description ="description",
                    Title = "title2"
                }
            };

            var boardsMapped = new List<BoardResponseDTO>
            {
                new BoardResponseDTO
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    Description = "description",
                    Title= "title",
                },

                new BoardResponseDTO
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156B"),
                    Description = "description",
                    Title= "title2",
                },
            };

            _mapperMock.Setup(m => m.Map<UserRoleGlobal>(role)).Returns(roleMapped);
            _boardRepositoryMock.Setup(b => b.GetBoardsByRole(roleMapped)).Returns(Task.FromResult(boards));
            _mapperMock.Setup(m => m.Map<List<BoardResponseDTO>>(boards)).Returns(boardsMapped);

            var result = await _boardAccessService.GetBoards(roleInput);
            Assert.Equal(boardsMapped.Count, result.Count);
            Assert.Equal(boardsMapped[0].Id, result[0].Id);

        }

        [Fact]
        public async Task HasAccess_ValidBoardIdAndRole_ReturnTrue()
        {
            string roleInput = "Admin";

            var boardId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");
            var role = UserRoleBL.Admin;
            var roleMapped = UserRoleGlobal.Admin;

            _boardRoleRepositoryMock.Setup(b => b.BoardHasSuchRole(boardId, roleMapped)).Returns(Task.FromResult(true));
            _mapperMock.Setup(m => m.Map<UserRoleGlobal>(role)).Returns(roleMapped);

            var result = await _boardAccessService.HasAccess(boardId, roleInput);
            Assert.True(result);
        }
    }
}