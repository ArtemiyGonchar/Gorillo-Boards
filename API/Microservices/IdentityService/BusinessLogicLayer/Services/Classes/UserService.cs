﻿using AutoMapper;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }


        //TODO implement checking if username exists in db before creating new one
        public async Task<Guid> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            //checking if user is in db

            userRegistrationDTO.PasswordHash = _passwordHasher.Hash(userRegistrationDTO.PasswordHash);

            var userMapped = _mapper.Map<User>(userRegistrationDTO);




            var userCreated = await _userRepository.CreateAsync(userMapped);
            return userCreated;
        }

        public async Task<bool> LoginUser(UserLoginDTO userLoginDTO)
        {
            var userMapped = _mapper.Map<User>(userLoginDTO);

            var user = await _userRepository.GetByUsername(userMapped.UserName);
            var userPass = user.PasswordHash;
            var verify = _passwordHasher.Verify(userMapped.PasswordHash, userPass);

            return verify;
        }
    }
}
