using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using Moq;

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
            //_mapper.Setup(x => x.Map<>)

            var userResult = await _authService.LoginUser(userLoginDto);

            Assert.NotNull(userResult);
            Assert.Equal(userJwtDto.UserName, userResult.UserName);
            Assert.Equal(userJwtDto.Role, userResult.Role);
        }
    }
}



