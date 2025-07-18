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

        //get user by id
        public async Task<GetUserDTO> GetUserById(Guid Id)
        {
            var user = await _userRepository.GetAsync(Id);
            if (user == null) //cheking if user is in db
            {
                throw new Exception("No such user");
            }

            var userMapped = _mapper.Map<GetUserDTO>(user);
            return userMapped;
        }

        //verify login
        public async Task<UserJwtDTO> LoginUser(UserLoginDTO userLoginDTO)
        {

            var user = await _userRepository.GetByUsername(userLoginDTO.UserName);

            if (user == null) //cheking if user is in db
            {
                throw new Exception("No such user");
            }

            var verify = _passwordHasher.Verify(userLoginDTO.PasswordHash, user.PasswordHash);

            if (verify == false)
            {
                return null;
            }

            var userJwtDto = _mapper.Map<UserJwtDTO>(user);

            return userJwtDto;
        }
    }
}
