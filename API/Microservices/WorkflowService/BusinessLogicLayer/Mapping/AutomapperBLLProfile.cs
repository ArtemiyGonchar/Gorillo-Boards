using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entites;
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
            CreateMap<TicketBoard, BoardCreatedDTO>().ReverseMap();
        }
    }
}
