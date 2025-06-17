using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enums;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapping
{
    public class AutomapperBLLProfile : Profile
    {
        public AutomapperBLLProfile()
        {
            CreateMap<UserRoleGlobal, UserRoleDTO>().ReverseMap();
            CreateMap<User, UserRegistrationDTO>().ReverseMap();
        }

    }
}
