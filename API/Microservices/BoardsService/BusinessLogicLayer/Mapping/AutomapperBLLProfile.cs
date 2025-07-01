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
            CreateMap<UserRoleGlobal, UserRoleBL>().ReverseMap();
            CreateMap<UserRoleGlobal, int>().ReverseMap(); //????
            CreateMap<Board, BoardCreateDTO>().ReverseMap();
            CreateMap<Board, BoardDTO>().ReverseMap();
            // CreateMap<List<Board>, <List<BoardDTO>().ReverseMap();
            CreateMap<Board, BoardResponseDTO>().ReverseMap();

            CreateMap<BoardRole, BoardCreateAllowedRoleDTO>().ReverseMap();
            CreateMap<BoardRole, BoardDeleteAllowedRoleDTO>().ReverseMap();
        }
    }
}
