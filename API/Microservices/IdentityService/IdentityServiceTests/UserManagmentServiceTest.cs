using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using GorilloBoards.Contracts.IntegrationEvents;
using GorilloBoards.Contracts.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace IdentityServiceTests
{
    public class UserManagmentServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserManagmentService _userService;

        public UserManagmentServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserManagmentService(_userRepositoryMock.Object, _passwordHasherMock.Object, _mapperMock.Object, _eventPublisherMock.Object);
        }

        [Fact]
        public async Task RegisterUser_ValidUserRegistrationDTO_ReturnGuid()
        {
            Guid expectedId = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A");

            var user = new User
            {
                Id = expectedId,
                AvatarUrl = null,
                UserName = "UnitTest",
                DisplayName = "Test",
                PasswordHash = "hashed-pass",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Role = 0
            };

            var userDTO = new UserRegistrationDTO
            {
                DisplayName = "Test",
                PasswordHash = "pass",
                Role = 0,
                UserName = "UnitTest"
            };


            var userPublish = new UserCreatedEvent
            {
                Id = expectedId,
                Role = userDTO.Role.ToString(),
                Username = "UnitTest"
            };

            _userRepositoryMock.Setup(r => r.GetByUsername("UnitTest")).ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(r => r.CreateAsync(user)).ReturnsAsync(expectedId);
            _passwordHasherMock.Setup(h => h.Hash("pass")).Returns("hashed-pass");
            _eventPublisherMock.Setup(e => e.Publish(userPublish));
            _mapperMock.Setup(m => m.Map<User>(userDTO)).Returns(user);

            var result = _userService.RegisterUser(userDTO);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllUsers_UsersInDb_ReturnGetUserDTO()
        {
            var userList = new List<User>
            {
                new User
                {
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    AvatarUrl = null,
                    UserName = "UnitTest",
                    DisplayName = "Test",
                    PasswordHash = "hashed-pass",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Role = 0
                }
            };

            var userListDTO = new List<GetUserDTO>
            {
                new GetUserDTO
                {
                    DisplayName = "Test",
                    Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                    UserName = "UnitTest",
                }
            };

            _mapperMock.Setup(m => m.Map<List<GetUserDTO>>(userList)).Returns(userListDTO);
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(userList);

            var result = await _userService.GetAllUsers();
            Assert.NotNull(result);
            Assert.Equal(result[0].DisplayName, userList[0].DisplayName);
        }

        [Fact]
        public async Task DeleteUserByUsername_stringUsername_ReturnTrue()
        {
            var userPublish = new UserDeletedEvent
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                Username = "UnitTest"
            };

            var user = new User
            {
                Id = Guid.Parse("A7061755-661B-401E-E470-08DDB0D1156A"),
                AvatarUrl = null,
                UserName = "UnitTest",
                DisplayName = "Test",
                PasswordHash = "hashed-pass",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Role = 0
            };

            var username = "UnitTest";
            _userRepositoryMock.Setup(r => r.GetByUsername(username)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.DeleteByUsername(username)).ReturnsAsync(true);
            _eventPublisherMock.Setup(e => e.Publish(userPublish));

            var result = await _userService.DeleteUserByUsername(username);
            Assert.True(result);
        }
    }
}
