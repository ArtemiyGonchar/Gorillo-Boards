using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
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

        public async Task<UserJwtDTO> LoginUser(UserLoginDTO userLoginDTO)
        {

            var userInDb = await _userRepository.GetByUsername(userLoginDTO.UserName) != null; //cheking if user is in db

            if (userInDb == false)
            {
                throw new ArgumentNullException(nameof(userLoginDTO)); //TODO rewrite exeption
            }
            var userMapped = _mapper.Map<User>(userLoginDTO);
            var user = await _userRepository.GetByUsername(userMapped.UserName);

            var userPass = user.PasswordHash;

            var verify = _passwordHasher.Verify(userMapped.PasswordHash, userPass);

            if (verify == false)
            {
                return null;
            }

            var userJwtDto = _mapper.Map<UserJwtDTO>(user);

            return userJwtDto;
        }
    }
}
