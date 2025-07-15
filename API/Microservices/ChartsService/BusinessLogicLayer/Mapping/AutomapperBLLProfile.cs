using AutoMapper;
using BusinessLogicLayer.DTO.Ticket.Request;
using DataAccessLayer.Entities;
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
            CreateMap<Ticket, TicketCreateDTO>().ReverseMap();
        }
    }
}
