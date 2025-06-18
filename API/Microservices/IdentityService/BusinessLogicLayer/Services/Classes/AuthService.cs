using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public Task<bool> LoginUser(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }
    }
}
