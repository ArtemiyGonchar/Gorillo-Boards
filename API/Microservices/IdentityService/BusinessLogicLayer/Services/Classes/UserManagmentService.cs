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
    public class UserManagmentService : IUserManagmentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserManagmentService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Guid> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var userInDb = await _userRepository.GetByUsername(userRegistrationDTO.UserName) == null; //cheking if user is in db

            if (!userInDb)
            {
                throw new Exception("Such user exists");
            }

            userRegistrationDTO.PasswordHash = _passwordHasher.Hash(userRegistrationDTO.PasswordHash);

            var userMapped = _mapper.Map<User>(userRegistrationDTO);
            var userId = await _userRepository.CreateAsync(userMapped);

            return userId;
        }

    }
}
