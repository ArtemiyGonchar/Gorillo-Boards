using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using Moq;
using System.ComponentModel;

namespace IdentityServiceTests
{
    public class AuthServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _userHasherMock;
        private readonly Mock<IMapper> _mapper;
        private readonly AuthService _authService;


        public AuthServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userHasherMock = new Mock<IPasswordHasher>();
            _mapper = new Mock<IMapper>();
            _authService = new AuthService(_userRepositoryMock.Object, _userHasherMock.Object, _mapper.Object);
        }

        [Fact]
        public async Task Auth_ValidLoging_ReturnUserJwtDto()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                AvatarUrl = null,
                UserName = "UnitTest",
                DisplayName = "Test",
                PasswordHash = "hashed-pass",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Role = 0
            };

            var userLoginDto = new UserLoginDTO
            {
                UserName = user.UserName,
                PasswordHash = "inputpass",
            };

            var userJwtDto = new UserJwtDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = 0
            };
            _userRepositoryMock.Setup(r => r.GetByUsername("UnitTest")).ReturnsAsync(user);
            _userHasherMock.Setup(h => h.Verify("inputpass", "hashed-pass")).Returns(true);
            _mapper.Setup(m => m.Map<UserJwtDTO>(user)).Returns(userJwtDto);

            var userResult = await _authService.LoginUser(userLoginDto);

            Assert.NotNull(userResult);
            Assert.Equal(userJwtDto.UserName, userResult.UserName);
            Assert.Equal(userJwtDto.Role, userResult.Role);
        }

        [Fact]
        public async Task GetUserById_ValidId_ReturnUserDTO()
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

            var userDTO = new GetUserDTO
            {
                Id = expectedId,
                UserName = "UnitTest",
                DisplayName = "Test",
            };

            _userRepositoryMock.Setup(r => r.GetAsync(expectedId)).ReturnsAsync(user);
            _mapper.Setup(m => m.Map<GetUserDTO>(user)).Returns(userDTO);

            var userResult = await _authService.GetUserById(expectedId);

            Assert.NotNull(userResult);
            Assert.Equal("UnitTest", userResult.UserName);
        }
    }
}



