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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        //TODO implement checking if username exists in db before creating new one
        public async Task<Guid> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var userMapped = _mapper.Map<User>(userRegistrationDTO);
            var userCreated = await _userRepository.CreateAsync(userMapped);
            return userCreated;
        }
    }
}
